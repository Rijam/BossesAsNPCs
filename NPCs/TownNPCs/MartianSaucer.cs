using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class MartianSaucer : ModNPC
	{
		//public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh";
		//public override string[] AltTextures => new[] { "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh_Alt_1" }; //Not implemented in 1.4 tML yet

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.MartianSaucer"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 5;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 1;
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
				//.SetBiomeAffection<SkyBiome>(AffectionLevel.Love)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Pumpking>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Betsy>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike)
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
			NPC.defense = 100;
			NPC.lifeMax = 2700;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtMartianSaucer>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("This advanced flying saucer has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Cyborg, Steampunker\nLike: Forest, Ice Queen, Pumpking, Betsy, Mechanic, Goblin Tinkerer\nDislike: Wizard, Tax Collector \nHate: None")
			});
		}
		/*public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return true;
		}*/

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/MartianSaucer_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/MartianSaucer_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/MartianSaucer_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedMartians && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMartianSaucer)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override string TownNPCName()
		{
			return "";
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new ();
			chat.Add("I am the Martian Saucer.");
			chat.Add("Do you believe in aliens? Well you should, you're looking right at one.");
			chat.Add("What? Didn't think a robot could be sentient?");
			chat.Add("I hope you have your tin-foil hat on.");
			chat.Add("Don't worry, I'm not going to probe you. That's that job for a Martian Scientist.");
			chat.Add("Martians are shorter than you Terrarians. As a result, your dwellings large enough for me in this form.");
			chat.Add("Maybe you Terrarians aren't so bad after all...");
			chat.Add("This sentence is false.\n...Don't answer that.");
			if (ModLoader.TryGetMod("RijamsMod", out Mod rijamsMod))
			{
				int intTrav = NPC.FindFirstNPC(rijamsMod.Find<ModNPC>("InterstellarTraveler").Type);
				if (intTrav >= 0)
				{
					chat.Add("I haven't seen a species like " + Main.npc[intTrav].GivenName + " before.");
				}
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
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("RunawayProbe").Type);
				shop.item[nextSlot].shopCustomPrice = 500000; //Match the Abominationn's shop
				nextSlot++;
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("MartianMemoryStick").Type);
				shop.item[nextSlot].shopCustomPrice = 300000; //Match the Abominationn's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.MartianConduitPlating);
			shop.item[nextSlot].shopCustomPrice = 100; //Made up value
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MartianCostumeMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MartianCostumeShirt);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MartianCostumePants);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MartianUniformHelmet);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MartianUniformTorso);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MartianUniformPants);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BrainScrambler);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.01);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LaserDrill);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7); //Special case to make it cheaper
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ChargedBlasterCannon);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.013 / 7); //Special case to make it cheaper
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.AntiGravityHook);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.013 / 7); //Special case to make it cheaper
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Xenopopper);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.XenoStaff);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LaserMachinegun);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ElectrosphereLauncher);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.InfluxWaver);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.CosmicCarKey);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.167);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.MartianSaucerTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.MartianPetItem); //Cosmic Skateboard
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.UFOMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (NPC.savedWizard)
			{
				shop.item[nextSlot].SetDefaults(ItemID.MusicBoxMartians);
				shop.item[nextSlot].shopCustomPrice = 20000 * 10;
				nextSlot++;
				if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false))
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWInvasion);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
				}
			}
			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.MartianSaucer.MSCostumeHeadpiece>());
			shop.item[nextSlot].shopCustomPrice = 50000;
			nextSlot++;
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
			projType = ProjectileID.ChargedBlasterOrb;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
			item = ItemID.None;
			scale = 1f;
			closeness = 0;
		}
	}
}