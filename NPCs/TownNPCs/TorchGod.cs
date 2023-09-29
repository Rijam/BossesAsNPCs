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
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using static BossesAsNPCs.BossesAsNPCsConfigServer;
using Terraria.DataStructures;

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
			// DisplayName.SetDefault(Language.GetTextValue("NPCName.TorchGod"));
			Main.npcFrameCount[Type] = 26;
			NPCID.Sets.ExtraFramesCount[Type] = 10;
			NPCID.Sets.AttackFrameCount[Type] = 5;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 80;
			NPCID.Sets.AttackAverageChance[Type] = 20; // Lower numbers actually make the NPC more likely to attack
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[Type] = true;

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
			NPC.aiStyle = 7;
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
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
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
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.FlameBurst, 1, 1, 100, Color.White, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{
			if (NPCHelper.DownedAnyBossWithConfigCheck() && ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode == AllInOneOptions.Mixed)
			{
				return true;
			}
			if (NPCHelper.DownedAnyBoss() && ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode == AllInOneOptions.OnlyOne)
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
					SoundEngine.PlaySound(new("Terraria/Sounds/Item_74") { Pitch = -1f }, NPC.Center); //Other good options 20 45 66 69 74 80 88
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

		//random taken from Torch Merchant by cace#7129
		//Sitting frame height is corrected here.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/TorchGod_Glow");
		private readonly Asset<Texture2D> background = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/TorchGod_FlamesBackground");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)NPC.position.Y << 32) | (uint)NPC.position.X);
			Color color = new(255, 255, 255, 100);
			Vector2 verticalOffset = new(0, 4);
			for (int i = 0; i < 5; i++)
			{
				float randomX = Utils.RandomInt(ref seed, -11, 11) * 0.05f;
				float randomY = Utils.RandomInt(ref seed, -5, 5) * 0.15f;

				if (NPC.frame.Y == 18 * NPC.frame.Height) // Sitting, move up 4 pixels
				{
					verticalOffset = new Vector2(0, 8);
				}

				spriteBatch.Draw(glowmask.Value, NPC.Center - screenPos - verticalOffset + new Vector2(randomX, randomY), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)NPC.position.Y << 32) | (uint)NPC.position.X);
			Color color = new(255, 255, 255, 100);

			if (NPC.frame.Y > 20 * NPC.frame.Height) //Only draw while attacking
			{
				for (int i = 0; i < 5; i++)
				{
					float randomX = Utils.RandomInt(ref seed, -50, 50) * 0.15f;
					float randomY = Utils.RandomInt(ref seed, -20, 20) * 0.15f;

					spriteBatch.Draw(background.Value, NPC.Center - screenPos - new Vector2(0, 4) + new Vector2(randomX, randomY), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
				}
			}
			return true;
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
			chat.Add(Language.GetTextValue(path + "Common"), 2);
			chat.Add(Language.GetTextValue(path + "Rare"), 0.1);
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
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = NPCHelper.StatusShopCycle() switch
			{
				0 => Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".NoShop"), // No shop selected
				1 => Language.GetTextValue("NPCName.KingSlime"), // 1 = King Slime
				2 => Language.GetTextValue("NPCName.KingSlime") + " 2", // 2 = King Slime 2
				3 => Language.GetTextValue("NPCName.EyeofCthulhu"), // 3 = EoC
				4 => Language.GetTextValue("NPCName.EyeofCthulhu") + " 2", // 4 = EoC 2
				5 => Language.GetTextValue("NPCName.EaterofWorldsHead"), // 5 = EoW
				6 => Language.GetTextValue("NPCName.EaterofWorldsHead") + " 2", // 6 = EoW 2
				7 => Language.GetTextValue("NPCName.BrainofCthulhu"),
				8 => Language.GetTextValue("NPCName.BrainofCthulhu") + " 2",
				9 => Language.GetTextValue("NPCName.QueenBee"),
				10 => Language.GetTextValue("NPCName.QueenBee") + " 2",
				11 => Language.GetTextValue("NPCName.SkeletronHead"),
				12 => Language.GetTextValue("NPCName.SkeletronHead") + " 2",
				13 => Language.GetTextValue("NPCName.Deerclops"),
				14 => Language.GetTextValue("NPCName.Deerclops") + " 2",
				15 => Language.GetTextValue("NPCName.WallofFlesh"),
				16 => Language.GetTextValue("NPCName.WallofFlesh") + " 2",
				17 => Language.GetTextValue("NPCName.QueenSlimeBoss"),
				18 => Language.GetTextValue("NPCName.QueenSlimeBoss") + " 2",
				19 => Language.GetTextValue("NPCName.TheDestroyer"),
				20 => Language.GetTextValue("NPCName.TheDestroyer") + " 2",
				21 => Language.GetTextValue("NPCName.Retinazer"),
				22 => Language.GetTextValue("NPCName.Retinazer") + " 2",
				23 => Language.GetTextValue("NPCName.Spazmatism"),
				24 => Language.GetTextValue("NPCName.Spazmatism") + " 2",
				25 => Language.GetTextValue("NPCName.SkeletronPrime"),
				26 => Language.GetTextValue("NPCName.SkeletronPrime") + " 2",
				27 => Language.GetTextValue("NPCName.Plantera"),
				28 => Language.GetTextValue("NPCName.Plantera") + " 2",
				29 => Language.GetTextValue("NPCName.Golem"),
				30 => Language.GetTextValue("NPCName.Golem") + " 2",
				31 => Language.GetTextValue("NPCName.HallowBoss"),
				32 => Language.GetTextValue("NPCName.HallowBoss") + " 2",
				33 => Language.GetTextValue("NPCName.DukeFishron"),
				34 => Language.GetTextValue("NPCName.DukeFishron") + " 2",
				35 => Language.GetTextValue("NPCName.DD2Betsy"),
				36 => Language.GetTextValue("NPCName.DD2Betsy") + " 2",
				37 => Language.GetTextValue("NPCName.CultistBoss"),
				38 => Language.GetTextValue("NPCName.CultistBoss") + " 2",
				39 => Language.GetTextValue("NPCName.MoonLordHead"),
				40 => Language.GetTextValue("NPCName.MoonLordHead") + " 2",
				41 => Language.GetTextValue("NPCName.BloodNautilus"),
				42 => Language.GetTextValue("NPCName.BloodNautilus") + " 2",
				43 => Language.GetTextValue("NPCName.Mothron"),
				44 => Language.GetTextValue("NPCName.Mothron") + " 2",
				45 => Language.GetTextValue("NPCName.Pumpking"),
				46 => Language.GetTextValue("NPCName.Pumpking") + " 2",
				47 => Language.GetTextValue("NPCName.IceQueen"),
				48 => Language.GetTextValue("NPCName.IceQueen") + " 2",
				49 => Language.GetTextValue("NPCName.MartianSaucer"),
				50 => Language.GetTextValue("NPCName.MartianSaucer") + " 2",
				_ => Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".NoShop"), // No shop selected
			};

			button2 = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".IncShop"); // Next Shop
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
			{
				button2 = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".DecShop"); // Previous Shop
			}
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{
				button2 = Language.GetTextValue("Mods." + Mod.Name + ".UI." + Name + ".FirstShop"); // First Shop
			}
		}

		private const string Shop1 = "Shop1";
		private const string Shop2 = "Shop2";
		private const string TorchGodShop1 = "TorchGodShop1", TorchGodShop2 = "TorchGodShop2", TorchGodShop3 = "TorchGodShop3";
		private const string TorchGodShop4 = "TorchGodShop4", TorchGodShop5 = "TorchGodShop5", TorchGodShop6 = "TorchGodShop6";
		private const string TorchGodShop7 = "TorchGodShop7", TorchGodShop8 = "TorchGodShop8", TorchGodShop9 = "TorchGodShop9";
		private const string TorchGodShop10 = "TorchGodShop10", TorchGodShop11 = "TorchGodShop11", TorchGodShop12 = "TorchGodShop12";
		private const string TorchGodShop13 = "TorchGodShop13", TorchGodShop14 = "TorchGodShop14", TorchGodShop15 = "TorchGodShop15";
		private const string TorchGodShop16 = "TorchGodShop16", TorchGodShop17 = "TorchGodShop17", TorchGodShop18 = "TorchGodShop18";
		private const string TorchGodShop19 = "TorchGodShop19", TorchGodShop20 = "TorchGodShop20", TorchGodShop21 = "TorchGodShop21";
		private const string TorchGodShop22 = "TorchGodShop22", TorchGodShop23 = "TorchGodShop23", TorchGodShop24 = "TorchGodShop24";
		private const string TorchGodShop25 = "TorchGodShop25", TorchGodShop26 = "TorchGodShop26", TorchGodShop27 = "TorchGodShop27";
		private const string TorchGodShop28 = "TorchGodShop28", TorchGodShop29 = "TorchGodShop29", TorchGodShop30 = "TorchGodShop30";
		private const string TorchGodShop31 = "TorchGodShop31", TorchGodShop32 = "TorchGodShop32", TorchGodShop33 = "TorchGodShop33";
		private const string TorchGodShop34 = "TorchGodShop34", TorchGodShop35 = "TorchGodShop35", TorchGodShop36 = "TorchGodShop36";
		private const string TorchGodShop37 = "TorchGodShop37", TorchGodShop38 = "TorchGodShop38", TorchGodShop39 = "TorchGodShop39";
		private const string TorchGodShop40 = "TorchGodShop40", TorchGodShop41 = "TorchGodShop41", TorchGodShop42 = "TorchGodShop42";
		private const string TorchGodShop43 = "TorchGodShop43", TorchGodShop44 = "TorchGodShop44", TorchGodShop45 = "TorchGodShop45";
		private const string TorchGodShop46 = "TorchGodShop46", TorchGodShop47 = "TorchGodShop47", TorchGodShop48 = "TorchGodShop48";
		private const string TorchGodShop49 = "TorchGodShop49", TorchGodShop50 = "TorchGodShop50";

		public static string ChooseCorrectShop()
		{
			return NPCHelper.StatusShopCycle() switch
			{
				0 => "", // No shop selected
				1 => TorchGodShop1, // 1 = King Slime
				2 => TorchGodShop2, // 2 = King Slime 2
				3 => TorchGodShop3, // 3 = EoC
				4 => TorchGodShop4, // 4 = EoC 2
				5 => TorchGodShop5, // 5 = EoW
				6 => TorchGodShop6, // 6 = EoW 2
				7 => TorchGodShop7,
				8 => TorchGodShop8,
				9 => TorchGodShop9,
				10 => TorchGodShop10,
				11 => TorchGodShop11,
				12 => TorchGodShop12,
				13 => TorchGodShop13,
				14 => TorchGodShop14,
				15 => TorchGodShop15,
				16 => TorchGodShop16,
				17 => TorchGodShop17,
				18 => TorchGodShop18,
				19 => TorchGodShop19,
				20 => TorchGodShop20,
				21 => TorchGodShop21,
				22 => TorchGodShop22,
				23 => TorchGodShop23,
				24 => TorchGodShop24,
				25 => TorchGodShop25,
				26 => TorchGodShop26,
				27 => TorchGodShop27,
				28 => TorchGodShop28,
				29 => TorchGodShop29,
				30 => TorchGodShop30,
				31 => TorchGodShop31,
				32 => TorchGodShop32,
				33 => TorchGodShop33,
				34 => TorchGodShop34,
				35 => TorchGodShop35,
				36 => TorchGodShop36,
				37 => TorchGodShop37,
				38 => TorchGodShop38,
				39 => TorchGodShop39,
				40 => TorchGodShop40,
				41 => TorchGodShop41,
				42 => TorchGodShop42,
				43 => TorchGodShop43,
				44 => TorchGodShop44,
				45 => TorchGodShop45,
				46 => TorchGodShop46,
				47 => TorchGodShop47,
				48 => TorchGodShop48,
				49 => TorchGodShop49,
				50 => TorchGodShop50,
				_ => "", // No shop selected
			};
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (firstButton)
			{
				if (NPCHelper.StatusShopCycle() <= 0 || NPCHelper.StatusShopCycle() >= 51)
				{
					Main.npcChatText = Language.GetTextValue(NPCHelper.DialogPath(Name) + "Common");
				}
				else
				{
					shop = ChooseCorrectShop();
				}
				NPCHelper.StatusShopCycle();
			}
			if (!firstButton)
			{
				AllInOneOptions mode = ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode;
				GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
				if (mode == AllInOneOptions.Off)
				{
					if (Main.keyState.IsKeyDown(Keys.LeftShift) || (gamePadState.IsConnected && gamePadState.Buttons.RightStick == ButtonState.Pressed))
					{
						NPCHelper.DecrementShopCycleMode0();
					}
					else if (Main.keyState.IsKeyDown(Keys.LeftControl) || (gamePadState.IsConnected && gamePadState.Buttons.LeftStick == ButtonState.Pressed))
					{
						NPCHelper.SetShopCycle(0);
						NPCHelper.IncrementShopCycleMode0();
					}
					else
					{
						NPCHelper.IncrementShopCycleMode0();
					}
				}
				else if (mode == AllInOneOptions.Mixed)
				{
					if (Main.keyState.IsKeyDown(Keys.LeftShift) || (gamePadState.IsConnected && gamePadState.Buttons.RightStick == ButtonState.Pressed))
					{
						NPCHelper.DecrementShopCycleMode1();
					}
					else if (Main.keyState.IsKeyDown(Keys.LeftControl) || (gamePadState.IsConnected && gamePadState.Buttons.LeftStick == ButtonState.Pressed))
					{
						NPCHelper.SetShopCycle(0);
						NPCHelper.IncrementShopCycleMode1();
					}
					else
					{
						NPCHelper.IncrementShopCycleMode1();
					}
				}
				else if (mode == AllInOneOptions.OnlyOne)
				{
					if (Main.keyState.IsKeyDown(Keys.LeftShift) || (gamePadState.IsConnected && gamePadState.Buttons.RightStick == ButtonState.Pressed))
					{
						NPCHelper.DecrementShopCycleMode2();
					}
					else if (Main.keyState.IsKeyDown(Keys.LeftControl) || (gamePadState.IsConnected && gamePadState.Buttons.LeftStick == ButtonState.Pressed))
					{
						NPCHelper.SetShopCycle(0);
						NPCHelper.IncrementShopCycleMode2();
					}
					else
					{
						NPCHelper.IncrementShopCycleMode2();
					}
				}
			}
			//Main.NewText("NPCHelper.StatusShopCycle() " + NPCHelper.StatusShopCycle());
		}

		public override void AddShops()
		{
			var npcTorchGodShop1 = new NPCShop(Type, TorchGodShop1);
			SetupShops.KingSlime(npcTorchGodShop1, Shop1);
			npcTorchGodShop1.Register();

			var npcTorchGodShop2 = new NPCShop(Type, TorchGodShop2);
			SetupShops.KingSlime(npcTorchGodShop2, Shop2);
			npcTorchGodShop2.Register();

			var npcTorchGodShop3 = new NPCShop(Type, TorchGodShop3);
			SetupShops.EyeOfCthulhu(npcTorchGodShop3, Shop1);
			npcTorchGodShop3.Register();

			var npcTorchGodShop4 = new NPCShop(Type, TorchGodShop4);
			SetupShops.EyeOfCthulhu(npcTorchGodShop4, Shop2);
			npcTorchGodShop4.Register();

			var npcTorchGodShop5 = new NPCShop(Type, TorchGodShop5);
			SetupShops.EaterOfWorlds(npcTorchGodShop5, Shop1);
			npcTorchGodShop5.Register();

			var npcTorchGodShop6 = new NPCShop(Type, TorchGodShop6);
			SetupShops.EaterOfWorlds(npcTorchGodShop6, Shop2);
			npcTorchGodShop6.Register();

			var npcTorchGodShop7 = new NPCShop(Type, TorchGodShop7);
			SetupShops.BrainOfCthulhu(npcTorchGodShop7, Shop1);
			npcTorchGodShop7.Register();

			var npcTorchGodShop8 = new NPCShop(Type, TorchGodShop8);
			SetupShops.BrainOfCthulhu(npcTorchGodShop8, Shop2);
			npcTorchGodShop8.Register();

			var npcTorchGodShop9 = new NPCShop(Type, TorchGodShop9);
			SetupShops.QueenBee(npcTorchGodShop9, Shop1);
			npcTorchGodShop9.Register();

			var npcTorchGodShop10 = new NPCShop(Type, TorchGodShop10);
			SetupShops.QueenBee(npcTorchGodShop10, Shop2);
			npcTorchGodShop10.Register();

			var npcTorchGodShop11 = new NPCShop(Type, TorchGodShop11);
			SetupShops.Skeletron(npcTorchGodShop11, Shop1);
			npcTorchGodShop11.Register();

			var npcTorchGodShop12 = new NPCShop(Type, TorchGodShop12);
			SetupShops.Skeletron(npcTorchGodShop12, Shop2);
			npcTorchGodShop12.Register();

			var npcTorchGodShop13 = new NPCShop(Type, TorchGodShop13);
			SetupShops.Deerclops(npcTorchGodShop13, Shop1);
			npcTorchGodShop13.Register();

			var npcTorchGodShop14 = new NPCShop(Type, TorchGodShop14);
			SetupShops.Deerclops(npcTorchGodShop14, Shop2);
			npcTorchGodShop14.Register();

			var npcTorchGodShop15 = new NPCShop(Type, TorchGodShop15);
			SetupShops.WallOfFlesh(npcTorchGodShop15, Shop1);
			npcTorchGodShop15.Register();

			var npcTorchGodShop16 = new NPCShop(Type, TorchGodShop16);
			SetupShops.WallOfFlesh(npcTorchGodShop16, Shop2);
			npcTorchGodShop16.Register();

			var npcTorchGodShop17 = new NPCShop(Type, TorchGodShop17);
			SetupShops.QueenSlime(npcTorchGodShop17, Shop1);
			npcTorchGodShop17.Register();

			var npcTorchGodShop18 = new NPCShop(Type, TorchGodShop18);
			SetupShops.QueenSlime(npcTorchGodShop18, Shop2);
			npcTorchGodShop18.Register();

			var npcTorchGodShop19 = new NPCShop(Type, TorchGodShop19);
			SetupShops.TheDestroyer(npcTorchGodShop19, Shop1);
			npcTorchGodShop19.Register();

			var npcTorchGodShop20 = new NPCShop(Type, TorchGodShop20);
			SetupShops.TheDestroyer(npcTorchGodShop20, Shop2);
			npcTorchGodShop20.Register();

			var npcTorchGodShop21 = new NPCShop(Type, TorchGodShop21);
			SetupShops.Spazmatism(npcTorchGodShop21, Shop1);
			npcTorchGodShop21.Register();

			var npcTorchGodShop22 = new NPCShop(Type, TorchGodShop22);
			SetupShops.Spazmatism(npcTorchGodShop22, Shop2);
			npcTorchGodShop22.Register();

			var npcTorchGodShop23 = new NPCShop(Type, TorchGodShop23);
			SetupShops.Retinazer(npcTorchGodShop23, Shop1);
			npcTorchGodShop23.Register();

			var npcTorchGodShop24 = new NPCShop(Type, TorchGodShop24);
			SetupShops.Retinazer(npcTorchGodShop24, Shop2);
			npcTorchGodShop24.Register();

			var npcTorchGodShop25 = new NPCShop(Type, TorchGodShop25);
			SetupShops.SkeletronPrime(npcTorchGodShop25, Shop1);
			npcTorchGodShop25.Register();

			var npcTorchGodShop26 = new NPCShop(Type, TorchGodShop26);
			SetupShops.SkeletronPrime(npcTorchGodShop26, Shop2);
			npcTorchGodShop26.Register();

			var npcTorchGodShop27 = new NPCShop(Type, TorchGodShop27);
			SetupShops.Plantera(npcTorchGodShop27, Shop1);
			npcTorchGodShop27.Register();

			var npcTorchGodShop28 = new NPCShop(Type, TorchGodShop28);
			SetupShops.Plantera(npcTorchGodShop28, Shop2);
			npcTorchGodShop28.Register();

			var npcTorchGodShop29 = new NPCShop(Type, TorchGodShop29);
			SetupShops.Golem(npcTorchGodShop29, Shop1);
			npcTorchGodShop29.Register();

			var npcTorchGodShop30 = new NPCShop(Type, TorchGodShop30);
			SetupShops.Golem(npcTorchGodShop30, Shop2);
			npcTorchGodShop30.Register();

			var npcTorchGodShop31 = new NPCShop(Type, TorchGodShop31);
			SetupShops.EmpressOfLight(npcTorchGodShop31, Shop1);
			npcTorchGodShop31.Register();

			var npcTorchGodShop32 = new NPCShop(Type, TorchGodShop32);
			SetupShops.EmpressOfLight(npcTorchGodShop32, Shop2);
			npcTorchGodShop32.Register();

			var npcTorchGodShop33 = new NPCShop(Type, TorchGodShop33);
			SetupShops.DukeFishron(npcTorchGodShop33, Shop1);
			npcTorchGodShop33.Register();

			var npcTorchGodShop34 = new NPCShop(Type, TorchGodShop34);
			SetupShops.DukeFishron(npcTorchGodShop34, Shop2);
			npcTorchGodShop34.Register();

			var npcTorchGodShop35 = new NPCShop(Type, TorchGodShop35);
			SetupShops.Betsy(npcTorchGodShop35, Shop1);
			npcTorchGodShop35.Register();

			var npcTorchGodShop36 = new NPCShop(Type, TorchGodShop36);
			SetupShops.Betsy(npcTorchGodShop36, Shop2);
			npcTorchGodShop36.Register();

			var npcTorchGodShop37 = new NPCShop(Type, TorchGodShop37);
			SetupShops.LunaticCultist(npcTorchGodShop37, Shop1);
			npcTorchGodShop37.Register();

			var npcTorchGodShop38 = new NPCShop(Type, TorchGodShop38);
			SetupShops.LunaticCultist(npcTorchGodShop38, Shop2);
			npcTorchGodShop38.Register();

			var npcTorchGodShop39 = new NPCShop(Type, TorchGodShop39);
			SetupShops.MoonLord(npcTorchGodShop39, Shop1);
			npcTorchGodShop39.Register();

			var npcTorchGodShop40 = new NPCShop(Type, TorchGodShop40);
			SetupShops.MoonLord(npcTorchGodShop40, Shop2);
			npcTorchGodShop40.Register();

			var npcTorchGodShop41 = new NPCShop(Type, TorchGodShop41);
			SetupShops.Dreadnautilus(npcTorchGodShop41, Shop1);
			npcTorchGodShop41.Register();

			var npcTorchGodShop42 = new NPCShop(Type, TorchGodShop42);
			SetupShops.Dreadnautilus(npcTorchGodShop42, Shop2);
			npcTorchGodShop42.Register();

			var npcTorchGodShop43 = new NPCShop(Type, TorchGodShop43);
			SetupShops.Mothron(npcTorchGodShop43, Shop1);
			npcTorchGodShop43.Register();

			var npcTorchGodShop44 = new NPCShop(Type, TorchGodShop44);
			SetupShops.Mothron(npcTorchGodShop44, Shop2);
			npcTorchGodShop44.Register();

			var npcTorchGodShop45 = new NPCShop(Type, TorchGodShop45);
			SetupShops.Pumpking(npcTorchGodShop45, Shop1);
			npcTorchGodShop45.Register();

			var npcTorchGodShop46 = new NPCShop(Type, TorchGodShop46);
			SetupShops.Pumpking(npcTorchGodShop46, Shop2);
			npcTorchGodShop46.Register();

			var npcTorchGodShop47 = new NPCShop(Type, TorchGodShop47);
			SetupShops.IceQueen(npcTorchGodShop47, Shop1);
			npcTorchGodShop47.Register();

			var npcTorchGodShop48 = new NPCShop(Type, TorchGodShop48);
			SetupShops.IceQueen(npcTorchGodShop48, Shop2);
			npcTorchGodShop48.Register();

			var npcTorchGodShop49 = new NPCShop(Type, TorchGodShop49);
			SetupShops.MartianSaucer(npcTorchGodShop49, Shop1);
			npcTorchGodShop49.Register();

			var npcTorchGodShop50 = new NPCShop(Type, TorchGodShop50);
			SetupShops.MartianSaucer(npcTorchGodShop50, Shop2);
			npcTorchGodShop50.Register();
		}

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
			randExtraCooldown = 30;
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

		//Normally you'd want to choose a random name, but these Town NPCs have no name.
		//public string GetNameForVariant(NPC npc) => npc.getNewNPCName();
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
			{
				return ModContent.Request<Texture2D>(Path + "_Bestiary");
			}
			return ModContent.Request<Texture2D>(Path);
		}
		
		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot(Path + "_Head");
	}
}