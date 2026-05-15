using System;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using BossesAsNPCs.NPCs;
using BossesAsNPCs.NPCs.TownNPCs;

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
					//item.GetStoreValue() is just `item.shopCustomPrice ?? item.value`
					item?.shopCustomPrice = (int?)Math.Round(item.GetStoreValue() * shopMulti);
				}
			}
			if (npc.type == NPCID.GoblinTinkerer)
			{
				if (shopName == "Terraria/GoblinTinkerer/Shop")
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
				if (shopName == "Terraria/GoblinTinkerer/Shop2")
				{
					foreach (Item item in items)
					{
						//item.GetStoreValue() is just `item.shopCustomPrice ?? item.value`
						item?.shopCustomPrice = (int?)Math.Round(item.GetStoreValue() * shopMulti);
					}
				}
			}
			if (npc.type == NPCID.Pirate)
			{
				if (shopName == "Terraria/Pirate/Shop")
				{
					foreach (Item item in items)
					{
						if (item is not null && SetupShops.PirateShopCopy.Contains(item.type))
						{
							item.shopCustomPrice = (int?)Math.Round(item.GetStoreValue() * shopMulti);
						}
					}
				}
				if (shopName == "Terraria/Pirate/Shop2")
				{
					foreach (Item item in items)
					{
						//item.GetStoreValue() is just `item.shopCustomPrice ?? item.value`
						item?.shopCustomPrice = (int?)Math.Round(item.GetStoreValue() * shopMulti);
					}
				}
			}
		}

		// Create the Shop 2 for the Goblin Tinkerer and Pirate right after the vanilla shops are created.
		private delegate void orig_RegisterGoblinTinkerer();
		private static Hook Hook_NPCShopDatabase_RegisterGoblinTinkerer;
		private delegate void orig_RegisterPirate();
		private static Hook Hook_NPCShopDatabase_RegisterPirate;

		public override void Load()
		{
			MethodInfo NPCShopDatabase_RegisterGoblinTinkerer = typeof(NPCShopDatabase).GetMethod("RegisterGoblinTinkerer", BindingFlags.Static | BindingFlags.NonPublic);
			// MonoModHooks.Modify()
			Hook_NPCShopDatabase_RegisterGoblinTinkerer = new Hook(NPCShopDatabase_RegisterGoblinTinkerer, On_NPCShopDatabase_RegisterGoblinTinkerer);
			MethodInfo NPCShopDatabase_RegisterPirate = typeof(NPCShopDatabase).GetMethod("RegisterPirate", BindingFlags.Static | BindingFlags.NonPublic);
			Hook_NPCShopDatabase_RegisterPirate = new Hook(NPCShopDatabase_RegisterPirate, On_NPCShopDatabase_RegisterPirate);
		}

		public override void Unload()
		{
			Hook_NPCShopDatabase_RegisterGoblinTinkerer.Undo();
			Hook_NPCShopDatabase_RegisterPirate.Undo();
		}

		private void On_NPCShopDatabase_RegisterGoblinTinkerer(orig_RegisterGoblinTinkerer orig)
		{
			orig();
			var npcShop2 = new NPCShop(NPCID.GoblinTinkerer, "Shop2");
			SetupShops.GoblinTinkerer(npcShop2, "Shop2");
			npcShop2.Register();
		}

		private void On_NPCShopDatabase_RegisterPirate(orig_RegisterPirate orig)
		{
			orig();
			var npcShop2 = new NPCShop(NPCID.Pirate, "Shop2");
			SetupShops.Pirate(npcShop2, "Shop2");
			npcShop2.Register();
		}

		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.Pirate && shop.FullName == "Terraria/Pirate/Shop")
			{
				NPCs.SetupShops.Pirate(shop, "Shop");
			}
			if (shop.NpcType == NPCID.GoblinTinkerer && shop.FullName == "Terraria/GoblinTinkerer/Shop")
			{
				NPCs.SetupShops.GoblinTinkerer(shop, "Shop");
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

		public override void RegisterChatButtons(NPC npc, NPCInteractionList interactions)
		{
			if (npc.type == NPCID.GoblinTinkerer)
			{
				NPCInteraction openShop = interactions.Interactions.OfType<NPCInteractions.Actions.OpenShop>().FirstOrDefault();
				if (openShop is not null)
				{
					interactions.InsertAfter(new NPCHelper.OpenShopCrossModSupport("Shop2", Language.GetTextValue("Mods.BossesAsNPCs.UI.Shop2")), openShop);
				}
				else
				{
					Mod.Logger.Warn($"Unable to find the original Shop button for the Goblin Tinkerer to insert the Shop 2 button!");
				}
			}
			if (npc.type == NPCID.Pirate)
			{
				NPCInteraction openShop = interactions.Interactions.OfType<NPCInteractions.Actions.OpenShop>().FirstOrDefault();
				if (openShop is not null)
				{
					interactions.InsertAfter(new NPCHelper.OpenShopCrossModSupport("Shop2", Language.GetTextValue("Mods.BossesAsNPCs.UI.Shop2")), openShop);
				}
				else
				{
					Mod.Logger.Warn($"Unable to find the original Shop button for the Pirate to insert the Shop 2 button!");
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