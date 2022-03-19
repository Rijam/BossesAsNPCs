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
	public class Spazmatism : ModNPC
	{
		//public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh";
		//public override string[] AltTextures => new[] { "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh_Alt_1" }; //Not implemented in 1.4 tML yet

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.Spazmatism"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Clothier];
			NPCID.Sets.ExtraFramesCount[Type] = 7;
			NPCID.Sets.AttackFrameCount[Type] = 2;
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
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<TheDestroyer>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Retinazer>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<SkeletronPrime>(), AffectionLevel.Love)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MoonLord>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Nurse, AffectionLevel.Dislike)
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
			NPC.defense = 15;
			NPC.lifeMax = 2300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Clothier;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtSpazmatism>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("This mechanical flamethrower has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: The Destroyer, Retinazer, Skeletron Prime, Mechanic\nLike: Forest, Eye of Cthulhu, Moon Lord, Steampunker, Cyborg, Goblin Tinkerer\nDislike: Nurse\nHate: None")
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
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Spazmatism_Gore_Head").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Twins_Gore_Tether").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Twins_Gore_Arm").Type, 1f);
				}
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EyeOfCthulhu_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedMechBoss2 && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnTwins)
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
			chat.Add("I am Spazmatism, one of The Twins.");
			chat.Add("Uh sorry, I don't know who Sclerepugnant is.");
			chat.Add("I am fresh out of Ichor, but I have plenty of Cursed Flames!");
			chat.Add("HEY, it's the Terrarian!");
			chat.Add("This is going to be a terrible night... if you don't buy something!");
			int retinazer = NPC.FindFirstNPC(ModContent.NPCType<Retinazer>());
			int eoc = NPC.FindFirstNPC(ModContent.NPCType<EyeOfCthulhu>());
			if (retinazer >= 0 && eoc >= 0)
			{
				chat.Add("Eye of Cthulhu, Rez, and I like to talk.", 0.5);
			}
			if (Main.LocalPlayer.HasBuff(BuffID.TwinEyesMinion))
			{
				chat.Add("Those mini Spazmatisms are so cute!", 0.5);
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0)
			{
				chat.Add("Go blame " + Main.npc[mechanic].GivenName + ", she's the one who made me!");
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
			shop.item[nextSlot].SetDefaults(ItemID.MechanicalEye);
			shop.item[nextSlot].shopCustomPrice = 250000; //Made up value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("MechEye").Type);
				shop.item[nextSlot].shopCustomPrice = 400000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.HallowedBar);
			shop.item[nextSlot].shopCustomPrice = 400 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SoulofSight);
			shop.item[nextSlot].shopCustomPrice = 800 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.TwinMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SpazmatismTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.MechanicalWheelPiece);
				shop.item[nextSlot].shopCustomPrice = 5000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.TwinsPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TwinsMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (NPC.savedWizard)
			{
				shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss2);
				shop.item[nextSlot].shopCustomPrice = 20000 * 10;
				nextSlot++;
				if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false))
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
				}
			}
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Spazmatism.SpazCostumeHeadpiece>());
			shop.item[nextSlot].shopCustomPrice = 50000;
			nextSlot++;
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return true;
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
			projType = ProjectileID.CursedFlameFriendly;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
}