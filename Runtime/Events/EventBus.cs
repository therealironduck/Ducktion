using System;
using System.Collections.Generic;
using TheRealIronDuck.Ducktion.Attributes;
using TheRealIronDuck.Ducktion.Logging;

namespace TheRealIronDuck.Ducktion.Events
{
    /// <summary>
    /// This is a simple event bus implementation. It allows you to listen to events and fire them.
    /// Events must implement the IEvent interface.
    ///
    /// The event bus can be enabled/disabled in the container configuration and fetched like any
    /// service.
    /// </summary>
    public class EventBus
    {
        /// <summary>
        /// The internal list of all registered event listeners. Events itself don't need to be registered
        /// specifically, as they are registered when the first listener is added.
        /// </summary>
        private readonly Dictionary<Type, List<Delegate>> _eventListeners = new();

        /// <summary>
        /// The logger is injected by the container. It is used to log info messages if no listeners
        /// were found for a specific event.
        /// </summary>
        [Resolve] private readonly DucktionLogger _logger;

        /// <summary>
        /// Register an event listener. The listener will be called when the event is fired and be
        /// given the event as a parameter.
        /// </summary>
        /// <param name="action">The action that should run when the event is fired</param>
        /// <typeparam name="TEvent">The event for which the listener is registered</typeparam>
        public void Listen<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            if (_eventListeners.TryGetValue(typeof(TEvent), out var listeners))
            {
                listeners.Add(action);
                return;
            }

            _eventListeners.Add(typeof(TEvent), new List<Delegate> { action });
        }
        
        /// <summary>
        /// Forget a previously registered event listener. The listener will no longer be called when
        /// the event is fired.
        /// </summary>
        /// <param name="action">The action to forget</param>
        /// <typeparam name="TEvent">The event where the listener was registered</typeparam>
        public void Forget<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            if (!_eventListeners.TryGetValue(typeof(TEvent), out var listeners))
            {
                _logger.Log(
                    LogLevel.Info,
                    $"No event listeners found for event {typeof(TEvent)}"
                );
                return;
            }

            listeners.Remove(action);
        }

        /// <summary>
        /// Fire an event. This will call all registered listeners for the event.
        /// </summary>
        /// <param name="event">The event which should be fired</param>
        public void Fire(IEvent @event)
        {
            if (!_eventListeners.ContainsKey(@event.GetType()))
            {
                _logger.Log(
                    LogLevel.Info,
                    $"No event listeners found for event {@event.GetType()}"
                );
                return;
            }

            _eventListeners[@event.GetType()].ForEach(listener => listener.DynamicInvoke(@event));
        }

        /// <summary>
        /// Clear all listeners for a specific event.
        /// </summary>
        /// <typeparam name="TEvent">The event which should be forgotten</typeparam>
        public void Clear<TEvent>() where TEvent : IEvent
        {
            _eventListeners.Remove(typeof(TEvent));
        }
    }
}