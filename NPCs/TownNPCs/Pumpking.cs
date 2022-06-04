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
using Microsoft.Xna.Framework;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Pumpking : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.Pumpking"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Merchant];
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
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Betsy>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MartianSaucer>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<KingSlime>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<DukeFishron>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike)
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
			NPC.defense = 36;
			NPC.lifeMax = 2200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Merchant;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtPumpking>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				new FlavorTextBestiaryInfoElement("Mods.BossesAsNPCs.Bestiary.Description." + GetType().Name),
				new FlavorTextBestiaryInfoElement(
					NPCHelper.LoveText(GetType().Name) +
					NPCHelper.LikeText(GetType().Name) +
					NPCHelper.DislikeText(GetType().Name) +
					NPCHelper.HateText(GetType().Name))
			});
		}

		public override void OnKill()
		{
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
			for (int k = 0; k < 2; k++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedHalloweenKing && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnPumpking)
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
            return new PumpkingProfile();
        }

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/Pumpking_Glow");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}
			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)NPC.position.Y << 32) | (uint)NPC.position.X);
			Color color = new(255, 255, 255, 0);
			int spriteWidth = 56;
			int spriteHeight = 56;
			int x = NPC.frame.X;
			int y = NPC.frame.Y;
			for (int i = 0; i < 5; i++)
			{
				float random1 = Utils.RandomInt(ref seed, -5, 11) * 0.05f;
				float random2 = Utils.RandomInt(ref seed, -5, 1) * 0.15f;
				Vector2 posOffset = new(NPC.position.X - Main.screenPosition.X - (spriteWidth - 16f) / 2f + random1 - 191f, NPC.position.Y - Main.screenPosition.Y + random2 - 204f);
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
			chat.Add("I am the Pumpking.");
			chat.Add("Don't get on my bad side!");
			chat.Add("I don't get the whole 'pumpkin spice' craze.");
			chat.Add("They are scythes, not sickles! Get it right!");
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add("Is this the type of party that involves passing out candy? I sure hope it is!", 2.0);
			}
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				int abominationn = NPC.FindFirstNPC(fargosMutant.Find<ModNPC>("Abominationn").Type);
				if (abominationn >= 0)
				{
					chat.Add(Main.npc[abominationn].GivenName + "'s hat suits him quite nicely.");
				}
			}
			return chat;
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = Language.GetTextValue("LegacyInterface.28") + " 2";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
				NPCHelper.SetShop1(true);
				NPCHelper.SetShop2(false);
			}
			if (!firstButton)
			{
				shop = true;
				NPCHelper.SetShop1(false);
				NPCHelper.SetShop2(true);
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(ItemID.PumpkinMoonMedallion);
			shop.item[nextSlot].shopCustomPrice = 150000; //Made up value
			nextSlot++;
			if (NPCHelper.StatusShop2())
			{
				shop.item[nextSlot].SetDefaults(ItemID.ScarecrowHat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.033); //Using the highest drop chances
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ScarecrowShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.033);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ScarecrowPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.033);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.JackOLanternMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SpookyWood);
				shop.item[nextSlot].shopCustomPrice = 5000; //Made up value
				nextSlot++;
				if (NPC.downedHalloweenTree)
				{
					if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
					{
						shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("SpookyBranch").Type);
						shop.item[nextSlot].shopCustomPrice = 200000; //Match the Abominationn's shop
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ItemID.SpookyHook);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SpookyTwig);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.StakeLauncher);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Stake);
					shop.item[nextSlot].shopCustomPrice = 15; //Same price as Arms Dealer/Witch Doctor
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CursedSapling);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.NecromanticScroll);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MourningWoodTrophy);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
					nextSlot++;
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.WitchBroom);
						shop.item[nextSlot].shopCustomPrice = 50000 * 5;
						nextSlot++;
					}
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.SpookyWoodMountItem); //Hexxed Branch
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.MourningWoodMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}
			}
			if (NPCHelper.StatusShop1())
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant2))
				{
					shop.item[nextSlot].SetDefaults(fargosMutant2.Find<ModItem>("SuspiciousLookingScythe").Type);
					shop.item[nextSlot].shopCustomPrice = 300000; //Match the Abominationn's shop
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.TheHorsemansBlade);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BatScepter);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BlackFairyDust);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SpiderEgg);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RavenStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CandyCornRifle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CandyCorn);
				shop.item[nextSlot].shopCustomPrice = 5; //Same price as Arms Dealer
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.JackOLanternLauncher);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ExplosiveJackOLantern);
				shop.item[nextSlot].shopCustomPrice = 15; //Same price as Arms Dealer
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ScytheWhip); //Dark Harvest
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PumpkingTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1); //same trophy price
				nextSlot++;
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.PumpkingPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.PumpkingMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxPumpkinMoon);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.GoodieBag);
					shop.item[nextSlot].shopCustomPrice = 5000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Pumpking.PkCostumeShoes>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
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
			projType = ProjectileID.FlamingJack;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class PumpkingProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}