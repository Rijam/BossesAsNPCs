using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class QueenSlime : ModNPC
	{
		//public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/KingSlime";
		//public override string[] AltTextures => new[] { "BossesAsNPCs/NPCs/TownNPCs/KingSlime_Alt_1" }; //Not implemented in 1.4 tML yet

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.QueenSlimeBoss"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.DyeTrader];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 2;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<KingSlime>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EmpressOfLight>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Nurse, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
				//Princess is automatically set
			; // < Mind the semicolon!
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 26;
			NPC.lifeMax = 1800;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath64;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.DyeTrader;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtQueenSlime>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				new FlavorTextBestiaryInfoElement("This gelatin oligarch has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Hallow, King Slime, Empress of Light\nLike: Forest, Queen Bee, Ice Queen, Party Girl, Nurse, Dryad \nDislike: Snow\nHate: None")
			});
		}
		/*public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return true;
		}*/

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				if (!Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/QueenSlime_Gore_Crown").Type, 1f);
				}
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/QueenSlime_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/QueenSlime_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/QueenSlime_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedQueenSlime && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnQueenSlime)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override string TownNPCName()
		{
			return "";
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new ();
			chat.Add("I am the Queen Slime.");
			chat.Add("Just a reminder that touching living slimes will dissolve your flesh.");
			chat.Add("My defeat will not stop my army of slimes!");
			chat.Add("I would prefer it if you stopped bouncing objects off of me.");
			chat.Add("Pink Gel may be 'sweet', but get any ideas!", 0.1);
			if (!NPC.downedSlimeKing)
			{
				chat.Add("You may have defeated me, but I'm only one half of the slime oligarchy!");
			}
			if (NPC.downedSlimeKing)
			{
				chat.Add("You have bested the slime oligarchy. I commend you.");
			}
			return chat;
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeCrystal);
			shop.item[nextSlot].shopCustomPrice = 200000; //Made up value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("JellyCrystal").Type);
				shop.item[nextSlot].shopCustomPrice = 250000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeMountSaddle); //Gelatinous Pillion
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25); //Formula: (Sell value / drop chance)
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.CrystalNinjaHelmet); //Crystal Assassin Hood
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.CrystalNinjaChestplate); //Crystal Assassin Shirt
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.CrystalNinjaLeggings); //Crystal Assassin Pants
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeHook); //Hook of Dissonance
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Smolstar); //Blade Staff
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GelBalloon);
			shop.item[nextSlot].shopCustomPrice = 40 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.VolatileGelatin);
				shop.item[nextSlot].shopCustomPrice = 50000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.QueenSlimePetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.QueenSlimeMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (NPC.savedWizard)
			{
				shop.item[nextSlot].SetDefaults(ItemID.MusicBoxQueenSlime);
				shop.item[nextSlot].shopCustomPrice = 20000 * 10;
				nextSlot++;
				if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false)) //Main.TOWMusicUnlocked
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
				}
			}
			shop.item[nextSlot].SetDefaults(ItemID.PinkGel);
			shop.item[nextSlot].shopCustomPrice = 3 * 10;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenSlime.QSAltCostumeHeadpiece>());
			shop.item[nextSlot].shopCustomPrice = 50000;
			nextSlot++;
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return !toKingStatue;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 25;
			knockback = 2f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.VolatileGelatinBall;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
}