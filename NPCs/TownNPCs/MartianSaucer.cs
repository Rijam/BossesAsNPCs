using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class MartianSaucer : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.MartianSaucer"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 5;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 1;
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
				//.SetBiomeAffection<SkyBiome>(AffectionLevel.Love)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Pumpking>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Betsy>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Dislike)
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
			NPC.defense = 100;
			NPC.lifeMax = 2700;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtMartianSaucer>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void OnKill()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedMartians && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMartianSaucer)
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
			return new MartianSaucerProfile();
		}

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/MartianSaucer_Glow");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}
			Color color = Color.White;
			int spriteWidth = 48;
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
			if (ModLoader.TryGetMod("RijamsMod", out Mod rijamsMod) && townNPCsCrossModSupport)
			{
				int intTrav = NPC.FindFirstNPC(rijamsMod.Find<ModNPC>("InterstellarTraveler").Type);
				if (intTrav >= 0)
				{
					chat.Add(Language.GetTextValue(path + "RijamsMod", Main.npc[intTrav].GivenName));
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
				shop.item[nextSlot].SetDefaults(ItemID.MartianConduitPlating);
				shop.item[nextSlot].shopCustomPrice = 100; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianCostumeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianCostumeShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianCostumePants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianUniformHelmet);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianUniformTorso);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianUniformPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrainScrambler);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.01);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LaserDrill);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7); //Special case to make it cheaper
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChargedBlasterCannon);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7); //Special case to make it cheaper
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.AntiGravityHook);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.013 / 7); //Special case to make it cheaper
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Xenopopper);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.XenoStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LaserMachinegun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ElectrosphereLauncher);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.InfluxWaver);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CosmicCarKey);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MartianSaucerTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MartianPetItem); //Cosmic Skateboard
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.UFOMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxMartians);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "RunawayProbe", shop, ref nextSlot, 500000); //Match the Abominationn's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MartianMemoryStick", shop, ref nextSlot, 300000); //Match the Abominationn's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "ShockGrenade", shop, ref nextSlot);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Wingman", shop, ref nextSlot, 0.14f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "NullificationRifle", shop, ref nextSlot, 0.25f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "SaucerControlConsole", shop, ref nextSlot, 0.2f);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "SuperDartLauncher", shop, ref nextSlot, 0.01f * 6);
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
			projType = ProjectileID.ChargedBlasterOrb;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
			item = ItemID.None;
			scale = 1f;
			closeness = 0;
		}
	}
	public class MartianSaucerProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}