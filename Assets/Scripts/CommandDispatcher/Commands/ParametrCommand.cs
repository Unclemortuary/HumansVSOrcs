using UnityEngine.Events;
using System; 
namespace CommandsDispatcher
{
    internal class ParametrCommand<T> : ICommandMessage
    {
        Action<T> uCommand;
        public void Set(object item)
        {
            if (item is Action<T>)
            {
                uCommand = (Action<T>)item;
            }
        }

        public object Invoke(object item)
        {
            if (item is T)
            {
                uCommand.Invoke((T)item);
            }
            return null;
        }

        public bool Compare(object item)
        {
            if (!(item is Action<T>)) return false;
            if (uCommand == null) return false;
            return ((Action<T>)item).Equals(uCommand);
        }

        public bool CompareParametrType(object item)
        {
            if (!(item is T)) return false;
            return true;
        }

        public bool CompareCallbackType(System.Type type)
        {
            return false;
        }
    }
}
