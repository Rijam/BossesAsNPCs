using BossesAsNPCs.Items;
using BossesAsNPCs.NPCs;
using BossesAsNPCs.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs
{
	public class BossesAsNPCs : Mod
	{
		internal static BossesAsNPCsConfigServer ConfigServer;
		internal static BossesAsNPCs Instance;
		// internal bool dialogueTweakUsingStaticOrBestiaryDrawSetting = false;

		public override void Load()
		{
			Instance = this;
			if (ModLoader.TryGetMod("Wikithis", out Mod wikithis) && !Main.dedServ)
			{
				wikithis.Call("AddModURL", this, "https://terrariamods.wiki.gg/wiki/Bosses_As_NPCs/{}");
				wikithis.Call("AddWikiTexture", this, ModContent.Request<Texture2D>("BossesAsNPCs/icon_small"));
			}
			if (ModLoader.TryGetMod("ItemCheckBlacklist", out Mod itemCheckBlacklist))
			{
				itemCheckBlacklist.Call("ItemCheckBlacklist", new List<int>() { ModContent.ItemType<TownNPCWeapon>(), ModContent.ItemType<DebugMethodTester>(), ModContent.ItemType<DebugMethodTester2>(),
					ModContent.ItemType<CaughtBetsy>(), ModContent.ItemType<CaughtBrainOfCthulhu>(), ModContent.ItemType<CaughtDeerclops>(), ModContent.ItemType<CaughtDreadnautilus>(),
					ModContent.ItemType<CaughtDukeFishron>(), ModContent.ItemType<CaughtEaterOfWorlds>(), ModContent.ItemType<CaughtEmpressOfLight>(), ModContent.ItemType<CaughtEyeOfCthulhu>(),
					ModContent.ItemType<CaughtGolem>(), ModContent.ItemType<CaughtIceQueen>(), ModContent.ItemType<CaughtKingSlime>(), ModContent.ItemType<CaughtLunaticCultist>(),
					ModContent.ItemType<CaughtMartianSaucer>(), ModContent.ItemType<CaughtMoonLord>(), ModContent.ItemType<CaughtMothron>(), ModContent.ItemType<CaughtPlantera>(),
					ModContent.ItemType<CaughtPumpking>(), ModContent.ItemType<CaughtQueenBee>(), ModContent.ItemType<CaughtQueenSlime>(), ModContent.ItemType<CaughtRetinazer>(),
					ModContent.ItemType<CaughtSkeletron>(), ModContent.ItemType<CaughtSkeletronPrime>(), ModContent.ItemType<CaughtSpazmatism>(), ModContent.ItemType<CaughtTheDestroyer>(),
					ModContent.ItemType<CaughtTorchGod>(), ModContent.ItemType<CaughtWallOfFlesh>()});
			}
		}

		public override void Unload()
		{
			NPCs.SetupShops.ClearCustomShops();
			ConfigServer = null;
			Instance = null;
			NPCs.SetupShops.GoblinTinkererShopCopy = null;
			NPCs.SetupShops.PirateShopCopy = null;
			// dialogueTweakUsingStaticOrBestiaryDrawSetting = false;
		}

		public override void PostSetupContent()
		{
			if (ModLoader.TryGetMod("DialogueTweak", out Mod dialogueTweak))
			{
				dialogueTweak.Call("ReplaceExtraButtonIcon",
					ModContent.NPCType<TorchGod>(),
					"BossesAsNPCs/NPCs/Icon_CycleShops");

				dialogueTweak.Call("ReplaceExtraButtonIcon",
					new List<int>
					{
						ModContent.NPCType<KingSlime>(),
						ModContent.NPCType<EyeOfCthulhu>(),
						ModContent.NPCType<EaterOfWorlds>(),
						ModContent.NPCType<BrainOfCthulhu>(),
						ModContent.NPCType<QueenBee>(),
						ModContent.NPCType<Skeletron>(),
						ModContent.NPCType<Deerclops>(),
						ModContent.NPCType<WallOfFlesh>(),
						ModContent.NPCType<QueenSlime>(),
						ModContent.NPCType<TheDestroyer>(),
						ModContent.NPCType<Retinazer>(),
						ModContent.NPCType<Spazmatism>(),
						ModContent.NPCType<SkeletronPrime>(),
						ModContent.NPCType<Plantera>(),
						ModContent.NPCType<Golem>(),
						ModContent.NPCType<EmpressOfLight>(),
						ModContent.NPCType<DukeFishron>(),
						ModContent.NPCType<Betsy>(),
						ModContent.NPCType<LunaticCultist>(),
						ModContent.NPCType<MoonLord>(),
						ModContent.NPCType<Dreadnautilus>(),
						ModContent.NPCType<Mothron>(),
						ModContent.NPCType<Pumpking>(),
						ModContent.NPCType<IceQueen>(),
						ModContent.NPCType<MartianSaucer>(),
					},
					"BossesAsNPCs/NPCs/Icon_Shop2");

				/*
				if (dialogueTweak.TryFind<ModConfig>("Configuration", out ModConfig dialogueTweakConfig))
				{
					// Trying to get the config value of PortraitDrawStyle to see if it is Static or Bestiary.
					// If so, the glow masks and such for the Town NPCs need to be drawn higher up.
					// https://github.com/Cyrillya/DialogueTweak/blob/1.4.4/Configuration.cs#L34

					FieldInfo DialogueTweakConfigPortraitDrawStyle = dialogueTweakConfig.GetType().GetField("PortraitDrawStyle", BindingFlags.Public | BindingFlags.Instance );
					Logger.DebugFormat("DialogueTweakConfigPortraitDrawStyle {0}", DialogueTweakConfigPortraitDrawStyle);
					object value = DialogueTweakConfigPortraitDrawStyle.GetValue(dialogueTweakConfig);
					Logger.DebugFormat("value {0}", value);
					if (value?.ToString() == "Static" || value?.ToString() == "Bestiary")
					{
						dialogueTweakUsingStaticOrBestiaryDrawSetting = true;
						Logger.Debug("Static or Bestiary draw style");
					}
					else
					{
						dialogueTweakUsingStaticOrBestiaryDrawSetting = false;
					}
				}
				*/
			}
			/*
			if (ModLoader.TryGetMod("BetterDialogue", out Mod dialect))
			{
				try
				{
					// Idk reflection
					PropertyInfo supportedNPCs = dialect.GetType().GetProperty("SupportedNPCs", BindingFlags.Public | BindingFlags.Static);
					List<int> list = (List<int>)supportedNPCs?.GetValue(dialect);
					list.Add(ModContent.ItemType<CaughtKingSlime>());
					list.Add(ModContent.ItemType<CaughtEyeOfCthulhu>());
					list.Add(ModContent.ItemType<CaughtEaterOfWorlds>());
					list.Add(ModContent.ItemType<CaughtBrainOfCthulhu>());
					list.Add(ModContent.ItemType<CaughtQueenBee>());
					list.Add(ModContent.ItemType<CaughtSkeletron>());
					list.Add(ModContent.ItemType<CaughtDeerclops>());
					list.Add(ModContent.ItemType<CaughtWallOfFlesh>());
					list.Add(ModContent.ItemType<CaughtQueenSlime>());
					list.Add(ModContent.ItemType<CaughtTheDestroyer>());
					list.Add(ModContent.ItemType<CaughtSpazmatism>());
					list.Add(ModContent.ItemType<CaughtRetinazer>());
					list.Add(ModContent.ItemType<CaughtSkeletronPrime>());
					list.Add(ModContent.ItemType<CaughtPlantera>());
					list.Add(ModContent.ItemType<CaughtGolem>());
					list.Add(ModContent.ItemType<CaughtEmpressOfLight>());
					list.Add(ModContent.ItemType<CaughtDukeFishron>());
					list.Add(ModContent.ItemType<CaughtBetsy>());
					list.Add(ModContent.ItemType<CaughtLunaticCultist>());
					list.Add(ModContent.ItemType<CaughtMoonLord>());
					list.Add(ModContent.ItemType<CaughtDreadnautilus>());
					list.Add(ModContent.ItemType<CaughtMothron>());
					list.Add(ModContent.ItemType<CaughtPumpking>());
					list.Add(ModContent.ItemType<CaughtIceQueen>());
					list.Add(ModContent.ItemType<CaughtMartianSaucer>());
					list.Add(ModContent.ItemType<CaughtTorchGod>());
					supportedNPCs.SetValue(dialect, list);
					Logger.Debug("Bosses as NPCs Dialect support added?");
				}
				catch
				{
					Logger.Warn("Bosses as NPCs Dialect support failed.");
				}
			}
			*/
		}

		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			ArgumentNullException.ThrowIfNull(args);

			if (args[0] is not string function)
				throw new ArgumentException("Expected a function name for the first argument");

			void CheckArgsLength(int expected, params string[] argNames)
			{
				if (args.Length != expected)
					throw new ArgumentOutOfRangeException($"Expected {expected} arguments for Mod.Call(\"{function}\", {string.Join(",", argNames)}), got {args.Length} arguments instead");
			}

			switch (function)
			{
				case "downedBetsy":
					return BossesAsNPCsWorld.downedBetsy;
				case "downedDungeonGuardian":
					return BossesAsNPCsWorld.downedDungeonGuardian;
				case "downedDarkMage":
					return BossesAsNPCsWorld.downedDarkMage;
				case "downedOgre":
					return BossesAsNPCsWorld.downedOgre;
				case "downedGoblinSummoner":
					return BossesAsNPCsWorld.downedGoblinSummoner;
				case "downedGoblinWarlock":
					return BossesAsNPCsWorld.downedGoblinSummoner;
				case "downedDreadnautilus":
					return BossesAsNPCsWorld.downedDreadnautilus;
				case "downedMothron":
					return BossesAsNPCsWorld.downedMothron;
				case "downedEoW":
					return BossesAsNPCsWorld.downedEoW;
				case "downedBoC":
					return BossesAsNPCsWorld.downedBoC;
				case "downedWoF":
					return BossesAsNPCsWorld.downedWoF;
				case "daytimeEoLDefeated":
					return BossesAsNPCsWorld.daytimeEoLDefeated;
				case "SellExpertMode":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExpertMode;
				case "SellMasterMode":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().SellMasterMode;
				case "SellExtraItems":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().SellExtraItems;
				case "shopMulti":
					return (ModContent.GetInstance<BossesAsNPCsConfigServer>().ShopPriceScaling / 100f);
				case "TownNPCsCrossModSupport":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;
				case "CatchNPCs":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs;
				case "AllInOneNPCMode":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode;
				case "GoblinSellInvasionItems":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().GoblinSellInvasionItems;
				case "PirateSellInvasionItems":
					return ModContent.GetInstance<BossesAsNPCsConfigServer>().PirateSellInvasionItems;
				case "GetStatusShop1":
					Logger.Warn($"Function \"{function}\" is obsolete. Please use one of the \"AddToShop\" calls.");
					return false;
				case "GetStatusShop2":
					Logger.Warn($"Function \"{function}\" is obsolete. Please use one of the \"AddToShop\" calls.");
					return false;
				// Call("CanSpawn", string bossNPCName)
				case "CanSpawn":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					return args[1].ToString() switch
					{
						"KingSlime" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnKingSlime,
						"EyeOfCthulhu" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoC,
						"EoC" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoC,
						"EaterOfWorlds" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoW,
						"EoW" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoW,
						"BrainOfCthulhu" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBoC,
						"BoC" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBoC,
						"QueenBee" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnQueenBee,
						"Skeletron" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnSkeletron,
						"Deerclops" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDeerclops,
						"WallOfFlesh" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnWoF,
						"WoF" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBoC,
						"QueenSlime" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnQueenSlime,
						"TheDestroyer" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDestroyer,
						"Destroyer" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDestroyer,
						"TheTwins" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnTwins,
						"Twins" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnTwins,
						"SkeletronPrime" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnSkeletronPrime,
						"Plantera" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnPlantera,
						"Golem" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnGolem,
						"EmpressOfLight" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoL,
						"EoL" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnEoL,
						"DukeFishron" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDukeFishron,
						"Betsy" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnBetsy,
						"LunaticCultist" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnLunaticCultist,
						"MoonLord" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMoonLord,
						"Dreadnautilus" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnDreadnautilus,
						"Mothron" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMothron,
						"Pumpking" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnPumpking,
						"IceQueen" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnIceQueen,
						"MartianSaucer" => ModContent.GetInstance<BossesAsNPCsConfigServer>().CanSpawnMartianSaucer,
						"TorchGod" => ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode > 0,
						"TheTorchGod" => ModContent.GetInstance<BossesAsNPCsConfigServer>().AllInOneNPCMode > 0,
						_ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs"),
					};
				// Call("GetCondition", string conditionName)
				case "GetCondition":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					return args[1].ToString() switch
					{
						"TownNPCsCrossModSupport" => ShopConditions.TownNPCsCrossModSupport,
						"SellExtraItems" => ShopConditions.SellExtraItems,
						"GoblinSellInvasionItems" => ShopConditions.GoblinSellInvasionItems,
						"PirateSellInvasionItems" => ShopConditions.PirateSellInvasionItems,
						"IsNotNpcShimmered" => ShopConditions.IsNotNpcShimmered,
						"Expert" => ShopConditions.Expert,
						"Master" => ShopConditions.Master,
						"DaytimeEoLDefated" => ShopConditions.DaytimeEoLDefated,
						"DownedBetsy" => ShopConditions.DownedBetsy,
						"DownedDungeonGuardian" => ShopConditions.DownedDungeonGuardian,
						"DownedDarkMage" => ShopConditions.DownedDarkMage,
						"DownedOgre" => ShopConditions.DownedOgre,
						"DownedGoblinWarlock" => ShopConditions.DownedGoblinWarlock,
						"DownedGoblinSummoner" => ShopConditions.DownedGoblinSummoner,
						"DownedMothron" => ShopConditions.DownedMothron,
						"DownedDreadnautilus" => ShopConditions.DownedDreadnautilus,
						"DownedEaterOfWorlds" => ShopConditions.DownedEaterOfWorlds,
						"DownedBrainOfCthulhu" => ShopConditions.DownedBrainOfCthulhu,
						"DownedWallOfFlesh" => ShopConditions.DownedWallOfFlesh,
						"RescuedWizard" => ShopConditions.RescuedWizard,
						"UnlockOWMusicOrDrunkWorld" => ShopConditions.UnlockOWMusicOrDrunkWorld,
						"CorruptionOrHardmode" => ShopConditions.CorruptionOrHardmode,
						"CrimsonOrHardmode" => ShopConditions.CrimsonOrHardmode,
						"UndergroundCavernsOrHardmode" => ShopConditions.UndergroundCavernsOrHardmode,
						"HallowOrCorruptionOrCrimson" => ShopConditions.HallowOrCorruptionOrCrimson,
						"InIceAndHallowOrCorruptionOrCrimson" => ShopConditions.InIceAndHallowOrCorruptionOrCrimson,
						_ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs"),
					};
				// Call("AddToShop", string priceMode, string npc, int item, List<Condition> condition, ...)
				case "AddToShop":
					switch (args[1].ToString())
					{
						case "DefaultPrice":
							CheckArgsLength(5, [args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString()]);
							// string npc, int item, Condition condition
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4]);
						case "CustomPrice":
							CheckArgsLength(6, [args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString()]);
							// string npc, int item, Condition condition, int customPrice
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (int)args[5]);
						case "WithDiv":
							CheckArgsLength(6, [args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString()]);
							// string npc, int item, Condition condition, float priceDiv
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (float)args[5]);
						case "WithDivAndMulti":
							CheckArgsLength(7, [args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString(), args[6].ToString()]);
							// string npc, int item, Condition condition, float priceDiv, float priceMulti
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (float)args[5], (float)args[6]);
						default:
							throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs");
					}
				// Call("DisableInternalCrossModSupport", string modName)
				case "DisableInternalCrossModSupport":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					Logger.DebugFormat("Internal cross mod support for {0} has been disabled.", args[1].ToString());
					return args[1].ToString() switch
					{
						"Fargowiltas" => SetupShops.Fargowiltas = false,
						"FargowiltasSouls" => SetupShops.FargowiltasSouls = false,
						"CalamityMod" => SetupShops.CalamityMod = false,
						"OrchidMod" => SetupShops.OrchidMod = false,
						"Polarities" => SetupShops.Polarities = false,
						"ThoriumMod" => SetupShops.ThoriumMod = false,
						"StormDiversMod" => SetupShops.StormDiversMod = false,
						"AmuletOfManyMinions" => SetupShops.AmuletOfManyMinions = false,
						"ClickerClass" => SetupShops.ClickerClass = false,
						"QwertyMod" => SetupShops.QwertyMod = false,
						"MagicStorage" => SetupShops.MagicStorage = false,
						"ItReallyMustBe" => SetupShops.ItReallyMustBe = false,
						"EchoesoftheAncients" => SetupShops.EchoesoftheAncients = false,
						"StarsAbove" => SetupShops.StarsAbove = false,
						"StarlightRiver" => SetupShops.StarlightRiver = false,
						"PboneUtils" => SetupShops.PboneUtils = false,
						"Avalon" => SetupShops.Avalon = false,
						"Redeption" => SetupShops.Redeption = false,
						"Consolaria" => SetupShops.Consolaria = false,
						"SOTS" => SetupShops.SOTS = false,
						"VitalityMod" => SetupShops.VitalityMod = false,
						"TheConfectionRebirth" => SetupShops.TheConfectionRebirth = false,
						"CrystiliumMod" => SetupShops.CrystiliumMod = false,
						_ => throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs"),
					};
				// Call("AddTownNPCCanLiveInCorruption", int npcType)
				case "AddTownNPCCanLiveInCorruption":
				case "AddTownNPCCanLiveInCorruptionType":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					TownNPCLiveInBadBiomeSets.CanLiveInCorruption[(int)args[1]] = true;
					return TownNPCLiveInBadBiomeSets.CanLiveInCorruption[(int)args[1]];
				// Call("AddTownNPCCanLiveInCorruption", NPC npcInstance)
				case "AddTownNPCCanLiveInCorruptionNPC":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					TownNPCLiveInBadBiomeSets.CanLiveInCorruption[((NPC)args[1]).type] = true;
					return TownNPCLiveInBadBiomeSets.CanLiveInCorruption[((NPC)args[1]).type];
				case "AddTownNPCCanLiveInCrimson":
				case "AddTownNPCCanLiveInCrimsonType":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					TownNPCLiveInBadBiomeSets.CanLiveInCrimson[(int)args[1]] = true;
					return TownNPCLiveInBadBiomeSets.CanLiveInCrimson[(int)args[1]];
				case "AddTownNPCCanLiveInCrimsonNPC":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					TownNPCLiveInBadBiomeSets.CanLiveInCrimson[((NPC)args[1]).type] = true;
					return TownNPCLiveInBadBiomeSets.CanLiveInCrimson[((NPC)args[1]).type];
				case "AddTownNPCCanLiveInDungeon":
				case "AddTownNPCCanLiveInDungeonType":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					TownNPCLiveInBadBiomeSets.CanLiveInDungeon[(int)args[1]] = true;
					return TownNPCLiveInBadBiomeSets.CanLiveInDungeon[(int)args[1]];
				case "AddTownNPCCanLiveInDungeonNPC":
					CheckArgsLength(2, [args[0].ToString(), args[1].ToString()]);
					TownNPCLiveInBadBiomeSets.CanLiveInDungeon[((NPC)args[1]).type] = true;
					return TownNPCLiveInBadBiomeSets.CanLiveInDungeon[((NPC)args[1]).type];
				default:
					throw new ArgumentException($"Function \"{function}\" is not defined by BossesAsNPCs");
			}
		}

		// Adapted from Thorium Mod
		/// <summary>
		/// Attempts to play a sound across the network. Only supports Volume and Pitch modifiers.
		/// </summary>
		/// <param name="soundStyle"> The SoundStyle of the sound. Can include Volume and Pitch modifiers. </param>
		/// <param name="position"> The position of the sound. </param>
		/// <param name="player"> The player who is creating the sound. </param>
		/// <returns>True if multiplayer, false if single player.</returns>
		public bool PlayNetworkSound(SoundStyle soundStyle, Vector2 position, Player player)
		{
			PlaySound(soundStyle, player);

			if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
			{
				// Create a packet to send.
				ModPacket packet = GetPacket();
				packet.Write((byte)BossesAsNPCsMessageType.PlayNetworkSound); // Message type
				packet.Write(soundStyle.SoundPath); // Sound path
				packet.Write(soundStyle.Volume); // Volume
				packet.Write(soundStyle.Pitch); // Pitch
				packet.Write(soundStyle.MaxInstances); // Max Instances
				packet.WriteVector2(position); // Position
				packet.Write((byte)player.whoAmI); // Who created the sound
				packet.Send(-1, player.whoAmI);

				return true;
			}
			return false;
		}

		/// <summary>
		/// Receives the packet and requests to play the sound.
		/// </summary>
		/// <param name="reader"></param>
		internal void PlayNetworkSoundReceive(BinaryReader reader)
		{
			string soundPath = reader.ReadString();
			float volume = reader.ReadSingle();
			float pitch = reader.ReadSingle();
			int maxInstances = reader.ReadInt32();
			Vector2 position = reader.ReadVector2();
			int playerIndex = reader.ReadByte();

			Player player = Main.player[playerIndex];
			PlayNetworkSound(new SoundStyle(soundPath) with { Volume = volume, Pitch = pitch, MaxInstances = maxInstances }, position, player);
		}
		/// <summary>
		/// Plays the sound at the player who created the sound's center.
		/// </summary>
		/// <param name="soundStyle"> The sound. </param>
		/// <param name="player"> The player who created the sound. </param>
		internal static void PlaySound(SoundStyle soundStyle, Player player)
		{
			SoundEngine.PlaySound(soundStyle, player.Center);
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			BossesAsNPCsMessageType msgType = (BossesAsNPCsMessageType)reader.ReadByte();
			switch (msgType)
			{
				case BossesAsNPCsMessageType.PlayNetworkSound:
					PlayNetworkSoundReceive(reader);
					break;
				default:
					Logger.WarnFormat("BossesAsNPCs: Unknown Message type: {0}", msgType);
					break;
			}
		}
	}
	internal enum BossesAsNPCsMessageType : byte
	{
		PlayNetworkSound
	}
}