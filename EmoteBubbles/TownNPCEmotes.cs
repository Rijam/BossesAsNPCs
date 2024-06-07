using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace BossesAsNPCs.EmoteBubbles
{
	// This abstract class is used for town NPC emotes quick setup.
	public abstract class ModTownEmote : ModEmoteBubble
	{
		// Redirecting texture path.
		public override string Texture => "BossesAsNPCs/EmoteBubbles/TownNPCEmotes";

		public override void SetStaticDefaults()
		{
			// Add NPC emotes to "Town" category.
			AddToCategory(EmoteID.Category.Town);
		}

		/// <summary>
		/// Which row of the sprite sheet is this NPC emote in?
		/// This is used to help get the correct frame rectangle for different emotes.
		/// </summary>
		public virtual int Row => 0;

		// You should decide the frame rectangle yourself by these two methods.
		public override Rectangle? GetFrame()
		{
			return new Rectangle(EmoteBubble.frame * 34, 28 * Row, 34, 28);
		}

		// Do note that you should never use EmoteBubble instance as the GetFrame() method above
		// in "Emote Menu Methods" (methods with -InEmoteMenu suffix).
		// Because in that case the value of EmoteBubble is always null.
		public override Rectangle? GetFrameInEmoteMenu(int frame, int frameCounter)
		{
			return new Rectangle(frame * 34, 28 * Row, 34, 28);
		}
	}

	// This is a showcase of using the same texture for different emotes.
	// Command names of these classes are defined using .hjson files in the Localization/ folder.
	public class KingSlimeEmote : ModTownEmote
	{
		public override int Row => 0;
	}

	public class EyeOfCthulhuEmote : ModTownEmote
	{
		public override int Row => 1;
	}

	public class EaterOfWorldsEmote : ModTownEmote
	{
		public override int Row => 2;
	}

	public class BrainOfCthulhuEmote : ModTownEmote
	{
		public override int Row => 3;
	}

	public class QueenBeeEmote : ModTownEmote
	{
		public override int Row => 4;
	}

	public class SkeletronEmote : ModTownEmote
	{
		public override int Row => 5;
	}

	public class DeerclopsEmote : ModTownEmote
	{
		public override int Row => 6;
	}
	public class WallOfFleshEmote : ModTownEmote
	{
		public override int Row => 7;
	}
	public class QueenSlimeEmote : ModTownEmote
	{
		public override int Row => 8;
	}
	public class TheDestoryerEmote : ModTownEmote
	{
		public override int Row => 9;
	}
	public class RetinazerEmote : ModTownEmote
	{
		public override int Row => 10;
	}
	public class SpazmatismEmote : ModTownEmote
	{
		public override int Row => 11;
	}
	public class SkeletronPrimeEmote : ModTownEmote
	{
		public override int Row => 12;
	}
	public class PlanteraEmote : ModTownEmote
	{
		public override int Row => 13;
	}
	public class GolemEmote : ModTownEmote
	{
		public override int Row => 14;
	}
	public class EmpressOfLightEmote : ModTownEmote
	{
		public override int Row => 15;
	}
	public class DukeFishronEmote : ModTownEmote
	{
		public override int Row => 16;
	}
	public class BetsyEmote : ModTownEmote
	{
		public override int Row => 17;
	}
	public class LunaticCultistEmote : ModTownEmote
	{
		public override int Row => 18;
	}
	public class MoonLordEmote : ModTownEmote
	{
		public override int Row => 19;
	}
	public class DreadnautilusEmote : ModTownEmote
	{
		public override int Row => 20;
	}
	public class MothronEmote : ModTownEmote
	{
		public override int Row => 21;
	}
	public class PumpkingEmote : ModTownEmote
	{
		public override int Row => 22;
	}
	public class IceQueenEmote : ModTownEmote
	{
		public override int Row => 23;
	}
	public class MartianSaucerEmote : ModTownEmote
	{
		public override int Row => 24;
	}
	public class TorchGodEmote : ModTownEmote
	{
		public override int Row => 25;
	}

	public class DarkMageEmote : ModTownEmote
	{
		public override int Row => 26;
	}
	public class OgreEmote : ModTownEmote
	{
		public override int Row => 27;
	}
	public class MourningWoodEmote : ModTownEmote
	{
		public override int Row => 28;
	}
	public class EverscreamEmote : ModTownEmote
	{
		public override int Row => 29;
	}
	public class SantaNK1Emote : ModTownEmote
	{
		public override int Row => 30;
	}
}
