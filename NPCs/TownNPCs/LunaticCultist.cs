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
	public class LunaticCultist : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.CultistBoss"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Clothier];
			NPCID.Sets.ExtraFramesCount[Type] = 7;
			NPCID.Sets.AttackFrameCount[Type] = 2;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 70;
			NPCID.Sets.HatOffsetY[Type] = 1;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new (0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Love)
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<MoonLord>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Skeletron>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Plantera>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Golem>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EmpressOfLight>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Like)
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<Dreadnautilus>(), AffectionLevel.Dislike)
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
			NPC.defense = 42;
			NPC.lifeMax = 3200;
			NPC.HitSound = SoundID.NPCHit55;
			NPC.DeathSound = SoundID.NPCDeath59;
			NPC.knockBackResist = 0.25f;
			AnimationType = NPCID.Clothier;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtLunaticCultist>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void OnKill()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, GoreID.CultistBoss1, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, GoreID.CultistBoss2, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, GoreID.Cultist2, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedAncientCultist && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnLunaticCultist)
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
            return new LunaticCultistProfile();
        }

		public override string GetChat()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 3; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (!NPC.downedMoonlord)
            {
				chat.Add(Language.GetTextValue(path + "PreML"));
			}
			if (NPC.downedMoonlord)
			{
				chat.Add(Language.GetTextValue(path + "PostML"));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			int demolitionist = NPC.FindFirstNPC(NPCID.Demolitionist);
			if (demolitionist >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Demolitionist", Main.npc[demolitionist].GivenName));
			}
			int plantera = NPC.FindFirstNPC(ModContent.NPCType<Plantera>());
			if (plantera >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Plantera"));
			}
			if (Main.time >= 0 && Main.time < 9000 && !Main.dayTime) //7:30 PM to 10:00 PM
			{
				chat.Add(Language.GetTextValue(path + "Dusk"));
			}
			if (ModLoader.TryGetMod("PboneUtils", out Mod pbonesUtilities) && townNPCsCrossModSupport)
			{
				int mysteriousTrader = NPC.FindFirstNPC(pbonesUtilities.Find<ModNPC>("MysteriousTrader").Type);
				if (mysteriousTrader >= 0)
				{
					chat.Add(Language.GetTextValue(path + "PbonesUtilities"));
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
				shop.item[nextSlot].SetDefaults(ItemID.LunarCraftingStation); //Ancient Manipulator
				shop.item[nextSlot].shopCustomPrice = 100000; //made up value
				nextSlot++;
				if (NPC.downedTowerSolar)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentSolar);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				if (NPC.downedTowerVortex)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentVortex);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				if (NPC.downedTowerNebula)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentNebula);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				if (NPC.downedTowerStardust)
				{
					shop.item[nextSlot].SetDefaults(ItemID.FragmentStardust);
					shop.item[nextSlot].shopCustomPrice = 2000 * 10;
					nextSlot++;
				}
				shop.item[nextSlot].SetDefaults(ItemID.BossMaskCultist);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.AncientCultistTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.LunaticCultistPetItem);
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.LunaticCultistMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss4);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic()) //Main.TOWMusicUnlocked
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss2);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.LunaticCultist.LCCostumeBodypiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "CultistSummon", shop, ref nextSlot, 750000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && townNPCsCrossModSupport)
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeLunaticCultist", shop, ref nextSlot, 10000);
					if (Main.bloodMoon)
					{
						NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeBloodMoon", shop, ref nextSlot, 10000);
					}
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "CelestialRune", shop, ref nextSlot);
						NPCHelper.SafelySetCrossModItem(fargosSouls, "MutantsPact", shop, ref nextSlot); //Mutant's Pact
					}
				}
				if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
				{
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistLazor", shop, ref nextSlot, 0.02f); //Mysterious Cultist Hood
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistBow", shop, ref nextSlot, 0.25f); //Lunatic Bow of Ice
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistSpear", shop, ref nextSlot, 0.25f); //Lunatic Spear of Fire
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistTome", shop, ref nextSlot, 0.25f); //Lunatic Spell of Ancient Light
					NPCHelper.SafelySetCrossModItem(stormsAdditions, "CultistStaff", shop, ref nextSlot, 0.25f); //Lunatic Staff of Lightning
					if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
					{
						NPCHelper.SafelySetCrossModItem(stormsAdditions, "LunaticHood", shop, ref nextSlot);  //Lunatic Hood of Command
					}
				}
				if (ModLoader.TryGetMod("EchoesoftheAncients", out Mod echoesOfTheAncients))
				{
					NPCHelper.SafelySetCrossModItem(echoesOfTheAncients, "LunarSilk", shop, ref nextSlot);
				}
			}
		}

		public override bool CanGoToStatue(bool toKingStatue)
		{
			return toKingStatue;
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
			projType = ProjectileID.NebulaBlaze2;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
	}
	public class LunaticCultistProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}