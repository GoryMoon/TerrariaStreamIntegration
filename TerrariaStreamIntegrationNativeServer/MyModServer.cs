using System;
using System.IO;
using System.Reflection;
using Harmony;
using NLog;
using NLog.Layouts;
using Terraria;
using TerrariaStreamIntegration.Packets;

// ReSharper disable UnusedMember.Global

namespace TerrariaStreamIntegration
{
	public class MyModServer
	{
		public static MyModServer Instance { get; private set; }
		public ActionManager ActionManager { get; private set; }
		public DeathCounter DeathCounter { get; }
		
		private readonly Logger _logger;
		public static Logger Log => Instance._logger;

		private PacketHandler _packetHandler;
		public static PacketHandler PacketHandler => Instance._packetHandler;

		private MyModServer(Logger logger)
		{
			Instance = this;
			DeathCounter.FileName = AssemblyDirectory + DeathCounter.FileName;
			DeathCounter = new DeathCounter();
			
			_logger = logger;
			_logger.Info("Starting patching");
			var harmony = HarmonyInstance.Create("se.gory_moon.terraria.stream_integration");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			_logger.Info("Done patching");
		}

		public void Load()
		{
			_logger.Info("Loading mod on: Server");
			ActionManager = new ActionManager();

			_packetHandler = new PacketHandler();
			_packetHandler.RegisterPacket(0, typeof(IntegrationPacket));
			_packetHandler.RegisterPacket(1, typeof(ManaHealPacket));
			_packetHandler.RegisterPacket(2, typeof(DropItemPacket));
			_packetHandler.RegisterPacket(3, typeof(SoundPacket));
			_packetHandler.RegisterPacket(4, typeof(BuffPacket));
		}

		public void Unload()
		{
			Instance = null;
			ActionManager = null;
		}
		
		public void UpdateWorld()
		{
			ActionManager.Update();
		}

		public static void Hijack(byte msg, MessageBuffer buffer)
		{
			Log.Debug($"Msg: {msg}");
		}
		
		public static void HandleModPacket(byte packet, int start, MessageBuffer buffer)
		{
			if (packet == 250)
			{
				if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
					++Netplay.Connection.StatusCount;
				if (buffer.reader == null)
					buffer.ResetReader();
				buffer.reader.BaseStream.Position = start;
				
				Log.Debug("Got message from a client");
				Instance._packetHandler?.HandlePacket(buffer.reader, buffer.whoAmI);
			}
		}

		private static string AssemblyDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
		
		// ModLoader Injection
		public static void InitMod()
		{
			var caller = Assembly.GetEntryAssembly()?.GetName().Name;
			var server = caller != null && caller.Equals("TerrariaServer");
			if (caller == null || !server)
			{
				return;
			}
			
			var config = new NLog.Config.LoggingConfiguration();
			var logfile = new NLog.Targets.FileTarget("logfile")
			{
				FileName = $"{AssemblyDirectory}/logs/server.log", 
				MaxArchiveFiles = 10, 
				Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${logger}|${threadid}|${message}|${exception:format=tostring}"),
				ArchiveOldFileOnStartup = true
			};
			config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
			LogManager.Configuration = config;

			try
			{
				var myMod = new MyModServer(LogManager.GetLogger("StreamIntegration"));
			}
			catch (Exception e)
			{
				var logger = LogManager.GetCurrentClassLogger();
				logger.Error(e, "Error loading mod");
			}
		}
	}
}