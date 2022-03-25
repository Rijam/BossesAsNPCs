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
	public class EmpressOfLight : ModNPC
	{
		//public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/EmpressOfLight";
		//public override string[] AltTextures => new[] { "BossesAsNPCs/NPCs/TownNPCs/EmpressOfLight_Alt_1" }; //Not implemented in 1.4 tML yet

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.HallowBoss"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Clothier];
			NPCID.Sets.ExtraFramesCount[Type] = 7;
			NPCID.Sets.AttackFrameCount[Type] = 2;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 70;
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
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<QueenSlime>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Plantera>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Golem>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<LunaticCultist>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Like)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Stylist, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EaterOfWorlds>(), AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Dislike)
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
			NPC.defense = 50;
			NPC.lifeMax = 7000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath65;
			NPC.knockBackResist = 0.25f;
			AnimationType = NPCID.Clothier;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtEmpressOfLight>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
				new FlavorTextBestiaryInfoElement("This vengeful fae goddess has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Hallow, Dryad, Queen Slime\nLike: Forest, Plantera, Golem, Lunatic Cultist, Wizard, Party Girl, Stylist\nDislike: Caverns, Eater of Worlds, Brain of Cthulhu\nHate: Graveyard")
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
				if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Head_alt").Type, 1f);
				}
				else
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Head").Type, 1f);
				}
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedEmpressOfLight && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoL)
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
			chat.Add("I am the Empress of Light.");
			chat.Add("The Bestiary describes me as a vengeful fae goddess; I agree with it.");
			chat.Add("Don't you dare touch those butterflies!");
			chat.Add("If you want a real challenge, fight me during the day.");
            chat.Add("\"A beautiful bullet hell\"? I don't use any firearms, but I am beautiful.");
			chat.Add("I bet you thought all fairies were small, didn't you?");
			chat.Add("My empire? That's a story for another time.");
			chat.Add("Don't ask about my tentacles...", 0.5);
			chat.Add("Is it a mouth or a nose?", 0.1);
			chat.Add("Don't be silly, there is no Emperor of Night.\n\nIs there?", 0.1);
			if (WorldGen.tGood >= 100)
            {
				chat.Add("I applaud you for spreading the Hallow. Good job!");
			}
			if (WorldGen.tEvil == 0 && WorldGen.tBlood == 0)
			{
				chat.Add("I applaud you for removing the evil in this world. Good job!");
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0 && Main.hardMode)
			{
				chat.Add("Buy as many Hallow Seeds from " + Main.npc[dryad].GivenName + " as you can!");
			}
			int plantera = NPC.FindFirstNPC(ModContent.NPCType<Plantera>());
			if (plantera >= 0)
			{
				chat.Add("I play my Stellar Tune in Plantera's metal band. You should come have a listen!");
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
				chat.Add("I can put on real light show for this party!", 2.0);
			}
			if (Main.LocalPlayer.armor[10].type == ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeHeadpiece>() &&
				Main.LocalPlayer.armor[11].type == ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeBodypiece>() &&
				Main.LocalPlayer.armor[12].type == ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeLegpiece>())
            {
				chat.Add("That's quite the stunning outfit you have on. Following in my image I see!", 2.0); 
			}
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				chat.Add("Oh, so you have a \'hit list\' of all the powerful creatures in this world? You planned on defeating me all along?", 0.25);
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
			shop.item[nextSlot].SetDefaults(ItemID.EmpressButterfly); //Prismatic Lacewing
			shop.item[nextSlot].shopCustomPrice = 400000; //Sell value * 5 = 250000
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("PrismaticPrimrose").Type);
				shop.item[nextSlot].shopCustomPrice = 600000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.FairyQueenMagicItem); //Nightglow
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);  //Formula: (Sell value / drop chance); It would be 200000 in this case
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PiercingStarlight); //Starlight
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.RainbowWhip); //Kaleidoscope
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.FairyQueenRangedItem); //Eventide
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.RainbowWings); //Empress Wings
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.07);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.HallowBossDye); //Prismatic Dye
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SparkleGuitar); //Stellar Tune
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.RainbowCursor); //Rainbow Cursor
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.EmpressBlade); //Terraprisma
			shop.item[nextSlot].shopCustomPrice = 200000 * 50; //Special case since it is technically a "100% drop chance".
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.FairyQueenMask); //Empress of Light Mask
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.FairyQueenTrophy); //Empress of Light Trophy
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.EmpressFlightBooster); //Soaring Insignia
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.FairyQueenPetItem); //Jewel of Light
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FairyQueenMasterTrophy); //Empress of Light Relic
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
            {
				if (NPC.savedWizard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxEmpressOfLight);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false))
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.HolyWater);
					shop.item[nextSlot].shopCustomPrice = 200; //For some reason Holy Water is double as valuable than Unholy/Blood Water.
					nextSlot++;
				}
				int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
				if (steampunker >= 0 && NPC.downedMechBossAny)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BlueSolution);
					shop.item[nextSlot].shopCustomPrice = 2500;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeHeadpiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeBodypiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.EmpressOfLight.EoLCostumeLegpiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return !toKingStatue;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 45;
			knockback = 6f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.FairyQueenRangedItemShot; //Twilight Lance
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
	}
}