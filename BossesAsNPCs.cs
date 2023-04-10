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
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossesAsNPCs
{
	public class BossesAsNPCs : Mod
	{
		internal static BossesAsNPCsConfigServer ConfigServer;
		internal static BossesAsNPCs Instance;

		public override void Load()
		{
			if (ModLoader.TryGetMod("Wikithis", out Mod wikithis) && !Main.dedServ)
			{
				// Special thanks to Wikithis for having an outdated mod calls description -_-
				// Actual special thanks to Confection Rebaked for having the correct format.
				wikithis.Call("AddModURL", this, "terrariamods.wiki.gg$Bosses_As_NPCs");
				wikithis.Call("AddWikiTexture", this, ModContent.Request<Texture2D>("BossesAsNPCs/icon_small"));
			}
		}

		public override void Unload()
		{
			NPCs.SetupShops.ClearCustomShops();
			ConfigServer = null;
			Instance = null;
		}

		public override void PostSetupContent()
		{
			if (ModLoader.TryGetMod("Census", out Mod censusMod))
			{
				censusMod.Call("TownNPCCondition", ModContent.NPCType<KingSlime>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.KingSlime"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EyeOfCthulhu>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.EyeOfCthulhu"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EaterOfWorlds>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.EaterOfWorlds"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<BrainOfCthulhu>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.BrainOfCthulhu"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<QueenBee>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.QueenBee"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Skeletron>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Skeletron"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Deerclops>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Deerclops"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<WallOfFlesh>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.WallOfFlesh"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<QueenSlime>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.QueenSlime"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<TheDestroyer>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TheDestroyer"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Retinazer>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TheTwins"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Spazmatism>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TheTwins"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<SkeletronPrime>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.SkeletronPrime"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Plantera>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Plantera"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Golem>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Golem"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<EmpressOfLight>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.EmpressOfLight"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<DukeFishron>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.DukeFishron"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Betsy>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Betsy"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<LunaticCultist>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.LunaticCultist"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<MoonLord>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.MoonLord"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Dreadnautilus>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Dreadnautilus"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Mothron>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Mothron"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<Pumpking>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.Pumpking"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<IceQueen>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.IceQueen"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<MartianSaucer>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.MartianSaucer"));
				censusMod.Call("TownNPCCondition", ModContent.NPCType<TorchGod>(), Language.GetTextValue($"Mods.BossesAsNPCs.CrossMod.Census.TorchGod"));
			}
			if (ModLoader.TryGetMod("DialogueTweak", out Mod dialogueTweak))
			{
				dialogueTweak.Call("ReplacePortrait",
					ModContent.NPCType<TorchGod>(),
					"BossesAsNPCs/NPCs/TownNPCs/TorchGod_Bestiary",
					() => true,
					() => Main.npc[NPC.FindFirstNPC(ModContent.NPCType<TorchGod>())].frame);
			}
		}

		//Adapted from absoluteAquarian's GraphicsLib
		public override object Call(params object[] args)
		{
			if (args is null)
				throw new ArgumentNullException(nameof(args));

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
					return NPCs.NPCHelper.StatusShop1();
				case "GetStatusShop2":
					Logger.Warn($"Function \"{function}\" is obsolete. Please use one of the \"AddToShop\" calls.");
					return NPCs.NPCHelper.StatusShop2();
				case "CanSpawn":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
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
				case "GetCondition":
					CheckArgsLength(2, new string[] { args[0].ToString(), args[1].ToString() });
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
				case "AddToShop":
					switch (args[1].ToString())
					{
						case "DefaultPrice":
							CheckArgsLength(5, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString() });
							// string npc, int item, Condition condition
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4]);
						case "CustomPrice":
							CheckArgsLength(6, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString() });
							// string npc, int item, Condition condition, int customPrice
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (int)args[5]);
						case "WithDiv":
							CheckArgsLength(6, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString() });
							// string npc, int item, Condition condition, float priceDiv
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (float)args[5]);
						case "WithDivAndMulti":
							CheckArgsLength(7, new string[] { args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString(), args[4].ToString(), args[5].ToString(), args[6].ToString() });
							// string npc, int item, Condition condition, float priceDiv, float priceMulti
							return NPCs.SetupShops.SetShopItem(args[2].ToString(), (int)args[3], (List<Condition>)args[4], (float)args[5], (float)args[6]);
						default:
							throw new ArgumentException($"Argument \"{args[1]}\" of Function \"{function}\" is not defined by Bosses As NPCs");
					}
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