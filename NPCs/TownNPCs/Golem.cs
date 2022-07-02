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
	public class Golem : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.Golem"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Merchant];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 0;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Love)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Plantera>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EmpressOfLight>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<LunaticCultist>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike)
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
			NPC.defense = 20;
			NPC.lifeMax = 3900;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.knockBackResist = 0.25f;
			AnimationType = NPCID.Merchant;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtGolem>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
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
			if (NPC.downedGolemBoss && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnGolem)
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
            return new GolemProfile();
        }

		//PostDraw taken from Torch Merchant by cace#7129
		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/Golem_Glow");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 screenOffset = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}
			Color color = new(50, 50, 50, 0);
			int spriteWidth = 40;
			int spriteHeight = 56;
			int x = NPC.frame.X;
			int y = NPC.frame.Y;
			for (int i = 0; i < 5; i++)
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
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 3; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (witchDoctor >= 0)
			{
				chat.Add(Language.GetTextValue(path + "WitchDoctor").Replace("{0}", Main.npc[witchDoctor].GivenName));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			if (Main.getGoodWorld || WorldGen.getGoodWorldGen)
            {
				chat.Add(Language.GetTextValue(path + "ForTheWorthy"));
			}
			if (Main.player[Main.myPlayer].ZoneOverworldHeight)
            {
				chat.Add(Language.GetTextValue(path + "Surface"));
			}
			if (Main.player[Main.myPlayer].ZoneLihzhardTemple)
			{
				chat.Add(Language.GetTextValue(path + "Temple1"));
				chat.Add(Language.GetTextValue(path + "Temple2"));
			}
			int plantera = NPC.FindFirstNPC(ModContent.NPCType<Plantera>());
			if (plantera >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Plantera"));
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
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			shop.item[nextSlot].SetDefaults(ItemID.LihzahrdPowerCell);
			shop.item[nextSlot].shopCustomPrice = 350000; //Made up value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("LihzahrdPowerCell2").Type);
				shop.item[nextSlot].shopCustomPrice = 600000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.Picksaw);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(43200 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeetleHusk);
			shop.item[nextSlot].shopCustomPrice = 5000 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Stynger);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.StyngerBolt);
			shop.item[nextSlot].shopCustomPrice = 75;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PossessedHatchet);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SunStone);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.EyeoftheGolem);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.HeatRay);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.StaffofEarth);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GolemFist);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GolemMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GolemTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;

			if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
			{
				shop.item[nextSlot].SetDefaults(calamityMod.Find<ModItem>("KnowledgeGolem").Type);
				shop.item[nextSlot].shopCustomPrice = 10000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(calamityMod.Find<ModItem>("AegisBlade").Type);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value / 5 / 0.1);
				nextSlot++;
			}

			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.ShinyStone);
				shop.item[nextSlot].shopCustomPrice = 50000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.GolemPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.GolemMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
            {
				if (NPC.savedWizard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss4);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.LihzahrdBrick);
				shop.item[nextSlot].shopCustomPrice = 2500;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LihzahrdAltar);
				shop.item[nextSlot].shopCustomPrice = 60 * 5 * 1000; //sells for 60 copper, but that seems way to cheap for an item that you should only have one of.
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeBodypiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Golem.GolemCostumeLegpiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return true;
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
			projType = ProjectileID.BallofFire;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class GolemProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}