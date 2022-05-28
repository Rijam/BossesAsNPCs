using System;
using Microsoft.Xna.Framework;
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
	public class Deerclops : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.Deerclops"));
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
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Betsy>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.SantaClaus, AffectionLevel.Like)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike)
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
			NPC.lifeMax = 700;
			NPC.HitSound = SoundID.DeerclopsHit;
			NPC.DeathSound = SoundID.DeerclopsDeath;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtDeerclops>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,
				new FlavorTextBestiaryInfoElement("This cyclopic monstrosity has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement(
					NPCHelper.LoveText() + "Snow, Betsy\n" +
					NPCHelper.LikeText() + "Forest, Ice Queen, Eye of Cthulhu, Zoologist, Santa\n" +
					NPCHelper.DislikeText() + "Tax Collector\n" +
					NPCHelper.HateText() + "None")
			});
		}

		public override void OnKill()
		{
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Antler1").Type, 1f);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Antler2").Type, 1f);
			for (int k = 0; k < 2; k++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedDeerclops && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDeerclops)
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
            return new DeerclopsProfile();
        }

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/Deerclops_Glow");
		
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			NPC.ai[2]++;
            if (NPC.ai[2] > 115)
			{
				NPC.ai[2] = 0;
			}
			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}

			Color color = new(255, 255, 255, 0);

			int spriteWidth = 42;
			int spriteHeight = 56;
			int x = NPC.frame.X;
			int y = NPC.frame.Y;
			if (NPC.frame.Y > 20 * spriteHeight) //Only draw while attacking
            {
				float sinOffsetY = (float)Math.Sin((NPC.ai[2] - 11) * Math.PI / 11.5f) * 2f; //NPC.ai[2] will range from 11 to 34 when attacking. This will produce a sine wave with one period.
				float sinOffsetX = (float)Math.Cos((NPC.ai[2] - 11) * Math.PI / 11.5f) * 2f;
				for (int i = 0; i < 5; i++)
				{
					Vector2 posOffset = new(NPC.position.X - Main.screenPosition.X - (spriteWidth - 16f) / 2f + sinOffsetX - 191f, NPC.position.Y - Main.screenPosition.Y + sinOffsetY - 204f);
					Vector2 posOffset2 = new(NPC.position.X - Main.screenPosition.X - (spriteWidth - 16f) / 2f - sinOffsetX - 191f, NPC.position.Y - Main.screenPosition.Y - sinOffsetY - 204f);
					if (NPC.direction == 1)
					{
						spriteBatch.Draw(glowmask.Value, posOffset + screenOffset, (Rectangle?)new Rectangle(x, y, spriteWidth, spriteHeight), color * 0.1f, 0f, default, 1f, SpriteEffects.FlipHorizontally, 1f);
						spriteBatch.Draw(glowmask.Value, posOffset2 + screenOffset, (Rectangle?)new Rectangle(x, y, spriteWidth, spriteHeight), color * 0.1f, 0f, default, 1f, SpriteEffects.FlipHorizontally, 1f);
					}
					else
					{
						spriteBatch.Draw(glowmask.Value, posOffset + screenOffset, (Rectangle?)new Rectangle(x, y, spriteWidth, spriteHeight), color * 0.1f, 0f, default, 1f, SpriteEffects.None, 1f);
						spriteBatch.Draw(glowmask.Value, posOffset2 + screenOffset, (Rectangle?)new Rectangle(x, y, spriteWidth, spriteHeight), color * 0.1f, 0f, default, 1f, SpriteEffects.None, 1f);
					}
				}
			}
		}
        public override Color? GetAlpha(Color drawColor)
        {
            return NPC.frame.Y > 20 * 56 ? Color.White : null;
        }

        public override string GetChat()
		{
			WeightedRandom<string> chat = new ();
			chat.Add("I am Deerclops.");
			chat.Add("Make sure to eat and keep your sanity up!");
			chat.Add("Stay out of the darkness, or she'll get you...");
			chat.Add("Sorry, I'm loud. I have gotten complaints from the other villagers.");
			chat.Add("Everyone else gets an achievement; where is mine?");
			if (Main.player[Main.myPlayer].ZoneSnow)
            {
				chat.Add("Are you going to hold a feast of some kind in this winter weather?");
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add("Oh a party! There better be a big feast for all of us!", 2.0);
			}
			int betsy = NPC.FindFirstNPC(ModContent.NPCType<Betsy>());
            if (betsy >= 0)
			{
				chat.Add("I heard Betsy is from another world, too.");
			}
			if (Main.dontStarveWorld || WorldGen.dontStarveWorldGen)
            {
				chat.Add("This world reminds of the place where I am from.");
				chat.Add("Good thing you only have to worry about your hunger in this world. Imagine if you had to manage your temperature too!");
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
			shop.item[nextSlot].SetDefaults(ItemID.DeerThing);
			shop.item[nextSlot].shopCustomPrice = 140000; //Made up value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("DeerThing2").Type);
				shop.item[nextSlot].shopCustomPrice = 120000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.ChesterPetItem); //Eye Bone
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33); //Formula: (Sell value / drop chance))
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Eyebrella);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.DontStarveShaderItem); //Radio Thing
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PewMaticHorn);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.WeatherPain);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.HoundiusShootius);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LucyTheAxe);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.DeerclopsMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.DeerclopsTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.BoneHelm);
				shop.item[nextSlot].shopCustomPrice = 20000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.DeerclopsPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DeerclopsMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
            {
				if (NPC.savedWizard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxDeerclops);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				Player player = Main.player[Main.myPlayer];
				bool theConstant = Main.dontStarveWorld || WorldGen.dontStarveWorldGen;
				if (player.ZoneGraveyard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.AbigailsFlower);
					shop.item[nextSlot].shopCustomPrice = 500 * 5;
					nextSlot++;
				}
				if (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BatBat);
					shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(2500 / 0.01) : (int)Math.Round(2500 / 0.004);
					nextSlot++;
				}
				if (player.ZoneSnow && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight) && (player.ZoneHallow || player.ZoneCorrupt || player.ZoneCrimson))
				{
					shop.item[nextSlot].SetDefaults(ItemID.HamBat);
					shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(10000 / 0.1) : (int)Math.Round(10000 / 0.04);
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.PigPetItem); //Monster Meat
				shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(10000 / 0.005) : (int)Math.Round(10000 / 0.001);
				nextSlot++;
				if (Main.hardMode && player.ZoneJungle)
				{
					shop.item[nextSlot].SetDefaults(ItemID.GlommerPetItem); //Glommer's Flower
					shop.item[nextSlot].shopCustomPrice = theConstant ? (int)Math.Round(50000 / 0.025) : (int)Math.Round(50000 / 0.01);
					nextSlot++;
				}
				int travelingMerchant = NPC.FindFirstNPC(NPCID.TravellingMerchant);
				if (travelingMerchant >= 0)
				{
					shop.item[nextSlot].SetDefaults(ItemID.PaintingWendy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.PaintingWillow);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.PaintingWilson);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.PaintingWolfgang);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeBodypiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Deerclops.DcCostumeLegpiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return toKingStatue;
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
			projType = ProjectileID.InsanityShadowFriendly;
			attackDelay = 10;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class DeerclopsProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}