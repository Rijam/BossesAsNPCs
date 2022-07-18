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
	public class WallOfFlesh : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.WallofFlesh"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 5;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 0;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				//.SetBiomeAffection<UnderworldBiome>(AffectionLevel.Love)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
				//.SetBiomeAffection<SkyBiome>(AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EaterOfWorlds>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Dreadnautilus>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Dislike)
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
			NPC.lifeMax = 800;
			NPC.HitSound = SoundID.NPCHit8;
			NPC.DeathSound = SoundID.NPCDeath10;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtWallOfFlesh>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void OnKill()
		{
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);

			for (int k = 0; k < 2; k++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity * (k + 1), ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Flesh").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (Main.hardMode && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnWoF)
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
            return new WallOfFleshProfile();
        }

		public override string GetChat()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 6; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (!Main.player[Main.myPlayer].ZoneUnderworldHeight)
            {
				chat.Add(Language.GetTextValue(path + "NotUnderworld"));
			}
			if (Main.expertMode)
            {
				chat.Add(Language.GetTextValue(path + "Expert"));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			int guide = NPC.FindFirstNPC(NPCID.Guide);
			if (guide >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Guide1", Main.npc[guide].GivenName));
				chat.Add(Language.GetTextValue(path + "Guide2", Main.npc[guide].GivenName));
			}
			if (ModLoader.TryGetMod("CalamityMod", out Mod _) && townNPCsCrossModSupport)
			{
				chat.Add(Language.GetTextValue(path + "Calamity1"));
				chat.Add(Language.GetTextValue(path + "Calamity2"));
			}
			else
            {
				if (!WorldGen.crimson)
                {
					chat.Add(Language.GetTextValue(path + "PwnCorruption"));
				}
				if (WorldGen.crimson)
				{
					chat.Add(Language.GetTextValue(path + "PwnCrimson"));
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
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			if (NPCHelper.StatusShop1())
			{
				shop.item[nextSlot].SetDefaults(ItemID.GuideVoodooDoll);
				shop.item[nextSlot].shopCustomPrice = 150000; //Made up value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Pwnhammer);
				shop.item[nextSlot].shopCustomPrice = 7800 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BreakerBlade);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13); //Formula: (Sell value / drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ClockworkAssaultRifle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.LaserRifle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.FireWhip);//Firecracker
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WarriorEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.RangerEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SorcererEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SummonerEmblem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.13);
				nextSlot++;
				int eoc = NPC.FindFirstNPC(ModContent.NPCType<EyeOfCthulhu>());
				if (eoc >= 0)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BadgersHat);
					shop.item[nextSlot].shopCustomPrice = 3000 * 20;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.FleshMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.WallofFleshTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;
				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.DemonHeart);
					shop.item[nextSlot].shopCustomPrice = 20000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.WallOfFleshGoatMountItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.WallofFleshMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss2);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWWallOfFlesh);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.DemoniteBrick);
					shop.item[nextSlot].shopCustomPrice = 1500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.CrimtaneBrick);
					shop.item[nextSlot].shopCustomPrice = 1500;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.WallOfFlesh.WoFCostumeBackpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "FleshyDoll", shop, ref nextSlot, 200000);
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeWallofFlesh", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeUnderworld", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Carnage", shop, ref nextSlot, 0.1f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlackHawkRemote", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BlastBarrel", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "Meowthrower", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "RogueEmblem", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "HermitsBoxofOneHundredMedicines", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "FleshHand", shop, ref nextSlot, 0.1f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "PungentEyeball", shop, ref nextSlot);
					}
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "BoneSerpentMinionItem", shop, ref nextSlot, 0.35f);
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
			projType = ModContent.ProjectileType<Projectiles.Leech>();
			attackDelay = 20;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class WallOfFleshProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}