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
	public class Skeletron : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod) => NPCHelper.ShouldLoad(Name);

		private const string Shop1 = "Shop1";
		private const string Shop2 = "Shop2";
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(Language.GetTextValue("NPCName.SkeletronHead"));
			Main.npcFrameCount[Type] = 25; // Main.npcFrameCount[NPCID.DyeTrader];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 2;
			NPCID.Sets.ShimmerTownTransform[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<LunaticCultist>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<SkeletronPrime>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MoonLord>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Dislike)
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
			NPC.lifeMax = 440;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtSkeletron>() : -1;
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

		public override void HitEffect(NPC.HitInfo hitInfo)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Jaw").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (NPC.downedBoss3 && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnSkeletron)
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
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 4; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			chat.Add(Language.GetTextValue(path + ".Rare"), 0.1);
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Dryad", Main.npc[dryad].GivenName));
			}
			int clothier = NPC.FindFirstNPC(NPCID.Clothier);
			if (clothier >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Clothier", Main.npc[clothier].GivenName));
			}
			int skeletronPrime = NPC.FindFirstNPC(ModContent.NPCType<SkeletronPrime>());
			if (skeletronPrime >= 0)
			{
				chat.Add(Language.GetTextValue(path + "SP"));
			}
			if (BossesAsNPCsWorld.downedDungeonGuardian)
            {
				chat.Add(Language.GetTextValue(path + "DG"));
			}
			if (Main.LocalPlayer.HasItem(ItemID.SDMG))
			{
				chat.Add(Language.GetTextValue(path + "SDMG"));
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
			SetupShops.Skeletron(npcShop1, Shop1);
			npcShop1.Register();

			var npcShop2 = new NPCShop(Type, Shop2);
			SetupShops.Skeletron(npcShop2, Shop2);
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
			projType = ProjectileID.BookOfSkullsSkull;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
}