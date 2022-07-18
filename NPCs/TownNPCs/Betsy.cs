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
	public class Betsy : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.DD2Betsy"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Clothier];
			NPCID.Sets.ExtraFramesCount[Type] = 7;
			NPCID.Sets.AttackFrameCount[Type] = 2;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 70;
			NPCID.Sets.HatOffsetY[Type] = 0;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<DukeFishron>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Deerclops>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Pumpking>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MartianSaucer>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Mothron>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
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
			NPC.lifeMax = 5000;
			NPC.HitSound = SoundID.DD2_BetsyHurt;
			NPC.DeathSound = SoundID.DD2_BetsyDeath;
			NPC.knockBackResist = 0.25f;
			AnimationType = NPCID.Clothier;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtBetsy>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
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
			if (BossesAsNPCsWorld.downedBetsy && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBetsy)
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
			return new BetsyProfile();
		}
		//SetNPCNameList is not needed for these Town NPCs because they don't have a name
		/*public override List<string> SetNPCNameList()
		{
			return new List<string>() { };
		}*/

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/Betsy_Glow");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}
			Color color = Color.White;
			int spriteWidth = 40;
			int spriteHeight = 56;
			int x = NPC.frame.X;
			int y = NPC.frame.Y;

			Vector2 posOffset = new(NPC.position.X - Main.screenPosition.X - (spriteWidth - 16f) / 2f - 191f, NPC.position.Y - Main.screenPosition.Y - 204f);
			spriteBatch.Draw(glowmask.Value, posOffset + screenOffset, (Rectangle?)new Rectangle(x, y, spriteWidth, spriteHeight), color, 0f, default, 1f, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
		}

		public override string GetChat()
		{
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 4; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				int abominationn = NPC.FindFirstNPC(fargosMutant.Find<ModNPC>("Abominationn").Type);
				if (abominationn >= 0)
				{
					chat.Add(Language.GetTextValue(path + "FargosMutantMod", Main.npc[abominationn].GivenName));
				}
			}
			if (ModLoader.TryGetMod("SGAmod", out Mod sGAmod))
			{
				int draken = NPC.FindFirstNPC(sGAmod.Find<ModNPC>("Dergon").Type);
				if (draken >= 0)
				{
					chat.Add(Language.GetTextValue(path + "SGAmod", Main.npc[draken].GivenName));
				}
			}
			return chat;
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				button2 = Language.GetTextValue("LegacyInterface.28") + " 2";
			}
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
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.DD2ElderCrystal);
				shop.item[nextSlot].shopCustomPrice = 40000;
				nextSlot++;
				if (BossesAsNPCsWorld.downedDarkMage)
				{
					shop.item[nextSlot].SetDefaults(ItemID.WarTable);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.1);  //Formula: (Sell value / drop chance))
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.WarTableBanner);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.1);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PetDragon); //Dragon Egg
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PetGato); //Gato Egg
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossMaskDarkMage);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossTrophyDarkmage);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
					nextSlot++;
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.DarkMageBookMountItem);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.DarkMageMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}
				if (BossesAsNPCsWorld.downedOgre)
				{
					shop.item[nextSlot].SetDefaults(ItemID.ApprenticeScarf);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SquireShield);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.HuntressBuckler);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MonkBelt);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.08);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BookStaff); //Tome of Infinite Wisdom
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PhoenixBow); //Phantom Phoenix
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2SquireDemonSword); //Brand of the Inferno
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MonkStaffT1); //Sleepy Octopod
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.MonkStaffT2); //Ghastly Glaive
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.07);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DD2PetGhost); //Creeper Egg
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.2);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossMaskOgre);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BossTrophyOgre);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
					nextSlot++;
					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.DD2OgrePetItem);
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.OgreMasterTrophy);
						shop.item[nextSlot].shopCustomPrice = 10000 * 5;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.DD2BetsyBow); //Aerial Bane
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);  //Formula: (Sell value * 2) * ((1/drop chance)/2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MonkStaffT3); //Sky Dragon's Fury
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ApprenticeStaffT3); //Betsy's Wrath
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DD2SquireBetsySword); //Flying Dragon
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BetsyWings);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.07);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BossMaskBetsy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BossTrophyBetsy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DD2BetsyPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BetsyMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxDD2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Betsy.BeCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}

			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					if (BossesAsNPCsWorld.downedDarkMage)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "ForbiddenTome", shop, ref nextSlot, 50000); //Match the Abominationn's shop
					}

					if (BossesAsNPCsWorld.downedOgre)
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "BatteredClub", shop, ref nextSlot, 150000); //Match the Abominationn's shop
					}

					NPCHelper.SafelySetCrossModItem(fargosMutant, "BetsyEgg", shop, ref nextSlot, 400000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DragonBreath", shop, ref nextSlot, 0.1f); //Dragon's Breath

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "BetsysHeart", shop, ref nextSlot); //Betsy's Heart
					}
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "BetsyScale", shop, ref nextSlot);
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "FlameCore", shop, ref nextSlot); //Betsy's Flame
					}
				}
				if (ModLoader.TryGetMod("PboneUtils", out Mod pbonesUtilities))
				{
					NPCHelper.SafelySetCrossModItem(pbonesUtilities, "DefendersCrystal", shop, ref nextSlot);
				}
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
			projType = ProjectileID.ApprenticeStaffT3Shot;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
	}
	public class BetsyProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;

		//Normally you'd want to choose a random name, but these Town NPCs have no name.
		//public string GetNameForVariant(NPC npc) => npc.getNewNPCName();
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}