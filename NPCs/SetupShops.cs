using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossesAsNPCs.NPCs.TownNPCs;
using Terraria.Localization;
using Terraria.GameContent.Animations;

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
		// string is the NPC name
		// int is the item
		// object[0] is the price (int)
		// object[1] is the condition function (List<Condition>)
		private static Dictionary<string, Dictionary<int, object[]>> customShops = new()
		{
			{ NPCString.KingSlime, new Dictionary<int, object[]> { } },
			{ NPCString.EyeOfCthulhu, new Dictionary<int, object[]> { } },
			{ NPCString.EaterOfWorlds, new Dictionary<int, object[]> { } },
			{ NPCString.BrainOfCthulhu, new Dictionary<int, object[]> { } },
			{ NPCString.QueenBee, new Dictionary<int, object[]> { } },
			{ NPCString.Skeletron, new Dictionary<int, object[]> { } },
			{ NPCString.Deerclops, new Dictionary<int, object[]> { } },
			{ NPCString.WallOfFlesh, new Dictionary<int, object[]> { } },
			{ NPCString.QueenSlime, new Dictionary<int, object[]> { } },
			{ NPCString.TheDestroyer, new Dictionary<int, object[]> { } },
			{ NPCString.Retinazer, new Dictionary<int, object[]> { } },
			{ NPCString.Spazmatism, new Dictionary<int, object[]> { } },
			{ NPCString.SkeletronPrime, new Dictionary<int, object[]> { } },
			{ NPCString.Plantera, new Dictionary<int, object[]> { } },
			{ NPCString.Golem, new Dictionary<int, object[]> { } },
			{ NPCString.EmpressOfLight, new Dictionary<int, object[]> { } },
			{ NPCString.DukeFishron, new Dictionary<int, object[]> { } },
			{ NPCString.Betsy, new Dictionary<int, object[]> { } },
			{ NPCString.LunaticCultist, new Dictionary<int, object[]> { } },
			{ NPCString.MoonLord, new Dictionary<int, object[]> { } },
			{ NPCString.Dreadnautilus, new Dictionary<int, object[]> { } },
			{ NPCString.Mothron, new Dictionary<int, object[]> { } },
			{ NPCString.Pumpking, new Dictionary<int, object[]> { } },
			{ NPCString.IceQueen, new Dictionary<int, object[]> { } },
			{ NPCString.MartianSaucer, new Dictionary<int, object[]> { } },
			{ NPCString.GoblinTinkerer, new Dictionary<int, object[]> { } },
			{ NPCString.Pirate, new Dictionary<int, object[]> { } }
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
		public static void AddToCustomShops(string npc, int item, int price, List<Condition> condition) => customShops[npc].Add(item, new object[] { price, condition });

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
			if (item > ItemLoader.ItemCount)
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("Cross mod SetShopItem(): Item type ID \"{0}\" exceeded the number of loaded items!", item);
				return false;
			}
			if (!CheckIfValidNPCName(npc))
			{
				return false;
			}

			AdjustConditions(ref condition);

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
			if (item > ItemLoader.ItemCount)
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("Cross mod SetShopItem(): Item type ID \"{0}\" exceeded the number of loaded items!", item);
				return false;
			}
			if (!CheckIfValidNPCName(npc))
			{
				return false;
			}

			AdjustConditions(ref condition);

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
			if (item > ItemLoader.ItemCount)
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("Cross mod SetShopItem(): Item type ID \"{0}\" exceeded the number of loaded items!", item);
				return false;
			}
			if (!CheckIfValidNPCName(npc))
			{
				return false;
			}

			AdjustConditions(ref condition);

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
			if (item > ItemLoader.ItemCount)
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("Cross mod SetShopItem(): Item type ID \"{0}\" exceeded the number of loaded items!", item);
				return false;
			}
			if (!CheckIfValidNPCName(npc))
			{
				return false;
			}

			AdjustConditions(ref condition);

			AddToCustomShops(npc, item, (int)Math.Round(CalcItemValue(item) / priceDiv * priceMulti), condition);
			return true;
		}

		/// <summary>
		/// Checks to see if the string matches one of the NPCs.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <returns>True if a match is found.</returns>
		public static bool CheckIfValidNPCName(string npc)
		{
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
			Item newItem = new(item);
			return newItem.value;
		}

		/// <summary>
		/// Changes the "vanilla" Expert and Master Mode conditions to the ones that include the config.
		/// Also adds cross mod support condition if it wasn't added already.
		/// </summary>
		/// <param name="condition"> Pass the condition list </param>
		public static void AdjustConditions(ref List<Condition> condition)
		{
			// Change the vanilla Expert and Master Mode conditions to the one that includes the config.
			// (You could get around this by making your own conditions, but why would you?)
			if (condition.Contains(Condition.InExpertMode))
			{
				condition.Remove(Condition.InExpertMode);
				condition.Add(ShopConditions.Expert);
			}
			if (condition.Contains(Condition.InMasterMode))
			{
				condition.Remove(Condition.InMasterMode);
				condition.Add(ShopConditions.Master);
			}
			// Add the cross mod support condition.
			if (!condition.Contains(ShopConditions.TownNPCsCrossModSupport))
			{
				condition.Add(ShopConditions.TownNPCsCrossModSupport);
			}
		}

		// If the internal support for the mod is enabled.

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

		#region King Slime
		/// <summary>
		/// King Slime's shop.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopName">The name of the shop.</param>
		public static void KingSlime(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.SlimeCrown) { shopCustomPrice = 50000 }); //Made up value since Slime Crown has no value
				shop.Add(new Item(ItemID.Solidifier) { shopCustomPrice = 20000 * 2 });
				//Formula: (Sell value / drop chance)
				shop.Add(new Item(ItemID.SlimySaddle) { shopCustomPrice = (int)Math.Round(50000 / 0.25) });
				shop.Add(new Item(ItemID.NinjaHood) { shopCustomPrice = (int)Math.Round(4000 / 0.33) });
				shop.Add(new Item(ItemID.NinjaShirt) { shopCustomPrice = (int)Math.Round(4000 / 0.33) });
				shop.Add(new Item(ItemID.NinjaPants) { shopCustomPrice = (int)Math.Round(4000 / 0.33) });
				shop.Add(new Item(ItemID.SlimeHook) { shopCustomPrice = (int)Math.Round(4000 / 0.33) });
				shop.Add(new Item(ItemID.SlimeGun) { shopCustomPrice = (int)Math.Round(3000 / 0.67) });
				shop.Add(new Item(ItemID.KingSlimeMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.KingSlimeTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.RoyalGel) { shopCustomPrice = 10000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.KingSlimePetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master); //Royal Delight
				shop.Add(new Item(ItemID.KingSlimeMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.Gel) { shopCustomPrice = 1 * 10 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.SlimeStaff) { shopCustomPrice = 20000 * 10 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PrinceUniform) { shopCustomPrice = 500000 }, Condition.NpcIsPresent(NPCID.Princess), ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PrincePants) { shopCustomPrice = 500000 }, Condition.NpcIsPresent(NPCID.Princess), ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PrinceCape) { shopCustomPrice = 500000 }, Condition.NpcIsPresent(NPCID.Princess), ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeGloves>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.KingSlime.KSAltCostumeGloves>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SlimyCrown", shop, 50000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeKingSlime", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CrownJewel", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimeKingsSlasher", shop, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "MedallionoftheFallenKing", shop, 0.01f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimyShield", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "KingSlimeFlask", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "KingSlimeCard", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "Gelthrower", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "TechniqueHiddenBlade", shop, 0.5f);
					NPCHelper.SafelySetCrossModItem(thorium, "ShinobiSlicer", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "GelGlove", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("StarlightRiver", out Mod starlightRiver) && StarlightRiver)
				{
					NPCHelper.SafelySetCrossModItem(starlightRiver, "Gelatine", shop, 5000); // No value
				}
				if (customShops.ContainsKey(NPCString.KingSlime))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.KingSlime])
					{
						// set.Value[0] is the price (int)
						// set.Value[1] is the condition (List<Condition>)
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.SuspiciousLookingEye) { shopCustomPrice = 75000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.DemoniteOre) { shopCustomPrice = 1000 * 5 }, ShopConditions.CorruptionOrHardmode);
				shop.Add(new Item(ItemID.CorruptSeeds) { shopCustomPrice = 500 * 5 }, ShopConditions.CorruptionOrHardmode);
				// In a Corruption World, in Hardmode, after defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 }, Condition.CorruptWorld, Condition.Hardmode, Condition.DownedEowOrBoc);
				// In a Corruption World, in Pre-HardMode, after defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 * 2 }, Condition.CorruptWorld, Condition.PreHardmode, Condition.DownedEowOrBoc);
				// In a Corruption World, before defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 * 5 }, ShopConditions.CorruptionOrHardmode, Condition.NotDownedEowOrBoc);

				shop.Add(new Item(ItemID.CrimtaneOre) { shopCustomPrice = 1300 * 5 }, ShopConditions.CrimsonOrHardmode);
				shop.Add(new Item(ItemID.CrimsonSeeds) { shopCustomPrice = 500 * 5 }, ShopConditions.CrimsonOrHardmode);

				shop.Add(new Item(ItemID.Binoculars) { shopCustomPrice = (int)Math.Round(30000 / 0.03) }); //Formula: (Sell value * 3 / drop chance))

				shop.Add(new Item(ItemID.BadgersHat) { shopCustomPrice = 3000 * 20 }, Condition.NpcIsPresent(ModContent.NPCType<WallOfFlesh>()));

				shop.Add(new Item(ItemID.EyeMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.EyeofCthulhuTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.EoCShield) { shopCustomPrice = 10000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.AviatorSunglasses) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);
				shop.Add(new Item(ItemID.EyeOfCthulhuPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.EyeofCthulhuMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousEye", shop, 80000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeEyeofCthulhu", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DeathstareRod", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "TeardropCleaver", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "LeashOfCthulhu", shop, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "AgitatingLens", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeSword", shop, 0.25f); //Eye Sored
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeGun", shop, 0.25f); //Eye Rifle
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeStaff", shop, 0.25f); //The Eyestalk
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeMinion", shop, 0.25f); //Eyeball Staff
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeHook", shop, 0.25f); //Eyeball Hook
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "EyeCard", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "Eyeruption", shop, 1f, 5f);
				}
				if (customShops.ContainsKey(NPCString.EyeOfCthulhu))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.EyeOfCthulhu])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.WormFood) { shopCustomPrice = 100000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.DemoniteOre) { shopCustomPrice = 1000 * 5 });
				shop.Add(new Item(ItemID.ShadowScale) { shopCustomPrice = 100 * 5 });
				//Formula: (Sell value / drop chance))
				shop.Add(new Item(ItemID.EatersBone) { shopCustomPrice = (int)Math.Round(75000 / 0.05) });
				shop.Add(new Item(ItemID.EaterMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.EaterofWorldsTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.WormScarf) { shopCustomPrice = 20000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.EaterOfWorldsPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.EaterofWorldsMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.VilePowder) { shopCustomPrice = 100 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.UnholyWater) { shopCustomPrice = 100 }, Condition.Hardmode, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PurpleSolution) { shopCustomPrice = 2500 }, Condition.DownedMechBossAny, Condition.NpcIsPresent(NPCID.Steampunker), ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.WormTooth) { shopCustomPrice = 20 * 5 }, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "WormyFood", shop, 100000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeEaterofWorlds", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeCorruption", shop, 10000);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "EaterStaff", shop, 0.1f); //Eater of Worlds Staff
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DarkenedHeart", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "EaterCard", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "PreservedCorruption", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "ConsumptionCannon", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "EaterOfPain", shop, 0.33f);
				}
				if (customShops.ContainsKey(NPCString.EaterOfWorlds))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.EaterOfWorlds])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.BloodySpine) { shopCustomPrice = 100000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.CrimtaneOre) { shopCustomPrice = 1300 * 5 });
				shop.Add(new Item(ItemID.TissueSample) { shopCustomPrice = 150 * 5 });
				//Formula: (Sell value / drop chance))
				shop.Add(new Item(ItemID.BoneRattle) { shopCustomPrice = (int)Math.Round(75000 / 0.05) });
				shop.Add(new Item(ItemID.BrainMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.BrainofCthulhuTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.BrainOfConfusion) { shopCustomPrice = 20000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.BrainOfCthulhuPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.BrainofCthulhuMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss3) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.ViciousPowder) { shopCustomPrice = 100 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.BloodWater) { shopCustomPrice = 100 }, Condition.Hardmode, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.RedSolution) { shopCustomPrice = 2500 }, Condition.DownedMechBossAny, Condition.NpcIsPresent(NPCID.Steampunker), ShopConditions.SellExtraItems);

				if (ModLoader.TryGetMod("RijamsMod", out Mod rijamsMod)) // It's my mod lol
				{
					NPCHelper.SafelySetCrossModItem(rijamsMod, "CrawlerChelicera", shop);
				}

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "GoreySpine", shop, 100000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeBrainofCthulhu", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeCrimson", shop, 10000);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "BrainStaff", shop, 0.1f); //Mind Break
					NPCHelper.SafelySetCrossModItem(fargosSouls, "CrimetroidEgg", shop, 0.04f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "GuttedHeart", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "BrainCard", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "PreservedCrimson", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "NeuralBasher", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "TheStalker", shop, 0.33f);
				}
				if (customShops.ContainsKey(NPCString.BrainOfCthulhu))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.BrainOfCthulhu])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.Abeemination) { shopCustomPrice = 125000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.BeeGun) { shopCustomPrice = (int)Math.Round(20000 / 0.33) });
				shop.Add(new Item(ItemID.BeeKeeper) { shopCustomPrice = (int)Math.Round(20000 / 0.33) });
				shop.Add(new Item(ItemID.BeesKnees) { shopCustomPrice = (int)Math.Round(20000 / 0.33) });
				shop.Add(new Item(ItemID.HiveWand) { shopCustomPrice = (int)Math.Round(5000 / 0.33) });
				shop.Add(new Item(ItemID.BeeHat) { shopCustomPrice = (int)Math.Round(5000 / 0.11) });
				shop.Add(new Item(ItemID.BeeShirt) { shopCustomPrice = (int)Math.Round(5000 / 0.11) });
				shop.Add(new Item(ItemID.BeePants) { shopCustomPrice = (int)Math.Round(5000 / 0.11) });
				shop.Add(new Item(ItemID.HoneyComb) { shopCustomPrice = (int)Math.Round(20000 / 0.33) });
				shop.Add(new Item(ItemID.Nectar) { shopCustomPrice = (int)Math.Round(30000 / 0.05) });
				shop.Add(new Item(ItemID.HoneyedGoggles) { shopCustomPrice = (int)Math.Round(50000 / 0.05) });
				shop.Add(new Item(ItemID.Beenade) { shopCustomPrice = (int)Math.Round(40 / 0.75) });
				shop.Add(new Item(ItemID.BeeWax) { shopCustomPrice = 20 * 5 });
				shop.Add(new Item(ItemID.BottledHoney) { shopCustomPrice = 8 * 5 });
				shop.Add(new Item(ItemID.BeeMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.QueenBeeTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.HiveBackpack) { shopCustomPrice = 20000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.QueenBeePetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.QueenBeeMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss5) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.Hive) { shopCustomPrice = 100 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.Stinger) { shopCustomPrice = 40 * 5 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.Bezoar) { shopCustomPrice = 20000 * 5 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.BeeHive) { shopCustomPrice = 50 * 5 }, Condition.InGraveyard, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.BeeWings) { shopCustomPrice = 80000 * 5 }, Condition.DownedMechBossAny, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "Abeemination2", shop, 150000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeQueenBee", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "HardenedHoneycomb", shop);
					NPCHelper.SafelySetCrossModItem(calamityMod, "TheBee", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TheSmallSting", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "QueenStinger", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode"))); //The Queen's Stinger
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BeeQueenMinionItem", shop, 0.44f); //Bee Queen's Crown
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "QueenBeeCard", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "HoneyDie", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "BeeSeeker", shop, 0.17f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "WaxyVial", shop, 0.17f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "QueenBeeFlask", shop, 0.17f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "RoyalOrb", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "SweetHeart", shop, 0.33f);
				}
				if (customShops.ContainsKey(NPCString.QueenBee))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.QueenBee])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.ClothierVoodooDoll) { shopCustomPrice = 130000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.SkeletronHand) { shopCustomPrice = (int)Math.Round(9000 / 0.12) });
				shop.Add(new Item(ItemID.BookofSkulls) { shopCustomPrice = (int)Math.Round(15000 / 0.11) });
				shop.Add(new Item(ItemID.ChippysCouch) { shopCustomPrice = (int)Math.Round(5000 / 0.14) });
				shop.Add(new Item(ItemID.SkeletronMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.SkeletronTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.BoneGlove) { shopCustomPrice = 20000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.SkeletronPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.SkeletronMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss5) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.BoneKey) { shopCustomPrice = 50000 * 5 }, ShopConditions.DownedDungeonGuardian, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.BoneWand) { shopCustomPrice = 50 * 5 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.Bone) { shopCustomPrice = 10 * 5 }, ShopConditions.SellExtraItems);

				if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC)) // I'll leave this here because it's a vanilla item and it's my mod.
				{
					if (fishermanNPC.TryFind<ModNPC>("Fisherman", out ModNPC fisherman))
					{
						shop.Add(new Item(ItemID.LockBox) { shopCustomPrice = 4000 * 5 }, Condition.NpcIsPresent(fisherman.Type));
					}
				}

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Skeletron.SkCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousSkull", shop, 150000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeSkeletron", shop, 10000);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "BoneZone", shop, 0.1f); //The Bone Zone
					NPCHelper.SafelySetCrossModItem(fargosSouls, "NecromanticBrew", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}

				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					//Skeletal Rod of Minion Guidance
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BoneWaypointRod", shop, 100); //Normally no value
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "SquireSkullAccessory", shop, 0.65f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "SkeletronCard", shop);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "BonyBackhand", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "GuildsStaff", shop, 0.25f);
				}
				if (customShops.ContainsKey(NPCString.Skeletron))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Skeletron])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.DeerThing) { shopCustomPrice = 140000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.ChesterPetItem) { shopCustomPrice = (int)Math.Round(20000 / 0.33) }); //Eye Bone
				shop.Add(new Item(ItemID.Eyebrella) { shopCustomPrice = (int)Math.Round(5000 / 0.33) });
				shop.Add(new Item(ItemID.DontStarveShaderItem) { shopCustomPrice = (int)Math.Round(10000 / 0.33) }); //Radio Thing
				shop.Add(new Item(ItemID.DizzyHat) { shopCustomPrice = (int)Math.Round(5000 / 0.0714) }); //Dizzy's Rare Gecko Chester
				shop.Add(new Item(ItemID.PewMaticHorn) { shopCustomPrice = (int)Math.Round(15000 / 0.25) });
				shop.Add(new Item(ItemID.WeatherPain) { shopCustomPrice = (int)Math.Round(15000 / 0.25) });
				shop.Add(new Item(ItemID.HoundiusShootius) { shopCustomPrice = (int)Math.Round(15000 / 0.25) });
				shop.Add(new Item(ItemID.LucyTheAxe) { shopCustomPrice = (int)Math.Round(15000 / 0.25) });
				shop.Add(new Item(ItemID.DeerclopsMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.DeerclopsTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.BoneHelm) { shopCustomPrice = 20000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.DeerclopsPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.DeerclopsMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxDeerclops) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.AbigailsFlower) { shopCustomPrice = 500 * 5 }, Condition.InGraveyard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.BatBat) { shopCustomPrice = (int)Math.Round(2500 / 0.004) }, ShopConditions.UndergroundCavernsOrHardmode, Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.BatBat) { shopCustomPrice = (int)Math.Round(2500 / 0.01) }, ShopConditions.UndergroundCavernsOrHardmode, Condition.DontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.HamBat) { shopCustomPrice = (int)Math.Round(10000 / 0.04) }, ShopConditions.InIceAndHallowOrCorruptionOrCrimson, Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.HamBat) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.InIceAndHallowOrCorruptionOrCrimson, Condition.DontStarveWorld, ShopConditions.SellExtraItems);
				//Monster Meat
				shop.Add(new Item(ItemID.PigPetItem) { shopCustomPrice = (int)Math.Round(10000 / 0.001) }, Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PigPetItem) { shopCustomPrice = (int)Math.Round(10000 / 0.005) }, Condition.DontStarveWorld, ShopConditions.SellExtraItems);
				//Glommer's Flower
				shop.Add(new Item(ItemID.GlommerPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.01) }, Condition.NotDontStarveWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.GlommerPetItem) { shopCustomPrice = (int)Math.Round(10000 / 0.025) }, Condition.DontStarveWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.PaintingWendy) { shopCustomPrice = 10000 * 5 }, Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PaintingWillow) { shopCustomPrice = 10000 * 5 }, Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PaintingWilson) { shopCustomPrice = 10000 * 5 }, Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.PaintingWolfgang) { shopCustomPrice = 10000 * 5 }, Condition.NpcIsPresent(NPCID.TravellingMerchant), ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "DeerThing2", shop, 120000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "Deerclawps", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DeerSinew", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "CyclopsClicker", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("StarlightRiver", out Mod starlightRiver) && StarlightRiver)
				{
					NPCHelper.SafelySetCrossModItem(starlightRiver, "HungryStomach", shop);
				}
				if (customShops.ContainsKey(NPCString.Deerclops))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Deerclops])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
		public static void WallOfFlesh(NPCShop shop, string shopName)
		{
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.GuideVoodooDoll) { shopCustomPrice = 150000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.Pwnhammer) { shopCustomPrice = 7800 * 5 });
				shop.Add(new Item(ItemID.BreakerBlade) { shopCustomPrice = (int)Math.Round(30000 / 0.13) });
				shop.Add(new Item(ItemID.ClockworkAssaultRifle) { shopCustomPrice = (int)Math.Round(30000 / 0.13) });
				shop.Add(new Item(ItemID.LaserRifle) { shopCustomPrice = (int)Math.Round(30000 / 0.13) });
				shop.Add(new Item(ItemID.FireWhip) { shopCustomPrice = (int)Math.Round(30000 / 0.13) }); //Firecracker
				shop.Add(new Item(ItemID.WarriorEmblem) { shopCustomPrice = (int)Math.Round(20000 / 0.13) });
				shop.Add(new Item(ItemID.RangerEmblem) { shopCustomPrice = (int)Math.Round(20000 / 0.13) });
				shop.Add(new Item(ItemID.SorcererEmblem) { shopCustomPrice = (int)Math.Round(20000 / 0.13) });
				shop.Add(new Item(ItemID.SummonerEmblem) { shopCustomPrice = (int)Math.Round(20000 / 0.13) });
				shop.Add(new Item(ItemID.BadgersHat) { shopCustomPrice = 3000 * 20 }, Condition.NpcIsPresent(ModContent.NPCType<EyeOfCthulhu>()));

				shop.Add(new Item(ItemID.FleshMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.WallofFleshTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.DemonHeart) { shopCustomPrice = 20000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.WallOfFleshGoatMountItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.WallofFleshMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWWallOfFlesh) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.DemoniteBrick) { shopCustomPrice = 1500 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.CrimtaneBrick) { shopCustomPrice = 1500 }, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBackpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "FleshyDoll", shop, 200000);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeWallofFlesh", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeUnderworld", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Carnage", shop, 0.1f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlackHawkRemote", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlastBarrel", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Meowthrower", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "RogueEmblem", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "HermitsBoxofOneHundredMedicines", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "FleshHand", shop, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "PungentEyeball", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BoneSerpentMinionItem", shop, 0.35f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "ShamanEmblem", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "OrchidEmblem", shop);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "MawOfFlesh", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BurningSuperDeathClicker", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(clickerClass, "ClickerEmblem", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "NinjaEmblem", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "ClericEmblem", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "BardEmblem", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "ShatteredDisk", shop, 10000); // No value
				}
				if (customShops.ContainsKey(NPCString.WallOfFlesh))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.WallOfFlesh])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.QueenSlimeCrystal) { shopCustomPrice = 200000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.QueenSlimeMountSaddle) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Gelatinous Pillion
				shop.Add(new Item(ItemID.CrystalNinjaHelmet) { shopCustomPrice = (int)Math.Round(20000 / 0.33) }); //Crystal Assassin Hood
				shop.Add(new Item(ItemID.CrystalNinjaChestplate) { shopCustomPrice = (int)Math.Round(20000 / 0.33) }); //Crystal Assassin Shirt
				shop.Add(new Item(ItemID.CrystalNinjaLeggings) { shopCustomPrice = (int)Math.Round(20000 / 0.33) }); //Crystal Assassin Pants
				shop.Add(new Item(ItemID.QueenSlimeHook) { shopCustomPrice = (int)Math.Round(5000 / 0.33) }); //Hook of Dissonance
				shop.Add(new Item(ItemID.Smolstar) { shopCustomPrice = (int)Math.Round(5000 / 0.33) }); //Blade Staff
				shop.Add(new Item(ItemID.GelBalloon) { shopCustomPrice = 40 * 5 });
				
				shop.Add(new Item(ItemID.QueenSlimeMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.QueenSlimeTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.VolatileGelatin) { shopCustomPrice = 50000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.QueenSlimePetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.QueenSlimeMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxQueenSlime) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.PinkGel) { shopCustomPrice = 3 * 10 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenSlime.QSAltCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenSlime.QSCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.QueenSlime.QSCostumeGloves>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "JellyCrystal", shop, 250000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "GelicWings", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "ClearKeychain", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "RoyalSlimePrism", shop);
				}
				if (customShops.ContainsKey(NPCString.QueenSlime))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.QueenSlime])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.MechanicalWorm) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(new Item(ItemID.HallowedBar) { shopCustomPrice = 400 * 5 });
				shop.Add(new Item(ItemID.SoulofMight) { shopCustomPrice = 800 * 5 });
				shop.Add(new Item(ItemID.WaffleIron) { shopCustomPrice = 150000 }, Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(new Item(ItemID.DestroyerMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.DestroyerTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.MechanicalWagonPiece) { shopCustomPrice = 5000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.DestroyerPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.DestroyerMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss3) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechWorm", shop, 400000);

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, 1000000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeDestroyer", shop, 10000);

					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, 10000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DestroyerGun", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "GroundStick", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ShopConditions.Expert);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechTail", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", shop);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "MechanicalPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.TheDestroyer))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.TheDestroyer])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.MechanicalEye) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(new Item(ItemID.HallowedBar) { shopCustomPrice = 400 * 5 });
				shop.Add(new Item(ItemID.SoulofSight) { shopCustomPrice = 800 * 5 });
				shop.Add(new Item(ItemID.WaffleIron) { shopCustomPrice = 150000 }, Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(new Item(ItemID.TwinMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.RetinazerTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.MechanicalWheelPiece) { shopCustomPrice = 5000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.TwinsPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.TwinsMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Retinazer.RetCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Retinazer.RetCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechEye", shop, 400000); //Match the Mutant's shop

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, 1000000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeTwins", shop, 10000);

					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, 10000, Condition.DownedMechBossAll);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Arbalest", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TwinRangs", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "FusedLens", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ShopConditions.Expert); //Mechanical Spikes
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechMask", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", shop);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "MechanicalPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.Retinazer))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Retinazer])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.MechanicalEye) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(new Item(ItemID.HallowedBar) { shopCustomPrice = 400 * 5 });
				shop.Add(new Item(ItemID.SoulofSight) { shopCustomPrice = 800 * 5 });
				shop.Add(new Item(ItemID.WaffleIron) { shopCustomPrice = 150000 }, Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(new Item(ItemID.TwinMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.SpazmatismTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.MechanicalWheelPiece) { shopCustomPrice = 5000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.TwinsPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.TwinsMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Spazmatism.SpazCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Spazmatism.SpazCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechEye", shop, 400000);

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, 1000000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeTwins", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, 10000, Condition.DownedMechBossAll);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Arbalest", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TwinRangs", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "FusedLens", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ShopConditions.Expert); //Mechanical Spikes
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechMask", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", shop);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "MechanicalPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.Spazmatism))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Spazmatism])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.MechanicalSkull) { shopCustomPrice = 250000 }); //Made up value since it has no value
				shop.Add(new Item(ItemID.MechdusaSummon) { shopCustomPrice = 1000000 }, Condition.DownedMechBossAll, Condition.ZenithWorld); // Ocram's Razor
				shop.Add(new Item(ItemID.HallowedBar) { shopCustomPrice = 400 * 5 });
				shop.Add(new Item(ItemID.SoulofFright) { shopCustomPrice = 800 * 5 });
				shop.Add(new Item(ItemID.WaffleIron) { shopCustomPrice = 150000 }, Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(new Item(ItemID.SkeletronPrimeMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.SkeletronPrimeTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.MechanicalBatteryPiece) { shopCustomPrice = 5000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.SkeletronPrimePetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.SkeletronPrimeMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss1) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant)  && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechSkull", shop, 400000);

					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, 1000000, Condition.DownedMechBossAll);
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeSkeletronPrime", shop, 10000);

					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, 10000, Condition.DownedMechBossAll);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "RefractorBlaster", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "ReinforcedPlating", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ShopConditions.Expert);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "FlawlessMechChestplate", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "BottomlessBoxofPaperclips", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "StrangePlating", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeCell", shop);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "MechanicalPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.SkeletronPrime))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.SkeletronPrime])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.TempleKey) { shopCustomPrice = 5000 }); //Made up value
				shop.Add(new Item(ItemID.GrenadeLauncher) { shopCustomPrice = (int)Math.Round(70000 / 0.14) });
				shop.Add(new Item(ItemID.VenusMagnum) { shopCustomPrice = (int)Math.Round(50000 / 0.14) });
				shop.Add(new Item(ItemID.NettleBurst) { shopCustomPrice = (int)Math.Round(40000 / 0.14) });
				shop.Add(new Item(ItemID.LeafBlower) { shopCustomPrice = (int)Math.Round(60000 / 0.14) });
				shop.Add(new Item(ItemID.FlowerPow) { shopCustomPrice = (int)Math.Round(60000 / 0.14) });
				shop.Add(new Item(ItemID.WaspGun) { shopCustomPrice = (int)Math.Round(100000 / 0.14) });
				shop.Add(new Item(ItemID.Seedler) { shopCustomPrice = (int)Math.Round(100000 / 0.14) });
				shop.Add(new Item(ItemID.PygmyStaff) { shopCustomPrice = (int)Math.Round(70000 / 0.25) });
				shop.Add(new Item(ItemID.ThornHook) { shopCustomPrice = (int)Math.Round(60000 / 0.1) });
				shop.Add(new Item(ItemID.TheAxe) { shopCustomPrice = (int)Math.Round(100000 / 0.02) });
				shop.Add(new Item(ItemID.Seedling) { shopCustomPrice = (int)Math.Round(20000 / 0.05) });
				shop.Add(new Item(ItemID.Seedling) { shopCustomPrice = (int)Math.Round(20000 / 0.05) });

				shop.Add(new Item(ItemID.PlanteraMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.PlanteraTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.SporeSac) { shopCustomPrice = 40000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.PlanteraPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.PlanteraMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxPlantera) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWPlantera) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.JungleGrassSeeds) { shopCustomPrice = 30 * 5 }, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeBackpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "PlanterasFruit", shop, 500000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgePlantera", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "LivingShard", shop);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BloomStone", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlossomFlux", shop, 0.1f);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "Dicer", shop, 0.1f); //The Dicer

					NPCHelper.SafelySetCrossModItem(fargosSouls, "MagicalBulb", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "PottedPalMinionItem", shop, 0.44f); //Potted Pal
				}
				if (ModLoader.TryGetMod("QwertyMod", out Mod qwertyMod) && QwertyMod)
				{
					NPCHelper.SafelySetCrossModItem(qwertyMod, "VitallumCoreUncharged", shop); //Vitallum Core
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "BulbScepter", shop, 0.66f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "FloralStinger", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "JunglesRage", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(polarities, "UnfoldingBlossom", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "BloomWeave", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "BudBomb", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaRed", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaGreen", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaYellow", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VuvuzelaBlue", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "VerdantOrnament", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "OvergrownPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.Plantera))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Plantera])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.LihzahrdPowerCell) { shopCustomPrice = 350000 }); //Made up value
				shop.Add(new Item(ItemID.Picksaw) { shopCustomPrice = (int)Math.Round(43200 / 0.25) });
				shop.Add(new Item(ItemID.BeetleHusk) { shopCustomPrice = 5000 * 5 });
				shop.Add(new Item(ItemID.Stynger) { shopCustomPrice = (int)Math.Round(70000 / 0.14) });
				shop.Add(new Item(ItemID.StyngerBolt) { shopCustomPrice = 75 });
				shop.Add(new Item(ItemID.PossessedHatchet) { shopCustomPrice = (int)Math.Round(70000 / 0.14) });
				shop.Add(new Item(ItemID.SunStone) { shopCustomPrice = (int)Math.Round(60000 / 0.14) });
				shop.Add(new Item(ItemID.EyeoftheGolem) { shopCustomPrice = (int)Math.Round(70000 / 0.14) });
				shop.Add(new Item(ItemID.StaffofEarth) { shopCustomPrice = (int)Math.Round(70000 / 0.14) });
				shop.Add(new Item(ItemID.GolemFist) { shopCustomPrice = (int)Math.Round(70000 / 0.14) });

				shop.Add(new Item(ItemID.GolemMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.GolemTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.ShinyStone) { shopCustomPrice = 50000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.GolemPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.GolemMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss4) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.LihzahrdBrick) { shopCustomPrice = 2500 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.LihzahrdAltar) { shopCustomPrice = 60 * 5 * 1000 }, ShopConditions.SellExtraItems); //sells for 60 copper, but that seems way to cheap for an item that you should only have one of.

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "LihzahrdPowerCell2", shop, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeGolem", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "AegisBlade", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "RockSlide", shop, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "ComputationOrb", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "LihzahrdTreasureBox", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "SunRay", shop, 0.14f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "LihzahrdPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.Golem))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Golem])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.LihzahrdPowerCell) { shopCustomPrice = 400000 }); //Prismatic Lacewing //Sell value * 5 = 250000
				//Formula: (Sell value / drop chance); It would be 200000 in this case
				shop.Add(new Item(ItemID.FairyQueenMagicItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Nightglow
				shop.Add(new Item(ItemID.PiercingStarlight) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Starlight
				shop.Add(new Item(ItemID.RainbowWhip) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Kaleidoscope
				shop.Add(new Item(ItemID.FairyQueenRangedItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Eventide
				shop.Add(new Item(ItemID.RainbowWings) { shopCustomPrice = (int)Math.Round(80000 / 0.07) }); //Empress Wings
				shop.Add(new Item(ItemID.HallowBossDye) { shopCustomPrice = (int)Math.Round(15000 / 0.25) }); //Prismatic Dye
				shop.Add(new Item(ItemID.SparkleGuitar) { shopCustomPrice = (int)Math.Round(100000 / 0.05) }); //Stellar Tune
				shop.Add(new Item(ItemID.RainbowCursor) { shopCustomPrice = (int)Math.Round(10000 / 0.05) }); //Rainbow Cursor

				//Special case since it is technically a "100% drop chance".
				shop.Add(new Item(ItemID.EmpressBlade) { shopCustomPrice = 200000 * 50 }, ShopConditions.DaytimeEoLDefated); //Terraprisma

				shop.Add(new Item(ItemID.FairyQueenMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) }); //Empress of Light Mask
				shop.Add(new Item(ItemID.FairyQueenTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }); //Empress of Light Trophy

				shop.Add(new Item(ItemID.EmpressFlightBooster) { shopCustomPrice = 50000 * 5 }, ShopConditions.Expert); //Soaring Insignia
				shop.Add(new Item(ItemID.FairyQueenPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master); //Jewel of Light
				shop.Add(new Item(ItemID.FairyQueenMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master); //Empress of Light Relic

				shop.Add(new Item(ItemID.MusicBoxEmpressOfLight) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ItemID.HolyWater) { shopCustomPrice = 200 }, Condition.Hardmode, ShopConditions.SellExtraItems); //For some reason Holy Water is double as valuable than Unholy/Blood Water.
				shop.Add(new Item(ItemID.BlueSolution) { shopCustomPrice = 2500 }, Condition.NpcIsPresent(NPCID.Steampunker), ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeEars>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "PrismaticPrimrose", shop, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "PrecisionSeal", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "EmpressSquireMinionItem", shop, 0.34f); //Chalice of the Empress
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "RainbowClicker", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "EmpressPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.EmpressOfLight))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.EmpressOfLight])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.TruffleWorm) { shopCustomPrice = 400000 }); //Made up value
				shop.Add(new Item(ItemID.BubbleGun) { shopCustomPrice = (int)Math.Round(50000 / 0.2) });
				shop.Add(new Item(ItemID.Flairon) { shopCustomPrice = (int)Math.Round(50000 / 0.2) });
				shop.Add(new Item(ItemID.RazorbladeTyphoon) { shopCustomPrice = (int)Math.Round(50000 / 0.2) });
				shop.Add(new Item(ItemID.TempestStaff) { shopCustomPrice = (int)Math.Round(50000 / 0.2) });
				shop.Add(new Item(ItemID.Tsunami) { shopCustomPrice = (int)Math.Round(50000 / 0.2) });
				shop.Add(new Item(ItemID.FishronWings) { shopCustomPrice = (int)Math.Round(80000 / 0.07) });
				shop.Add(new Item(ItemID.FishronWings) { shopCustomPrice = (int)Math.Round(80000 / 0.07) });

				shop.Add(new Item(ItemID.DukeFishronMask) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.DukeFishronTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.ShrimpyTruffle) { shopCustomPrice = 50000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.DukeFishronPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.DukeFishronMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxDukeFishron) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "TruffleWorm2", shop, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeDukeFishron", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DukesDecapitator", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BrinyBaron", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "FishStick", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(fargosSouls, "MutantAntibodies", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("QwertyMod", out Mod qwertyMod) && QwertyMod)
				{
					NPCHelper.SafelySetCrossModItem(qwertyMod, "BubbleBrewerBaton", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(qwertyMod, "Cyclone", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(qwertyMod, "Whirlpool", shop, 0.33f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "SeafoamClicker", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "DukesRegalCarnyx", shop, 0.20f);
					NPCHelper.SafelySetCrossModItem(thorium, "Brinefang", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "SoulAnchor", shop, 0.20f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "TyphoonPrism", shop);
				}
				if (customShops.ContainsKey(NPCString.DukeFishron))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.DukeFishron])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.DD2ElderCrystal) { shopCustomPrice = 40000 });

				//Formula: (Sell value / drop chance))
				shop.Add(new Item(ItemID.ApprenticeScarf) { shopCustomPrice = (int)Math.Round(30000 / 0.25) }, ShopConditions.DownedDarkMage);
				shop.Add(new Item(ItemID.SquireShield) { shopCustomPrice = (int)Math.Round(30000 / 0.25) }, ShopConditions.DownedDarkMage);
				shop.Add(new Item(ItemID.WarTable) { shopCustomPrice = (int)Math.Round(20000 / 0.1) }, ShopConditions.DownedDarkMage);
				shop.Add(new Item(ItemID.WarTableBanner) { shopCustomPrice = (int)Math.Round(20000 / 0.1) }, ShopConditions.DownedDarkMage);
				shop.Add(new Item(ItemID.DD2PetDragon) { shopCustomPrice = (int)Math.Round(20000 / 0.17) }, ShopConditions.DownedDarkMage); //Dragon Egg
				shop.Add(new Item(ItemID.DD2PetGato) { shopCustomPrice = (int)Math.Round(20000 / 0.17) }, ShopConditions.DownedDarkMage); //Gato Egg
				shop.Add(new Item(ItemID.BossMaskDarkMage) { shopCustomPrice = (int)Math.Round(7500 / 0.14) }, ShopConditions.DownedDarkMage);
				shop.Add(new Item(ItemID.BossTrophyDarkmage) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.DownedDarkMage);
				shop.Add(new Item(ItemID.DarkMageBookMountItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.DownedDarkMage,
					ShopConditions.Master);
				shop.Add(new Item(ItemID.DarkMageMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.DownedDarkMage,
					ShopConditions.Master);

				shop.Add(new Item(ItemID.HuntressBuckler) { shopCustomPrice = (int)Math.Round(30000 / 0.17) }, ShopConditions.DownedOgre);
				shop.Add(new Item(ItemID.MonkBelt) { shopCustomPrice = (int)Math.Round(30000 / 0.17) }, ShopConditions.DownedOgre);
				shop.Add(new Item(ItemID.BookStaff) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.DownedOgre); //Tome of Infinite Wisdom
				shop.Add(new Item(ItemID.DD2PhoenixBow) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.DownedOgre); //Phantom Phoenix
				shop.Add(new Item(ItemID.DD2SquireDemonSword) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.DownedOgre); //Brand of the Inferno
				shop.Add(new Item(ItemID.MonkStaffT1) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.DownedOgre); //Sleepy Octopod
				shop.Add(new Item(ItemID.MonkStaffT2) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.DownedOgre); //Ghastly Glaive
				shop.Add(new Item(ItemID.DD2PetGhost) { shopCustomPrice = (int)Math.Round(20000 / 0.2) }, ShopConditions.DownedOgre); //Creeper Egg
				shop.Add(new Item(ItemID.BossMaskOgre) { shopCustomPrice = (int)Math.Round(7500 / 0.14) }, ShopConditions.DownedOgre);
				shop.Add(new Item(ItemID.BossTrophyOgre) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, ShopConditions.DownedOgre);
				shop.Add(new Item(ItemID.DD2OgrePetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.DownedOgre,
					ShopConditions.Master);
				shop.Add(new Item(ItemID.OgreMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.DownedOgre,
					ShopConditions.Master);

				shop.Add(new Item(ItemID.DD2BetsyBow) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Aerial Bane
				shop.Add(new Item(ItemID.MonkStaffT3) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Sky Dragon's Fury
				shop.Add(new Item(ItemID.ApprenticeStaffT3) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Betsy's Wrath
				shop.Add(new Item(ItemID.DD2SquireBetsySword) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }); //Flying Dragon
				shop.Add(new Item(ItemID.BetsyWings) { shopCustomPrice = (int)Math.Round(80000 / 0.07) });
				
				shop.Add(new Item(ItemID.BossMaskBetsy) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.BossTrophyBetsy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.DD2BetsyPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.BetsyMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxDD2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWInvasion) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}

			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "ForbiddenTome", shop, 50000, ShopConditions.DownedDarkMage); //Match the Abominationn's shop

					NPCHelper.SafelySetCrossModItem(fargosMutant, "BatteredClub", shop, 150000, ShopConditions.DownedOgre); //Match the Abominationn's shop

					NPCHelper.SafelySetCrossModItem(fargosMutant, "BetsyEgg", shop, 400000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DragonBreath", shop, 0.1f); //Dragon's Breath

					NPCHelper.SafelySetCrossModItem(fargosSouls, "BetsysHeart", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode"))); //Betsy's Heart
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) && EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "BetsyScale", shop);
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "FlameCore", shop, ShopConditions.Expert); //Betsy's Flame
				}
				if (ModLoader.TryGetMod("PboneUtils", out Mod pbonesUtilities) && PboneUtils)
				{
					NPCHelper.SafelySetCrossModItem(pbonesUtilities, "DefendersCrystal", shop);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "WyvernsNest", shop, 1f, 5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "ArcaneClicker", shop, 0.20f, ShopConditions.DownedDarkMage);
					NPCHelper.SafelySetCrossModItem(clickerClass, "SnottyClicker", shop, 0.20f, ShopConditions.DownedOgre);
					NPCHelper.SafelySetCrossModItem(clickerClass, "DraconicClicker", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "DragonFang", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "DragonHeartWand", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "BetsysBellow", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "ValhallasDescent", shop, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(thorium, "MediumRareSteak", shop, 1f, 5f);
				}
				if (customShops.ContainsKey(NPCString.Betsy))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Betsy])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.LunarCraftingStation) { shopCustomPrice = 100000 }); //Ancient Manipulator //Made up value
				shop.Add(new Item(ItemID.FragmentSolar) { shopCustomPrice = 2000 * 10 }, Condition.DownedSolarPillar);
				shop.Add(new Item(ItemID.FragmentVortex) { shopCustomPrice = 2000 * 10 }, Condition.DownedSolarPillar);
				shop.Add(new Item(ItemID.FragmentNebula) { shopCustomPrice = 2000 * 10 }, Condition.DownedSolarPillar);
				shop.Add(new Item(ItemID.FragmentStardust) { shopCustomPrice = 2000 * 10 }, Condition.DownedSolarPillar);

				shop.Add(new Item(ItemID.BossMaskCultist) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.AncientCultistTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.LunaticCultistPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.LunaticCultistMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxBoss4) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBoss2) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CultistSummon", shop, 750000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeLunaticCultist", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeBloodMoon", shop, 10000, Condition.BloodMoon);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "CelestialRune", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
					NPCHelper.SafelySetCrossModItem(fargosSouls, "MutantsPact", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode"))); //Mutant's Pact
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistLazor", shop, 0.02f); //Mysterious Cultist Hood
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistBow", shop, 0.25f); //Lunatic Bow of Ice
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistSpear", shop, 0.25f); //Lunatic Spear of Fire
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistTome", shop, 0.25f); //Lunatic Spell of Ancient Light
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistStaff", shop, 0.25f); //Lunatic Staff of Lightning
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunaticHood", shop, ShopConditions.Expert);  //Lunatic Hood of Command
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarSelenianBlade", shop, 0.05f);  // Selenian Blade // Made slightly cheaper than 2%
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarVortexShotgun", shop, 0.05f);  // Storm Diver Shotgun
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarPredictorBrain", shop, 0.05f);  // Predictor Brain
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunarStargazerLaser", shop, 0.05f);  // Stargazer Core

					Condition randomVanity(int tick) => new("Mods.BossesAsNPCs.Conditions.RandomVanityS", () => Main.GameUpdateCount % 4 == tick);

					// Randomly choose a vantiy set every time the shop is opened.

					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SelenianBMask", shop, 0.05f, randomVanity(0));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SelenianBody", shop, 0.05f, randomVanity(0));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SelenianLegs", shop, 0.05f, randomVanity(0));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StormDiverBMask", shop, 0.05f, randomVanity(1));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StormDiverBody", shop, 0.05f, randomVanity(1));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StormDiverLegs", shop, 0.05f, randomVanity(1));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PredictorBMask", shop, 0.05f, randomVanity(2));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PredictorBody", shop, 0.05f, randomVanity(2));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "PredictorLegs", shop, 0.05f, randomVanity(2));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StargazerBMask", shop, 0.05f, randomVanity(3));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StargazerBody", shop, 0.05f, randomVanity(3));
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "StargazerLegs", shop, 0.05f, randomVanity(3));
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) && EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "LunarSilk", shop);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "AbyssFragment", shop, 1f, 2f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "MiceFragment", shop, 1f, 2f, ShopConditions.DownedAnyPillar);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "WhiteDwarfFragment", shop, ShopConditions.DownedAllPillars);
					NPCHelper.SafelySetCrossModItem(thorium, "CelestialFragment", shop, ShopConditions.DownedAllPillars);
					NPCHelper.SafelySetCrossModItem(thorium, "ShootingStarFragment", shop, ShopConditions.DownedAllPillars);

					NPCHelper.SafelySetCrossModItem(thorium, "AncientFlame", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "AncientSpark", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "AncientFrost", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "AstralFang", shop, 0.33f);
					NPCHelper.SafelySetCrossModItem(thorium, "CosmicFluxStaff", shop, 0.33f);
				}
				if (customShops.ContainsKey(NPCString.LunaticCultist))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.LunaticCultist])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.CelestialSigil) { shopCustomPrice = 500000 });
				shop.Add(new Item(ItemID.PortalGun) { shopCustomPrice = 100000 * 5 });
				shop.Add(new Item(ItemID.LunarOre) { shopCustomPrice = 3000 * 5 });
				shop.Add(new Item(ItemID.Meowmere) { shopCustomPrice = (int)Math.Round(200000 / 0.22) });
				shop.Add(new Item(ItemID.Terrarian) { shopCustomPrice = (int)Math.Round(100000 / 0.22) });
				shop.Add(new Item(ItemID.StarWrath) { shopCustomPrice = (int)Math.Round(200000 / 0.22) });
				shop.Add(new Item(ItemID.SDMG) { shopCustomPrice = (int)Math.Round(150000 / 0.22) });
				shop.Add(new Item(ItemID.LastPrism) { shopCustomPrice = (int)Math.Round(100000 / 0.22) });
				shop.Add(new Item(ItemID.LunarFlareBook) { shopCustomPrice = (int)Math.Round(100000 / 0.22) });
				shop.Add(new Item(ItemID.RainbowCrystalStaff) { shopCustomPrice = (int)Math.Round(100000 / 0.22) });
				shop.Add(new Item(ItemID.MoonlordTurretStaff) { shopCustomPrice = (int)Math.Round(100000 / 0.22) }); //Lunar Portal Staff
				shop.Add(new Item(ItemID.Celeb2) { shopCustomPrice = (int)Math.Round(100000 / 0.22) }); //Celebration Mk2
				shop.Add(new Item(ItemID.MeowmereMinecart) { shopCustomPrice = (int)Math.Round(100000 / 0.1) });

				shop.Add(new Item(ItemID.BossMaskMoonlord) { shopCustomPrice = (int)Math.Round(7500 / 0.14) });
				shop.Add(new Item(ItemID.MoonLordTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.GravityGlobe) { shopCustomPrice = 400000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.SuspiciousLookingTentacle) { shopCustomPrice = 10000 * 5 }, ShopConditions.Expert);
				shop.Add(new Item(ItemID.LongRainbowTrailWings) { shopCustomPrice = 10000 * 5 }, ShopConditions.Expert);

				shop.Add(new Item(ItemID.MoonLordPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.MoonLordMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxLunarBoss) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWMoonLord) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MoonLord.MLCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MoonLord.MLCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MoonLordLegs) { shopCustomPrice = 20000 * 5 }, ShopConditions.SellExtraItems);
				
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.UnlockedBiomeTorches, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.UnlockedBiomeTorches, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.UnlockedBiomeTorches, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CelestialSigil2", shop, 1000000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMoonLord", shop, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CelestialOnion", shop, 100000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "UtensilPoker", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DeviousAestheticus", shop, 0.05f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "GalacticGlobe", shop, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) && EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "TrueThirdEye", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Cosmic_Key", shop, 100000);
				}
				if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage) && MagicStorage)
				{
					NPCHelper.SafelySetCrossModItem(magicStorage, "RadiantJewel", shop, 0.05f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "Nirvana", shop, 0.5f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "TheCore", shop, 0.5f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "LordsClicker", shop);
					NPCHelper.SafelySetCrossModItem(clickerClass, "TheClicker", shop, 0.20f);
					NPCHelper.SafelySetCrossModItem(clickerClass, "TorchClicker", shop, ShopConditions.UnlockedBiomeTorches);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "AngelsEnd", shop, 0.11f);
					NPCHelper.SafelySetCrossModItem(thorium, "LifeAndDeath", shop, 0.11f);
					NPCHelper.SafelySetCrossModItem(thorium, "SonicAmplifier", shop, 0.11f);
				}
				if (ModLoader.TryGetMod("StarsAbove", out Mod theStarsAbove) && StarsAbove)
				{
					NPCHelper.SafelySetCrossModItem(theStarsAbove, "LuminitePrism", shop);
				}
				if (customShops.ContainsKey(NPCString.MoonLord))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.MoonLord])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.BloodMoonStarter) { shopCustomPrice = 60000 }); //Bloody Tear
				shop.Add(new Item(ItemID.BunnyHood) { shopCustomPrice = (int)Math.Round(4000 / 0.0133) });
				shop.Add(new Item(ItemID.PedguinHat) { shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3) });
				shop.Add(new Item(ItemID.PedguinShirt) { shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3) });
				shop.Add(new Item(ItemID.PedguinPants) { shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3) });
				shop.Add(new Item(ItemID.KiteBunnyCorrupt) { shopCustomPrice = (int)Math.Round(4000 / 0.04) });
				shop.Add(new Item(ItemID.KiteBunnyCrimson) { shopCustomPrice = (int)Math.Round(4000 / 0.04) });
				shop.Add(new Item(ItemID.TopHat) { shopCustomPrice = 2000 * 5 }); //Technically a 90% drop chance, but in certain cases you could sell the hat for more than you bought it
				shop.Add(new Item(ItemID.TheBrideHat) { shopCustomPrice = 1000 * 5 });
				shop.Add(new Item(ItemID.TheBrideDress) { shopCustomPrice = 1000 * 5 });
				shop.Add(new Item(ItemID.MoneyTrough) { shopCustomPrice = (int)Math.Round(20000 / 0.005 / 2) }); //0.5% from Blood Zombies & Dripplers. Not using the 6.67% from Zombie Merman & Wandering Eye Fish
				shop.Add(new Item(ItemID.SharkToothNecklace) { shopCustomPrice = (int)Math.Round(10000 / 0.0067 / 2) });
				shop.Add(new Item(ItemID.ChumBucket) { shopCustomPrice = 500 * 5 * 2 });
				shop.Add(new Item(ItemID.BloodRainBow) { shopCustomPrice = (int)Math.Round(10000 / 0.125) });
				shop.Add(new Item(ItemID.VampireFrogStaff) { shopCustomPrice = (int)Math.Round(10000 / 0.125) });
				shop.Add(new Item(ItemID.BloodFishingRod) { shopCustomPrice = (int)Math.Round(20000 / 0.0417) }); //Chum Caster
				shop.Add(new Item(ItemID.CombatBook) { shopCustomPrice = 500000 }, new Condition("Advanced Combat Techniques has not been used", () => !NPC.combatBookWasUsed)); //Advanced Combat Techniques

				shop.Add(new Item(ItemID.KOCannon) { shopCustomPrice = (int)Math.Round(35000 / 0.01 / 10) }, Condition.Hardmode); //Dropped by ANY enemy during a Blood Moon
				shop.Add(new Item(ItemID.Bananarang) { shopCustomPrice = (int)Math.Round(15000 / 0.0333) }, Condition.Hardmode);
				// No Trifold Map lol
				shop.Add(new Item(ItemID.BloodHamaxe) { shopCustomPrice = (int)Math.Round(20000 / 0.125) }, Condition.Hardmode); //Haemorrhaxe
				shop.Add(new Item(ItemID.SharpTears) { shopCustomPrice = (int)Math.Round(40000 / 0.125) }, Condition.Hardmode); //Blood Thorn
				shop.Add(new Item(ItemID.DripplerFlail) { shopCustomPrice = (int)Math.Round(40000 / 0.125) }, Condition.Hardmode); //Drippler Crippler
				shop.Add(new Item(ItemID.SanguineStaff) { shopCustomPrice = 50000 * 5 }, Condition.Hardmode);  //50% drop chance in normal mode, but I wanted it to be more expensive

				shop.Add(new Item(ItemID.BloodMoonMonolith) { shopCustomPrice = (int)Math.Round(10000 / 0.1111) });
				shop.Add(new Item(ItemID.DreadoftheRedSea) { shopCustomPrice = (int)Math.Round(1000 / 0.05) }, Condition.BloodMoon); // Don't actually know the odds.

				shop.Add(new Item(ItemID.MusicBoxEerie) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBloodMoon) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousLookingLure", shop, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodUrchin", shop, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "HemoclawCrab", shop, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodSushiPlatter", shop, 200000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "BloodOrb", shop, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BouncingEyeball", shop, (0.025f * 2f));
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SqueakyToy", shop, 0.1f, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DreadShell", shop, 0.2f, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodDrop", shop); //Bloody Drop
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodyRifle", shop, 0.125f); //Bloodshot Rifle
				}
				if (ModLoader.TryGetMod("ItReallyMustBe", out Mod dreadnautilusIsABoss) && ItReallyMustBe)
				{
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "FunnyBait", shop); //Blood Bait
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadPistol", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusTrophy", shop, 0.1f);

					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "BloodyCarKey", shop, 0.25f, ShopConditions.Master);
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusRelic", shop, 0.1f, ShopConditions.Master);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "BloodMoonFlask", shop, (0.025f * 2));
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "HemoClicker", shop, 0.04f * 2f);
					NPCHelper.SafelySetCrossModItem(clickerClass, "SpiralClicker", shop, 0.50f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "LuckyRabbitsFoot", shop, 0.1f);
					NPCHelper.SafelySetCrossModItem(thorium, "Blood", shop, 0.2f);
					NPCHelper.SafelySetCrossModItem(thorium, "SeveredHand", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "GraveBuster", shop, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(thorium, "GoodBook", shop, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(thorium, "BloodFeasterStaff", shop, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "BloodDrinker", shop, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "RifleSpear", shop, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "EvisceratingClaw", shop, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "BattleHorn", shop, 0.02f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "Bagpipe", shop, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "TechniqueBloodLotus", shop, 0.05f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "ShadeBand", shop, 0.1f, Condition.Hardmode);
					NPCHelper.SafelySetCrossModItem(thorium, "NecroticStaff", shop, 0.1f, Condition.Hardmode);
				}
				if (customShops.ContainsKey(NPCString.Dreadnautilus))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Dreadnautilus])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.SolarTablet) { shopCustomPrice = 20000 });
				shop.Add(new Item(ItemID.EyeSpring) { shopCustomPrice = (int)Math.Round(30000 / 0.0667) });
				shop.Add(new Item(ItemID.BrokenBatWing) { shopCustomPrice = (int)Math.Round(25000 / 0.025 / 2) });
				shop.Add(new Item(ItemID.MoonStone) { shopCustomPrice = (int)Math.Round(75000 / 0.0286 / 4) });
				shop.Add(new Item(ItemID.NeptunesShell) { shopCustomPrice = (int)Math.Round(75000 / 0.02 / 4) });
				shop.Add(new Item(ItemID.Steak) { shopCustomPrice = (int)Math.Round(6000 / 0.01 / 6) });
				shop.Add(new Item(ItemID.DeathSickle) { shopCustomPrice = (int)Math.Round(75000 / 0.025 / 2) });
				shop.Add(new Item(ItemID.ButchersChainsaw) { shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2) });
				shop.Add(new Item(ItemID.ButcherMask) { shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2) });
				shop.Add(new Item(ItemID.ButcherApron) { shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2) });
				shop.Add(new Item(ItemID.DeadlySphereStaff) { shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2) });
				shop.Add(new Item(ItemID.ToxicFlask) { shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2) });
				shop.Add(new Item(ItemID.DrManFlyMask) { shopCustomPrice = (int)Math.Round(5000 / 0.0396 / 2) });
				shop.Add(new Item(ItemID.DrManFlyLabCoat) { shopCustomPrice = (int)Math.Round(5000 / 0.0396 / 2) });
				shop.Add(new Item(ItemID.NailGun) { shopCustomPrice = (int)Math.Round(10000 / 0.04) });
				shop.Add(new Item(ItemID.Nail) { shopCustomPrice = 100 }); //Match the price of the Arm's Dealer
				shop.Add(new Item(ItemID.PsychoKnife) { shopCustomPrice = (int)Math.Round(10000 / 0.025) });
				shop.Add(new Item(ItemID.BrokenHeroSword) { shopCustomPrice = (int)Math.Round(75000 / 0.25) });
				shop.Add(new Item(ItemID.TheEyeOfCthulhu) { shopCustomPrice = (int)Math.Round(125000 / 0.33) });
				shop.Add(new Item(ItemID.MothronWings) { shopCustomPrice = (int)Math.Round(80000 / 0.05) });

				Condition randomPainting(int tick) => new("Mods.BossesAsNPCs.Conditions.MothronPaintingsS", () => Main.GameUpdateCount % 8 == tick);

				// Randomly choose a painting every time the shop is opened.

				shop.Add(new Item(ItemID.WingsofEvil) { shopCustomPrice = (int)Math.Round(1000 / 0.067) }, randomPainting(0));
				shop.Add(new Item(ItemID.MidnightSun) { shopCustomPrice = (int)Math.Round(1000 / 0.017) }, randomPainting(1));
				shop.Add(new Item(ItemID.Buddies) { shopCustomPrice = (int)Math.Round(1000 / 0.0044) }, randomPainting(2));
				shop.Add(new Item(ItemID.ThisIsGettingOutOfHand) { shopCustomPrice = (int)Math.Round(1000 / 0.017) }, randomPainting(3));
				shop.Add(new Item(ItemID.AMachineforTerrarians) { shopCustomPrice = (int)Math.Round(1000 / 0.017) }, randomPainting(4));
				shop.Add(new Item(ItemID.Requiem) { shopCustomPrice = (int)Math.Round(1000 / 0.017) }, randomPainting(5));
				shop.Add(new Item(ItemID.Eyezorhead) { shopCustomPrice = (int)Math.Round(1000 / 0.067) }, randomPainting(6));
				shop.Add(new Item(ItemID.OcularResonance) { shopCustomPrice = (int)Math.Round(1000 / 0.067) }, randomPainting(7));

				shop.Add(new Item(ItemID.MusicBoxEclipse) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWBloodMoon) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MothronEgg", shop, 150000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "SolarVeil", shop);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DefectiveSphere", shop, 0.2f);
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "SqueyereMinionItem", shop, 0.1f); //Crest of Eyes
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients) &&  EchoesoftheAncients)
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Broken_Hero_GunParts", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "BrokenHeroScepter", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "EclipticClicker", shop, 0.04f * 2f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "TeslaDefibrillator", shop, 0.025f);
					NPCHelper.SafelySetCrossModItem(thorium, "SwampSpike", shop, 0.025f);
					NPCHelper.SafelySetCrossModItem(thorium, "FireAxe", shop, 0.033f);
					NPCHelper.SafelySetCrossModItem(thorium, "GarlicBread", shop, 0.01f);
					NPCHelper.SafelySetCrossModItem(thorium, "BrokenHeroFragment", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "SunrayStaff", shop, 0.05f);
					NPCHelper.SafelySetCrossModItem(thorium, "SunflareGuitar", shop, 0.05f);
					NPCHelper.SafelySetCrossModItem(thorium, "StalkersSnippers", shop, 0.05f);
				}
				if (customShops.ContainsKey(NPCString.Mothron))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Mothron])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" ||NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.PumpkinMoonMedallion) { shopCustomPrice = 150000 });
				//Using the highest drop chances
				shop.Add(new Item(ItemID.ScarecrowHat) { shopCustomPrice = (int)Math.Round(6000 / 0.033) });
				shop.Add(new Item(ItemID.ScarecrowShirt) { shopCustomPrice = (int)Math.Round(6000 / 0.033) });
				shop.Add(new Item(ItemID.ScarecrowPants) { shopCustomPrice = (int)Math.Round(6000 / 0.033) });
				shop.Add(new Item(ItemID.JackOLanternMask) { shopCustomPrice = (int)Math.Round(50000 / 0.05) });
				shop.Add(new Item(ItemID.SpookyWood) { shopCustomPrice = 5000 }); //Made up value

				shop.Add(new Item(ItemID.SpookyHook) { shopCustomPrice = (int)Math.Round(40000 / 0.2) }, Condition.DownedMourningWood);
				shop.Add(new Item(ItemID.SpookyTwig) { shopCustomPrice = (int)Math.Round(25000 / 0.2) }, Condition.DownedMourningWood);
				shop.Add(new Item(ItemID.StakeLauncher) { shopCustomPrice = (int)Math.Round(100000 / 0.2) }, Condition.DownedMourningWood);
				shop.Add(new Item(ItemID.Stake) { shopCustomPrice = 15 }, Condition.DownedMourningWood); //Same price as Arms Dealer/Witch Doctor
				shop.Add(new Item(ItemID.CursedSapling) { shopCustomPrice = (int)Math.Round(20000 / 0.2) }, Condition.DownedMourningWood);
				shop.Add(new Item(ItemID.NecromanticScroll) { shopCustomPrice = (int)Math.Round(40000 / 0.2) }, Condition.DownedMourningWood);
				shop.Add(new Item(ItemID.MourningWoodTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, Condition.DownedMourningWood); //same trophy price

				shop.Add(new Item(ItemID.WitchBroom) { shopCustomPrice = 50000 * 5 }, Condition.DownedMourningWood, ShopConditions.Expert);
				shop.Add(new Item(ItemID.SpookyWoodMountItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, Condition.DownedMourningWood, ShopConditions.Master); //Hexxed Branch
				shop.Add(new Item(ItemID.MourningWoodMasterTrophy) { shopCustomPrice = 10000 * 5 }, Condition.DownedMourningWood, ShopConditions.Master); //Hexxed Branch

				shop.Add(new Item(ItemID.TheHorsemansBlade) { shopCustomPrice = (int)Math.Round(100000 / 0.125) });
				shop.Add(new Item(ItemID.BatScepter) { shopCustomPrice = (int)Math.Round(100000 / 0.125) });
				shop.Add(new Item(ItemID.BlackFairyDust) { shopCustomPrice = (int)Math.Round(25000 / 0.125) });
				shop.Add(new Item(ItemID.SpiderEgg) { shopCustomPrice = (int)Math.Round(20000 / 0.125) });
				shop.Add(new Item(ItemID.RavenStaff) { shopCustomPrice = (int)Math.Round(100000 / 0.125) });
				shop.Add(new Item(ItemID.CandyCornRifle) { shopCustomPrice = (int)Math.Round(100000 / 0.125) });
				shop.Add(new Item(ItemID.CandyCorn) { shopCustomPrice = 5 }); //Same price as Arms Dealer
				shop.Add(new Item(ItemID.JackOLanternLauncher) { shopCustomPrice = (int)Math.Round(100000 / 0.125) });
				shop.Add(new Item(ItemID.ExplosiveJackOLantern) { shopCustomPrice = 15 }); //Same price as Arms Dealer
				shop.Add(new Item(ItemID.ScytheWhip) { shopCustomPrice = (int)Math.Round(100000 / 0.125) }); //Dark Harvest
				shop.Add(new Item(ItemID.PumpkingTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.PumpkingPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master);
				shop.Add(new Item(ItemID.PumpkingMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxPumpkinMoon) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWInvasion) { shopCustomPrice = 20000 * 10 },
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
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SpookyBranch", shop, 200000, Condition.DownedMourningWood); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousLookingScythe", shop, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "PumpkingsCape", shop, 0.2f, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode"))); //Pumpking's Cape
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "GoldenRogueSquireMinionItem", shop, 0.13f); //Golden Rogue Crest
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SpookyCore", shop, 0.07f); //Spooky Emblem
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "MourningTorch", shop, 0.1f, Condition.DownedMourningWood);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "WitchClicker", shop, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(clickerClass, "LanternClicker", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "PaganGrasp", shop, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(thorium, "Effigy", shop, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(thorium, "CharonsBeacon", shop, 0.1f, Condition.DownedMourningWood);
					NPCHelper.SafelySetCrossModItem(thorium, "Witchblade", shop, 0.1f);
					NPCHelper.SafelySetCrossModItem(thorium, "SnackLantern", shop, 0.2f);
					NPCHelper.SafelySetCrossModItem(thorium, "HauntingBassDrum", shop, 0.1f);
				}
				if (customShops.ContainsKey(NPCString.Pumpking))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Pumpking])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" ||NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.NaughtyPresent) { shopCustomPrice = 150000 }); //Made up value
				shop.Add(new Item(ItemID.ElfHat) { shopCustomPrice = (int)Math.Round(6000 / 0.017) });
				shop.Add(new Item(ItemID.ElfShirt) { shopCustomPrice = (int)Math.Round(6000 / 0.017) });
				shop.Add(new Item(ItemID.ElfPants) { shopCustomPrice = (int)Math.Round(6000 / 0.017) });

				shop.Add(new Item(ItemID.ChristmasTreeSword) { shopCustomPrice = (int)Math.Round(100000 / 0.078) }, Condition.DownedEverscream);
				shop.Add(new Item(ItemID.ChristmasHook) { shopCustomPrice = (int)Math.Round(40000 / 0.078) }, Condition.DownedEverscream);
				shop.Add(new Item(ItemID.Razorpine) { shopCustomPrice = (int)Math.Round(90000 / 0.078) }, Condition.DownedEverscream);
				shop.Add(new Item(ItemID.FestiveWings) { shopCustomPrice = (int)Math.Round(80000 / 0.017) }, Condition.DownedEverscream);
				shop.Add(new Item(ItemID.EverscreamTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, Condition.DownedEverscream);
				
				shop.Add(new Item(ItemID.EverscreamPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, Condition.DownedEverscream, ShopConditions.Master); //Shrub Star
				shop.Add(new Item(ItemID.EverscreamMasterTrophy) { shopCustomPrice = 10000 * 5 }, Condition.DownedEverscream, ShopConditions.Master);

				shop.Add(new Item(ItemID.ElfMelter) { shopCustomPrice = (int)Math.Round(100000 / 0.125) }, Condition.DownedSantaNK1);
				shop.Add(new Item(ItemID.ChainGun) { shopCustomPrice = (int)Math.Round(90000 / 0.125) }, Condition.DownedSantaNK1);
				shop.Add(new Item(ItemID.SantaNK1Trophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) }, Condition.DownedSantaNK1);
				shop.Add(new Item(ItemID.SantankMountItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, Condition.DownedSantaNK1, ShopConditions.Master); //Toy Tank
				shop.Add(new Item(ItemID.SantankMasterTrophy) { shopCustomPrice = 10000 * 5 }, Condition.DownedSantaNK1, ShopConditions.Master);

				shop.Add(new Item(ItemID.BlizzardStaff) { shopCustomPrice = (int)Math.Round(90000 / 0.08) });
				shop.Add(new Item(ItemID.SnowmanCannon) { shopCustomPrice = (int)Math.Round(90000 / 0.08) });
				shop.Add(new Item(ItemID.NorthPole) { shopCustomPrice = (int)Math.Round(90000 / 0.08) });
				shop.Add(new Item(ItemID.BabyGrinchMischiefWhistle) { shopCustomPrice = (int)Math.Round(5000 / 0.017) }); //Has no value
				shop.Add(new Item(ItemID.IceQueenTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.IceQueenPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master); //Frozen Crown
				shop.Add(new Item(ItemID.IceQueenMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxFrostMoon) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWInvasion) { shopCustomPrice = 20000 * 10 },
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

			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "FestiveOrnament", shop, 200000, Condition.DownedEverscream); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "NaughtyList", shop, 200000, Condition.DownedSantaNK1); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "IceKingsRemains", shop, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "IceQueensCrown", shop, 0.2f, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SantankScrap", shop); //Mechanical Scrap
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "IceSentry", shop, 0.1f); //Frozen Queen's Staff
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "FrostCube", shop, 0.07f); //Frozen Queen's Core
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "RCRemote", shop, 0.02f);
					NPCHelper.SafelySetCrossModItem(orchidMod, "FragilePresent", shop, 0.1f, Condition.DownedSantaNK1);
					NPCHelper.SafelySetCrossModItem(orchidMod, "IceFlakeCone", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("Polarities", out Mod polarities) && Polarities)
				{
					NPCHelper.SafelySetCrossModItem(polarities, "CandyCaneAtlatl", shop, 0.23f, Condition.DownedEverscream);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "NaughtyClicker", shop, 0.1f, Condition.DownedSantaNK1);
					NPCHelper.SafelySetCrossModItem(clickerClass, "FrozenClicker", shop, 0.1f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "Permafrost", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "ChristmasCheer", shop, 0.07f, Condition.DownedEverscream);
					NPCHelper.SafelySetCrossModItem(thorium, "JingleBells", shop, 0.1f, Condition.DownedSantaNK1);
					NPCHelper.SafelySetCrossModItem(thorium, "SoftServeSunderer", shop);
					NPCHelper.SafelySetCrossModItem(thorium, "Cryotherapy", shop, 0.1f);
				}
				if (customShops.ContainsKey(NPCString.IceQueen))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.IceQueen])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
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
			if (shopName == "Shop1" || NPCHelper.StatusShop1())
			{
				shop.Add(new Item(ItemID.MartianConduitPlating) { shopCustomPrice = 100 });
				shop.Add(new Item(ItemID.MartianCostumeMask) { shopCustomPrice = (int)Math.Round(10000 / 0.05) });
				shop.Add(new Item(ItemID.MartianCostumeShirt) { shopCustomPrice = (int)Math.Round(10000 / 0.05) });
				shop.Add(new Item(ItemID.MartianCostumePants) { shopCustomPrice = (int)Math.Round(10000 / 0.05) });
				shop.Add(new Item(ItemID.MartianUniformHelmet) { shopCustomPrice = (int)Math.Round(10000 / 0.05) });
				shop.Add(new Item(ItemID.MartianUniformTorso) { shopCustomPrice = (int)Math.Round(10000 / 0.05) });
				shop.Add(new Item(ItemID.MartianUniformPants) { shopCustomPrice = (int)Math.Round(10000 / 0.05) });
				shop.Add(new Item(ItemID.BrainScrambler) { shopCustomPrice = (int)Math.Round(50000 / 0.01) });
				shop.Add(new Item(ItemID.LaserDrill) { shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7) }); //Special case to make it cheaper
				shop.Add(new Item(ItemID.ChargedBlasterCannon) { shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7) });
				shop.Add(new Item(ItemID.AntiGravityHook) { shopCustomPrice = (int)Math.Round(25000 / 0.013 / 7) });
				shop.Add(new Item(ItemID.Xenopopper) { shopCustomPrice = (int)Math.Round(100000 / 0.167) });
				shop.Add(new Item(ItemID.XenoStaff) { shopCustomPrice = (int)Math.Round(100000 / 0.167) });
				shop.Add(new Item(ItemID.LaserMachinegun) { shopCustomPrice = (int)Math.Round(100000 / 0.167) });
				shop.Add(new Item(ItemID.ElectrosphereLauncher) { shopCustomPrice = (int)Math.Round(100000 / 0.167) });
				shop.Add(new Item(ItemID.InfluxWaver) { shopCustomPrice = (int)Math.Round(100000 / 0.167) });
				shop.Add(new Item(ItemID.CosmicCarKey) { shopCustomPrice = (int)Math.Round(100000 / 0.167) });
				shop.Add(new Item(ItemID.MartianSaucerTrophy) { shopCustomPrice = (int)Math.Round(10000 / 0.1) });

				shop.Add(new Item(ItemID.MartianPetItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25) }, ShopConditions.Master); //Cosmic Skateboard
				shop.Add(new Item(ItemID.UFOMasterTrophy) { shopCustomPrice = 10000 * 5 }, ShopConditions.Master);

				shop.Add(new Item(ItemID.MusicBoxMartians) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(new Item(ItemID.MusicBoxOWInvasion) { shopCustomPrice = 20000 * 10 },
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);

				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeHeadpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeBodypiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
				shop.Add(new Item(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeLegpiece>()) { shopCustomPrice = 50000 }, ShopConditions.SellExtraItems);
			}
			if (shopName == "Shop2" || NPCHelper.StatusShop2())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "RunawayProbe", shop, 500000); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MartianMemoryStick", shop, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "ShockGrenade", shop);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Wingman", shop, 0.14f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "NullificationRifle", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SaucerControlConsole", shop, 0.2f, new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")));
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SuperDartLauncher", shop, 0.01f * 6);
				}
				if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
				{
					NPCHelper.SafelySetCrossModItem(orchidMod, "MartianBeamer", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
				{
					NPCHelper.SafelySetCrossModItem(clickerClass, "HighTechClicker", shop, 0.25f);
				}
				if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
				{
					NPCHelper.SafelySetCrossModItem(thorium, "VoltModule", shop, 0.05f);
					NPCHelper.SafelySetCrossModItem(thorium, "ShieldDroneBeacon", shop, 0.04f);
					NPCHelper.SafelySetCrossModItem(thorium, "CellReconstructor", shop, 0.04f);
					NPCHelper.SafelySetCrossModItem(thorium, "ElectroRebounder", shop, 0.5f);
					NPCHelper.SafelySetCrossModItem(thorium, "TheTriangle", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "Turntable", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "SuperPlasmaCannon", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "Kinetoscythe", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "CosmicDagger", shop, 0.25f);
					NPCHelper.SafelySetCrossModItem(thorium, "LivewireCrasher", shop, 0.25f);
				}
				if (customShops.ContainsKey(NPCString.MartianSaucer))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.MartianSaucer])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
					}
				}
			}
		}
		#endregion

		#region Goblin Tinkerer
		/// <summary>
		/// Goblin Tinkerer's extra shop. These shop items are affected by the shop price scaling config.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopMulti">The float multiplier for the shop price scaling. It is the config number / 100f</param>
		public static void GoblinTinkerer(NPCShop shop, float shopMulti)
		{
			shop.Add(new Item(ItemID.GoblinBattleStandard) { shopCustomPrice = (int)Math.Round(25000 * shopMulti) }); //Made up value
			shop.Add(new Item(ItemID.Harpoon) { shopCustomPrice = (int)Math.Round(5400 / 0.005 / 5 * shopMulti) }); //Special case to make it cheaper
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && CalamityMod && ShopConditions.TownNPCsCrossModSupport.IsMet())
			{
				NPCHelper.SafelySetCrossModItem(calamityMod, "PlasmaRod", shop, (0.07f * 5), shopMulti);
			}
			if (ModLoader.TryGetMod("OrchidMod", out Mod orchidMod) && OrchidMod)
			{
				NPCHelper.SafelySetCrossModItem(orchidMod, "GoblinArmyFlask", shop, (0.02f * 5), shopMulti);
				NPCHelper.SafelySetCrossModItem(orchidMod, "GoblinArmyCard", shop, (0.02f * 5), shopMulti);
				NPCHelper.SafelySetCrossModItem(orchidMod, "GoblinStick", shop, 0.33f, shopMulti, ShopConditions.DownedGoblinWarlock);
			}
			if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
			{
				NPCHelper.SafelySetCrossModItem(clickerClass, "ShadowyClicker", shop, (0.05f * 5), shopMulti);
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
			{
				NPCHelper.SafelySetCrossModItem(thorium, "YewWoodBlowpipe", shop, 0.05f);
				NPCHelper.SafelySetCrossModItem(thorium, "YewWood", shop);
				NPCHelper.SafelySetCrossModItem(thorium, "DarkGate", shop, 0.05f);
				NPCHelper.SafelySetCrossModItem(thorium, "SpikeBomb", shop);
			}
			shop.Add(new Item(ItemID.ShadowFlameHexDoll) { shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti) }, ShopConditions.DownedGoblinWarlock);
			shop.Add(new Item(ItemID.ShadowFlameBow) { shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti) }, ShopConditions.DownedGoblinWarlock);
			shop.Add(new Item(ItemID.ShadowFlameKnife) { shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti) }, ShopConditions.DownedGoblinWarlock);

			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && Fargowiltas)
			{
				NPCHelper.SafelySetCrossModItem(fargosMutant, "ShadowflameIcon", shop, 0.01f, shopMulti, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport); //10 gold
			}
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod2) && CalamityMod)
			{
				NPCHelper.SafelySetCrossModItem(calamityMod2, "BurningStrife", shop, (0.33f * 5), shopMulti, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(calamityMod2, "TheFirstShadowflame", shop, (0.33f * 5), shopMulti, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport);
			}
			if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && FargowiltasSouls)
			{
				NPCHelper.SafelySetCrossModItem(fargosSouls, "WretchedPouch", shop, (0.2f * 5), shopMulti, ShopConditions.DownedGoblinWarlock,
					new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")),
					ShopConditions.TownNPCsCrossModSupport);
			}
			if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions) && AmuletOfManyMinions)
			{
				NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "GoblinGunnerMinionItem", shop, (0.44f * 5), shopMulti, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport); //Goblin Radio Beacon
			}
			if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions) && StormDiversMod)
			{
				NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameBMask", shop, 1f, shopMulti, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport); //Shadowflare Mask
				NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameChestplate", shop, 1f, shopMulti, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport); //Shadowflare Robe
				NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameGreaves", shop, 1f, shopMulti, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport); //Shadowflare Greaves
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium2) &&  ThoriumMod)
			{
				NPCHelper.SafelySetCrossModItem(thorium2, "ShadowPurgeCaltrop", shop, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium2, "ShadowflameWarhorn", shop, 0.17f, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium2, "ShadowTippedJavelin", shop, ShopConditions.DownedGoblinWarlock, ShopConditions.TownNPCsCrossModSupport);
			}
			if (customShops.ContainsKey(NPCString.GoblinTinkerer))
			{
				foreach (KeyValuePair<int, object[]> set in customShops[NPCString.GoblinTinkerer])
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
				}
			}
		}
		#endregion

		#region Pirate
		/// <summary>
		/// Pirate's extra shop. These shop items are affected by the shop price scaling config.
		/// </summary>
		/// <param name="shop">The NPCShop shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="shopMulti">The float multiplier for the shop price scaling. It is the config number / 100f</param>
		public static void Pirate(NPCShop shop, float shopMulti)
		{
			shop.Add(new Item(ItemID.PirateMap) { shopCustomPrice = (int)Math.Round(50000 * shopMulti) }); //Made up value

			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				NPCHelper.SafelySetCrossModItem(fargosMutant, "PirateFlag", shop, (int)Math.Round(150000 * shopMulti), ShopConditions.TownNPCsCrossModSupport); //Match the Deviantt's shop
			}
			//Formula: (Sell value / drop chance)
			shop.Add(new Item(ItemID.CoinGun) { shopCustomPrice = (int)Math.Round(60000 / 0.02 * shopMulti) });
			shop.Add(new Item(ItemID.LuckyCoin) { shopCustomPrice = (int)Math.Round(10000 / 0.067 * shopMulti) });
			shop.Add(new Item(ItemID.DiscountCard) { shopCustomPrice = (int)Math.Round(10000 / 0.067 * shopMulti) });
			shop.Add(new Item(ItemID.PirateStaff) { shopCustomPrice = (int)Math.Round(10000 / 0.067 * shopMulti) });
			shop.Add(new Item(ItemID.GoldRing) { shopCustomPrice = (int)Math.Round(10000 / 0.067 * shopMulti) });
			shop.Add(new Item(ItemID.PirateMinecart) { shopCustomPrice = (int)Math.Round(5000 / 0.05 * shopMulti) });
			shop.Add(new Item(ItemID.Cutlass) { shopCustomPrice = (int)Math.Round(36000 / 0.1 * shopMulti) });
			shop.Add(new Item(ItemID.FlyingDutchmanTrophy) { shopCustomPrice = (int)Math.Round(10000 * 0.1 * shopMulti) });

			shop.Add(new Item(ItemID.PirateShipMountItem) { shopCustomPrice = (int)Math.Round(50000 / 0.25 * shopMulti) }, ShopConditions.Master); //Black Spot
			shop.Add(new Item(ItemID.FlyingDutchmanMasterTrophy) { shopCustomPrice = (int)Math.Round(10000 * 5 * shopMulti) }, ShopConditions.Master);

			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && CalamityMod)
			{
				NPCHelper.SafelySetCrossModItem(calamity, "MidasPrime", shop, (0.04f * 5), shopMulti, ShopConditions.TownNPCsCrossModSupport);
			}
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant2) && ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls) && Fargowiltas && FargowiltasSouls)
			{
				NPCHelper.SafelySetCrossModItem(fargosMutant2, "GoldenDippingVat", shop, (0.07f * 5), shopMulti,
					new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")), ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(fargosSouls, "SecurityWallet", shop, (0.1f * 5), shopMulti,
					new Condition(ShopConditions.EternityModeS, () => (bool)fargosSouls.Call("EternityMode")), ShopConditions.TownNPCsCrossModSupport);
			}
			if (ModLoader.TryGetMod("ClickerClass", out Mod clickerClass) && ClickerClass)
			{
				NPCHelper.SafelySetCrossModItem(clickerClass, "CaptainsClicker", shop, (0.125f * 5), shopMulti, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(clickerClass, "GoldenTicket", shop, (0.25f * 5), shopMulti, ShopConditions.TownNPCsCrossModSupport);
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && ThoriumMod)
			{
				NPCHelper.SafelySetCrossModItem(thorium, "DeadEyePatch", shop, 0.04f, ShopConditions.TownNPCsCrossModSupport);
				// NPCHelper.SafelySetCrossModItem(thorium, "CaptainsPoniard", shop, ShopConditions.TownNPCsCrossModSupport); Thorium already adds it
				NPCHelper.SafelySetCrossModItem(thorium, "BountyBanner", shop, 0.1f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "Concertina", shop, 0.15f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "TheJuggernaut", shop, 0.2f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "ShipsHelm", shop, 0.2f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "HandCannon", shop, 0.2f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "DutchmansAvarice", shop, 0.2f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "TwentyFourCaratTuba", shop, 0.2f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "GreedfulGurdy", shop, 0.1f, ShopConditions.TownNPCsCrossModSupport);
				NPCHelper.SafelySetCrossModItem(thorium, "GreedyMagnet", shop, 0.1f, ShopConditions.TownNPCsCrossModSupport);
			}
			if (customShops.ContainsKey(NPCString.Pirate))
			{
				foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Pirate])
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = (int)set.Value[0] }, ((List<Condition>)set.Value[1]).ToArray());
				}
			}
		}
		#endregion
	}
}