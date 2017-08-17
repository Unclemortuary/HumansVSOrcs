namespace CommandsDispatcher
{
    interface ICommandMessage
    {
        void Set(object item);
        object Invoke(object item);
        bool Compare(object item);
        bool CompareCallbackType(System.Type type);
        bool CompareParametrType(object item);
    }
}