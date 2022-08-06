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
	public class TheDestroyer : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.TheDestroyer"));
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
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Retinazer>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Spazmatism>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<SkeletronPrime>(), AffectionLevel.Love)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EaterOfWorlds>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MoonLord>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Dislike)
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
			NPC.defense = 30;
			NPC.lifeMax = 8000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Merchant;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtTheDestroyer>() : -1;
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
			if (Main.netMode != NetmodeID.Server)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedMechBoss1 && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDestroyer)
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
            return new TheDestroyerProfile();
        }

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/TheDestroyer_Glow");
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
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
            if (mechanic >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Mechanic", Main.npc[mechanic].GivenName));
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
				shop.item[nextSlot].SetDefaults(ItemID.MechanicalWorm);
				shop.item[nextSlot].shopCustomPrice = 250000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.HallowedBar);
				shop.item[nextSlot].shopCustomPrice = 400 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SoulofMight);
				shop.item[nextSlot].shopCustomPrice = 800 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DestroyerMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DestroyerTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MechanicalWagonPiece);
					shop.item[nextSlot].shopCustomPrice = 5000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DestroyerPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DestroyerMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss3);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TheDestroyer.DeCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MechWorm", shop, ref nextSlot, 400000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(fargosMutant, "MechanicalAmalgam", shop, ref nextSlot, 1000000);
					}
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeDestroyer", shop, ref nextSlot, 10000);

					if (NPCHelper.DownedMechBossAll())
					{
						NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeMechs", shop, ref nextSlot, 10000);
					}
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "DestroyerGun", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "GroundStick", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "PrimeAccess", shop, ref nextSlot);
					}
				}
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return true;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 15;
			knockback = 2f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<Projectiles.MiniProbe>();
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 5f;
			gravityCorrection = 5f;
			randomOffset = 5;
		}
	}
	public class TheDestroyerProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}