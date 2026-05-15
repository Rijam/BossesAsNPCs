#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs
{
	[ReinitializeDuringResizeArrays]
	public class TownNPCLiveInBadBiomeSets
	{
		public static bool[] CanLiveInCorruption = NPCID.Sets.Factory.CreateNamedSet("CanLiveInCorruption")
			.Description("Town NPCs in this set can be housed in the Corruption.").RegisterBoolSet(false);

		public static bool[] CanLiveInCrimson = NPCID.Sets.Factory.CreateNamedSet("CanLiveInCrimson")
			.Description("Town NPCs in this set can be housed in the Crimson.").RegisterBoolSet(false);

		public static bool[] CanLiveInDungeon = NPCID.Sets.Factory.CreateNamedSet("CanLiveInDungeon")
			.Description("Town NPCs in this set can be housed in the Dungeon.").RegisterBoolSet(false);
	}

	public class BossesAsNPCsLiveInBadBiomeEdits : ModSystem
	{
		// Concepts adapted from Extra Townsfolk (MoreTownsfolk) by Lurrae & Skulgan

		internal static bool HousingCorruptionNPC = false;
		internal static bool HousingCrimsonNPC = false;
		// internal static bool HousingDungeonNPC = false;
		// internal static bool PlayerTalkingToCorruptionNPC = false;
		// internal static bool PlayerTalkingToCrimsonNPC = false;
		// internal static bool PlayerTalkingToDungeonNPC = false;

		public override void Load()
		{
			// Load detours
			Terraria.On_WorldGen.ScoreRoom += Detour_WorldGen_ScoreRoom;
			Terraria.On_WorldGen.GetTileTypeCountByCategory += Detour_WorldGen_GetTileTypeCountByCategory;
			// Terraria.GameContent.On_ShopHelper.ProcessMood += Detour_ShopHelper_ProcessMood;
			Terraria.GameContent.On_ShopHelper.IsPlayerInEvilBiomes += Detour_ShopHelper_IsPlayerInEvilBiomes;
			Terraria.GameContent.Personalities.On_DungeonBiome.IsInBiome += Detour_DungeonBiome_IsInBiome;
		}

		public override void Unload()
		{
			HousingCorruptionNPC = false;
			HousingCrimsonNPC = false;
		}

		/// <summary>
		/// Normally this is true if player.ZoneDungeon is true. However, player.ZoneDungeon requires the player to be standing in front of dungeon walls.
		/// This will let the NPC consider themselves in the Dungeon even if the player isn't standing in front of dungeon walls.
		/// </summary>
		/// <param name="orig"></param>
		/// <param name="self"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		private bool Detour_DungeonBiome_IsInBiome(On_DungeonBiome.orig_IsInBiome orig, DungeonBiome self, Player player)
		{
			// Same check for player.ZoneDungeon except without the check for standing in front of dungeon walls.
			// 250 is a magic number. There is no SceneMetrics.DungeonTileThreshold
			if (Main.SceneMetrics.DungeonTileCount >= 250 && player.Center.Y > Main.worldSurface * 16.0)
			{
				return true;
			}
			return orig(self, player);
		}

		/// <summary>
		/// Used by the happiness system to add the Hate Biome message if the NPC is in the Corruption, Crimson, or Dungeon.
		/// This will bypass that if the player is talking to a NPC who can live in one of those biomes.
		/// </summary>
		/// <param name="orig"></param>
		/// <param name="self"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		private bool Detour_ShopHelper_IsPlayerInEvilBiomes(On_ShopHelper.orig_IsPlayerInEvilBiomes orig, ShopHelper self, Player player)
		{
			try
			{
				// Get the current NPC being talked to.
				// Reflection is needed because it is private.
				// FieldInfo Field_ShopHelper_currentNPCBeingTalkedTo = self.GetType().GetField("_currentNPCBeingTalkedTo", BindingFlags.NonPublic | BindingFlags.Instance);
				// NPC currentNPC = (NPC)Field_ShopHelper_currentNPCBeingTalkedTo.GetValue(self);
				NPC currentNPC = Get_ShopHelper_currentNPCBeingTalkedTo(self);

				// If the player is talking to an NPC that could live in the bad biomes.
				// if (PlayerTalkingToCorruptionNPC || PlayerTalkingToCrimsonNPC || PlayerTalkingToDungeonNPC)
				if (TownNPCLiveInBadBiomeSets.CanLiveInCorruption[currentNPC.type] || TownNPCLiveInBadBiomeSets.CanLiveInCrimson[currentNPC.type] || TownNPCLiveInBadBiomeSets.CanLiveInDungeon[currentNPC.type])
				{
					IShoppingBiome[] dangerousBiomes = Get_ShopHelper_dangerousBiomes(self); // Get the list of dangerous biomes from vanilla.
					List<IShoppingBiome> dangerousBiomesList = dangerousBiomes.ToList(); // Convert to list.
					// Keep track of the biomes.
					IShoppingBiome? corruption = null;
					IShoppingBiome? crimson = null;
					IShoppingBiome? dungeon = null;

					// For each biome in the list...
					foreach (IShoppingBiome biome in dangerousBiomesList)
					{
						// We don't want the NPC go hate living in the biome that they are supposed to able to live in.
						// If the NPC can live in the biome and that biome is found, keep track of it.
						if (TownNPCLiveInBadBiomeSets.CanLiveInCorruption[currentNPC.type] && biome is CorruptionBiome)
						{
							corruption = biome;
						}
						if (TownNPCLiveInBadBiomeSets.CanLiveInCrimson[currentNPC.type] && biome is CrimsonBiome)
						{
							crimson = biome;
						}
						if (TownNPCLiveInBadBiomeSets.CanLiveInDungeon[currentNPC.type] && biome is DungeonBiome)
						{
							dungeon = biome;
						}
					}
					// Remove the biomes from the list if the NPC is supposed to be able to live there.
					if (corruption != null)
					{
						dangerousBiomesList.Remove(corruption);
					}
					if (crimson != null)
					{
						dangerousBiomesList.Remove(crimson);
					}
					if (dungeon != null)
					{
						dangerousBiomesList.Remove(dungeon);
					}

					// Modified from vanilla ShopHelper.IsPlayerInEvilBiomes()
					// Go through the remaining biomes in the list.
					foreach (IShoppingBiome biome in dangerousBiomesList)
					{
						IShoppingBiome aShoppingBiome = biome;

						// If the player/NPC is considered to be in the biome,
						// Add the Hate Biome text (and consequently set the prices to the max)
						if (aShoppingBiome.IsInBiome(player))
						{
							// Invoke the method to add the happiness report.
							// Reflection is needed because the method is private.
							/*
							MethodInfo Method_ShopHelper_AddHappinessReportText = self.GetType().GetMethod("AddHappinessReportText", BindingFlags.NonPublic | BindingFlags.Instance);
							Method_ShopHelper_AddHappinessReportText.Invoke(self,
								[
									"HateBiome", new
									{
										BiomeName = ShopHelper.BiomeNameByKey(aShoppingBiome.NameKey)
									},
									default
								]
							);
							*/

							Invoke_ShopHelper_AddHappinessReportText(self,
								"HateBiome", new
								{
									BiomeName = ShopHelper.BiomeNameByKey(aShoppingBiome.NameKey)
								},
								default
							);

							return true; // They are in a bad biome that they are not supposed to be able to live in.
						}
					}
					return false; // The bad biome they are in is ok for them to live in.
				}
			}
			catch (Exception e)
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.Error($"Bosses as NPCs: On_ShopHelper_IsPlayerInEvilBiomes: Error when trying to process the NPC. {e}");
			}

			return orig(self, player); // Run original code if the NPC doesn't apply to the above.
		}

		/*
		/// <summary>
		/// ShopHelper.ProcessMood() calls ShopHelper.IsPlayerInEvilBiomes()
		/// Check if the NPC it is processing the mood for can live in a bad biome.
		/// We can do it this way if we don't want to reflect _currentNPCBeingTalkedTo in ShopHelper.IsPlayerInEvilBiomes().
		/// </summary>
		/// <param name="orig"></param>
		/// <param name="self"></param>
		/// <param name="player"></param>
		/// <param name="npc"></param
		private void Detour_ShopHelper_ProcessMood(On_ShopHelper.orig_ProcessMood orig, ShopHelper self, Player player, NPC npc)
		{
			if (TownNPCLiveInBadBiomeSets.CanLiveInCorruption[npc.type])
			{
				PlayerTalkingToCorruptionNPC = true;
			}
			if (TownNPCLiveInBadBiomeSets.CanLiveInCrimson[npc.type])
			{
				PlayerTalkingToCrimsonNPC = true;
			}
			if (TownNPCLiveInBadBiomeSets.CanLiveInDungeon[npc.type])
			{
				PlayerTalkingToDungeonNPC = true;
			}

			orig(self, player, npc);

			PlayerTalkingToCorruptionNPC = false;
			PlayerTalkingToCrimsonNPC = false;
			PlayerTalkingToDungeonNPC = false;
		}
		*/

		/// <summary>
		/// Called by WorldGen.ScoreRoom()
		/// The player can't move a Town NPC to a house if it is corrupted (Corruption or Crimson tiles in the area).
		/// This reports 0 Corruption or Crimson tiles if the Town NPC can live there.
		/// </summary>
		/// <param name="orig"></param>
		/// <param name="tileTypeCounts"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		private int Detour_WorldGen_GetTileTypeCountByCategory(On_WorldGen.orig_GetTileTypeCountByCategory orig, int[] tileTypeCounts, Terraria.Enums.TileScanGroup group)
		{
			if (HousingCorruptionNPC && group == TileScanGroup.Corruption)
			{
				return 0;
			}
			if (HousingCrimsonNPC && group == TileScanGroup.Crimson)
			{
				return 0;
			}
			// Doesn't count the Dungeon, so it's not included.

			return orig(tileTypeCounts, group);
		}

		/// <summary>
		/// WorldGen.ScoreRoom() calls WorldGen.GetTileTypeCountByCategory()
		/// This has the logic for checking for the valid home. WorldGen.GetTileTypeCountByCategory() returns the score for determining
		/// if the home is corrupted. If we are moving an NPC that can live in the Corruption or Crimson, we need to tell WorldGen.GetTileTypeCountByCategory()
		/// so it can say the score is 0 (not corrupted).
		/// </summary>
		/// <param name="orig"></param>
		/// <param name="ignoreNPC"></param>
		/// <param name="npcTypeAskingToScoreRoom"></param>
		private void Detour_WorldGen_ScoreRoom(On_WorldGen.orig_ScoreRoom orig, int ignoreNPC, int npcTypeAskingToScoreRoom, Terraria.DataStructures.IRoomCheckFeedback feedback)
		{
			if (TownNPCLiveInBadBiomeSets.CanLiveInCorruption[npcTypeAskingToScoreRoom])
			{
				HousingCorruptionNPC = true;
			}
			if (TownNPCLiveInBadBiomeSets.CanLiveInCrimson[npcTypeAskingToScoreRoom])
			{
				HousingCrimsonNPC = true;
			}
			// if (TownNPCLiveInBadBiomeSets.CanLiveInDungeon[npcTypeAskingToScoreRoom])
			// {
			//	HousingDungeonNPC = true;
			// }

			orig(ignoreNPC, npcTypeAskingToScoreRoom, feedback);

			HousingCrimsonNPC = false;
			HousingCorruptionNPC = false;
			// HousingDungeonNPC = false;
		}

		/// <summary>
		/// Get the private instanced field ShopHelper._dangerousBiomes
		/// </summary>
		private static readonly FieldInfo Field_ShopHelper_dangerousBiomes = typeof(Terraria.GameContent.ShopHelper).GetField("_dangerousBiomes", BindingFlags.NonPublic | BindingFlags.Instance)!;

		/// <summary>
		/// ShopHelper._dangerousBiomes is an array that contains the Corruption, Crimson, and Dungeon.
		/// </summary>
		/// <returns>IShoppingBiome[] Array containing the AShoppingBiome classes.</returns>
		internal static IShoppingBiome[] Get_ShopHelper_dangerousBiomes(ShopHelper instance)
		{
			return (IShoppingBiome[])Field_ShopHelper_dangerousBiomes.GetValue(instance)!;
		}

		/// <summary>
		/// Get the private instanced field ShopHelper._currentNPCBeingTalkedTo
		/// </summary>
		private static readonly FieldInfo Field_ShopHelper_currentNPCBeingTalkedTo = typeof(Terraria.GameContent.ShopHelper).GetField("_currentNPCBeingTalkedTo", BindingFlags.NonPublic | BindingFlags.Instance)!;

		/// <summary>
		/// ShopHelper._currentNPCBeingTalkedTo
		/// </summary>
		/// <param name="instance">ShopHelper instance</param>
		/// <returns>NPC _currentNPCBeingTalkedTo</returns>
		internal static NPC Get_ShopHelper_currentNPCBeingTalkedTo(ShopHelper instance)
		{
			return (NPC)Field_ShopHelper_currentNPCBeingTalkedTo.GetValue(instance)!;
		}

		/// <summary>
		/// Get the private method ShopHelper.AddHappinessReportText()
		/// </summary>
		private static readonly MethodInfo Method_ShopHelper_AddHappinessReportText = typeof(Terraria.GameContent.ShopHelper).GetMethod("AddHappinessReportText", BindingFlags.NonPublic | BindingFlags.Instance)!;

		/// <summary>
		/// Invokes the method ShopHelper.AddHappinessReportText()
		/// </summary>
		/// <param name="instance">ShopHelper instance</param>
		/// <param name="textKeyInCategory"></param>
		/// <param name="substitutes"></param>
		/// <param name="otherNPCType"></param>
		internal static void Invoke_ShopHelper_AddHappinessReportText(ShopHelper instance, string textKeyInCategory, object? substitutes = null, int otherNPCType = 0)
		{
			// MethodInfo Method_ShopHelper_AddHappinessReportText = instance.GetType().GetMethod("AddHappinessReportText", BindingFlags.NonPublic | BindingFlags.Instance);
			Method_ShopHelper_AddHappinessReportText.Invoke(instance, [textKeyInCategory, substitutes, otherNPCType]);
		}
	}
}