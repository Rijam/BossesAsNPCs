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
	[Label("$Mods.BossesAsNPCs.Config.Server.LabelBig")]
	public class BossesAsNPCsConfigServer : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		//General Options
		[Header("$Mods.BossesAsNPCs.Config.Server.HeaderGeneral")]

		//Boost Town NPC Spawn Rates
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelGeneral1")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipGeneral1")]
		[DefaultValue(false)]
		public bool BoostTownNPCRates { get; set; }

		//Sell Expert Mode Items in Non-Expert Worlds
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelGeneral2")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipGeneral2")]
		[DefaultValue(false)]
		public bool SellExpertMode { get; set; }

		//Sell Master Mode Items in Non-Master Worlds
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelGeneral3")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipGeneral3")]
		[DefaultValue(false)]
		public bool SellMasterMode { get; set; }

		//Sell Extra Items
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelGeneral4")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipGeneral4")]
		[DefaultValue(true)]
		public bool SellExtraItems { get; set; }

		//Shop Price Scaling
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelGeneral5")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipGeneral5")]
		[Increment(5)]
		[Range(25, 400)]
		[DefaultValue(100)]
		[Slider]
		public int ShopPriceScaling { get; set; }

		//Catch Town NPCs
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelGeneral6")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipGeneral6")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool CatchNPCs { get; set; }

		//Spawn Options
		[Header("$Mods.BossesAsNPCs.Config.Server.HeaderSpawn")]

		//King Slime Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnKS")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnKS")]
		[DefaultValue(true)]
		public bool CanSpawnKingSlime { get; set; }

		//Eye of Cthulhu Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnEoC")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnEoC")]
		[DefaultValue(true)]
		public bool CanSpawnEoC { get; set; }

		//Eater of Worlds Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnEoW")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnEoW")]
		[DefaultValue(true)]
		public bool CanSpawnEoW { get; set; }

		//Brain of Cthulhu Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnBoC")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnBoC")]
		[DefaultValue(true)]
		public bool CanSpawnBoC { get; set; }

		//Queen Bee Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnQB")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnQB")]
		[DefaultValue(true)]
		public bool CanSpawnQueenBee { get; set; }

		//Skeletron Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnSk")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnSk")]
		[DefaultValue(true)]
		public bool CanSpawnSkeletron { get; set; }

		//Deerclops Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnDc")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnDc")]
		[DefaultValue(true)]
		public bool CanSpawnDeerclops { get; set; }

		//Wall of Flesh Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnWoF")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnWoF")]
		[DefaultValue(true)]
		public bool CanSpawnWoF { get; set; }

		//Queen Slime Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnQS")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnQS")]
		[DefaultValue(true)]
		public bool CanSpawnQueenSlime { get; set; }

		//The Destroyer Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnDe")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnDe")]
		[DefaultValue(true)]
		public bool CanSpawnDestroyer { get; set; }

		//The Twins Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnTw")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnTw")]
		[DefaultValue(true)]
		public bool CanSpawnTwins { get; set; }

		//Skeletron Prime Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnSP")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnSP")]
		[DefaultValue(true)]
		public bool CanSpawnSkeletronPrime { get; set; }

		//Plantera Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnPl")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnPl")]
		[DefaultValue(true)]
		public bool CanSpawnPlantera { get; set; }

		//Golem Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnGo")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnGo")]
		[DefaultValue(true)]
		public bool CanSpawnGolem { get; set; }

		//Empress of Light Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnEoL")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnEoL")]
		[DefaultValue(true)]
		public bool CanSpawnEoL { get; set; }

		//Duke Fishron Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnDF")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnDF")]
		[DefaultValue(true)]
		public bool CanSpawnDukeFishron { get; set; }

		//Betsy Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnBe")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnBe")]
		[DefaultValue(true)]
		public bool CanSpawnBetsy { get; set; }

		//Lunatic Cultist Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnLC")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnLC")]
		[DefaultValue(true)]
		public bool CanSpawnLunaticCultist { get; set; }

		//Moon Lord Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnML")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnML")]
		[DefaultValue(true)]
		public bool CanSpawnMoonLord { get; set; }

		//Goblin Tinkerer Sells Goblin Army Items
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnGoblinSell")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnGoblinSell")]
		[DefaultValue(true)]
		public bool GoblinSellInvasionItems { get; set; }

		//Pirate Sells Pirate Invasion Items
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnPirateSell")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnPirateSell")]
		[DefaultValue(true)]
		public bool PirateSellInvasionItems { get; set; }

		//Pumpking Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnPk")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnPk")]
		[DefaultValue(true)]
		public bool CanSpawnPumpking { get; set; }

		//Ice Queen Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnIQ")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnIQ")]
		[DefaultValue(true)]
		public bool CanSpawnIceQueen { get; set; }

		//Martian Saucer Can Spawn
		[Label("$Mods.BossesAsNPCs.Config.Server.LabelSpawnMS")]
		[Tooltip("$Mods.BossesAsNPCs.Config.Server.TooltipSpawnMS")]
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
				message = Language.GetTextValue("Mods.BossesAsNPCs.NPCDialog.Config.Server.MultiplayerMessage");
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
		/* */
	}
}