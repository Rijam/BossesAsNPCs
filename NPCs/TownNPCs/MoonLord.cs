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
using Microsoft.Xna.Framework;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class MoonLord : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.MoonLordHead"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Clothier];
			NPCID.Sets.ExtraFramesCount[Type] = 7;
			NPCID.Sets.AttackFrameCount[Type] = 2;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 70;
			NPCID.Sets.HatOffsetY[Type] = 1;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Love)
				//.SetBiomeAffection<Sky>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<LunaticCultist>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Skeletron>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<TheDestroyer>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Retinazer>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Spazmatism>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<SkeletronPrime>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
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
			NPC.defense = 70;
			NPC.lifeMax = 14500;
			NPC.HitSound = SoundID.NPCHit57;
			NPC.DeathSound = SoundID.NPCDeath62;
			NPC.knockBackResist = 0.25f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtMoonLord>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				new FlavorTextBestiaryInfoElement("This eldritch god has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Graveyard, Lunatic Cultist\nLike: Eye of Cthulhu, Brain of Cthulhu, Skeletron, The Destroyer, Retinazer, Spazmatism, Skeletron Prime, Mechanic\nDislike: Empress of Light, Dryad \nHate: None")
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/MoonLord_Gore_Head").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/MoonLord_Gore_Torso").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/MoonLord_Gore_Arm").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedMoonlord && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMoonLord)
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
            return new MoonLordProfile();
        }

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/MoonLord_Glow");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}
			Color color = new(100, 100, 100, 100);
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
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new ();
			chat.Add("I am the Moon Lord.");
			chat.Add("I don't know who Steve is, but at least I found my legs.");
			chat.Add("My plans to take over Terraria have been stopped by you. I commend you for such efforts.");
			chat.Add("You are quite the fighter. You went out of your way to defeat me.");
			chat.Add("The Celestial Pillars were sent to conquer your world.");
			chat.Add("There is only one other God that I fear. Luckily, it only haunts torches.");
			chat.Add("No, there is no 'Sun Lord'.");
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add("I've decided to let " + Main.npc[dryad].GivenName + " live. She is the last of her kind anyway.");
			}
			int lunaticCultist = NPC.FindFirstNPC(ModContent.NPCType<LunaticCultist>());
			if (lunaticCultist >= 0)
			{
				chat.Add("It's nice to have people worship me, as they should be.");
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0)
			{
				chat.Add(Main.npc[mechanic].GivenName + " was doing a fantastic job, until you came along and rescued her.");
			}
			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller))
			{
				int torchMan = NPC.FindFirstNPC(torchSeller.Find<ModNPC>("TorchSellerNPC").Type);
				if (torchMan >= 0)
				{
					chat.Add("Are you sure that " + Main.npc[torchMan].GivenName + " is not... Him?");
				}
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
			shop.item[nextSlot].SetDefaults(ItemID.CelestialSigil);
			shop.item[nextSlot].shopCustomPrice = 500000; //made up value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("CelestialSigil2").Type);
				shop.item[nextSlot].shopCustomPrice = 1000000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.PortalGun);
			shop.item[nextSlot].shopCustomPrice = 100000 * 5; 
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LunarOre);
			shop.item[nextSlot].shopCustomPrice = 3000 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Meowmere);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(200000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Terrarian);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.StarWrath);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(200000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SDMG);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LastPrism);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LunarFlareBook);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.RainbowCrystalStaff);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MoonlordTurretStaff); //Lunar Portal Staff
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Celeb2); //Celebration Mk2
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MeowmereMinecart);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.1);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BossMaskMoonlord);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MoonLordTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.GravityGlobe);
				shop.item[nextSlot].shopCustomPrice = 400000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SuspiciousLookingTentacle);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LongRainbowTrailWings);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.MoonLordPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoonLordMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
            {
				if (NPC.savedWizard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxLunarBoss);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false)) //Main.TOWMusicUnlocked
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWMoonLord);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MoonLord.MLCostumeBodypiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoonLordLegs);
				shop.item[nextSlot].shopCustomPrice = 20000 * 5;
				nextSlot++;
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return toKingStatue;
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
			projType = ProjectileID.MoonlordArrowTrail;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
	}
	public class MoonLordProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/MoonLord");

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("BossesAsNPCs/NPCs/TownNPCs/MoonLord_Head");
	}
}