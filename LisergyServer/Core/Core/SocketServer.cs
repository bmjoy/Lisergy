﻿using Game;
using Game.Events;
using LisergyServer.Commands;
using System;
using System.Net;
using System.Net.Sockets;
using Telepathy;

namespace LisergyServer.Core
{
    public abstract class SocketServer
    {
        private bool _running = false;

        protected readonly Server _socketServer;
        private Message _msg;
        private CommandExecutor _commandExecutor;
        protected StrategyGame _game;
        private int _port;

        public SocketServer(int port)
        {
            _port = port;
            _commandExecutor = new CommandExecutor();
            _commandExecutor.RegisterCommand(new HelpCommand(_commandExecutor));
            _socketServer = new Server();
            Serialization.LoadSerializers();
            _game = SetupGame();
            RegisterCommands(_game, _commandExecutor); 
           
        }

        public abstract void RegisterCommands(StrategyGame game, CommandExecutor executor);
        protected abstract ServerPlayer Auth(GameEvent ev, int connectionID);
        public abstract void Tick();
        public abstract void Disconnect(int connectionID);
        public abstract ServerType GetServerType();
        public abstract StrategyGame SetupGame();

        public void Stop()
        {
            _socketServer.Stop();
            _running = false;
        }

        public void RunServer()
        {
            _socketServer.Start(_port);
            _running = true;
            while (_running)
            {
                try
                {
                    _commandExecutor.HandleConsoleCommands();
                    Tick();
                    ReadSocketMessages();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Press any key to die");
                }
            }
        }

        private void ReadSocketMessages()
        {
            while (_socketServer.GetNextMessage(out _msg))
            {
                switch (_msg.eventType)
                {
                    case EventType.Connected:
                        Console.WriteLine("New Connection Received");
                        break;
                    case EventType.Data:
                        var message = _msg.data;

                        var ev = Serialization.ToEventRaw(message);
                        Game.Log.Debug($"Received {ev}");
                        var caller = Auth(ev, _msg.connectionId);
                        if (caller != null)
                        {
                            _game.NetworkEvents.RunCallbacks(caller, message);
                        }
                        break;
                    case EventType.Disconnected:
                        Disconnect(_msg.connectionId);
                        break;
                }
            }
        }
    }
}
