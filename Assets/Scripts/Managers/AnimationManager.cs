using System;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class AnimationManager : Singleton<AnimationManager>
    {
        [SerializeField] private float m_cardMoveAnimationDuration = 1;
        [SerializeField] private float m_changeColorAnimationDuration = .5f;

        public Tweener MoveUIAnimationWithCallback(RectTransform rectTransform, Vector3 target, float delay = 0,
            float duration = 0, Action callBack = null)
        {
            return rectTransform.DOAnchorPos(target, duration == 0 ? m_cardMoveAnimationDuration : duration)
                .OnComplete(() => { callBack?.Invoke(); }).SetDelay(delay);
        }
        
        public Tweener CardsMoveAnimationWithCallback(CardView cardView, Vector3 target, float delay = 0,
            float duration = 0, Action callBack = null)
        {
            return cardView.transform.DOMove(target, duration == 0 ? m_cardMoveAnimationDuration : duration)
                .OnComplete(() => { callBack?.Invoke(); }).SetDelay(delay);
        }

        public Tweener CardsMoveAnimationWithCallback(CardView cardView, Vector3 target, float delay = 0,
            Action callBack = null,
            [CanBeNull] Func<bool> condition = null, Action callBackOfCondition = null)
        {
            return cardView.transform.DOMove(target, m_cardMoveAnimationDuration).OnComplete(() =>
            {
                callBack?.Invoke();
                if (condition != null && condition.Invoke())
                {
                    Debug.Log("<color=#FF0000> Condition is working</color>");
                    callBackOfCondition?.Invoke();
                }
            }).SetDelay(delay);
        }

        public Tweener CardsRotateAnimationWithCallback(CardView cardView, Vector3 targetRotation,
            Action callBack = null)
        {
            return cardView.transform.DORotate(targetRotation, .25f).OnComplete(() => { callBack?.Invoke(); });
        }

        public Tweener CardsScaleAnimationWithCallback(CardView cardView, Vector3 targetRotation,
            Action callBack = null)
        {
            string tweenName = CreateTweenName(cardView.name, "ScaleAnimation");

            return cardView.transform.DOScale(targetRotation, .25f)
                .OnComplete(() => { callBack?.Invoke(); }).SetTarget(tweenName);
        }

        public Tweener UIFadeAnimation(Image image, float alpha,Action callback = null)
        {
            EventManager.Broadcast(new OnWaitingForUIChangingStartedEvent());
            return image.DOFade(alpha, .5f).OnComplete(() =>
            {
                callback?.Invoke();
                EventManager.Broadcast(new OnWaitingForUIChangingFinishedEvent());
            });
        }
        
        public Tweener UIFadeAnimation(CanvasGroup uiCanvasGroup, float alpha,Action callback = null)
        {
            EventManager.Broadcast(new OnWaitingForUIChangingStartedEvent());
            uiCanvasGroup.gameObject.SetActive(true);
            return uiCanvasGroup.DOFade(alpha, .5f).OnComplete(() =>
            {
                callback?.Invoke();
                EventManager.Broadcast(new OnWaitingForUIChangingFinishedEvent());
            });
        }

        public Tweener UIFadeInOutAnimation(CanvasGroup uiCanvasGroup)
        {
            EventManager.Broadcast(new OnWaitingForUIChangingStartedEvent());
            uiCanvasGroup.alpha = 0;
            uiCanvasGroup.gameObject.SetActive(true);
            return uiCanvasGroup.DOFade(1, .5f).OnComplete(() =>
            {
                uiCanvasGroup.DOFade(0, .5f).OnComplete(() =>
                {
                    uiCanvasGroup.gameObject.SetActive(false);
                    EventManager.Broadcast(new OnWaitingForUIChangingFinishedEvent());
                }).SetDelay(.5f);
            });
        }

        public Tweener ChangeColor(Image image, Color color, float delay = 0, int loops = 0, Action callBack = null)
        {
            string tweenName = CreateTweenName(image.name, "ChangeColor");

            return image.DOColor(color, m_changeColorAnimationDuration).OnComplete(() => { callBack?.Invoke(); })
                .SetLoops(loops, LoopType.Yoyo).SetDelay(delay).SetTarget(tweenName);
        }

        public void KillTween(Tweener tweener)
        {
            if (tweener != null)
                DOTween.Kill(tweener);
        }

        public void DelayedCall(Action action, float duration)
        {
            DOVirtual.DelayedCall(duration, () => action?.Invoke());
        }

        public string CreateTweenName(string elementName, string animationName)
        {
            return $"{elementName}{animationName}Tween";
        }
    }
}