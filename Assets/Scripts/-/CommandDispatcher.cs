
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace CommandsDispatcher
{
    internal class NullParametr { };
    public class CommandDispatcher<T> where T : struct, IComparable, IConvertible, IFormattable
    {

        private Dictionary<string, List<ICommandMessage>> commandDictionary;

        private static CommandDispatcher<T> commandManager;

        public static CommandDispatcher<T> Instance
        {
            get
            {
                if (commandManager == null)
                {
                    commandManager = new CommandDispatcher<T>();
                    commandManager.Init();
                }
                return commandManager;
            }
        }

        private void Init()
        {
            if (commandDictionary == null)
            {
                commandDictionary = new Dictionary<string, List<ICommandMessage>>();
            }
        }

        public void StartListening(T commandName, Action listener, int? id = null)
        {
            SimpleCommand simpleCommand = new SimpleCommand();
            simpleCommand.Set(listener);
            AddCommandMessage(commandName, simpleCommand, id);
        }

        public void StartListening<U>(T commandName, Action<U> listener, int? id = null)
        {
            ParametrCommand<U> parametrCommand = new ParametrCommand<U>();
            parametrCommand.Set(listener);
            AddCommandMessage(commandName, parametrCommand, id);
        }

        public void StartListeningFunc<P>(T commandName, Func<P> listener, int? id = null)
        {
            SimpleCommandAnswer<P> simpleCommandAnswer = new SimpleCommandAnswer<P>();
            simpleCommandAnswer.Set(listener);
            AddCommandMessage(commandName, simpleCommandAnswer, id);
        }
        public void StartListeningFunc<U, P>(T commandName, Func<U, P> listener, int? id = null)
        {
            ParametrCommandAnswer<U, P> parametrCommandAnswer = new ParametrCommandAnswer<U, P>();
            parametrCommandAnswer.Set(listener);
            AddCommandMessage(commandName, parametrCommandAnswer, id);
        }



        private void AddCommandMessage(T commandName, ICommandMessage parametrCommand, int? id = null)
        {
            List<ICommandMessage> commandList = null;
            string value = commandName.ToString();
            value = id != null ? value + id.ToString() : value;
            if (Instance.commandDictionary.TryGetValue(value, out commandList))
            {
                commandList.Add(parametrCommand);
            }
            else
            {
                commandList = new List<ICommandMessage>();
                commandList.Add(parametrCommand);
                Instance.commandDictionary.Add(value, commandList);
            }
        }


        public void StopListening(T commandName, Action listener, int? id = null)
        {
            RemoveMessage(commandName, listener, id);
        }

        public void StopListening<U>(T commandName, Action<U> listener, int? id = null)
        {
            RemoveMessage(commandName, listener, id);
        }

        public void StopListeningFunc<P>(T commandName, Func<P> listener, int? id = null)
        {
            RemoveMessage(commandName, listener, id);
        }

        public void StopListeningFunc<U, P>(T commandName, Func<U, P> listener, int? id = null)
        {
            RemoveMessage(commandName, listener, id);
        }


        private void RemoveMessage(T commandName, object listener, int? id = null)
        {
            if (commandManager == null)
                return;
            List<ICommandMessage> commandList = null;
            string value = commandName.ToString();
            value = id != null ? value + id.ToString() : value;
            if (Instance.commandDictionary.TryGetValue(value, out commandList))
            {
                for (int i = commandList.Count - 1; i >= 0; i--)
                {

                    if (commandList[i].Compare(listener))
                        commandList.RemoveAt(i);
                }
            }
        }

        public void TriggerCommand(T commandName)
        {
            TriggerCommand<object>(commandName, new NullParametr(), null);
        }

        public void TriggerCommand(T commandName, int id)
        {
            TriggerCommand<object>(commandName, new NullParametr(), id);
        }

        public void TriggerCommand<U>(T commandName, U parametr, int? id = null)
        {
            Trigger<U, NullParametr>(commandName, parametr, null, id, false);
        }


        public void TriggerFunc<P>(T commandName, Action<P> callback, int? id = null)
        {
            Trigger<NullParametr, P>(commandName, new NullParametr(), callback, id, true);
        }

        public void TriggerFunc<U, P>(T commandName, U parametr, Action<P> callback, int? id = null)
        {
            Trigger<U, P>(commandName, parametr, callback, id, true);
        }



        private void Trigger<U, P>(T commandName, U parametr, Action<P> callback, int? id = null, bool func = false)
        {
            List<ICommandMessage> commandList = null;
            string value = commandName.ToString();
            value = id != null ? value + id.ToString() : value;

            if (Instance.commandDictionary.TryGetValue(value, out commandList))
            {
                for (int i = commandList.Count - 1; i >= 0; i--)
                {
                    if (commandList[i].CompareParametrType(parametr))
                    {
                        //Type p = typeof(P);
                        if (func)
                        {
                            if (commandList[i].CompareCallbackType(typeof(P)))
                            {
                                object answer = commandList[i].Invoke(parametr);
                                callback((P)answer);
                            }
                        }
                        else
                        {
                            if (callback == null)
                            {
                                /*object answer =*/
                                commandList[i].Invoke(parametr);
                            }
                        }
                    }

                }
            }
        }

        /*    public void ShowAllDispatcher()
            {
                foreach (var v in commandDictionary)
                {
                    Debug.Log(v.Key); 
                }
            }*/
    } // class CommandDispatcher //

}