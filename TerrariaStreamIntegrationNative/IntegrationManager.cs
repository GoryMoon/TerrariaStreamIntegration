﻿using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using TerrariaStreamIntegration.Packets;

namespace TerrariaStreamIntegration
{
    public class IntegrationManager
    {
        
        private ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();
        private CancellationTokenSource _source;
        private Task _task;

        public void Start()
        {
            _source = new CancellationTokenSource();
            var token = _source.Token;
            _task = Task.Factory.StartNew(() =>
            {
                MyMod.Log.Info("Starting Integration connection");
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        using (var client = new NamedPipeClientStream(".", "Terraria", PipeDirection.In))
                        {
                            using (var reader = new StreamReader(client))
                            {
                                while (!token.IsCancellationRequested && !client.IsConnected)
                                {
                                    try
                                    {
                                        client.Connect(1000);
                                    }
                                    catch (TimeoutException)
                                    {
                                        // Ignore
                                    }
                                    catch (Win32Exception e)
                                    {
                                        MyMod.Log.Error(e, "Error in pipe connection");
                                        Thread.Sleep(500);
                                    }
                                    catch (Exception e)
                                    {
                                        MyMod.Log.Error(e, "Error in pipe connection");
                                        Thread.Sleep(500);
                                    }
                                }
                                MyMod.Log.Debug("Connected to Integration");
                                while (!token.IsCancellationRequested && client.IsConnected)
                                {
                                    if (reader.Peek() > 0)
                                    {
                                        var line = reader.ReadLine();
                                        if (line != null)
                                        {
                                            if (Utils.InGame())
                                            {
                                                Handle(line);
                                            }
                                            else
                                            {
                                                _messages.Enqueue(line);
                                            }
                                        }
                                    }
                                    reader.DiscardBufferedData();
                                    Thread.Sleep(50);
                                }
                                MyMod.Log.Debug("Disconnected to Integration");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MyMod.Log.Error(e, "Error in socket connection");
                    }
                }
            }, token);
        }

        public void Close()
        {
            if (_source != null)
            {
                _source.Cancel(true);
                _task?.Wait(1000);
                _task = null;
                _messages = null;
            }
        }
        
        public void Update()
        {
            while (_messages.TryDequeue(out var line))
            {
                Handle(line);
            }
        }

        private static void Handle(string line)
        {
            if (line.StartsWith("Action: "))
            {
                MyMod.Log.Debug(line);
                var action = line.Substring(8);
                MyMod.PacketHandler.SendToServer(new IntegrationPacket(action, false));
            }
            else if (line.StartsWith("Message: "))
            {
                MyMod.Log.Debug(line);
                var message = line.Substring(9);
                MyMod.PacketHandler.SendToServer(new IntegrationPacket(message, true));
            }
        }
    }
}