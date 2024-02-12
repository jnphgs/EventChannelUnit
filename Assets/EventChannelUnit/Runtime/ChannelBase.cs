using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace EventChannelUnit.Runtime
{
    #region Channel Base

    /// <summary>
    /// use for search all Channel
    /// </summary>
    public abstract class ChannelBaseObject : ScriptableObject
    {
        [SerializeField, TextArea(3, 5)] protected string description;
        public abstract Delegate[] GetInvocationList();
    }

    /// <summary>
    /// use for void channel
    /// </summary>
    public abstract class ChannelBase : ChannelBaseObject
    {
        public UnityAction OnEventRaised;
        public override Delegate[] GetInvocationList() => OnEventRaised?.GetInvocationList();
        public virtual void RaiseEvent() => OnEventRaised?.Invoke();
    }
    
    /// <summary>
    /// use for single arg channel
    /// </summary>
    public abstract class ChannelBase<T0> : ChannelBaseObject
    {
        public event UnityAction<T0> OnEventRaised;
        public override Delegate[] GetInvocationList() => OnEventRaised?.GetInvocationList();
        public virtual void RaiseEvent(T0 arg) => OnEventRaised?.Invoke(arg);
    }
    #endregion
    
    #region Event Unit for Visual Scripting

    public abstract class ChannelEventUnitBase<TArgs> : EventUnit<TArgs>
    {
        protected abstract string EventHeader();
        protected string EventChannelName => guid.ToString();
        protected string EventName => EventHeader() + EventChannelName;
        
        [DoNotSerialize] public ValueInput Channel { get; protected set; }
        protected override bool register => true;
        public GameObject GraphGameObject;
        public GraphReference GraphReference;
        
        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(EventName);
        }
        public override void StartListening(GraphStack stack)
        {
            GraphGameObject = stack.gameObject;
            GraphReference = stack.ToReference();
            base.StartListening(stack);
        }

        protected override void StopListening(GraphStack stack, bool destroyed)
        {
            GraphGameObject = null;
            GraphReference = null;
            base.StopListening(stack, destroyed);
        }
        public override string ToString()
        {
            return $"{GraphGameObject.name} ({graph.title} >> {base.ToString()})";
        }
    }
    
    /// <summary>
    /// Event Unit for void channel
    /// </summary>
    public class ChannelEventUnit<TCh> : ChannelEventUnitBase<EmptyEventArgs> where TCh : ChannelBase
    {
        protected override string EventHeader() => $"{typeof(TCh).Name}";
        private TCh _channel;
        
        protected override void Definition()
        {
            base.Definition();
            Channel = ValueInput<TCh>(nameof(Channel), null);
        }

        public override void StartListening(GraphStack stack)
        {
            base.StartListening(stack);
            _channel = Flow.FetchValue<TCh>(Channel, stack.ToReference());
            if (_channel) _channel.OnEventRaised += RaiseEvent;
        }

        protected override void StopListening(GraphStack stack, bool destroyed)
        {
            base.StopListening(stack, destroyed);
            if (_channel) _channel.OnEventRaised -= RaiseEvent;
        }
        private void RaiseEvent()
        {
            EventBus.Trigger(EventName);
        }
    }

    /// <summary>
    /// Event Unit for single arg channel
    /// </summary>
    public class ChannelEventUnit<TCh, T0> : ChannelEventUnitBase<T0> where TCh : ChannelBase<T0>
    {
        protected override string EventHeader() => $"{typeof(TCh).Name}";
        [DoNotSerialize] public ValueOutput Value { get; protected set; }
        protected virtual string ValueOutputLabel => nameof(Value);
        private TCh _channel;
        protected override void Definition()
        {
            base.Definition();
            Channel = ValueInput<TCh>(nameof(Channel), null);
            Value = ValueOutput<T0>(ValueOutputLabel);
        }

        public override void StartListening(GraphStack stack)
        {
            base.StartListening(stack);
            _channel = Flow.FetchValue<TCh>(Channel, stack.ToReference());
            if (_channel) _channel.OnEventRaised += RaiseEvent;
        }

        protected override void StopListening(GraphStack stack, bool destroyed)
        {
            base.StopListening(stack, destroyed);
            if (_channel) _channel.OnEventRaised -= RaiseEvent;
        }
        protected override void AssignArguments(Flow flow, T0 data)
        {
            flow.SetValue(Value, data);
        }
        
        private void RaiseEvent(T0 value)
        {
            EventBus.Trigger(EventName, value);
        }
    }
    
    #endregion
}
