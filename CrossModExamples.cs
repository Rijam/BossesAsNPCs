/*
using System;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyMod
{
	// This class showcases some examples on how to add your own items to these Town NPCs by using GlobalNPC
	// With this method, a weak or strong reference is not needed.
	// For all of the NPC names and Mod.Calls, see below.
	public class CrossModShopExample : GlobalNPC
	{
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			// First, see if Bosses As NPCs is loaded.
			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
			{
				// Call the shopMulti. This is the "Shope Price Scaling" config in Bosses As NPCs.
				// We need to multiply our item's value with this in order to scale the price according to the config.
				float shopMulti = (float)bossesAsNPCs.Call("shopMulti");

				// Here we check if the NPC is the Goblin Tinkerer because we want to add an item to his shop.
				if (type == NPCID.GoblinTinkerer)
				{
					// We call to see if "Goblin Tinkerer Sells Goblin Army Items" config is enabled.
					// We also call to see if the Goblin Summoner has been defeated in this world. Vanilla does not have a bool for this enemy, but Bosses As NPCs adds one.
					// Lastly, we call to see if the "Town NPCs Cross Mod Support" config is enabled in Bosses As NPCs.
					if ((bool)bossesAsNPCs.Call("GoblinSellInvasionItems") && (bool)bossesAsNPCs.Call("downedGoblinSummoner") && (bool)bossesAsNPCs.Call("TownNPCsCrossModSupport"))
					{
						// Set the item that we want.
						shop.item[nextSlot].SetDefaults(ModContent.ItemType<My10PercentDropChanceItem>());

						// Here we set the price of the item. This item of ours has a 10% drop chance. We want our price to match that.
						// First, we get the value of the item. You could replace this with any integer value that you want.
						// Then we divide it by 5. We do this because the formula that Bosses As NPCs uses is the sell price. The normal value is 5 times the sell price.
						// Then we divide by our drop chance. 10% chance = 0.1
						// Lastly, we multiply by the shopMulti so it scales with the price scaling config.
						// The shopCustomPrice needs to be an int, so we cast it as such. The Math.Round is not necessary, but without it the value will always round down.
						shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value / 5 / 0.1 * shopMulti);

						// Increment the slot.
						nextSlot++;
					}
				}

				// Here we check to see the if it's the Ice Queen NPC.
				if (type == bossesAsNPCs.Find<ModNPC>("IceQueen").Type)
				{
					// We want to add our item to the "Shop 2" because that's where modded items should go.
					// Using Mod Calls, we check that it is not Shop 1 but is Shop 2.
					// Lastly, we call to see if the "Town NPCs Cross Mod Support" config is enabled in Bosses As NPCs.
					if (!(bool)bossesAsNPCs.Call("GetStatusShop1") && (bool)bossesAsNPCs.Call("GetStatusShop2") && (bool)bossesAsNPCs.Call("TownNPCsCrossModSupport"))
					{
						// This item is an Expert Mode item.
						// We check Main.expertMode OR if the Bosses As NPCs config "Sell Expert Mode Items in Non-Expert Worlds" is set to true.
						if (Main.expertMode || (bool)bossesAsNPCs.Call("SellExpertMode"))
						{
							// Set the item that we want.
							shop.item[nextSlot].SetDefaults(ModContent.ItemType<MyExpertItem>());

							// Here we set the price of the item. This example is a little different because our item is a 100% drop chance.
							// First, we get the value of the item. You could replace this with any integer value that you want.
							// We do not divide by 5 here. The formula that Bosses As NPCs uses is the sell price * 5. This value is already 5 times the sell price.
							// Lastly, we multiply by the shopMulti so it scales with the price scaling config.
							shop.item[nextSlot].shopCustomPrice = (int)Math.Round(shop.item[nextSlot].value * shopMulti);

							// Increment the slot.
							nextSlot++;
						}
					}
				}

				// Here we check to see the if it's the Brain of Cthulhu NPC.
				if (type == bossesAsNPCs.Find<ModNPC>("BrainOfCthulhu").Type)
				{
					// Check that "Shop 2" is open and the "Town NPCs Cross Mod Support" is enabled.
					if (!(bool)bossesAsNPCs.Call("GetStatusShop1") && (bool)bossesAsNPCs.Call("GetStatusShop2") && (bool)bossesAsNPCs.Call("TownNPCsCrossModSupport"))
					{
						// Now we check to see if the "Sell Extra Items" config is enabled.
						if ((bool)bossesAsNPCs.Call("SellExtraItems"))
						{
							// Set the item that we want.
							shop.item[nextSlot].SetDefaults(ModContent.ItemType<MyVanityOrMatieralItem>());

							// Here we set the price of the item. This item of ours has a 5% drop chance. We want our price to match that.
							// We manually set the value of the item to 30000 (3 gold).
							// Then we divide by our drop chance. 5% chance = 0.05
							// Lastly, we multiply by the shopMulti so it scales with the price scaling config.
							shop.item[nextSlot].shopCustomPrice = (int)Math.Round(30000 / 0.05 * shopMulti);

							// Increment the slot.
							nextSlot++;
						}
					}
				}
			}
		}
	}


	// This class showcases an example on how to Happiness to your Town NPCs and Bosses As NPCs' Town NPCs using GlobalNPC.
	// With this method, a weak or strong reference is not needed.
	// For all of the NPC names and Mod.Calls, see below.
	public class CrossModHappinessExample : GlobalNPC
	{
		public override void SetStaticDefaults()
		{
			// Here we get the Type and Happiness of our Town NPC.
			int myNPCType = ModContent.NPCType<MyNPC>();
			var myNPCHappiness = NPCHappiness.Get(myNPCType);

			// Next, see if Bosses As NPCs is loaded.
			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
			{
				// We call to see if the "Town NPCs Cross Mod Support" config is enabled in Bosses As NPCs.
				if ((bool)bossesAsNPCs.Call("TownNPCsCrossModSupport"))
				{
					// We get the Type and Happiness of our chosen NPC, in this case Moon Lord.
					int moonLordType = bossesAsNPCs.Find<ModNPC>("MoonLord").Type;
					var moonLordHappiness = NPCHappiness.Get(moonLordType);

					// Then we make both NPCs love each other.
					// Make sure to follow HappinessVar.SetNPCAffection(TypeVar, ...)
					myNPCHappiness.SetNPCAffection(moonLordType, AffectionLevel.Love);
					moonLordHappiness.SetNPCAffection(myNPCType, AffectionLevel.Love);
				}
			}
		}
	}
}
*/

