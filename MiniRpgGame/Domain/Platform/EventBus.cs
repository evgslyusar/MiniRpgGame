using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MiniRpgGame.Domain.Platform
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent @event);
        CancelSubscription Subscribe<TEvent>(Action<TEvent> eventHandler);
    }

    public class EventBus : IEventBus
    {
        private class Handlers
        {
            private readonly object _internalHandlersLock = new object();
            private readonly List<object> _internalHandlers;

            public Handlers(params object[] handlers)
            {
                _internalHandlers = new List<object>(handlers);
            }

            public void Add(object handler)
            {
                lock (_internalHandlersLock)
                    _internalHandlers.Add(handler);
            }

            public void Remove(object handler)
            {
                lock (_internalHandlersLock)
                    _internalHandlers.Remove(handler);
            }

            public IEnumerable<object> AsEnumerable()
            {
                lock (_internalHandlersLock)
                    return _internalHandlers.ToArray();
            }
        }

        private readonly ConcurrentDictionary<Type, Handlers> _subscriptions = new ConcurrentDictionary<Type, Handlers>();

        public void Publish<TEvent>(TEvent @event)
        {
            if (!_subscriptions.TryGetValue(typeof (TEvent), out var handlers))
                return;
            foreach (var handler in handlers.AsEnumerable())
                ((Action<TEvent>) handler)(@event);
        }

        public CancelSubscription Subscribe<TEvent>(Action<TEvent> eventHandler)
        {
            AddSubscription(eventHandler);
            return GetCancelSubscriptionDelegate(eventHandler);
        }

        private void AddSubscription<TEvent>(Action<TEvent> handler)
        {
            var type = typeof (TEvent);

            _subscriptions.AddOrUpdate(type, 
                addValue: new Handlers(handler),
                updateValueFactory: (key, existingHandlers) => 
                {
                    existingHandlers.Add(handler);
                    return existingHandlers;
                }
            );
        }

        private CancelSubscription GetCancelSubscriptionDelegate<TEvent>(Action<TEvent> eventHandler)
        {
            return () =>
                {
                    if (!_subscriptions.TryGetValue(typeof (TEvent), out var handlers))
                        return;
                    handlers.Remove(eventHandler);
                };
        }
    }

    public delegate void CancelSubscription();
}