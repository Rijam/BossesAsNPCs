using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using BossesAsNPCs.EmoteBubbles;
using Microsoft.Xna.Framework;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Skeletron : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod) => NPCHelper.ShouldLoad(Name);

		private const string Shop1 = "Shop1";
		private const string Shop2 = "Shop2";
		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load()
		{
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name + "_Head");
		}

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 25;
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 2;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
			NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<SkeletronEmote>();

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<DungeonBiome>(AffectionLevel.Love)
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Like)
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike)
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
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)),
				new Profiles.DefaultNPCProfile(GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name, ShimmerHeadIndex, GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name + "_Party")
			);

			// Here we define which portrait to use for the Town NPC when the portrait style setting is set to detailed.
			NPCID.Sets.NPCPortraits.Add(Type, NPCID.Sets.PrioritizedPortrait()
				.With(NPCID.Sets.ShimmeredPortraitCondition, NPCID.Sets.BasicPortrait(GetType().Namespace.Replace('.', '/') + "/Shimmered/" + Name + "_Portrait"))
				.Default(NPCID.Sets.BasicPortrait($"{Texture}_Portrait")));
			NPCID.Sets.NPCPortraitsCloseUpOffsets.Add(Type, new Vector2(-3f, 0f)); // Here we can change the offsets of Town NPC when the portrait style setting is set to profile.
			NPCID.Sets.NPCPortraitsFullBodyRetroOffsets.Add(Type, new Vector2(0f, 0f)); // Here we can change the offsets of Town NPC when the portrait style setting is set to retro.

			// Specify the debuffs it is immune to
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.BoneJavelin] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.BloodButcherer] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.TentacleSpike] = true;

			TownNPCLiveInBadBiomeSets.CanLiveInDungeon[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
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
			bestiaryEntry.Info.AddRange(
			[
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				// new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			]);
			if (NPCHelper.ShouldAddHappinessInfoBox())
			{
				bestiaryEntry.Info.Add(new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name)));
			}
		}

		public override void HitEffect(NPC.HitInfo hitInfo)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				int partyHatGore = NPC.GetPartyHatGore();
				if (partyHatGore > 0)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, partyHatGore);
				}
				if (NPC.IsShimmerVariant)
				{
					if (partyHatGore == 0)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, -10), NPC.velocity + new Vector2(0, -6), ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Hat_Shimmered").Type, 1f);
					}
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head_Shimmered").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Jaw_Shimmered").Type, 1f);
					for (int k = 0; k < 2; k++)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm_Shimmered").Type, 1f);
						Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg_Shimmered").Type, 1f);
					}
				}
				else
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Jaw").Type, 1f);
					for (int k = 0; k < 2; k++)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
						Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
					}
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (NPC.downedBoss3 && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnSkeletron)
			{
				return true;
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}

		// public override void PostAI() => NPC.color = NPC.IsShimmerVariant ? Main.DiscoColor : default; // Make the color of the NPC rainbow when shimmered.
		public override void PostAI() {}

		public override string GetChat()
		{
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 5; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			chat.Add(Language.GetTextValue(path + "Rare"), 0.1);
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			if (Condition.BloodMoon.IsMet())
			{
				chat.Add(Language.GetTextValue(path + "BloodMoon"), 2.0);
			}
			if (Condition.InGraveyard.IsMet())
			{
				chat.Add(Language.GetTextValue(path + "Graveyard"));
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

		public override void RegisterChatButtons(NPCInteractionList interactions)
		{
			NPCHelper.RegisterShop1AndShop2(interactions, Shop1, Shop2);
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