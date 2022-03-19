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
	public class Skeletron : ModNPC
	{
		//public override string Texture => "BossesAsNPCs/NPCs/TownNPCs/QueenBee";
		//public override string[] AltTextures => new[] { "BossesAsNPCs/NPCs/TownNPCs/QueenBee_Alt_1" }; //Not implemented in 1.4 tML yet

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.SkeletronHead"));
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
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtSkeletron>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				new FlavorTextBestiaryInfoElement("This disembodied bones has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement("Love: Graveyard, Clothier\nLike: Forest, Lunatic Cultist, Skeletron Prime, Eye of Cthulhu, Brain of Cthulhu, Moon Lord, Merchant\nDislike: Angler\nHate: None")
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
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Skeletron_Gore_Head").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Skeletron_Gore_Jaw").Type, 1f);

				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Skeletron_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.position, NPC.velocity, ModContent.Find<ModGore>("BossesAsNPCs/Skeletron_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
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

		public override string TownNPCName()
		{
			return "";
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new ();
			chat.Add("I am Skeletron.");
			chat.Add("I'm the backbone of this world.");
			chat.Add("I am what is left of Cthulhu's skeleton.");
			chat.Add("The curse on the Dungeon has been lifted, though I think you'll find it quite dangerous still.");
			chat.Add("Sans what? I don't know what you are talking about.", 0.1);
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add("No hard feelings to " + Main.npc[dryad].GivenName + " for ripping me out of Cthulhu.");
			}
			int clothier = NPC.FindFirstNPC(NPCID.Clothier);
			if (clothier >= 0)
			{
				chat.Add(Main.npc[clothier].GivenName + " was such a great host! We should get together again some time!");
			}
			int skeletronPrime = NPC.FindFirstNPC(ModContent.NPCType<SkeletronPrime>());
			if (skeletronPrime >= 0)
			{
				chat.Add("If you thought I couldn't get any more awesome, check out Skeletron Prime!");
			}
			if (BossesAsNPCsWorld.downedDungeonGuardian)
            {
				chat.Add("Impressive, you managed to defeat my Guardian form. Here, have this key. I'm not sure what it does, so you figure it out.");
			}
			if (Main.LocalPlayer.HasItem(ItemID.SDMG))
			{
				chat.Add("Omegatron is such an awesome guy.");
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
			shop.item[nextSlot].SetDefaults(ItemID.ClothierVoodooDoll);
			shop.item[nextSlot].shopCustomPrice = 130000; //Made up value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("SuspiciousSkull").Type);
				shop.item[nextSlot].shopCustomPrice = 150000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.SkeletronHand);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(9000 / 0.12); //Formula: (Sell value / drop chance)
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BookofSkulls);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.ChippysCouch);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SkeletronMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.SkeletronTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.BoneGlove);
				shop.item[nextSlot].shopCustomPrice = 20000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.SkeletronPetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SkeletronMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (NPC.savedWizard)
			{
				shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
				shop.item[nextSlot].shopCustomPrice = 20000 * 10;
				nextSlot++;
				if (WorldGen.drunkWorldGen || Main.drunkWorld || Main.Configuration.Get("UnlockMusicSwap", false))
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
				}
			}
			if (BossesAsNPCsWorld.downedDungeonGuardian)
            {
				shop.item[nextSlot].SetDefaults(ItemID.BoneKey);
				shop.item[nextSlot].shopCustomPrice = 50000 * 5;
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.BoneWand);
			shop.item[nextSlot].shopCustomPrice = 50 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Bone);
			shop.item[nextSlot].shopCustomPrice = 10 * 5;
			nextSlot++;
			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC))
			{
				int fisherman = NPC.FindFirstNPC(fishermanNPC.Find<ModNPC>("Fisherman").Type);
				if (fisherman >= 0)
				{
					shop.item[nextSlot].SetDefaults(ItemID.LockBox);
					shop.item[nextSlot].shopCustomPrice = 4000 * 5;
					nextSlot++;
				}
			}
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