/*
using System;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace MyMod
{
	// This class showcases some examples on how to add your own items to these Town NPCs by using Mod.Calls in PostSetupContent()
	// With this method, a weak or strong reference is not needed.
	// For all of the NPC names and Mod.Calls, see below.
	public class CrossModShopExample : Mod
	{
		public override void PostSetupContent()
		{
			// First, see if Bosses As NPCs is loaded.
			if (ModLoader.TryGetMod("BossesAsNPCs", out Mod bossesAsNPCs))
			{
				// Using Mod.Calls we can easily add items to the shops.
				// You do not need to worry about adding the shop to the Torch God, as he will automatically get the corresponding boss' shop.
				// You do not need to worry about setting the shop scaling for prices, that is handled automatically as well.
				// You do not need to worry about checking if the NPC/shop is enabled, that is handled automatically as well.

				// First, call "AddToShop". Then you have 4 choices for the price setting.
				//   "DefaultPrice"         This will set the price to whatever the value of the item is.
				//   "CustomPrice"          This will set the price to whatever value you specify here.
				//   "WithDiv"              This will get the value of the item divided by 5 and then divided by the value you specify here. [value / 5 / priceDiv]
				//   "WithDivAndMulti"      This will get the value of the item, divide it by the value you specify, and multiply it by the value you specify. [(value / priceDiv) * priceMulti]
				// Quick warning: The modifiers in "WithDiv" and "WithDivAndMulti" will not work on vanilla items, use "CustomPrice" instead and calculate the value here.

				// The 3rd parameter is which shop you want to add your item to. A list of valid strings are at the bottom of this file.
				// The 4th parameter is the item you want to be sold. (int data type)
				// The 5th parameter is the availability. The item will only be sold once this condition is true. (Func<bool> data type)

				// Parameter and data types for each call:
				// "DefaultPrice"
				//     bossesAsNPCs.Call(string call, string priceMode, string shop, int itemType, Func<bool> availability)
				// "CustomPrice"
				//     bossesAsNPCs.Call(string call, string priceMode, string shop, int itemType, Func<bool> availability, int customPrice)
				// "WithDiv"
				//     bossesAsNPCs.Call(string call, string priceMode, string shop, int itemType, Func<bool> availability, float priceDiv)
				// "WithDivAndMulti"
				//     bossesAsNPCs.Call(string call, string priceMode, string shop, int itemType, Func<bool> availability, float priceDiv, float priceMulti)


				// Here we want to add an item to the Goblin Tinkerer's shop that is a 10% drop chance if the Goblin Summoner has been defeated.
				// Explaining the call line by line:
				bossesAsNPCs.Call("AddToShop", // Call "AddToShop" to add something to a shop
						"WithDiv", // Select the price mode. "WithDiv" will take a float as the 6th parameter
						"GoblinTinkerer", // Select the shop we want to add the item to. In this case, Goblin Tinkerer
						ModContent.ItemType<My10PercentDropChanceItem>(), // Our mod item that we want to be sold
						() => (bool)bossesAsNPCs.Call("downedGoblinSummoner"), // The Func<bool> for our availability. We call to see if the Goblin Summoner has been defeated.
						0.1f); // The float that will divide our price.

				// Here is an example selling an Expert Mode exclusive item at the value of the item.
				// We set the Func<bool> to Main.expertMode OR if the Bosses As NPCs config "Sell Expert Mode Items in Non-Expert Worlds" is set to true.
				bossesAsNPCs.Call("AddToShop", "DefaultPrice", "IceQueen", ModContent.ItemType<MyExpertItem>(), () => Main.expertMode || (bool)bossesAsNPCs.Call("SellExpertMode"));

				// If it makes it easier for you, you can create variables.
				int itemType = ModContent.ItemType<MyVanityOrMatieralItem>(); // Our item that we want to use
				Func<bool> availability = () => (bool)bossesAsNPCs.Call("SellExtraItems"); // Only available if the "Sell Extra Items" config is enabled.

				// Here we set the price of the item. This item of ours has a 5% drop chance. We want our price to match that.
				// We manually set the value of the item to 30000 (3 gold).
				// Then we divide by our drop chance. 5% chance = 0.05
				int price = (int)Math.Round(30000 / 0.05f);

				bossesAsNPCs.Call("AddToShop", "CustomPrice", "BrainOfCthulhu", itemType, availability, price);

				// Here is an example using a vanilla item. Here we are adding Souls of Light to the Empress of Light's shop for 5 gold. The item is always available.
				// As stated above, the modifiers in "WithDiv" and "WithDivAndMulti" will not work on vanilla items. So we use "CustomPrice" instead.
				// You need to cast the ItemID as an (int).
				// Item.buyPrice() is a nice method that will make the correct coin value for you.
				bossesAsNPCs.Call("AddToShop", "CustomPrice", "EmpressOfLight", (int)ItemID.SoulofLight, () => true, Item.buyPrice(gold: 5));


				// This last example wants to add an item that is 20 times more expensive than its value if Moon Lord has been defeated.
				bossesAsNPCs.Call("AddToShop", "WithDivAndMulti", "Golem", ModContent.ItemType<MyExpensiveItem>(), () => NPC.downedMoonlord, 1f, 20f);


				// For all of the Turbo Nerds out there who want to see how this is implemented in the mod, look at NPCs/SetupShops.cs (or BossesAsNPCs.cs for the Mod Calls)
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
					// You MUST use TryFind. If you use Find, your mod will not load if the given NPC is unloaded.
					if (bossesAsNPCs.TryFind<ModNPC>("MoonLord", out ModNPC moonLord))
					{
						var moonLordHappiness = NPCHappiness.Get(moonLord.Type);

						// Then we make both NPCs love each other.
						// Make sure to follow HappinessVar.SetNPCAffection(TypeVar, ...)
						myNPCHappiness.SetNPCAffection(moonLord.Type, AffectionLevel.Love);
						moonLordHappiness.SetNPCAffection(myNPCType, AffectionLevel.Love);
					}
				}
			}
		}
	}
}
*/

