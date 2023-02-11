using Microsoft.Xna.Framework;
using Steamworks;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.PlayerDrawLayer;
using static Terraria.Player;

namespace BossesAsNPCs.Items
{
	public class PlanterasAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plantera's Axe");
			// Tooltip.SetDefault("[c/403638:Used by the Plantera Town NPC]\n[c/403638:because the normal The Axe is upside-down]");
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.DefaultToGuitar(24, 24);
			Item.maxStack = 1;
			Item.value = 10000;
			Item.rare = ItemRarityID.Yellow;
			Item.scale = 0.75f;
		}
		public override bool? UseItem(Player player)
		{
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				Vector2 playerPos = new(player.position.X + player.width * 0.5f, player.position.Y + player.height * 0.5f);
				float mousePosX = Main.mouseX + Main.screenPosition.X - playerPos.X;
				float mousePosY = Main.mouseY + Main.screenPosition.Y - playerPos.Y;
				float pitch = (float)Math.Sqrt(mousePosX * mousePosX + mousePosY * mousePosY);
				float adjustedScreenHeight = Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
				pitch /= adjustedScreenHeight / 2f;
				if (pitch > 1f)
				{
					pitch = 1f;
				}

				pitch = pitch * 2f - 1f;
				Math.Clamp(pitch, -1f, 1f);

				pitch = (float)Math.Round(pitch * Player.musicNotes);
				pitch = (Main.musicPitch = pitch / (float)Player.musicNotes);
				SoundEngine.PlaySound(SoundID.Item47 with { MaxInstances = 2 }, player.position); // Changed MaxInstances to 2 (from 1) to make spam clicking sound a little better.
				NetMessage.SendData(MessageID.InstrumentSound, -1, -1, null, player.whoAmI, pitch);
				return false;
			}
			return null;
		}

		public override void HoldItemFrame(Player player)
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
	}
}