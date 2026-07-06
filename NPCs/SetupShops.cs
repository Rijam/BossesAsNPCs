using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossesAsNPCs.NPCs.TownNPCs;

namespace BossesAsNPCs.NPCs
{
	/// <summary>
	/// Returns the associated string.
	/// </summary>
	public struct NPCString
	{
		public static string KingSlime => "KingSlime";
		public static string EyeOfCthulhu => "EyeOfCthulhu";
		public static string EaterOfWorlds => "EaterOfWorlds";
		public static string BrainOfCthulhu => "BrainOfCthulhu";
		public static string QueenBee => "QueenBee";
		public static string Skeletron => "Skeletron";
		public static string Deerclops => "Deerclops";
		public static string WallOfFlesh => "WallOfFlesh";
		public static string QueenSlime => "QueenSlime";
		public static string TheDestroyer => "TheDestroyer";
		public static string Retinazer => "Retinazer";
		public static string Spazmatism => "Spazmatism";
		public static string SkeletronPrime => "SkeletronPrime";
		public static string Plantera => "Plantera";
		public static string Golem => "Golem";
		public static string EmpressOfLight => "EmpressOfLight";
		public static string DukeFishron => "DukeFishron";
		public static string Betsy => "Betsy";
		public static string LunaticCultist => "LunaticCultist";
		public static string MoonLord => "MoonLord";
		public static string Dreadnautilus => "Dreadnautilus";
		public static string Mothron => "Mothron";
		public static string Pumpking => "Pumpking";
		public static string IceQueen => "IceQueen";
		public static string MartianSaucer => "MartianSaucer";
		public static string TorchGod => "TorchGod";
		public static string GoblinTinkerer => "GoblinTinkerer";
		public static string Pirate => "Pirate";
	}

	/// <summary>
	/// Contains the predefined shops and the ability to add new items.
	/// </summary>
	public class SetupShops
	{

		// string (key) is the NPC name
		// ShopItem int is the item
		// ShopItem int is the price
		// ShopItem List<Condition> are the conditions
		private static Dictionary<string, List<ShopItem>> customShops = new()
		{
			{ NPCString.KingSlime, new List<ShopItem> { } },
			{ NPCString.EyeOfCthulhu, new List<ShopItem> { } },
			{ NPCString.EaterOfWorlds, new List<ShopItem> { } },
			{ NPCString.BrainOfCthulhu, new List<ShopItem> { } },
			{ NPCString.QueenBee, new List<ShopItem> { } },
			{ NPCString.Skeletron, new List<ShopItem> { } },
			{ NPCString.Deerclops, new List < ShopItem > { } },
			{ NPCString.WallOfFlesh, new List<ShopItem> { } },
			{ NPCString.QueenSlime, new List<ShopItem> { } },
			{ NPCString.TheDestroyer, new List<ShopItem> { } },
			{ NPCString.Retinazer, new List<ShopItem> { } },
			{ NPCString.Spazmatism,new List<ShopItem> { } },
			{ NPCString.SkeletronPrime, new List<ShopItem> { } },
			{ NPCString.Plantera, new List<ShopItem> { } },
			{ NPCString.Golem, new List<ShopItem> { } },
			{ NPCString.EmpressOfLight, new List<ShopItem> { } },
			{ NPCString.DukeFishron, new List<ShopItem> { } },
			{ NPCString.Betsy, new List<ShopItem> { } },
			{ NPCString.LunaticCultist, new List<ShopItem> { } },
			{ NPCString.MoonLord, new List<ShopItem> { } },
			{ NPCString.Dreadnautilus, new List<ShopItem> { } },
			{ NPCString.Mothron, new List<ShopItem> { } },
			{ NPCString.Pumpking, new List<ShopItem> { } },
			{ NPCString.IceQueen, new List<ShopItem> { } },
			{ NPCString.MartianSaucer, new List<ShopItem> { } },
			{ NPCString.GoblinTinkerer, new List<ShopItem> { } },
			{ NPCString.Pirate, new List<ShopItem> { } }
		};

		public static void ClearCustomShops()
		{
			customShops.Clear();
			customShops = null;
		}

		/// <summary>
		/// Adds to the dictionary.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="price">The price for the item</param>
		/// <param name="condition">The availability of the item</param>
		public static void AddToCustomShops(string npc, int item, int price, List<Condition> condition) => customShops[npc].Add(new ShopItem(item, price, condition));

		/// <summary>
		/// Attempts to do AddToCustomShops() after some checks.
		/// First it checks that item is within range of all loaded items.
		/// Second, it checks if the npc string matches one of the NPCs.
		/// Price will be based on the value of the item.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="condition">The availability of the item</param>
		/// <returns>Returns false if failed.</returns>
		public static bool SetShopItem(string npc, int item, List<Condition> condition)
		{
			if (!CheckIfValid(npc, item))
			{
				return false;
			}

			AdjustConditions(npc, ref condition);

			AddToCustomShops(npc, item, CalcItemValue(item), condition);
			return true;
		}
		/// <summary>
		/// Attempts to do AddToCustomShops() after some checks.
		/// First it checks that item is within range of all loaded items.
		/// Second, it checks if the npc string matches one of the NPCs.
		/// The price of the item will be the customPrice.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="condition">The availability of the item</param>
		/// <param name="customPrice">The custom price in copper coins</param>
		/// <returns>Returns false if failed.</returns>
		public static bool SetShopItem(string npc, int item, List<Condition> condition, int customPrice)
		{
			if (!CheckIfValid(npc, item))
			{
				return false;
			}

			AdjustConditions(npc, ref condition);

			AddToCustomShops(npc, item, customPrice, condition);
			return true;
		}
		/// <summary>
		/// Attempts to do AddToCustomShops() after some checks.
		/// First it checks that item is within range of all loaded items.
		/// Second, it checks if the npc string matches one of the NPCs.
		/// The price of the item will be the item's value / 5 / priceDiv.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="condition">The availability of the item</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		/// <returns>Returns false if failed.</returns>
		public static bool SetShopItem(string npc, int item, List<Condition> condition, float priceDiv)
		{
			if (!CheckIfValid(npc, item))
			{
				return false;
			}

			AdjustConditions(npc, ref condition);

			AddToCustomShops(npc, item, (int)Math.Round(CalcItemValue(item) / 5 / priceDiv), condition);
			return true;
		}
		/// <summary>
		/// Attempts to do AddToCustomShops() after some checks.
		/// First it checks that item is within range of all loaded items.
		/// Second, it checks if the npc string matches one of the NPCs.
		/// The price of the item will be the item's (value / priceDiv) * priceMulti.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="condition">The availability of the item</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		/// <param name="priceMulti">The price will be multiplied by this amount after the priceDiv.</param>
		/// <returns>Returns false if failed.</returns>
		public static bool SetShopItem(string npc, int item, List<Condition> condition, float priceDiv, float priceMulti)
		{
			if (!CheckIfValid(npc, item))
			{
				return false;
			}

			AdjustConditions(npc, ref condition);

			AddToCustomShops(npc, item, (int)Math.Round(CalcItemValue(item) / priceDiv * priceMulti), condition);
			return true;
		}

		/// <summary>
		/// Checks to see if the string matches one of the NPCs.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <returns>True if a match is found.</returns>
		public static bool CheckIfValid(string npc, int item)
		{
			if (item > ItemLoader.ItemCount)
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("Cross mod SetShopItem(): Item type ID \"{0}\" exceeded the number of loaded items!", item);
				return false;
			}

			if (npc == NPCString.KingSlime ||
				npc == NPCString.EyeOfCthulhu ||
				npc == NPCString.EaterOfWorlds ||
				npc == NPCString.BrainOfCthulhu ||
				npc == NPCString.QueenBee ||
				npc == NPCString.Skeletron ||
				npc == NPCString.Deerclops ||
				npc == NPCString.WallOfFlesh ||
				npc == NPCString.QueenSlime ||
				npc == NPCString.TheDestroyer ||
				npc == NPCString.Retinazer ||
				npc == NPCString.Spazmatism ||
				npc == NPCString.SkeletronPrime ||
				npc == NPCString.Plantera ||
				npc == NPCString.Golem ||
				npc == NPCString.EmpressOfLight ||
				npc == NPCString.DukeFishron ||
				npc == NPCString.Betsy ||
				npc == NPCString.LunaticCultist ||
				npc == NPCString.MoonLord ||
				npc == NPCString.Dreadnautilus ||
				npc == NPCString.Mothron ||
				npc == NPCString.Pumpking ||
				npc == NPCString.IceQueen ||
				npc == NPCString.MartianSaucer ||
				npc == NPCString.GoblinTinkerer ||
				npc == NPCString.Pirate)
			{
				return true;
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("Cross mod SetShopItem(): NPC string \"{0}\" is not a valid npc type!", npc);
				return false;
			}
		}

		/// <summary>
		/// Gets the value of the item.
		/// </summary>
		/// <param name="item">The ID for the item</param>
		/// <returns>The value of the item.</returns>
		public static int CalcItemValue(int item)
		{
			// Item newItem = new(item);
			// return newItem.value;
			return ContentSamples.ItemsByType[item]?.value ?? 0;
		}

		/// <summary>
		/// Changes the "vanilla" Expert and Master Mode conditions to the ones that include the config.
		/// Also adds cross mod support condition if it wasn't added already.
		/// </summary>
		/// <param name="condition"> Pass the condition list </param>
		public static void AdjustConditions(string npc, ref List<Condition> condition)
		{
			// Change the vanilla Expert and Master Mode conditions to the one that includes the config.
			// (You could get around this by making your own conditions, but why would you?)
			if (condition.Remove(Condition.InExpertMode)) // Remove returns false if the item is not found.
			{
				condition.Add(ShopConditions.Expert);
			}
			if (condition.Remove(Condition.InMasterMode))
			{
				condition.Add(ShopConditions.Master);
			}

			if (npc == NPCString.GoblinTinkerer)
			{
				condition.Add(ShopConditions.GoblinSellInvasionItems);
			}
			if (npc == NPCString.Pirate)
			{
				condition.Add(ShopConditions.PirateSellInvasionItems);
			}

			// Add the cross mod support condition.
			if (!condition.Contains(ShopConditions.TownNPCsCrossModSupport))
			{
				condition.Add(ShopConditions.TownNPCsCrossModSupport);
			}
		}

		/// <summary>
		/// Checks if an item is in a shop.
		/// </summary>
		/// <param name="npc">The string for the which NPC's shop to check.</param>
		/// <param name="itemID">The item ID to search for.</param>
		/// <returns>True if found.</returns>
		public static bool CustomShopContainsItem(string npc, int itemID)
		{
#if DEBUG
			BossesAsNPCs.Instance.Logger.DebugFormat("      Searching Custom Shop {0}, item ID {1}", npc, itemID);
#endif
			foreach (ShopItem item in customShops[npc])
			{
#if DEBUG
				BossesAsNPCs.Instance.Logger.DebugFormat("                Item ID {0} {1}", item.ItemType, ContentSamples.ItemsByType[item.ItemType]?.Name);
#endif
				if (item.ItemType == itemID)
				{
#if DEBUG
					BossesAsNPCs.Instance.Logger.DebugFormat("      {0} contained {1}", npc, itemID);
#endif
					return true;
				}
			}
			return false;
		}

