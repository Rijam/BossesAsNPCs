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
        public static bool downedBetsy = false;
        public static bool downedDungeonGuardian = false;
        public static bool downedDarkMage = false;
        public static bool downedOgre = false;
        public static bool downedGoblinSummoner = false;

        //internal static bool[] boostTownNPCSpawnRate;

        public override void OnWorldLoad()
        {
            downedBetsy = false;
            downedDungeonGuardian = false;
            downedDarkMage = false;
            downedOgre = false;
            downedGoblinSummoner = false;

            //boostTownNPCSpawnRate = new bool[Main.netMode == NetmodeID.Server ? 255 : 1];
        }

        public override void OnWorldUnload()
        {
            downedBetsy = false;
            downedDungeonGuardian = false;
            downedDarkMage = false;
            downedOgre = false;
            downedGoblinSummoner = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
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
        }
        public override void LoadWorldData(TagCompound tag)
        {
            downedBetsy = tag.ContainsKey("downedBetsy");
            downedDungeonGuardian = tag.ContainsKey("downedDungeonGuardian");
            downedDarkMage = tag.ContainsKey("downedDarkMage");
            downedOgre = tag.ContainsKey("downedOgre");
            downedGoblinSummoner = tag.ContainsKey("downedGoblinSummoner");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[1] = downedBetsy;
            flags[2] = downedDungeonGuardian;
            flags[3] = downedDarkMage;
            flags[4] = downedOgre;
            flags[5] = downedGoblinSummoner;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedBetsy = flags[1];
            downedDungeonGuardian = flags[2];
            downedDarkMage = flags[3];
            downedOgre = flags[4];
            downedGoblinSummoner = flags[5];
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