using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using BossesAsNPCs.EmoteBubbles;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class TorchGod : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode > 0 || NPCHelper.bypassMode;
		}

		private static ITownNPCProfile NPCProfile;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 26;
			NPCID.Sets.ExtraFramesCount[Type] = 10;
			NPCID.Sets.AttackFrameCount[Type] = 5;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 80;
			NPCID.Sets.AttackAverageChance[Type] = 5; // Lower numbers actually make the NPC more likely to attack
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
			NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<TorchGodEmote>();

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new ()
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MoonLord>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Golem>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Pumpking>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Betsy>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Dreadnautilus>(), AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike)
			//Princess is automatically set
			; // < Mind the semicolon!

			NPCProfile = new TorchGodProfile();

			// Specify the debuffs it is immune to
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.damage = 10;
			NPC.defense = 40;
			NPC.lifeMax = 20000;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.LiquidsWaterLava;
			NPC.knockBackResist = 0.25f;
			AnimationType = NPCID.Merchant;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtTorchGod>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(
			[
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			]);
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
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FlameBurst, 1, 1, 100, Color.White, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (NPCHelper.DownedAnyBossWithConfigCheck() && ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode == BossesAsNPCsConfigServer.AllInOneOptions.Mixed)
			{
				return true;
			}
			if (NPCHelper.DownedAnyBoss() && ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode == BossesAsNPCsConfigServer.AllInOneOptions.OnlyOne)
			{
				return true;
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}
		//SetNPCNameList is not needed for these Town NPCs because they don't have a name
		/*public override List<string> SetNPCNameList()
		{
			return new List<string>() { };
		}*/

		public override void PostAI() => NPC.color = NPC.IsShimmerVariant ? Main.DiscoColor : default; // Make the color of the NPC rainbow when shimmered.

		// Hacky solution to play the frames the way I wanted them
		public override void FindFrame(int frameHeight)
		{
			int attackTimeDiv = NPCID.Sets.AttackTime[NPC.type] / 8;
			if (NPC.ai[0] == 10) // Attacking
			{
				if (NPC.localAI[3] == 1)
				{
					SoundEngine.PlaySound(new("Terraria/Sounds/Item_74") { Pitch = -1f }, NPC.Center); // Other good options 20 45 66 69 74 80 88
				}
				if (NPC.localAI[3] < attackTimeDiv * 1)
				{
					Lighting.AddLight(NPC.Center, 1f, 0.0f, 0.0f);
					NPC.frame.Y = 20 * frameHeight;
				}
				else if (NPC.localAI[3] < attackTimeDiv * 2)
				{
					Lighting.AddLight(NPC.Center, 0.9f, 0.1f, 0.1f);
					NPC.frame.Y = 21 * frameHeight;
				}
				else if (NPC.localAI[3] < attackTimeDiv * 3)
				{
					Lighting.AddLight(NPC.Center, 0.8f, 0.2f, 0.2f);
					NPC.frame.Y = 22 * frameHeight;
				}
				else if (NPC.localAI[3] < attackTimeDiv * 4)
				{
					Lighting.AddLight(NPC.Center, 0.7f, 0.3f, 0.5f);
					NPC.frame.Y = 23 * frameHeight;
				}
				else if (NPC.localAI[3] < attackTimeDiv * 5)
				{
					Lighting.AddLight(NPC.Center, 0.6f, 0.4f, 0.8f);
					NPC.frame.Y = 24 * frameHeight;
				}
				else if (NPC.localAI[3] < attackTimeDiv * 6)
				{
					Lighting.AddLight(NPC.Center, 0.5f, 0.5f, 1f);
					NPC.frame.Y = 25 * frameHeight;
				}
				else if (NPC.localAI[3] <= attackTimeDiv * 8)
				{
					Lighting.AddLight(NPC.Center, 0.5f, 0.0f, 0.5f);
					NPC.frame.Y = 25 * frameHeight;
				}
				else
				{
					NPC.frame.Y = 0 * frameHeight;
				}
			}
		}

		// random taken from Torch Merchant by cace#7129
		// Sitting frame height is corrected here.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/TorchGod_Glow");
		private readonly Asset<Texture2D> background = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/TorchGod_FlamesBackground");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)NPC.position.Y << 32) | (uint)NPC.position.X);
			Color color = NPCHelper.GlowColor(NPC, 255, 255, 255, 100);

			for (int i = 0; i < 5; i++)
			{
				float randomX = Utils.RandomInt(ref seed, -11, 11) * 0.05f;
				float randomY = Utils.RandomInt(ref seed, -5, 5) * 0.15f;

				spriteBatch.Draw(glowmask.Value, NPC.Center - screenPos + NPCHelper.DrawingOffsets(NPC) + new Vector2(randomX, randomY), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)NPC.position.Y << 32) | (uint)NPC.position.X);
			Color color = NPCHelper.GlowColor(NPC, 255, 255, 255, 100);

			if (NPC.frame.Y > 20 * NPC.frame.Height) // Only draw while attacking
			{
				for (int i = 0; i < 5; i++)
				{
					float randomX = Utils.RandomInt(ref seed, -50, 50) * 0.15f;
					float randomY = Utils.RandomInt(ref seed, -20, 20) * 0.15f;

					spriteBatch.Draw(background.Value, NPC.Center - screenPos + NPCHelper.DrawingOffsets(NPC) + new Vector2(randomX, randomY), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
				}
			}
			return true;
		}

		public override string GetChat()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 9; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			chat.Add(Language.GetTextValue(path + "Common"), 2);
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
			if (Main.LocalPlayer.unlockedBiomeTorches)
			{
				chat.Add(Language.GetTextValue(path + "HasFavor1"));
				chat.Add(Language.GetTextValue(path + "HasFavor2"));
			}
			else
			{
				chat.Add(Language.GetTextValue(path + "NoFavor1"));
				chat.Add(Language.GetTextValue(path + "NoFavor2"));
			}
			int nurse = NPC.FindFirstNPC(NPCID.Nurse);
			if (nurse >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Nurse", Main.npc[nurse].GivenName));
			}
			int moonLord = NPC.FindFirstNPC(ModContent.NPCType<MoonLord>());
			if (moonLord >= 0)
			{
				chat.Add(Language.GetTextValue(path + "MoonLord"));
			}
			int plantera = NPC.FindFirstNPC(ModContent.NPCType<Plantera>());
			if (plantera >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Plantera", Main.npc[plantera].FullName), 0.25);
			}
			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && townNPCsCrossModSupport)
			{
				int torchMan = NPC.FindFirstNPC(torchSeller.Find<ModNPC>("TorchSellerNPC").Type);
				if (torchMan >= 0)
				{
					chat.Add(Language.GetTextValue(path + "TorchMerchant", Main.npc[torchMan].GivenName));
				}
			}
			if (Main.LocalPlayer.name == "Redigit")
			{
				chat.Add(Language.GetTextValue(path + "NameIsRedigit"), 2);
			}
			if (ModLoader.TryGetMod("OverhaulMod", out Mod _) && townNPCsCrossModSupport)
			{
				chat.Add(Language.GetTextValue(path + "Overhaul"));
			}
			return chat;
		}

		public override void RegisterChatButtons(NPCInteractionList interactions)
		{
			// Close, Happiness, and Housing are added first.
			// Add Previous Page and Next Page buttons
			interactions.Append(new NPCHelper.TorchGodPreviousPage());
			interactions.Append(new NPCHelper.TorchGodNextPage());
			// Add the shops for every other Boss NPC.
			NPCHelper.TorchGodRegisterShopsMode0(interactions); // Mode 0 should technically never be seen. Torch God doesn't load while it is in mode 0.
			NPCHelper.TorchGodRegisterShopsMode1(interactions);
			NPCHelper.TorchGodRegisterShopsMode2(interactions);
		}

		// Shops are registered in NPCHelper

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return toKingStatue;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 35;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 5;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<Projectiles.FireBolt>();
			attackDelay = 40;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
	}
	public class TorchGodProfile : ITownNPCProfile
	{
		public string Path => (GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/');

		public int RollVariation() => 0;

		// Normally you'd want to choose a random name, but these Town NPCs have no name.
		// public string GetNameForVariant(NPC npc) => npc.getNewNPCName();
		public string GetNameForVariant(NPC npc) => null;

		private Asset<Texture2D> bestiaryTexture;
		private Asset<Texture2D> regularTexture;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
			{
				return bestiaryTexture ??= ModContent.Request<Texture2D>(Path + "_Bestiary");
			}
			return regularTexture ??= ModContent.Request<Texture2D>(Path);
		}
		
		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot(Path + "_Head");
	}
}