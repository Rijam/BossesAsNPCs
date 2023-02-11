using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossesAsNPCs
{
	// This is a dummy biome to allow the Graveyard to be a likable biome for Town NPCs.
	// Has the side effect of possibly overriding the happiness for other biomes.
	public class GraveyardBiome : ModBiome
	{
		//public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(Language.GetTextValue("Bestiary_Biomes.Graveyard"));
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{
			return player.ZoneGraveyard;
		}
	}
	/*// This is a dummy biome to allow the Sky to be a likable biome for Town NPCs.
	// Has the side effect of possibly overriding the happiness for other biomes.
	public class SkyBiome : ModBiome
	{
		public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky");
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{
			return player.ZoneSkyHeight;
		}
	}
	// This is a dummy biome to allow the Underworld to be a likable biome for Town NPCs.
	// Has the side effect of possibly overriding the happiness for other biomes.
	public class UnderworldBiome : ModBiome
	{
		public override bool IsPrimaryBiome => true; // Allows this biome to impact NPC prices

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Underworld");
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player)
		{
			return player.ZoneUnderworldHeight;
		}
	}*/
}