using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using BossesAsNPCs.NPCs.TownNPCs;

namespace BossesAsNPCs.Items
{
    //Code adapted from Fargo's Mutant Mod (CaughtNPCItem.cs)
    //and code adapted from Alchemist NPC (BrewerHorcrux.cs and similar)
    #region KingSlime
    public class CaughtKingSlime : ModItem
	{
		readonly private static string name = Language.GetTextValue("NPCName.KingSlime");
		readonly private static string whichNPCString = "KingSlime";
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
		}
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 10;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item44;
			Item.makeNPC = (short)ModContent.NPCType<KingSlime>();
			Item.tileBoost += 20;
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<KingSlime>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
		internal static void SpawnText(string npcName)
		{
			string chatmessage = Language.GetTextValue("Mods.BossesAsNPCs.UI.CapturedSpawnText1") + " " + npcName + " " + Language.GetTextValue("Mods.BossesAsNPCs.UI.CapturedSpawnText2");
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText(chatmessage, 50, 125, 255);
			}
			else
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(chatmessage), new Color(50, 125, 255));
			}
		}
	}
    #endregion
    #region Eye of Cthulhu
    public class CaughtEyeOfCthulhu : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.EyeofCthulhu");
		readonly private static string whichNPCString = "EyeOfCthulhu";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<EyeOfCthulhu>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<EyeOfCthulhu>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Eater of Worlds
	public class CaughtEaterOfWorlds : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.EaterofWorldsHead");
		readonly private static string whichNPCString = "EaterOfWorlds";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<EaterOfWorlds>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<EaterOfWorlds>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Brain of Cthulhu
	public class CaughtBrainOfCthulhu : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.BrainofCthulhu");
		readonly private static string whichNPCString = "BrainOfCthulhu";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<BrainOfCthulhu>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<BrainOfCthulhu>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Queen Bee
	public class CaughtQueenBee : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.QueenBee");
		readonly private static string whichNPCString = "QueenBee";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<QueenBee>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<QueenBee>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Skeletron
	public class CaughtSkeletron : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.SkeletronHead");
		readonly private static string whichNPCString = "Skeletron";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Skeletron>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Skeletron>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Deerclops
	public class CaughtDeerclops : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.Deerclops");
		readonly private static string whichNPCString = "Deerclops";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Deerclops>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Deerclops>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Wall of Flesh
	public class CaughtWallOfFlesh : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.WallofFlesh");
		readonly private static string whichNPCString = "WallOfFlesh";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<WallOfFlesh>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<WallOfFlesh>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Queen Slime
	public class CaughtQueenSlime : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.QueenSlimeBoss");
		readonly private static string whichNPCString = "QueenSlime";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<QueenSlime>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<QueenSlime>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region The Destroyer
	public class CaughtTheDestroyer : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.TheDestroyer");
		readonly private static string whichNPCString = "TheDestroyer";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<TheDestroyer>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<TheDestroyer>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Retinazer
	public class CaughtRetinazer : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.Retinazer");
		readonly private static string whichNPCString = "Retinazer";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Retinazer>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Retinazer>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Spazmatism
	public class CaughtSpazmatism : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.Spazmatism");
		readonly private static string whichNPCString = "Spazmatism";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Spazmatism>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Spazmatism>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Skeletron Prime
	public class CaughtSkeletronPrime : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.SkeletronPrime");
		readonly private static string whichNPCString = "SkeletronPrime";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<SkeletronPrime>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<SkeletronPrime>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Plantera
	public class CaughtPlantera : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.Plantera");
		readonly private static string whichNPCString = "Plantera";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Plantera>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Plantera>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Golem
	public class CaughtGolem : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.Golem");
		readonly private static string whichNPCString = "Golem";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Golem>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Golem>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Empress of Light
	public class CaughtEmpressOfLight : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.HallowBoss");
		readonly private static string whichNPCString = "EmpressOfLight";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<EmpressOfLight>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<EmpressOfLight>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Duke Fishron
	public class CaughtDukeFishron : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.DukeFishron");
		readonly private static string whichNPCString = "DukeFishron";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<DukeFishron>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<DukeFishron>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Betsy
	public class CaughtBetsy : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.DD2Betsy");
		readonly private static string whichNPCString = "Betsy";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Betsy>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Betsy>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Lunatic Cultist
	public class CaughtLunaticCultist : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.CultistBoss");
		readonly private static string whichNPCString = "LunaticCultist";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<LunaticCultist>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<LunaticCultist>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Moon Lord
	public class CaughtMoonLord : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.MoonLordHead");
		readonly private static string whichNPCString = "MoonLord";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<MoonLord>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<MoonLord>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Pumpking
	public class CaughtPumpking : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.Pumpking");
		readonly private static string whichNPCString = "Pumpking";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<Pumpking>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Pumpking>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Ice Queen
	public class CaughtIceQueen : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.IceQueen");
		readonly private static string whichNPCString = "IceQueen";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<IceQueen>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<IceQueen>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Martian Saucer
	public class CaughtMartianSaucer : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.MartianSaucer");
		readonly private static string whichNPCString = "MartianSaucer";
		public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/" + whichNPCString;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = (short)ModContent.NPCType<MartianSaucer>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<MartianSaucer>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
}