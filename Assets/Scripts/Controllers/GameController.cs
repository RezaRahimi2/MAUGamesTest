using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Events;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    #region Fields

    //This is used to cancel asynchronous operations if needed, such as waiting for certain conditions to be met.
    public CancellationTokenSource CancellationToken;
    
    [SerializeField]private bool m_waitForAnimationFinished = false;
    [SerializeField]private bool m_waitForInput = false;
    [SerializeField]private bool m_waitForUIChanging = false;
    
    // This list holds all the players in the game, which could be AI or human.
    [SerializeField]private List<IPlayer> m_players;
    //This stack represents the deck of cards. A stack is used because it mimics the real-world action of drawing a card from the top of the deck.
    [SerializeField]private Stack<CardModel> m_deck;
    
    [SerializeField]private int m_roundsPlayed;
    [SerializeField]private int m_dealerIndex;
    [SerializeField]private List<int> playersIndexBasedDealer;
    
    #endregion

    #region UnityMethods
    private void Start()
    {
        CancellationToken = new CancellationTokenSource();
        EventManager.Broadcast(new OnNewGameEvent());
        Initialize();
    }

    #endregion

    #region GameLogic

    //This method sets up the game, including subscribing to various events, initializing the players and the deck, and starting the game.
   async UniTask Initialize()
    {
        EventManager.Subscribe<OnCardAnimationIsStartedEvent>((arg =>
        {
            m_waitForAnimationFinished = true;
        }));
        
        EventManager.Subscribe<OnCardAnimationIsFinishedEvent>((arg =>
        {
            m_waitForAnimationFinished = false;
        }));
        
        EventManager.Subscribe<OnWaitingForInputIsStartedEvent>((arg =>
        {
            m_waitForInput = true;
        }));
        
        EventManager.Subscribe<OnWaitingForInputIsFinishedEvent>((arg =>
        {
            m_waitForInput = false;
        }));
        
        EventManager.Subscribe<OnWaitingForUIChangingStartedEvent>((arg =>
        {
            m_waitForUIChanging = true;
        }));
        
        EventManager.Subscribe<OnWaitingForUIChangingFinishedEvent>(arg =>
        {
            m_waitForUIChanging = false;
        });
        
        // Initialize m_aiPlayers and m_deck
        m_players = new List<IPlayer>();
        m_deck = new Stack<CardModel>();
        
        // Add m_aiPlayers to the list
        for (int i = 0; i < 3; i++)
        {
            AIPlayer aiPlayer = new AIPlayer();
            aiPlayer.IsBot = true;
            m_players.Add(aiPlayer);
        }
        
        m_players.Add(new HumanPlayer());

        List<CardModel> cards = new List<CardModel>();
        // Initialize m_deck
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                cards.Add(new CardModel(suit, value));
            }
        }

        // Shuffle the m_deck
        cards = cards.OrderBy(a => Guid.NewGuid()).ToList();

        cards.ForEach(x =>
        {
            m_deck.Push(x);
        });
        
        // Start the game
        await StartGame();
    }
    
   //This coroutine manages the game flow, including dealing cards, starting rounds, and determining winners.
    async UniTask StartGame()
    {
        // Reset rounds and points
        m_roundsPlayed = 0;
        // Randomly assign the dealer
        m_dealerIndex = UnityEngine.Random.Range(0, m_players.Count);

        playersIndexBasedDealer = new List<int>();
        for (int i = 0; i < m_players.Count; i++)
        {
            playersIndexBasedDealer.Add((m_dealerIndex + i) % m_players.Count);
        }

        for (var i = 0; i < m_players.Count; i++)
        {
            m_players[i].PlayerIndex = i;
            m_players[i].Hand = new List<CardModel>();
            m_players[i].Points = 0;            
        }
        
        EventManager.Broadcast(new OnStartGameEvent(ref m_players));
       
        await UniTask.WaitWhile(IsWaiting,cancellationToken: CancellationToken.Token);
        
        DealCards();
        
        // Wait until the observer triggers the event
        await UniTask.WaitWhile(IsWaiting,cancellationToken: CancellationToken.Token);
        
        EventManager.Broadcast(new OnDealCardsEvent(){PlayersIndexBasedDealer = playersIndexBasedDealer});
        
        // Start the first round
        await StartRound();
    }

    //This method deals 13 cards to each player from the deck.
    void DealCards()
    {
        // Deal 13 cards to each player
        for (int i = 0; i < 13 ; i++)
        {
            for (int j = 0; j < m_players.Count; j++)
            {
                // Calculate the index of the player to deal to
                int playerIndex = (m_dealerIndex + j) % m_players.Count;

                CardModel cardModel = m_deck.Pop();
                m_players[playerIndex].Hand.Add(cardModel);
#if Debugging
                Debug.Log($"Player {player.PlayerIndex} receive {cardModel.Suit}_{cardModel.Value}");
#endif
            }
        }

        EventManager.Broadcast(new OnCardsInitializedEvent(){Players = m_players});
    }

    async UniTask StartRound()
    {
        await UniTask.WaitWhile(IsWaiting,cancellationToken: CancellationToken.Token);
        EventManager.Broadcast(new OnRoundStartEvent(){RoundNumber = m_roundsPlayed + 1});
        await UniTask.WaitWhile(IsWaiting,cancellationToken: CancellationToken.Token);
        IPlayer player;
        // Each player plays a card
        for (int i = 0; i < m_players.Count; i++)
        {
            player = m_players[playersIndexBasedDealer[i]];
            player.IsReadyToPlay = true;
            EventManager.Broadcast(new OnSetPlayerTurnEvent(){Player =player});
            player.PlayCard();
            // Wait until the observer triggers the event
            await UniTask.WaitWhile(IsWaiting);
            Debug.Log($"Player{i} plays {player.PlayedCardModel.Suit}-{player.PlayedCardModel.Value}");
            player.IsReadyToPlay = false;
        }

        // Determine the winner of the round and update points
        DetermineRoundWinner();

        // Check if game is over
        if (++m_roundsPlayed == 13)
        {
            DetermineGameWinner();
        }
        else
        {
            // Start the next round
            await  StartRound();
        }
    }

    // This method determines the winner of the round based on the cards played.
    void DetermineRoundWinner()
    {
        // Assume the first player is the winner
        IPlayer roundWinner = m_players[0];
        CardModel winningCard = roundWinner.PlayedCardModel;

        // Compare with the rest of the m_players
        for (int i = 1; i < m_players.Count; i++)
        {
            CardModel currentCard = m_players[i].PlayedCardModel;
            Debug.Log($"<color=#FF0000>Compare {winningCard.Suit}-{winningCard.Value} with {currentCard.Suit}-{currentCard.Value}</color>");
            if ((int)currentCard.Suit > (int)winningCard.Suit ||
                ((int)currentCard.Suit == (int)winningCard.Suit && (int)currentCard.Value > (int)winningCard.Value))
            {
                roundWinner = m_players[i];
                winningCard = currentCard;
                Debug.Log($"<color=#FFFF00>RoundWinner is {i} with {winningCard.Suit}-{winningCard.Value}</color>");
            }
        }

        // Update points for the winner
        roundWinner.Points++;
    
        EventManager.Broadcast(new OnRoundWinnerEvent(){ Player = roundWinner});
    }

    //This method determines the overall game winner based on the points accumulated.
    void DetermineGameWinner()
    {
        // Assume the first player is the winner
        IPlayer gameWinner = m_players[0];

        // Compare with the rest of the m_aiPlayers
        for (int i = 1; i < m_players.Count; i++)
        {
            if (m_players[i].Points > gameWinner.Points)
            {
                gameWinner = m_players[i];
            }
        }
        EventManager.Broadcast(new OnWinnerEvent(){PlayerIndex = gameWinner.PlayerIndex});
        // Display the winner
        Debug.Log("The winner is: " + gameWinner.PlayerIndex);

    }
    
    //This method checks if the game is waiting for an animation to finish, for user input, or for UI changes.
    private bool IsWaiting()
    {
        return m_waitForAnimationFinished || m_waitForInput || m_waitForUIChanging;
    }
     
    #endregion
 
}