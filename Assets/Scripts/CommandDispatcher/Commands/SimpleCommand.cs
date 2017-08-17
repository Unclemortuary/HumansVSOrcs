
using UnityEngine.Events;
using System;
namespace CommandsDispatcher
{
    internal class SimpleCommand : ICommandMessage
    {
        internal Action uEvent;
        public void Set(object item)
        {
            if (item is Action)
            {
                uEvent = (Action)item;
            }
        }

        public object Invoke(object item)
        {
            uEvent.Invoke();
            return null; 
        }

        public bool Compare(object item)
        {
            if (!(item is Action)) return false;
            if (uEvent == null) return false;
            return ((Action)item).Equals(uEvent);
        }

        public bool CompareParametrType(object item)
        {
            if (item is NullParametr) return true;
            return false;
        }
        public bool CompareCallbackType(System.Type type)
        {
            return false;
        }
    }
}