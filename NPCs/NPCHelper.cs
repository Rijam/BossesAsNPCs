using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using System.Linq;
using static BossesAsNPCs.BossesAsNPCsConfigServer;

namespace BossesAsNPCs.NPCs
{
	/// <summary>
	/// NPCHelper is a small class that "automates" many repeated things for the Town NPCs.
	/// </summary>
	public class NPCHelper
	{
		/// Mod name
		private static readonly string mod = ModContent.GetInstance<BossesAsNPCs>().Name;

		/// <summary>
		/// Automatically gets the localized Loved text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LoveText(string npc)
		{
			return "[c/b3f2b3:" + Language.GetTextValue("RandomWorldName_Noun.Love") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Love") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Liked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string LikeText(string npc)
		{
			return "[c/ddf2b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Like") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Like") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Disliked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string DislikeText(string npc)
		{
			return "[c/f2e0b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Dislike") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Dislike") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Hated text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string HateText(string npc)
		{
			return "[c/f2b5b3:" + Language.GetTextValue("RandomWorldName_Noun.Hate") + "]: " + Language.GetTextValue("Mods." + mod + ".Bestiary.Happiness." + npc + ".Hate");
		}

		/// <summary>
		/// Automatically gets the path to the localized Bestiary Description.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string BestiaryPath(string npc)
		{
			return "Mods." + mod + ".Bestiary.Description." + npc;
		}

		/// <summary>
		/// Automatically gets the base path to the localized dialog. Add `+ "Key"` to get the dialog.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <returns>string</returns>
		public static string DialogPath(string npc)
		{
			return "Mods." + mod + ".NPCs." + npc + ".NPCDialog.";
		}

		/// <summary>
		/// Gets if the player has unlocked the Otherworldly music. Doesn't actually check for the player, but the shop runs client side so it doesn't matter.
		/// </summary>
		/// <returns>bool</returns>
		public static bool UnlockOWMusic()
		{
			return Main.Configuration.Get("UnlockMusicSwap", false);
		}

		private static bool shop1;
		private static bool shop2;

		/// <summary>
		/// Sets the shop1 bool. Set it to the opposite of the SetShop2()
		/// </summary>
		public static void SetShop1(bool tOrF)
		{
			shop1 = tOrF;
		}

		/// <summary>
		/// Sets the shop2 bool. Set it to the opposite of the SetShop1()
		/// </summary>
		public static void SetShop2(bool tOrF)
		{
			shop2 = tOrF;
		}

		/// <summary>
		/// Gets if shop1 is open.
		/// </summary>
		/// <returns>bool</returns>
		public static bool StatusShop1()
		{
			return shop1;
		}

		/// <summary>
		/// Gets if shop2 is open.
		/// </summary>
		/// <returns>bool</returns>
		public static bool StatusShop2()
		{
			return shop2;
		}

		private static int shopCycler = 0;
		// 1 = King Slime
		// 2 = King Slime 2
		// 3 = EoC
		// 4 = EoC 2
		// 5 = EoW
		// 6 = EoW 2
		// 7 = BoC
		// 8 = BoC 2
		// 9 = Queen Bee
		// 10 = Queen Bee 2
		// 11 = Skeletron
		// 12 = Skeletron 2
		// 13 = Deerclops
		// 14 = Deerclops 2
		// 15 = WoF
		// 16 = WoF 2
		// 17 = Queen Slime
		// 18 = Queen Slime 2
		// 19 = The Destroyer
		// 20 = The Destroyer 2
		// 21 = Retinazer
		// 22 = Retinazer 2
		// 23 = Spazmatism
		// 24 = Spazmatism 2
		// 25 = Skeletron Prime
		// 26 = Skeletron Prime 2
		// 27 = Plantera
		// 28 = Plantera 2
		// 29 = Golem
		// 30 = Golem 2
		// 31 = EoL
		// 32 = EoL 2
		// 33 = Duke Fishron
		// 34 = Duke Fishron 2
		// 35 = Betsy
		// 36 = Betsy 2
		// 37 = Lunatic Culist
		// 38 = Lunatic Culist 2
		// 39 = Moon Lord
		// 40 = Moon Lord 2
		// 41 = Dreadnautilus
		// 42 = Dreadnautilus 2
		// 43 = Mothron
		// 44 = Mothron 2
		// 45 = Pumpking
		// 46 = Pumpking 2
		// 47 = Ice Queen
		// 48 = Ice Queen 2
		// 49 = Martian Saucer
		// 50 = Martian Saucer 2

		/// <summary>
		/// Sets the shopCycler int.
		/// </summary>
		public static void SetShopCycle(int type)
		{
			shopCycler = type;
		}

		/// <summary>
		/// Gets the current shop selected.
		/// </summary>
		public static int StatusShopCycle()
		{
			return shopCycler;
		}

		/// <summary>
		/// Increments the shopCycler int. If it exceeds 50, it will be set to 1 again.
		/// Will get every shop.
		/// </summary>
		public static void IncrementShopCycleMode0()
		{
			shopCycler++;

			if (shopCycler > 50)
			{
				shopCycler = 0;
				IncrementShopCycleMode0();
			}
		}
		/// <summary>
		/// Decrements the shopCycler int. If it exceeds 50, it will be set to 1 again.
		/// Will get every shop.
		/// </summary>
		public static void DecrementShopCycleMode0()
		{
			shopCycler--;

			if (shopCycler <= 0)
			{
				shopCycler = 50;
			}
		}

		/// <summary>
		/// Increments the shopCycler int. If it exceeds the calculated number of shops, it will be set to 1 again.
		/// Will only select the shop if the config for that shop is disabled.
		/// </summary>
		public static void IncrementShopCycleMode1()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();

			// Bools true if the config is *off* and the boss has been defeated.
			bool KS = !config.CanSpawnKingSlime && NPC.downedSlimeKing;
			bool EoC = !config.CanSpawnEoC && NPC.downedBoss1;
			bool EoW = !config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW;
			bool BoC = !config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC;
			bool QB = !config.CanSpawnQueenBee && NPC.downedQueenBee;
			bool Sk = !config.CanSpawnSkeletron && NPC.downedBoss3;
			bool Dc = !config.CanSpawnDeerclops && NPC.downedDeerclops;
			bool WoF = !config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF;
			bool QS = !config.CanSpawnQueenSlime && NPC.downedQueenSlime;
			bool De = !config.CanSpawnDestroyer && NPC.downedMechBoss1;
			bool Tw = !config.CanSpawnTwins && NPC.downedMechBoss2;
			bool SP = !config.CanSpawnSkeletronPrime && NPC.downedMechBoss3;
			bool Pl = !config.CanSpawnPlantera && NPC.downedPlantBoss;
			bool Go = !config.CanSpawnGolem && NPC.downedGolemBoss;
			bool EoL = !config.CanSpawnEoL && NPC.downedEmpressOfLight;
			bool DF = !config.CanSpawnDukeFishron && NPC.downedFishron;
			bool Be = !config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy;
			bool LC = !config.CanSpawnLunaticCultist && NPC.downedAncientCultist;
			bool ML = !config.CanSpawnMoonLord && NPC.downedMoonlord;
			bool Dn = !config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus;
			bool Mo = !config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron;
			bool Pk = !config.CanSpawnPumpking && NPC.downedHalloweenKing;
			bool IQ = !config.CanSpawnIceQueen && NPC.downedChristmasIceQueen;
			bool MS = !config.CanSpawnMartianSaucer && NPC.downedMartians;

			int numOfShops = (KS.ToInt() + EoC.ToInt() + EoW.ToInt() + BoC.ToInt() + QB.ToInt() + Sk.ToInt() + Dc.ToInt() + WoF.ToInt()
				+ QS.ToInt() + De.ToInt() + (Tw.ToInt() * 2) + SP.ToInt() + Pl.ToInt() + Go.ToInt() + EoL.ToInt() + DF.ToInt() + Be.ToInt()
				+ LC.ToInt() + ML.ToInt() + Dn.ToInt() + Mo.ToInt() + Pk.ToInt() + IQ.ToInt() + MS.ToInt()) * 2;

			if (numOfShops > 0) // Only call if at least one of the shops are enabled.
			{
				shopCycler++;
			}
				
			if (!KS && shopCycler == 1) // If the bool is false (not a valid shop), go to the next shop.
				shopCycler += 2;
			if (!EoC && shopCycler == 3)
				shopCycler += 2;
			if (!EoW && shopCycler == 5)
				shopCycler += 2;
			if (!BoC && shopCycler == 7)
				shopCycler += 2;
			if (!QB && shopCycler == 9)
				shopCycler += 2;
			if (!Sk && shopCycler == 11)
				shopCycler += 2;
			if (!Dc && shopCycler == 13)
				shopCycler += 2;
			if (!WoF && shopCycler == 15)
				shopCycler += 2;
			if (!QS && shopCycler == 17)
				shopCycler += 2;
			if (!De && shopCycler == 19)
				shopCycler += 2;
			if (!Tw && shopCycler == 21)
				shopCycler += 4;
			if (!SP && shopCycler == 25)
				shopCycler += 2;
			if (!Pl && shopCycler == 27)
				shopCycler += 2;
			if (!Go && shopCycler == 29)
				shopCycler += 2;
			if (!EoL && shopCycler == 31)
				shopCycler += 2;
			if (!DF && shopCycler == 33)
				shopCycler += 2;
			if (!Be && shopCycler == 35)
				shopCycler += 2;
			if (!LC && shopCycler == 37)
				shopCycler += 2;
			if (!ML && shopCycler == 39)
				shopCycler += 2;
			if (!Dn && shopCycler == 41)
				shopCycler += 2;
			if (!Mo && shopCycler == 43)
				shopCycler += 2;
			if (!Pk && shopCycler == 45)
				shopCycler += 2;
			if (!IQ && shopCycler == 47)
				shopCycler += 2;
			if (!MS && shopCycler == 49)
				shopCycler += 2;

			if (shopCycler > 50)
			{
				shopCycler = 0;
				if (numOfShops > 0) // Only call if at least one of the shops are enabled.
				{
					IncrementShopCycleMode1();
				}
			}
		}
		/// <summary>
		/// Decrements the shopCycler int. If it exceeds the calculated number of shops, it will be set to 1 again.
		/// Will only select the shop if the config for that shop is disabled.
		/// </summary>
		public static void DecrementShopCycleMode1()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();

