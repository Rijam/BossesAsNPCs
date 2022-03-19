using BossesAsNPCs.NPCs.TownNPCs;
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
	}
}