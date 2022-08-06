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
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class BrainOfCthulhu : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.BrainofCthulhu"));
			Main.npcFrameCount[Type] = 25; // Main.npcFrameCount[NPCID.DyeTrader];
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
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Hate)
				.SetNPCAffection(ModContent.NPCType<EaterOfWorlds>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<WallOfFlesh>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MoonLord>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Dreadnautilus>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Like)
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EmpressOfLight>(), AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike)
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
			NPC.defense = 15;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath11;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtBrainOfCthulhu>() : -1;
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
			if ((NPC.downedBoss2 && (WorldGen.crimson || WorldGen.drunkWorldGen || Main.drunkWorld) || Main.hardMode) && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBoC)
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
			return new BrainOfCthulhuProfile();
		}

		public override string GetChat()
		{
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 5; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			if (WorldGen.tBlood >= 100)
			{
				chat.Add(Language.GetTextValue(path + "Blood"));
			}
			if (WorldGen.tGood > 0)
			{
				chat.Add(Language.GetTextValue(path + "Good"));
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Dryad", Main.npc[dryad].GivenName));
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0 && NPC.downedMechBossAny)
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
				shop.item[nextSlot].SetDefaults(ItemID.BloodySpine);
				shop.item[nextSlot].shopCustomPrice = 100000; //Made up value since it has no value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.CrimtaneOre);
				shop.item[nextSlot].shopCustomPrice = 1300 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TissueSample);
				shop.item[nextSlot].shopCustomPrice = 150 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BoneRattle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.05); //Formula: (Sell value / drop chance))
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrainMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrainofCthulhuTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BrainOfConfusion);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BrainOfCthulhuPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.BrainofCthulhuMasterTrophy);
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
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.ViciousPowder);
					shop.item[nextSlot].shopCustomPrice = 100;
					nextSlot++;
					if (Main.hardMode)
					{
						shop.item[nextSlot].SetDefaults(ItemID.BloodWater);
						shop.item[nextSlot].shopCustomPrice = 100;
						nextSlot++;
					}
					int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
					if (steampunker >= 0 && NPC.downedMechBossAny)
					{
						shop.item[nextSlot].SetDefaults(ItemID.RedSolution);
						shop.item[nextSlot].shopCustomPrice = 2500;
						nextSlot++;
					}
					if (ModLoader.TryGetMod("RijamsMod", out Mod rijamsMod) && townNPCsCrossModSupport) // It's my mod lol
					{
						NPCHelper.SafelySetCrossModItem(rijamsMod, "CrawlerChelicera", shop, ref nextSlot);
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.BrainOfCthulhu.BoCCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "GoreySpine", shop, ref nextSlot, 100000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeBrainofCthulhu", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeCrimson", shop, ref nextSlot, 10000);
				}

				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "BrainStaff", shop, ref nextSlot, 0.1f); //Mind Break
					NPCHelper.SafelySetCrossModItem(fargosSouls, "CrimetroidEgg", shop, ref nextSlot, 0.04f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "GuttedHeart", shop, ref nextSlot);
					}
				}
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return true; //Either king or queen
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
			projType = ProjectileID.GoldenShowerFriendly;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class BrainOfCthulhuProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}