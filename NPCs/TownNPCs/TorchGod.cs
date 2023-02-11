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

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class TorchGod : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode > 0 || NPCHelper.bypassMode;
		}

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
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
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

		public override void HitEffect(int hitDirection, double damage)
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
			if (NPCHelper.DownedAnyBossWithConfigCheck() && ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode == 1)
			{
				return true;
			}
			if (NPCHelper.DownedAnyBoss() && ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode == 2)
			{
				return true;
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return new TorchGodProfile();
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

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (NPCHelper.StatusShopCycle() <= 0 || NPCHelper.StatusShopCycle() >= 51)
				{
					Main.npcChatText = Language.GetTextValue(NPCHelper.DialogPath(Name) + "Common");
					shop = false;
				}
				else
				{
					shop = true;
				}
				NPCHelper.StatusShopCycle();
			}
			if (!firstButton)
			{
				int mode = ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode;
				shop = false;
				GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
				if (mode == 0)
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
				else if (mode == 1)
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
				else if (mode == 2)
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

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			switch (NPCHelper.StatusShopCycle())
			{
				case 1:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.KingSlime(shop, ref nextSlot);
					break;
				case 2:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.KingSlime(shop, ref nextSlot);
					break;
				case 3:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.EyeOfCthulhu(shop, ref nextSlot);
					break;
				case 4:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.EyeOfCthulhu(shop, ref nextSlot);
					break;
				case 5:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.EaterOfWorlds(shop, ref nextSlot);
					break;
				case 6:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.EaterOfWorlds(shop, ref nextSlot);
					break;
				case 7:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.BrainOfCthulhu(shop, ref nextSlot);
					break;
				case 8:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.BrainOfCthulhu(shop, ref nextSlot);
					break;
				case 9:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.QueenBee(shop, ref nextSlot);
					break;
				case 10:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.QueenBee(shop, ref nextSlot);
					break;
				case 11:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Skeletron(shop, ref nextSlot);
					break;
				case 12:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Skeletron(shop, ref nextSlot);
					break;
				case 13:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Deerclops(shop, ref nextSlot);
					break;
				case 14:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Deerclops(shop, ref nextSlot);
					break;
				case 15:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.WallOfFlesh(shop, ref nextSlot);
					break;
				case 16:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.WallOfFlesh(shop, ref nextSlot);
					break;
				case 17:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.QueenSlime(shop, ref nextSlot);
					break;
				case 18:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.QueenSlime(shop, ref nextSlot);
					break;
				case 19:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.TheDestroyer(shop, ref nextSlot);
					break;
				case 20:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.TheDestroyer(shop, ref nextSlot);
					break;
				case 21:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Retinazer(shop, ref nextSlot);
					break;
				case 22:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Retinazer(shop, ref nextSlot);
					break;
				case 23:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Spazmatism(shop, ref nextSlot);
					break;
				case 24:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Spazmatism(shop, ref nextSlot);
					break;
				case 25:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.SkeletronPrime(shop, ref nextSlot);
					break;
				case 26:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.SkeletronPrime(shop, ref nextSlot);
					break;
				case 27:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Plantera(shop, ref nextSlot);
					break;
				case 28:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Plantera(shop, ref nextSlot);
					break;
				case 29:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Golem(shop, ref nextSlot);
					break;
				case 30:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Golem(shop, ref nextSlot);
					break;
				case 31:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.EmpressOfLight(shop, ref nextSlot);
					break;
				case 32:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.EmpressOfLight(shop, ref nextSlot);
					break;
				case 33:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.DukeFishron(shop, ref nextSlot);
					break;
				case 34:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.DukeFishron(shop, ref nextSlot);
					break;
				case 35:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Betsy(shop, ref nextSlot);
					break;
				case 36:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Betsy(shop, ref nextSlot);
					break;
				case 37:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.LunaticCultist(shop, ref nextSlot);
					break;
				case 38:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.LunaticCultist(shop, ref nextSlot);
					break;
				case 39:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.MoonLord(shop, ref nextSlot);
					break;
				case 40:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.MoonLord(shop, ref nextSlot);
					break;
				case 41:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Dreadnautilus(shop, ref nextSlot);
					break;
				case 42:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Dreadnautilus(shop, ref nextSlot);
					break;
				case 43:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Mothron(shop, ref nextSlot);
					break;
				case 44:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Mothron(shop, ref nextSlot);
					break;
				case 45:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.Pumpking(shop, ref nextSlot);
					break;
				case 46:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.Pumpking(shop, ref nextSlot);
					break;
				case 47:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.IceQueen(shop, ref nextSlot);
					break;
				case 48:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.IceQueen(shop, ref nextSlot);
					break;
				case 49:
					NPCHelper.SetShop1(true); NPCHelper.SetShop2(false);
					SetupShops.MartianSaucer(shop, ref nextSlot);
					break;
				case 50:
					NPCHelper.SetShop1(false); NPCHelper.SetShop2(true);
					SetupShops.MartianSaucer(shop, ref nextSlot);
					break;
				default:
					break;
			}
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