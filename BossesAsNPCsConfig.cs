using System;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BossesAsNPCs
{
	/// <summary>
	/// ExampleConfigServer has Server-wide effects. Things that happen on the server, on the world, or influence autoload go here
	/// ConfigScope.ServerSide ModConfigs are SHARED from the server to all clients connecting in MP.
	/// </summary>
	public class BossesAsNPCsConfigServer : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		//General Options
		[Header("HeaderGeneral")]

		//Boost Town NPC Spawn Rates
		[DefaultValue(false)]
		public bool BoostTownNPCRates { get; set; }

		//Sell Expert Mode Items in Non-Expert Worlds
		[DefaultValue(false)]
		public bool SellExpertMode { get; set; }

		//Sell Master Mode Items in Non-Master Worlds
		[DefaultValue(false)]
		public bool SellMasterMode { get; set; }

		//Sell Extra Items
		[DefaultValue(true)]
		public bool SellExtraItems { get; set; }

		//Shop Price Scaling
		[Increment(5)]
		[Range(25, 400)]
		[DefaultValue(100)]
		[Slider]
		public int ShopPriceScaling { get; set; }

		//BossesAsNPCs Cross Mod Support
		[DefaultValue(true)]
		public bool TownNPCsCrossModSupport { get; set; }

		//Catch Town NPCs
		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

		//All In One NPC Mode
		[BackgroundColor(39, 77, 253)]
		[DefaultValue(AllInOneOptions.Off)]
		[DrawTicks]
		[ReloadRequired]
		public AllInOneOptions AllInOneNPCMode { get; set; }

		//Spawn Options
		[Header("HeaderSpawn")]

		//King Slime Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnKingSlime { get; set; }

		//Eye of Cthulhu Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnEoC { get; set; }

		//Eater of Worlds Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnEoW { get; set; }

		//Brain of Cthulhu Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnBoC { get; set; }

		//Queen Bee Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnQueenBee { get; set; }

		//Skeletron Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnSkeletron { get; set; }

		//Deerclops Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnDeerclops { get; set; }

		//Wall of Flesh Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnWoF { get; set; }

		//Queen Slime Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnQueenSlime { get; set; }

		//The Destroyer Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnDestroyer { get; set; }

		//The Twins Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnTwins { get; set; }

		//Skeletron Prime Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnSkeletronPrime { get; set; }

		//Plantera Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnPlantera { get; set; }

		//Golem Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnGolem { get; set; }

		//Empress of Light Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnEoL { get; set; }

		//Duke Fishron Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnDukeFishron { get; set; }

		//Betsy Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnBetsy { get; set; }

		//Lunatic Cultist Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnLunaticCultist { get; set; }

		//Moon Lord Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnMoonLord { get; set; }

		//Goblin Tinkerer Sells Goblin Army Items
		[DefaultValue(true)]
		public bool GoblinSellInvasionItems { get; set; }

		//Pirate Sells Pirate Invasion Items
		[DefaultValue(true)]
		public bool PirateSellInvasionItems { get; set; }

		//Dreadnautilus Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnDreadnautilus { get; set; }

		//Mothron Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnMothron { get; set; }

		//Pumpking Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnPumpking { get; set; }

		//Ice Queen Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnIceQueen { get; set; }

		//Martian Saucer Can Spawn
		[DefaultValue(true)]
		public bool CanSpawnMartianSaucer { get; set; }

		public enum AllInOneOptions
		{
			Off,
			Mixed,
			OnlyOne
		}

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
				message = Language.GetTextValue("Mods.BossesAsNPCs.NPCDialog.Config.Server.MultiplayerMessage");
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
		/* */
	}
}