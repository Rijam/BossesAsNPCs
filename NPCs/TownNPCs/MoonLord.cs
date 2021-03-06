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
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtMoonLord>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void OnKill()
		{
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Torso").Type, 1f);
			for (int k = 0; k < 2; k++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
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
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 8; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Dryad", Main.npc[dryad].GivenName));
			}
			int lunaticCultist = NPC.FindFirstNPC(ModContent.NPCType<LunaticCultist>());
			if (lunaticCultist >= 0)
			{
				chat.Add(Language.GetTextValue(path + "LunaticCultist"));
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Mechanic", Main.npc[mechanic].GivenName));
			}
			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && townNPCsCrossModSupport)
			{
				int torchMan = NPC.FindFirstNPC(torchSeller.Find<ModNPC>("TorchSellerNPC").Type);
				if (torchMan >= 0)
				{
					chat.Add(Language.GetTextValue(path + "TorchMerchant", Main.npc[torchMan].GivenName));
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
				shop.item[nextSlot].SetDefaults(ItemID.CelestialSigil);
				shop.item[nextSlot].shopCustomPrice = 500000; //made up value
				nextSlot++;
				
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
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
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
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CelestialSigil2", shop, ref nextSlot, 1000000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMoonLord", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CelestialOnion", shop, ref nextSlot, 100000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "UtensilPoker", shop, ref nextSlot, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DeviousAestheticus", shop, ref nextSlot, 0.05f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "GalacticGlobe", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "TrueThirdEye", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Cosmic_Key", shop, ref nextSlot, 100000);
				}
				if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage))
				{
					NPCHelper.SafelySetCrossModItem(magicStorage, "RadiantJewel", shop, ref nextSlot, 0.05f);
				}
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

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}