			// Bools true if the config is *off* and the boss has been defeated.
			bool KS = !config.CanSpawnKingSlime && NPC.downedSlimeKing;
			bool EoC = !config.CanSpawnEoC && NPC.downedBoss1;
			bool EoW = !config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW;
			bool BoC = !config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC;
			bool QB = !config.CanSpawnQueenBee && NPC.downedQueenBee;
			bool Sk = !config.CanSpawnSkeletron && NPC.downedBoss3;
			bool Dc = !config.CanSpawnDeerclops && NPC.downedDeerclops;
			bool WoF = !config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF;
			bool QS = !config.CanSpawnQueenSlime && NPC.downedQueenSlime;
			bool De = !config.CanSpawnDestroyer && NPC.downedMechBoss1;
			bool Tw = !config.CanSpawnTwins && NPC.downedMechBoss2;
			bool SP = !config.CanSpawnSkeletronPrime && NPC.downedMechBoss3;
			bool Pl = !config.CanSpawnPlantera && NPC.downedPlantBoss;
			bool Go = !config.CanSpawnGolem && NPC.downedGolemBoss;
			bool EoL = !config.CanSpawnEoL && NPC.downedEmpressOfLight;
			bool DF = !config.CanSpawnDukeFishron && NPC.downedFishron;
			bool Be = !config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy;
			bool LC = !config.CanSpawnLunaticCultist && NPC.downedAncientCultist;
			bool ML = !config.CanSpawnMoonLord && NPC.downedMoonlord;
			bool Dn = !config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus;
			bool Mo = !config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron;
			bool Pk = !config.CanSpawnPumpking && NPC.downedHalloweenKing;
			bool IQ = !config.CanSpawnIceQueen && NPC.downedChristmasIceQueen;
			bool MS = !config.CanSpawnMartianSaucer && NPC.downedMartians;

