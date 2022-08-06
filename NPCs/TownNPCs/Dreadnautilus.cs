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
	public class Dreadnautilus : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.BloodNautilus"));
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
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Like)
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Hate)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<DukeFishron>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<WallOfFlesh>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Mothron>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Dislike)
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
			NPC.defense = 24;
			NPC.lifeMax = 700;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Clothier;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? (short)ModContent.ItemType<Items.CaughtDreadnautilus>() : (short)-1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
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
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Mouth").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (BossesAsNPCsWorld.downedDreadnautilus && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDreadnautilus)
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
			return new DreadnautilusProfile();
		}
		//SetNPCNameList is not needed for these Town NPCs because they don't have a name
		/*public override List<string> SetNPCNameList()
		{
			return new List<string>() { };
		}*/

		public override string GetChat()
		{
			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 4; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			chat.Add(Language.GetTextValue(path + "Rare"), 0.5);
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			if (NPC.killCount[Item.NPCtoBanner(NPCID.Mothron)] > 0)
			{
				chat.Add(Language.GetTextValue(path + "KillCount", NPC.killCount[Item.NPCtoBanner(NPCID.BloodNautilus)].ToString()), 0.5);
			}
			if (Main.dayTime && !Main.raining)
			{
				chat.Add(Language.GetTextValue(path + "DayNoRain"));
			}
			if (!Main.dayTime)
			{
				chat.Add(Language.GetTextValue(path + "Night"));
			}
			if (Main.raining)
			{
				chat.Add(Language.GetTextValue(path + "Rain"));
			}
			if (!Main.dayTime && Main.bloodMoon)
			{
				for (int i = 1; i <= 2; i++)
				{
					chat.Add(Language.GetTextValue(path + "BloodMoon" + i));
				}
				int zoologist = NPC.FindFirstNPC(NPCID.BestiaryGirl);
				if (zoologist >= 0)
				{
					chat.Add(Language.GetTextValue(path + "Zoologist", Main.npc[zoologist].GivenName));
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
				shop.item[nextSlot].SetDefaults(ItemID.BloodMoonStarter); //Bloody Tear
				shop.item[nextSlot].shopCustomPrice = 60000;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BunnyHood);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.0133);  //Formula: (Sell value /drop chance);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PedguinHat);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PedguinShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.PedguinPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.0067 / 3);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KiteBunnyCorrupt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.04);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KiteBunnyCrimson);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.04);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TopHat);
				shop.item[nextSlot].shopCustomPrice = 2000 * 5; //Technically a 90% drop chance, but in certain cases you could sell the hat for more than you bought it
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TheBrideHat);
				shop.item[nextSlot].shopCustomPrice = 1000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.TheBrideDress);
				shop.item[nextSlot].shopCustomPrice = 1000 * 5;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.MoneyTrough);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.005 / 2); //0.5% from Blood Zombies & Dripplers. Not using the 6.67% from Zombie Merman & Wandering Eye Fish
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SharkToothNecklace);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.0067 / 2);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.ChumBucket);
				shop.item[nextSlot].shopCustomPrice = 500 * 5 * 2;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BloodRainBow);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.VampireFrogStaff);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.125);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.BloodFishingRod); //Chum Caster
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.0417);
				nextSlot++;
				if (!NPC.combatBookWasUsed)
				{
					shop.item[nextSlot].SetDefaults(ItemID.CombatBook); //Advanced Combat Techniques
					shop.item[nextSlot].shopCustomPrice = 500000; //Normally no value
					nextSlot++;
				}
				if (Main.hardMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.KOCannon);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(35000 / 0.01 / 10); //Dropped by ANY enemy during a Blood Moon
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.Bananarang);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(15000 / 0.0333);
					nextSlot++;
					// No Trifold Map lol
					shop.item[nextSlot].SetDefaults(ItemID.BloodHamaxe); //Haemorrhaxe
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SharpTears); //Blood Thorn
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.DripplerFlail); //Drippler Crippler
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(40000 / 0.125);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SanguineStaff);
					shop.item[nextSlot].shopCustomPrice = 50000 * 5; //50% drop chance in normal mode, but I wanted it to be more expensive
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.BloodMoonMonolith);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1111);
				nextSlot++;
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxEerie);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBloodMoon);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.Dreadnautilus.DnCostumeLegpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SuspiciousLookingLure", shop, ref nextSlot, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodUrchin", shop, ref nextSlot, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "HemoclawCrab", shop, ref nextSlot, 100000); //Match the Deviantt's shop
					NPCHelper.SafelySetCrossModItem(fargosMutant, "BloodSushiPlatter", shop, ref nextSlot, 200000); //Match the Deviantt's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "BloodOrb", shop, ref nextSlot, 1f, 5f);
					NPCHelper.SafelySetCrossModItem(calamityMod, "BouncingEyeball", shop, ref nextSlot, (0.025f * 2f));
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "SqueakyToy", shop, ref nextSlot, 0.1f);
						NPCHelper.SafelySetCrossModItem(fargosSouls, "DreadShell", shop, ref nextSlot, 0.2f);
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodDrop", shop, ref nextSlot); //Bloody Drop
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "BloodyRifle", shop, ref nextSlot, 0.125f); //Bloodshot Rifle
				}
				if (ModLoader.TryGetMod("ItReallyMustBe", out Mod dreadnautilusIsABoss))
				{
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "FunnyBait", shop, ref nextSlot); //Blood Bait
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadPistol", shop, ref nextSlot, 0.25f);
					NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusTrophy", shop, ref nextSlot, 0.1f);

					if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
					{
						NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "BloodyCarKey", shop, ref nextSlot, 0.25f);
						NPCHelper.SafelySetCrossModItem(dreadnautilusIsABoss, "DreadnautilusRelic", shop, ref nextSlot, 0.1f);
					}
				}
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return true;
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 45;
			knockback = 6f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<Projectiles.FriendlyBloodShot>();
			attackDelay = 20;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class DreadnautilusProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;

		//Normally you'd want to choose a random name, but these Town NPCs have no name.
		//public string GetNameForVariant(NPC npc) => npc.getNewNPCName();
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}