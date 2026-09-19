using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items
{
	public class PlanterasAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plantera's Axe");
			// Tooltip.SetDefault("[c/403638:Used by the Plantera Town NPC]\n[c/403638:because the normal The Axe is upside-down]");
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.TheAxe;
		}
		public override void SetDefaults()
		{
			Item.DefaultToGuitar(24, 24);
			Item.value = 100000;
			Item.rare = ItemRarityID.Yellow;
			Item.scale = 0.75f;
		}

		public override void HoldItemFrame(Player player)
		{
			/*
			if (!Main.dedServ && !player.pulley)
			{
				player.itemLocation += new Vector2(-10 * player.direction, 5 * player.gravDir);
			}
			*/
		}

		public override void HoldStyle(Player player, Rectangle heldItemFrame)
		{
			if (!Main.dedServ && !player.pulley)
			{
				player.itemLocation += new Vector2(-10 * player.direction, 5 * player.gravDir);
			}
		}

		public override void UseItemFrame(Player player)
		{
			if (!Main.dedServ)
			{
				player.itemLocation += new Vector2(-10 * player.direction, 5 * player.gravDir);
			}
		}

		public override bool? UseItem(Player player)
		{
			// Adapted from the work I did on the Pianist's Glove from Rijam's Mod.
			int note = CalcNote(player);
			if (Main.mouseLeft && Main.mouseLeftRelease && note > 0)
			{
				PlayTheAxe(note, player);
				return false;
			}
			return null;
		}

		public override void HoldItem(Player player)
		{
			int note = CalcNote(player);
			if (note > 0)
			{
				CursorNotes(note, player);
			}
		}

		public static int CalcNote(Player player)
		{
			int playerPosX = (int)player.Center.X / 16;
			int playerPosY = (int)player.Center.Y / 16;
			if (WorldGen.InWorld(playerPosX, playerPosY))
			{
				float OneSixth = 1f / 6f;
				// This is different to how vanilla calculates the note based on the distance.
				// Vanilla also only allows 6 notes per octave instead of all 12.

				// Get the mouse position.
				float mousePosX = (float)Main.mouseX + Main.screenPosition.X - player.Center.X;
				float mousePosY = (float)Main.mouseY + Main.screenPosition.Y - player.Center.Y;

				// Main.NewText($"mousePosX {mousePosX} mousePosY {mousePosY} Main.Camera.ScaledSize.X {Main.Camera.ScaledSize.X}");

				// Calculate X and Y separately.
				// This means the distance becomes a rectangular area with diagonal steps instead of a perfect circle.
				// Normally that would be a problem for distance calculations, but in this application it actually makes it easier to get the correct note.
				//		___________
				//	   |--__   __--|
				//	   |  __-P-__  |
				//	   |--_______--|

				// At 100% zoom and a 1920px wide screen, this gives -960 to 959.
				float pitchX = (float)Math.Abs(mousePosX);
				// At 100% zoom and a 1080px height screen, this gives -540 to 549.
				float pitchY = (float)Math.Abs(mousePosY);

				// At 100% zoom and a 1920px wide screen, this gives a range from the player's center to 80% of the screen width which is 48 tiles (2 tiles per note).
				// At 200% zoom and a 1920px wide screen, it is 24 tiles (1 tile per note).
				pitchX /= (Main.Camera.ScaledSize.X / 2f) * 0.8f; // 80% of the screen is 1f.
				pitchY /= (Main.Camera.ScaledSize.Y / 2f) * 0.8f; // 80% of the screen is 1f.

				pitchX *= 4f;
				pitchX = Math.Clamp(pitchX, 0f, 4f); // Multiply and clamp to 4f so 80% of the screen is 4f.
				pitchY *= 4f;
				pitchY = Math.Clamp(pitchY, 0f, 4f); // Multiply and clamp to 4f so 80% of the screen is 4f.

				// Main.NewText($"pitchX {pitchX} pitchX/OneSixth {pitchX / OneSixth} {(int)(pitchX / OneSixth) + 1}");
				// Main.NewText($"pitchY {pitchY} pitchX/OneSixth {pitchY / OneSixth} {(int)(pitchY / OneSixth) + 1}");

				float combinedPitch = MathHelper.Max(pitchX, pitchY); // Take the one furthest from the player.

				// Pitch ranges from about -0.01 to 4f
				// Divide by 1/6 and cast to int to get 0 to 24. Add one for 1 to 25.
				// 1 -> C low
				// 13 -> C middle
				// 25 -> C high
				return ((int)(combinedPitch / OneSixth)) + 1;
			}
			return 0;
		}

		public static void PlayTheAxe(int noteValue, Player player)
		{
			float pitchToPlay = noteValue switch
			{
				1 => -0.5f,         // C low
				2 => -0.45833f,     // C#
				3 => -0.41667f,     // D
				4 => -0.375f,       // D#
				5 => -0.33333f,     // E
				6 => -0.29167f,     // F
				7 => -0.25f,        // F#
				8 => -0.20833f,     // G
				9 => -0.16667f,     // G#
				10 => -0.125f,      // A
				11 => -0.08333f,    // A#
				12 => -0.04167f,    // B
				13 => 0,            // C middle
				14 => 0.08333f,     // C#
				15 => 0.16667f,     // D
				16 => 0.25f,        // D#
				17 => 0.33333f,     // E
				18 => 0.41667f,     // F
				19 => 0.5f,         // F#
				20 => 0.58333f,     // G
				21 => 0.66667f,     // G#
				22 => 0.75f,        // A
				23 => 0.83333f,     // A#
				24 => 0.91667f,     // B
				25 => 1,            // C middle
				_ => 0
			};

			SoundStyle DistortionGuitar = new("Terraria/Sounds/Item_47")
			{
				Pitch = pitchToPlay,
				Volume = 2f,
				MaxInstances = 3
			};

			ModContent.GetInstance<BossesAsNPCs>().PlayNetworkSound(DistortionGuitar, player.position, player);
		}
		public static void CursorNotes(int noteValue, Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				Main.mouseText = true;

				string noteName = noteValue switch
				{
					1 => "C",   // C low
					2 => "C#",
					3 => "D",
					4 => "D#",
					5 => "E",
					6 => "F",
					7 => "F#",
					8 => "G",
					9 => "G#",
					10 => "A",
					11 => "A#",
					12 => "B",
					13 => "C",  // C middle
					14 => "C#",
					15 => "D",
					16 => "D#",
					17 => "E",
					18 => "F",
					19 => "F#",
					20 => "G",
					21 => "G#",
					22 => "A",
					23 => "A#",
					24 => "B",
					25 => "C",  // C high
					_ => "C"
				};

				Main.instance.MouseText($"{noteName}");
			}
		}
	}
}