			int numOfShops = (KS.ToInt() + EoC.ToInt() + EoW.ToInt() + BoC.ToInt() + QB.ToInt() + Sk.ToInt() + Dc.ToInt() + WoF.ToInt()
				+ QS.ToInt() + De.ToInt() + (Tw.ToInt() * 2) + SP.ToInt() + Pl.ToInt() + Go.ToInt() + EoL.ToInt() + DF.ToInt() + Be.ToInt()
				+ LC.ToInt() + ML.ToInt() + Dn.ToInt() + Mo.ToInt() + Pk.ToInt() + IQ.ToInt() + MS.ToInt()) * 2;

			if (numOfShops > 0) // Only call if at least one of the shops are enabled.
			{
				shopCycler -= 2;
			}

			if (!MS && shopCycler == 49) // If the bool is false (not a valid shop), go to the next shop.
				shopCycler -= 2;
			if (!IQ && shopCycler == 47)
				shopCycler -= 2;
			if (!Pk && shopCycler == 45)
				shopCycler -= 2;
			if (!Mo && shopCycler == 43)
				shopCycler -= 2;
			if (!Dn && shopCycler == 41)
				shopCycler -= 2;
			if (!ML && shopCycler == 39)
				shopCycler -= 2;
			if (!LC && shopCycler == 37)
				shopCycler -= 2;
			if (!Be && shopCycler == 35)
				shopCycler -= 2;
			if (!DF && shopCycler == 33)
				shopCycler -= 2;
			if (!EoL && shopCycler == 31)
				shopCycler -= 2;
			if (!Go && shopCycler == 29)
				shopCycler -= 2;
			if (!Pl && shopCycler == 27)
				shopCycler -= 2;
			if (!SP && shopCycler == 25)
				shopCycler -= 2;
			if (!Tw && shopCycler == 23)
				shopCycler -= 2;
			if (!Tw && shopCycler == 21)
				shopCycler -= 2;
			if (!De && shopCycler == 19)
				shopCycler -= 2;
			if (!QS && shopCycler == 17)
				shopCycler -= 2;
			if (!WoF && shopCycler == 15)
				shopCycler -= 2;
			if (!Dc && shopCycler == 13)
				shopCycler -= 2;
			if (!Sk && shopCycler == 11)
				shopCycler -= 2;
			if (!QB && shopCycler == 9)
				shopCycler -= 2;
			if (!BoC && shopCycler == 7)
				shopCycler -= 2;
			if (!EoW && shopCycler == 5)
				shopCycler -= 2;
			if (!EoC && shopCycler == 3)
				shopCycler -= 2;
			if (!KS && shopCycler == 1)
				shopCycler -= 2;

			if (numOfShops > 0) // Only call if at least one of the shops are enabled.
			{
				shopCycler++;
			}

			if (shopCycler <= 0)
			{
				shopCycler = 51;
				if (numOfShops > 0) // Only call if at least one of the shops are enabled.
				{
					DecrementShopCycleMode1();
				}
			}
			if (shopCycler == 51)
			{
				shopCycler = 0;
			}
		}

		/// <summary>
		/// Increments the shopCycler int. If it exceeds the calculated number of shops, it will be set to 1 again.
		/// Will only select the shop if the config for that shop is enabled.
		/// </summary>
		public static void IncrementShopCycleMode2()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			bool KS = config.CanSpawnKingSlime && NPC.downedSlimeKing;
			bool EoC = config.CanSpawnEoC && NPC.downedBoss1;
			bool EoW = config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW;
			bool BoC = config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC;
			bool QB = config.CanSpawnQueenBee && NPC.downedQueenBee;
			bool Sk = config.CanSpawnSkeletron && NPC.downedBoss3;
			bool Dc = config.CanSpawnDeerclops && NPC.downedDeerclops;
			bool WoF = config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF;
			bool QS = config.CanSpawnQueenSlime && NPC.downedQueenSlime;
			bool De = config.CanSpawnDestroyer && NPC.downedMechBoss1;
			bool Tw = config.CanSpawnTwins && NPC.downedMechBoss2;
			bool SP = config.CanSpawnSkeletronPrime && NPC.downedMechBoss3;
			bool Pl = config.CanSpawnPlantera && NPC.downedPlantBoss;
			bool Go = config.CanSpawnGolem && NPC.downedGolemBoss;
			bool EoL = config.CanSpawnEoL && NPC.downedEmpressOfLight;
			bool DF = config.CanSpawnDukeFishron && NPC.downedFishron;
			bool Be = config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy;
			bool LC = config.CanSpawnLunaticCultist && NPC.downedAncientCultist;
			bool ML = config.CanSpawnMoonLord && NPC.downedMoonlord;
			bool Dn = config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus;
			bool Mo = config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron;
			bool Pk = config.CanSpawnPumpking && NPC.downedHalloweenKing;
			bool IQ = config.CanSpawnIceQueen && NPC.downedChristmasIceQueen;
			bool MS = config.CanSpawnMartianSaucer && NPC.downedMartians;

			int numOfShops = (KS.ToInt() + EoC.ToInt() + EoW.ToInt() + BoC.ToInt() + QB.ToInt() + Sk.ToInt() + Dc.ToInt() + WoF.ToInt()
				+ QS.ToInt() + De.ToInt() + (Tw.ToInt() * 2) + SP.ToInt() + Pl.ToInt() + Go.ToInt() + EoL.ToInt() + DF.ToInt() + Be.ToInt()
				+ LC.ToInt() + ML.ToInt() + Dn.ToInt() + Mo.ToInt() + Pk.ToInt() + IQ.ToInt() + MS.ToInt()) * 2;

