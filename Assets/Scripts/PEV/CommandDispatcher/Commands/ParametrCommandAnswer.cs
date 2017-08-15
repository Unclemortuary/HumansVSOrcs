using UnityEngine.Events;
using System;
using UnityEngine;
namespace CommandsDispatcher
{
    internal class ParametrCommandAnswer<U,T> : ICommandMessage
    {
        Func<U,T> uCommand;
        public void Set(object item)
        {
            if (item is Func<U,T>)
            {
                uCommand = (Func<U,T>)item;
            }
        }

        public object Invoke(object item)
        {
            object answer = null;
            if (item is U)
            {
                answer = uCommand.Invoke((U)item);
            }
            return answer;
        }

        public bool Compare(object item)
        {
            if (!(item is Func<U,T>)) return false;
            if (uCommand == null) return false;
            return ((Func<U,T>)item).Equals(uCommand);
        }

        public bool CompareParametrType(object item)
        {
            if (!(item is U)) return false;
            return true;
        }
        public bool CompareCallbackType(System.Type type)
        {
            if (!(type == typeof(T))) return false;
            return true;
        }
    }
}
