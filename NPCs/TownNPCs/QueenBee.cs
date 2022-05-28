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
	public class QueenBee : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.QueenBee"));
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
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Love)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Hate)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Plantera>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Golem>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Like)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<IceQueen>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike)
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
			NPC.lifeMax = 340;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtQueenBee>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("This highly aggressive monstrosity has decided to become your roommate."),
				new FlavorTextBestiaryInfoElement(
					NPCHelper.LoveText() + "Jungle, Dryad\n" +
					NPCHelper.LikeText() + "Caverns, Planetra, Golem, Queen Slime, Ice Queen, Witch Doctor, Zoologist\n" +
					NPCHelper.DislikeText() + "Graveyard, Demolitionist\n" +
					NPCHelper.HateText() + "Snow")
			});
		}

		public override void OnKill()
		{
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Wing").Type, 1f);
			for (int k = 0; k < 2; k++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedQueenBee && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnQueenBee)
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
            return new QueenBeeProfile();
        }

		public override string GetChat()
		{
			WeightedRandom<string> chat = new();
			chat.Add("I am the Queen Bee.");
			chat.Add("My larvae are very delicate, please be careful.");
			chat.Add("Don't worry, my wings are, in fact, large enough to allow me to fly.");
			chat.Add("Honey has great healing properties, just don't get stuck in it.");
			chat.Add("I hope you don't have melissophobia.");
			chat.Add("What? Expecting me to say \"Buzz Buzz\"?");
			chat.Add("What is the difference between a hornet, wasp, and yellow jacket? I don't know, I'm a bee.");
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add("I made some sweet pastries for today's party! They're probably the sweetest pastries you'll ever eat!", 2.0);
			}
			if (Main.notTheBeesWorld || WorldGen.notTheBees)
            {
				chat.Add("I feel very at home in this land.");
				chat.Add("Ah, jungle as far as my compound eyes can see!");
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
			shop.item[nextSlot].SetDefaults(ItemID.Abeemination);
			shop.item[nextSlot].shopCustomPrice = 125000; //Made up value since it has no value
			nextSlot++;
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
			{
				shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("Abeemination2").Type);
				shop.item[nextSlot].shopCustomPrice = 150000; //Match the Mutant's shop
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(ItemID.BeeGun);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000/ 0.33); //Formula: (Sell value / drop chance)
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeeKeeper);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeesKnees);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.HiveWand);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeeHat);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeeShirt);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeePants);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.11);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.HoneyComb);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.33);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Nectar);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.05); //Formula: (Sell value /drop chance)
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.HoneyedGoggles);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.05);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.Beenade);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40 / 0.75);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeeWax);
			shop.item[nextSlot].shopCustomPrice = 20 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BottledHoney);
			shop.item[nextSlot].shopCustomPrice = 8 * 5;
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.BeeMask);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
			nextSlot++;
			shop.item[nextSlot].SetDefaults(ItemID.QueenBeeTrophy);
			shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
			nextSlot++;
			if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.HiveBackpack);
				shop.item[nextSlot].shopCustomPrice = 20000 * 5;
				nextSlot++;
			}
			if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
            {
				shop.item[nextSlot].SetDefaults(ItemID.QueenBeePetItem);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.QueenBeeMasterTrophy);
				shop.item[nextSlot].shopCustomPrice = 10000 * 5;
				nextSlot++;
			}
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
            {
				if (NPC.savedWizard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss5);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
					}
				}
				shop.item[nextSlot].SetDefaults(ItemID.Hive);
				shop.item[nextSlot].shopCustomPrice = 100;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Stinger);
				shop.item[nextSlot].shopCustomPrice = 40 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Bezoar);
				shop.item[nextSlot].shopCustomPrice = 20000 * 5;
				nextSlot++;
				if (Main.LocalPlayer.ZoneGraveyard)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BeeHive);
					shop.item[nextSlot].shopCustomPrice = 50 * 5;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeBodypiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.QueenBee.QBCostumeLegpiece>());
				shop.item[nextSlot].shopCustomPrice = 50000;
				nextSlot++;
				if (NPC.downedMechBossAny)
				{
					shop.item[nextSlot].SetDefaults(ItemID.BeeWings);
					shop.item[nextSlot].shopCustomPrice = 80000 * 5;
					nextSlot++;
				}
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
			projType = ProjectileID.HornetStinger;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class QueenBeeProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}