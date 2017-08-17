using UnityEngine.Events;
using System;
using UnityEngine;
namespace CommandsDispatcher
{
    internal class SimpleCommandAnswer<T> : ICommandMessage
    {
        Func<T> uFunc;
        Type type;  
        public void Set(object item)
        {
            if (item is Func<T>)
            {
                uFunc = (Func<T>)item;
            }
        }

        public object Invoke(object item)
        {
            object answer = null; 
            answer = uFunc.Invoke();
            return answer;
        }

        public bool Compare(object item)
        {
            if (!(item is Func<T>)) return false;
            if (uFunc == null) return false;
            return ((Func<T>)item).Equals(uFunc);
        }

        public bool CompareParametrType(object item)
        {
            if (item is NullParametr) return true;
            return false;
        }

        public bool CompareCallbackType(System.Type type)
        {
            if (!(type == typeof(T))) return false;
            return true;
        }
    }
}
