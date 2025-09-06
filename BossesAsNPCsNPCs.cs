using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossesAsNPCs.NPCs.TownNPCs;
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
		public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
		{
			int shopPriceScaling = ModContent.GetInstance<BossesAsNPCsConfigServer>().ShopPriceScaling;
			float shopMulti = (shopPriceScaling / 100f);
			if (npc.type == ModContent.NPCType<KingSlime>() || npc.type == ModContent.NPCType<EyeOfCthulhu>() || npc.type == ModContent.NPCType<EaterOfWorlds>()
				|| npc.type == ModContent.NPCType<EaterOfWorlds>() || npc.type == ModContent.NPCType<BrainOfCthulhu>() || npc.type == ModContent.NPCType<QueenBee>()
				|| npc.type == ModContent.NPCType<Skeletron>() || npc.type == ModContent.NPCType<Deerclops>() || npc.type == ModContent.NPCType<WallOfFlesh>()
				|| npc.type == ModContent.NPCType<TheDestroyer>() || npc.type == ModContent.NPCType<Retinazer>() || npc.type == ModContent.NPCType<Spazmatism>()
				|| npc.type == ModContent.NPCType<SkeletronPrime>() || npc.type == ModContent.NPCType<Plantera>() || npc.type == ModContent.NPCType<Golem>()
				|| npc.type == ModContent.NPCType<EmpressOfLight>() || npc.type == ModContent.NPCType<DukeFishron>() || npc.type == ModContent.NPCType<Betsy>()
				|| npc.type == ModContent.NPCType<LunaticCultist>() || npc.type == ModContent.NPCType<MoonLord>() || npc.type == ModContent.NPCType<Dreadnautilus>()
				|| npc.type == ModContent.NPCType<Mothron>() || npc.type == ModContent.NPCType<Pumpking>() || npc.type == ModContent.NPCType<IceQueen>()
				|| npc.type == ModContent.NPCType<MartianSaucer>() || npc.type == ModContent.NPCType<TorchGod>())
			{
				foreach (Item item in items)
				{
					if (item is not null)
					{
						//item.GetStoreValue() is just `item.shopCustomPrice ?? item.value`
						item.shopCustomPrice = (int?)Math.Round(item.GetStoreValue() * shopMulti);
					}
				}
			}
			if (npc.type == NPCID.GoblinTinkerer)
			{
				foreach (Item item in items)
				{
					// Only change the price of the items that were added by this mod. Vanilla and other mods won't be affected (unless they add the same items).
					// (I tried to store the Item instead of the type and compare that, but it was never true.)
					if (item is not null && SetupShops.GoblinTinkererShopCopy.Contains(item.type))
					{
						item.shopCustomPrice = (int?)Math.Round(item.GetStoreValue() * shopMulti);
					}
				}
			}
			if (npc.type == NPCID.Pirate)
			{
				foreach (Item item in items)
				{
					if (item is not null && SetupShops.PirateShopCopy.Contains(item.type))
					{
						item.shopCustomPrice = (int?)Math.Round(item.GetStoreValue() * shopMulti);
					}
				}
			}
		}

		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.Pirate)
			{
				NPCs.SetupShops.Pirate(shop);
			}
			if (shop.NpcType == NPCID.GoblinTinkerer)
			{
				NPCs.SetupShops.GoblinTinkerer(shop);
			}

			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				if (torchSeller.TryFind<ModNPC>("TorchSellerNPC", out ModNPC torchMan) && shop.NpcType == torchMan.Type)
				{
					shop.Add(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeHeadpiece>(), Condition.NpcIsPresent(ModContent.NPCType<TorchGod>()));
					shop.Add(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeBodypiece>(), Condition.NpcIsPresent(ModContent.NPCType<TorchGod>()));
					shop.Add(ModContent.ItemType<Items.Vanity.TorchGod.TGCostumeLegpiece>(), Condition.NpcIsPresent(ModContent.NPCType<TorchGod>()));
				}
			}
		}

		public override void SetStaticDefaults()
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