using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossesAsNPCs;
using Terraria.GameContent.Events;

namespace BossesAsNPCs.Items
{
	public class DebugMethodTester : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return false;
		}
		public override string Texture => "Terraria/Images/Item_" + ItemID.BlueSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Method Tester");
			Tooltip.SetDefault("Displays the custom downed bools\nLeft click to display them in chat");
		}

		public override void SetDefaults()
		{
			Item.color = Color.RoyalBlue;
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.White;
			Item.value = 0;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item9;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player)
		{
			Main.NewText("DD2Event.DownedInvasionT3 " + DD2Event.DownedInvasionT3);
			Main.NewText("daytimeEoLDefeated " + BossesAsNPCsWorld.daytimeEoLDefeated);
			Main.NewText("downedBetsy " + BossesAsNPCsWorld.downedBetsy);
			Main.NewText("downedDungeonGuardian " + BossesAsNPCsWorld.downedDungeonGuardian);
			Main.NewText("downedDarkMage " + BossesAsNPCsWorld.downedDarkMage);
			Main.NewText("downedOgre " + BossesAsNPCsWorld.downedOgre);
			Main.NewText("downedGoblinSummoner " + BossesAsNPCsWorld.downedGoblinSummoner);
			Main.NewText("downedDreadnautilus " + BossesAsNPCsWorld.downedDreadnautilus);
			Main.NewText("downedMothron " + BossesAsNPCsWorld.downedMothron);
			Main.NewText("downedEoW " + BossesAsNPCsWorld.downedEoW);
			Main.NewText("downedBoC " + BossesAsNPCsWorld.downedBoC);
			Main.NewText("downedWoF " + BossesAsNPCsWorld.downedWoF);
			return true;
		}
	}
	public class DebugMethodTester2 : DebugMethodTester
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return false;
		}
		public override string Texture => "Terraria/Images/Item_" + ItemID.RedSolution;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[c/ff0000:Debug] - Method Tester2");
			Tooltip.SetDefault("Resets the custom downed bools\nLeft click to reset");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
		}
		public override bool? UseItem(Player player)
		{
			BossesAsNPCsWorld.daytimeEoLDefeated = false;
			BossesAsNPCsWorld.downedBetsy = false;
			BossesAsNPCsWorld.downedDungeonGuardian = false;
			BossesAsNPCsWorld.downedDarkMage = false;
			BossesAsNPCsWorld.downedOgre = false;
			BossesAsNPCsWorld.downedGoblinSummoner = false;
			BossesAsNPCsWorld.downedDreadnautilus = false;
			BossesAsNPCsWorld.downedMothron = false;
			BossesAsNPCsWorld.downedEoW = false;
			BossesAsNPCsWorld.downedBoC = false;
			BossesAsNPCsWorld.downedWoF = false;
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
			return true;
		}
	}
}