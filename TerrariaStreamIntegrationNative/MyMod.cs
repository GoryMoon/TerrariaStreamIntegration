using System;
using System.IO;
using System.Reflection;
using Harmony;
using Microsoft.Xna.Framework;
using NLog;
using NLog.Layouts;
using Terraria;
using TerrariaStreamIntegration.Packets;

// ReSharper disable UnusedMember.Global

namespace TerrariaStreamIntegration
{
	public class MyMod
	{
		public ActionManager ActionManager { get; private set; }
		private IntegrationManager _integrationManager;
		public DeathCounter DeathCounter { get; private set; }
		
		public static MyMod Instance { get; private set; }

		private readonly Logger _logger;
		public static Logger Log => Instance._logger;

		private PacketHandler _packetHandler;
		public static PacketHandler PacketHandler => Instance._packetHandler;

		private MyMod(Logger logger)
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

		~MyMod()
		{
			Unload();
		}
		
		internal void Load()
		{
			_logger.Info($"Loading mod on: {(Utils.IsClient() ? "Client": "Server")}");
			ActionManager = new ActionManager();
			
			_packetHandler = new PacketHandler();
			_packetHandler.RegisterPacket(0, typeof(IntegrationPacket));
			_packetHandler.RegisterPacket(1, typeof(ManaHealPacket));
			_packetHandler.RegisterPacket(2, typeof(DropItemPacket));
			_packetHandler.RegisterPacket(3, typeof(SoundPacket));
			_packetHandler.RegisterPacket(4, typeof(BuffPacket));
			
			if (Utils.IsClient())
			{
				_integrationManager = new IntegrationManager();
				_integrationManager.Start();
			}
		}
		
		internal void Unload()
		{
			if (_integrationManager != null && Utils.IsClient())
			{
				_integrationManager.Close();
			}
			Instance = null;
			_integrationManager = null;
			ActionManager = null;
		}

		// ReSharper disable once InconsistentNaming
		internal void UpdateUI(GameTime gameTime)
		{
			if (_integrationManager != null && Utils.IsClient() && Utils.InGame())
			{
				_integrationManager.Update();
			}
		}

		internal void UpdateWorld()
		{
			if (Utils.IsServer())
			{
				ActionManager.Update();
			}
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
				
				Log.Debug("Got message from a server");
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
			if (caller == null || server)
			{
				return;
			}
			
			var config = new NLog.Config.LoggingConfiguration();
			var logfile = new NLog.Targets.FileTarget("logfile")
			{
				FileName = $"{AssemblyDirectory}/logs/client.log", 
				MaxArchiveFiles = 10, 
				Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${logger}|${threadid}|${message}|${exception:format=tostring}"),
				ArchiveOldFileOnStartup = true
			};
			config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
			LogManager.Configuration = config;

			try
			{
				Main.OnEnginePreload += () =>
				{
					var myMod = new MyMod(LogManager.GetLogger("StreamIntegration"));
				};
			}
			catch (Exception e)
			{
				var logger = LogManager.GetCurrentClassLogger();
				logger.Error(e, "Error loading mod");
			}
		}
	}
}