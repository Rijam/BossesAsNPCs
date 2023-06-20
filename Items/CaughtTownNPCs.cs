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
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
		}
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			Item.ResearchUnlockCount = 3;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.SlimeBlock;
			ItemID.Sets.AnimatesAsSoul[Type] = true;
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
			Item.makeNPC = ModContent.NPCType<KingSlime>();
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
			string chatMessage = Language.GetTextValue("Mods." + ModContent.GetInstance<BossesAsNPCs>().Name + ".UI.CapturedSpawnText", npcName);
			if (Main.netMode != NetmodeID.Server)
			{
				Main.NewText(chatMessage, 50, 125, 255);
			}
			else
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(chatMessage), new Color(50, 125, 255));
			}
		}
	}
    #endregion
    #region Eye of Cthulhu
    public class CaughtEyeOfCthulhu : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.EyeofCthulhu");
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<EyeOfCthulhu>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.LesionBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<EaterOfWorlds>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<BrainOfCthulhu>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Hive;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<QueenBee>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.BoneBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Skeletron>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Deerclops>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<WallOfFlesh>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.PinkSlimeBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<QueenSlime>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.TungstenBrick;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<TheDestroyer>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.IronBrick;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Retinazer>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.LeadBrick;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Spazmatism>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.SilverBrick;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<SkeletronPrime>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Hay;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Plantera>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.LihzahrdBrick;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Golem>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<EmpressOfLight>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<DukeFishron>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Betsy>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<LunaticCultist>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 23));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<MoonLord>();
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
	#region Dreadnautilus
	public class CaughtDreadnautilus : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.BloodNautilus");
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ReefBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Dreadnautilus>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Dreadnautilus>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
	#region Mothron
	public class CaughtMothron : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.Mothron");
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.FleshBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Mothron>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<Mothron>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Pumpkin;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<Pumpking>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 25));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.IceBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<IceQueen>();
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
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1];
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MartianConduitPlating;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<MartianSaucer>();
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

	#region Torch God
	public class CaughtTorchGod : CaughtKingSlime
	{
		readonly private static string name = Language.GetTextValue("NPCName.TorchGod");
		public override string Texture => Mod.Name + "/NPCs/TownNPCs/" + Name.Split("Caught")[1] + "_Bestiary";
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault(name);
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 26));
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.LivingFireBlock;
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ModContent.ItemType<CaughtKingSlime>());
			Item.makeNPC = ModContent.NPCType<TorchGod>();
		}

		public override bool CanUseItem(Player player)
		{
			Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return (NPC.CountNPCS(ModContent.NPCType<TorchGod>()) < 1 && !Collision.SolidCollision(mousePos, player.width, player.height));
		}

		public override void OnConsumeItem(Player player)
		{
			SpawnText(name);
		}
	}
	#endregion
}