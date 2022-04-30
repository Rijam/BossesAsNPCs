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
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Xna.Framework;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class EmpressOfLight : ModNPC
	{

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

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Head_alt").Type, 1f);
				}
				else
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Head").Type, 1f);
				}
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/EmpressOfLight_Gore_Leg").Type, 1f);
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

		public override ITownNPCProfile TownNPCProfile()
        {
            return new EmpressOfLightProfile();
        }

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/EmpressOfLight_Wings");
		private int colorIndex = 0;
		private Color color = new(255, 255, 255, 0);
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			NPC.ai[2] += 3;

			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}

			Color colorMagenta = new(207, 0, 151, 100);
			Color colorYellow = new(241, 240, 161, 100);
			Color colorLBlue = new(0, 241, 255, 100);
			Color colorDBlue = new(82, 123, 239, 100);


			if (colorIndex == 0)
			{
				color = colorMagenta;
			}
			else if (colorIndex == 1)
			{
				color = Color.Lerp(colorMagenta, colorYellow, NPC.ai[2] / 100f);
			}
			else if (colorIndex == 2)
			{
				color = Color.Lerp(colorYellow, colorLBlue, NPC.ai[2] / 100f);
			}
			else if (colorIndex == 3)
			{
				color = Color.Lerp(colorLBlue, colorDBlue, NPC.ai[2] / 100f);
			}
			else if (colorIndex == 4)
			{
				color = Color.Lerp(colorDBlue, colorMagenta, NPC.ai[2] / 100f);
			}

			int spriteWidth = 40;
			int spriteHeight = 56;
			int x = NPC.frame.X;
			int y = NPC.frame.Y;
			for (int i = 0; i < 10; i++)
			{
				Vector2 posOffset = new(NPC.position.X - Main.screenPosition.X - (spriteWidth - 16f) / 2f - 191f, NPC.position.Y - Main.screenPosition.Y - 204f);
				if (NPC.direction == 1)
				{
					spriteBatch.Draw(glowmask.Value, posOffset + screenOffset, (Rectangle?)new Rectangle(x, y, spriteWidth, spriteHeight), color, 0f, default, 1f, SpriteEffects.FlipHorizontally, 1f);
				}
				else
				{
					spriteBatch.Draw(glowmask.Value, posOffset + screenOffset, (Rectangle?)new Rectangle(x, y, spriteWidth, spriteHeight), color, 0f, default, 1f, SpriteEffects.None, 1f);
				}
			}

			if (NPC.ai[2] > 100)
            {
				colorIndex++;
				NPC.ai[2] = 0;
				if (colorIndex > 4)
				{
					colorIndex = 1;
				}
			}
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
	public class EmpressOfLightProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				return ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/EmpressOfLight");

			if (npc.altTexture == 1)
				return ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/EmpressOfLight_Alt_1");

			return ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/EmpressOfLight");
		}

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("BossesAsNPCs/NPCs/TownNPCs/EmpressOfLight_Head");
	}
}