		// If the internal support for the mod is enabled.
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static bool Fargowiltas = true;
		public static bool FargowiltasSouls = true;
		public static bool CalamityMod = true;
		public static bool OrchidMod = true;
		public static bool Polarities = true;
		public static bool ThoriumMod = true;
		public static bool StormDiversMod = true;
		public static bool AmuletOfManyMinions = true;
		public static bool ClickerClass = true;
		public static bool QwertyMod = true;
		public static bool MagicStorage = true;
		public static bool ItReallyMustBe = true;
		public static bool EchoesoftheAncients = true;
		public static bool StarsAbove = true;
		public static bool StarlightRiver = true;
		public static bool PboneUtils = true;
		public static bool Avalon = true;
		public static bool Redeption = true;
		public static bool Consolaria = true;
		public static bool SOTS = true;
		public static bool VitalityMod = true;
		public static bool TheConfectionRebirth = true;
		public static bool CrystiliumMod = true;
		public static bool TheDepths = true;
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning restore IDE0079 // Remove unnecessary suppression

		#region King Slime
		/// <summary>
		/// King Slime's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void KingSlime(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.SlimeCrown) { shopCustomPrice = 50000 }); //Made up value since Slime Crown has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Solidifier, priceMulti: 2));
				//Formula: (Sell value / drop chance)
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SlimySaddle, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NinjaHood, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NinjaShirt, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NinjaPants, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SlimeHook, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SlimeGun, 0.67));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.KingSlimeMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.KingSlimeTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.RoyalGel, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.KingSlimePetItem, 0.25), ShopConditions.Master); //Royal Delight
				shop.Add(NPCHelper.ItemWithPrice(ItemID.KingSlimeMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10), // #145: KingSlime
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Gel, priceMulti: 10), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SlimeStaff, priceMulti: 10), ShopConditions.SellExtraItems); // priceDiv: 0.033 == 60 gold. Going to keep it at 20 gold.
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeCape>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeGloves>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSAltCostumeGloves>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.KingSlime;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SlimyCrown", npcString, 50000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeKingSlime", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CrownJewel", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "ThankYouPainting", npcString, 0.01f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimeKingsSlasher", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "MedallionoftheFallenKing", npcString, 0.01f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimyShield", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "KingSlimeFlask", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "KingSlimeCard", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "WardenSlime", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "ScrollTier1", npcString);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "Gelthrower", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "TechniqueHiddenBlade", npcString, 0.5f);
					NPCHelper.SafelySetCrossModItem(thorium, "ShinobiSlicer", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "GelGlove", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("StarlightRiver", out Mod starlightRiver) && StarlightRiver)
				{
					NPCHelper.SafelySetCrossModItem(starlightRiver, "Gelatine", npcString, 5000); // No value
					NPCHelper.SafelySetCrossModItem(starlightRiver, "SlimePrinceHead", npcString, 0.5f);
					NPCHelper.SafelySetCrossModItem(starlightRiver, "SlimePrinceChest", npcString, 0.5f);
					NPCHelper.SafelySetCrossModItem(starlightRiver, "SlimePrinceLegs", npcString, 0.5f);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "BandofSlime", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(avalon, "BirthofaMonster", npcString, 0.11f);
					NPCHelper.SafelySetCrossModItem(avalon, "StaminaCrystal", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "StickyKeychain", npcString, 0.25f);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.KingSlime, NPCString.KingSlime, ModContent.NPCType<KingSlime>());
				if (customShops.TryGetValue(NPCString.KingSlime, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						// set.Value.Item1 is the price (int)
						// set.Value.Item2 is the condition (List<Condition>)
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region EyeOfCthulhu
		/// <summary>
		/// Eye of Cthulhu's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void EyeOfCthulhu(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.SuspiciousLookingEye) { shopCustomPrice = 75000 }); //Made up value since it has no value
				// In in Hardmode, after defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 }, Condition.Hardmode, Condition.DownedEowOrBoc);
				// In in Pre-HardMode, after defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 * 2 }, Condition.PreHardmode, Condition.DownedEowOrBoc);
				// In before defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 * 5 }, Condition.NotDownedEowOrBoc);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DemoniteOre, priceMulti: 5), ShopConditions.CorruptionOrHardmode);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CorruptSeeds, priceMulti: 5), ShopConditions.CorruptionOrHardmode);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.CrimtaneOre, priceMulti: 5), ShopConditions.CrimsonOrHardmode);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CrimsonSeeds, priceMulti: 5), ShopConditions.CrimsonOrHardmode);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.Binoculars, 0.03)); //Formula: (Sell value * 3 / drop chance))

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BadgersHat, priceMulti: 20), Condition.NpcIsPresent(ModContent.NPCType<WallOfFlesh>()));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.EyeMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EyeofCthulhuTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.EoCShield, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.AviatorSunglasses, priceMulti: 5), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EyeOfCthulhuPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EyeofCthulhuMasterTrophy, priceMulti: 5), ShopConditions.Master);

				// #145: Demon Altar and Crimson Altar?

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.EyeOfCthulhu;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousEye", npcString, 80000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeEyeofCthulhu", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DeathstareRod", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "TeardropCleaver", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "LeashOfCthulhu", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "AgitatingLens", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeSword", npcString, 0.25f); //Eye Sored
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeGun", npcString, 0.25f); //Eye Rifle
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeStaff", npcString, 0.25f); //The Eyestalk
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeMinion", npcString, 0.25f); //Eyeball Staff
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeHook", npcString, 0.25f); //Eyeball Hook
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "EyeCard", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "ScrollTier2", npcString);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "Eyeruption", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "Pawn", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "BacciliteOre", npcString, priceDiv: 1f, priceMulti: 5f, ShopConditions.WorldContagion(avalon));
					NPCHelper.SafelySetCrossModItem(avalon, "IckyArrow", npcString, ShopConditions.WorldContagion(avalon));
					NPCHelper.SafelySetCrossModItem(avalon, "BloodyArrow", npcString, ShopConditions.WorldContagion(avalon));
					NPCHelper.SafelySetCrossModItem(avalon, "ContagionSeeds", npcString, ShopConditions.WorldContagion(avalon));
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.EyeofCthulhu, NPCString.EyeOfCthulhu, ModContent.NPCType<EyeOfCthulhu>());
				if (customShops.TryGetValue(NPCString.EyeOfCthulhu, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region EaterOfWorlds
		/// <summary>
		/// Eater of Worlds' shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void EaterOfWorlds(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.WormFood) { shopCustomPrice = 100000 }); //Made up value since it has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DemoniteOre, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ShadowScale, priceMulti: 5));
				//Formula: (Sell value / drop chance))
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EatersBone, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterofWorldsTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.WormScarf, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterOfWorldsPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterofWorldsMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10), // #145: EaterOfWorlds
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.VilePowder, valueDiv: 1), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.UnholyWater, valueDiv: 1), Condition.Hardmode, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PurpleSolution, valueDiv: 1), Condition.DownedMechBossAny, Condition.NpcIsPresent(NPCID.Steampunker), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WormTooth, valueDiv: 1), ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.EaterOfWorlds;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "WormyFood", npcString, 100000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeEaterofWorlds", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeCorruption", npcString, 10000);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "EaterLauncherJr", npcString, 0.1f); // The Blastbiter
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DarkenedHeart", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "EaterCard", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "PreservedCorruption", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "ScrollTier2", npcString);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "ConsumptionCannon", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "EaterOfPain", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("Redeption", out Mod redeption) && Redeption)
				{
					NPCHelper.SafelySetCrossModItem(redeption, "EldritchRoot", npcString, 0.0025f);
				}
				if (ModLoader.TryGetMod("Consolaria", out Mod consolaria) && Consolaria)
				{
					NPCHelper.SafelySetCrossModItem(consolaria, "SuspiciousLookingApple", npcString, 0.05f);
				}
				if (ModLoader.TryGetMod("SOTS", out Mod secretsOfTheShadows) && SOTS)
				{
					NPCHelper.SafelySetCrossModItem(secretsOfTheShadows, "PyramidKey", npcString);
					NPCHelper.SafelySetCrossModItem(secretsOfTheShadows, "ToothAche", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.EaterofWorldsHead, NPCString.EaterOfWorlds, ModContent.NPCType<EaterOfWorlds>());
				if (customShops.TryGetValue(NPCString.EaterOfWorlds, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region BrainOfCthulhu
		/// <summary>
		/// Brain of Cthulhu's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void BrainOfCthulhu(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.BloodySpine) { shopCustomPrice = 100000 }); //Made up value since it has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CrimtaneOre, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TissueSample, priceMulti: 5));
				//Formula: (Sell value / drop chance))
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BoneRattle, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrainMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrainofCthulhuTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrainOfConfusion, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrainOfCthulhuPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrainofCthulhuMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss3, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.ViciousPowder, valueDiv: 1), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BloodWater, valueDiv: 1), Condition.Hardmode, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RedSolution, valueDiv: 1), Condition.DownedMechBossAny, Condition.NpcIsPresent(NPCID.Steampunker), ShopConditions.SellExtraItems);

				// Added by my mod through Mod.Call
				/*if (ModLoader.TryGetMod("RijamsMod", out Mod rijamsMod)) // It's my mod lol
				{
					NPCHelper.SafelySetCrossModItem(rijamsMod, "CrawlerChelicera", NPCString.BrainOfCthulhu);
				}*/

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.BrainOfCthulhu;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "GoreySpine", npcString, 100000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeBrainofCthulhu", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeCrimson", npcString, 10000);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "BrainStaff", npcString, 0.1f); //Mind Break
					NPCHelper.SafelySetCrossModItem(fargosSouls, "CrimetroidEgg", npcString, 0.04f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "GuttedHeart", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "BrainCard", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "PreservedCrimson", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "ScrollTier1", npcString);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "NeuralBasher", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "TheStalker", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("SOTS", out Mod secretsOfTheShadows) && SOTS)
				{
					NPCHelper.SafelySetCrossModItem(secretsOfTheShadows, "PyramidKey", npcString);
					NPCHelper.SafelySetCrossModItem(secretsOfTheShadows, "Vertebraeker", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.BrainofCthulhu, NPCString.BrainOfCthulhu, ModContent.NPCType<BrainOfCthulhu>());
				if (customShops.TryGetValue(NPCString.BrainOfCthulhu, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region QueenBee
		/// <summary>
		/// Queen Bee's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void QueenBee(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.Abeemination) { shopCustomPrice = 125000 }); //Made up value since it has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeGun, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeKeeper, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeesKnees, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HiveWand, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeHat, 0.11));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeShirt, 0.11));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeePants, 0.11));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HoneyComb, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Nectar, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HoneyedGoggles, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Beenade, valueDiv: 1));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeWax, valueDiv: 1));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BottledHoney, valueDiv: 1));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenBeeTrophy, 0.1));

				// #145: Queen of Bees painting

				shop.Add(NPCHelper.ItemWithPrice(ItemID.HiveBackpack, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenBeePetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenBeeMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss5, priceMulti: 10), // #145: QueenBee
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.Hive) { shopCustomPrice = 100 }, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Stinger, valueDiv: 1), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Bezoar, valueDiv: 1), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeHive, valueDiv: 1), Condition.InGraveyard, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeeWings, valueDiv: 1), Condition.DownedMechBossAny, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.QueenBee;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "Abeemination2", npcString, 150000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeQueenBee", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "HardenedHoneycomb", npcString);
					NPCHelper.SafelySetCrossModItem(calamityMod, "TheBee", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TheSmallSting", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "QueenStinger", npcString, ShopConditions.EternityMode(fargosSouls)); //The Queen's Stinger
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BeeQueenMinionItem", npcString, 0.44f); //Bee Queen's Crown
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "QueenBeeCard", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "HoneyDie", npcString, 0.25f);
					// NPCHelper.SafelySetCrossModItem(orchidMod, "BeeSeeker", npcString, 0.17f);
					// NPCHelper.SafelySetCrossModItem(orchidMod, "WaxyVial", npcString, 0.17f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "QueenBeeFlask", npcString, 0.17f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "BeeRune", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "RoyalOrb", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "SweetHeart", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "FightoftheBumblebee", npcString);
				}
				if (ModLoader.TryGetMod("SOTS", out Mod secretsOfTheShadows) && SOTS)
				{
					NPCHelper.SafelySetCrossModItem(secretsOfTheShadows, "RoyalJelly", npcString);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "Honeydrop", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.QueenBee, NPCString.QueenBee, ModContent.NPCType<QueenBee>());
				if (customShops.TryGetValue(NPCString.QueenBee, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Skeletron
		/// <summary>
		/// Skeletron's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Skeletron(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.ClothierVoodooDoll) { shopCustomPrice = 130000 }); //Made up value since it has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronHand, 0.12));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BookofSkulls, 0.11));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChippysCouch, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BoneGlove, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss5, priceMulti: 10), // #145: Skeletron
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BoneKey, valueDiv: 1), ShopConditions.DownedDungeonGuardian, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BoneWand, valueDiv: 1), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Bone, valueDiv: 1), ShopConditions.SellExtraItems);

				if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC)) // I'll leave this here because it's a vanilla item and it's my mod.
				{
					if (fishermanNPC.TryFind<ModNPC>("Fisherman", out ModNPC fisherman))
					{
						shop.Add(NPCHelper.ItemWithPrice(ItemID.LockBox, valueDiv: 1), Condition.NpcIsPresent(fisherman.Type));
					}
				}
				
				// #145: Chippy's Set

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, ShopConditions.IsNotNpcShimmered);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, ShopConditions.IsNotNpcShimmered);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkeletronsRedHat>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.IsNpcShimmered);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkShimmeredCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.IsNpcShimmered);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkShimmeredAltCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.IsNpcShimmered);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkShimmeredCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.IsNpcShimmered);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkShimmeredCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.IsNpcShimmered);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Skeletron;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousSkull", npcString, 150000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeSkeletron", npcString, 10000);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "BoneZone", npcString, 0.1f); //The Bone Zone
					NPCHelper.SafelySetCrossModItem(fargosSouls, "NecromanticBrew", npcString, ShopConditions.EternityMode(fargosSouls));
				}

				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					//Skeletal Rod of Minion Guidance
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BoneWaypointRod", npcString, 100); //Normally no value
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "SquireSkullAccessory", npcString, 0.65f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "SkeletronCard", npcString);
					NPCHelper.SafelySetCrossModItem(orchidMod, "ScrollTier3", npcString);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "BonyBackhand", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "GuildsStaff", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("SOTS", out Mod secretsOfTheShadows) && SOTS)
				{
					NPCHelper.SafelySetCrossModItem(secretsOfTheShadows, "Baguette", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.SkeletronHead, NPCString.Skeletron, ModContent.NPCType<Skeletron>());
				if (customShops.TryGetValue(NPCString.Skeletron, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Deerclops
		/// <summary>
		/// Deerclops' shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Deerclops(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.DeerThing) { shopCustomPrice = 140000 }); //Made up value since it has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChesterPetItem, 0.33)); // Eye Bone
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Eyebrella, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DontStarveShaderItem, 0.33)); // Radio Thing
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DizzyHat, 0.0714)); // Dizzy's Rare Gecko Chester
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PewMaticHorn, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WeatherPain, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HoundiusShootius, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LucyTheAxe, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DeerclopsMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DeerclopsTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BoneHelm, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DeerclopsPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DeerclopsMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxDeerclops, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.AbigailsFlower, valueDiv: 1), Condition.InGraveyard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BatBat, 0.004), ShopConditions.UndergroundCavernsOrHardmode, Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BatBat, 0.01), ShopConditions.UndergroundCavernsOrHardmode, Condition.DontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HamBat, 0.04), ShopConditions.InIceAndHallowOrCorruptionOrCrimson, Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HamBat, 0.1) , ShopConditions.InIceAndHallowOrCorruptionOrCrimson, Condition.DontStarveWorld, ShopConditions.SellExtraItems);
				//Monster Meat
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PigPetItem, 0.001), Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PigPetItem, 0.005), Condition.DontStarveWorld, ShopConditions.SellExtraItems);
				//Glommer's Flower
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GlommerPetItem, 0.01), Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GlommerPetItem, 0.025), Condition.DontStarveWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.PaintingWendy, priceMulti: 5, valueDiv: 1), Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PaintingWillow, priceMulti: 5, valueDiv: 1), Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PaintingWilson, priceMulti: 5, valueDiv: 1), Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PaintingWolfgang, priceMulti: 5, valueDiv: 1), Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Deerclops;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "DeerThing2", npcString, 120000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "Deerclawps", npcString, ShopConditions.EternityMode(fargosSouls));
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DeerSinew", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "CyclopsClicker", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("StarlightRiver", out Mod starlightRiver) && StarlightRiver)
				{
					NPCHelper.SafelySetCrossModItem(starlightRiver, "HungryStomach", npcString);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "MonsterTooth", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Deerclops, NPCString.Deerclops, ModContent.NPCType<Deerclops>());
				if (customShops.TryGetValue(NPCString.Deerclops, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region WallOfFlesh
		/// <summary>
		/// Wall of Flesh's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void WallOfFlesh(NPCShop shop, string shopName, bool hackIsWoFAfterTorchGodHasRun)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.GuideVoodooDoll) { shopCustomPrice = 150000 }); //Made up value since it has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Pwnhammer, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BreakerBlade, 0.13));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ClockworkAssaultRifle, 0.13));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LaserRifle, 0.13));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FireWhip, 0.13)); // Firecracker
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WarriorEmblem, 0.13));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RangerEmblem, 0.13));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SorcererEmblem, 0.13));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SummonerEmblem, 0.13));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BadgersHat, priceMulti: 20), Condition.NpcIsPresent(ModContent.NPCType<EyeOfCthulhu>()));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.FleshMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WallofFleshTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.DemonHeart, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WallOfFleshGoatMountItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WallofFleshMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWWallOfFlesh, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.DemoniteBrick) { shopCustomPrice = 1500 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.CrimtaneBrick) { shopCustomPrice = 1500 }, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBackpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				// It just so happens that every NPC is loaded before the Torch God, except for the Wall of Flesh.
				// That means with All in One Town NPCs set to Mixed, Wall of Flesh gets two sets of all of the modded items because Torch God adds them to the customShops, then so does Wall of Flesh.
				// That also means the opposite is true for the Torch God. He gets two sets of all modded items. But, that is not much of a problem because his shops are disabled if the Town NPC is loaded.
				// This checks is for the Wall of Flesh to skip adding the modded items if the Torch God has already done it.
				if (hackIsWoFAfterTorchGodHasRun && ModContent.NPCType<TorchGod>() > 0)
				{
					goto SkipRegisteringModdedStuffTwice;
				}

				string npcString = NPCString.WallOfFlesh;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "FleshyDoll", npcString, 200000);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeWallofFlesh", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeUnderworld", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Carnage", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlackHawkRemote", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlastBarrel", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Meowthrower", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "RogueEmblem", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "HermitsBoxofOneHundredMedicines", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "FleshHand", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "PungentEyeball", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BoneSerpentMinionItem", npcString, 0.35f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "GuardianEmblem", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "ShapeshifterEmblem", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "OrchidEmblem", npcString);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "MawOfFlesh", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BurningSuperDeathClicker", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(clickerClass, "ClickerEmblem", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "NinjaEmblem", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "ClericEmblem", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "BardEmblem", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "RedSpiderLily", npcString);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "BloodHunterEmblem", npcString);
					NPCHelper.SafelySetCrossModItem(vitalityMod, "GluttonousLeash", npcString);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "FleshyTendril", npcString);
				}
				if (ModLoader.TryGetMod("TheConfectionRebirth", out Mod theConfectionRebirth) && TheConfectionRebirth)
				{
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "GrandSlammer", npcString);
				}
				if (ModLoader.TryGetMod("TheDepths", out Mod theDepths) && TheDepths)
				{
					NPCHelper.SafelySetCrossModItem(theDepths, "HungryLeash", npcString, 1f, priceMulti: 5f, ShopConditions.Expert);
					NPCHelper.SafelySetCrossModItem(theDepths, "ChasmeTrophy", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(theDepths, "ShadowChasmeMask", npcString, 0.14f);
					NPCHelper.SafelySetCrossModItem(theDepths, "ChasmeSoulMask", npcString, 0.14f);
					NPCHelper.SafelySetCrossModItem(theDepths, "POWHammer", npcString);
					NPCHelper.SafelySetCrossModItem(theDepths, "ShadeBlade", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(theDepths, "QuartzCannon", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(theDepths, "ShadowClaw", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(theDepths, "StaffOfAThousandYears", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(theDepths, "Onyx", npcString, 0.06f);
					NPCHelper.SafelySetCrossModItem(theDepths, "ShalestoneShackle", npcString, 1f, priceMulti: 5f, ShopConditions.Expert);
					NPCHelper.SafelySetCrossModItem(theDepths, "MidnightHorseshoe", npcString, 0.25f, ShopConditions.Master);
					NPCHelper.SafelySetCrossModItem(theDepths, "ChasmeRelic", npcString, 1f, priceMulti: 5f, ShopConditions.Master);
				}

				GenerateShops.GenerateDropsToAddToTheShops(NPCID.WallofFlesh, NPCString.WallOfFlesh, ModContent.NPCType<WallOfFlesh>());

				SkipRegisteringModdedStuffTwice:

				if (customShops.TryGetValue(NPCString.WallOfFlesh, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region QueenSlime
		/// <summary>
		/// Queen Slime's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void QueenSlime(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.QueenSlimeCrystal) { shopCustomPrice = 200000 }); //Made up value since it has no value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenSlimeMountSaddle, 0.25)); // Gelatinous Pillion
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CrystalNinjaHelmet, 0.33)); // Crystal Assassin Hood
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CrystalNinjaChestplate, 0.33)); // Crystal Assassin Shirt
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CrystalNinjaLeggings, 0.33)); // Crystal Assassin Pants
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenSlimeHook, 0.33)); // Hook of Dissonance
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Smolstar, 0.33)); // Blade Staff
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GelBalloon, priceMulti: 5)); // Sparkle Slime Balloon
				
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenSlimeMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenSlimeTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.VolatileGelatin, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenSlimePetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenSlimeMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxQueenSlime, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.PinkGel, priceMulti: 10), ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenSlime.QSAltCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenSlime.QSCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenSlime.QSCostumeGloves>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.QueenSlime;
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "LoreQueenSlime", npcString, 10000);
				}
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "JellyCrystal", npcString, 250000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "GelicWings", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "ClearKeychain", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "GuardianCrystalNinjaHelm", npcString);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "YoumuHilt", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.QueenSlimeBoss, NPCString.QueenSlime, ModContent.NPCType<QueenSlime>());
				if (customShops.TryGetValue(NPCString.QueenSlime, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region TheDestroyer
		/// <summary>
		/// The Destroyer's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void TheDestroyer(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.MechanicalWorm) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofMight, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalWagonPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss3, priceMulti: 10), // #145: TheDestroyer
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.TheDestroyer;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechWorm", npcString, 400000);

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", npcString, 1000000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeDestroyer", npcString, 10000);

					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", npcString, 10000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DestroyerGun", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "GroundStick", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", npcString, ShopConditions.Expert);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechTail", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", npcString);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "SonicHat", npcString);
					NPCHelper.SafelySetCrossModItem(avalon, "ScrollofTome", npcString);
				}
				if (ModLoader.TryGetMod("TheConfectionRebirth", out Mod theConfectionRebirth) && TheConfectionRebirth)
				{
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "NeapoliniteOre", npcString, priceDiv: 1f, priceMulti: 5f);
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "HallowedOre", npcString, priceDiv: 1f, priceMulti: 5f);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.TheDestroyer, NPCString.TheDestroyer, ModContent.NPCType<TheDestroyer>());
				if (customShops.TryGetValue(NPCString.TheDestroyer, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Retinazer
		/// <summary>
		/// Retinazer's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Retinazer(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.MechanicalEye) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofSight, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RetinazerTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalWheelPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss2, priceMulti: 10), // #145: TheTwins
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Retinazer.RetCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Retinazer.RetCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Retinazer;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechEye", npcString, 400000); //Match the Mutant's shop

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", npcString, 1000000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeTwins", npcString, 10000);

					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", npcString, 10000, Condition.DownedMechBossAll);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Arbalest", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TwinRangs", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "FusedLens", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", npcString, ShopConditions.Expert); //Mechanical Spikes
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechMask", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", npcString);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "SonicShirt", npcString);
					NPCHelper.SafelySetCrossModItem(avalon, "ScrollofTome", npcString);
				}
				if (ModLoader.TryGetMod("TheConfectionRebirth", out Mod theConfectionRebirth) && TheConfectionRebirth)
				{
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "NeapoliniteOre", npcString, priceDiv: 1f, priceMulti: 5f);
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "HallowedOre", npcString, priceDiv: 1f, priceMulti: 5f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "Retilazer", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Retinazer, NPCString.Retinazer, ModContent.NPCType<Retinazer>());
				if (customShops.TryGetValue(NPCString.Retinazer, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Spazmatism
		/// <summary>
		/// Spazmatism's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Spazmatism(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.MechanicalEye) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofSight, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SpazmatismTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalWheelPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss2, priceMulti: 10), // #145: TheTwins
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Spazmatism.SpazCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Spazmatism.SpazCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Spazmatism;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechEye", npcString, 400000);

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", npcString, 1000000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeTwins", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", npcString, 10000, Condition.DownedMechBossAll);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Arbalest", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TwinRangs", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "FusedLens", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", npcString, ShopConditions.Expert); //Mechanical Spikes
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechMask", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", npcString);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "SonicShirt", npcString);
					NPCHelper.SafelySetCrossModItem(avalon, "ScrollofTome", npcString);
					NPCHelper.SafelySetCrossModItem(avalon, "GreekExtinguisher", npcString);
				}
				if (ModLoader.TryGetMod("TheConfectionRebirth", out Mod theConfectionRebirth) && TheConfectionRebirth)
				{
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "NeapoliniteOre", npcString, priceDiv: 1f, priceMulti: 5f);
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "HallowedOre", npcString, priceDiv: 1f, priceMulti: 5f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "Spazmatica", npcString);
					NPCHelper.SafelySetCrossModItem(vitalityMod, "CursedFlamesprayer", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Spazmatism, NPCString.Spazmatism, ModContent.NPCType<Spazmatism>());
				if (customShops.TryGetValue(NPCString.Spazmatism, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region SkeletronPrime
		/// <summary>
		/// Skeletron Prime's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void SkeletronPrime(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.MechanicalSkull) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofFright, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimeMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimeTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalBatteryPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimePetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimeMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10), // #145: SkeletronPrime
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mechdusa.MdCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, Condition.RemixWorld, Condition.ForTheWorthyWorld);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.SkeletronPrime;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant)  && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechSkull", npcString, 400000);

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", npcString, 1000000, Condition.DownedMechBossAll);
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeSkeletronPrime", npcString, 10000);

					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", npcString, 10000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "RefractorBlaster", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "ReinforcedPlating", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", npcString, ShopConditions.Expert);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechChestplate", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", npcString);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "SonicShoes", npcString);
					NPCHelper.SafelySetCrossModItem(avalon, "ScrollofTome", npcString);
				}
				if (ModLoader.TryGetMod("TheConfectionRebirth", out Mod theConfectionRebirth) && TheConfectionRebirth)
				{
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "NeapoliniteOre", npcString, priceDiv: 1f, priceMulti: 5f);
					NPCHelper.SafelySetCrossModItem(theConfectionRebirth, "HallowedOre", npcString, priceDiv: 1f, priceMulti: 5f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "MechanicalHandful", npcString);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "Rageblade", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.SkeletronPrime, NPCString.SkeletronPrime, ModContent.NPCType<SkeletronPrime>());
				if (customShops.TryGetValue(NPCString.SkeletronPrime, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Plantera
		/// <summary>
		/// Plantera's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Plantera(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.TempleKey) { shopCustomPrice = 5000 }); // Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GrenadeLauncher, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.VenusMagnum, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NettleBurst, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LeafBlower, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FlowerPow, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaspGun, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Seedler, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PygmyStaff, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ThornHook, 0.1));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TheAxe, 0.02));
				// #145: Vulgar Display of Flower 12.5% chance
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Seedling, 0.05));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.PlanteraMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PlanteraTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.SporeSac, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PlanteraPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PlanteraMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxPlantera, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWPlantera, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.JungleGrassSeeds, priceMulti: 5), ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeBackpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Plantera;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "PlanterasFruit", npcString, 500000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgePlantera", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "LivingShard", npcString);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BloomStone", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlossomFlux", npcString, 0.1f);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "Dicer", npcString, 0.1f); //The Dicer

					NPCHelper.SafelySetCrossModItem(fargosSouls, "MagicalBulb", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "PottedPalMinionItem", npcString, 0.44f); //Potted Pal
				}
				if (ModLoader.TryGetMod("QwertyMod", out Mod qwertyMod) && QwertyMod)
				{
					NPCHelper.SafelySetCrossModItem(qwertyMod, "VitallumCoreUncharged", npcString); //Vitallum Core
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "BulbScepter", npcString, 0.66f);
					// NPCHelper.SafelySetCrossModItem(orchidMod, "FloralStinger", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "PlanteraStandard", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "JunglesRage", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(polarities, "UnfoldingBlossom", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "BloomWeave", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "BudBomb", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaRed", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaGreen", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaYellow", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaBlue", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VerdantOrnament", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "LifeDew", npcString);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "FoliageStaff", npcString);
					NPCHelper.SafelySetCrossModItem(vitalityMod, "PocketMachete", npcString);
					NPCHelper.SafelySetCrossModItem(vitalityMod, "SporeSpreader", npcString);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "DekuNut", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Plantera, NPCString.Plantera, ModContent.NPCType<Plantera>());
				if (customShops.TryGetValue(NPCString.Plantera, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Golem
		/// <summary>
		/// Golem's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Golem(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.LihzahrdPowerCell) { shopCustomPrice = 350000 }); // Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Picksaw, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeetleHusk, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Stynger, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.StyngerBolt, valueDiv: 1));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PossessedHatchet, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SunStone, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EyeoftheGolem, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HeatRay, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.StaffofEarth, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GolemFist, 0.14));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.GolemMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GolemTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.ShinyStone, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GolemPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.GolemMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss4, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.LihzahrdBrick) { shopCustomPrice = 2500 }, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LihzahrdAltar, priceMulti: 5 * 1000), ShopConditions.SellExtraItems); // Sells for 60 copper, but that seems way to cheap for an item that you should only have one of.

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Golem;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "LihzahrdPowerCell2", npcString, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeGolem", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "AegisBlade", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "RockSlide", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "ComputationOrb", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "LihzahrdTreasureBox", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "SunRay", npcString, 0.14f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "TempleWarhammer", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Golem, NPCString.Golem, ModContent.NPCType<Golem>());
				if (customShops.TryGetValue(NPCString.Golem, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region EmpressOfLight
		/// <summary>
		/// Empress of Light's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void EmpressOfLight(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.EmpressButterfly) { shopCustomPrice = 400000 }); // Prismatic Lacewing // Sell value * 5 = 250000
				// Formula: (Sell value / drop chance); It would be 200000 in this case
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FairyQueenMagicItem, 0.25)); // Nightglow
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PiercingStarlight, 0.25)); // Starlight
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RainbowWhip, 0.25)); // Kaleidoscope
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FairyQueenRangedItem, 0.25)); // Eventide
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RainbowWings, 0.07)); // Empress Wings
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowBossDye, 0.25)); // Prismatic Dye
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SparkleGuitar, 0.05)); // Stellar Tune
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RainbowCursor, 0.05)); // Rainbow Cursor

				// Special case since it is technically a "100% drop chance".
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EmpressBlade, priceMulti: 50), ShopConditions.DaytimeEoLDefated); //Terraprisma

				shop.Add(NPCHelper.ItemWithPrice(ItemID.FairyQueenMask, 0.14)); //Empress of Light Mask
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FairyQueenTrophy, 0.1)); //Empress of Light Trophy

				shop.Add(NPCHelper.ItemWithPrice(ItemID.EmpressFlightBooster, priceMulti: 5), ShopConditions.Expert); //Soaring Insignia
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FairyQueenPetItem, 0.25), ShopConditions.Master); //Jewel of Light
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FairyQueenMasterTrophy, priceMulti: 5), ShopConditions.Master); //Empress of Light Relic

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxEmpressOfLight, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.HolyWater, valueDiv: 1), Condition.Hardmode, ShopConditions.SellExtraItems); // For some reason Holy Water is double as valuable than Unholy/Blood Water.
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BlueSolution, valueDiv: 1), Condition.NpcIsPresent(NPCID.Steampunker), ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeEars>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.EmpressOfLight;
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "LoreEmpressofLight", npcString, 10000);
				}
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "PrismaticPrimrose", npcString, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "PrismaRegalia", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "PrecisionSeal", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "EmpressSquireMinionItem", npcString, 0.34f); //Chalice of the Empress
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "RainbowClicker", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "SunRay", npcString, 0.14f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "GuardianEmpressMaterial", npcString);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "LightShow", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.HallowBoss, NPCString.EmpressOfLight, ModContent.NPCType<EmpressOfLight>());
				if (customShops.TryGetValue(NPCString.EmpressOfLight, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region DukeFishron
		/// <summary>
		/// Duke Fishron's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void DukeFishron(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.TruffleWorm) { shopCustomPrice = 400000 }); // Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BubbleGun, 0.2), Condition.NotRemixWorld);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.AquaScepter, 0.2), Condition.RemixWorld);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Flairon, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RazorbladeTyphoon, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TempestStaff, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Tsunami, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FishronWings, 0.07));
				// #145: Eletric Eel 16.67%

				shop.Add(NPCHelper.ItemWithPrice(ItemID.DukeFishronMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DukeFishronTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.ShrimpyTruffle, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DukeFishronPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DukeFishronMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxDukeFishron, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.DukeFishron;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "TruffleWorm2", npcString, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeDukeFishron", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DukesDecapitator", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BrinyBaron", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "FishStick", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "MutantAntibodies", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("QwertyMod", out Mod qwertyMod) && QwertyMod)
				{
					NPCHelper.SafelySetCrossModItem(qwertyMod, "BubbleBrewerBaton", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(qwertyMod, "Cyclone", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(qwertyMod, "Whirlpool", npcString, 0.33f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "SeafoamClicker", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "DukesRegalCarnyx", npcString, 0.20f);
					NPCHelper.SafelySetCrossModItem(thorium, "Brinefang", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "SoulAnchor", npcString, 0.20f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "Cyclone", npcString);
					NPCHelper.SafelySetCrossModItem(vitalityMod, "DukesTusk", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.DukeFishron, NPCString.DukeFishron, ModContent.NPCType<DukeFishron>());
				if (customShops.TryGetValue(NPCString.DukeFishron, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Betsy
		/// <summary>
		/// Betsy's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Betsy(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.DD2ElderCrystal) { shopCustomPrice = 40000 });

				// Formula: (Sell value / drop chance))
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ApprenticeScarf, 0.25), ShopConditions.DownedDarkMage);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SquireShield, 0.25), ShopConditions.DownedDarkMage);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WarTable, 0.1), ShopConditions.DownedDarkMage);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WarTableBanner, 0.1), ShopConditions.DownedDarkMage);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2PetDragon, 0.17), ShopConditions.DownedDarkMage); // Dragon Egg
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2PetGato, 0.17), ShopConditions.DownedDarkMage); // Gato Egg
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossMaskDarkMage, 0.14), ShopConditions.DownedDarkMage);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossTrophyDarkmage, 0.1), ShopConditions.DownedDarkMage);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DarkMageBookMountItem, 0.25), ShopConditions.DownedDarkMage,
					ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DarkMageMasterTrophy, priceMulti: 5), ShopConditions.DownedDarkMage,
					ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.HuntressBuckler, 0.17), ShopConditions.DownedOgre);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MonkBelt, 0.17), ShopConditions.DownedOgre);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BookStaff, 0.1), ShopConditions.DownedOgre); // Tome of Infinite Wisdom
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2PhoenixBow, 0.1), ShopConditions.DownedOgre); // Phantom Phoenix
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2SquireDemonSword, 0.1), ShopConditions.DownedOgre); // Brand of the Inferno
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MonkStaffT1, 0.1), ShopConditions.DownedOgre); // Sleepy Octopod
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MonkStaffT2, 0.1), ShopConditions.DownedOgre); // Ghastly Glaive
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2PetGhost, 0.2), ShopConditions.DownedOgre); // Creeper Egg
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossMaskOgre, 0.14), ShopConditions.DownedOgre);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossTrophyOgre, 0.1), ShopConditions.DownedOgre);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2OgrePetItem, 0.25), ShopConditions.DownedOgre,
					ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.OgreMasterTrophy, priceMulti: 5), ShopConditions.DownedOgre,
					ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2BetsyBow, 0.25)); // Aerial Bane
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MonkStaffT3, 0.25)); // Sky Dragon's Fury
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ApprenticeStaffT3, 0.25)); // Betsy's Wrath
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2SquireBetsySword, 0.25)); // Flying Dragon
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BetsyWings, 0.07));
				
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossMaskBetsy, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossTrophyBetsy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.DD2BetsyPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BetsyMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxDD2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWInvasion, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				Condition randomVanity(int tick) => new("Mods.BossesAsNPCs.Conditions.RandomVanityS", () => Main.GameUpdateCount % 3 == tick);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DarkMage.DMCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DarkMage.DMCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DarkMage.DMCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Ogre.OgCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(2));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Ogre.OgCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(2));
			}

			if (shopName == "Shop2")
			{
				string npcString = NPCString.Betsy;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "ForbiddenTome", npcString, 50000, ShopConditions.DownedDarkMage); //Match the Abominationn's shop

					NPCHelper.SafelySetCrossModItem(fargosMutant, "BatteredClub", npcString, 150000, ShopConditions.DownedOgre); //Match the Abominationn's shop

					NPCHelper.SafelySetCrossModItem(fargosMutant, "BetsyEgg", npcString, 400000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DragonBreath", npcString, 0.1f); //Dragon's Breath

					NPCHelper.SafelySetCrossModItem(fargosSouls, "BetsysHeart", npcString, ShopConditions.EternityMode(fargosSouls)); //Betsy's Heart
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) && EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "BetsyScale", npcString);
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "FlameCore", npcString, ShopConditions.Expert); //Betsy's Flame
				}
				if (ModLoader.TryGetMod("PboneUtils", out Mod pbonesUtilities) && PboneUtils)
				{
					NPCHelper.SafelySetCrossModItem(pbonesUtilities, "DefendersCrystal", npcString);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "WyvernsNest", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "ArcaneClicker", npcString, 0.20f, ShopConditions.DownedDarkMage);
					NPCHelper.SafelySetCrossModItem(clickerClass, "SnottyClicker", npcString, 0.20f, ShopConditions.DownedOgre);
					NPCHelper.SafelySetCrossModItem(clickerClass, "DraconicClicker", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "DarkTome", npcString, ShopConditions.Expert, ShopConditions.DownedDarkMage);
					NPCHelper.SafelySetCrossModItem(thorium, "TabooWand", npcString, ShopConditions.Expert, ShopConditions.DownedDarkMage);
					NPCHelper.SafelySetCrossModItem(thorium, "DarkMageStaff", npcString, ShopConditions.Expert, ShopConditions.DownedDarkMage); // Dark Gift
					NPCHelper.SafelySetCrossModItem(thorium, "ArcaneAnelace", npcString, ShopConditions.Expert, ShopConditions.DownedDarkMage);
					NPCHelper.SafelySetCrossModItem(thorium, "BrewBlueprint", npcString, ShopConditions.DownedOgre);
					NPCHelper.SafelySetCrossModItem(thorium, "OgreSandal", npcString, ShopConditions.Expert, ShopConditions.DownedOgre);
					NPCHelper.SafelySetCrossModItem(thorium, "OgreSnotGun", npcString, ShopConditions.Expert, ShopConditions.DownedOgre);
					NPCHelper.SafelySetCrossModItem(thorium, "Hippocraticrossbow", npcString, ShopConditions.Expert, ShopConditions.DownedOgre);
					NPCHelper.SafelySetCrossModItem(thorium, "DragonFang", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "DragonHeartWand", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "BetsysBellow", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "ValhallasDescent", npcString, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(thorium, "MediumRareSteak", npcString, 1f, 5f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "DragonDagger", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.DD2Betsy, NPCString.Betsy, ModContent.NPCType<Betsy>());
				if (customShops.TryGetValue(NPCString.Betsy, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region LunaticCultist
		/// <summary>
		/// Lunatic Cultist's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void LunaticCultist(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.LunarCraftingStation) { shopCustomPrice = 100000 }); // Ancient Manipulator // Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentSolar, priceMulti: 10), Condition.DownedSolarPillar);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentVortex, priceMulti: 10), Condition.DownedSolarPillar);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentNebula, priceMulti: 10), Condition.DownedSolarPillar);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentStardust, priceMulti: 10), Condition.DownedSolarPillar);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossMaskCultist, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.AncientCultistTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunaticCultistPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunaticCultistMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss4, priceMulti: 10), // #145: LunaticCultist
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.LunaticCultist;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CultistSummon", npcString, 750000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeLunaticCultist", npcString, 10000);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "CelestialRune", npcString, ShopConditions.EternityMode(fargosSouls));
					NPCHelper.SafelySetCrossModItem(fargosSouls, "MutantsPact", npcString, ShopConditions.EternityMode(fargosSouls)); //Mutant's Pact
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistLazor", npcString, 0.02f); //Mysterious Cultist Hood
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistBow", npcString, 0.25f); //Lunatic Bow of Ice
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistSpear", npcString, 0.25f); //Lunatic Spear of Fire
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistTome", npcString, 0.25f); //Lunatic Spell of Ancient Light
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistStaff", npcString, 0.25f); //Lunatic Staff of Lightning
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunaticHood", npcString, ShopConditions.Expert);  //Lunatic Hood of Command
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarSelenianBlade", npcString, 0.05f);  // Selenian Blade // Made slightly cheaper than 2%
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarVortexShotgun", npcString, 0.05f);  // Storm Diver Shotgun
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarPredictorBrain", npcString, 0.05f);  // Predictor Brain
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarStargazerLaser", npcString, 0.05f);  // Stargazer Core

					Condition randomVanity(int tick) => new("Mods.BossesAsNPCs.Conditions.RandomVanityS", () => Main.GameUpdateCount % 4 == tick);

					// Randomly choose a vantiy set every time the shop is opened.

					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SelenianBMask", npcString, 0.05f, randomVanity(0));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SelenianBody", npcString, 0.05f, randomVanity(0));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SelenianLegs", npcString, 0.05f, randomVanity(0));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StormDiverBMask", npcString, 0.05f, randomVanity(1));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StormDiverBody", npcString, 0.05f, randomVanity(1));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StormDiverLegs", npcString, 0.05f, randomVanity(1));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PredictorBMask", npcString, 0.05f, randomVanity(2));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PredictorBody", npcString, 0.05f, randomVanity(2));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PredictorLegs", npcString, 0.05f, randomVanity(2));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StargazerBMask", npcString, 0.05f, randomVanity(3));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StargazerBody", npcString, 0.05f, randomVanity(3));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StargazerLegs", npcString, 0.05f, randomVanity(3));
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) && EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "LunarSilk", npcString);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "AbyssFragment", npcString, 1f, 2f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "MiceFragment", npcString, 1f, 2f, ShopConditions.DownedAnyPillar);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "WhiteDwarfFragment", npcString, ShopConditions.DownedAllPillars);
					NPCHelper.SafelySetCrossModItem(thorium, "CelestialFragment", npcString, ShopConditions.DownedAllPillars);
					NPCHelper.SafelySetCrossModItem(thorium, "ShootingStarFragment", npcString, ShopConditions.DownedAllPillars);

					NPCHelper.SafelySetCrossModItem(thorium, "AncientFlame", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "AncientSpark", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "AncientFrost", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "AstralFang", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "CosmicFluxStaff", npcString, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "LunaticsHood", npcString, 0.2f);
					NPCHelper.SafelySetCrossModItem(thorium, "LunaticsRobe", npcString, 0.2f);
					NPCHelper.SafelySetCrossModItem(thorium, "LunaticsLeggings", npcString, 0.2f);
					NPCHelper.SafelySetCrossModItem(thorium, "AncientLight", npcString);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "RitualSyringe", npcString);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "PearlescentOrb", npcString);
					NPCHelper.SafelySetCrossModItem(starsAbove, "ResonanceGem", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.CultistBoss, NPCString.LunaticCultist, ModContent.NPCType<LunaticCultist>());
				if (customShops.TryGetValue(NPCString.LunaticCultist, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region MoonLord
		/// <summary>
		/// Moon Lord's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void MoonLord(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.CelestialSigil) { shopCustomPrice = 500000 });
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PortalGun, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunarOre, priceMulti: 5));
				// Even though Moon Lord now drops two of these items, I've left the chances at 0.22
				// #145: now 20%
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Meowmere, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Terrarian, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.StarWrath, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SDMG, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LastPrism, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunarFlareBook, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RainbowCrystalStaff, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoonlordTurretStaff, 0.22)); // Lunar Portal Staff
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Celeb2, 0.22)); // Celebration Mk2
				// #145: Possession 20%
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MeowmereMinecart, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossMaskMoonlord, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoonLordTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.GravityGlobe, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SuspiciousLookingTentacle, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LongRainbowTrailWings, priceMulti: 5), ShopConditions.Expert);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoonLordPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoonLordMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxLunarBoss, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWMoonLord, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MoonLord.MLCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MoonLord.MLCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoonLordLegs, priceMulti: 5), ShopConditions.SellExtraItems);
				
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.UnlockedBiomeTorches, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.UnlockedBiomeTorches, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.UnlockedBiomeTorches, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.MoonLord;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CelestialSigil2", npcString, 1000000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMoonLord", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CelestialOnion", npcString, 100000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "UtensilPoker", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DeviousAestheticus", npcString, 0.05f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "GalacticGlobe", npcString, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) && EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "TrueThirdEye", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Cosmic_Key", npcString, 100000);
				}
				if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage) && MagicStorage)
				{
					NPCHelper.SafelySetCrossModItem(magicStorage, "RadiantJewel", npcString, 0.05f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "Nirvana", npcString, 0.5f);
					// NPCHelper.SafelySetCrossModItem(orchidMod, "TheCore", npcString, 0.5f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "MoonLordRune", npcString, 0.5f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "MoonLordShield", npcString, 0.5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "LordsClicker", npcString);
					NPCHelper.SafelySetCrossModItem(clickerClass, "TheClicker", npcString, 0.20f);
					NPCHelper.SafelySetCrossModItem(clickerClass, "TorchClicker", npcString, ShopConditions.UnlockedBiomeTorches);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "AngelsEnd", npcString, 0.11f);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeAndDeath", npcString, 0.11f);
					NPCHelper.SafelySetCrossModItem(thorium, "SonicAmplifier", npcString, 0.11f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "PearlescentOrb", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.MoonLordCore, NPCString.MoonLord, ModContent.NPCType<MoonLord>());
				if (customShops.TryGetValue(NPCString.MoonLord, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Dreadnautilus
		/// <summary>
		/// Dreadnautilus' shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Dreadnautilus(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.BloodMoonStarter) { shopCustomPrice = 60000 }); //Bloody Tear
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BunnyHood, 0.0133));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PedguinHat, 0.0067, secondDiv: 3));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PedguinShirt, 0.0067, secondDiv: 3));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PedguinPants, 0.0067, secondDiv: 3));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.KiteBunnyCorrupt, 0.04));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.KiteBunnyCrimson, 0.04));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TopHat, priceMulti: 5)); //Technically a 90% drop chance, but in certain cases you could sell the hat for more than you bought it
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TheBrideHat, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TheBrideDress, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoneyTrough, 0.005, secondDiv: 2)); //0.5% from Blood Zombies & Dripplers. Not using the 6.67% from Zombie Merman & Wandering Eye Fish
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SharkToothNecklace, 0.0067, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChumBucket, priceMulti: 5 * 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BloodRainBow, 0.125));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.VampireFrogStaff, 0.125));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BloodFishingRod, 0.0417)); //Chum Caster
				shop.Add(new Item(ItemID.CombatBook) { shopCustomPrice = 500000 }, new Condition("Advanced Combat Techniques has not been used", () => !NPC.combatBookWasUsed)); //Advanced Combat Techniques

				shop.Add(NPCHelper.ItemWithPrice(ItemID.KOCannon, 0.01, secondDiv: 10), Condition.Hardmode); //Dropped by ANY enemy during a Blood Moon
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Bananarang, 0.0333), Condition.Hardmode);
				// No Trifold Map lol
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BloodHamaxe, 0.125), Condition.Hardmode); //Haemorrhaxe
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SharpTears, 0.125), Condition.Hardmode); //Blood Thorn
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DripplerFlail, 0.125), Condition.Hardmode); //Drippler Crippler
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SanguineStaff, priceMulti: 5), Condition.Hardmode);  //50% drop chance in normal mode, but I wanted it to be more expensive

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BloodMoonMonolith, 0.1111));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DreadoftheRedSea, 0.05), Condition.BloodMoon); // Don't actually know the odds.

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxEerie, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBloodMoon, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Dreadnautilus;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousLookingLure", npcString, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodUrchin", npcString, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "HemoclawCrab", npcString, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodSushiPlatter", npcString, 200000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "LoreBloodMoon", npcString, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BloodOrb", npcString, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BouncingEyeball", npcString, (0.025f * 2f));
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SqueakyToy", npcString, 0.1f, ShopConditions.EternityMode(fargosSouls));
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DreadShell", npcString, 0.2f, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodDrop", npcString); //Bloody Drop
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodyRifle", npcString, 0.125f); //Bloodshot Rifle
				}
				if (ModLoader.TryGetMod("ItReallyMustBe", out Mod dreadnautilusIsABoss) && ItReallyMustBe)
				{
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "FunnyBait", npcString); //Blood Bait
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadPistol", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusTrophy", npcString, 0.1f);

					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "BloodyCarKey", npcString, 0.25f, ShopConditions.Master);
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusRelic", npcString, 0.1f, ShopConditions.Master);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "BloodMoonFlask", npcString, (0.025f * 2));
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "HemoClicker", npcString, 0.04f * 2f);
					NPCHelper.SafelySetCrossModItem(clickerClass, "SpiralClicker", npcString, 0.50f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "LuckyRabbitsFoot", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(thorium, "Blood", npcString, 0.2f);
					NPCHelper.SafelySetCrossModItem(thorium, "SeveredHand", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "GraveBuster", npcString, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(thorium, "GoodBook", npcString, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(thorium, "BloodFeasterStaff", npcString, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "BloodDrinker", npcString, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "RifleSpear", npcString, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "EvisceratingClaw", npcString, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "BattleHorn", npcString, 0.02f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "Bagpipe", npcString, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "TechniqueBloodLotus", npcString, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "ShadeBand", npcString, 0.1f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "NecroticStaff", npcString, 0.1f, Condition.Hardmode);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "BloodsBoundary", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.BloodNautilus, NPCString.Dreadnautilus, ModContent.NPCType<Dreadnautilus>());
				if (customShops.TryGetValue(NPCString.Dreadnautilus, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Mothron
		/// <summary>
		/// Mothron's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Mothron(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.SolarTablet) { shopCustomPrice = 20000 });
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EyeSpring, 0.0667));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrokenBatWing, 0.025, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoonStone, 0.0286, secondDiv: 4));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NeptunesShell, 0.02, secondDiv: 4));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Steak, 0.01, secondDiv: 6));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DeathSickle, 0.025, secondDiv: 2) );
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ButchersChainsaw, 0.025, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ButcherMask, 0.02, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ButcherApron, 0.02, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ButcherPants, 0.02, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DeadlySphereStaff, 0.025, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ToxicFlask, 0.025, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DrManFlyMask, 0.0396, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DrManFlyLabCoat, 0.0396, secondDiv: 2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NailGun, 0.04));
				shop.Add(new Item(ItemID.Nail) { shopCustomPrice = 100 }); //Match the price of the Arm's Dealer
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PsychoKnife, 0.025));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrokenHeroSword, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TheEyeOfCthulhu, 0.33));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MothronWings, 0.05));

				Condition randomPainting(int tick) => new("Mods.BossesAsNPCs.Conditions.MothronPaintingsS", () => Main.GameUpdateCount % 8 == tick);

				// Randomly choose a painting every time the shop is opened.

				shop.Add(NPCHelper.ItemWithPrice(ItemID.WingsofEvil, 0.067), randomPainting(0));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MidnightSun, 0.017), randomPainting(1));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Buddies, 0.0044), randomPainting(2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ThisIsGettingOutOfHand, 0.017), randomPainting(3));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.AMachineforTerrarians, 0.017), randomPainting(4));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Requiem, 0.017), randomPainting(5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Eyezorhead, 0.067), randomPainting(6));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.OcularResonance, 0.067), randomPainting(7));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxEclipse, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBloodMoon, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Mothron;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MothronEgg", npcString, 150000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "SolarVeil", npcString);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DefectiveSphere", npcString, 0.2f);
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "SqueyereMinionItem", npcString, 0.1f); //Crest of Eyes
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) &&  EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Broken_Hero_GunParts", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "BrokenHeroScepter", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "EclipticClicker", npcString, 0.04f * 2f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "TeslaDefibrillator", npcString, 0.025f);
					NPCHelper.SafelySetCrossModItem(thorium, "SwampSpike", npcString, 0.025f);
					NPCHelper.SafelySetCrossModItem(thorium, "FireAxe", npcString, 0.033f);
					NPCHelper.SafelySetCrossModItem(thorium, "GarlicBread", npcString, 0.01f);
					NPCHelper.SafelySetCrossModItem(thorium, "BrokenHeroFragment", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "SunrayStaff", npcString, 0.05f);
					NPCHelper.SafelySetCrossModItem(thorium, "SunflareGuitar", npcString, 0.05f);
					NPCHelper.SafelySetCrossModItem(thorium, "StalkersSnippers", npcString, 0.05f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "BrokenVigilanteTome", npcString);
				}
				if (ModLoader.TryGetMod("CrystiliumMod", out Mod crystiliumMod) && CrystiliumMod)
				{
					NPCHelper.SafelySetCrossModItem(crystiliumMod, "BrokenStaff", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Mothron, NPCString.Mothron, ModContent.NPCType<Mothron>());
				if (customShops.TryGetValue(NPCString.Mothron, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Pumpking
		/// <summary>
		/// Pumpking's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void Pumpking(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.PumpkinMoonMedallion) { shopCustomPrice = 150000 });
				//Using the highest drop chances
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ScarecrowHat, 0.033));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ScarecrowShirt, 0.033));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ScarecrowPants, 0.033));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.JackOLanternMask, 0.05));
				shop.Add(new Item(ItemID.SpookyWood) { shopCustomPrice = 5000 }); //Made up value

				shop.Add(NPCHelper.ItemWithPrice(ItemID.SpookyHook, 0.2), Condition.DownedMourningWood);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SpookyTwig, 0.2), Condition.DownedMourningWood);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.StakeLauncher, 0.2), Condition.DownedMourningWood);
				shop.Add(new Item(ItemID.Stake) { shopCustomPrice = 15 }, Condition.DownedMourningWood); //Same price as Arms Dealer/Witch Doctor
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CursedSapling, 0.2), Condition.DownedMourningWood);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NecromanticScroll, 0.2), Condition.DownedMourningWood);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MourningWoodTrophy, 0.1), Condition.DownedMourningWood); //same trophy price

				shop.Add(NPCHelper.ItemWithPrice(ItemID.WitchBroom, priceMulti: 5), Condition.DownedMourningWood, ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SpookyWoodMountItem, 0.25), Condition.DownedMourningWood, ShopConditions.Master); //Hexxed Branch
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MourningWoodMasterTrophy, priceMulti: 5), Condition.DownedMourningWood, ShopConditions.Master); //Hexxed Branch

				shop.Add(NPCHelper.ItemWithPrice(ItemID.TheHorsemansBlade, 0.125));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BatScepter, 0.125));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BlackFairyDust, 0.125));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SpiderEgg, 0.125));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RavenStaff, 0.125));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CandyCornRifle, 0.125));
				shop.Add(new Item(ItemID.CandyCorn) { shopCustomPrice = 5 }); //Same price as Arms Dealer
				shop.Add(NPCHelper.ItemWithPrice(ItemID.JackOLanternLauncher, 0.125));
				shop.Add(new Item(ItemID.ExplosiveJackOLantern) { shopCustomPrice = 15 }); //Same price as Arms Dealer
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ScytheWhip, 0.125)); //Dark Harvest
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PumpkingTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.PumpkingPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PumpkingMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxPumpkinMoon, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWInvasion, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.GoodieBag) { shopCustomPrice = 5000 }, ShopConditions.SellExtraItems);

				Condition randomVanity(int tick) => new("Mods.BossesAsNPCs.Conditions.RandomVanityS", () => Main.GameUpdateCount % 2 == tick);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeShoes>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MourningWood.MWCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MourningWood.MWCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MourningWood.MWCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.Pumpking;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SpookyBranch", npcString, 200000, Condition.DownedMourningWood); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousLookingScythe", npcString, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "PumpkingsCape", npcString, 0.2f, ShopConditions.EternityMode(fargosSouls)); //Pumpking's Cape
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "GoldenRogueSquireMinionItem", npcString, 0.13f); //Golden Rogue Crest
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SpookyCore", npcString, 0.07f); //Spooky Emblem
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "MourningTorch", npcString, 0.1f, Condition.DownedMourningWood);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "WitchClicker", npcString, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(clickerClass, "LanternClicker", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "PaganGrasp", npcString, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(thorium, "Effigy", npcString, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(thorium, "CharonsBeacon", npcString, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(thorium, "Witchblade", npcString, 0.1f);
					NPCHelper.SafelySetCrossModItem(thorium, "SnackLantern", npcString, 0.2f);
					NPCHelper.SafelySetCrossModItem(thorium, "HauntingBassDrum", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "JackOCrack", npcString);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "GuppyHead", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.MourningWood, NPCString.Pumpking, ModContent.NPCType<Pumpking>());
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Pumpking, NPCString.Pumpking, ModContent.NPCType<Pumpking>());
				if (customShops.TryGetValue(NPCString.Pumpking, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region IceQueen
		/// <summary>
		/// Ice Queen's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void IceQueen(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.NaughtyPresent) { shopCustomPrice = 150000 }); //Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ElfHat, 0.017));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ElfShirt, 0.017));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ElfPants, 0.017));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChristmasTreeSword, 0.078), Condition.DownedEverscream);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChristmasHook, 0.078), Condition.DownedEverscream);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Razorpine, 0.078), Condition.DownedEverscream);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FestiveWings, 0.017), Condition.DownedEverscream);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EverscreamTrophy, 0.1), Condition.DownedEverscream);
				
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EverscreamPetItem, 0.25), Condition.DownedEverscream, ShopConditions.Master); //Shrub Star
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EverscreamMasterTrophy, priceMulti: 5), Condition.DownedEverscream, ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.ElfMelter, 0.125), Condition.DownedSantaNK1);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChainGun, 0.125), Condition.DownedSantaNK1);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SantaNK1Trophy, 0.1), Condition.DownedSantaNK1);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SantankMountItem, 0.25), Condition.DownedSantaNK1, ShopConditions.Master); //Toy Tank
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SantankMasterTrophy, priceMulti: 5), Condition.DownedSantaNK1, ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BlizzardStaff, 0.08));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SnowmanCannon, 0.08));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.NorthPole, 0.08));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BabyGrinchMischiefWhistle, 0.017));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ReindeerBells, 0.017));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.IceQueenTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.IceQueenPetItem, 0.25), ShopConditions.Master); //Frozen Crown
				shop.Add(NPCHelper.ItemWithPrice(ItemID.IceQueenMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxFrostMoon, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWInvasion, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.Present) { shopCustomPrice = 5000 }, ShopConditions.SellExtraItems);

				Condition randomVanity(int tick) => new("Mods.BossesAsNPCs.Conditions.RandomVanityS", () => Main.GameUpdateCount % 3 == tick);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeCape>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(0));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Everscream.EsCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Everscream.EsCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Everscream.EsCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(1));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SantaNK1.SNKCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(2));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SantaNK1.SNKCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(2));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SantaNK1.SNKCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(2));
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SantaNK1.SNKCostumeBackpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems, randomVanity(2));
			}

			if (shopName == "Shop2")
			{
				string npcString = NPCString.IceQueen;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "FestiveOrnament", npcString, 200000, Condition.DownedEverscream); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "NaughtyList", npcString, 200000, Condition.DownedSantaNK1); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "IceKingsRemains", npcString, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "IceQueensCrown", npcString, 0.2f, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SantankScrap", npcString); //Mechanical Scrap
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "IceSentry", npcString, 0.1f); //Frozen Queen's Staff
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "FrostCube", npcString, 0.07f); //Frozen Queen's Core
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SantankMinion", npcString); 
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SantaShotgun", npcString);
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SantaWires", npcString);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "RCRemote", npcString, 0.02f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "FrostRune", npcString, 0.02f);
					// NPCHelper.SafelySetCrossModItem(orchidMod, "FragilePresent", npcString, 0.1f, Condition.DownedSantaNK1);
					// NPCHelper.SafelySetCrossModItem(orchidMod, "IceFlakeCone", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "CandyCaneAtlatl", npcString, 0.23f, Condition.DownedEverscream);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "NaughtyClicker", npcString, 0.1f, Condition.DownedSantaNK1);
					NPCHelper.SafelySetCrossModItem(clickerClass, "FrozenClicker", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "Permafrost", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "ChristmasCheer", npcString, 0.07f, Condition.DownedEverscream);
					NPCHelper.SafelySetCrossModItem(thorium, "JingleBells", npcString, 0.1f, Condition.DownedSantaNK1);
					NPCHelper.SafelySetCrossModItem(thorium, "SoftServeSunderer", npcString);
					NPCHelper.SafelySetCrossModItem(thorium, "Cryotherapy", npcString, 0.1f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "Jollylash", npcString);
					NPCHelper.SafelySetCrossModItem(vitalityMod, "Piercicle", npcString);
					NPCHelper.SafelySetCrossModItem(vitalityMod, "ShiverFragment", npcString);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod starsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(starsAbove, "GuppyHead", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.Everscream, NPCString.IceQueen, ModContent.NPCType<IceQueen>());
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.SantaNK1, NPCString.IceQueen, ModContent.NPCType<IceQueen>());
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.IceQueen, NPCString.IceQueen, ModContent.NPCType<IceQueen>());
				if (customShops.TryGetValue(NPCString.IceQueen, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region MartianSaucer
		/// <summary>
		/// Martian Saucer's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void MartianSaucer(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1")
			{
				shop.Add(new Item(ItemID.MartianConduitPlating) { shopCustomPrice = 100 });
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianCostumeMask, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianCostumeShirt, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianCostumePants, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianUniformHelmet, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianUniformTorso, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianUniformPants, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BrainScrambler, 0.01));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LaserDrill, 0.013, secondDiv: 7)); //Special case to make it cheaper
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChargedBlasterCannon, 0.013, secondDiv: 7));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.AntiGravityHook, 0.013, secondDiv: 7));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Xenopopper, 0.167));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.XenoStaff, 0.167));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LaserMachinegun, 0.167));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ElectrosphereLauncher, 0.167));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.InfluxWaver, 0.167));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CosmicCarKey, 0.167));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianSaucerTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MartianPetItem, 0.25), ShopConditions.Master); //Cosmic Skateboard
				shop.Add(NPCHelper.ItemWithPrice(ItemID.UFOMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxMartians, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWInvasion, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2")
			{
				string npcString = NPCString.MartianSaucer;
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "RunawayProbe", npcString, 500000); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MartianMemoryStick", npcString, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "ShockGrenade", npcString);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Wingman", npcString, 0.14f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "NullificationRifle", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SaucerControlConsole", npcString, 0.2f, ShopConditions.EternityMode(fargosSouls));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SuperDartLauncher", npcString, 0.01f * 6);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					// NPCHelper.SafelySetCrossModItem(orchidMod, "MartianBeamer", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "MartianWarhammer", npcString);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "HighTechClicker", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "VoltModule", npcString, 0.05f);
					NPCHelper.SafelySetCrossModItem(thorium, "ShieldDroneBeacon", npcString, 0.04f);
					NPCHelper.SafelySetCrossModItem(thorium, "CellReconstructor", npcString, 0.04f);
					NPCHelper.SafelySetCrossModItem(thorium, "ElectroRebounder", npcString, 0.5f);
					NPCHelper.SafelySetCrossModItem(thorium, "TheTriangle", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "Turntable", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "SuperPlasmaCannon", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "Kinetoscythe", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "CosmicDagger", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "LivewireCrasher", npcString, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "MolecularStabilizer", npcString, 0.25f);
				}
				if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
				{
					NPCHelper.SafelySetCrossModItem(vitalityMod, "MartianScrap", npcString);
				}
				if (ModLoader.TryGetMod("Avalon", out Mod avalon) && Avalon)
				{
					NPCHelper.SafelySetCrossModItem(avalon, "StaminaCrystal", npcString);
				}
				GenerateShops.GenerateDropsToAddToTheShops(NPCID.MartianSaucerCore, NPCString.MartianSaucer, ModContent.NPCType<MartianSaucer>());
				if (customShops.TryGetValue(NPCString.MartianSaucer, out List<ShopItem> value))
				{
					foreach (ShopItem set in value)
					{
						shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
					}
				}
			}
		}
		#endregion

		#region Goblin Tinkerer

		internal static List<int> GoblinTinkererShopCopy = [];

		/// <summary>
		/// Goblin Tinkerer's extra shop. These shop items are affected by the shop price scaling config.
		/// </summary>
		/// <param name="realShop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		public static void GoblinTinkerer(NPCShop realShop)
		{
			// Add the shop items to a fake shop so I can then save the items that I've added (and not vanilla's or any other mods' items) to the List<int> above.
			// The List<int> is used for changing the price of the items and I only want to change the price of the items that I've added.
			NPCShop shop = new(NPCID.GoblinTinkerer, "ShopBossesAsNPCs");

			string npcString = NPCString.GoblinTinkerer;

			shop.Add(new Item(ItemID.GoblinBattleStandard) { shopCustomPrice = 25000 }, ShopConditions.GoblinSellInvasionItems); //Made up value
			shop.Add(NPCHelper.ItemWithPrice(ItemID.Harpoon, 0.005, secondDiv: 5), ShopConditions.GoblinSellInvasionItems); //Special case to make it cheaper
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
			{
				NPCHelper.SafelySetCrossModItem(calamityMod, "PlasmaRod", npcString, (0.07f * 5), ShopConditions.GoblinSellInvasionItems);
			}
			if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
			{
				NPCHelper.SafelySetCrossModItem(orchidMod, "GoblinArmyFlask", npcString, (0.02f * 5), ShopConditions.GoblinSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(orchidMod, "GoblinArmyCard", npcString, (0.02f * 5), ShopConditions.GoblinSellInvasionItems);
				// NPCHelper.SafelySetCrossModItem(orchidMod, "GoblinStick", npcString, 0.33f, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
			}
			if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
			{
				NPCHelper.SafelySetCrossModItem(clickerClass, "ShadowyClicker", npcString, (0.05f * 5), ShopConditions.GoblinSellInvasionItems);
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
			{
				NPCHelper.SafelySetCrossModItem(thorium, "YewWoodBlowpipe", npcString, 0.05f, ShopConditions.GoblinSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "YewWood", npcString, ShopConditions.GoblinSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "DarkGate", npcString, 0.05f, ShopConditions.GoblinSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "SpikeBomb", npcString, ShopConditions.GoblinSellInvasionItems);
			}
			shop.Add(new Item(ItemID.ShadowFlameHexDoll) { shopCustomPrice = (int)Math.Round(20000 / 0.17) }, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
			shop.Add(new Item(ItemID.ShadowFlameBow) { shopCustomPrice = (int)Math.Round(20000 / 0.17) }, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
			shop.Add(new Item(ItemID.ShadowFlameKnife) { shopCustomPrice = (int)Math.Round(20000 / 0.17) }, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);

			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
			{
				NPCHelper.SafelySetCrossModItem(fargosMutant, "ShadowflameIcon", npcString, 0.01f, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems); //10 gold
			}
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod2) && CalamityMod)
			{
				NPCHelper.SafelySetCrossModItem(calamityMod2, "BurningStrife", npcString, (0.33f * 5), ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(calamityMod2, "TheFirstShadowflame", npcString, (0.33f * 5), ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
			}
			if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
			{
				NPCHelper.SafelySetCrossModItem(fargosSouls, "WretchedPouch", npcString, (0.2f * 5), ShopConditions.DownedGoblinWarlock,
					ShopConditions.EternityMode(fargosSouls), ShopConditions.GoblinSellInvasionItems,
					ShopConditions.TownNPCsCrossModSupport);
			}
			if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
			{
				NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "GoblinGunnerMinionItem", npcString, (0.44f * 5), ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems); //Goblin Radio Beacon
			}
			if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
			{
				NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameBMask", npcString, 1f, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems); //Shadowflare Mask
				NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameChestplate", npcString, 1f, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems); //Shadowflare Robe
				NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameGreaves", npcString, 1f, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems); //Shadowflare Greaves
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium2) && ThoriumMod)
			{
				NPCHelper.SafelySetCrossModItem(thorium2, "ShadowPurgeCaltrop", npcString, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium2, "ShadowflameWarhorn", npcString, 0.17f, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium2, "ShadowTippedJavelin", npcString, ShopConditions.DownedGoblinWarlock, ShopConditions.GoblinSellInvasionItems);
			}
			if (ModLoader.TryGetMod("VitalityMod", out Mod vitalityMod) && VitalityMod)
			{
				NPCHelper.SafelySetCrossModItem(vitalityMod, "ShadowStone", npcString);
			}
			if (customShops.TryGetValue(NPCString.GoblinTinkerer, out List<ShopItem> value))
			{
				foreach (ShopItem set in value)
				{
					shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
				}
			}
			foreach (NPCShop.Entry entry in shop.Entries)
			{
				GoblinTinkererShopCopy.Add(entry.Item.type);
				realShop.Add(entry);
			}
			GenerateShops.GenerateDropsToAddToTheShops(NPCID.GoblinSummoner, NPCString.GoblinTinkerer, NPCID.GoblinTinkerer, [ShopConditions.GoblinSellInvasionItems]);
		}
		#endregion

		#region Pirate

		internal static List<int> PirateShopCopy = [];

		/// <summary>
		/// Pirate's extra shop. These shop items are affected by the shop price scaling config.
		/// </summary>
		/// <param name="realShop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		public static void Pirate(NPCShop realShop)
		{
			NPCShop shop = new(NPCID.Pirate, "ShopBossesAsNPCs");

			string npcString = NPCString.Pirate;

			shop.Add(new Item(ItemID.PirateMap) { shopCustomPrice = 50000 }, ShopConditions.PirateSellInvasionItems); //Made up value

			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				NPCHelper.SafelySetCrossModItem(fargosMutant, "PirateFlag", npcString, 150000, ShopConditions.PirateSellInvasionItems); //Match the Deviantt's shop
			}
			//Formula: (Sell value / drop chance)
			shop.Add(NPCHelper.ItemWithPrice(ItemID.CoinGun, 0.02), ShopConditions.PirateSellInvasionItems);
			shop.Add(NPCHelper.ItemWithPrice(ItemID.LuckyCoin, 0.067), ShopConditions.PirateSellInvasionItems);
			shop.Add(NPCHelper.ItemWithPrice(ItemID.DiscountCard, 0.067), ShopConditions.PirateSellInvasionItems);
			shop.Add(NPCHelper.ItemWithPrice(ItemID.PirateStaff, 0.067), ShopConditions.PirateSellInvasionItems);
			shop.Add(NPCHelper.ItemWithPrice(ItemID.GoldRing, 0.067), ShopConditions.PirateSellInvasionItems);
			shop.Add(NPCHelper.ItemWithPrice(ItemID.PirateMinecart, 0.05), ShopConditions.PirateSellInvasionItems);
			shop.Add(NPCHelper.ItemWithPrice(ItemID.Cutlass, 0.1), ShopConditions.PirateSellInvasionItems);
			// #145: Barrel Launcher 10%
			shop.Add(NPCHelper.ItemWithPrice(ItemID.FlyingDutchmanTrophy, 0.1), ShopConditions.PirateSellInvasionItems);

			shop.Add(NPCHelper.ItemWithPrice(ItemID.PirateShipMountItem, 0.25), ShopConditions.Master, ShopConditions.PirateSellInvasionItems); //Black Spot
			shop.Add(NPCHelper.ItemWithPrice(ItemID.FlyingDutchmanMasterTrophy, priceMulti: 5), ShopConditions.Master, ShopConditions.PirateSellInvasionItems);

			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && CalamityMod)
			{
				NPCHelper.SafelySetCrossModItem(calamity, "MidasPrime", npcString, (0.04f * 5), ShopConditions.PirateSellInvasionItems);
			}
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant2) && ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && Fargowiltas && FargowiltasSouls)
			{
				NPCHelper.SafelySetCrossModItem(fargosMutant2, "GoldenDippingVat", npcString, (0.07f * 5),
					ShopConditions.EternityMode(fargosSouls), ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(fargosSouls, "SecurityWallet", npcString, (0.1f * 5),
					ShopConditions.EternityMode(fargosSouls), ShopConditions.PirateSellInvasionItems);
			}
			if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
			{
				NPCHelper.SafelySetCrossModItem(clickerClass, "CaptainsClicker", npcString, (0.125f * 5), ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(clickerClass, "GoldenTicket", npcString, (0.25f * 5), ShopConditions.PirateSellInvasionItems);
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
			{
				NPCHelper.SafelySetCrossModItem(thorium, "DeadEyePatch", npcString, 0.04f, ShopConditions.PirateSellInvasionItems);
				// NPCHelper.SafelySetCrossModItem(thorium, "CaptainsPoniard", npcString, ShopConditions.PirateSellInvasionItems); Thorium already adds it
				NPCHelper.SafelySetCrossModItem(thorium, "BountyBanner", npcString, 0.1f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "Concertina", npcString, 0.15f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "TheJuggernaut", npcString, 0.2f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "ShipsHelm", npcString, 0.2f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "HandCannon", npcString, 0.2f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "DutchmansAvarice", npcString, 0.2f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "TwentyFourCaratTuba", npcString, 0.2f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "GreedfulGurdy", npcString, 0.1f, ShopConditions.PirateSellInvasionItems);
				NPCHelper.SafelySetCrossModItem(thorium, "GreedyMagnet", npcString, 0.1f, ShopConditions.PirateSellInvasionItems);
			}
			if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
			{
				NPCHelper.SafelySetCrossModItem(orchidMod, "PirateWarhammer", npcString);
				NPCHelper.SafelySetCrossModItem(orchidMod, "PirateStandard", npcString);
			}
			if (ModLoader.TryGetMod("SOTS", out Mod secretsOfTheShadows) && SOTS)
			{
				NPCHelper.SafelySetCrossModItem(secretsOfTheShadows, "Chocolate", npcString);
			}
			if (customShops.TryGetValue(NPCString.Pirate, out List<ShopItem> value))
			{
				foreach (ShopItem set in value)
				{
					shop.Add(new Item(set.ItemType) { shopCustomPrice = set.Price }, set.Condition.ToArray());
				}
			}
			foreach (NPCShop.Entry entry in shop.Entries)
			{
				PirateShopCopy.Add(entry.Item.type);
				realShop.Add(entry);
			}
			GenerateShops.GenerateDropsToAddToTheShops(NPCID.PirateCaptain, NPCString.Pirate, NPCID.Pirate, [ShopConditions.PirateSellInvasionItems]);
			GenerateShops.GenerateDropsToAddToTheShops(NPCID.PirateShipCannon, NPCString.Pirate, NPCID.Pirate, [ShopConditions.PirateSellInvasionItems]);
		}
		#endregion
	}
}