/* What are the internal NPC names for these Town NPCs?
 * Most are pretty obvious, but remember to capitalize each word. Even words like "of".
 * 
 * English Name			Class Name
 * 
 * King Slime		=	KingSlime
 * Eye of Cthulhu	=	EyeOfCthulhu
 * Eater of Worlds	=	EaterOfWorlds
 * Brain of Cthulhu	=	BrainOfCthulhu
 * Queen Bee		=	QueenBee
 * Skeletron		=	Skeletron
 * Deerclops		=	Deerclops
 * Wall of Flesh	=	WallOfFlesh
 * Queen Slime		=	QueenSlime
 * The Destroyer	=	TheDestroyer
 * Retinazer		=	Retinazer
 * Spazmatism		=	Spazmatism
 * Plantera			=	Plantera
 * Golem			=	Golem
 * Empress of Light	=	EmpressOfLight
 * Duke Fishron		=	DukeFishron
 * Betsy			=	Betsy
 * Lunatic Cultist	=	LunaticCultist
 * Moon Lord		=	MoonLord
 * Dreadnaultilus	=	Dreadnaultilus
 * Mothron			=	Mothron
 * Pumpking			=	Pumpking
 * Ice Queen		=	IceQueen
 * Martian Saucer	=	MartianSaucer
 */

/* Mod Calls
 * More info on https://terrariamods.fandom.com/wiki/User:Rijam/Bosses_As_NPCs#Mod_Calls
 * 
 * Call								Data Type
 * 
 * "downedBetsy"					bool
 * "downedDungeonGuardian"			bool
 * "downedDarkMage"					bool
 * "downedOgre"						bool
 * "downedGoblinSummoner"			bool
 * "downedDreadnautilus"			bool
 * "downedMothron"					bool
 * "SellExpertMode"					bool
 * "SellMasterMode"					bool
 * "SellExtraItems"					bool
 * "shopMulti"						float
 * "TownNPCsCrossModSupport"		bool
 * "CatchNPCs"						bool
 * "GoblinSellInvasionItems"		bool
 * "PirateSellInvasionItems"		bool
 * "GetStatusShop1"					bool
 * "GetStatusShop2"					bool
 */