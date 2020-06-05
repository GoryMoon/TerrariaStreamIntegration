﻿using System.Collections.Generic;
 using System.IO;
 using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using Microsoft.Xna.Framework;
using Terraria;
 using Terraria.IO;
 using Terraria.Localization;

 // ReSharper disable InconsistentNaming
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable ArrangeTypeModifiers

namespace TerrariaStreamIntegration
{
    [HarmonyPatch(typeof(Main), "QuitGame")]
    class Main_QuitGame_Patch
    {
        private static void Prefix()
        {
            MyMod.Instance.Unload();
        }
    }

    [HarmonyPatch(typeof(Main), "UpdateUIStates", typeof(GameTime))]
    class Main_UpdateUIStates_Patch
    {
        private static void Postfix(GameTime gameTime)
        {
            MyMod.Instance.UpdateUI(gameTime);
        }
    }

    [HarmonyPatch(typeof(Main), "Initialize_AlmostEverything")]
    class Main_Initialize_AlmostEverything_Patch
    {
        private static void Postfix()
        {
            MyMod.Instance.Load();
        }
    }
    
    [HarmonyPatch(typeof(WorldGen), "UpdateWorld")]
    class WorldGen_UpdateWorld_Patch
    {
        private static void Prefix()
        {
            MyMod.Instance.UpdateWorld();
        }
    }
    
    [HarmonyPatch(typeof(WorldFile), "LoadHeader", typeof(BinaryReader))]
    class WorldFile_LoadHeader_Patch
    {
        private static void Postfix()
        {
            MyMod.Instance.DeathCounter.Load();
        }
    }
    
    [HarmonyPatch(typeof(WorldFile), "SaveWorldHeader", typeof(BinaryWriter))]
    class WorldFile_SaveWorldHeader_Patch
    {
        private static void Postfix()
        {
            MyMod.Instance.DeathCounter.Save();
        }
    }
    
    [HarmonyPatch(typeof(Player), "DropTombstone", typeof(int), typeof(NetworkText), typeof(int))]
    class Player_DropTombstone_Patch
    {
        private static void Postfix(Player __instance)
        {
            MyMod.Instance.DeathCounter.OnPlayerDeath(__instance);
        }
    }
    
    [HarmonyPatch(typeof(NPC), "NewNPC", typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(float), typeof(float), typeof(float), typeof(int))]
    class NPC_NewNPC_Patch
    {
        private static void Postfix(int __result)
        {
            Utils.LatestSpawn = __result;
        }
    }

    [HarmonyPatch(typeof(MessageBuffer), "GetData", new[] {typeof(int), typeof(int), typeof(int)},
        new[] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out})]
    class MessageBuffer_GetData_Patch
    {
        private static readonly MethodInfo _handleNetMethod = SymbolExtensions.GetMethodInfo(() => MyMod.HandleModPacket(0, 0, null));
        
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var patched = false;
            foreach (var instruction in instructions)
            {
                if (!patched)
                {
                    if (instruction.opcode == OpCodes.Stind_I4)
                    {
                        patched = true;
                        yield return instruction;
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldloc_1);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Call, _handleNetMethod);
                        yield return new CodeInstruction(OpCodes.Nop);
                        continue;
                    }
                }
                yield return instruction;
            }
        }
    }

    /*
    [HarmonyPatch(typeof(MessageBuffer), "GetData", new[]{typeof(int), typeof(int), typeof(int)}, new[]{ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out})]
    class MessageBuffer_GetData_Patch
    {
        private static readonly FieldInfo _maxMsgField = AccessTools.Field(typeof(Main), "maxMsg");
        private static readonly FieldInfo _activeNetDiagnosticsUIField = AccessTools.Field(typeof(Main), "ActiveNetDiagnosticsUI");
        private static readonly FieldInfo _netModeField = AccessTools.Field(typeof(Main), "netMode");
        private static readonly MethodInfo _handleNetMethod = SymbolExtensions.GetMethodInfo(() => MyMod.HandleModPacket(null));
        
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var handleNetLabel = generator.DefineLabel();
            var stelem = 0;
            var handleSwitchInjected = false;

            var lodloc = 0;
            var patchPacketId = true;
            var droppingInstructions = false;
            var packetIfLabel = generator.DefineLabel();
            
            foreach (var instruction in instructions)
            {
                // Allow higher packet ids
                if (patchPacketId)
                {
                    if (instruction.opcode == OpCodes.Ldloc_0 && lodloc < 2)
                    {
                        lodloc++;
                        if (lodloc == 2)
                        {
                            droppingInstructions = true;
                        }
                    }

                    if (droppingInstructions)
                    {
                        if (instruction.opcode == OpCodes.Ret)
                        {
                            droppingInstructions = false;
                        }
                        continue;
                    }

                    if (instruction.opcode == OpCodes.Ldsfld && (FieldInfo) instruction.operand == _activeNetDiagnosticsUIField)
                    {
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldc_I4, 140);
                        yield return new CodeInstruction(OpCodes.Clt);
                        yield return new CodeInstruction(OpCodes.Stloc_2);
                        yield return new CodeInstruction(OpCodes.Ldloc_2);
                        yield return new CodeInstruction(OpCodes.Brfalse_S, packetIfLabel);
                    }

                    if (instruction.opcode == OpCodes.Ldsfld && (FieldInfo) instruction.operand == _netModeField)
                    {
                        instruction.labels.Add(packetIfLabel);
                        patchPacketId = false;
                    }
                }
                
                
                
                // HandlePacket switch case
                if (instruction.opcode == OpCodes.Switch && !handleSwitchInjected)
                {
                    handleSwitchInjected = true;
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldc_I4, 250);
                    yield return new CodeInstruction(OpCodes.Sub);
                    yield return new CodeInstruction(OpCodes.Switch, new []{handleNetLabel});
                }
                else
                {
                    if (stelem == 5)
                    {
                        yield return new CodeInstruction(OpCodes.Ret);
                        var handle250 = new CodeInstruction(OpCodes.Ldarg_0);
                        handle250.labels.Add(handleNetLabel);
                        yield return handle250;
                        yield return new CodeInstruction(OpCodes.Call, _handleNetMethod);
                    }
                    
                    if (instruction.opcode == OpCodes.Stelem_I1)
                    {
                        stelem++;
                    }
                    yield return instruction;
                }
            }
        }
    }*/
}