using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BossesAsNPCs
{
	/// <summary>
	/// ExampleConfigServer has Server-wide effects. Things that happen on the server, on the world, or influence autoload go here
	/// ConfigScope.ServerSide ModConfigs are SHARED from the server to all clients connecting in MP.
	/// </summary>
	[Label("Bosses As NPCs Server Options")]
	public class BossesAsNPCsConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("[c/00FF00:General Options]")]

		[Label("[i:4080]   Boost Town NPC Spawn Rates")]
		[Tooltip("This option toggles if the Town NPC spawn rates will be boosted\n" +
			"  When On: Town NPC spawn rates WILL be boosted.\n" +
			"  When Off: Town NPC spawn rates WILL NOT be boosted.\n" +
			"    Turn On to boost Town NPC spawn rates.\n" +
			"Default value: Off")]
		[DefaultValue(false)]
		public bool BoostTownNPCRates { get; set; }

		[Label("[i:3090]   Sell Expert Mode Items in Non-Expert Worlds")]
		[Tooltip("This option toggles if the NPCs will sell\n" +
			"Expert Mode items in non-Expert Mode and non-Master Mode worlds.\n" +
			"  When On: Expert Mode items WILL be sold in ALL modes.\n" +
			"  When Off: Expert Mode items WILL ONLY be sold in Expert Mode and Master Mode.\n" +
			"    Turn On to get Expert Mode items in all modes.\n" +
			"Default value: Off")]
		[DefaultValue(false)]
		public bool SellExpertMode { get; set; }

		[Label("[i:4929]   Sell Master Mode Items in Non-Master Worlds")]
		[Tooltip("This option toggles if the NPCs will sell\n" +
			"Master Mode items in non-Master Mode worlds.\n" +
			"  When On: Master Mode items WILL be sold in ALL modes.\n" +
			"  When Off: Master Mode items WILL ONLY be sold in Master Mode.\n" +
			"    Turn On to get Master Mode items in all modes.\n" +
			"Default value: Off")]
		[DefaultValue(false)]
		public bool SellMasterMode { get; set; }

		[Label("[i:87]   Shop Price Scaling")]
		[Tooltip("This option sets the scaling for the prices in the Town NPCs' shops.\n" +
			"  25 means 1/4 (quarter) the normal price.\n" +
			"  50 means 1/2 (half) the normal price.\n" +
			"  100 means normal price.\n" +
			"  200 means 2x (double) the normal price.\n" +
			"  400 means 4x (quadruple) the normal price.\n" +
			"    Change this value if you want the shop to be cheaper or more expensive.\n" +
			"Default value: 100")]
		[Increment(5)]
		[Range(25, 400)]
		[DefaultValue(100)]
		[Slider]
		public int ShopPriceScaling { get; set; }

		[Label("[i:1991]   Catch Town NPCs")]
		[Tooltip("This option toggles if the Town NPCs added by this mod can be\n" +
			"caught with a Bug Net (Fargo's Mutant Mod style)\n" +
			"  When On: The Town NPCs CAN be caught.\n" +
			"  When Off: The Town NPCs CAN NOT be caught.\n" +
			"    Turn On to catch the Town NPCs.\n" +
			"Warning: caught NPC items will become unloaded if this option is Off.\n" +
			"Default value: Off\n" +
			"Requires a Reload.")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

		[Header("[c/00FF00:Spawn Options]")]

		[Label("[i:2493]   King Slime Can Spawn")]
		[Tooltip("This option toggles if the King Slime NPC can spawn.\n" +
			"  When On: The King Slime NPC WILL spawn.\n" +
			"  When Off: The King Slime NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the King Slime NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnKingSlime { get; set; }

		[Label("[i:2112]   Eye of Cthulhu Can Spawn")]
		[Tooltip("This option toggles if the Eye of Cthulhu NPC can spawn.\n" +
			"  When On: The Eye of Cthulhu NPC WILL spawn.\n" +
			"  When Off: The Eye of Cthulhu NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Eye of Cthulhu NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnEoC { get; set; }

		[Label("[i:2111]   Eater of Worlds Can Spawn")]
		[Tooltip("This option toggles if the Eater of Worlds NPC can spawn.\n" +
			"  When On: The Eater of Worlds NPC WILL spawn.\n" +
			"  When Off: The Eater of Worlds NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Eater of Worlds NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnEoW { get; set; }

		[Label("[i:2104]   Brain of Cthulhu Can Spawn")]
		[Tooltip("This option toggles if the Brain of Cthulhu NPC can spawn.\n" +
			"  When On: The Brain of Cthulhu NPC WILL spawn.\n" +
			"  When Off: The Brain of Cthulhu NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Brain of Cthulhu NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnBoC { get; set; }

		[Label("[i:2108]   Queen Bee Can Spawn")]
		[Tooltip("This option toggles if the Queen Bee NPC can spawn.\n" +
			"  When On: The Queen Bee NPC WILL spawn.\n" +
			"  When Off: The Queen Bee NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Queen Bee NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnQueenBee { get; set; }

		[Label("[i:1281]   Skeletron Can Spawn")]
		[Tooltip("This option toggles if the Skeletron NPC can spawn.\n" +
			"  When On: The Skeletron NPC WILL spawn.\n" +
			"  When Off: The Skeletron NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Skeletron NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnSkeletron { get; set; }

		[Label("[i:5109]   Deerclops Can Spawn")]
		[Tooltip("This option toggles if the Deerclops NPC can spawn.\n" +
			"  When On: The Deerclops NPC WILL spawn.\n" +
			"  When Off: The Deerclops NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Deerclops NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnDeerclops { get; set; }

		[Label("[i:2105]   Wall of Flesh Can Spawn")]
		[Tooltip("This option toggles if the Wall of Flesh NPC can spawn.\n" +
			"  When On: The Wall of Flesh NPC WILL spawn.\n" +
			"  When Off: The Wall of Flesh NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Wall of Flesh NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnWoF { get; set; }

		[Label("[i:4959]   Queen Slime Can Spawn")]
		[Tooltip("This option toggles if the Queen Slime NPC can spawn.\n" +
			"  When On: The Queen Slime NPC WILL spawn.\n" +
			"  When Off: The Queen Slime NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Queen Slime NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnQueenSlime { get; set; }

		[Label("[i:2113]   The Destroyer Can Spawn")]
		[Tooltip("This option toggles if the Destroyer NPC can spawn.\n" +
			"  When On: The Destroyer NPC WILL spawn.\n" +
			"  When Off: The Destroyer NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Destroyer NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnDestroyer { get; set; }

		[Label("[i:2106]   The Twins Can Spawn")]
		[Tooltip("This option toggles if the Retinazer & Spazmatism NPCs can spawn.\n" +
			"  When On: The Twins NPCs WILL spawn.\n" +
			"  When Off: The Twins NPCs WILL NOT spawn.\n" +
			"    Turn Off to stop the Twins NPCs from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnTwins { get; set; }

		[Label("[i:2107]   Skeletron Prime Can Spawn")]
		[Tooltip("This option toggles if the Skeletron Prime NPC can spawn.\n" +
			"  When On: The Skeletron Prime NPC WILL spawn.\n" +
			"  When Off: The Skeletron Prime NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Skeletron Prime NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnSkeletronPrime { get; set; }

		[Label("[i:2109]   Plantera Can Spawn")]
		[Tooltip("This option toggles if the Plantera NPC can spawn.\n" +
			"  When On: The Plantera NPC WILL spawn.\n" +
			"  When Off: The Plantera NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Plantera NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnPlantera { get; set; }

		[Label("[i:2110]   Golem Can Spawn")]
		[Tooltip("This option toggles if the Golem NPC can spawn.\n" +
			"  When On: The Golem NPC WILL spawn.\n" +
			"  When Off: The Golem NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Golem NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnGolem { get; set; }

		[Label("[i:4784]   Empress of Light Can Spawn")]
		[Tooltip("This option toggles if the Empress of Light NPC can spawn.\n" +
			"  When On: The Empress of Light NPC WILL spawn.\n" +
			"  When Off: The Empress of Light NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Empress of Light NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnEoL { get; set; }

		[Label("[i:2588]   Duke Fishron Can Spawn")]
		[Tooltip("This option toggles if the Duke Fishron NPC can spawn.\n" +
			"  When On: The Duke Fishron NPC WILL spawn.\n" +
			"  When Off: The Duke Fishron NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Duke Fishron NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnDukeFishron { get; set; }

		[Label("[i:3863]   Betsy Can Spawn")]
		[Tooltip("This option toggles if the Betsy NPC can spawn.\n" +
			"  When On: The Betsy NPC WILL spawn.\n" +
			"  When Off: The Betsy NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Betsy NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnBetsy { get; set; }

		[Label("[i:3372]   Lunatic Cultist Can Spawn")]
		[Tooltip("This option toggles if the Lunatic Cultist NPC can spawn.\n" +
			"  When On: The Lunatic Cultist NPC WILL spawn.\n" +
			"  When Off: The Lunatic Cultist NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Lunatic Cultist NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnLunaticCultist { get; set; }

		[Label("[i:3373]   Moon Lord Can Spawn")]
		[Tooltip("This option toggles if the Moon Lord NPC can spawn.\n" +
			"  When On: The Moon Lord NPC WILL spawn.\n" +
			"  When Off: The Moon Lord NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Moon Lord NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnMoonLord { get; set; }

		[Label("[i:361]   Goblin Tinkerer Sells Goblin Army Items")]
		[Tooltip("This option toggles if the Goblin Tinkerer can sell Goblin Army items.\n" +
			"  When On: The Goblin Tinkerer WILL sell Goblin Army items.\n" +
			"  When Off: The Goblin Tinkerer WILL NOT sell Goblin Army items.\n" +
			"    Turn Off to stop the Goblin Army items from the Goblin Tinkerer's shop.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool GoblinSellInvasionItems { get; set; }

		[Label("[i:1315]   Pirate Sells Pirate Invasion Items")]
		[Tooltip("This option toggles if the Pirate can sell Pirate Invasion items.\n" +
			"  When On: The Pirate WILL sell Pirate Invasion items.\n" +
			"  When Off: The Pirate WILL NOT sell Pirate Invasion items.\n" +
			"    Turn Off to stop the Pirate Invasion items from the Pirate's shop.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool PirateSellInvasionItems { get; set; }

		[Label("[i:1857]   Pumpking Can Spawn")]
		[Tooltip("This option toggles if the Pumpking NPC can spawn.\n" +
			"  When On: The Pumpking NPC WILL spawn.\n" +
			"  When Off: The Pumpking NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Pumpking NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnPumpking { get; set; }

		[Label("[i:4814]   Ice Queen Can Spawn")]
		[Tooltip("This option toggles if the Ice Queen NPC can spawn.\n" +
			"  When On: The Ice Queen NPC WILL spawn.\n" +
			"  When Off: The Ice Queen NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Ice Queen NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnIceQueen { get; set; }

		[Label("[i:2769]   Martian Saucer Can Spawn")]
		[Tooltip("This option toggles if the Martian Saucer NPC can spawn.\n" +
			"  When On: The Martian Saucer NPC WILL spawn.\n" +
			"  When Off: The Martian Saucer NPC WILL NOT spawn.\n" +
			"    Turn Off to stop the Martian Saucer NPC from spawning.\n" +
			"Default value: On")]
		[DefaultValue(true)]
		public bool CanSpawnMartianSaucer { get; set; }

		/* Not written by Rijam */
		public static bool IsPlayerLocalServerOwner(int whoAmI)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				RemoteClient client = Netplay.Clients[i];
				if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
				{
					return true;
				}
			}
			return false;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				return true;
			}

			if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = "You are not the server owner so you can not change this config!";
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
		/* */
	}
}