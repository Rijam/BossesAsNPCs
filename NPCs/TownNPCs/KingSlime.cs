using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using System.Collections.Generic;
using Terraria.GameContent.Personalities;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class KingSlime : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.KingSlime"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.DyeTrader];
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
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<QueenSlime>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<Pumpking>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<DukeFishron>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Golfer, AffectionLevel.Like)
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
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.DyeTrader;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtKingSlime>() : -1;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement(NPCHelper.BestiaryPath(Name)),
				new FlavorTextBestiaryInfoElement(NPCHelper.LoveText(Name) + NPCHelper.LikeText(Name) + NPCHelper.DislikeText(Name) + NPCHelper.HateText(Name))
			});
		}

		public override void OnKill()
		{
			if (!Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Crown").Type, 1f);
			}
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);

			for (int k = 0; k < 2; k++)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Leg").Type, 1f);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedSlimeKing && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnKingSlime)
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
            return new KingSlimeProfile();
        }

		public override string GetChat()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 5; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			chat.Add(Language.GetTextValue(path + "Rare"), 0.1);
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			if (!NPC.downedQueenSlime)
			{
				chat.Add(Language.GetTextValue(path + "QS1"));
			}
			if (NPC.downedQueenSlime)
			{
				chat.Add(Language.GetTextValue(path + "QS2"));
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
				shop.item[nextSlot].SetDefaults(ItemID.SlimeCrown);
				shop.item[nextSlot].shopCustomPrice = 50000; //Made up value since Slime Crown has no value
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.Solidifier);
				shop.item[nextSlot].shopCustomPrice = 20000 * 2;
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SlimySaddle);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25); //Formula: (Sell value / drop chance)
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NinjaHood);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NinjaShirt);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.NinjaPants);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SlimeHook);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(4000 / 0.33);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.SlimeGun);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(3000 / 0.67);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KingSlimeMask);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(7500 / 0.14);
				nextSlot++;
				shop.item[nextSlot].SetDefaults(ItemID.KingSlimeTrophy);
				shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.1);
				nextSlot++;

				if (Main.expertMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.RoyalGel);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
				{
					shop.item[nextSlot].SetDefaults(ItemID.KingSlimePetItem); //Royal Delight
					shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25);
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.KingSlimeMasterTrophy);
					shop.item[nextSlot].shopCustomPrice = 10000 * 5;
					nextSlot++;
				}
				if (ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems)
				{
					if (NPC.savedWizard)
					{
						shop.item[nextSlot].SetDefaults(ItemID.MusicBoxBoss1);
						shop.item[nextSlot].shopCustomPrice = 20000 * 10;
						nextSlot++;
						if (WorldGen.drunkWorldGen || Main.drunkWorld || NPCHelper.UnlockOWMusic())
						{
							shop.item[nextSlot].SetDefaults(ItemID.MusicBoxOWBoss1);
							shop.item[nextSlot].shopCustomPrice = 20000 * 10;
							nextSlot++;
						}
					}
					shop.item[nextSlot].SetDefaults(ItemID.Gel);
					shop.item[nextSlot].shopCustomPrice = 1 * 10;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ItemID.SlimeStaff);
					shop.item[nextSlot].shopCustomPrice = 20000 * 10;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeHeadpiece>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					if (NPC.CountNPCS(NPCID.Princess) > 0)
					{
						shop.item[nextSlot].SetDefaults(ItemID.PrinceUniform);
						shop.item[nextSlot].shopCustomPrice = 500000;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.PrincePants);
						shop.item[nextSlot].shopCustomPrice = 500000;
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ItemID.PrinceCape);
						shop.item[nextSlot].shopCustomPrice = 500000;
						nextSlot++;
					}
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.KingSlime.KSCostumeGloves>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
					shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.KingSlime.KSAltCostumeGloves>());
					shop.item[nextSlot].shopCustomPrice = 50000;
					nextSlot++;
				}
			}
			if (NPCHelper.StatusShop2() && townNPCsCrossModSupport)
			{
				if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
				{
					NPCHelper.SafelySetCrossModItem(fargosMutant, "SlimyCrown", shop, ref nextSlot, 50000); //Match the Mutant's shop
				}
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
				{
					NPCHelper.SafelySetCrossModItem(calamityMod, "KnowledgeKingSlime", shop, ref nextSlot, 10000);
					NPCHelper.SafelySetCrossModItem(calamityMod, "CrownJewel", shop, ref nextSlot, 0.1f);
				}
				if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
				{
					NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimeKingsSlasher", shop, ref nextSlot, 0.1f);
					NPCHelper.SafelySetCrossModItem(fargosSouls, "MedallionoftheFallenKing", shop, ref nextSlot, 0.01f);

					bool eternityMode = (bool)fargosSouls.Call("EternityMode");
					if (eternityMode)
					{
						NPCHelper.SafelySetCrossModItem(fargosSouls, "SlimyShield", shop, ref nextSlot);
					}
				}
			}
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
			projType = ModContent.ProjectileType<Projectiles.SpikedSlimeProjectile>();
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 10f;
		}
	}
	public class KingSlimeProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				return ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

			if (npc.altTexture == 1)
				return ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Alt_1");

			return ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));
		}

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}