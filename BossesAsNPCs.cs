using BossesAsNPCs.NPCs.TownNPCs;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossesAsNPCs
{
	public class BossesAsNPCs : Mod
	{
		internal static BossesAsNPCsConfigServer ConfigServer;
		internal static BossesAsNPCs Instance;
		public override void Unload()
		{
			NPCs.SetupShops.ClearCustomShops();
			ConfigServer = null;
			Instance = null;
		}
		public override void PostSetupContent()
		{
			if (ModLoader.TryGetMod("Census", out Mod censusMod))
			{
				censusMod.Call("TownNPCCondition", ModContent.NPCType<KingSlime>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.KingSlime"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EyeOfCthulhu>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.EyeOfCthulhu"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EaterOfWorlds>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.EaterOfWorlds"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<BrainOfCthulhu>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.BrainOfCthulhu"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<QueenBee>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.QueenBee"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Skeletron>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Skeletron"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Deerclops>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Deerclops"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<WallOfFlesh>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.WallOfFlesh"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<QueenSlime>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.QueenSlime"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<TheDestroyer>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TheDestroyer"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Retinazer>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TheTwins"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Spazmatism>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TheTwins"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<SkeletronPrime>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.SkeletronPrime"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Plantera>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Plantera"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Golem>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Golem"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EmpressOfLight>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.EmpressOfLight"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<DukeFishron>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.DukeFishron"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Betsy>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Betsy"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<LunaticCultist>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.LunaticCultist"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<MoonLord>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.MoonLord"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Dreadnautilus>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Dreadnautilus"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Mothron>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Mothron"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Pumpking>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Pumpking"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<IceQueen>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.IceQueen"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<MartianSaucer>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.MartianSaucer"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<TorchGod>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TorchGod"));
			}
		}

		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			if (args is null)
				throw new ArgumentNullException(nameof(args));

			if (args[0] is not string function)
				throw new ArgumentException("Expected a function name for the first argument");

			void CheckArgsLength(int expected, params string[] argNames)
			{
				if (args.Length != expected)
					throw new ArgumentOutOfRangeException($"Expected {expected} arguments for Mod.Call(\"{function}\", {string.Join(",", argNames)}), got {args.Length} arguments instead");
			}

			switch (function)
			{
				case "downedBetsy":
					return BossesAsNPCsWorld.downedBetsy;
				case "downedDungeonGuardian":
					return BossesAsNPCsWorld.downedDungeonGuardian;
				case "downedDarkMage":
					return BossesAsNPCsWorld.downedDarkMage;
				case "downedOgre":
					return BossesAsNPCsWorld.downedOgre;
				case "downedGoblinSummoner":
					return BossesAsNPCsWorld.downedGoblinSummoner;
				case "downedDreadnautilus":
					return BossesAsNPCsWorld.downedDreadnautilus;
				case "downedMothron":
					return BossesAsNPCsWorld.downedMothron;
				case "SellExpertMode":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode;
				case "SellMasterMode":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode;
				case "SellExtraItems":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems;
				case "shopMulti":
					return (ModContent.GetInstance<BossesAsNPCsConfigServer>().ShopPriceScaling / 100f);
				case "TownNPCsCrossModSupport":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
				case "CatchNPCs":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
				case "AllInOneNPCMode":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode;
				case "GoblinSellInvasionItems":
                    return ModContent.GetInstance<BossesAsNPCsConfigServer>().GoblinSellInvasionItems;
				case "PirateSellInvasionItems":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().PirateSellInvasionItems;
				case "GetStatusShop1":
					Logger.Warn($"Function \"{function}\" is obsolete. Please use one of the \"AddToShop\" calls.");
					return NPCs.NPCHelper.StatusShop1();
				case "GetStatusShop2":
					Logger.Warn($"Function \"{function}\" is obsolete. Please use one of the \"AddToShop\" calls.");
					return NPCs.NPCHelper.StatusShop2();
				case "CanSpawn":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
					return args[1].ToString() switch
					{
						"KingSlime" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnKingSlime,
						"EyeOfCthulhu" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoC,
						"EoC" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoC,
						"EaterOfWorlds" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoW,
						"EoW" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoW,
						"BrainOfCthulhu" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBoC,
						"BoC" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBoC,
						"QueenBee" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnQueenBee,
						"Skeletron" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnSkeletron,
						"Deerclops" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDeerclops,
						"WallOfFlesh" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnWoF,
						"WoF" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBoC,
						"QueenSlime" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnQueenSlime,
						"TheDestroyer" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDestroyer,
						"Destroyer" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDestroyer,
						"TheTwins" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnTwins,
						"Twins" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnTwins,
						"SkeletronPrime" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnSkeletronPrime,
						"Plantera" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnPlantera,
						"Golem" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnGolem,
						"EmpressOfLight" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoL,
						"EoL" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoL,
						"DukeFishron" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDukeFishron,
						"Betsy" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBetsy,
						"LunaticCultist" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnLunaticCultist,
						"MoonLord" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMoonLord,
						"Dreadnautilus" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDreadnautilus,
						"Mothron" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMothron,
						"Pumpking" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnPumpking,
						"IceQueen" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnIceQueen,
						"MartianSaucer" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMartianSaucer,
						"TorchGod" => ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode > 0,
						"TheTorchGod" => ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode > 0,
						_ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs"),
					};
				case "AddToShop":
					switch (args[1].ToString())
					{
						case "DefaultPrice":
							CheckArgsLength(5, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString() });
							// string npc, int item, bool condition
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (Func<bool>)args[4]);
						case "CustomPrice":
							CheckArgsLength(6, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString() });
							// string npc, int item, bool condition, int customPrice
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (Func<bool>)args[4], (int)args[5]);
						case "WithDiv":
							CheckArgsLength(6, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString() });
							// string npc, int item, bool condition, float priceDiv
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (Func<bool>)args[4], (float)args[5]);
						case "WithDivAndMulti":
							CheckArgsLength(7, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString(), args[6].ToString() });
							// string npc, int item, bool condition, float priceDiv, float priceMulti
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (Func<bool>)args[4], (float)args[5], (float)args[6]);
						default:
							throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs");
					}
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by BossesAsNPCs");
			}
		}
	}
}