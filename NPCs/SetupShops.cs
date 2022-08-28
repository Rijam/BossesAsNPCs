using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossesAsNPCs.NPCs.TownNPCs;
using Terraria.Localization;
using System.Linq.Expressions;

namespace BossesAsNPCs.NPCs
{
	/// <summary>
	/// Returns the associated string.
	/// </summary>
	public class NPCString
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
		public static string GoblinTinkerer => "GoblinTinkerer";
		public static string Pirate => "Pirate";
	}

	/// <summary>
	/// Contains the predefined shops and the ability to add new items.
	/// </summary>
	public class SetupShops
	{
		private static bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

		// string is the NPC name
		// int is the item
		// object[0] is the price (int)
		// object[1] is the condition function (Func<bool>)
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

		public static void ClearCustomShops() => customShops.Clear();

		/// <summary>
		/// Adds to the dictionary.
		/// </summary>
		/// <param name="npc">The string for the corresponding NPC</param>
		/// <param name="item">The ID for the item</param>
		/// <param name="price">The price for the item</param>
		/// <param name="condition">The availability of the item</param>
		public static void AddToCustomShops(string npc, int item, int price, Func<bool> condition) => customShops[npc].Add(item, new object[] { price, condition });

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
		public static bool SetShopItem(string npc, int item, Func<bool> condition)
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
		public static bool SetShopItem(string npc, int item, Func<bool> condition, int customPrice)
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
		public static bool SetShopItem(string npc, int item, Func<bool> condition, float priceDiv)
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
		public static bool SetShopItem(string npc, int item, Func<bool> condition, float priceDiv, float priceMulti)
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
		/// <returns>The value of the item. Will return -1 if the ModItem is vanilla.</returns>
		public static int CalcItemValue(int item)
		{
			ModItem modItem = ItemLoader.GetItem(item);
			if (modItem == null) // Vanilla item
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.DebugFormat("Cross mod SetShopItem(): Vanilla item detected \"{0}\". Price may be inaccurate. It is recommended to manually set the price with \"CustomPrice\".", item);
				return -1;
			}
			return modItem.Item.value;
		}

		/// <summary>
		/// CalcItemValue() cannot determine the value of vanilla items. This will get the value of the item as it is being set.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <returns>ModItems will return currentPrice. If it is a vanilla item, it will be the value of the current shop slot.</returns>
		public static int PostCalcItemValue(Chest shop, ref int nextSlot, int currentPrice)
		{
			if (currentPrice > 0) // If the price is positive, then that is the price.
			{
				return currentPrice;
			}
			// If it is negative (because we said vanilla items had -1 value), then get the value of the slot.
			return shop.item[nextSlot].value;
		}

		#region King Slime
		/// <summary>
		/// King Slime's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void KingSlime(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.SlimeCrown);
				shop.item[nextSlot].shopCustomPrice = 50000; //Made up value since Slime Crown has no value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Solidifier);
				shop.item[nextSlot].shopCustomPrice = 20000 * 2;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SlimySaddle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25); //Formula: (Sell value / drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NinjaHood);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NinjaShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NinjaPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SlimeHook);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SlimeGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.67);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KingSlimeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KingSlimeTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.RoyalGel);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.KingSlimePetItem); //Royal Delight
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.KingSlimeMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.Gel);
					shop.item[nextSlot].shopCustomPrice = 1 * 10;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SlimeStaff);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					if (NPC.CountNPCS(NPCID.Princess) > 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.PrinceUniform);
						shop.item[nextSlot].shopCustomPrice = 500000;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.PrincePants);
						shop.item[nextSlot].shopCustomPrice = 500000;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.PrinceCape);
						shop.item[nextSlot].shopCustomPrice = 500000;
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeGloves>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.KingSlime.KSAltCostumeGloves>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SlimyCrown", shop, ref nextSlot, 50000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeKingSlime", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CrownJewel", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimeKingsSlasher", shop, ref nextSlot, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "MedallionoftheFallenKing", shop, ref nextSlot, 0.01f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimyShield", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.KingSlime))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.KingSlime])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region EyeOfCthulhu
		/// <summary>
		/// Eye of Cthulhu's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void EyeOfCthulhu(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.SuspiciousLookingEye);
				shop.item[nextSlot].shopCustomPrice = 75000; //Made up value since it has no value
				nextSlot++;
				if (!WorldGen.crimson || Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DemoniteOre);
					shop.item[nextSlot].shopCustomPrice = 1000 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CorruptSeeds);
					shop.item[nextSlot].shopCustomPrice = 500 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.UnholyArrow);
					if (NPC.downedBoss2)
					{
						if (Main.hardMode)
						{
							shop.item[nextSlot].shopCustomPrice = 40;
						}
						else
						{
							shop.item[nextSlot].shopCustomPrice = 40 * 2;
						}
					}
					else
					{
						shop.item[nextSlot].shopCustomPrice = 40 * 5;
					}
					nextSlot++;
				}
				if (WorldGen.crimson || Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.CrimtaneOre);
					shop.item[nextSlot].shopCustomPrice = 1300 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CrimsonSeeds);
					shop.item[nextSlot].shopCustomPrice = 500 * 5;
					nextSlot++;
				}

				shop.item[nextSlot].SetDefaults(ItemID.Binoculars);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.03); //Formula: (Sell value * 3 / drop chance))
				nextSlot++;
				int wof = NPC.FindFirstNPC(ModContent.NPCType<WallOfFlesh>());
				if (wof >= 0)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BadgersHat);
					shop.item[nextSlot].shopCustomPrice = 3000 * 20;
					nextSlot++;
				}

				shop.item[nextSlot].SetDefaults(ItemID.EyeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EyeofCthulhuTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.EoCShield);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AviatorSunglasses);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.EyeOfCthulhuPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.EyeofCthulhuMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousEye", shop, ref nextSlot, 80000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeEyeofCthulhu", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DeathstareRod", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "TeardropCleaver", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "LeashOfCthulhu", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "AgitatingLens", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeSword", shop, ref nextSlot, 0.25f); //Eye Sored
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeGun", shop, ref nextSlot, 0.25f); //Eye Rifle
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeStaff", shop, ref nextSlot, 0.25f); //The Eyestalk
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeMinion", shop, ref nextSlot, 0.25f); //Eyeball Staff
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "EyeHook", shop, ref nextSlot, 0.25f); //Eyeball Hook
				}
				if (customShops.ContainsKey(NPCString.EyeOfCthulhu))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.EyeOfCthulhu])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region EaterOfWorlds
		/// <summary>
		/// Eater of Worlds' shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void EaterOfWorlds(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.WormFood);
				shop.item[nextSlot].shopCustomPrice = 100000; //Made up value since it has no value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DemoniteOre);
				shop.item[nextSlot].shopCustomPrice = 1000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ShadowScale);
				shop.item[nextSlot].shopCustomPrice = 100 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EatersBone);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.05); //Formula: (Sell value / drop chance))
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EaterMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EaterofWorldsTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.WormScarf);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.EaterOfWorldsPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.EaterofWorldsMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.VilePowder);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
					if (Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.UnholyWater);
						shop.item[nextSlot].shopCustomPrice = 100;
						nextSlot++;
					}
					int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
					if (steampunker >= 0 && NPC.downedMechBossAny)
					{
						shop.item[nextSlot].SetDefaults(ItemID.PurpleSolution);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ItemID.WormTooth);
					shop.item[nextSlot].shopCustomPrice = 20 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "WormyFood", shop, ref nextSlot, 100000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeEaterofWorlds", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeCorruption", shop, ref nextSlot, 10000);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "EaterStaff", shop, ref nextSlot, 0.1f); //Eater of Worlds Staff

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "DarkenedHeart", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.EaterOfWorlds))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.EaterOfWorlds])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region BrainOfCthulhu
		/// <summary>
		/// Brain of Cthulhu's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void BrainOfCthulhu(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.BloodySpine);
				shop.item[nextSlot].shopCustomPrice = 100000; //Made up value since it has no value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CrimtaneOre);
				shop.item[nextSlot].shopCustomPrice = 1300 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TissueSample);
				shop.item[nextSlot].shopCustomPrice = 150 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BoneRattle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.05); //Formula: (Sell value / drop chance))
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrainMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrainofCthulhuTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BrainOfConfusion);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BrainOfCthulhuPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BrainofCthulhuMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss3);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.ViciousPowder);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
					if (Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.BloodWater);
						shop.item[nextSlot].shopCustomPrice = 100;
						nextSlot++;
					}
					int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
					if (steampunker >= 0 && NPC.downedMechBossAny)
					{
						shop.item[nextSlot].SetDefaults(ItemID.RedSolution);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					if (ModLoader.TryGetMod("RijamsMod", out Mod rijamsMod) && townNPCsCrossModSupport) // It's my mod lol
					{
						NPCHelper.SafelySetCrossModItem(rijamsMod, "CrawlerChelicera", shop, ref nextSlot);
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "GoreySpine", shop, ref nextSlot, 100000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeBrainofCthulhu", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeCrimson", shop, ref nextSlot, 10000);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "BrainStaff", shop, ref nextSlot, 0.1f); //Mind Break
					NPCHelper.SafelySetCrossModItem(fargosSouls, "CrimetroidEgg", shop, ref nextSlot, 0.04f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "GuttedHeart", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.BrainOfCthulhu))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.BrainOfCthulhu])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region QueenBee
		/// <summary>
		/// Queen Bee's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void QueenBee(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.Abeemination);
				shop.item[nextSlot].shopCustomPrice = 125000; //Made up value since it has no value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeeGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33); //Formula: (Sell value / drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeeKeeper);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeesKnees);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HiveWand);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeeHat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeeShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeePants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HoneyComb);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Nectar);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.05); //Formula: (Sell value /drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HoneyedGoggles);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Beenade);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40 / 0.75);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeeWax);
				shop.item[nextSlot].shopCustomPrice = 20 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BottledHoney);
				shop.item[nextSlot].shopCustomPrice = 8 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.QueenBeeTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.HiveBackpack);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.QueenBeePetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.QueenBeeMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss5);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.Hive);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Stinger);
					shop.item[nextSlot].shopCustomPrice = 40 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Bezoar);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
					if (Main.LocalPlayer.ZoneGraveyard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.BeeHive);
						shop.item[nextSlot].shopCustomPrice = 50 * 5;
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					if (NPC.downedMechBossAny)
					{
						shop.item[nextSlot].SetDefaults(ItemID.BeeWings);
						shop.item[nextSlot].shopCustomPrice = 80000 * 5;
						nextSlot++;
					}
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "Abeemination2", shop, ref nextSlot, 150000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeQueenBee", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "HardenedHoneycomb", shop, ref nextSlot);
					NPCHelper.SafelySetCrossModItem(calamityMod, "TheBee", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TheSmallSting", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "QueenStinger", shop, ref nextSlot); //The Queen's Stinger
					}
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BeeQueenMinionItem", shop, ref nextSlot, 0.44f); //Bee Queen's Crown
				}
				if (customShops.ContainsKey(NPCString.QueenBee))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.QueenBee])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Skeletron
		/// <summary>
		/// Skeletron's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Skeletron(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.ClothierVoodooDoll);
				shop.item[nextSlot].shopCustomPrice = 130000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SkeletronHand);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(9000 / 0.12); //Formula: (Sell value / drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BookofSkulls);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChippysCouch);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SkeletronMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SkeletronTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BoneGlove);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.SkeletronPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SkeletronMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					if (BossesAsNPCsWorld.downedDungeonGuardian)
					{
						shop.item[nextSlot].SetDefaults(ItemID.BoneKey);
						shop.item[nextSlot].shopCustomPrice = 50000 * 5;
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ItemID.BoneWand);
					shop.item[nextSlot].shopCustomPrice = 50 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Bone);
					shop.item[nextSlot].shopCustomPrice = 10 * 5;
					nextSlot++;
					if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC)) // I'll leave this here because it's a vanilla item and it's my mod.
					{
						int fisherman = NPC.FindFirstNPC(fishermanNPC.Find<ModNPC>("Fisherman").Type);
						if (fisherman >= 0)
						{
							shop.item[nextSlot].SetDefaults(ItemID.LockBox);
							shop.item[nextSlot].shopCustomPrice = 4000 * 5;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Skeletron.SkCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Skeletron.SkCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousSkull", shop, ref nextSlot, 150000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeSkeletron", shop, ref nextSlot, 10000);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "BoneZone", shop, ref nextSlot, 0.1f); //The Bone Zone

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "NecromanticBrew", shop, ref nextSlot);
					}
				}

				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					//Skeletal Rod of Minion Guidance
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BoneWaypointRod", shop, ref nextSlot, 100); //Normally no value
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "SquireSkullAccessory", shop, ref nextSlot, 0.65f);
				}
				if (customShops.ContainsKey(NPCString.Skeletron))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Skeletron])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Deerclops
		/// <summary>
		/// Deerclops' shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Deerclops(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.DeerThing);
				shop.item[nextSlot].shopCustomPrice = 140000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChesterPetItem); //Eye Bone
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33); //Formula: (Sell value / drop chance))
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Eyebrella);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DontStarveShaderItem); //Radio Thing
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PewMaticHorn);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WeatherPain);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HoundiusShootius);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LucyTheAxe);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DeerclopsMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DeerclopsTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;
				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BoneHelm);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DeerclopsPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DeerclopsMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxDeerclops);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					Player player = Main.player[Main.myPlayer];
					bool theConstant = Main.dontStarveWorld || WorldGen.dontStarveWorldGen;
					if (player.ZoneGraveyard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.AbigailsFlower);
						shop.item[nextSlot].shopCustomPrice = 500 * 5;
						nextSlot++;
					}
					if (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.BatBat);
						shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(2500 / 0.01) : (int)Math.Round(2500 / 0.004);
						nextSlot++;
					}
					if (player.ZoneSnow && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight) && (player.ZoneHallow || player.ZoneCorrupt || player.ZoneCrimson))
					{
						shop.item[nextSlot].SetDefaults(ItemID.HamBat);
						shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(10000 / 0.1) : (int)Math.Round(10000 / 0.04);
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ItemID.PigPetItem); //Monster Meat
					shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(10000 / 0.005) : (int)Math.Round(10000 / 0.001);
					nextSlot++;
					if (Main.hardMode && player.ZoneJungle)
					{
						shop.item[nextSlot].SetDefaults(ItemID.GlommerPetItem); //Glommer's Flower
						shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(50000 / 0.025) : (int)Math.Round(50000 / 0.01);
						nextSlot++;
					}
					int travelingMerchant = NPC.FindFirstNPC(NPCID.TravellingMerchant);
					if (travelingMerchant >= 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.PaintingWendy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.PaintingWillow);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.PaintingWilson);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.PaintingWolfgang);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "DeerThing2", shop, ref nextSlot, 120000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "Deerclawps", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(fargosSouls, "DeerSinew", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.Deerclops))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Deerclops])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region WallOfFlesh
		/// <summary>
		/// Wall of Flesh's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void WallOfFlesh(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.GuideVoodooDoll);
				shop.item[nextSlot].shopCustomPrice = 150000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Pwnhammer);
				shop.item[nextSlot].shopCustomPrice = 7800 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BreakerBlade);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13); //Formula: (Sell value / drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ClockworkAssaultRifle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LaserRifle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FireWhip);//Firecracker
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WarriorEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RangerEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SorcererEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SummonerEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				int eoc = NPC.FindFirstNPC(ModContent.NPCType<EyeOfCthulhu>());
				if (eoc >= 0)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BadgersHat);
					shop.item[nextSlot].shopCustomPrice = 3000 * 20;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.FleshMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WallofFleshTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;
				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DemonHeart);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.WallOfFleshGoatMountItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.WallofFleshMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWWallOfFlesh);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.DemoniteBrick);
					shop.item[nextSlot].shopCustomPrice = 1500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CrimtaneBrick);
					shop.item[nextSlot].shopCustomPrice = 1500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBackpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "FleshyDoll", shop, ref nextSlot, 200000);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeWallofFlesh", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeUnderworld", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Carnage", shop, ref nextSlot, 0.1f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlackHawkRemote", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlastBarrel", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Meowthrower", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "RogueEmblem", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "HermitsBoxofOneHundredMedicines", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "FleshHand", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "PungentEyeball", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BoneSerpentMinionItem", shop, ref nextSlot, 0.35f);
				}
				if (customShops.ContainsKey(NPCString.WallOfFlesh))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.WallOfFlesh])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region QueenSlime
		/// <summary>
		/// Queen Slime's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void QueenSlime(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeCrystal);
				shop.item[nextSlot].shopCustomPrice = 200000; //Made up value
				nextSlot++;

				shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeMountSaddle); //Gelatinous Pillion
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25); //Formula: (Sell value / drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CrystalNinjaHelmet); //Crystal Assassin Hood
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CrystalNinjaChestplate); //Crystal Assassin Shirt
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CrystalNinjaLeggings); //Crystal Assassin Pants
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeHook); //Hook of Dissonance
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Smolstar); //Blade Staff
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.GelBalloon);
				shop.item[nextSlot].shopCustomPrice = 40 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;
				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.VolatileGelatin);
					shop.item[nextSlot].shopCustomPrice = 50000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.QueenSlimePetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxQueenSlime);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.PinkGel);
					shop.item[nextSlot].shopCustomPrice = 3 * 10;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenSlime.QSAltCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenSlime.QSCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenSlime.QSCostumeGloves>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "JellyCrystal", shop, ref nextSlot, 250000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "GelicWings", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.QueenSlime))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.QueenSlime])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region TheDestroyer
		/// <summary>
		/// The Destroyer's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void TheDestroyer(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.MechanicalWorm);
				shop.item[nextSlot].shopCustomPrice = 250000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HallowedBar);
				shop.item[nextSlot].shopCustomPrice = 400 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SoulofMight);
				shop.item[nextSlot].shopCustomPrice = 800 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DestroyerMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DestroyerTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MechanicalWagonPiece);
					shop.item[nextSlot].shopCustomPrice = 5000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DestroyerPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DestroyerMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss3);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechWorm", shop, ref nextSlot, 400000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, ref nextSlot, 1000000);
					}
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeDestroyer", shop, ref nextSlot, 10000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, ref nextSlot, 10000);
					}
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DestroyerGun", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "GroundStick", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.TheDestroyer))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.TheDestroyer])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Retinazer
		/// <summary>
		/// Retinazer's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Retinazer(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.MechanicalEye);
				shop.item[nextSlot].shopCustomPrice = 250000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HallowedBar);
				shop.item[nextSlot].shopCustomPrice = 400 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SoulofSight);
				shop.item[nextSlot].shopCustomPrice = 800 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TwinMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RetinazerTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MechanicalWheelPiece);
					shop.item[nextSlot].shopCustomPrice = 5000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.TwinsPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.TwinsMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Retinazer.RetCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Retinazer.RetCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechEye", shop, ref nextSlot, 400000); //Match the Mutant's shop

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, ref nextSlot, 1000000);
					}
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeTwins", shop, ref nextSlot, 10000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, ref nextSlot, 10000);
					}
					NPCHelper.SafelySetCrossModItem(calamityMod, "Arbalest", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TwinRangs", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "FusedLens", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ref nextSlot); //Mechanical Spikes
					}
				}
				if (customShops.ContainsKey(NPCString.Retinazer))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Retinazer])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Spazmatism
		/// <summary>
		/// Spazmatism's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Spazmatism(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.MechanicalEye);
				shop.item[nextSlot].shopCustomPrice = 250000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HallowedBar);
				shop.item[nextSlot].shopCustomPrice = 400 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SoulofSight);
				shop.item[nextSlot].shopCustomPrice = 800 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TwinMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SpazmatismTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MechanicalWheelPiece);
					shop.item[nextSlot].shopCustomPrice = 5000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.TwinsPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.TwinsMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Spazmatism.SpazCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Spazmatism.SpazCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechEye", shop, ref nextSlot, 400000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, ref nextSlot, 1000000);
					}
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeTwins", shop, ref nextSlot, 10000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, ref nextSlot, 10000);
					}
					NPCHelper.SafelySetCrossModItem(calamityMod, "Arbalest", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "TwinRangs", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "FusedLens", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ref nextSlot); //Mechanical Spikes
					}
				}
				if (customShops.ContainsKey(NPCString.Spazmatism))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Spazmatism])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region SkeletronPrime
		/// <summary>
		/// Skeletron Prime's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void SkeletronPrime(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.MechanicalSkull);
				shop.item[nextSlot].shopCustomPrice = 250000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HallowedBar);
				shop.item[nextSlot].shopCustomPrice = 400 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SoulofFright);
				shop.item[nextSlot].shopCustomPrice = 800 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SkeletronPrimeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SkeletronPrimeTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MechanicalBatteryPiece);
					shop.item[nextSlot].shopCustomPrice = 5000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.SkeletronPrimePetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SkeletronPrimeMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.SkeletronPrime.SPCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechSkull", shop, ref nextSlot, 400000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, ref nextSlot, 1000000);
					}
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeSkeletronPrime", shop, ref nextSlot, 10000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, ref nextSlot, 10000);
					}
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "RefractorBlaster", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "ReinforcedPlating", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.SkeletronPrime))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.SkeletronPrime])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Plantera
		/// <summary>
		/// Plantera's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Plantera(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				/*shop.item[nextSlot].SetDefaults(ItemID.PlanteraBulb);
				shop.item[nextSlot].shopCustomPrice = 300000; //Made up value
				nextSlot++;*/
				shop.item[nextSlot].SetDefaults(ItemID.TempleKey);
				shop.item[nextSlot].shopCustomPrice = 5000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.GrenadeLauncher);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.VenusMagnum);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NettleBurst);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LeafBlower);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FlowerPow);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WaspGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Seedler);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PygmyStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ThornHook);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.1);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TheAxe);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.02);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Seedling);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PlanteraMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PlanteraTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.SporeSac);
					shop.item[nextSlot].shopCustomPrice = 40000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.PlanteraPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.PlanteraMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxPlantera);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWPlantera);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.JungleGrassSeeds);
					shop.item[nextSlot].shopCustomPrice = 30 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Plantera.PlCostumeBackpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "PlanterasFruit", shop, ref nextSlot, 500000); //Match the Mutant's shop
				}

				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgePlantera", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "LivingShard", shop, ref nextSlot);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BloomStone", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlossomFlux", shop, ref nextSlot, 0.1f);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "Dicer", shop, ref nextSlot, 0.1f); //The Dicer

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "MagicalBulb", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "PottedPalMinionItem", shop, ref nextSlot, 0.44f); //Potted Pal
				}
				if (ModLoader.TryGetMod("QwertyMod", out Mod qwertyMod))
				{
					NPCHelper.SafelySetCrossModItem(qwertyMod, "VitallumCoreUncharged", shop, ref nextSlot); //Vitallum Core
				}
				if (customShops.ContainsKey(NPCString.Plantera))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Plantera])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Golem
		/// <summary>
		/// Golem's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Golem(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.LihzahrdPowerCell);
				shop.item[nextSlot].shopCustomPrice = 350000; //Made up value
				nextSlot++;

				shop.item[nextSlot].SetDefaults(ItemID.Picksaw);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(43200 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BeetleHusk);
				shop.item[nextSlot].shopCustomPrice = 5000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Stynger);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.StyngerBolt);
				shop.item[nextSlot].shopCustomPrice = 75;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PossessedHatchet);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SunStone);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EyeoftheGolem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HeatRay);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.StaffofEarth);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.GolemFist);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.GolemMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.GolemTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.ShinyStone);
					shop.item[nextSlot].shopCustomPrice = 50000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.GolemPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.GolemMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss4);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.LihzahrdBrick);
					shop.item[nextSlot].shopCustomPrice = 2500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.LihzahrdAltar);
					shop.item[nextSlot].shopCustomPrice = 60 * 5 * 1000; //sells for 60 copper, but that seems way to cheap for an item that you should only have one of.
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "LihzahrdPowerCell2", shop, ref nextSlot, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeGolem", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "AegisBlade", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "RockSlide", shop, ref nextSlot, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "ComputationOrb", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "LihzahrdTreasureBox", shop, ref nextSlot);
					}
				}
				if (customShops.ContainsKey(NPCString.Golem))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Golem])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region EmpressOfLight
		/// <summary>
		/// Empress of Light's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void EmpressOfLight(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.EmpressButterfly); //Prismatic Lacewing
				shop.item[nextSlot].shopCustomPrice = 400000; //Sell value * 5 = 250000
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FairyQueenMagicItem); //Nightglow
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);  //Formula: (Sell value / drop chance); It would be 200000 in this case
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PiercingStarlight); //Starlight
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RainbowWhip); //Kaleidoscope
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FairyQueenRangedItem); //Eventide
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RainbowWings); //Empress Wings
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.07);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HallowBossDye); //Prismatic Dye
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SparkleGuitar); //Stellar Tune
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RainbowCursor); //Rainbow Cursor
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				if (BossesAsNPCsWorld.daytimeEoLDefeated)
				{
					shop.item[nextSlot].SetDefaults(ItemID.EmpressBlade); //Terraprisma
					shop.item[nextSlot].shopCustomPrice = 200000 * 50; //Special case since it is technically a "100% drop chance".
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.FairyQueenMask); //Empress of Light Mask
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FairyQueenTrophy); //Empress of Light Trophy
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;
				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.EmpressFlightBooster); //Soaring Insignia
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FairyQueenPetItem); //Jewel of Light
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.FairyQueenMasterTrophy); //Empress of Light Relic
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxEmpressOfLight);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					if (Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.HolyWater);
						shop.item[nextSlot].shopCustomPrice = 200; //For some reason Holy Water is double as valuable than Unholy/Blood Water.
						nextSlot++;
					}
					int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
					if (steampunker >= 0 && NPC.downedMechBossAny)
					{
						shop.item[nextSlot].SetDefaults(ItemID.BlueSolution);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "PrismaticPrimrose", shop, ref nextSlot, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "PrecisionSeal", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "EmpressSquireMinionItem", shop, ref nextSlot, 0.34f); //Chalice of the Empress
				}
				if (customShops.ContainsKey(NPCString.EmpressOfLight))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.EmpressOfLight])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region DukeFishron
		/// <summary>
		/// Duke Fishron's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void DukeFishron(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.TruffleWorm);
				shop.item[nextSlot].shopCustomPrice = 400000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BubbleGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Flairon);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RazorbladeTyphoon);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TempestStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Tsunami);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FishronWings);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.07);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DukeFishronMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DukeFishronTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.ShrimpyTruffle);
					shop.item[nextSlot].shopCustomPrice = 50000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DukeFishronPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DukeFishronMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxDukeFishron);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.DukeFishron.DFCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "TruffleWorm2", shop, ref nextSlot, 600000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeDukeFishron", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DukesDecapitator", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BrinyBaron", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "FishStick", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "MutantAntibodies", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("QwertyMod", out Mod qwertyMod))
				{
					NPCHelper.SafelySetCrossModItem(qwertyMod, "BubbleBrewerBaton", shop, ref nextSlot, 0.33f);
					NPCHelper.SafelySetCrossModItem(qwertyMod, "Cyclone", shop, ref nextSlot, 0.33f);
					NPCHelper.SafelySetCrossModItem(qwertyMod, "Whirlpool", shop, ref nextSlot, 0.33f);
				}
				if (customShops.ContainsKey(NPCString.DukeFishron))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.DukeFishron])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Betsy
		/// <summary>
		/// Betsy's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Betsy(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.DD2ElderCrystal);
				shop.item[nextSlot].shopCustomPrice = 40000;
				nextSlot++;
				if (BossesAsNPCsWorld.downedDarkMage)
				{
					shop.item[nextSlot].SetDefaults(ItemID.WarTable);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.1);  //Formula: (Sell value / drop chance))
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.WarTableBanner);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.1);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PetDragon); //Dragon Egg
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PetGato); //Gato Egg
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossMaskDarkMage);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossTrophyDarkmage);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
					nextSlot++;
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.DarkMageBookMountItem);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.DarkMageMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}
				if (BossesAsNPCsWorld.downedOgre)
				{
					shop.item[nextSlot].SetDefaults(ItemID.ApprenticeScarf);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SquireShield);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.HuntressBuckler);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MonkBelt);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BookStaff); //Tome of Infinite Wisdom
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PhoenixBow); //Phantom Phoenix
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2SquireDemonSword); //Brand of the Inferno
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MonkStaffT1); //Sleepy Octopod
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MonkStaffT2); //Ghastly Glaive
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PetGhost); //Creeper Egg
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossMaskOgre);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossTrophyOgre);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
					nextSlot++;
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.DD2OgrePetItem);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.OgreMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.DD2BetsyBow); //Aerial Bane
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);  //Formula: (Sell value * 2) * ((1/drop chance)/2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MonkStaffT3); //Sky Dragon's Fury
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ApprenticeStaffT3); //Betsy's Wrath
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DD2SquireBetsySword); //Flying Dragon
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BetsyWings);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.07);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BossMaskBetsy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BossTrophyBetsy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DD2BetsyPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BetsyMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxDD2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}

			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					if (BossesAsNPCsWorld.downedDarkMage)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "ForbiddenTome", shop, ref nextSlot, 50000); //Match the Abominationn's shop
					}

					if (BossesAsNPCsWorld.downedOgre)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "BatteredClub", shop, ref nextSlot, 150000); //Match the Abominationn's shop
					}

					NPCHelper.SafelySetCrossModItem(fargosMutant, "BetsyEgg", shop, ref nextSlot, 400000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DragonBreath", shop, ref nextSlot, 0.1f); //Dragon's Breath

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "BetsysHeart", shop, ref nextSlot); //Betsy's Heart
					}
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "BetsyScale", shop, ref nextSlot);
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "FlameCore", shop, ref nextSlot); //Betsy's Flame
					}
				}
				if (ModLoader.TryGetMod("PboneUtils", out Mod pbonesUtilities))
				{
					NPCHelper.SafelySetCrossModItem(pbonesUtilities, "DefendersCrystal", shop, ref nextSlot);
				}
				if (customShops.ContainsKey(NPCString.Betsy))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Betsy])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region LunaticCultist
		/// <summary>
		/// Lunatic Cultist's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void LunaticCultist(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.LunarCraftingStation); //Ancient Manipulator
				shop.item[nextSlot].shopCustomPrice = 100000; //made up value
				nextSlot++;
				if (NPC.downedTowerSolar)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentSolar);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				if (NPC.downedTowerVortex)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentVortex);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				if (NPC.downedTowerNebula)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentNebula);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				if (NPC.downedTowerStardust)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentStardust);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.BossMaskCultist);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.AncientCultistTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.LunaticCultistPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.LunaticCultistMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss4);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CultistSummon", shop, ref nextSlot, 750000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeLunaticCultist", shop, ref nextSlot, 10000);
					if (Main.bloodMoon)
					{
						NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeBloodMoon", shop, ref nextSlot, 10000);
					}
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "CelestialRune", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(fargosSouls, "MutantsPact", shop, ref nextSlot); //Mutant's Pact
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistLazor", shop, ref nextSlot, 0.02f); //Mysterious Cultist Hood
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistBow", shop, ref nextSlot, 0.25f); //Lunatic Bow of Ice
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistSpear", shop, ref nextSlot, 0.25f); //Lunatic Spear of Fire
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistTome", shop, ref nextSlot, 0.25f); //Lunatic Spell of Ancient Light
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistStaff", shop, ref nextSlot, 0.25f); //Lunatic Staff of Lightning
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunaticHood", shop, ref nextSlot);  //Lunatic Hood of Command
					}
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "LunarSilk", shop, ref nextSlot);
				}
				if (customShops.ContainsKey(NPCString.LunaticCultist))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.LunaticCultist])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region MoonLord
		/// <summary>
		/// Moon Lord's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void MoonLord(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.CelestialSigil);
				shop.item[nextSlot].shopCustomPrice = 500000; //made up value
				nextSlot++;

				shop.item[nextSlot].SetDefaults(ItemID.PortalGun);
				shop.item[nextSlot].shopCustomPrice = 100000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LunarOre);
				shop.item[nextSlot].shopCustomPrice = 3000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Meowmere);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(200000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Terrarian);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.StarWrath);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(200000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SDMG);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LastPrism);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LunarFlareBook);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RainbowCrystalStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoonlordTurretStaff); //Lunar Portal Staff
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Celeb2); //Celebration Mk2
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MeowmereMinecart);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.1);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BossMaskMoonlord);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoonLordTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.GravityGlobe);
					shop.item[nextSlot].shopCustomPrice = 400000 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SuspiciousLookingTentacle);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.LongRainbowTrailWings);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MoonLordPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MoonLordMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxLunarBoss);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWMoonLord);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MoonLord.MLCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MoonLordLegs);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
					if (Main.LocalPlayer.unlockedBiomeTorches)
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeHeadpiece>());
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeBodypiece>());
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeLegpiece>());
						shop.item[nextSlot].shopCustomPrice = 50000;
						nextSlot++;
					}
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CelestialSigil2", shop, ref nextSlot, 1000000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMoonLord", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CelestialOnion", shop, ref nextSlot, 100000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "UtensilPoker", shop, ref nextSlot, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DeviousAestheticus", shop, ref nextSlot, 0.05f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "GalacticGlobe", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "TrueThirdEye", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Cosmic_Key", shop, ref nextSlot, 100000);
				}
				if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage))
				{
					NPCHelper.SafelySetCrossModItem(magicStorage, "RadiantJewel", shop, ref nextSlot, 0.05f);
				}
				if (customShops.ContainsKey(NPCString.MoonLord))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.MoonLord])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Dreadnautilus
		/// <summary>
		/// Dreadnautilus' shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Dreadnautilus(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.BloodMoonStarter); //Bloody Tear
				shop.item[nextSlot].shopCustomPrice = 60000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BunnyHood);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.0133);  //Formula: (Sell value /drop chance);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PedguinHat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PedguinShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PedguinPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KiteBunnyCorrupt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.04);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KiteBunnyCrimson);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.04);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TopHat);
				shop.item[nextSlot].shopCustomPrice = 2000 * 5; //Technically a 90% drop chance, but in certain cases you could sell the hat for more than you bought it
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TheBrideHat);
				shop.item[nextSlot].shopCustomPrice = 1000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TheBrideDress);
				shop.item[nextSlot].shopCustomPrice = 1000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoneyTrough);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.005 / 2); //0.5% from Blood Zombies & Dripplers. Not using the 6.67% from Zombie Merman & Wandering Eye Fish
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SharkToothNecklace);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.0067 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
				shop.item[nextSlot].shopCustomPrice = 500 * 5 * 2;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BloodRainBow);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.VampireFrogStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BloodFishingRod); //Chum Caster
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.0417);
				nextSlot++;
				if (!NPC.combatBookWasUsed)
				{
					shop.item[nextSlot].SetDefaults(ItemID.CombatBook); //Advanced Combat Techniques
					shop.item[nextSlot].shopCustomPrice = 500000; //Normally no value
					nextSlot++;
				}
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.KOCannon);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(35000 / 0.01 / 10); //Dropped by ANY enemy during a Blood Moon
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Bananarang);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.0333);
					nextSlot++;
					// No Trifold Map lol
					shop.item[nextSlot].SetDefaults(ItemID.BloodHamaxe); //Haemorrhaxe
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SharpTears); //Blood Thorn
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DripplerFlail); //Drippler Crippler
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SanguineStaff);
					shop.item[nextSlot].shopCustomPrice = 50000 * 5; //50% drop chance in normal mode, but I wanted it to be more expensive
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.BloodMoonMonolith);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1111);
				nextSlot++;
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxEerie);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBloodMoon);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousLookingLure", shop, ref nextSlot, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodUrchin", shop, ref nextSlot, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "HemoclawCrab", shop, ref nextSlot, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodSushiPlatter", shop, ref nextSlot, 200000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "BloodOrb", shop, ref nextSlot, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BouncingEyeball", shop, ref nextSlot, (0.025f * 2f));
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "SqueakyToy", shop, ref nextSlot, 0.1f);
						NPCHelper.SafelySetCrossModItem(fargosSouls, "DreadShell", shop, ref nextSlot, 0.2f);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodDrop", shop, ref nextSlot); //Bloody Drop
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodyRifle", shop, ref nextSlot, 0.125f); //Bloodshot Rifle
				}
				if (ModLoader.TryGetMod("ItReallyMustBe", out Mod dreadnautilusIsABoss))
				{
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "FunnyBait", shop, ref nextSlot); //Blood Bait
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadPistol", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusTrophy", shop, ref nextSlot, 0.1f);

					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "BloodyCarKey", shop, ref nextSlot, 0.25f);
						NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusRelic", shop, ref nextSlot, 0.1f);
					}
				}
				if (customShops.ContainsKey(NPCString.Dreadnautilus))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Dreadnautilus])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Mothron
		/// <summary>
		/// Mothron's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Mothron(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.SolarTablet);
				shop.item[nextSlot].shopCustomPrice = 20000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EyeSpring);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.0667);  //Formula: (Sell value /drop chance);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrokenBatWing);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoonStone);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.0286 / 4);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NeptunesShell);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.02 / 4);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Steak);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.01 / 6);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DeathSickle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButchersChainsaw);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButcherMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButcherApron);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButcherPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DeadlySphereStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ToxicFlask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DrManFlyMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.0396 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DrManFlyLabCoat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.0396 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NailGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.04);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Nail);
				shop.item[nextSlot].shopCustomPrice = 100; //Match the price of the Arm's Dealer
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PsychoKnife);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.025);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrokenHeroSword);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TheEyeOfCthulhu);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(125000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MothronWings);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.05);
				nextSlot++;
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxEclipse);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBloodMoon);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MothronEgg", shop, ref nextSlot, 150000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "SolarVeil", shop, ref nextSlot);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DefectiveSphere", shop, ref nextSlot, 0.2f);
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "SqueyereMinionItem", shop, ref nextSlot, 0.1f); //Crest of Eyes
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Broken_Hero_GunParts", shop, ref nextSlot, 0.25f);
				}
				if (customShops.ContainsKey(NPCString.Mothron))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Mothron])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Pumpking
		/// <summary>
		/// Pumpking's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void Pumpking(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.PumpkinMoonMedallion);
				shop.item[nextSlot].shopCustomPrice = 150000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ScarecrowHat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.033); //Using the highest drop chances
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ScarecrowShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.033);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ScarecrowPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.033);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.JackOLanternMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SpookyWood);
				shop.item[nextSlot].shopCustomPrice = 5000; //Made up value
				nextSlot++;
				if (NPC.downedHalloweenTree)
				{
					shop.item[nextSlot].SetDefaults(ItemID.SpookyHook);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SpookyTwig);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.StakeLauncher);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Stake);
					shop.item[nextSlot].shopCustomPrice = 15; //Same price as Arms Dealer/Witch Doctor
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CursedSapling);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.NecromanticScroll);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MourningWoodTrophy);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
					nextSlot++;
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WitchBroom);
						shop.item[nextSlot].shopCustomPrice = 50000 * 5;
						nextSlot++;
					}
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.SpookyWoodMountItem); //Hexxed Branch
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.MourningWoodMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}

				shop.item[nextSlot].SetDefaults(ItemID.TheHorsemansBlade);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BatScepter);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BlackFairyDust);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SpiderEgg);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RavenStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CandyCornRifle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CandyCorn);
				shop.item[nextSlot].shopCustomPrice = 5; //Same price as Arms Dealer
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.JackOLanternLauncher);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ExplosiveJackOLantern);
				shop.item[nextSlot].shopCustomPrice = 15; //Same price as Arms Dealer
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ScytheWhip); //Dark Harvest
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PumpkingTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
				nextSlot++;
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.PumpkingPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.PumpkingMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxPumpkinMoon);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.GoodieBag);
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeShoes>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{

				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					if (NPC.downedHalloweenTree)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "SpookyBranch", shop, ref nextSlot, 200000); //Match the Abominationn's shop
					}
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousLookingScythe", shop, ref nextSlot, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "PumpkingsCape", shop, ref nextSlot, 0.2f); //Pumpking's Cape
					}
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "GoldenRogueSquireMinionItem", shop, ref nextSlot, 0.13f); //Golden Rogue Crest
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SpookyCore", shop, ref nextSlot, 0.07f); //Spooky Emblem
				}
				if (customShops.ContainsKey(NPCString.Pumpking))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Pumpking])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region IceQueen
		/// <summary>
		/// Ice Queen's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void IceQueen(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.NaughtyPresent);
				shop.item[nextSlot].shopCustomPrice = 150000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ElfHat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.017);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ElfShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.017);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ElfPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.017);
				nextSlot++;
				if (NPC.downedChristmasTree)
				{
					shop.item[nextSlot].SetDefaults(ItemID.ChristmasTreeSword);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.078);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.ChristmasHook);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.078);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Razorpine);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.078);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.FestiveWings);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.017);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.EverscreamTrophy);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
					nextSlot++;
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.EverscreamPetItem); //Shrub Star
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.EverscreamMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}
				if (NPC.downedChristmasSantank)
				{
					shop.item[nextSlot].SetDefaults(ItemID.EldMelter); //Elf Melter, lol Re-Logic misspelled it.
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.ChainGun);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SantaNK1Trophy);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
					nextSlot++;
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.SantankMountItem); //Toy Tank
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.SantankMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.BlizzardStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.08);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SnowmanCannon);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.08);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NorthPole);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.08);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BabyGrinchMischiefWhistle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.017); //Has no value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ReindeerBells);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.01);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.IceQueenTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
				nextSlot++;
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.IceQueenPetItem); //Frozen Crown
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.IceQueenMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxFrostMoon);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.Present);
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeCape>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}

			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
				{
					if (NPC.downedChristmasTree)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "FestiveOrnament", shop, ref nextSlot, 200000); //Match the Abominationn's shop
					}
					if (NPC.downedChristmasSantank)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "NaughtyList", shop, ref nextSlot, 200000); //Match the Abominationn's shop
					}
					NPCHelper.SafelySetCrossModItem(fargosMutant, "IceKingsRemains", shop, ref nextSlot, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "IceQueensCrown", shop, ref nextSlot, 0.2f);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SantankScrap", shop, ref nextSlot); //Mechanical Scrap
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "IceSentry", shop, ref nextSlot, 0.1f); //Frozen Queen's Staff
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "FrostCube", shop, ref nextSlot, 0.07f); //Frozen Queen's Core
				}
				if (customShops.ContainsKey(NPCString.IceQueen))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.IceQueen])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region MartianSaucer
		/// <summary>
		/// Martian Saucer's shop.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		public static void MartianSaucer(Chest shop, ref int nextSlot)
		{
			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.MartianConduitPlating);
				shop.item[nextSlot].shopCustomPrice = 100; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianCostumeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianCostumeShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianCostumePants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianUniformHelmet);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianUniformTorso);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianUniformPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrainScrambler);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.01);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LaserDrill);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7); //Special case to make it cheaper
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChargedBlasterCannon);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7); //Special case to make it cheaper
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.AntiGravityHook);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.013 / 7); //Special case to make it cheaper
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Xenopopper);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.XenoStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LaserMachinegun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ElectrosphereLauncher);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.InfluxWaver);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CosmicCarKey);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianSaucerTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MartianPetItem); //Cosmic Skateboard
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.UFOMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxMartians);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "RunawayProbe", shop, ref nextSlot, 500000); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MartianMemoryStick", shop, ref nextSlot, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "ShockGrenade", shop, ref nextSlot);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Wingman", shop, ref nextSlot, 0.14f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "NullificationRifle", shop, ref nextSlot, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "SaucerControlConsole", shop, ref nextSlot, 0.2f);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SuperDartLauncher", shop, ref nextSlot, 0.01f * 6);
				}
				if (customShops.ContainsKey(NPCString.MartianSaucer))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.MartianSaucer])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion

		#region Goblin Tinkerer
		/// <summary>
		/// Goblin Tinkerer's extra shop. These shop items are affected by the shop price scaling config.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="shopMulti">The float multiplier for the shop price scaling. It is the config number / 100f</param>
		public static void GoblinTinkerer(Chest shop, ref int nextSlot, float shopMulti)
		{
			shop.item[nextSlot].SetDefaults(ItemID.GoblinBattleStandard);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 * shopMulti); //Made up value
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Harpoon);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5400 / 0.005 / 5 * shopMulti);  //Special case to make it cheaper
			nextSlot++;
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				NPCHelper.SafelySetCrossModItem(calamityMod, "PlasmaRod", shop, ref nextSlot, (0.07f * 5), shopMulti);
			}
			if (BossesAsNPCsWorld.downedGoblinSummoner)
			{
				shop.item[nextSlot].SetDefaults(ItemID.ShadowFlameHexDoll);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ShadowFlameBow);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ShadowFlameKnife);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti);
				nextSlot++;

				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
				{
					if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "ShadowflameIcon", shop, ref nextSlot, 0.01f, shopMulti); //10 gold
					}
					if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod2))
					{
						NPCHelper.SafelySetCrossModItem(calamityMod2, "BurningStrife", shop, ref nextSlot, (0.33f * 5), shopMulti);
						NPCHelper.SafelySetCrossModItem(calamityMod2, "TheFirstShadowflame", shop, ref nextSlot, (0.33f * 5), shopMulti);
					}
					if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
					{
						bool eternityMode = (bool)fargosSouls.Call("EternityMode");
						if (eternityMode)
						{
							NPCHelper.SafelySetCrossModItem(fargosSouls, "WretchedPouch", shop, ref nextSlot, (0.2f * 5), shopMulti);
						}
					}
					if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
					{
						NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "GoblinGunnerMinionItem", shop, ref nextSlot, (0.44f * 5), shopMulti); //Goblin Radio Beacon
					}
					if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameBMask", shop, ref nextSlot, 1f, shopMulti); //Shadowflare Mask
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameChestplate", shop, ref nextSlot, 1f, shopMulti); //Shadowflare Robe
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameGreaves", shop, ref nextSlot, 1f, shopMulti); //Shadowflare Greaves
					}
					if (customShops.ContainsKey(NPCString.GoblinTinkerer))
					{
						foreach (KeyValuePair<int, object[]> set in customShops[NPCString.GoblinTinkerer])
						{
							if (((Func<bool>)set.Value[1]).Invoke())
							{
								shop.item[nextSlot].SetDefaults(set.Key);
								shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
								nextSlot++;
							}
						}
					}
				}
			}
		}
		#endregion

		#region Pirate
		/// <summary>
		/// Pirate's extra shop. These shop items are affected by the shop price scaling config.
		/// </summary>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="nextSlot">The ref nextSlot. Pass ref nextSlot in most cases.</param>
		/// <param name="shopMulti">The float multiplier for the shop price scaling. It is the config number / 100f</param>
		public static void Pirate(Chest shop, ref int nextSlot, float shopMulti)
		{
			shop.item[nextSlot].SetDefaults(ItemID.PirateMap);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti); //Made up value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("PirateFlag").Type);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 * shopMulti); //Match the Deviantt's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.CoinGun);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.0025 * shopMulti);  //Formula: (Sell value / drop chance)
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LuckyCoin);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.005 * shopMulti);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.DiscountCard);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.01 * shopMulti);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PirateStaff);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.01 * shopMulti);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GoldRing);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.02 * shopMulti);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PirateMinecart);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.05 * shopMulti);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Cutlass);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(36000 / 0.1 * shopMulti);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.FlyingDutchmanTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 * 0.1 * shopMulti);
			nextSlot++;
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.PirateShipMountItem); //Black Spot
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25 * shopMulti);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FlyingDutchmanMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 * 5 * shopMulti);
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant2) && ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant2, "GoldenDippingVat", shop, ref nextSlot, (0.07f * 5), shopMulti);
						NPCHelper.SafelySetCrossModItem(fargosSouls, "SecurityWallet", shop, ref nextSlot, (0.1f * 5), shopMulti);
					}
				}
				if (customShops.ContainsKey(NPCString.Pirate))
				{
					foreach (KeyValuePair<int, object[]> set in customShops[NPCString.Pirate])
					{
						if (((Func<bool>)set.Value[1]).Invoke())
						{
							shop.item[nextSlot].SetDefaults(set.Key);
							shop.item[nextSlot].shopCustomPrice = PostCalcItemValue(shop, ref nextSlot, (int)set.Value[0]);
							nextSlot++;
						}
					}
				}
			}
		}
		#endregion
	}
}