using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using BossesAsNPCs.NPCs.TownNPCs;


namespace BossesAsNPCs.NPCs
{
	public class GenerateShops
	{
		/// <summary>
		/// Generates the drops from the NPC and adds them to the shop if they meet certain criteria.
		/// </summary>
		/// <param name="enemyNPCID">The NPC ID for the NPC that the drops are from.</param>
		/// <param name="npcShop">The name of the custom shop.</param>
		/// <param name="extraConditions">Optional: Extra condition to be added to each item in the shop.</param>
		public static void GenerateDropsToAddToTheShops(int enemyNPCID, string npcShop, int townNPCID, List<Condition> extraConditions = null)
		{
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport && ModContent.GetInstance<BossesAsNPCsConfigServer>().GenerateItemsFromBestiary)
			{
				ItemDropDatabase dropsDatabase = Main.ItemDropsDB;
				GenerateDropsFromBestiary(dropsDatabase, enemyNPCID, npcShop, townNPCID, extraConditions);
				GenerateDropsFromTreasureBag();
			}
		}

		/// <summary>
		/// Finds an item from the specified NPC and searches the shops to see if it should be added.
		/// </summary>
		/// <param name="dropsDatabase">The bestiary item drop database.</param>
		/// <param name="enemyNPCID">The enemy NPC ID to search for items.</param>
		/// <param name="npcShop">The shop string for the shop to add the item to.</param>
		/// <param name="townNPCID">The town NPC ID for who the shop belongs to.</param>
		/// <param name="extraConditions">Extra conditions to add to the item in the shop.</param>
		public static void GenerateDropsFromBestiary(ItemDropDatabase dropsDatabase, int enemyNPCID, string npcShop, int townNPCID, List<Condition> extraConditions)
		{
			BossesAsNPCs.Instance.Logger.InfoFormat("Getting drops from {0} {1}, Town NPC ID {2}.", enemyNPCID, npcShop, townNPCID);
			BestiaryEntry bestiaryEntry = Main.BestiaryDB.FindEntryByNPCID(enemyNPCID);
			if (bestiaryEntry == null)
			{
				BossesAsNPCs.Instance.Logger.WarnFormat("NPC {0} had no Bestiary data", enemyNPCID);
				return;
			}

			List<DropRateInfo> list = [];

			List<IItemDropRule> rulesForNPCID = dropsDatabase.GetRulesForNPCID(enemyNPCID, includeGlobalDrops: false);
			DropRateInfoChainFeed ratesInfo = new(1f);

			foreach (IItemDropRule item in rulesForNPCID)
			{
				item.ReportDroprates(list, ratesInfo);
				// BossesAsNPCs.Instance.Logger.InfoFormat("  ReportDroprates {0}", item.ToString());
			}

			foreach (DropRateInfo item2 in list)
			{
#if DEBUG
				BossesAsNPCs.Instance.Logger.InfoFormat("  DropRateInfo {0} {1}", item2.itemId, item2.dropRate);
#endif
				// Skip Treasure Bags
				if (ItemID.Sets.BossBag[item2.itemId] || item2.itemId <= 0)
				{
#if DEBUG
					BossesAsNPCs.Instance.Logger.DebugFormat("    Skipped item {0}", item2.itemId);
#endif
					continue;
				}
				// Skip items that are already added to the shop.
				// Fewer checks if the NPC is the Goblin Tinkerer or Pirate. They only have a "Shop" shop. (Not "Shop1" and "Shop2" like the boss NPCs.)
				if (townNPCID == NPCID.GoblinTinkerer || townNPCID == NPCID.Pirate)
				{
					if (item2.itemId < ItemID.Count) // If it's a vanilla item, Check the normal shop first because it is more likely to be there.
					{
						if (DoesShopContainItem(townNPCID, item2.itemId, "Shop")
							|| DoesShopContainItem(townNPCID, item2.itemId, "ShopBossesAsNPCs")
							|| SetupShops.CustomShopContainsItem(npcShop, item2.itemId))
						{
#if DEBUG
							BossesAsNPCs.Instance.Logger.DebugFormat("    Skipped item {0}", item2.itemId);
#endif
							continue;
						}
					}
					else // Modded item. Check the custom shop list first because it is more likely to be there.
					{
						if (SetupShops.CustomShopContainsItem(npcShop, item2.itemId)
							|| DoesShopContainItem(townNPCID, item2.itemId, "Shop")
							|| DoesShopContainItem(townNPCID, item2.itemId, "ShopBossesAsNPCs"))
						{
#if DEBUG
							BossesAsNPCs.Instance.Logger.DebugFormat("    Skipped item {0}", item2.itemId);
#endif
							continue;
						}
					}
				}
				// Else not the Goblin Tinkerer or Pirate.
				else
				{
					// If it's a vanilla item, Check "Shop1" first because it is more likely to be there.
					if (item2.itemId < ItemID.Count)
					{
						if (DoesShopContainItem(townNPCID, item2.itemId, "Shop1")
							|| CheckTorchGodShops(item2.itemId, enemyNPCID)
							|| SetupShops.CustomShopContainsItem(npcShop, item2.itemId)
							|| DoesShopContainItem(townNPCID, item2.itemId, "Shop2"))
						{
#if DEBUG
							BossesAsNPCs.Instance.Logger.DebugFormat("    Skipped item {0}", item2.itemId);
#endif
							continue;
						}
					}
					else // Modded item. Check the custom shop list then "Shop2" first because it is more likely to be there.
					{
						if (SetupShops.CustomShopContainsItem(npcShop, item2.itemId)
							|| DoesShopContainItem(townNPCID, item2.itemId, "Shop2")
							|| CheckTorchGodShops(item2.itemId, enemyNPCID)
							|| DoesShopContainItem(townNPCID, item2.itemId, "Shop1"))
						{
#if DEBUG
							BossesAsNPCs.Instance.Logger.DebugFormat("    Skipped item {0}", item2.itemId);
#endif
							continue;
						}
					}
				}

				//bestiaryEntry.Info.Add(new ItemDropBestiaryInfoElement(item2));
				List<Condition> conditions = [];
				/*
				NPC newNPC = new();
				newNPC.SetDefaults(npcId);
				DropAttemptInfo info = new()
				{
					player = default,
					npc = newNPC,
					IsInSimulation = true,
					IsExpertMode = false,
					IsMasterMode = false,
					item = item2.itemId,
					rng = Main.rand
				};
				foreach (IItemDropRuleCondition itemDropRuleCondition in item2.conditions)
				{
					Condition newCondition = new(itemDropRuleCondition.GetConditionDescription() ?? "No description found", () => itemDropRuleCondition.CanDrop(info));
					conditions.Add(newCondition);
					BossesAsNPCs.Instance.Logger.InfoFormat("    Added Condition {0}", newCondition.Description);
				}*/

				Item item = ContentSamples.ItemsByType[item2.itemId];

				int customPrice = (int)Math.Round(item.value / 5 / item2.dropRate);
				if (customPrice == 0)
				{
					customPrice++;
				}
				if (item.expert || item.rare == ItemRarityID.Expert)
				{
					conditions.Add(ShopConditions.Expert);
				}
				if (item.master || item.rare == ItemRarityID.Master)
				{
					conditions.Add(ShopConditions.Master);
				}
				if (extraConditions is not null)
				{
					foreach (Condition exCond in extraConditions)
					{
						conditions.Add(exCond);
					}
				}
				SetupShops.SetShopItem(npcShop, item2.itemId, conditions, customPrice);
				BossesAsNPCs.Instance.Logger.InfoFormat("      Added Item to shop {0} {1} ({2}) from {3}", item2.itemId, item.Name, item.ModItem?.GetType(), item.ModItem?.Mod);
			}
		}