			if (numOfShops > 0) // Only call if at least one of the shops are enabled.
			{
				shopCycler++;
			}

			if (!KS && shopCycler == 1) // If disabled, go to the next shop.
				shopCycler += 2;
			if (!EoC && shopCycler == 3)
				shopCycler += 2;
			if (!EoW && shopCycler == 5)
				shopCycler += 2;
			if (!BoC && shopCycler == 7)
				shopCycler += 2;
			if (!QB && shopCycler == 9)
				shopCycler += 2;
			if (!Sk && shopCycler == 11)
				shopCycler += 2;
			if (!Dc && shopCycler == 13)
				shopCycler += 2;
			if (!WoF && shopCycler == 15)
				shopCycler += 2;
			if (!QS && shopCycler == 17)
				shopCycler += 2;
			if (!De && shopCycler == 19)
				shopCycler += 2;
			if (!Tw && shopCycler == 21)
				shopCycler += 4;
			if (!SP && shopCycler == 25)
				shopCycler += 2;
			if (!Pl && shopCycler == 27)
				shopCycler += 2;
			if (!Go && shopCycler == 29)
				shopCycler += 2;
			if (!EoL && shopCycler == 31)
				shopCycler += 2;
			if (!DF && shopCycler == 33)
				shopCycler += 2;
			if (!Be && shopCycler == 35)
				shopCycler += 2;
			if (!LC && shopCycler == 37)
				shopCycler += 2;
			if (!ML && shopCycler == 39)
				shopCycler += 2;
			if (!Dn && shopCycler == 41)
				shopCycler += 2;
			if (!Mo && shopCycler == 43)
				shopCycler += 2;
			if (!Pk && shopCycler == 45)
				shopCycler += 2;
			if (!IQ && shopCycler == 47)
				shopCycler += 2;
			if (!MS && shopCycler == 49)
				shopCycler += 2;

