using System;
using System.Collections.Generic;
using System.Text;
using EventChannelUnit.Runtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace EventChannelUnit.Editor
{
    public abstract class ChannelEditorBase<TCh, TUnitArg> : UnityEditor.Editor 
        where TCh : ChannelBaseObject
    {
        public string LogText;
        protected TCh Channel; 
        
        protected readonly List<string> Logs = new List<string>();
        protected string LastDelegateAsText = "";
        protected int UpdateTimerCount = 0; // updateTimer
        protected const int UpdateTimerSpan = 60; // if count > this, update
        
        protected List<Object> Objects;
        protected List<ChannelEventUnitBase<TUnitArg>> Units;
        protected ListView ObjectListenersListView;
        protected ListView UnitListenersListView;
        
        protected virtual void OnEnable()
        {
            if (Channel == null) Channel = target as TCh;
            EditorApplication.update += Update;
        }

        protected virtual void OnDisable()
        {
            EditorApplication.update -= Update;
            Channel = null;
        }
        protected void Update()
        {
            if (UpdateTimerCount++ < UpdateTimerSpan) return;
            UpdateTimerCount = 0;
            
            var delegateAsText = DelegatesToText(Channel.GetInvocationList());
            if (LastDelegateAsText != delegateAsText)
            {
                Units = GetUnitListeners();
                UnitListenersListView.itemsSource = Units;
                UnitListenersListView.Rebuild();
                LastDelegateAsText = delegateAsText;
            }
        }
        protected string DelegatesToText(Delegate[] delegateSubscribers)
        {
            if (delegateSubscribers == null) return "";

            var subscribers = new List<string>();
            foreach (var subscriber in delegateSubscribers)
            {
                if(subscriber.Target is ChannelEventUnitBase<TUnitArg> subscriberTarget)
                {
                    subscribers.Add(subscriberTarget.ToString());
                }

                if (subscriber.Target is Object obj)
                {
                    subscribers.Add(ObjectToLabelText(obj));
                }
            }
            var sb = new StringBuilder();
            foreach (var subscriber in subscribers) sb.AppendLine(subscriber);
            return sb.ToString();
        }
        
        protected string ObjectToLabelText(Object obj)
        {
            return $"{obj.name} ({obj.GetType().Name})";
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // Draw default elements in the inspector
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            // Add a ListView to show Object Listeners
            root.Add(SectionLabel("Object Listeners:"));
            Objects = GetObjectListeners();
            ObjectListenersListView = new ListView(Objects, 20, MakeItem, BindObjects);
            root.Add(ObjectListenersListView);
            
            // Add a ListView to show Unit Listeners
            root.Add(SectionLabel("Unit Listeners:"));
            Units = GetUnitListeners();
            UnitListenersListView = new ListView(Units, 20, MakeItem, BindUnits);
            root.Add(UnitListenersListView);
            
            // Test for Raise Event
            root.Add(SectionLabel("Raise Event:"));
            root.Add(CreateTestValueField());
            root.Add(CreateTestValueButton());
            
            // Log
            root.Add(SectionLabel("Log:"));
            var log = new TextField() { bindingPath = nameof(LogText) };
            log.BindProperty(new SerializedObject(this));
            root.Add(log);
            
            return root;
        }
        
        protected abstract VisualElement CreateTestValueField();
        protected abstract VisualElement CreateTestValueButton();

        private Label SectionLabel(string text)
        {
            return new Label
            {
                text = text,
                style =
                {
                    borderBottomWidth = 1,
                    borderBottomColor = Color.grey,
                    marginBottom = 2,
                    marginTop = 12
                }
            };
        }


        
        protected VisualElement MakeItem()
        {
            var element = new VisualElement();
            var label = new Label();
            element.Add(label);
            return element;
        }

        protected void BindObjects(VisualElement element, int index)
        {
            if (index >= Objects.Count) return;
            var item = Objects[index];
            Label label = (Label)element.ElementAt(0);
            label.text = ObjectToLabelText(item);
            // Attach a ClickEvent to the label
            label.RegisterCallback<MouseDownEvent>(evt =>
            {
                // Ping the item in the Hierarchy
                EditorGUIUtility.PingObject(item);
            });
        }
        protected void BindUnits(VisualElement element, int index)
        {
            if (index >= Units.Count) return;
            var item = Units[index];
            Label label = (Label)element.ElementAt(0);
            label.text = item.ToString();
            label.RegisterCallback<MouseDownEvent>(evt =>
            {
                // Ping the item in the Hierarchy
                EditorGUIUtility.PingObject(item.GraphGameObject);
                OpenWindow(item.GraphReference, item);
            });
        }
        
        protected List<Object> GetObjectListeners()
        {
            var listeners = new List<Object>();
            if (Channel == null) return listeners;

            // Get all delegates subscribed to the OnEventRaised action
            var delegateSubscribers = Channel.GetInvocationList();
            if (delegateSubscribers == null) return listeners;

            foreach (var subscriber in delegateSubscribers)
            {
                // Append to the list and return
                if(subscriber.Target is Object subscriberTarget && subscriberTarget != this) 
                {
                    listeners.Add(subscriberTarget);
                }
            }
            return listeners;
        }

        protected List<ChannelEventUnitBase<TUnitArg>> GetUnitListeners()
        {
            var listeners = new List<ChannelEventUnitBase<TUnitArg>>();
            if (Channel == null) return listeners;

            // Get all delegates subscribed to the OnEventRaised action
            var delegateSubscribers = Channel.GetInvocationList();
            if (delegateSubscribers == null) return listeners;

            foreach (var subscriber in delegateSubscribers)
            {
                // Append to the list and return
                if(subscriber.Target is ChannelEventUnitBase<TUnitArg> subscriberTarget)
                {
                    listeners.Add(subscriberTarget);
                }
            }
            return listeners;
        }
        
        protected void OpenWindow(GraphReference reference, IGraphElement element)
        {
            if (reference != null)
            {
                GraphWindow.OpenActive(reference);
                try {
                    var graphWindow = EditorWindow.GetWindow<GraphWindow>();
                    graphWindow.context.graph.zoom = 1f;
                    graphWindow.context.selection.Select( element );
                    graphWindow.context.canvas.ViewElements(new [] { element });
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            } 
        }
    }

    // for void channel
    public abstract class ChannelBaseEditor : ChannelEditorBase<ChannelBase, EmptyEventArgs>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if (Channel) Channel.OnEventRaised += AppendLog;
        }

        protected override void OnDisable()
        {
            if (Channel) Channel.OnEventRaised -= AppendLog;
            base.OnDisable();
        }
        protected void AppendLog()
        {
            Logs.Add($"{DateTime.Now}: Invoked.");
            if(Logs.Count > 10) Logs.RemoveAt(0);
            var sb = new StringBuilder();
            foreach (var log in Logs) sb.AppendLine(log);
            LogText = sb.ToString();
        }
        protected override VisualElement CreateTestValueButton()
        {
            return new Button(() => { Channel.RaiseEvent(); }) { text = "Raise" };
        }
    }

    // for single arg channel
    public abstract class ChannelBaseEditor<TCh, T> : ChannelEditorBase<TCh, T> where TCh : ChannelBase<T>
    {
        protected T TestValue;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            if (Channel) Channel.OnEventRaised += AppendLog;
        }

        protected override void OnDisable()
        {
            if (Channel) Channel.OnEventRaised -= AppendLog;
            base.OnDisable();
        }
        
        protected void AppendLog(T value)
        {
            Logs.Add($"{DateTime.Now}: {value.ToString()}");
            if(Logs.Count > 10) Logs.RemoveAt(0);
            var sb = new StringBuilder();
            foreach (var log in Logs) sb.AppendLine(log);
            LogText = sb.ToString();
        }

        protected override VisualElement CreateTestValueButton()
        {
            return new Button(() => { Channel.RaiseEvent(TestValue); }) { text = "Raise" };
        }
    }
}
