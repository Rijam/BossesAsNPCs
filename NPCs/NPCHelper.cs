using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossesAsNPCs.NPCs
{
	/// <summary>
	/// NPCHelper is a small class that "automates" many repeated things for the Town NPCs.
	/// </summary>
	public class NPCHelper
	{
		/// Mod name
		private static readonly string mod = ModContent.GetInstance<BossesAsNPCs>().Name;

		/// <summary>
		/// Automatically gets the localized Loved text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LoveText(string npc)
		{
			return "[c/b3f2b3:" + Language.GetTextValue("RandomWorldName_Noun.Love") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Love") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Liked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LikeText(string npc)
		{
			return "[c/ddf2b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Like") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Like") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Disliked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string DislikeText(string npc)
		{
			return "[c/f2e0b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Dislike") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Dislike") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Hated text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string HateText(string npc)
		{
			return "[c/f2b5b3:" + Language.GetTextValue("RandomWorldName_Noun.Hate") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Hate");
		}

		/// <summary>
		/// Automatically gets the path to the localized Bestiary Description.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string BestiaryPath(string npc)
		{
			return "Mods." + mod + ".Bestiary.Description." + npc;
		}

		/// <summary>
		/// Automatically gets the base path to the localized dialog. Add `+ "Key"` to get the dialog.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		public static string DialogPath(string npc)
		{
			return $"Mods.{mod}.NPCs.{npc}.NPCDialog.";
		}

		/// <summary>
		/// Automatically gets the path to the localized dialog.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="key">The dialog key.</param>
		public static string DialogPath(string npc, string key)
		{
			return $"Mods.{mod}.NPCs.{npc}.NPCDialog.{key}";
		}

		/// <summary>
		/// Gets if the player has unlocked the Otherworldly music. Doesn't actually check for the player, but the shop runs client side so it doesn't matter.
		/// </summary>
		/// <returns>bool</returns>
		public static bool UnlockOWMusic()
		{
			return Main.Configuration.Get("UnlockMusicSwap", false);
		}

		/// <summary>
		/// Returns a list of all of the Town NPCs within 25 tiles.
		/// searchMode = 1: Everything that could be a Town NPC, including Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
		/// searchMode = 2: Town NPCs and Town Pets. Excludes Old Man, Traveling Merchant, and Skeleton Merchant.
		/// searchMode = 3: Only real Town NPCs. Excludes Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
		/// npcTypeListHouse is a list of the npc.type for all of the Town NPCs within 25 tiles.
		/// npcTypeListNearBy is a list of the npc.type for all of the Town NPCs within 50 tiles.
		/// npcTypeListVillage is a list of the npc.type for all of the Town NPCs within 120 tiles.
		/// npcTypeListAll is a list of the npc.type for all of the Town NPCs in the world.
		/// Use .Count if you want the total number of Town NPCs for the given list.
		/// Adapted from vanilla
		/// </summary>
		/// <returns>List<NPC> of all the Town NPCs within 25 tiles.</returns>
		public static List<NPC> GetNearbyResidentNPCs(NPC npc, int searchMode, out List<int> npcTypeListHouse, out List<int> npcTypeListNearBy, out List<int> npcTypeListVillage, out List<int> npcTypeListAll)
		{
			List<NPC> list = [];
			npcTypeListHouse = [];
			npcTypeListNearBy = [];
			npcTypeListVillage = [];
			npcTypeListAll = [];
			Vector2 npc1Home = new(npc.homeTileX, npc.homeTileY);
			if (npc.homeless)
			{
				npc1Home = new Vector2(npc.Center.X / 16f, npc.Center.Y / 16f);
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (i == npc.whoAmI)
				{
					continue;
				}
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.townNPC && NearbyResidentSearchMode(npc, nPC, searchMode))
				{
					Vector2 npc2Home = new(nPC.homeTileX, nPC.homeTileY);
					if (nPC.homeless)
					{
						npc2Home = nPC.Center / 16f;
					}
					float distance = Vector2.Distance(npc1Home, npc2Home);
					if (distance < 25f)
					{
						list.Add(nPC);
						npcTypeListHouse.Add(nPC.type);
					}
					if (distance < 50f)
					{
						npcTypeListNearBy.Add(nPC.type);
					}
					if (distance < 120f)
					{
						npcTypeListVillage.Add(nPC.type);
					}
					npcTypeListAll.Add(nPC.type);
				}
			}
			return list;
		}

		/// <summary>
		/// Used by NPCHelper.GetNearbyResidentNPCs().
		/// Returns true or false based on the search mode. See the method for info on the searchMode.
		/// </summary>
		/// <returns>True if the NPC fits the search mode requirements</returns>
		public static bool NearbyResidentSearchMode(NPC npc1, NPC npc2, int searchMode)
		{
			switch (searchMode)
			{
				case 1: // Everything that could be a Town NPC, including Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
					if (NPCID.Sets.ActsLikeTownNPC[npc2.type] || npc2.housingCategory >= 1)
					{
						return true;
					}
					return false;
				case 2: // Town NPCs and Town Pets. Excludes Old Man, Traveling Merchant, and Skeleton Merchant.
					if (npc2.type != NPCID.OldMan || npc2.type != NPCID.TravellingMerchant || npc2.type != NPCID.SkeletonMerchant || !NPCID.Sets.ActsLikeTownNPC[npc2.type])
					{
						return true;
					}
					return false;
				case 3: // Only real Town NPCs. Excludes Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
					if (npc2.type != NPCID.OldMan || npc2.type != NPCID.TravellingMerchant || npc2.type != NPCID.SkeletonMerchant || !NPCID.Sets.ActsLikeTownNPC[npc2.type] && !WorldGen.TownManager.CanNPCsLiveWithEachOther(npc1, npc2))
					{
						return true;
					}
					return false;
				default:
					return false;
			}
		}

		/// <summary>
		/// Searches the shop (or chest) to see if an item is in it. slotNumber is the slot the item is in.
		/// </summary>
		/// <returns>True if the item is found</returns>
		public static bool FindItemInShop(int[] shop, int item, out int? slotNumber)
		{
			slotNumber = null;
			for (int i = 0; i < shop.Length; i++)
			{
				if (shop[i] == item)
				{
					slotNumber = i;
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Searches the shop (or chest) to see if an item is in it. slotNumber is the slot the item is in.
		/// </summary>
		/// <returns>True if the item is found</returns>
		public static bool FindItemInShop(Chest shop, int item, out int? slotNumber)
		{
			slotNumber = null;
			for (int i = 0; i < shop.maxItems; i++)
			{
				if (shop.item[i].type == item)
				{
					slotNumber = i;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if any boss has been defeated. (Only tracks the ones that are NPCs in this mod.)
		/// </summary>
		public static bool DownedAnyBoss()
		{
			if (NPC.downedSlimeKing || NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedQueenBee || NPC.downedBoss3 || NPC.downedDeerclops || BossesAsNPCsWorld.downedWoF
				|| NPC.downedQueenSlime || NPC.downedMechBossAny || NPC.downedPlantBoss || NPC.downedGolemBoss || NPC.downedEmpressOfLight || NPC.downedFishron
				|| BossesAsNPCsWorld.downedBetsy || NPC.downedAncientCultist || NPC.downedMoonlord || BossesAsNPCsWorld.downedDreadnautilus || BossesAsNPCsWorld.downedMothron
				|| NPC.downedHalloweenKing || NPC.downedChristmasIceQueen || NPC.downedMartians)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if any boss has been defeated. (Only tracks the ones that are NPCs in this mod.)
		/// Also checks that the configs for those bosses are disabled.
		/// </summary>
		public static bool DownedAnyBossWithConfigCheck()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			if ((NPC.downedSlimeKing && !config.CanSpawnKingSlime) ||
				(NPC.downedBoss1 && !config.CanSpawnEoC) ||
				(BossesAsNPCsWorld.downedEoW && !config.CanSpawnEoW) ||
				(BossesAsNPCsWorld.downedBoC && !config.CanSpawnBoC) ||
				(NPC.downedQueenBee && !config.CanSpawnQueenBee) ||
				(NPC.downedBoss3 && !config.CanSpawnSkeletron) ||
				(NPC.downedDeerclops && !config.CanSpawnDeerclops) ||
				(BossesAsNPCsWorld.downedWoF && !config.CanSpawnWoF) ||
				(NPC.downedQueenSlime && !config.CanSpawnQueenSlime) ||
				(NPC.downedMechBoss1 && !config.CanSpawnDestroyer) ||
				(NPC.downedMechBoss2 && !config.CanSpawnTwins) ||
				(NPC.downedMechBoss3 && !config.CanSpawnSkeletronPrime) ||
				(NPC.downedPlantBoss && !config.CanSpawnPlantera) ||
				(NPC.downedGolemBoss && !config.CanSpawnGolem) ||
				(NPC.downedEmpressOfLight && !config.CanSpawnEoL) ||
				(NPC.downedFishron && !config.CanSpawnDukeFishron) ||
				(BossesAsNPCsWorld.downedBetsy && !config.CanSpawnBetsy) ||
				(NPC.downedAncientCultist && !config.CanSpawnLunaticCultist) ||
				(NPC.downedMoonlord && !config.CanSpawnMoonLord) ||
				(BossesAsNPCsWorld.downedDreadnautilus && !config.CanSpawnDreadnautilus) ||
				(BossesAsNPCsWorld.downedMothron && !config.CanSpawnMothron) ||
				(NPC.downedHalloweenKing && !config.CanSpawnPumpking) ||
				(NPC.downedChristmasIceQueen && !config.CanSpawnIceQueen) ||
				(NPC.downedMartians && !config.CanSpawnMartianSaucer))
			{
				return true;
			}
			// Still allow the Torch God to spawn even if no other bosses were disabled.
			if (DownedAnyBoss() &&
				config.CanSpawnKingSlime &&
				config.CanSpawnEoC &&
				config.CanSpawnEoW &&
				config.CanSpawnBoC &&
				config.CanSpawnQueenBee &&
				config.CanSpawnSkeletron &&
				config.CanSpawnDeerclops &&
				config.CanSpawnWoF &&
				config.CanSpawnQueenSlime &&
				config.CanSpawnDestroyer &&
				config.CanSpawnTwins &&
				config.CanSpawnSkeletronPrime &&
				config.CanSpawnPlantera &&
				config.CanSpawnGolem &&
				config.CanSpawnEoL &&
				config.CanSpawnDukeFishron &&
				config.CanSpawnBetsy &&
				config.CanSpawnLunaticCultist &&
				config.CanSpawnMoonLord &&
				config.CanSpawnDreadnautilus &&
				config.CanSpawnMothron &&
				config.CanSpawnPumpking &&
				config.CanSpawnIceQueen &&
				config.CanSpawnMartianSaucer)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Safely returns the integer of the ModItem from the given mod.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <returns>int if found, 0 if not found.</returns>
		public static int SafelyGetCrossModItem(Mod mod, string itemString)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				return outItem.Type;
			}
			ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelyGetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			return 0;
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the value.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, /*NPCShop shop*/ string npcString, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				// shop.Add(outItem.Type, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
				List<Condition> listCondition = condition.ToList();
				listCondition.Add(ShopConditions.TownNPCsCrossModSupport);
				SetupShops.AddToCustomShops(npcString, outItem.Type, ContentSamples.ItemsByType[outItem.Type]?.value ?? 0, listCondition);
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the customPrice.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="customPrice">The custom price of the item.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, /*NPCShop shop*/ string npcString, int customPrice, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				//shop.Add(new Item(outItem.Type) { shopCustomPrice = customPrice }, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
				List<Condition> listCondition = condition.ToList();
				listCondition.Add(ShopConditions.TownNPCsCrossModSupport);
				SetupShops.AddToCustomShops(npcString, outItem.Type, customPrice, listCondition);
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's value / 5 / priceDiv.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, /*NPCShop shop*/ string npcString, float priceDiv, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				//shop.Add(new Item(outItem.Type) { shopCustomPrice = (int)Math.Round(outItem.Item.value / 5 / priceDiv) }, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
				List<Condition> listCondition = condition.ToList();
				listCondition.Add(ShopConditions.TownNPCsCrossModSupport);
				SetupShops.AddToCustomShops(npcString, outItem.Type, (int)Math.Round(ContentSamples.ItemsByType[outItem.Type]?.value ?? 0 / 5 / priceDiv), listCondition);
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's (value / priceDiv) * priceMulti.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		/// <param name="priceMulti">The price will be multiplied by this amount after the priceDiv.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, /*NPCShop shop*/ string npcString, float priceDiv, float priceMulti, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				// shop.Add(new Item(outItem.Type) { shopCustomPrice = (int)Math.Round(outItem.Item.value / priceDiv * priceMulti) }, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
				List<Condition> listCondition = condition.ToList();
				listCondition.Add(ShopConditions.TownNPCsCrossModSupport);
				SetupShops.AddToCustomShops(npcString, outItem.Type, (int)Math.Round(ContentSamples.ItemsByType[outItem.Type]?.value ?? 0 / priceDiv * priceMulti), listCondition);
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// <br>Creates a new Item object with a <code>shopCustomPrice</code> based on the values entered.</br>
		/// <br>Calculation is: <code>item.value / valueDiv / priceDiv * priceMulti / secondDiv</code></br>
		/// </summary>
		/// <param name="itemID">The Item ID of the item.</param>
		/// <param name="priceDiv">Divides the price</param>
		/// <param name="priceMulti">Multiplies the price</param>
		/// <param name="secondDiv">Divides the price again. This is after the multiplication</param>
		/// <param name="valueDiv">The value is 5 times the sell price, this defaults to 5 to divide the value by 5.</param>
		/// <returns></returns>
		public static Item ItemWithPrice(int itemID, double priceDiv = 1, double priceMulti = 1, double secondDiv = 1, int valueDiv = 5)
		{
			Item item = new(itemID); // ContentSamples.ItemsByType[itemID];
			item.shopCustomPrice = (int?)Math.Round((item.shopCustomPrice ?? item.value) / valueDiv / priceDiv * priceMulti / secondDiv);
			return item;
		}

		/// <summary>
		/// <br>Modifies an Item object with a <code>shopCustomPrice</code> based on the values entered.</br>
		/// <br>Calculation is: <code>item.value / valueDiv / priceDiv * priceMulti / secondDiv</code></br>
		/// </summary>
		/// <param name="item">An existing item object</param>
		/// <param name="priceDiv">Divides the price</param>
		/// <param name="priceMulti">Multiplies the price</param>
		/// <param name="secondDiv">Divides the price again. This is after the multiplication</param>
		/// <param name="valueDiv">The value is 5 times the sell price, this defaults to 5 to divide the value by 5.</param>
		/// <returns></returns>
		public static Item ItemWithPrice(Item item, double priceDiv = 1, double priceMulti = 1, double secondDiv = 1, int valueDiv = 5)
		{
			item.shopCustomPrice = (int?)Math.Round((item.shopCustomPrice ?? item.value) / valueDiv / priceDiv * priceMulti / secondDiv);
			return item;
		}

		/// <summary>
		/// Automatically gets the localized Loved text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string LoveText(string npc, string yourMod, string otherMod)
		{
			return "[c/b3f2b3:" + Language.GetTextValue("RandomWorldName_Noun.Love") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Love") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Liked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string LikeText(string npc, string yourMod, string otherMod)
		{
			return "[c/ddf2b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Like") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Like") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Disliked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string DislikeText(string npc, string yourMod, string otherMod)
		{
			return "[c/f2e0b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Dislike") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Dislike") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Hated text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string HateText(string npc, string yourMod, string otherMod)
		{
			return "[c/f2b5b3:" + Language.GetTextValue("RandomWorldName_Noun.Hate") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Hate");
		}

		/// <summary>
		/// Automatically gets the path to the localized Bestiary Description.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string BestiaryPath(string npc, string yourMod, string otherMod)
		{
			return "Mods." + yourMod + ".Bestiary.Description." + otherMod + "." + npc;
		}

		/// <summary>
		/// Automatically gets the base path to the localized dialog. Add `+ "Key"` to get the dialog.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string DialogPath(string npc, string yourMod, string otherMod)
		{
			return "Mods." + yourMod + ".NPCs." + otherMod + "." + npc + ".NPCDialog.";
		}

		// This is used to bypass the NPCs unloading from the All In One config.
		public static readonly bool bypassMode = false;

		/// <summary>
		/// Determines whether the NPC should unload based on the All In One config.
		/// </summary>
		/// <param name="npc">Just pass `Name`</param>
		public static bool ShouldLoad(string npc)
		{
			if (bypassMode)
			{
				return true;
			}
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			BossesAsNPCsConfigServer.AllInOneOptions mode = config.AllInOneNPCMode;
			if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Off)
			{
				return true;
			}
			if (mode == BossesAsNPCsConfigServer.AllInOneOptions.OnlyOne)
			{
				return false;
			}
			if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Mixed)
			{
				if (npc == NPCString.KingSlime && !config.CanSpawnKingSlime) return false;
				if (npc == NPCString.EyeOfCthulhu && !config.CanSpawnEoC) return false;
				if (npc == NPCString.EaterOfWorlds && !config.CanSpawnEoW) return false;
				if (npc == NPCString.BrainOfCthulhu && !config.CanSpawnBoC) return false;
				if (npc == NPCString.QueenBee && !config.CanSpawnQueenBee) return false;
				if (npc == NPCString.Skeletron && !config.CanSpawnSkeletron) return false;
				if (npc == NPCString.Deerclops && !config.CanSpawnDeerclops) return false;
				if (npc == NPCString.WallOfFlesh && !config.CanSpawnWoF) return false;
				if (npc == NPCString.QueenSlime && !config.CanSpawnQueenSlime) return false;
				if (npc == NPCString.TheDestroyer && !config.CanSpawnDestroyer) return false;
				if (npc == NPCString.Retinazer && !config.CanSpawnTwins) return false;
				if (npc == NPCString.Spazmatism && !config.CanSpawnTwins) return false;
				if (npc == NPCString.SkeletronPrime && !config.CanSpawnSkeletronPrime) return false;
				if (npc == NPCString.Plantera && !config.CanSpawnPlantera) return false;
				if (npc == NPCString.Golem && !config.CanSpawnGolem) return false;
				if (npc == NPCString.EmpressOfLight && !config.CanSpawnEoL) return false;
				if (npc == NPCString.DukeFishron && !config.CanSpawnDukeFishron) return false;
				if (npc == NPCString.Betsy && !config.CanSpawnBetsy) return false;
				if (npc == NPCString.LunaticCultist && !config.CanSpawnLunaticCultist) return false;
				if (npc == NPCString.MoonLord && !config.CanSpawnMoonLord) return false;
				if (npc == NPCString.Dreadnautilus && !config.CanSpawnDreadnautilus) return false;
				if (npc == NPCString.Mothron && !config.CanSpawnMothron) return false;
				if (npc == NPCString.Pumpking && !config.CanSpawnPumpking) return false;
				if (npc == NPCString.IceQueen && !config.CanSpawnIceQueen) return false;
				if (npc == NPCString.MartianSaucer && !config.CanSpawnMartianSaucer) return false;
			}
			return true;
		}

		/// <summary>
		/// Inherits NPCInteractions.Actions.OpenShop and changes the Condition to check for the TownNPCsCrossModSupport config.
		/// </summary>
		public class OpenShopCrossModSupport(string shopName, string customTextKey = null) : NPCInteractions.Actions.OpenShop(shopName, customTextKey)
		{
			public override bool Condition() => base.Condition() && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
		}

		/// <summary>
		/// Registers both the Shop and Shop2 at once. The custom name for shop 2 and the TownNPCsCrossModSupport condition is automatically set.
		/// </summary>
		/// <param name="shop1"></param>
		/// <param name="shop2"></param>
		public static void RegisterShop1AndShop2(NPCInteractionList interactions, string shop1, string shop2)
		{
			NPCInteractionList.Entry shop1Entry = interactions.Prepend(NPCInteractions.Shop(shop1));
			interactions.InsertAfter(new OpenShopCrossModSupport(shop2, Language.GetTextValue("Mods.BossesAsNPCs.UI.Shop2")), shop1Entry);
		}

		/// <summary>
		/// Which page are we looking at in the Torch God's menu.
		/// </summary>
		internal static int ShopPage { get; set; } = 1;
		/// <summary>
		/// Three total pages. Pre-HM, HM, and Event
		/// </summary>
		internal static int TotalShopPage = 3;

		// This button transform into "No Shop Selected" if there are no shops.
		public class TorchGodNextPage : NPCInteraction
		{
			private static BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			private static BossesAsNPCsConfigServer.AllInOneOptions mode = config.AllInOneNPCMode;

			public override string GetText()
			{
				string nextShop = Language.GetTextValue("Mods.BossesAsNPCs.UI.TorchGod.NextPage");
				string noShop = Language.GetTextValue("Mods.BossesAsNPCs.UI.TorchGod.NoShop");
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Off)
				{
					return nextShop;
				}
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Mixed)
				{
					if (HowManyShopsForMode1() > 0)
					{
						return nextShop;
					}
				}
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.OnlyOne)
				{
					if (HowManyShopsForMode2() > 0)
					{
						return nextShop;
					}
				}
				return noShop;
			}
			public override bool Condition()
			{
				return TalkNPCType == ModContent.NPCType<TownNPCs.TorchGod>(); // Not necessary. Could just be true.
			}
			public override void Interact()
			{
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Mixed)
				{
					if (HowManyShopsForMode1() <= 0)
					{
						Main.npcChatText = Language.GetTextValue(NPCHelper.DialogPath("TorchGod", "NoShop"));
						Main.DoNPCPortraitHop();
						return;
					}
				}
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.OnlyOne)
				{
					if (HowManyShopsForMode2() <= 0)
					{
						Main.npcChatText = Language.GetTextValue(NPCHelper.DialogPath("TorchGod", "NoShop"));
						Main.DoNPCPortraitHop();
						return;
					}
				}
				ShopPage++;
				if (ShopPage > TotalShopPage)
				{
					ShopPage = 1;
				}
				// Main.NewText($"ShopPage {ShopPage}");
			}
		}

		// This button will disappear is there are no shops.
		public class TorchGodPreviousPage : NPCInteraction
		{
			private static BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			private static BossesAsNPCsConfigServer.AllInOneOptions mode = config.AllInOneNPCMode;

			public override string GetText()
			{
				string prevShop = Language.GetTextValue("Mods.BossesAsNPCs.UI.TorchGod.PreviousPage");
				string noShop = Language.GetTextValue("Mods.BossesAsNPCs.UI.TorchGod.NoShop");
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Off)
				{
					return prevShop;
				}
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Mixed)
				{
					if (HowManyShopsForMode1() > 0)
					{
						return prevShop;
					}
				}
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.OnlyOne)
				{
					if (HowManyShopsForMode2() > 0)
					{
						return prevShop;
					}
				}
				return noShop;
			}
			public override bool Condition()
			{
				bool torchGod = TalkNPCType == ModContent.NPCType<TownNPCs.TorchGod>(); // Not necessary. Could just be true.
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.Mixed)
				{
					if (HowManyShopsForMode1() <= 0)
					{
						return false;
					}
				}
				if (mode == BossesAsNPCsConfigServer.AllInOneOptions.OnlyOne)
				{
					if (HowManyShopsForMode2() <= 0)
					{
						return false;
					}
				}
				return torchGod;
			}
			public override void Interact()
			{
				ShopPage--;
				if (ShopPage < 1)
				{
					ShopPage = TotalShopPage;
				}
				// Main.NewText($"ShopPage {ShopPage}");
			}
		}


		public class TorchGodOpenShop1Mode0(string shopName, int pageNumber, string customTextKey = null) : NPCInteractions.Actions.OpenShop(shopName, customTextKey)
		{
			public override bool Condition()
			{
				BossesAsNPCsConfigServer.AllInOneOptions mode = ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode;
				return base.Condition() && mode == BossesAsNPCsConfigServer.AllInOneOptions.Off && ShopPage == pageNumber;
			}
		}

		public class TorchGodOpenShop2Mode0(string shopName, int pageNumber, string customTextKey = null) : TorchGodOpenShop1Mode0(shopName, pageNumber, customTextKey)
		{
			public override bool Condition() => base.Condition() && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
		}

		public static void TorchGodRegisterShop1AndShop2Mode0(NPCInteractionList interactions, string shopFromWho, int pageNumber, string button)
		{
			interactions.Append(new TorchGodOpenShop1Mode0($"BossesAsNPCs/{shopFromWho}/Shop1", pageNumber, button));
			interactions.Append(new TorchGodOpenShop1Mode0($"BossesAsNPCs/{shopFromWho}/Shop2", pageNumber, $"{button} 2"));
		}

		/// <summary>
		/// Registers all of Torch God's shops when AllInOneNPCMode == Off
		/// </summary>
		/// <param name="interactions"></param>
		public static void TorchGodRegisterShopsMode0(NPCInteractionList interactions)
		{
			TorchGodRegisterShop1AndShop2Mode0(interactions, "KingSlime", 1, Language.GetTextValue("NPCName.KingSlime"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "EyeOfCthulhu", 1, Language.GetTextValue("NPCName.EyeofCthulhu"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "EaterOfWorlds", 1, Language.GetTextValue("NPCName.EaterofWorldsHead"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "BrainOfCthulhu", 1, Language.GetTextValue("NPCName.BrainofCthulhu"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "QueenBee", 1, Language.GetTextValue("NPCName.QueenBee"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Skeletron", 1, Language.GetTextValue("NPCName.SkeletronHead"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Deerclops", 1, Language.GetTextValue("NPCName.Deerclops"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "WallOfFlesh", 1, Language.GetTextValue("NPCName.WallofFlesh"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "QueenSlime", 2, Language.GetTextValue("NPCName.QueenSlimeBoss"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "TheDestroyer", 2, Language.GetTextValue("NPCName.TheDestroyer"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Retinazer", 2, Language.GetTextValue("NPCName.Retinazer"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Spazmatism", 2, Language.GetTextValue("NPCName.Spazmatism"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "SkeletronPrime", 2, Language.GetTextValue("NPCName.SkeletronPrime"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Plantera", 2, Language.GetTextValue("NPCName.Plantera"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Golem", 2, Language.GetTextValue("NPCName.Golem"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "EmpressOfLight", 2, Language.GetTextValue("NPCName.HallowBoss"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "DukeFishron", 2, Language.GetTextValue("NPCName.DukeFishron"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Betsy", 2, Language.GetTextValue("NPCName.DD2Betsy"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "LunaticCultist", 2, Language.GetTextValue("NPCName.CultistBoss"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "MoonLord", 2, Language.GetTextValue("NPCName.MoonLordHead"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Dreadnautilus", 3, Language.GetTextValue("NPCName.BloodNautilus"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Mothron", 3, Language.GetTextValue("NPCName.Mothron"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "Pumpking", 3, Language.GetTextValue("NPCName.Pumpking"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "IceQueen", 3, Language.GetTextValue("NPCName.IceQueen"));
			TorchGodRegisterShop1AndShop2Mode0(interactions, "MartianSaucer", 3, Language.GetTextValue("NPCName.MartianSaucer"));
		}

		public class TorchGodOpenShop1Mode1(string shopName, int pageNumber, Func<bool> bossCondition, string customTextKey = null) : NPCInteractions.Actions.OpenShop(shopName, customTextKey)
		{
			public override bool Condition()
			{
				BossesAsNPCsConfigServer.AllInOneOptions mode = ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode;
				return base.Condition() && mode == BossesAsNPCsConfigServer.AllInOneOptions.Mixed && ShopPage == pageNumber && bossCondition.Invoke();
			}
		}

		public class TorchGodOpenShop2Mode1(string shopName, int pageNumber, Func<bool> bossCondition, string customTextKey = null) : TorchGodOpenShop1Mode1(shopName, pageNumber, bossCondition, customTextKey)
		{
			public override bool Condition() => base.Condition() && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
		}

		public static void TorchGodRegisterShop1AndShop2Mode1(NPCInteractionList interactions, string shopFromWho, int pageNumber, string button, Func<bool> bossCondition)
		{
			interactions.Append(new TorchGodOpenShop1Mode1($"BossesAsNPCs/{shopFromWho}/Shop1", pageNumber, bossCondition, button));
			interactions.Append(new TorchGodOpenShop1Mode1($"BossesAsNPCs/{shopFromWho}/Shop2", pageNumber, bossCondition, $"{button} 2"));
		}

		/// <summary>
		/// Registers all of Torch God's shops when AllInOneNPCMode == Mixed
		/// </summary>
		/// <param name="interactions"></param>
		public static void TorchGodRegisterShopsMode1(NPCInteractionList interactions)
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			TorchGodRegisterShop1AndShop2Mode1(interactions, "KingSlime", 1, Language.GetTextValue("NPCName.KingSlime"), () => !config.CanSpawnKingSlime && NPC.downedSlimeKing);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "EyeOfCthulhu", 1, Language.GetTextValue("NPCName.EyeofCthulhu"), () => !config.CanSpawnEoC && NPC.downedBoss1);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "EaterOfWorlds", 1, Language.GetTextValue("NPCName.EaterofWorldsHead"), () => !config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "BrainOfCthulhu", 1, Language.GetTextValue("NPCName.BrainofCthulhu"), () => !config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "QueenBee", 1, Language.GetTextValue("NPCName.QueenBee"), () => !config.CanSpawnQueenBee && NPC.downedQueenBee);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Skeletron", 1, Language.GetTextValue("NPCName.SkeletronHead"), () => !config.CanSpawnSkeletron && NPC.downedBoss3);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Deerclops", 1, Language.GetTextValue("NPCName.Deerclops"), () => !config.CanSpawnDeerclops && NPC.downedDeerclops);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "WallOfFlesh", 1, Language.GetTextValue("NPCName.WallofFlesh"), () => !config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "QueenSlime", 2, Language.GetTextValue("NPCName.QueenSlimeBoss"), () => !config.CanSpawnQueenSlime && NPC.downedQueenSlime);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "TheDestroyer", 2, Language.GetTextValue("NPCName.TheDestroyer"), () => !config.CanSpawnDestroyer && NPC.downedMechBoss1);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Retinazer", 2, Language.GetTextValue("NPCName.Retinazer"), () => !config.CanSpawnTwins && NPC.downedMechBoss2);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Spazmatism", 2, Language.GetTextValue("NPCName.Spazmatism"), () => !config.CanSpawnTwins && NPC.downedMechBoss2);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "SkeletronPrime", 2, Language.GetTextValue("NPCName.SkeletronPrime"), () => !config.CanSpawnSkeletronPrime && NPC.downedMechBoss3);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Plantera", 2, Language.GetTextValue("NPCName.Plantera"), () => !config.CanSpawnPlantera && NPC.downedPlantBoss);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Golem", 2, Language.GetTextValue("NPCName.Golem"), () => !config.CanSpawnGolem && NPC.downedGolemBoss);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "EmpressOfLight", 2, Language.GetTextValue("NPCName.HallowBoss"), () => !config.CanSpawnEoL && NPC.downedEmpressOfLight);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "DukeFishron", 2, Language.GetTextValue("NPCName.DukeFishron"), () => !config.CanSpawnDukeFishron && NPC.downedFishron);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Betsy", 2, Language.GetTextValue("NPCName.DD2Betsy"), () => !config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "LunaticCultist", 2, Language.GetTextValue("NPCName.CultistBoss"), () => !config.CanSpawnLunaticCultist && NPC.downedAncientCultist);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "MoonLord", 2, Language.GetTextValue("NPCName.MoonLordHead"), () => !config.CanSpawnMoonLord && NPC.downedMoonlord);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Dreadnautilus", 3, Language.GetTextValue("NPCName.BloodNautilus"), () => !config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Mothron", 3, Language.GetTextValue("NPCName.Mothron"), () => !config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "Pumpking", 3, Language.GetTextValue("NPCName.Pumpking"), () => !config.CanSpawnPumpking && NPC.downedHalloweenKing);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "IceQueen", 3, Language.GetTextValue("NPCName.IceQueen"), () => !config.CanSpawnIceQueen && NPC.downedChristmasIceQueen);
			TorchGodRegisterShop1AndShop2Mode1(interactions, "MartianSaucer", 3, Language.GetTextValue("NPCName.MartianSaucer"), () => !config.CanSpawnMartianSaucer && NPC.downedMartians);
		}

		public class TorchGodOpenShop1Mode2(string shopName, int pageNumber, Func<bool> bossCondition, string customTextKey = null) : NPCInteractions.Actions.OpenShop(shopName, customTextKey)
		{
			public override bool Condition()
			{
				BossesAsNPCsConfigServer.AllInOneOptions mode = ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode;
				return base.Condition() && mode == BossesAsNPCsConfigServer.AllInOneOptions.OnlyOne && ShopPage == pageNumber && bossCondition.Invoke();
			}
		}

		public class TorchGodOpenShop2Mode2(string shopName, int pageNumber, Func<bool> bossCondition, string customTextKey = null) : TorchGodOpenShop1Mode2(shopName, pageNumber, bossCondition, customTextKey)
		{
			public override bool Condition() => base.Condition() && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
		}

		public static void TorchGodRegisterShop1AndShop2Mode2(NPCInteractionList interactions, string shopFromWho, int pageNumber, string button, Func<bool> bossCondition)
		{
			interactions.Append(new TorchGodOpenShop1Mode2($"BossesAsNPCs/{shopFromWho}/Shop1", pageNumber, bossCondition, button));
			interactions.Append(new TorchGodOpenShop1Mode2($"BossesAsNPCs/{shopFromWho}/Shop2", pageNumber, bossCondition, $"{button} 2"));
		}

		/// <summary>
		/// Registers all of Torch God's shops when AllInOneNPCMode == Only One
		/// </summary>
		/// <param name="interactions"></param>
		public static void TorchGodRegisterShopsMode2(NPCInteractionList interactions)
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			TorchGodRegisterShop1AndShop2Mode2(interactions, "KingSlime", 1, Language.GetTextValue("NPCName.KingSlime"), () => config.CanSpawnKingSlime && NPC.downedSlimeKing);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "EyeOfCthulhu", 1, Language.GetTextValue("NPCName.EyeofCthulhu"), () => config.CanSpawnEoC && NPC.downedBoss1);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "EaterOfWorlds", 1, Language.GetTextValue("NPCName.EaterofWorldsHead"), () => config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "BrainOfCthulhu", 1, Language.GetTextValue("NPCName.BrainofCthulhu"), () => config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "QueenBee", 1, Language.GetTextValue("NPCName.QueenBee"), () => config.CanSpawnQueenBee && NPC.downedQueenBee);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Skeletron", 1, Language.GetTextValue("NPCName.SkeletronHead"), () => config.CanSpawnSkeletron && NPC.downedBoss3);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Deerclops", 1, Language.GetTextValue("NPCName.Deerclops"), () => config.CanSpawnDeerclops && NPC.downedDeerclops);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "WallOfFlesh", 1, Language.GetTextValue("NPCName.WallofFlesh"), () => config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "QueenSlime", 2, Language.GetTextValue("NPCName.QueenSlimeBoss"), () => config.CanSpawnQueenSlime && NPC.downedQueenSlime);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "TheDestroyer", 2, Language.GetTextValue("NPCName.TheDestroyer"), () => config.CanSpawnDestroyer && NPC.downedMechBoss1);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Retinazer", 2, Language.GetTextValue("NPCName.Retinazer"), () => config.CanSpawnTwins && NPC.downedMechBoss2);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Spazmatism", 2, Language.GetTextValue("NPCName.Spazmatism"), () => config.CanSpawnTwins && NPC.downedMechBoss2);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "SkeletronPrime", 2, Language.GetTextValue("NPCName.SkeletronPrime"), () => config.CanSpawnSkeletronPrime && NPC.downedMechBoss3);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Plantera", 2, Language.GetTextValue("NPCName.Plantera"), () => config.CanSpawnPlantera && NPC.downedPlantBoss);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Golem", 2, Language.GetTextValue("NPCName.Golem"), () => config.CanSpawnGolem && NPC.downedGolemBoss);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "EmpressOfLight", 2, Language.GetTextValue("NPCName.HallowBoss"), () => config.CanSpawnEoL && NPC.downedEmpressOfLight);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "DukeFishron", 2, Language.GetTextValue("NPCName.DukeFishron"), () => config.CanSpawnDukeFishron && NPC.downedFishron);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Betsy", 2, Language.GetTextValue("NPCName.DD2Betsy"), () => config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "LunaticCultist", 2, Language.GetTextValue("NPCName.CultistBoss"), () => config.CanSpawnLunaticCultist && NPC.downedAncientCultist);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "MoonLord", 2, Language.GetTextValue("NPCName.MoonLordHead"), () => config.CanSpawnMoonLord && NPC.downedMoonlord);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Dreadnautilus", 3, Language.GetTextValue("NPCName.BloodNautilus"), () => config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Mothron", 3, Language.GetTextValue("NPCName.Mothron"), () => config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "Pumpking", 3, Language.GetTextValue("NPCName.Pumpking"), () => config.CanSpawnPumpking && NPC.downedHalloweenKing);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "IceQueen", 3, Language.GetTextValue("NPCName.IceQueen"), () => config.CanSpawnIceQueen && NPC.downedChristmasIceQueen);
			TorchGodRegisterShop1AndShop2Mode2(interactions, "MartianSaucer", 3, Language.GetTextValue("NPCName.MartianSaucer"), () => config.CanSpawnMartianSaucer && NPC.downedMartians);
		}

		public static int HowManyShopsForMode1()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();

			// Bools true if the config is *off* and the boss has been defeated.
			bool KS = !config.CanSpawnKingSlime && NPC.downedSlimeKing;
			bool EoC = !config.CanSpawnEoC && NPC.downedBoss1;
			bool EoW = !config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW;
			bool BoC = !config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC;
			bool QB = !config.CanSpawnQueenBee && NPC.downedQueenBee;
			bool Sk = !config.CanSpawnSkeletron && NPC.downedBoss3;
			bool Dc = !config.CanSpawnDeerclops && NPC.downedDeerclops;
			bool WoF = !config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF;
			bool QS = !config.CanSpawnQueenSlime && NPC.downedQueenSlime;
			bool De = !config.CanSpawnDestroyer && NPC.downedMechBoss1;
			bool Tw = !config.CanSpawnTwins && NPC.downedMechBoss2;
			bool SP = !config.CanSpawnSkeletronPrime && NPC.downedMechBoss3;
			bool Pl = !config.CanSpawnPlantera && NPC.downedPlantBoss;
			bool Go = !config.CanSpawnGolem && NPC.downedGolemBoss;
			bool EoL = !config.CanSpawnEoL && NPC.downedEmpressOfLight;
			bool DF = !config.CanSpawnDukeFishron && NPC.downedFishron;
			bool Be = !config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy;
			bool LC = !config.CanSpawnLunaticCultist && NPC.downedAncientCultist;
			bool ML = !config.CanSpawnMoonLord && NPC.downedMoonlord;
			bool Dn = !config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus;
			bool Mo = !config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron;
			bool Pk = !config.CanSpawnPumpking && NPC.downedHalloweenKing;
			bool IQ = !config.CanSpawnIceQueen && NPC.downedChristmasIceQueen;
			bool MS = !config.CanSpawnMartianSaucer && NPC.downedMartians;

			int numOfShops = (KS.ToInt() + EoC.ToInt() + EoW.ToInt() + BoC.ToInt() + QB.ToInt() + Sk.ToInt() + Dc.ToInt() + WoF.ToInt()
				+ QS.ToInt() + De.ToInt() + (Tw.ToInt() * 2) + SP.ToInt() + Pl.ToInt() + Go.ToInt() + EoL.ToInt() + DF.ToInt() + Be.ToInt()
				+ LC.ToInt() + ML.ToInt() + Dn.ToInt() + Mo.ToInt() + Pk.ToInt() + IQ.ToInt() + MS.ToInt()) * 2;

			// Main.NewText($"HowManyShopsForMode1 {numOfShops}");

			return numOfShops;
		}

		public static int HowManyShopsForMode2()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();

			bool KS = config.CanSpawnKingSlime && NPC.downedSlimeKing;
			bool EoC = config.CanSpawnEoC && NPC.downedBoss1;
			bool EoW = config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW;
			bool BoC = config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC;
			bool QB = config.CanSpawnQueenBee && NPC.downedQueenBee;
			bool Sk = config.CanSpawnSkeletron && NPC.downedBoss3;
			bool Dc = config.CanSpawnDeerclops && NPC.downedDeerclops;
			bool WoF = config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF;
			bool QS = config.CanSpawnQueenSlime && NPC.downedQueenSlime;
			bool De = config.CanSpawnDestroyer && NPC.downedMechBoss1;
			bool Tw = config.CanSpawnTwins && NPC.downedMechBoss2;
			bool SP = config.CanSpawnSkeletronPrime && NPC.downedMechBoss3;
			bool Pl = config.CanSpawnPlantera && NPC.downedPlantBoss;
			bool Go = config.CanSpawnGolem && NPC.downedGolemBoss;
			bool EoL = config.CanSpawnEoL && NPC.downedEmpressOfLight;
			bool DF = config.CanSpawnDukeFishron && NPC.downedFishron;
			bool Be = config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy;
			bool LC = config.CanSpawnLunaticCultist && NPC.downedAncientCultist;
			bool ML = config.CanSpawnMoonLord && NPC.downedMoonlord;
			bool Dn = config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus;
			bool Mo = config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron;
			bool Pk = config.CanSpawnPumpking && NPC.downedHalloweenKing;
			bool IQ = config.CanSpawnIceQueen && NPC.downedChristmasIceQueen;
			bool MS = config.CanSpawnMartianSaucer && NPC.downedMartians;

			int numOfShops = (KS.ToInt() + EoC.ToInt() + EoW.ToInt() + BoC.ToInt() + QB.ToInt() + Sk.ToInt() + Dc.ToInt() + WoF.ToInt()
				+ QS.ToInt() + De.ToInt() + (Tw.ToInt() * 2) + SP.ToInt() + Pl.ToInt() + Go.ToInt() + EoL.ToInt() + DF.ToInt() + Be.ToInt()
				+ LC.ToInt() + ML.ToInt() + Dn.ToInt() + Mo.ToInt() + Pk.ToInt() + IQ.ToInt() + MS.ToInt()) * 2;

			// Main.NewText($"HowManyShopsForMode2 {numOfShops}");

			return numOfShops;
		}

		public static bool PartyPortraitCondition()
		{
			int talkNPC = Main.LocalPlayer.talkNPC;
			if (talkNPC < 0 || talkNPC >= Main.maxNPCs)
				return false;

			return Main.npc[talkNPC].altTexture == 1;
		}

		public static Vector2 DrawingOffsets(NPC npc)
		{
			// Move the position up by 4 pixels plus the gfxOffY value (that is for climbing half blocks).
			// Main.NPCAddHeight() makes it so if the Town NPC is sitting, it also moves the glow mask up by 4 more pixels.
			Vector2 verticalOffset = new(0, -4 + npc.gfxOffY + Main.NPCAddHeight(npc));

			// If the NPC is actually a dummy for the Profile and Retro portrait:
			if (npc.IsAPortraitDummy)
			{
				// The Profile dummy will have a scale of 3f. The Retro portrait will have a scale of 2f.
				verticalOffset.Y += npc.scale == 2f ? -28 : -56; // Move our drawing up.

				// The offsets from NPCID.Sets.NPCPortraitsCloseUpOffsets and NPCID.Sets.NPCPortraitsFullBodyRetroOffsets are already taken into account.

				// A similar thing can be done with NPC.IsABestiaryIconDummy if the image in the bestiary doesn't line up.
			}
			return verticalOffset;
		}

		public static Color GlowColor(NPC npc, Color color, bool shimmerUsesDiscoColor = true, bool portraitUsesDiscoColor = false)
		{
			if (!npc.IsAPortraitDummy && npc.IsShimmerVariant && shimmerUsesDiscoColor)
			{
				color = Main.DiscoColor;
			}
			if (npc.IsAPortraitDummy && npc.IsShimmerVariant && portraitUsesDiscoColor)
			{
				color = Main.DiscoColor;
			}
			return npc.GetShimmerColor(color);
		}
		public static Color GlowColor(NPC npc, byte r, byte g, byte b, byte a, bool shimmerUsesDiscoColor = true, bool portraitUsesDiscoColor = false)
		{
			return GlowColor(npc, new Color(r, g, b, a), shimmerUsesDiscoColor, portraitUsesDiscoColor);
		}
	}
	public static class ShopConditions
	{
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible

		/// <summary> Use ShopConditions.Expert instead </summary>
		internal static Condition SellExpertMode =			new("Mods.BossesAsNPCs.Conditions.SellExpertMode",			() => ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode);
		/// <summary> Use ShopConditions.Master instead </summary>
		internal static Condition SellMasterMode =			new("Mods.BossesAsNPCs.Conditions.SellMasterMode",			() => ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode);
		public static Condition SellExtraItems =			new("Mods.BossesAsNPCs.Conditions.SellExtraItems",			() => ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems);
		public static Condition TownNPCsCrossModSupport =	new("Mods.BossesAsNPCs.Conditions.TownNPCsCrossModSupport", () => ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport);
		public static Condition GoblinSellInvasionItems =	new("Mods.BossesAsNPCs.Conditions.GoblinSellInvasionItems", () => ModContent.GetInstance<BossesAsNPCsConfigServer>().GoblinSellInvasionItems);
		public static Condition PirateSellInvasionItems =	new("Mods.BossesAsNPCs.Conditions.PirateSellInvasionItems", () => ModContent.GetInstance<BossesAsNPCsConfigServer>().PirateSellInvasionItems);

		public static Condition IsNotNpcShimmered =		new("Mods.BossesAsNPCs.Conditions.IsNotNpcShimmered",	() => !Condition.IsNpcShimmered.IsMet());
		public static Condition Expert =				new("Mods.BossesAsNPCs.Conditions.Expert",				() => Condition.InExpertMode.IsMet() || SellExpertMode.IsMet());
		public static Condition Master =				new("Mods.BossesAsNPCs.Conditions.Master",				() => Condition.InMasterMode.IsMet() || SellMasterMode.IsMet());
		public static Condition Legendary =				new("Mods.BossesAsNPCs.Conditions.Legendary",			() => Master.IsMet() && (Condition.ForTheWorthyWorld.IsMet() || Condition.ZenithWorld.IsMet()));
		
		public static Condition NoAltars =				new("Mods.BossesAsNPCs.Conditions.NoAltars",			() => WorldGen.Skyblock.noAltars);

		public static Condition DaytimeEoLDefated =		new("Mods.BossesAsNPCs.Conditions.DaytimeEoLDefated",		() => BossesAsNPCsWorld.daytimeEoLDefeated);
		public static Condition DownedBetsy =			new("Mods.BossesAsNPCs.Conditions.DownedBetsy",				() => BossesAsNPCsWorld.downedBetsy);
		public static Condition DownedDungeonGuardian = new("Mods.BossesAsNPCs.Conditions.DownedDungeonGuardian",	() => BossesAsNPCsWorld.downedDungeonGuardian);
		public static Condition DownedDarkMage =		new("Mods.BossesAsNPCs.Conditions.DownedDarkMage",			() => BossesAsNPCsWorld.downedDarkMage);
		public static Condition DownedOgre =			new("Mods.BossesAsNPCs.Conditions.DownedOgre",				() => BossesAsNPCsWorld.downedOgre);
		public static Condition DownedGoblinWarlock =	new("Mods.BossesAsNPCs.Conditions.DownedGoblinWarlock",		() => BossesAsNPCsWorld.downedGoblinSummoner);
		public static Condition DownedGoblinSummoner = DownedGoblinWarlock;
		public static Condition DownedMothron =			new("Mods.BossesAsNPCs.Conditions.DownedMothron",			() => BossesAsNPCsWorld.downedMothron);
		public static Condition DownedDreadnautilus =	new("Mods.BossesAsNPCs.Conditions.DownedDreadnautilus",		() => BossesAsNPCsWorld.downedDreadnautilus);
		public static Condition DownedEaterOfWorlds =	new("Mods.BossesAsNPCs.Conditions.DownedEaterOfWorlds",		() => BossesAsNPCsWorld.downedEoW);
		public static Condition DownedBrainOfCthulhu =	new("Mods.BossesAsNPCs.Conditions.DownedBrainOfCthulhu",	() => BossesAsNPCsWorld.downedBoC);
		public static Condition DownedWallOfFlesh =		new("Mods.BossesAsNPCs.Conditions.DownedWallOfFlesh",		() => BossesAsNPCsWorld.downedWoF);
		public static Condition DownedAnyPillar =		new("Mods.BossesAsNPCs.Conditions.DownedAnyPillar",			() => NPC.downedTowerSolar || NPC.downedTowerVortex || NPC.downedTowerNebula || NPC.downedTowerStardust);
		public static Condition DownedAllPillars =		new("Mods.BossesAsNPCs.Conditions.DownedAllPillars",		() => NPC.downedTowers);
		public static Condition UnlockedBiomeTorches =	new("Mods.BossesAsNPCs.Conditions.UnlockedBiomeTorches",	() => Main.LocalPlayer.unlockedBiomeTorches);

		public static Condition RescuedWizard =						new("Mods.BossesAsNPCs.Conditions.RescuedWizard",						() => NPC.savedWizard);
		public static Condition UnlockOWMusicOrDrunkWorld =			new("Mods.BossesAsNPCs.Conditions.UnlockOWMusicOrDrunkWorld",			() => NPCHelper.UnlockOWMusic() || Condition.DrunkWorld.IsMet());
		public static Condition CorruptionOrHardmode =				new("Mods.BossesAsNPCs.Conditions.CorruptionOrHardmode",				() => Condition.CorruptWorld.IsMet() || Condition.Hardmode.IsMet());
		public static Condition CrimsonOrHardmode =					new("Mods.BossesAsNPCs.Conditions.CrimsonOrHardmode",					() => Condition.CrimsonWorld.IsMet() || Condition.Hardmode.IsMet());
		public static Condition UndergroundCavernsOrHardmode =		new("Mods.BossesAsNPCs.Conditions.UndergroundCavernsOrHardmode",		() => (Condition.InDirtLayerHeight.IsMet() || Condition.InRockLayerHeight.IsMet()) || Condition.Hardmode.IsMet());
		public static Condition HallowOrCorruptionOrCrimson =		new("Mods.BossesAsNPCs.Conditions.HallowOrCorruptionOrCrimson",			() => Condition.InHallow.IsMet() || Condition.InCorrupt.IsMet() || Condition.InCrimson.IsMet());
		public static Condition InIceAndHallowOrCorruptionOrCrimson = new("Mods.BossesAsNPCs.Conditions.InIceAndHallowOrCorruptionOrCrimson", () => (Condition.InDirtLayerHeight.IsMet() || Condition.InRockLayerHeight.IsMet()) && Condition.InSnow.IsMet() && HallowOrCorruptionOrCrimson.IsMet());

		public static string TownNPCRangeS(string range) => Language.GetTextValue("Mods.BossesAsNPCs.Conditions.TownNPCRangeS", range);
		public static string CountTownNPCsS(int number) => Language.GetTextValue("Mods.BossesAsNPCs.Conditions.CountTownNPCsS", number);

		public static Condition EternityMode(Mod passedMod) { return new("Mods.BossesAsNPCs.Conditions.EternityModeS", () => (bool)passedMod.Call("EternityMode")); }
		public static Condition WorldContagion(Mod passedMod) { return new("Mods.BossesAsNPCs.Conditions.WorldContagionS", () => (bool)passedMod.Call("Contagion")); }

#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression
	}
}