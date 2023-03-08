using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.GameContent.Personalities;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class KingSlime : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod) => NPCHelper.ShouldLoad(Name);

		private static Profiles.StackedNPCProfile NPCProfile;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault(Language.GetTextValue("NPCName.KingSlime"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.DyeTrader];
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
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<QueenSlime>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Pumpking>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<DukeFishron>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Golfer, AffectionLevel.Like)
				//Princess is automatically set
			; // < Mind the semicolon!

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party")
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
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.DyeTrader;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtKingSlime>() : -1;
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

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				if (!Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Crown").Type, 1f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (NPC.downedSlimeKing && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnKingSlime)
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

			int kingSlime = NPC.FindFirstNPC(ModContent.NPCType<KingSlime>());
			NPCHelper.GetNearbyResidentNPCs(Main.npc[kingSlime], 1, out List<int> npcTypeListHouse, out List<int> npcTypeListNearBy, out List<int> npcTypeListVillage, out List<int> _);

			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 8; i++) // Always available default 8 quotes.
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (!Main.dayTime) // Night time.
			{
				chat.Add(Language.GetTextValue(path + "Night"));
			}
			chat.Add(Language.GetTextValue(path + "Rare"), 0.1); // Always available, but 10 times rarer.

			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp) // During a party.
			{
				chat.Add(Language.GetTextValue(path + "Party1"), 2.0); // 2 times more common.
				chat.Add(Language.GetTextValue(path + "Party2"), 2.0);
			}
			if (!NPC.downedQueenSlime) // Queen Slime is not defeated.
			{
				chat.Add(Language.GetTextValue(path + "QS1"));
			}
			if (NPC.downedQueenSlime) // Queen Slime is defeated.
			{
				chat.Add(Language.GetTextValue(path + "QS2"));
				int queenSlime = NPC.FindFirstNPC(ModContent.NPCType<QueenSlime>());
				if (queenSlime >= 0 && npcTypeListVillage.Contains(ModContent.NPCType<QueenSlime>())) // If Queen Slime is within 120 tiles
				{
					chat.Add(Language.GetTextValue(path + "QS3"));
					chat.Add(Language.GetTextValue(path + "QS4"));
				}
			}
			if (Main.slimeRain) // During Slime Rain.
			{
				chat.Add(Language.GetTextValue(path + "SlimeRain"), 2.0);
			}
			if (Main.IsItRaining) // During rain (including Thunderstorm).
			{
				chat.Add(Language.GetTextValue(path + "Rain1"), 1.5);
				chat.Add(Language.GetTextValue(path + "Rain2"), 1.5);
			}
			if (Main.eclipse) // During Solar Eclipse.
			{
				chat.Add(Language.GetTextValue(path + "Eclipse"), 1.5);
			}
			if (Main.bloodMoon) // During Blood Moon
			{
				chat.Add(Language.GetTextValue(path + "BloodMoon"), 1.5);
			}
			if (Main.IsItAHappyWindyDay) // During Windy Day
			{
				chat.Add(Language.GetTextValue(path + "WindyDay1"), 1.5);
				int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
				if (partyGirl >= 0) // Party Girl is present
				{
					chat.Add(Language.GetTextValue(path + "WindyDay2", Main.npc[partyGirl].GivenName), 1.5);
				}
			}
			if (Main.IsItStorming) // During Thunderstorm
			{
				chat.Add(Language.GetTextValue(path + "Thunderstorm"), 1.5);
			}
			if (Main.LocalPlayer.ZoneGraveyard) // If the player is in a Graveyard biome. Yes, all biome checks actually check the player.
			{
				chat.Add(Language.GetTextValue(path + "Graveyard"), 1.5);
			}
			int eoc = NPC.FindFirstNPC(ModContent.NPCType<EyeOfCthulhu>());
			if (eoc >= 0 && npcTypeListNearBy.Contains(ModContent.NPCType<EyeOfCthulhu>())) // If EoC is within 50 tiles.
			{
				chat.Add(Language.GetTextValue(path + "EoC"));
			}
			int pk = NPC.FindFirstNPC(ModContent.NPCType<Pumpking>());
			if (pk >= 0 && npcTypeListNearBy.Contains(ModContent.NPCType<Pumpking>()))
			{
				chat.Add(Language.GetTextValue(path + "Pk"));
			}
			int df = NPC.FindFirstNPC(ModContent.NPCType<DukeFishron>());
			if (df >= 0 && npcTypeListNearBy.Contains(ModContent.NPCType<DukeFishron>()))
			{
				chat.Add(Language.GetTextValue(path + "DF"));
			}
			int guide = NPC.FindFirstNPC(NPCID.Guide);
			if (guide >= 0 && npcTypeListNearBy.Contains(NPCID.Guide))
			{
				chat.Add(Language.GetTextValue(path + "Guide", Main.npc[guide].GivenName));
			}
			int dyeTrader = NPC.FindFirstNPC(NPCID.DyeTrader);
			if (dyeTrader >= 0 && npcTypeListNearBy.Contains(NPCID.DyeTrader))
			{
				chat.Add(Language.GetTextValue(path + "DyeTrader", Main.npc[dyeTrader].GivenName));
			}
			int golfer = NPC.FindFirstNPC(NPCID.Golfer);
			if (golfer >= 0 && npcTypeListNearBy.Contains(NPCID.Golfer))
			{
				chat.Add(Language.GetTextValue(path + "Golfer", Main.npc[golfer].GivenName));
			}
			int betsy = NPC.FindFirstNPC(ModContent.NPCType<Betsy>());
			if (betsy >= 0 && npcTypeListNearBy.Contains(ModContent.NPCType<Betsy>()))
			{
				chat.Add(Language.GetTextValue(path + "Betsy"));
			}
			int squireSlime = NPC.FindFirstNPC(NPCID.TownSlimeCopper);
			if (squireSlime >= 0 && npcTypeListHouse.Contains(NPCID.TownSlimeCopper)) // Within 25 tiles.
			{
				chat.Add(Language.GetTextValue(path + "SquireSlime", Main.npc[squireSlime].FullName));
			}

			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && townNPCsCrossModSupport)
			{
				if (torchSeller.TryFind<ModNPC>("TorchSellerNPC", out ModNPC torchManModNPC)) // Using TryFind is safer for cross mod things.
				{
					int torchMan = NPC.FindFirstNPC(torchManModNPC.Type);
					if (torchMan >= 0)
					{
						chat.Add(Language.GetTextValue(path + "TorchMerchant", Main.npc[torchMan].FullName));
					}
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
			SetupShops.KingSlime(shop, ref nextSlot);
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
			projType = ModContent.ProjectileType<Projectiles.SpikedSlimeProjectile>();
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
}