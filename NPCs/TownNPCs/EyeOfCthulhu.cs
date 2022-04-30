using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using Terraria.GameContent;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class EyeOfCthulhu : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.EyeofCthulhu"));
			Main.npcFrameCount[Type] = 25; // Main.npcFrameCount[NPCID.DyeTrader];
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
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Retinazer>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Retinazer>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Skeletron>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MoonLord>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Painter, AffectionLevel.Like)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like)
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
			NPC.lifeMax = 280;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtEyeOfCthulhu>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("This optical organ has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Forest, Retinazer, Spazmatism \nLike: Desert, Brain of Cthulhu, Skeletron, Moon Lord, Guide, Painter, Party Girl\nDislike: None \nHate: None")
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EyeOfCthulhu_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EyeOfCthulhu_Gore_Arm").Type, 1f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EyeOfCthulhu_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedBoss1 && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoC)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override ITownNPCProfile TownNPCProfile()
        {
            return new EyeOfCthulhuProfile();
        }

		public override string GetChat()
		{
			WeightedRandom<string> chat = new ();
			chat.Add("I am the Eye of Cthulhu.");
			chat.Add("I have my eye on you...");
			chat.Add("Besides that tree, I think I am the mascot of this world.");
			chat.Add("Somebody once called me the 'Eye of Terror'.");
			chat.Add("I have been defeated in three separate universes! How is that fair?");
			chat.Add("What is your fascination with eyes anyway?");
			int retinazer = NPC.FindFirstNPC(ModContent.NPCType<Retinazer>());
			int spazmatism = NPC.FindFirstNPC(ModContent.NPCType<Spazmatism>());
			if (retinazer >= 0 && spazmatism >= 0)
            {
				chat.Add("Rez, Spaz, and I like to socialize.", 0.5);
			}
			if (Main.LocalPlayer.HasBuff(BuffID.SuspiciousTentacle) || NPC.downedMoonlord)
			{
				chat.Add("There is nothing truer than the original!", 0.5);
			}
			if (Main.LocalPlayer.HasItem(ItemID.TheEyeOfCthulhu))
			{
				chat.Add("I'm so popular that they put me on merchandise.");
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
			shop.item[nextSlot].SetDefaults(ItemID.SuspiciousLookingEye);
			shop.item[nextSlot].shopCustomPrice = 75000; //Made up value since it has no value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("SuspiciousEye").Type);
				shop.item[nextSlot].shopCustomPrice = 80000; //Match the Mutant's shop
				nextSlot++;
			}
			if (!WorldGen.crimson || Main.hardMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.DemoniteOre);
				shop.item[nextSlot].shopCustomPrice = 1000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CorruptSeeds);
				shop.item[nextSlot].shopCustomPrice = 500 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.UnholyArrow);
				if (NPC.downedBoss2)
                {
					if (Main.hardMode)
                    {
						shop.item[nextSlot].shopCustomPrice = 40;
					}
					else
                    {
						shop.item[nextSlot].shopCustomPrice = 40 * 2;
					}
				}
				else
                {
					shop.item[nextSlot].shopCustomPrice = 40 * 5;
				}
				nextSlot++;
			}
			if (WorldGen.crimson || Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.CrimtaneOre);
				shop.item[nextSlot].shopCustomPrice = 1300 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CrimsonSeeds);
				shop.item[nextSlot].shopCustomPrice = 500 * 5;
				nextSlot++;
			}

			shop.item[nextSlot].SetDefaults(ItemID.Binoculars);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.03); //Formula: (Sell value * 3 / drop chance))
			nextSlot++;
			int wof = NPC.FindFirstNPC(ModContent.NPCType<WallOfFlesh>());
			if (wof >= 0)
            {
				shop.item[nextSlot].SetDefaults(ItemID.BadgersHat);
				shop.item[nextSlot].shopCustomPrice = 3000 * 20;
				nextSlot++;
			}
			
			shop.item[nextSlot].SetDefaults(ItemID.EyeMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.EyeofCthulhuTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.EoCShield);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.AviatorSunglasses);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EyeOfCthulhuPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EyeofCthulhuMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
            {
				if (NPC.savedWizard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false)) //Main.TOWMusicUnlocked
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeHeadpiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EoCCostumeBodypiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EyeOfCthulhu.EyeCostumeLegpiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return true; //Either king or queen
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
			projType = ModContent.ProjectileType<Projectiles.EyeOrnament>();
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class EyeOfCthulhuProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/EyeOfCthulhu");

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("BossesAsNPCs/NPCs/TownNPCs/EyeOfCthulhu_Head");
	}
}