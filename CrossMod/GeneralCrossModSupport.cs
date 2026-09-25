using BossesAsNPCs.Items;
using BossesAsNPCs.NPCs.TownNPCs;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BossesAsNPCs.CrossMod
{
	public class GeneralCrossModSupport : ModSystem
	{
		// For shops, see NPCs/SetupShops.cs
		// For happiness, see NPCs/NPCHappiness.cs

		public override void Load()
		{
			// Wikithis support
			if (ModLoader.TryGetMod("Wikithis", out Mod wikithis) && InternalCrossModSupportList.Wikithis && !Main.dedServ)
			{
				wikithis.Call("AddModURL", this, "https://terrariamods.wiki.gg/wiki/Bosses_As_NPCs/{}");
				wikithis.Call("AddWikiTexture", this, ModContent.Request<Texture2D>("BossesAsNPCs/icon_small"));
			}

			// Item Checklist Blacklist support
			if (ModLoader.TryGetMod("ItemCheckBlacklist", out Mod itemCheckBlacklist) && InternalCrossModSupportList.ItemCheckBlacklist)
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

		}

		public override void PostSetupContent()
		{
			// Dialogue Panel Rework support
			if (ModLoader.TryGetMod("DialogueTweak", out Mod dialogueTweak) && InternalCrossModSupportList.DialogueTweak)
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

			// Dialect support
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

			// Register the Town NPCs from WackyNPCs to live in evil biomes.
			if (ModLoader.TryGetMod("WackyNPCs", out Mod wackyNPCs) && InternalCrossModSupportList.WackyNPCs && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				if (wackyNPCs.TryFind<ModNPC>("Corru", out ModNPC corru)) // Corruption Fangirl
				{
					Mod.Call("AddTownNPCCanLiveInCorruption", corru.Type);
				}
				if (wackyNPCs.TryFind<ModNPC>("Crim", out ModNPC crim)) // Crimson Fangirl
				{
					Mod.Call("AddTownNPCCanLiveInCrimson", crim.Type);
				}
				if (wackyNPCs.TryFind<ModNPC>("CKBarkeep", out ModNPC ckBarkeep)) // Crimson Barkeep
				{
					Mod.Call("AddTownNPCCanLiveInCrimson", ckBarkeep.Type);
				}
				if (wackyNPCs.TryFind<ModNPC>("Flann", out ModNPC flann)) // Flann the Lucky
				{
					Mod.Call("AddTownNPCCanLiveInCrimson", flann.Type);
				}
				if (wackyNPCs.TryFind<ModNPC>("CyberFangirl", out ModNPC cyberFangirl)) // Cyber Fangirl
				{
					Mod.Call("AddTownNPCCanLiveInDungeon", cyberFangirl.Type);
				}
			}
			// Register the Town NPCs from MoreTownsfolk to live in evil biomes.
			// MoreTownsfolk already does this, but I'm adding them to my lists for completeness.
			if (ModLoader.TryGetMod("MoreTownsfolk", out Mod moreTownsfolk) && InternalCrossModSupportList.MoreTownsfolk && ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport)
			{
				if (moreTownsfolk.TryFind<ModNPC>("Harvester", out ModNPC harvester)) // Harvester
				{
					Mod.Call("AddTownNPCCanLiveInCrimson", harvester.Type);
				}
				if (moreTownsfolk.TryFind<ModNPC>("Occultist", out ModNPC occultist)) // Occultist
				{
					Mod.Call("AddTownNPCCanLiveInCorruption", occultist.Type);
				}
			}
		}
	}
}