			if (shopCycler > 50)
			{
				shopCycler = 0;
				if (numOfShops > 0) // Only call if at least one of the shops are enabled.
				{
					IncrementShopCycleMode2();
				}
			}
		}
		/// <summary>
		/// Decrements the shopCycler int. If it exceeds the calculated number of shops, it will be set to 1 again.
		/// Will only select the shop if the config for that shop is enabled.
		/// </summary>
		public static void DecrementShopCycleMode2()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			bool KS = config.CanSpawnKingSlime && NPC.downedSlimeKing;
			bool EoC = config.CanSpawnEoC && NPC.downedBoss1;
			bool EoW = config.CanSpawnEoW && BossesAsNPCsWorld.downedEoW;
			bool BoC = config.CanSpawnBoC && BossesAsNPCsWorld.downedBoC;
			bool QB = config.CanSpawnQueenBee && NPC.downedQueenBee;
			bool Sk = config.CanSpawnSkeletron && NPC.downedBoss3;
			bool Dc = config.CanSpawnDeerclops && NPC.downedDeerclops;
			bool WoF = config.CanSpawnWoF && BossesAsNPCsWorld.downedWoF;
			bool QS = config.CanSpawnQueenSlime && NPC.downedQueenSlime;
			bool De = config.CanSpawnDestroyer && NPC.downedMechBoss1;
			bool Tw = config.CanSpawnTwins && NPC.downedMechBoss2;
			bool SP = config.CanSpawnSkeletronPrime && NPC.downedMechBoss3;
			bool Pl = config.CanSpawnPlantera && NPC.downedPlantBoss;
			bool Go = config.CanSpawnGolem && NPC.downedGolemBoss;
			bool EoL = config.CanSpawnEoL && NPC.downedEmpressOfLight;
			bool DF = config.CanSpawnDukeFishron && NPC.downedFishron;
			bool Be = config.CanSpawnBetsy && BossesAsNPCsWorld.downedBetsy;
			bool LC = config.CanSpawnLunaticCultist && NPC.downedAncientCultist;
			bool ML = config.CanSpawnMoonLord && NPC.downedMoonlord;
			bool Dn = config.CanSpawnDreadnautilus && BossesAsNPCsWorld.downedDreadnautilus;
			bool Mo = config.CanSpawnMothron && BossesAsNPCsWorld.downedMothron;
			bool Pk = config.CanSpawnPumpking && NPC.downedHalloweenKing;
			bool IQ = config.CanSpawnIceQueen && NPC.downedChristmasIceQueen;
			bool MS = config.CanSpawnMartianSaucer && NPC.downedMartians;

			int numOfShops = (KS.ToInt() + EoC.ToInt() + EoW.ToInt() + BoC.ToInt() + QB.ToInt() + Sk.ToInt() + Dc.ToInt() + WoF.ToInt()
				+ QS.ToInt() + De.ToInt() + (Tw.ToInt() * 2) + SP.ToInt() + Pl.ToInt() + Go.ToInt() + EoL.ToInt() + DF.ToInt() + Be.ToInt()
				+ LC.ToInt() + ML.ToInt() + Dn.ToInt() + Mo.ToInt() + Pk.ToInt() + IQ.ToInt() + MS.ToInt()) * 2;

			if (numOfShops > 0) // Only call if at least one of the shops are enabled.
			{
				shopCycler -= 2;
			}

			if (!MS && shopCycler == 49) // If disabled, go to the next shop.
				shopCycler -= 2;
			if (!IQ && shopCycler == 47)
				shopCycler -= 2;
			if (!Pk && shopCycler == 45)
				shopCycler -= 2;
			if (!Mo && shopCycler == 43)
				shopCycler -= 2;
			if (!Dn && shopCycler == 41)
				shopCycler -= 2;
			if (!ML && shopCycler == 39)
				shopCycler -= 2;
			if (!LC && shopCycler == 37)
				shopCycler -= 2;
			if (!Be && shopCycler == 35)
				shopCycler -= 2;
			if (!DF && shopCycler == 33)
				shopCycler -= 2;
			if (!EoL && shopCycler == 31)
				shopCycler -= 2;
			if (!Go && shopCycler == 29)
				shopCycler -= 2;
			if (!Pl && shopCycler == 27)
				shopCycler -= 2;
			if (!SP && shopCycler == 25)
				shopCycler -= 2;
			if (!Tw && shopCycler == 23)
				shopCycler -= 2;
			if (!Tw && shopCycler == 21)
				shopCycler -= 2;
			if (!De && shopCycler == 19)
				shopCycler -= 2;
			if (!QS && shopCycler == 17)
				shopCycler -= 2;
			if (!WoF && shopCycler == 15)
				shopCycler -= 2;
			if (!Dc && shopCycler == 13)
				shopCycler -= 2;
			if (!Sk && shopCycler == 11)
				shopCycler -= 2;
			if (!QB && shopCycler == 9)
				shopCycler -= 2;
			if (!BoC && shopCycler == 7)
				shopCycler -= 2;
			if (!EoW && shopCycler == 5)
				shopCycler -= 2;
			if (!EoC && shopCycler == 3)
				shopCycler -= 2;
			if (!KS && shopCycler == 1)
				shopCycler -= 2;

			if (numOfShops > 0) // Only call if at least one of the shops are enabled.
			{
				shopCycler++;
			}

			if (shopCycler <= 0)
			{
				shopCycler = 51;
				if (numOfShops > 0) // Only call if at least one of the shops are enabled.
				{
					DecrementShopCycleMode2();
				}
			}
			if (shopCycler == 51)
			{
				shopCycler = 0;
			}
		}

		/// <summary>
		/// Returns a list of all of the Town NPCs within 25 tiles.
		/// searchMode = 1: Everything that could be a Town NPC, including Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
		/// searchMode = 2: Town NPCs and Town Pets. Excludes Old Man, Traveling Merchant, and Skeleton Merchant.
		/// searchMode = 3: Only real Town NPCs. Excludes Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
		/// npcTypeListHouse is a list of the npc.type for all of the Town NPCs within 25 tiles.
		/// npcTypeListNearBy is a list of the npc.type for all of the Town NPCs within 50 tiles.
		/// npcTypeListVillage is a list of the npc.type for all of the Town NPCs within 120 tiles.
		/// npcTypeListAll is a list of the npc.type for all of the Town NPCs in the world.
		/// Use .Count if you want the total number of Town NPCs for the given list.
		/// Adapted from vanilla
		/// </summary>
		/// <returns>List<NPC> of all the Town NPCs within 25 tiles.</returns>
		public static List<NPC> GetNearbyResidentNPCs(NPC npc, int searchMode, out List<int> npcTypeListHouse, out List<int> npcTypeListNearBy, out List<int> npcTypeListVillage, out List<int> npcTypeListAll)
		{
			List<NPC> list = new();
			npcTypeListHouse = new();
			npcTypeListNearBy = new();
			npcTypeListVillage = new();
			npcTypeListAll = new();
			Vector2 npc1Home = new(npc.homeTileX, npc.homeTileY);
			if (npc.homeless)
			{
				npc1Home = new Vector2(npc.Center.X / 16f, npc.Center.Y / 16f);
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (i == npc.whoAmI)
				{
					continue;
				}
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.townNPC && NearbyResidentSearchMode(npc, nPC, searchMode))
				{
					Vector2 npc2Home = new(nPC.homeTileX, nPC.homeTileY);
					if (nPC.homeless)
					{
						npc2Home = nPC.Center / 16f;
					}
					float distance = Vector2.Distance(npc1Home, npc2Home);
					if (distance < 25f)
					{
						list.Add(nPC);
						npcTypeListHouse.Add(nPC.type);
					}
					if (distance < 50f)
					{
						npcTypeListNearBy.Add(nPC.type);
					}
					if (distance < 120f)
					{
						npcTypeListVillage.Add(nPC.type);
					}
					npcTypeListAll.Add(nPC.type);
				}
			}
			return list;
		}

		/// <summary>
		/// Used by NPCHelper.GetNearbyResidentNPCs().
		/// Returns true or false based on the search mode. See the method for info on the searchMode.
		/// </summary>
		/// <returns>True if the NPC fits the search mode requirements</returns>
		public static bool NearbyResidentSearchMode(NPC npc1, NPC npc2, int searchMode)
		{
			switch (searchMode)
			{
				case 1: // Everything that could be a Town NPC, including Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
					if (NPCID.Sets.ActsLikeTownNPC[npc2.type] || npc2.housingCategory >= 1)
					{
						return true;
					}
					return false;
				case 2: // Town NPCs and Town Pets. Excludes Old Man, Traveling Merchant, and Skeleton Merchant.
					if (npc2.type != NPCID.OldMan || npc2.type != NPCID.TravellingMerchant || npc2.type != NPCID.SkeletonMerchant || !NPCID.Sets.ActsLikeTownNPC[npc2.type])
					{
						return true;
					}
					return false;
				case 3: // Only real Town NPCs. Excludes Town Pets, Old Man, Traveling Merchant, and Skeleton Merchant.
					if (npc2.type != NPCID.OldMan || npc2.type != NPCID.TravellingMerchant || npc2.type != NPCID.SkeletonMerchant || !NPCID.Sets.ActsLikeTownNPC[npc2.type] && !WorldGen.TownManager.CanNPCsLiveWithEachOther(npc1, npc2))
					{
						return true;
					}
					return false;
				default:
					return false;
			}
		}

		/// <summary>
		/// Searches the shop (or chest) to see if an item is in it. slotNumber is the slot the item is in.
		/// See ItemOriginDesc.CheckIfInShop() for a player version.
		/// </summary>
		/// <returns>True if the item is found</returns>
		public static bool FindItemInShop(int[] shop, int item, out int? slotNumber)
		{
			slotNumber = null;
			for (int i = 0; i < Chest.maxItems; i++)
			{
				if (shop[i] == item)
				{
					slotNumber = i;
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Searches the shop (or chest) to see if an item is in it. slotNumber is the slot the item is in.
		/// See ItemOriginDesc.CheckIfInShop() for a player version.
		/// </summary>
		/// <returns>True if the item is found</returns>
		public static bool FindItemInShop(Chest shop, int item, out int? slotNumber)
		{
			slotNumber = null;
			for (int i = 0; i < Chest.maxItems; i++)
			{
				if (shop.item[i].type == item)
				{
					slotNumber = i;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if any boss has been defeated. (Only tracks the ones that are NPCs in this mod.)
		/// </summary>
		public static bool DownedAnyBoss()
		{
			if (NPC.downedSlimeKing || NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedQueenBee || NPC.downedBoss3 || NPC.downedDeerclops || BossesAsNPCsWorld.downedWoF
				|| NPC.downedQueenSlime || NPC.downedMechBossAny || NPC.downedPlantBoss || NPC.downedGolemBoss || NPC.downedEmpressOfLight || NPC.downedFishron
				|| BossesAsNPCsWorld.downedBetsy || NPC.downedAncientCultist || NPC.downedMoonlord || BossesAsNPCsWorld.downedDreadnautilus || BossesAsNPCsWorld.downedMothron
				|| NPC.downedHalloweenKing || NPC.downedChristmasIceQueen || NPC.downedMartians)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if any boss has been defeated. (Only tracks the ones that are NPCs in this mod.)
		/// Also checks that the configs for those bosses are disabled.
		/// </summary>
		public static bool DownedAnyBossWithConfigCheck()
		{
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			if ((NPC.downedSlimeKing && !config.CanSpawnKingSlime) ||
				(NPC.downedBoss1 && !config.CanSpawnEoC) ||
				(BossesAsNPCsWorld.downedEoW && !config.CanSpawnEoW) ||
				(BossesAsNPCsWorld.downedBoC && !config.CanSpawnBoC) ||
				(NPC.downedQueenBee && !config.CanSpawnQueenBee) ||
				(NPC.downedBoss3 && !config.CanSpawnSkeletron) ||
				(NPC.downedDeerclops && !config.CanSpawnDeerclops) ||
				(BossesAsNPCsWorld.downedWoF && !config.CanSpawnWoF) ||
				(NPC.downedQueenSlime && !config.CanSpawnQueenSlime) ||
				(NPC.downedMechBoss1 && !config.CanSpawnDestroyer) ||
				(NPC.downedMechBoss2 && !config.CanSpawnTwins) ||
				(NPC.downedMechBoss3 && !config.CanSpawnSkeletronPrime) ||
				(NPC.downedPlantBoss && !config.CanSpawnPlantera) ||
				(NPC.downedGolemBoss && !config.CanSpawnGolem) ||
				(NPC.downedEmpressOfLight && !config.CanSpawnEoL) ||
				(NPC.downedFishron && !config.CanSpawnDukeFishron) ||
				(BossesAsNPCsWorld.downedBetsy && !config.CanSpawnBetsy) ||
				(NPC.downedAncientCultist && !config.CanSpawnLunaticCultist) ||
				(NPC.downedMoonlord && !config.CanSpawnMoonLord) ||
				(BossesAsNPCsWorld.downedDreadnautilus && !config.CanSpawnDreadnautilus) ||
				(BossesAsNPCsWorld.downedMothron && !config.CanSpawnMothron) ||
				(NPC.downedHalloweenKing && !config.CanSpawnPumpking) ||
				(NPC.downedChristmasIceQueen && !config.CanSpawnIceQueen) ||
				(NPC.downedMartians && !config.CanSpawnMartianSaucer))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Safely returns the integer of the ModItem from the given mod.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <returns>int if found, 0 if not found.</returns>
		public static int SafelyGetCrossModItem(Mod mod, string itemString)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				return outItem.Type;
			}
			ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelyGetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			return 0;
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the value.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, NPCShop shop, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.Add(outItem.Type, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the customPrice.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="customPrice">The custom price of the item.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, NPCShop shop, int customPrice, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.Add(new Item(outItem.Type) { shopCustomPrice = customPrice }, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's value / 5 / priceDiv.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, NPCShop shop, float priceDiv, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.Add(new Item(outItem.Type) { shopCustomPrice = (int)Math.Round(outItem.Item.value / 5 / priceDiv) }, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Safely sets the shop item of the ModItem from the given slot in the given slot.
		/// Will not set the shop item if the ModItem is not found.
		/// The price of the item will be the item's (value / priceDiv) * priceMulti.
		/// </summary>
		/// <param name="mod">The mod that the item is from.</param>
		/// <param name="itemString">The class name of the item.</param>
		/// <param name="shop">The Chest shop of the Town NPC. Pass shop in most cases.</param>
		/// <param name="priceDiv">The price will be divided by this amount.</param>
		/// <param name="priceMulti">The price will be multiplied by this amount after the priceDiv.</param>
		public static void SafelySetCrossModItem(Mod mod, string itemString, NPCShop shop, float priceDiv, float priceMulti, params Condition[] condition)
		{
			mod.TryFind<ModItem>(itemString, out ModItem outItem);
			if (outItem != null)
			{
				shop.Add(new Item(outItem.Type) { shopCustomPrice = (int)Math.Round(outItem.Item.value / priceDiv * priceMulti) }, condition.Append(ShopConditions.TownNPCsCrossModSupport).ToArray());
			}
			else
			{
				ModContent.GetInstance<BossesAsNPCs>().Logger.WarnFormat("SafelySetCrossModItem(): ModItem type \"{0}\" from \"{1}\" was not found.", itemString, mod);
			}
		}

		/// <summary>
		/// Automatically gets the localized Loved text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string LoveText(string npc, string yourMod, string otherMod)
		{
			return "[c/b3f2b3:" + Language.GetTextValue("RandomWorldName_Noun.Love") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Love") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Liked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string LikeText(string npc, string yourMod, string otherMod)
		{
			return "[c/ddf2b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Like") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Like") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Disliked text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string DislikeText(string npc, string yourMod, string otherMod)
		{
			return "[c/f2e0b3:" + Language.GetTextValue("Mods.BossesAsNPCs.UI.Dislike") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Dislike") + "\n";
		}

		/// <summary>
		/// Automatically gets the localized Hated text for the Bestiary.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string HateText(string npc, string yourMod, string otherMod)
		{
			return "[c/f2b5b3:" + Language.GetTextValue("RandomWorldName_Noun.Hate") + "]: " + Language.GetTextValue("Mods." + yourMod + ".Bestiary.Happiness." + otherMod + "." + npc + ".Hate");
		}

		/// <summary>
		/// Automatically gets the path to the localized Bestiary Description.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string BestiaryPath(string npc, string yourMod, string otherMod)
		{
			return "Mods." + yourMod + ".Bestiary.Description." + otherMod + "." + npc;
		}

		/// <summary>
		/// Automatically gets the base path to the localized dialog. Add `+ "Key"` to get the dialog.
		/// </summary>
		/// <param name="npc">The NPC's (class) name. In most cases, just pass Name</param>
		/// <param name="yourMod">The name of your mod.</param>
		/// <param name="otherMod">The name of the other mod that the NPC is associated with.</param>
		/// <returns>string</returns>
		public static string DialogPath(string npc, string yourMod, string otherMod)
		{
			return "Mods." + yourMod + ".NPCs." + otherMod + "." + npc + ".NPCDialog.";
		}

		// This is used to bypass the NPCs unloading from the All In One config.
		public static readonly bool bypassMode = false;

		/// <summary>
		/// Determines whether the NPC should unload based on the All In One config.
		/// </summary>
		/// <param name="npc">Just pass `Name`</param>
		public static bool ShouldLoad(string npc)
		{
			if (bypassMode)
			{
				return true;
			}
			BossesAsNPCsConfigServer config = ModContent.GetInstance<BossesAsNPCsConfigServer>();
			AllInOneOptions mode = config.AllInOneNPCMode;
			if (mode == 0)
			{
				return true;
			}
			if (mode == AllInOneOptions.OnlyOne)
			{
				return false;
			}
			if (mode == AllInOneOptions.Mixed)
			{
				if (npc == NPCString.KingSlime && !config.CanSpawnKingSlime) return false;
				if (npc == NPCString.EyeOfCthulhu && !config.CanSpawnEoC) return false;
				if (npc == NPCString.EaterOfWorlds && !config.CanSpawnEoW) return false;
				if (npc == NPCString.BrainOfCthulhu && !config.CanSpawnBoC) return false;
				if (npc == NPCString.QueenBee && !config.CanSpawnQueenBee) return false;
				if (npc == NPCString.Skeletron && !config.CanSpawnSkeletron) return false;
				if (npc == NPCString.Deerclops && !config.CanSpawnDeerclops) return false;
				if (npc == NPCString.WallOfFlesh && !config.CanSpawnWoF) return false;
				if (npc == NPCString.QueenSlime && !config.CanSpawnQueenSlime) return false;
				if (npc == NPCString.TheDestroyer && !config.CanSpawnDestroyer) return false;
				if (npc == NPCString.Retinazer && !config.CanSpawnTwins) return false;
				if (npc == NPCString.Spazmatism && !config.CanSpawnTwins) return false;
				if (npc == NPCString.SkeletronPrime && !config.CanSpawnSkeletronPrime) return false;
				if (npc == NPCString.Plantera && !config.CanSpawnPlantera) return false;
				if (npc == NPCString.Golem && !config.CanSpawnGolem) return false;
				if (npc == NPCString.EmpressOfLight && !config.CanSpawnEoL) return false;
				if (npc == NPCString.DukeFishron && !config.CanSpawnDukeFishron) return false;
				if (npc == NPCString.Betsy && !config.CanSpawnBetsy) return false;
				if (npc == NPCString.LunaticCultist && !config.CanSpawnLunaticCultist) return false;
				if (npc == NPCString.MoonLord && !config.CanSpawnMoonLord) return false;
				if (npc == NPCString.Dreadnautilus && !config.CanSpawnDreadnautilus) return false;
				if (npc == NPCString.Mothron && !config.CanSpawnMothron) return false;
				if (npc == NPCString.Pumpking && !config.CanSpawnPumpking) return false;
				if (npc == NPCString.IceQueen && !config.CanSpawnIceQueen) return false;
				if (npc == NPCString.MartianSaucer && !config.CanSpawnMartianSaucer) return false;
			}
			return true;
		}
	}
	public static class ShopConditions
	{
#pragma warning disable CA2211 // Non-constant fields should not be visible

		/// <summary> Use ShopConditions.Expert instead </summary>
		public static Condition SellExpertMode =			new("Mods.BossesAsNPCs.Conditions.SellExpertMode",			() => ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode);
		/// <summary> Use ShopConditions.Master instead </summary>
		public static Condition SellMasterMode =			new("Mods.BossesAsNPCs.Conditions.SellMasterMode",			() => ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode);
		public static Condition SellExtraItems =			new("Mods.BossesAsNPCs.Conditions.SellExtraItems",			() => ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems);
		public static Condition TownNPCsCrossModSupport =	new("Mods.BossesAsNPCs.Conditions.TownNPCsCrossModSupport", () => ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport);
		public static Condition GoblinSellInvasionItems =	new("Mods.BossesAsNPCs.Conditions.GoblinSellInvasionItems", () => ModContent.GetInstance<BossesAsNPCsConfigServer>().GoblinSellInvasionItems);
		public static Condition PirateSellInvasionItems =	new("Mods.BossesAsNPCs.Conditions.PirateSellInvasionItems", () => ModContent.GetInstance<BossesAsNPCsConfigServer>().PirateSellInvasionItems);

		public static Condition IsNotNpcShimmered =		new("Mods.BossesAsNPCs.Conditions.IsNotNpcShimmered",	() => !Condition.IsNpcShimmered.IsMet());
		public static Condition Expert =				new("Mods.BossesAsNPCs.Conditions.Expert",				() => Condition.InExpertMode.IsMet() || SellExpertMode.IsMet());
		public static Condition Master =				new("Mods.BossesAsNPCs.Conditions.Master",				() => Condition.InMasterMode.IsMet() || SellMasterMode.IsMet());
		public static Condition Legendary =				new("Mods.BossesAsNPCs.Conditions.Legendary",			() => Master.IsMet() && (Condition.ForTheWorthyWorld.IsMet() || Condition.ZenithWorld.IsMet()));

		public static Condition DaytimeEoLDefated =		new("Mods.BossesAsNPCs.Conditions.DaytimeEoLDefated",		() => BossesAsNPCsWorld.daytimeEoLDefeated);
		public static Condition DownedBetsy =			new("Mods.BossesAsNPCs.Conditions.DownedBetsy",				() => BossesAsNPCsWorld.downedBetsy);
		public static Condition DownedDungeonGuardian = new("Mods.BossesAsNPCs.Conditions.DownedDungeonGuardian",	() => BossesAsNPCsWorld.downedDungeonGuardian);
		public static Condition DownedDarkMage =		new("Mods.BossesAsNPCs.Conditions.DownedDarkMage",			() => BossesAsNPCsWorld.downedDarkMage);
		public static Condition DownedOgre =			new("Mods.BossesAsNPCs.Conditions.DownedOgre",				() => BossesAsNPCsWorld.downedOgre);
		public static Condition DownedGoblinWarlock =	new("Mods.BossesAsNPCs.Conditions.DownedGoblinWarlock",		() => BossesAsNPCsWorld.downedGoblinSummoner);
		public static Condition DownedGoblinSummoner = DownedGoblinWarlock;
		public static Condition DownedMothron =			new("Mods.BossesAsNPCs.Conditions.DownedMothron",			() => BossesAsNPCsWorld.downedMothron);
		public static Condition DownedDreadnautilus =	new("Mods.BossesAsNPCs.Conditions.DownedDreadnautilus",		() => BossesAsNPCsWorld.downedDreadnautilus);
		public static Condition DownedEaterOfWorlds =	new("Mods.BossesAsNPCs.Conditions.DownedEaterOfWorlds",		() => BossesAsNPCsWorld.downedEoW);
		public static Condition DownedBrainOfCthulhu =	new("Mods.BossesAsNPCs.Conditions.DownedBrainOfCthulhu",	() => BossesAsNPCsWorld.downedBoC);
		public static Condition DownedWallOfFlesh =		new("Mods.BossesAsNPCs.Conditions.DownedWallOfFlesh",		() => BossesAsNPCsWorld.downedWoF);
		public static Condition DownedAnyPillar =		new("Mods.BossesAsNPCs.Conditions.DownedAnyPillar",			() => NPC.downedTowerSolar || NPC.downedTowerVortex || NPC.downedTowerNebula || NPC.downedTowerStardust);
		public static Condition DownedAllPillars =		new("Mods.BossesAsNPCs.Conditions.DownedAllPillars",		() => NPC.downedTowers);
		public static Condition UnlockedBiomeTorches =	new("Mods.BossesAsNPCs.Conditions.UnlockedBiomeTorches",	() => Main.LocalPlayer.unlockedBiomeTorches);

		public static Condition RescuedWizard =						new("Mods.BossesAsNPCs.Conditions.RescuedWizard",						() => NPC.savedWizard);
		public static Condition UnlockOWMusicOrDrunkWorld =			new("Mods.BossesAsNPCs.Conditions.UnlockOWMusicOrDrunkWorld",			() => NPCHelper.UnlockOWMusic() || Condition.DrunkWorld.IsMet());
		public static Condition CorruptionOrHardmode =				new("Mods.BossesAsNPCs.Conditions.CorruptionOrHardmode",				() => Condition.CorruptWorld.IsMet() || Condition.Hardmode.IsMet());
		public static Condition CrimsonOrHardmode =					new("Mods.BossesAsNPCs.Conditions.CrimsonOrHardmode",					() => Condition.CrimsonWorld.IsMet() || Condition.Hardmode.IsMet());
		public static Condition UndergroundCavernsOrHardmode =		new("Mods.BossesAsNPCs.Conditions.UndergroundCavernsOrHardmode",		() => (Condition.InDirtLayerHeight.IsMet() || Condition.InRockLayerHeight.IsMet()) || Condition.Hardmode.IsMet());
		public static Condition HallowOrCorruptionOrCrimson =		new("Mods.BossesAsNPCs.Conditions.HallowOrCorruptionOrCrimson",			() => Condition.InHallow.IsMet() || Condition.InCorrupt.IsMet() || Condition.InCrimson.IsMet());
		public static Condition InIceAndHallowOrCorruptionOrCrimson = new("Mods.BossesAsNPCs.Conditions.InIceAndHallowOrCorruptionOrCrimson", () => (Condition.InDirtLayerHeight.IsMet() || Condition.InRockLayerHeight.IsMet()) && Condition.InSnow.IsMet() && HallowOrCorruptionOrCrimson.IsMet());

		public static string TownNPCRangeS(string range) => Language.GetTextValue("Mods.BossesAsNPCs.Conditions.TownNPCRangeS", range);
		public static string CountTownNPCsS(int number) => Language.GetTextValue("Mods.BossesAsNPCs.Conditions.CountTownNPCsS", number);
		public static string EternityModeS = "Mods.BossesAsNPCs.Conditions.EternityModeS";

#pragma warning restore CA2211 // Non-constant fields should not be visible
	}
}