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
                || type == ModContent.NPCType<MartianSaucer>())
            {
                foreach (Item item in shop.item)
                {
                    int shopPrice = item.shopCustomPrice ?? 0; //Some hackery with changing the int? type into int
                    item.shopCustomPrice = (int?)Math.Round(shopPrice * shopMulti);
                }
            }
            if (type == NPCID.Pirate && ModContent.GetInstance<BossesAsNPCsConfigServer>().PirateSellInvasionItems)
            {
                shop.item[nextSlot].SetDefaults(ItemID.PirateMap);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 * shopMulti); //Made up value
                nextSlot++;
                if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant) && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
                {
                    shop.item[nextSlot].SetDefaults(fargosMutant.Find<ModItem>("PirateFlag").Type);
                    shop.item[nextSlot].shopCustomPrice = (int)Math.Round(150000 * shopMulti); //Match the Deviantt's shop
                    nextSlot++;
                }
                shop.item[nextSlot].SetDefaults(ItemID.CoinGun);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(60000 / 0.0025 * shopMulti);  //Formula: (Sell value / drop chance)
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.LuckyCoin);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.005 * shopMulti);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.DiscountCard);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.01 * shopMulti);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.PirateStaff);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.01 * shopMulti);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.GoldRing);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 / 0.02 * shopMulti);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.PirateMinecart);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5000 / 0.05 * shopMulti);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.Cutlass);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(36000 / 0.1 * shopMulti);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.FlyingDutchmanTrophy);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 * 0.1 * shopMulti);
                nextSlot++;
                if (Main.masterMode || ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.PirateShipMountItem); //Black Spot
                    shop.item[nextSlot].shopCustomPrice = (int)Math.Round(50000 / 0.25 * shopMulti);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.FlyingDutchmanMasterTrophy);
                    shop.item[nextSlot].shopCustomPrice = (int)Math.Round(10000 * 5 * shopMulti);
                    nextSlot++;
                }
                if (ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
                {
                    if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant2) && ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
                    {
                        bool eternityMode = (bool)fargosSouls.Call("EternityMode");
                        if (eternityMode)
                        {
                            NPCHelper.SafelySetCrossModItem(fargosMutant2, "GoldenDippingVat", shop, ref nextSlot, (0.07f * 5), shopMulti);
                        }
                    }
                }
            }
            if (type == NPCID.GoblinTinkerer && ModContent.GetInstance<BossesAsNPCsConfigServer>().GoblinSellInvasionItems)
            {
                shop.item[nextSlot].SetDefaults(ItemID.GoblinBattleStandard);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(25000 * shopMulti); //Made up value
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.Harpoon);
                shop.item[nextSlot].shopCustomPrice = (int)Math.Round(5400 / 0.005 / 5 * shopMulti);  //Special case to make it cheaper
                nextSlot++;
                if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod) && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
                {
                    NPCHelper.SafelySetCrossModItem(calamityMod, "PlasmaRod", shop, ref nextSlot, (0.07f * 5), shopMulti);
                }
                if (BossesAsNPCsWorld.downedGoblinSummoner)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.ShadowFlameHexDoll);
                    shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.ShadowFlameBow);
                    shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti);
                    nextSlot++;
                    shop.item[nextSlot].SetDefaults(ItemID.ShadowFlameKnife);
                    shop.item[nextSlot].shopCustomPrice = (int)Math.Round(20000 / 0.17 * shopMulti);
                    nextSlot++;

                    if (ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
                    {
                        if (ModLoader.TryGetMod("Fargowiltas", out Mod fargosMutant))
                        {
                            NPCHelper.SafelySetCrossModItem(fargosMutant, "ShadowflameIcon", shop, ref nextSlot, 0.01f, shopMulti); //10 gold
                        }
                        if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod2))
                        {
                            NPCHelper.SafelySetCrossModItem(calamityMod2, "BurningStrife", shop, ref nextSlot, (0.33f * 5), shopMulti);
                            NPCHelper.SafelySetCrossModItem(calamityMod2, "TheFirstShadowflame", shop, ref nextSlot, (0.33f * 5), shopMulti);
                        }
                        if (ModLoader.TryGetMod("FargowiltasSouls", out Mod fargosSouls))
                        {
                            bool eternityMode = (bool)fargosSouls.Call("EternityMode");
                            if (eternityMode)
                            {
                                NPCHelper.SafelySetCrossModItem(fargosSouls, "WretchedPouch", shop, ref nextSlot, (0.2f * 5), shopMulti);
                            }
                        }
                        if (ModLoader.TryGetMod("AmuletOfManyMinions", out Mod amuletOfManyMinions))
                        {
                            NPCHelper.SafelySetCrossModItem(amuletOfManyMinions, "GoblinGunnerMinionItem", shop, ref nextSlot, (0.44f * 5), shopMulti); //Goblin Radio Beacon
                        }
                        if (ModLoader.TryGetMod("StormDiversMod", out Mod stormsAdditions))
                        {
                            NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameBMask", shop, ref nextSlot, 1f, shopMulti); //Shadowflare Mask
                            NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameChestplate", shop, ref nextSlot, 1f, shopMulti); //Shadowflare Robe
                            NPCHelper.SafelySetCrossModItem(stormsAdditions, "ShadowFlameGreaves", shop, ref nextSlot, 1f, shopMulti); //Shadowflare Greaves
                        }
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
        }
	}
}