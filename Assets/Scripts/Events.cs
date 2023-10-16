using System.Collections.Generic;

namespace Events
{
    public struct OnChangeCardFrontEvent
    {
        public CardFrontsEnum CardFrontEnum;

        public OnChangeCardFrontEvent(CardFrontsEnum value)
        {
            CardFrontEnum = value;
        }
    }
    
    public struct OnChangeCardBackEvent
    {
        public CardBackEnum CardBackEnum;

        public OnChangeCardBackEvent(CardBackEnum value)
        {
            CardBackEnum = value;
        }
    }

    public struct OnChangeBackgroundEvent
    {
        public BackgroundsEnum BackgroundsEnum;

        public OnChangeBackgroundEvent(BackgroundsEnum backgroundsEnum)
        {
            BackgroundsEnum = backgroundsEnum;
        }
    }

    public struct OnNewGameEvent
    {
        
    }

    struct OnCardEndDragEvent
    {
        public PlayerCardData PlayerCardData;
        public OnCardEndDragEvent(PlayerCardData playerCardData)
        {
            this.PlayerCardData = playerCardData;
        }
    }
    
    struct OnCardTapEvent
    {
        public PlayerCardData PlayerCardData;
        public OnCardTapEvent(PlayerCardData playerCardData)
        {
            this.PlayerCardData = playerCardData;
        }
    }

    struct OnCardAnimationIsStartedEvent
    {
    }
    
    struct OnCardAnimationIsFinishedEvent
    {
    }
    
    struct OnWaitingForInputIsStartedEvent
    {
    }
    
    struct  OnWaitingForInputIsFinishedEvent
    {
    }
    
    struct OnWaitingForUIChangingStartedEvent
    {
    }
    
    struct  OnWaitingForUIChangingFinishedEvent
    {
    }

    struct OnSetPlayerTurnEvent
    {
        public IPlayer Player;

        public OnSetPlayerTurnEvent(IPlayer player)
        {
            Player = player;
        }
    }
    
    struct OnPlayedCardEvent
    {
        public PlayerCardData PlayerCardData;

        public OnPlayedCardEvent(PlayerCardData playerCardData)
        {
            PlayerCardData = playerCardData;
        }
    }

    struct OnStartGameEvent
    {
        public List<IPlayer> Players;
        public OnStartGameEvent(ref List<IPlayer> players)
        {
            Players = players;
        }
    }

    struct OnRoundStartEvent
    {
        public int RoundNumber;
        public OnRoundStartEvent(int roundNumber)
        {
            RoundNumber = roundNumber;
        }
    }
    
    struct OnCardsInitializedEvent
    {
        public List<IPlayer> Players;

        public OnCardsInitializedEvent(List<IPlayer> players)
        {
            Players = players;
        }
    }

    struct OnDealCardsEvent
    {
        public List<int> PlayersIndexBasedDealer;

        public OnDealCardsEvent(List<int> playersIndexBasedDealer)
        {
            PlayersIndexBasedDealer = playersIndexBasedDealer;
        }
    }

    public struct OnCardDraggedToTableEvent
    {
        public bool IsEnter;
        public OnCardDraggedToTableEvent(bool isEnter)
        {
            IsEnter = isEnter;
        }
    }
    
    public struct OnRoundWinnerEvent
    {
        public IPlayer Player;
        public OnRoundWinnerEvent(IPlayer player)
        {
            Player = player;
        }
    }

    public struct OnWinnerEvent
    {
        public int PlayerIndex;

        public OnWinnerEvent(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }

    public struct OnBackgroundLoadedEvent
    {
    }
}