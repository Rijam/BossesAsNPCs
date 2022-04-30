using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using System.Collections.Generic;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class EaterOfWorlds : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.EaterofWorldsHead"));
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
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Hate)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<WallOfFlesh>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<TheDestroyer>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Like)
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EmpressOfLight>(), AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike)
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
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtEaterOfWorlds>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				new FlavorTextBestiaryInfoElement("This abyssal worm has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Graveyard, Brain of Cthulhu\nLike: Forest, Wall of Flesh, The Destroyer, Tavernkeep, Arms Dealer\nDislike: Jungle, Dryad, Empress of Light\nHate: Hallow")
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EaterOfWorlds_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EaterOfWorlds_Gore_Arm").Type, 1f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/KingSlime_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if ((NPC.downedBoss2 && (!WorldGen.crimson || WorldGen.drunkWorldGen || Main.drunkWorld) || Main.hardMode) && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoW)
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
            return new EaterOfWorldsProfile();
        }

		public override string GetChat()
		{
			WeightedRandom<string> chat = new ();
			chat.Add("I am the Eater of Worlds.");
			chat.Add("No, I am not Cthulhu's spine. Where did you even hear that?");
			chat.Add("Which way to the nearest restroom?! I think I'm going to vomit.");
			chat.Add("Of course piercing weapons are effective -- they go right through you!");
			chat.Add("A horrible chill goes down your wallet... I think the only solution is to buy something.");
			if (Main.expertMode)
            {
				chat.Add("Your explosives are 80% less effective against me in this world!");
			}
			if (WorldGen.tEvil >= 100)
			{
				chat.Add("Yes! Let the Corruption spread!");
			}
			if (WorldGen.tGood > 0)
			{
				chat.Add("That nasty Hallow is ruining my plans of spreading the Corruption!");
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add("I'm not sure if you have noticed, but " + Main.npc[dryad].GivenName + " really hates me.");
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
			shop.item[nextSlot].SetDefaults(ItemID.WormFood);
			shop.item[nextSlot].shopCustomPrice = 100000; //Made up value since it has no value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("WormyFood").Type);
				shop.item[nextSlot].shopCustomPrice = 100000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.DemoniteOre);
			shop.item[nextSlot].shopCustomPrice = 1000 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ShadowScale);
			shop.item[nextSlot].shopCustomPrice = 100 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.EatersBone);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.05); //Formula: (Sell value / drop chance))
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.EaterMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.EaterofWorldsTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.WormScarf);
				shop.item[nextSlot].shopCustomPrice = 20000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.EaterOfWorldsPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EaterofWorldsMasterTrophy);
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
					if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false))
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.VilePowder);
				shop.item[nextSlot].shopCustomPrice = 100;
				nextSlot++;
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.UnholyWater);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
				}
				int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
				if (steampunker >= 0 && NPC.downedMechBossAny)
				{
					shop.item[nextSlot].SetDefaults(ItemID.PurpleSolution);
					shop.item[nextSlot].shopCustomPrice = 2500;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.WormTooth);
				shop.item[nextSlot].shopCustomPrice = 20 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeBodypiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EaterOfWorlds.EoWCostumeLegpiece>());
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
			projType = ProjectileID.EatersBite;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class EaterOfWorldsProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/EaterOfWorlds");

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("BossesAsNPCs/NPCs/TownNPCs/EaterOfWorlds_Head");
	}
}