		/// <summary>
		/// Searches the shop to see if it contains the item.
		/// </summary>
		/// <param name="townNPCID">The town NPC ID of the shop.</param>
		/// <param name="itemId">The item ID that we are searching for.</param>
		/// <param name="shopName">The name of the shop.</param>
		/// <returns>True if found.</returns>
		public static bool DoesShopContainItem(int townNPCID, int itemId, string shopName = "Shop")
		{
#if DEBUG
			BossesAsNPCs.Instance.Logger.DebugFormat("      Attempting to search shop {0}", NPCShopDatabase.GetShopName(townNPCID, shopName));
#endif
			if (NPCShopDatabase.TryGetNPCShop(NPCShopDatabase.GetShopName(townNPCID, shopName), out AbstractNPCShop shop))
			{
#if DEBUG
				BossesAsNPCs.Instance.Logger.DebugFormat("      Searching Shop {0}", shop.FullName, itemId);
#endif
				foreach (AbstractNPCShop.Entry entry in shop.ActiveEntries)
				{
#if DEBUG
					BossesAsNPCs.Instance.Logger.DebugFormat("                Item ID {0} {1}", entry.Item.type, ContentSamples.ItemsByType[entry.Item.type]?.Name);
#endif
					if (entry.Item.type == itemId)
					{
#if DEBUG
						BossesAsNPCs.Instance.Logger.DebugFormat("      {0} contained {1}", shop.FullName, itemId);
#endif
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Checks the Torch God's shops for the item.
		/// </summary>
		/// <param name="itemId">The item ID to search for.</param>
		/// <param name="enemyNPCID">The enemy ID the item is from.</param>
		/// <returns>True if found.</returns>
		public static bool CheckTorchGodShops(int itemId, int enemyNPCID)
		{
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode > 0 || NPCHelper.bypassMode)
			{
				int shopNum = GetCorrespondingTorchGodShop(enemyNPCID);
				if (DoesShopContainItem(ModContent.NPCType<TorchGod>(), itemId, "TorchGodShop" + shopNum)
					|| DoesShopContainItem(ModContent.NPCType<TorchGod>(), itemId, "TorchGodShop" + (shopNum + 1)))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the Torch God shop ID based on the supplied enemy ID.
		/// </summary>
		/// <param name="enemyID">The enemy ID. Example: NPCID.KingSlime</param>
		/// <returns>ID of the shop that corresponds to that enemy.</returns>
		public static int GetCorrespondingTorchGodShop(int enemyID)
		{
			return enemyID switch
			{
				NPCID.KingSlime => 1,
				NPCID.EyeofCthulhu => 3,
				NPCID.EaterofWorldsHead => 5,
				NPCID.BrainofCthulhu => 7,
				NPCID.QueenBee => 9,
				NPCID.SkeletronHead => 11,
				NPCID.Deerclops => 13,
				NPCID.WallofFlesh => 15,
				NPCID.QueenSlimeBoss => 17,
				NPCID.TheDestroyer => 19,
				NPCID.Retinazer => 21,
				NPCID.Spazmatism => 23,
				NPCID.SkeletronPrime => 25,
				NPCID.Plantera => 27,
				NPCID.Golem => 29,
				NPCID.HallowBoss => 31,
				NPCID.DukeFishron => 33,
				NPCID.DD2Betsy => 35,
				NPCID.CultistBoss => 37,
				NPCID.MoonLordCore => 39,
				NPCID.BloodNautilus => 41,
				NPCID.Mothron => 43,
				NPCID.MourningWood => 45,
				NPCID.Pumpking => 45,
				NPCID.Everscream => 47,
				NPCID.SantaNK1 => 47,
				NPCID.IceQueen => 47,
				NPCID.MartianSaucerCore => 49,
				_ => 0,
			};
		}

		public static void GenerateDropsFromTreasureBag()
		{
			// To do
		}
	}
}