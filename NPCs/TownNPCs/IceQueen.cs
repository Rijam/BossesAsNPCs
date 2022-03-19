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
	public class IceQueen : ModNPC
	{
		//public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh";
		//public override string[] AltTextures => new[] { "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh_Alt_1" }; //Not implemented in 1.4 tML yet

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.IceQueen"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.DyeTrader];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
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
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Love)
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<Pumpking>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Betsy>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MartianSaucer>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Deerclops>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<QueenSlime>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.SantaClaus, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Painter, AffectionLevel.Dislike)
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
			NPC.defense = 38;
			NPC.lifeMax = 3400;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.DyeTrader;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtIceQueen>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				new FlavorTextBestiaryInfoElement("This icy monstrosity has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Snow, Pumpking\nLike: Ocean, Betsy, Martian Saucer, Deerclops, Queen Slime, Queen Bee, Santa,\nDislike: Graveyard, Painter\nHate: None")
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
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/IceQueen_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/IceQueen_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/IceQueen_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedChristmasIceQueen && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnIceQueen)
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
			chat.Add("I am the Ice Queen.");
			chat.Add("Don't you dare make any cool puns. That would not be nICE.");
			chat.Add("That snowmen mafia would be no match against me.");
			chat.Add("How about a friendly snowball fight?");
			int santa = NPC.FindFirstNPC(NPCID.SantaClaus);
			if (santa >= 0)
            {
				chat.Add("Have you been good this year for Santa Claus?");
				chat.Add("Instead of getting coal this year, you are getting a beating from me!");
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
			shop.item[nextSlot].SetDefaults(ItemID.NaughtyPresent);
			shop.item[nextSlot].shopCustomPrice = 150000; //Made up value
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ElfHat);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.017);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ElfShirt);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.017);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ElfPants);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.017);
			nextSlot++;
			if (NPC.downedChristmasTree)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant3))
				{
					shop.item[nextSlot].SetDefaults(fargosMutant3.Find<ModItem>("FestiveOrnament").Type);
					shop.item[nextSlot].shopCustomPrice = 200000; //Match the Abominationn's shop
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.ChristmasTreeSword);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.078);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChristmasHook);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.078);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Razorpine);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.078);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FestiveWings);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.017);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EverscreamTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
				nextSlot++;
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.EverscreamPetItem); //Shrub Star
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.EverscreamMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
			}
			if (NPC.downedChristmasSantank)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant2))
				{
					shop.item[nextSlot].SetDefaults(fargosMutant2.Find<ModItem>("NaughtyList").Type);
					shop.item[nextSlot].shopCustomPrice = 200000; //Match the Abominationn's shop
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.EldMelter); //Elf Melter, lol Re-Logic misspelled it.
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChainGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SantaNK1Trophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
				nextSlot++;
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.SantankMountItem); //Toy Tank
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SantankMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
			}
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("IceKingsRemains").Type);
				shop.item[nextSlot].shopCustomPrice = 300000; //Match the Abominationn's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.BlizzardStaff);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.08);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SnowmanCannon);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.08);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.NorthPole);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(90000 / 0.08);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BabyGrinchMischiefWhistle);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.017); //Has no value
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ReindeerBells);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.01);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.IceQueenTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
			nextSlot++;
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.IceQueenPetItem); //Frozen Crown
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.IceQueenMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (NPC.savedWizard)
			{
				shop.item[nextSlot].SetDefaults(ItemID.MusicBoxFrostMoon);
				shop.item[nextSlot].shopCustomPrice = 20000 * 10;
				nextSlot++;
				if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false))
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
				}
			}
			shop.item[nextSlot].SetDefaults(ItemID.Present);
			shop.item[nextSlot].shopCustomPrice = 5000;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.IceQueen.IQCostumeHeadpiece>());
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
			projType = ProjectileID.NorthPoleSpear;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
			gravityCorrection = 2;
		}
	}
}