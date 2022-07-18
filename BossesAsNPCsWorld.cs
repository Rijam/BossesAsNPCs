using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossesAsNPCs.NPCs.TownNPCs;

namespace BossesAsNPCs
{ 
    public class BossesAsNPCsWorld : ModSystem
    {
        public static bool daytimeEoLDefeated = false;
        public static bool downedBetsy = false;
        public static bool downedDungeonGuardian = false;
        public static bool downedDarkMage = false;
        public static bool downedOgre = false;
        public static bool downedGoblinSummoner = false;
        public static bool downedMothron = false;
        public static bool downedDreadnautilus = false;

        public override void OnWorldLoad()
        {
            daytimeEoLDefeated = false;
            downedBetsy = false;
            downedDungeonGuardian = false;
            downedDarkMage = false;
            downedOgre = false;
            downedGoblinSummoner = false;
            downedMothron = false;
            downedDreadnautilus = false;
        }

        public override void OnWorldUnload()
        {
            daytimeEoLDefeated = false;
            downedBetsy = false;
            downedDungeonGuardian = false;
            downedDarkMage = false;
            downedOgre = false;
            downedGoblinSummoner = false;
            downedMothron = false;
            downedDreadnautilus = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (daytimeEoLDefeated)
			{
                tag["daytimeEoLDefeated"] = true;
			}
            if (downedBetsy)
            {
                tag["downedBetsy"] = true;
            }
            if (downedDungeonGuardian)
            {
                tag["downedDungeonGuardian"] = true;
            }
            if (downedDarkMage)
            {
                tag["downedDarkMage"] = true;
            }
            if (downedOgre)
            {
                tag["downedOgre"] = true;
            }
            if (downedGoblinSummoner)
            {
                tag["downedGoblinSummoner"] = true;
            }
            if (downedMothron)
            {
                tag["downedMothron"] = true;
            }
            if (downedDreadnautilus)
            {
                tag["downedDreadnautilus"] = true;
            }
        }
        public override void LoadWorldData(TagCompound tag)
        {
            daytimeEoLDefeated = tag.ContainsKey("daytimeEoLDefeated");
            downedBetsy = tag.ContainsKey("downedBetsy");
            downedDungeonGuardian = tag.ContainsKey("downedDungeonGuardian");
            downedDarkMage = tag.ContainsKey("downedDarkMage");
            downedOgre = tag.ContainsKey("downedOgre");
            downedGoblinSummoner = tag.ContainsKey("downedGoblinSummoner");
            downedMothron = tag.ContainsKey("downedMothron");
            downedDreadnautilus = tag.ContainsKey("downedDreadnautilus");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = daytimeEoLDefeated;
            flags[1] = downedBetsy;
            flags[2] = downedDungeonGuardian;
            flags[3] = downedDarkMage;
            flags[4] = downedOgre;
            flags[5] = downedGoblinSummoner;
            flags[6] = downedMothron;
            flags[7] = downedDreadnautilus;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            daytimeEoLDefeated = flags[0];
            downedBetsy = flags[1];
            downedDungeonGuardian = flags[2];
            downedDarkMage = flags[3];
            downedOgre = flags[4];
            downedGoblinSummoner = flags[5];
            downedMothron = flags[6];
            downedDreadnautilus = flags[7];
        }
        public override void PreUpdateWorld()
        {
            if (ModContent.GetInstance<BossesAsNPCsConfigServer>().BoostTownNPCRates)
            {
                Main.checkForSpawns += 81;
            }
        }
    }
}