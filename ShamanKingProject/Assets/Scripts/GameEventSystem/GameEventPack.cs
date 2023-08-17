using UniRx;
namespace Gamemanager
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    interface IEventPublisher
    {
        void Send(GameEventMessageBase msg);
    }
    
    /// <summary>
    /// ゲーム事件のまとめ
    /// </summary>
    public abstract class GameEventPack : IEventPublisher
    {
        Dictionary<Type,IEventPublisher> eventPublishers_ = new();
       
        
        /// <summary>
        /// 通知を送るメソッド
        /// </summary>
        public void Send(GameEventMessageBase msg)
        {
            foreach (var publisher in eventPublishers_.Values)
            {
                publisher.Send(msg);
            }
        }

        protected IObservable<T> getSubject<T>()
        {
            if (!eventPublishers_.TryGetValue(typeof(T), out var publisher))
            {
                var subject = new GameMessageSubject<T>();
                eventPublishers_.Add(typeof(T),subject);
                publisher = subject;
            }

            if (publisher is IGameMessageObservable<T> observable)
            {
                return observable.Observable;
            }

            throw new UnityException($"Get Subject Error, Msg Type: {typeof(T)}");
        }

        public IDisposable SetSubscribe<T>(IObservable<T> target, Action<T> action)
        {
            var disposable = target.Subscribe(action);
            GameManager.Instance.MainGameMediator.AddToDisposables(disposable);
            return disposable;
        }
    }

    interface IGameMessageObservable<T>
    {
        public IObservable<T> Observable { get; }
    }

    class GameMessageSubject<T> : IEventPublisher, IGameMessageObservable<T>
    {
        Subject<T> subject_ = new();
        public IObservable<T> Observable => subject_;
        public void Send(GameEventMessageBase msg)
        {
            if (msg is T focusMessage)
            {
                subject_.OnNext(focusMessage);
            }
        }
    }
}