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

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class WallOfFlesh : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod) => NPCHelper.ShouldLoad(Name);

		private const string Shop1 = "Shop1";
		private const string Shop2 = "Shop2";
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(Language.GetTextValue("NPCName.WallofFlesh"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 5;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 0;
			NPCID.Sets.ShimmerTownTransform[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				//.SetBiomeAffection<UnderworldBiome>(AffectionLevel.Love)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
				//.SetBiomeAffection<SkyBiome>(AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EaterOfWorlds>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Dreadnautilus>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Dislike)
				//Princess is automatically set
			; // < Mind the semicolon!

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture))
			);
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
			NPC.lifeMax = 800;
			NPC.HitSound = SoundID.NPCHit8;
			NPC.DeathSound = SoundID.NPCDeath10;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtWallOfFlesh>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void HitEffect(NPC.HitInfo hitInfo)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * (k + 1), ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Flesh").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if ((Main.hardMode || BossesAsNPCsWorld.downedWoF) && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnWoF)
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
            return NPCProfile;
        }

		public override void PostAI() => NPC.color = NPC.IsShimmerVariant ? Main.DiscoColor : default; // Make the color of the NPC rainbow when shimmered.

		public override string GetChat()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 6; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (!Main.player[Main.myPlayer].ZoneUnderworldHeight)
            {
				chat.Add(Language.GetTextValue(path + "NotUnderworld"));
			}
			if (Main.expertMode)
            {
				chat.Add(Language.GetTextValue(path + "Expert"));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			int guide = NPC.FindFirstNPC(NPCID.Guide);
			if (guide >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Guide1", Main.npc[guide].GivenName));
				chat.Add(Language.GetTextValue(path + "Guide2", Main.npc[guide].GivenName));
			}
			if (ModLoader.TryGetMod("CalamityMod", out Mod _) && townNPCsCrossModSupport)
			{
				chat.Add(Language.GetTextValue(path + "Calamity1"));
				chat.Add(Language.GetTextValue(path + "Calamity2"));
			}
			else
            {
				if (!WorldGen.crimson)
                {
					chat.Add(Language.GetTextValue(path + "PwnCorruption"));
				}
				if (WorldGen.crimson)
				{
					chat.Add(Language.GetTextValue(path + "PwnCrimson"));
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

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				shop = Shop1;
				NPCHelper.SetShop1(true);
				NPCHelper.SetShop2(false);
			}
			if (!firstButton)
			{
				shop = Shop2;
				NPCHelper.SetShop1(false);
				NPCHelper.SetShop2(true);
			}
		}

		public override void AddShops()
		{
			var npcShop1 = new NPCShop(Type, Shop1);
			SetupShops.WallOfFlesh(npcShop1, Shop1);
			npcShop1.Register();

			var npcShop2 = new NPCShop(Type, Shop2);
			SetupShops.WallOfFlesh(npcShop2, Shop2);
			npcShop2.Register();
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
			projType = ModContent.ProjectileType<Projectiles.Leech>();
			attackDelay = 20;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
}