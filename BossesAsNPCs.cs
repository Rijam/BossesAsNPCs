using BossesAsNPCs.NPCs.TownNPCs;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
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
				censusMod.Call("TownNPCCondition", ModContent.NPCType<KingSlime>(), "Defeat King Slime");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EyeOfCthulhu>(), "Defeat the Eye of Cthulhu");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EaterOfWorlds>(), "Defeat the Eater of Worlds in a Corruption world,\nor in a Hardmode in a Crimson world");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<BrainOfCthulhu>(), "Defeat the Brain of Cthulhu in a Crimson world,\nor in a Hardmode in a Corruption world");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<QueenBee>(), "Defeat Queen Bee");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Skeletron>(), "Defeat Skeletron");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Deerclops>(), "Defeat Deerclops");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<WallOfFlesh>(), "Defeat the Wall of Flesh");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<QueenSlime>(), "Defeat Queen Slime");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<TheDestroyer>(), "Defeat The Destroyer");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Retinazer>(), "Defeat The Twins");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Spazmatism>(), "Defeat The Twins");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<SkeletronPrime>(), "Defeat Skeletron Prime");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Plantera>(), "Defeat Plantera");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Golem>(), "Defeat Golem");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EmpressOfLight>(), "Defeat the Empress of Light");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<DukeFishron>(), "Defeat Duke Fishron");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Betsy>(), "Defeat Betsy");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<LunaticCultist>(), "Defeat the Lunatic Cultist");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<MoonLord>(), "Defeat Moon Lord");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Pumpking>(), "Defeat the Pumpking");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<IceQueen>(), "Defeat the Ice Queen");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<MartianSaucer>(), "Defeat the Martian Saucer");
			}
		}
		public override void PostAddRecipes()
		{
			//Doesn't seem to work anymore
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<KingSlime>()] += 2;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<EyeOfCthulhu>()] = 2;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<EaterOfWorlds>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<BrainOfCthulhu>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<QueenBee>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Skeletron>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Deerclops>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<WallOfFlesh>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<QueenSlime>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<TheDestroyer>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Retinazer>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Spazmatism>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<SkeletronPrime>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Plantera>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Golem>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<QueenSlime>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<EmpressOfLight>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<DukeFishron>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Betsy>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<LunaticCultist>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<MoonLord>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Pumpking>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<IceQueen>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<MartianSaucer>()] = 4;
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