/* What are the internal NPC names for these Town NPCs?
 * Most are pretty obvious, but remember to capitalize each word. Even words like "of".
 * 
 * English Name           Class Name
 * 
 * King Slime        =    KingSlime
 * Eye of Cthulhu    =    EyeOfCthulhu
 * Eater of Worlds   =    EaterOfWorlds
 * Brain of Cthulhu  =    BrainOfCthulhu
 * Queen Bee         =    QueenBee
 * Skeletron         =    Skeletron
 * Deerclops         =    Deerclops
 * Wall of Flesh     =    WallOfFlesh
 * Queen Slime       =    QueenSlime
 * The Destroyer     =    TheDestroyer
 * Retinazer         =    Retinazer
 * Spazmatism        =    Spazmatism
 * Plantera          =    Plantera
 * Golem             =    Golem
 * Empress of Light  =    EmpressOfLight
 * Duke Fishron      =    DukeFishron
 * Betsy             =    Betsy
 * Lunatic Cultist   =    LunaticCultist
 * Moon Lord         =    MoonLord
 * Dreadnaultilus    =    Dreadnaultilus
 * Mothron           =    Mothron
 * Pumpking          =    Pumpking
 * Ice Queen         =    IceQueen
 * Martian Saucer    =    MartianSaucer
 */

/* Mod Calls
 * More info on https://terrariamods.fandom.com/wiki/User:Rijam/Bosses_As_NPCs#Mod_Calls
 * 
 * Call                             Data Type
 * 
 * "downedBetsy"                    bool
 * "downedDungeonGuardian"          bool
 * "downedDarkMage"                 bool
 * "downedOgre"                     bool
 * "downedGoblinSummoner"           bool
 * "downedDreadnautilus"            bool
 * "downedMothron"                  bool
 * "SellExpertMode"                 bool
 * "SellMasterMode"                 bool
 * "SellExtraItems"                 bool
 * "shopMulti"                      float
 * "TownNPCsCrossModSupport"        bool
 * "CatchNPCs"                      bool
 * "AllInOneNPCMode"                int
 * "GoblinSellInvasionItems"        bool
 * "PirateSellInvasionItems"        bool
 * "CanSpawn"                       bool
 *    "KingSlime"
 *    "EyeOfCthulhu"
 *    "EaterOfWorlds"
 *    "BrainOfCthulhu"
 *    "QueenBee"
 *    "Skeletron"
 *    "Deerclops"
 *    "WallOfFlesh"
 *    "QueenSlime"
 *    "TheDestroyer"
 *    "TheTwins"
 *    "SkeletronPrime"
 *    "Plantera"
 *    "Golem"
 *    "EmpressOfLight"
 *    "DukeFishron"
 *    "Betsy"
 *    "LunaticCultist"
 *    "MoonLord"
 *    "Dreadnautilus"
 *    "Mothron"
 *    "Pumpking"
 *    "IceQueen"
 *    "MartianSaucer"
 *    "TorchGod"
 * "AddToShop"                     bool
 *    "DefaultPrice"
 *    "CustomPrice"
 *    "WithDiv"
 *    "WithDivAndMulti"

 * The following are the valid shops that you can specify with "AddToShop"
 *  "KingSlime"
 *  "EyeOfCthulhu"
 *  "EaterOfWorlds"
 *  "BrainOfCthulhu"
 *  "QueenBee"
 *  "Skeletron"
 *  "Deerclops"
 *  "WallOfFlesh"
 *  "QueenSlime"
 *  "TheDestroyer"
 *  "Retinazer"
 *  "Spazmatism"
 *  "SkeletronPrime"
 *  "Plantera"
 *  "Golem"
 *  "EmpressOfLight"
 *  "DukeFishron"
 *  "Betsy"
 *  "LunaticCultist"
 *  "MoonLord"
 *  "Dreadnautilus"
 *  "Mothron"
 *  "Pumpking"
 *  "IceQueen"
 *  "MartianSaucer"
 *  "GoblinTinkerer"
 *  "Pirate"
 */