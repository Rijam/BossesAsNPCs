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

namespace BossesAsNPCs.NPCs
{
	public class NPCHelper
	{
		public static string LoveText()
		{
			return "[c/b3f2b3:" + Language.GetTextValue("RandomWorldName_Noun.Love") + "]: ";
		}
		public static string LikeText()
		{
			return "[c/ddf2b3:Like]: ";
		}
		public static string DislikeText()
		{
			return "[c/f2e0b3:Dislike]: ";
		}
		public static string HateText()
		{
			return "[c/f2b5b3:" + Language.GetTextValue("RandomWorldName_Noun.Hate") + "]: ";
		}
		public static bool UnlockOWMusic()
		{
			return Main.Configuration.Get("UnlockMusicSwap", false);
		}

		private static bool shop1;
		private static bool shop2;

		public static void SetShop1(bool tOrF)
		{
			shop1 = tOrF;
		}
		public static void SetShop2(bool tOrF)
		{
			shop2 = tOrF;
		}
		public static bool StatusShop1()
		{
			return shop1;
		}
		public static bool StatusShop2()
		{
			return shop2;
		}
	}
}