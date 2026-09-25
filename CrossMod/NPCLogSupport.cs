using BossesAsNPCs.NPCs;
using BossesAsNPCs.NPCs.TownNPCs;
using System;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossesAsNPCs.CrossMod
{
	public class NPCLogSupport : ModSystem
	{
		public override void Load()
		{
			/* Mod call doesn't work
			if (ModLoader.TryGetMod("NPCLog", out Mod npcLog) && InternalCrossModSupportList.NPCLog)
			{
				npcLog.Call("AddUnlockStep", ModContent.NPCType<KingSlime>(), GetCensusSpawnCondition("KingSlime"), () => NPC.downedSlimeKing);
			}
			*/

			// Since the Mod.Call doesn't work, detour its own method that adds support for Calamity and add my NPCs during that method.
			if (ModLoader.TryGetMod("NPCLog", out Mod npcLog) && InternalCrossModSupportList.NPCLog)
			{
				try
				{
					Type type_npcLog = npcLog.GetType(); // Get the typeof the mod
					Assembly assembly_npcLog = type_npcLog.Assembly; // Get the assembly of the mod
					Type type_npcLog_ModdedUnlocks = assembly_npcLog.GetType("NPCLog.Data.ModdedUnlocks"); // Get the class
					MethodInfo targetMethod = type_npcLog_ModdedUnlocks.GetMethod("LoadCalamity", BindingFlags.Static | BindingFlags.NonPublic); // Get the method

					MonoModHooks.Add(targetMethod, On_NPCLog_ModdedUnlocks_LoadCalamity); // Create the detour hook
				}
				catch
				{
					ModContent.GetInstance<BossesAsNPCs>().Logger.Error($"Failed to detour NPCLog.Data.ModdedUnlocks.LoadCalamity!");
				}
			}
		}

		private delegate void orig_NPCLog_ModdedUnlocks_LoadCalamity();

		private void On_NPCLog_ModdedUnlocks_LoadCalamity(orig_NPCLog_ModdedUnlocks_LoadCalamity orig)
		{
			orig(); // Run the original method.

			Type type_npcLog_ModdedUnlocks = ModLoader.GetMod("NPCLog").GetType().Assembly.GetType("NPCLog.Data.ModdedUnlocks"); // Get the class

			MethodInfo Method_NPCLog_ModdedUnlocks_AddStep = type_npcLog_ModdedUnlocks.GetMethod("AddStep", BindingFlags.Public | BindingFlags.Static); // Get the method
			MethodInfo Method_NPCLog_ModdedUnlocks_SetNote = type_npcLog_ModdedUnlocks.GetMethod("SetNote", BindingFlags.Public | BindingFlags.Static); // Get the method

			string note = "Added by Bosses As NPCs. The config must also allow this Town NPC to spawn.";

			// Use reflection to invoke the AddStep and SetNote methods.
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<KingSlime>(), GetCensusSpawnCondition("KingSlime"), () => NPC.downedSlimeKing]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<EyeOfCthulhu>(), GetCensusSpawnCondition("EyeOfCthulhu"), () => NPC.downedBoss1]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<EaterOfWorlds>(), GetCensusSpawnCondition("EaterOfWorlds"), () => BossesAsNPCsWorld.downedEoW]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<BrainOfCthulhu>(), GetCensusSpawnCondition("BrainOfCthulhu"), () => BossesAsNPCsWorld.downedBoC]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<QueenBee>(), GetCensusSpawnCondition("QueenBee"), () => NPC.downedQueenBee]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Skeletron>(), GetCensusSpawnCondition("Skeletron"), () => NPC.downedBoss3]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Deerclops>(), GetCensusSpawnCondition("Deerclops"), () => NPC.downedDeerclops]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<WallOfFlesh>(), GetCensusSpawnCondition("WallOfFlesh"), () => BossesAsNPCsWorld.downedWoF]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<QueenSlime>(), GetCensusSpawnCondition("QueenSlime"), () => NPC.downedQueenSlime]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<TheDestroyer>(), GetCensusSpawnCondition("TheDestroyer"), () => NPC.downedMechBoss1]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Retinazer>(), GetCensusSpawnCondition("Retinazer"), () => NPC.downedMechBoss2]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Spazmatism>(), GetCensusSpawnCondition("Spazmatism"), () => NPC.downedMechBoss2]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<SkeletronPrime>(), GetCensusSpawnCondition("SkeletronPrime"), () => NPC.downedMechBoss3]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Plantera>(), GetCensusSpawnCondition("Plantera"), () => NPC.downedPlantBoss]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Golem>(), GetCensusSpawnCondition("Golem"), () => NPC.downedGolemBoss]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<EmpressOfLight>(), GetCensusSpawnCondition("EmpressOfLight"), () => NPC.downedEmpressOfLight]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<DukeFishron>(), GetCensusSpawnCondition("DukeFishron"), () => NPC.downedFishron]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Betsy>(), GetCensusSpawnCondition("Betsy"), () => BossesAsNPCsWorld.downedBetsy]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<LunaticCultist>(), GetCensusSpawnCondition("LunaticCultist"), () => NPC.downedAncientCultist]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<MoonLord>(), GetCensusSpawnCondition("MoonLord"), () => NPC.downedMoonlord]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Dreadnautilus>(), GetCensusSpawnCondition("Dreadnautilus"), () => BossesAsNPCsWorld.downedDreadnautilus]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Mothron>(), GetCensusSpawnCondition("Mothron"), () => BossesAsNPCsWorld.downedMothron]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<Pumpking>(), GetCensusSpawnCondition("Pumpking"), () => NPC.downedHalloweenKing]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<IceQueen>(), GetCensusSpawnCondition("IceQueen"), () => NPC.downedChristmasIceQueen]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<MartianSaucer>(), GetCensusSpawnCondition("MartianSaucer"), () => NPC.downedMartians]);
			Method_NPCLog_ModdedUnlocks_AddStep.Invoke(null, [ModContent.NPCType<TorchGod>(), GetCensusSpawnCondition("TorchGod"), () => NPCHelper.DownedAnyBoss()]);

			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<KingSlime>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<EyeOfCthulhu>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<EaterOfWorlds>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<BrainOfCthulhu>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<QueenBee>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Skeletron>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Deerclops>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<WallOfFlesh>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<QueenSlime>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<TheDestroyer>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Retinazer>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Spazmatism>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<SkeletronPrime>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Plantera>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Golem>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<EmpressOfLight>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<DukeFishron>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Betsy>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<LunaticCultist>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<MoonLord>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Dreadnautilus>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Mothron>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<Pumpking>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<IceQueen>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<MartianSaucer>(), note]);
			Method_NPCLog_ModdedUnlocks_SetNote.Invoke(null, [ModContent.NPCType<TorchGod>(), note]);
		}

		private static string GetCensusSpawnCondition(string npc) => Language.GetTextValue($"Mods.BossesAsNPCs.NPCs.{npc}.Census.SpawnCondition");
	}
}
