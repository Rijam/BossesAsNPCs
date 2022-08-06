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

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Mothron : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.Mothron"));
			Main.npcFrameCount[Type] = 25; //Main.npcFrameCount[NPCID.Clothier];
			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 70;
			NPCID.Sets.HatOffsetY[Type] = 0;

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
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Deerclops>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<QueenBee>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Betsy>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Dreadnautilus>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Like)
				.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Golfer, AffectionLevel.Dislike)
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
			NPC.lifeMax = 600;
			NPC.HitSound = SoundID.NPCHit44;
			NPC.DeathSound = SoundID.NPCDeath46;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Clothier;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtMothron>() : (short)-1;
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
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Wing1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Wing2").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
			}	
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (BossesAsNPCsWorld.downedMothron && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMothron)
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
			return new MothronProfile();
		}
		//SetNPCNameList is not needed for these Town NPCs because they don't have a name
		/*public override List<string> SetNPCNameList()
		{
			return new List<string>() { };
		}*/

		public override string GetChat()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 6; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			if (NPC.killCount[Item.NPCtoBanner(NPCID.Mothron)] > 0)
			{
				chat.Add(Language.GetTextValue(path + "KillCount", NPC.killCount[Item.NPCtoBanner(NPCID.Mothron)].ToString()), 0.5);
			}
			if (Main.LocalPlayer.ZoneJungle && Main.LocalPlayer.ZoneRockLayerHeight)
			{
				chat.Add(Language.GetTextValue(path + "UndergroundJungle"));
			}
			int eoc = NPC.FindFirstNPC(ModContent.NPCType<EyeOfCthulhu>());
			if (eoc >= 0)
			{
				chat.Add(Language.GetTextValue(path + "EoC"));
			}
			int deerclops = NPC.FindFirstNPC(ModContent.NPCType<Deerclops>());
			if (deerclops >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Deerclops"));
			}
			int witchDoctor = NPC.FindFirstNPC(NPCID.WitchDoctor);
			if (witchDoctor >= 0)
			{
				chat.Add(Language.GetTextValue(path + "WitchDoctor", Main.npc[witchDoctor].GivenName));
			}
			int merchant = NPC.FindFirstNPC(NPCID.Merchant);
			if (merchant >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Merchant", Main.npc[merchant].GivenName));
			}
			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && townNPCsCrossModSupport)
			{
				int torchMan = NPC.FindFirstNPC(torchSeller.Find<ModNPC>("TorchSellerNPC").Type);
				if (torchMan >= 0)
				{
					chat.Add(Language.GetTextValue(path + "TorchMerchant", Main.npc[torchMan].GivenName));
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
				shop.item[nextSlot].SetDefaults(ItemID.SolarTablet);
				shop.item[nextSlot].shopCustomPrice = 20000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.EyeSpring);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.0667);  //Formula: (Sell value /drop chance);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrokenBatWing);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoonStone);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.0286 / 4);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NeptunesShell);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.02 / 4);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Steak);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(6000 / 0.01 / 6);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DeathSickle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButchersChainsaw);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButcherMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButcherApron);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ButcherPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.02 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DeadlySphereStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ToxicFlask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(100000 / 0.025 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DrManFlyMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.0396 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.DrManFlyLabCoat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.0396 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NailGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.04);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Nail);
				shop.item[nextSlot].shopCustomPrice = 100; //Match the price of the Arm's Dealer
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PsychoKnife);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.025);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BrokenHeroSword);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(75000 / 0.25);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TheEyeOfCthulhu);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(125000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MothronWings);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(80000 / 0.05);
				nextSlot++;
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxEclipse);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBloodMoon);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Mothron.MoCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "MothronEgg", shop, ref nextSlot, 150000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "SolarVeil", shop, ref nextSlot);
					NPCHelper.SafelySetCrossModItem(calamityMod, "DefectiveSphere", shop, ref nextSlot, 0.2f);
				}
				if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
				{
					NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "SqueyereMinionItem", shop, ref nextSlot, 0.1f); //Crest of Eyes
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "Broken_Hero_GunParts", shop, ref nextSlot, 0.25f);
				}
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return !toKingStatue;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 5;
			knockback = 1f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<Projectiles.MothronEgg>();
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 3f;
			randomOffset = 1f;
		}
	}
	public class MothronProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;

		//Normally you'd want to choose a random name, but these Town NPCs have no name.
		//public string GetNameForVariant(NPC npc) => npc.getNewNPCName();
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}