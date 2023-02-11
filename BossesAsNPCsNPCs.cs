using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossesAsNPCs.NPCs.TownNPCs;
using Terraria.Localization;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using BossesAsNPCs.NPCs;

namespace BossesAsNPCs
{
	public class BossesAsNPCsNPCs : GlobalNPC
	{
		public override void OnKill(NPC npc)
		{
			if (npc.type == NPCID.DD2Betsy)
			{
				BossesAsNPCsWorld.downedBetsy = true;
				if (Main.netMode == NetmodeID.Server)
				{
					BossesAsNPCsWorld.downedBetsy = true;
					NetMessage.SendData(MessageID.WorldData);
				}
				if (Main.netMode == NetmodeID.Server && !BossesAsNPCsWorld.downedBetsy) //Try again if it didn't work the first time
				{
					if (Terraria.GameContent.Events.DD2Event.DownedInvasionT3)
					{
						BossesAsNPCsWorld.downedBetsy = true;
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
			if (npc.type == NPCID.DungeonGuardian)
			{
				BossesAsNPCsWorld.downedDungeonGuardian = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
			if (npc.type == NPCID.DD2DarkMageT1 || npc.type == NPCID.DD2DarkMageT3)
			{
				BossesAsNPCsWorld.downedDarkMage = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
			if (npc.type == NPCID.DD2OgreT2 || npc.type == NPCID.DD2OgreT3)
			{
				BossesAsNPCsWorld.downedOgre = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
			if (npc.type == NPCID.GoblinSummoner)
			{
				BossesAsNPCsWorld.downedGoblinSummoner = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
			if (npc.type == NPCID.Mothron)
			{
				BossesAsNPCsWorld.downedMothron = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
			if (npc.type == NPCID.BloodNautilus)
			{
				BossesAsNPCsWorld.downedDreadnautilus = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
			if (npc.type == NPCID.HallowBoss)
			{
				if (npc.AI_120_HallowBoss_IsGenuinelyEnraged())
				{
					BossesAsNPCsWorld.daytimeEoLDefeated = true;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
			if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
			{
				if (npc.boss)
				{
					BossesAsNPCsWorld.downedEoW = true;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
				if (NPC.CountNPCS(NPCID.EaterofWorldsHead) <= 1)
				{
					BossesAsNPCsWorld.downedEoW = true;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
				if (NPC.downedBoss2 && !BossesAsNPCsWorld.downedEoW) //Guaranteed to work on the second kill
				{
					BossesAsNPCsWorld.downedEoW = true;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
			if (npc.type == NPCID.BrainofCthulhu)
			{
				BossesAsNPCsWorld.downedBoC = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
			if (npc.type == NPCID.WallofFlesh)
			{
				BossesAsNPCsWorld.downedWoF = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
				if (Main.netMode == NetmodeID.Server && !BossesAsNPCsWorld.downedWoF) //Try again if it didn't work the first time
				{
					if (Main.hardMode)
					{
						BossesAsNPCsWorld.downedWoF = true;
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			int shopPriceScaling = ModContent.GetInstance<BossesAsNPCsConfigServer>().ShopPriceScaling;
			float shopMulti = (shopPriceScaling / 100f);
			if (type == ModContent.NPCType<KingSlime>() || type == ModContent.NPCType<EyeOfCthulhu>() || type == ModContent.NPCType<EaterOfWorlds>()
				|| type == ModContent.NPCType<EaterOfWorlds>() || type == ModContent.NPCType<BrainOfCthulhu>() || type == ModContent.NPCType<QueenBee>()
				|| type == ModContent.NPCType<Skeletron>() || type == ModContent.NPCType<Deerclops>() || type == ModContent.NPCType<WallOfFlesh>()
				|| type == ModContent.NPCType<TheDestroyer>() || type == ModContent.NPCType<Retinazer>() || type == ModContent.NPCType<Spazmatism>()
				|| type == ModContent.NPCType<SkeletronPrime>() || type == ModContent.NPCType<Plantera>() || type == ModContent.NPCType<Golem>()
				|| type == ModContent.NPCType<EmpressOfLight>() || type == ModContent.NPCType<DukeFishron>() || type == ModContent.NPCType<Betsy>()
				|| type == ModContent.NPCType<LunaticCultist>() || type == ModContent.NPCType<MoonLord>() || type == ModContent.NPCType<Dreadnautilus>()
				|| type == ModContent.NPCType<Mothron>() || type == ModContent.NPCType<Pumpking>() || type == ModContent.NPCType<IceQueen>()
				|| type == ModContent.NPCType<MartianSaucer>() || type == ModContent.NPCType<TorchGod>())
			{
				foreach (Item item in shop.item)
				{
					int shopPrice = item.shopCustomPrice ?? 0; //Some hackery with changing the int? type into int
					item.shopCustomPrice = (int?)Math.Round(shopPrice * shopMulti);
				}
			}
			if (type == NPCID.Pirate && ModContent.GetInstance<BossesAsNPCsConfigServer>().PirateSellInvasionItems)
			{
				NPCs.SetupShops.Pirate(shop, ref nextSlot, shopMulti);
			}
			if (type == NPCID.GoblinTinkerer && ModContent.GetInstance<BossesAsNPCsConfigServer>().GoblinSellInvasionItems)
			{
				NPCs.SetupShops.GoblinTinkerer(shop, ref nextSlot, shopMulti);
			}

			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				if (torchSeller.TryFind<ModNPC>("TorchSellerNPC", out ModNPC torchMan) && type == torchMan.Type)
				{
					if (NPC.CountNPCS(ModContent.NPCType<TorchGod>()) > 0)
					{
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeHeadpiece>());
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeBodypiece>());
						nextSlot++;
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeLegpiece>());
						nextSlot++;
					}
				}
			}
		}
		public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<KingSlime>()] = 2;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<EyeOfCthulhu>()] = 2;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<EaterOfWorlds>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<BrainOfCthulhu>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<QueenBee>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Skeletron>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Deerclops>()] = 3;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<WallOfFlesh>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<QueenSlime>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<TheDestroyer>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Retinazer>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Spazmatism>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<SkeletronPrime>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Plantera>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Golem>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<QueenSlime>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<EmpressOfLight>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<DukeFishron>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Betsy>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<LunaticCultist>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<MoonLord>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Dreadnautilus>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Mothron>()] = 5;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<Pumpking>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<IceQueen>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<MartianSaucer>()] = 4;
			ContentSamples.NpcBestiaryRarityStars[ModContent.NPCType<TorchGod>()] = 5;
		}
	}
}