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
			}
		}

		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			if (args is null)
				throw new ArgumentNullException(nameof(args));

			if (args[0] is not string function)
				throw new ArgumentException("Expected a function name for the first argument");

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
				case "GoblinSellInvasionItems":
                    return ModContent.GetInstance<BossesAsNPCsConfigServer>().GoblinSellInvasionItems;
				case "PirateSellInvasionItems":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().PirateSellInvasionItems;
				case "GetStatusShop1":
					return NPCs.NPCHelper.StatusShop1();
				case "GetStatusShop2":
					return NPCs.NPCHelper.StatusShop2();
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by BossesAsNPCs");
			}
		}
	}
}