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
	public class Plantera : ModNPC
	{
		//public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh";
		//public override string[] AltTextures => new[] { "BossesAsNPCs/NPCs/TownNPCs/WallOfFlesh_Alt_1" }; //Not implemented in 1.4 tML yet

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.Plantera"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
			NPCID.Sets.ExtraFramesCount[Type] = 10;
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
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Love)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Golem>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EmpressOfLight>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<LunaticCultist>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Like)
				.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Dislike)
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
			NPC.defense = 30;
			NPC.lifeMax = 3000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtPlantera>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("This floral guardian has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Jungle, Witch Doctor, Dryad\nLike: Caverns, Golem, Queen Bee, Empress of Light, Lunatic Cultist, Steampunker\nDislike: Graveyard, Dye Trader\nHate: Snow")
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
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Plantera_Gore_Head").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Plantera_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Plantera_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedPlantBoss && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnPlantera)
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
			chat.Add("I am Plantera.");
			chat.Add("Why don't you stop and smell the roses?");
			chat.Add("I'm very protective of my bulbs, please don't disturb them.");
			chat.Add("Sorry if I seem snappy... get it?");
			chat.Add("I grow restless... for you to buy something!");
			int eol = NPC.FindFirstNPC(ModContent.NPCType<EmpressOfLight>());
			int golem = NPC.FindFirstNPC(ModContent.NPCType<Golem>());
			int cultist = NPC.FindFirstNPC(ModContent.NPCType<LunaticCultist>());
			if (eol <= -1 && golem <= -1 && cultist <= -1 && !NPC.downedMoonlord)
            {
				chat.Add("I was thinking of starting a band...");
			}
			if (eol >= 0 && golem >= 0 && cultist <= -1 && !NPC.downedMoonlord)
			{
				chat.Add("I'm starting a metal band with the Empress of Light! We are trying to get Golem to be our drummer.");
			}
			if (eol >= 0 && golem >= 0 && cultist >= 0 && !NPC.downedMoonlord)
			{
				chat.Add("The Empress of Light, Golem, Lunatic Cultist, and I are recording our first album! Lunatic Cultist's mysterious voice is perfect for our songs.");
			}
			if (eol >= 0 && golem >= 0 && cultist >= 0 && NPC.downedMoonlord)
			{
				chat.Add("The Empress of Light, Golem, Lunatic Cultist, and I finished out first album! You should check it out!");
			}
			if (Main.LocalPlayer.HasBuff(BuffID.Plantero))
			{
				chat.Add("Look at that little Plantero go!");
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
			/*shop.item[nextSlot].SetDefaults(ItemID.PlanteraBulb);
			shop.item[nextSlot].shopCustomPrice = 300000; //Made up value
			nextSlot++;*/
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("PlanterasFruit").Type);
				shop.item[nextSlot].shopCustomPrice = 500000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.TempleKey);
			shop.item[nextSlot].shopCustomPrice = 5000; //Made up value
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.GrenadeLauncher);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.VenusMagnum);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.NettleBurst);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.LeafBlower);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.FlowerPow);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.WaspGun);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Seedler);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PygmyStaff);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(70000 / 0.25);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ThornHook);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.1);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.TheAxe);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.02);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Seedling);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PlanteraMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.PlanteraTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.SporeSac);
				shop.item[nextSlot].shopCustomPrice = 40000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.PlanteraPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PlanteraMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
            {
				if (NPC.savedWizard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxPlantera);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false))
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWPlantera);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.JungleGrassSeeds);
				shop.item[nextSlot].shopCustomPrice = 30 * 5;
				nextSlot++;
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return !toKingStatue;
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
			projType = ModContent.ProjectileType<Projectiles.DoubleEighthNote>();
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
		public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
		{
			item = ModContent.ItemType<Items.PlanterasAxe>();
			scale = 1f;
			closeness = 40;
		}
	}
}