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
using Microsoft.Xna.Framework;

namespace BossesAsNPCs.NPCs.TownNPCs
{
	[AutoloadHead]
	public class MoonLord : ModNPC
	{
		public override bool IsLoadingEnabled(Mod mod) => NPCHelper.ShouldLoad(Name);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("NPCName.MoonLordHead"));
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Clothier];
			NPCID.Sets.ExtraFramesCount[Type] = 7;
			NPCID.Sets.AttackFrameCount[Type] = 2;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 2;
			NPCID.Sets.AttackTime[Type] = 30;
			NPCID.Sets.AttackAverageChance[Type] = 70;
			NPCID.Sets.HatOffsetY[Type] = 1;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Love)
				//.SetBiomeAffection<Sky>(AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<LunaticCultist>(), AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<EyeOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<BrainOfCthulhu>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Skeletron>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<TheDestroyer>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Retinazer>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<Spazmatism>(), AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<SkeletronPrime>(), AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(ModContent.NPCType<EmpressOfLight>(), AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike)
				.SetNPCAffection(ModContent.NPCType<TorchGod>(), AffectionLevel.Hate)
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
			NPC.defense = 70;
			NPC.lifeMax = 14500;
			NPC.HitSound = SoundID.NPCHit57;
			NPC.DeathSound = SoundID.NPCDeath62;
			NPC.knockBackResist = 0.25f;
			AnimationType = NPCID.Guide;
			Main.npcCatchable[NPC.type] = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
			NPC.catchItem = ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs ? ModContent.ItemType<Items.CaughtMoonLord>() : -1;
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

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Head").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Torso").Type, 1f);
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore_Arm").Type, 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			if (NPC.downedMoonlord && ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMoonLord)
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
            return new MoonLordProfile();
        }

		//Note about the glow mask, the sitting frame needs to be 2 visible pixels higher.
		private readonly Asset<Texture2D> glowmask = ModContent.Request<Texture2D>("BossesAsNPCs/NPCs/TownNPCs/GlowMasks/MoonLord_Glow");
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects spriteEffects = NPC.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color color = Color.White;

			spriteBatch.Draw(glowmask.Value, NPC.Center - screenPos - new Vector2(0, 4), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects, 1f);
		}

		public override string GetChat()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			string path = NPCHelper.DialogPath(Name);
			WeightedRandom<string> chat = new ();
			for (int i = 1; i <= 7; i++)
			{
				chat.Add(Language.GetTextValue(path + "Default" + i));
			}
			if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
			{
				chat.Add(Language.GetTextValue(path + "Party"), 2.0);
			}
			int dryad = NPC.FindFirstNPC(NPCID.Dryad);
			if (dryad >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Dryad", Main.npc[dryad].GivenName));
			}
			int lunaticCultist = NPC.FindFirstNPC(ModContent.NPCType<LunaticCultist>());
			if (lunaticCultist >= 0)
			{
				chat.Add(Language.GetTextValue(path + "LunaticCultist"));
			}
			int torchGod = NPC.FindFirstNPC(ModContent.NPCType<TorchGod>());
			if (torchGod >= 0)
			{
				chat.Add(Language.GetTextValue(path + "TorchGod"));
				int moonLord = NPC.FindFirstNPC(ModContent.NPCType<MoonLord>());
				NPCHelper.GetNearbyResidentNPCs(Main.npc[moonLord], 3, out List<int> npcTypeListHouse, out List<int> _, out List<int> _, out List<int> _);
				if (npcTypeListHouse.Contains(ModContent.NPCType<TorchGod>()))
				{
					chat.Add(Language.GetTextValue(path + "TorchGodVeryClose"), 10);
				}
			}
			else
			{
				chat.Add(Language.GetTextValue(path + "NoTorchGod"));
			}
			int mechanic = NPC.FindFirstNPC(NPCID.Mechanic);
			if (mechanic >= 0)
			{
				chat.Add(Language.GetTextValue(path + "Mechanic", Main.npc[mechanic].GivenName));
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
			SetupShops.MoonLord(shop, ref nextSlot);
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
			projType = ProjectileID.MoonlordArrowTrail;
			attackDelay = 1;
		}
		
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 16f;
		}
	}
	public class MoonLordProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => null;

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) => ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/'));

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot((GetType().Namespace + "." + GetType().Name.Split("Profile")[0]).Replace('.', '/') + "_Head");
	}
}