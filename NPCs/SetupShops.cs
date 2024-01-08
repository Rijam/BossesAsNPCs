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
		private static Dictionary<string, Dictionary<int, Tuple<int, List<Condition>>>> customShops = new()
		{
			{ NPCString.KingSlime, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.EyeOfCthulhu, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.EaterOfWorlds, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.BrainOfCthulhu, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.QueenBee, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Skeletron, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Deerclops, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.WallOfFlesh, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.QueenSlime, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.TheDestroyer, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Retinazer, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Spazmatism, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.SkeletronPrime, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Plantera, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Golem, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.EmpressOfLight, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.DukeFishron, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Betsy, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.LunaticCultist, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.MoonLord, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Dreadnautilus, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Mothron, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Pumpking, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.IceQueen, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.MartianSaucer, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.GoblinTinkerer, new Dictionary<int, Tuple<int, List<Condition>>> { } },
			{ NPCString.Pirate, new Dictionary<int, Tuple<int, List<Condition>>> { } }
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
		public static void AddToCustomShops(string npc, int item, int price, List<Condition> condition) => customShops[npc].Add(item, new Tuple<int, List<Condition>>(price, condition));

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
#pragma warning restore CA2211 // Non-constant fields should not be visible

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

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.UnlockOWMusicOrDrunkWorld, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Gel, priceMulti: 10), ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SlimeStaff, priceMulti: 10), ShopConditions.SellExtraItems);
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.KingSlime])
					{
						// set.Value.Item1 is the price (int)
						// set.Value.Item2 is the condition (List<Condition>)
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DemoniteOre, priceMulti: 5), ShopConditions.CorruptionOrHardmode);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.CorruptSeeds, priceMulti: 5), ShopConditions.CorruptionOrHardmode);
				// In a Corruption World, in Hardmode, after defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 }, Condition.CorruptWorld, Condition.Hardmode, Condition.DownedEowOrBoc);
				// In a Corruption World, in Pre-HardMode, after defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 * 2 }, Condition.CorruptWorld, Condition.PreHardmode, Condition.DownedEowOrBoc);
				// In a Corruption World, before defeating EoW
				shop.Add(new Item(ItemID.UnholyArrow) { shopCustomPrice = 40 * 5 }, ShopConditions.CorruptionOrHardmode, Condition.NotDownedEowOrBoc);

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

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss1, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.EyeOfCthulhu])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DemoniteOre, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ShadowScale, priceMulti: 5));
				//Formula: (Sell value / drop chance))
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EatersBone, 0.05));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterofWorldsTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.WormScarf, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterOfWorldsPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EaterofWorldsMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.EaterOfWorlds])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.BrainOfCthulhu])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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

				shop.Add(NPCHelper.ItemWithPrice(ItemID.HiveBackpack, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenBeePetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.QueenBeeMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss5, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.QueenBee])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronHand, 0.12));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BookofSkulls, 0.11));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.ChippysCouch, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BoneGlove, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss5, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Skeletron])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Deerclops])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.WallOfFlesh])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.QueenSlime])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofMight, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalWagonPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.DestroyerMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss3, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.TheDestroyer])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofSight, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RetinazerTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalWheelPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Retinazer])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofSight, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SpazmatismTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalWheelPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TwinsMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss2, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Spazmatism])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.HallowedBar, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SoulofFright, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.WaffleIron, priceMulti: 5), Condition.DownedMechBossAll, Condition.ZenithWorld);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimeMask, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimeTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MechanicalBatteryPiece, priceMulti: 5), ShopConditions.Expert);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimePetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SkeletronPrimeMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss1, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.SkeletronPrime])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Plantera])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(new Item(ItemID.LihzahrdPowerCell) { shopCustomPrice = 350000 }); // Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Picksaw, 0.25));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BeetleHusk, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Stynger, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.StyngerBolt, valueDiv: 1));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PossessedHatchet, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SunStone, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.EyeoftheGolem, 0.14));
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Golem])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.EmpressOfLight])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(new Item(ItemID.TruffleWorm) { shopCustomPrice = 400000 }); // Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.BubbleGun, 0.2), Condition.NotRemixWorld);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.AquaScepter, 0.2), Condition.RemixWorld);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Flairon, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RazorbladeTyphoon, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.TempestStaff, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Tsunami, 0.2));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FishronWings, 0.07));

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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.DukeFishron])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Betsy])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(new Item(ItemID.LunarCraftingStation) { shopCustomPrice = 100000 }); // Ancient Manipulator // Made up value
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentSolar, priceMulti: 10), Condition.DownedSolarPillar);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentVortex, priceMulti: 10), Condition.DownedSolarPillar);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentNebula, priceMulti: 10), Condition.DownedSolarPillar);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.FragmentStardust, priceMulti: 10), Condition.DownedSolarPillar);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.BossMaskCultist, 0.14));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.AncientCultistTrophy, 0.1));

				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunaticCultistPetItem, 0.25), ShopConditions.Master);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunaticCultistMasterTrophy, priceMulti: 5), ShopConditions.Master);

				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxBoss4, priceMulti: 10),
					ShopConditions.RescuedWizard, ShopConditions.SellExtraItems);
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MusicBoxOWBoss2, priceMulti: 10),
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.LunaticCultist])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
				shop.Add(NPCHelper.ItemWithPrice(ItemID.PortalGun, priceMulti: 5));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunarOre, priceMulti: 5));
				// Even though Moon Lord now drops two of these items, I've left the chances at 0.22
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Meowmere, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Terrarian, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.StarWrath, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.SDMG, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LastPrism, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.LunarFlareBook, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.RainbowCrystalStaff, 0.22));
				shop.Add(NPCHelper.ItemWithPrice(ItemID.MoonlordTurretStaff, 0.22)); // Lunar Portal Staff
				shop.Add(NPCHelper.ItemWithPrice(ItemID.Celeb2, 0.22)); // Celebration Mk2
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.MoonLord])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Dreadnautilus])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Mothron])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Pumpking])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.IceQueen])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
					foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.MartianSaucer])
					{
						shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
			shop.Add(NPCHelper.ItemWithPrice(ItemID.Harpoon, 0.005, secondDiv: 5, priceMulti: shopMulti)); //Special case to make it cheaper
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
				foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.GoblinTinkerer])
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
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
			shop.Add(NPCHelper.ItemWithPrice(ItemID.CoinGun, 0.02, priceMulti: shopMulti));
			shop.Add(NPCHelper.ItemWithPrice(ItemID.LuckyCoin, 0.067, priceMulti: shopMulti));
			shop.Add(NPCHelper.ItemWithPrice(ItemID.DiscountCard, 0.067, priceMulti: shopMulti));
			shop.Add(NPCHelper.ItemWithPrice(ItemID.PirateStaff, 0.067, priceMulti: shopMulti));
			shop.Add(NPCHelper.ItemWithPrice(ItemID.GoldRing, 0.067, priceMulti: shopMulti));
			shop.Add(NPCHelper.ItemWithPrice(ItemID.PirateMinecart, 0.05, priceMulti: shopMulti));
			shop.Add(NPCHelper.ItemWithPrice(ItemID.Cutlass, 0.1, priceMulti: shopMulti));
			shop.Add(NPCHelper.ItemWithPrice(ItemID.FlyingDutchmanTrophy, 0.1, priceMulti: shopMulti));

			shop.Add(NPCHelper.ItemWithPrice(ItemID.PirateShipMountItem, 0.25, priceMulti: shopMulti), ShopConditions.Master); //Black Spot
			shop.Add(NPCHelper.ItemWithPrice(ItemID.FlyingDutchmanMasterTrophy, priceMulti: 5 * shopMulti), ShopConditions.Master);

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
				foreach (KeyValuePair<int, Tuple<int, List<Condition>>> set in customShops[NPCString.Pirate])
				{
					shop.Add(new Item(set.Key) { shopCustomPrice = set.Value.Item1 }, (set.Value.Item2).ToArray());
				}
			}
		}
		#endregion
	}
}