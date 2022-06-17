using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EditorTools
{
    [System.Serializable]
    public struct Event
    {
        public UnityAction action;
    }
    
    [System.Serializable]
    public struct Event<T>
    {
        public UnityAction<T> action;
    }

    public class NotifyOnChangeAttribute : System.Attribute
    {
        public bool Recursive { get; set; } = false;
    }

    public class GenerateCustomEditorAttribute : System.Attribute
    {
    }
    
    public class EditorButtonAttribute : System.Attribute
    {}
    
    public class FlattenAttribute : System.Attribute
    {}

    public class ChangedCallbackAttribute : System.Attribute
    {
        public string[] Paths;

        public ChangedCallbackAttribute(params string[] paths)
        {
            Paths = paths;
        }
    }
    
    
    [System.Serializable]
    public struct Data0
    {
        public int thing;
        
        [System.Serializable]
        public struct NestedStruct
        {
            public int nestedThing;
        }

        public NestedStruct someStuff;
        public string someName;
    }

    [System.Serializable]
    public struct Data0Events
    {
        public T GetByPath<T>(string path)
        {
            switch (path)
            {
                case nameof(Data0.thing): return (T) (object) thingChanged;
                case nameof(Data0.someName): return (T) (object) someNameChanged;
                case "": return (T) (object) someNameChanged;
                case nameof(Data0.someStuff) + "." + nameof(Data0.NestedStruct.nestedThing):
                    return (T) (object) someStuffEvents.nestedThingChanged;
                default:
                {
                    Debug.LogErrorFormat("No event found for key {0}", path);
                    return default(T);
                }
            }
        }
        
        
        public Event<int> thingChanged;
        public Event everythingChanged;
    
        public struct NestedStructEvents
        {
            public Event<int> nestedThingChanged;
        }
    
        public NestedStructEvents someStuffEvents;
        public Event<string> someNameChanged;
    }
    
    [GenerateCustomEditor]
    public class Script : MonoBehaviour
    {
        [Flatten]
        [SerializeField] internal Data0 _data;

        internal Data0Events _events;

        [ChangedCallback("_data.thing")]
        public void OnThingChanged(int oldValue, int newValue)
        {
            Debug.LogFormat("_data.thing changed from {0} to {1}", oldValue, newValue);
        }
        
        [ChangedCallback]
        public void OnAnyChanged()
        {
            Debug.LogFormat("Everything just changed");
        }
    }
}

