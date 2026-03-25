using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.Mechdusa
{
	[AutoloadEquip(EquipType.Head)]
	public class MdCostumeHeadpiece : VanityBase
	{
		public static Asset<Texture2D> RezEye;
		public static Asset<Texture2D> SpazEye;
		public static Asset<Texture2D> EyeTether;

		public override void Load()
		{
			base.Load();
			RezEye = ModContent.Request<Texture2D>($"{GetType().Namespace.Replace('.', '/')}/RezEye");
			SpazEye = ModContent.Request<Texture2D>($"{GetType().Namespace.Replace('.', '/')}/SpazEye");
			EyeTether = ModContent.Request<Texture2D>($"{GetType().Namespace.Replace('.', '/')}/EyeTether");
		}

		/// <summary>
		/// Creates the DrawData and adds them to the draw cache for one floating eye with a tether.
		/// </summary>
		/// <param name="drawInfo">PlayerDrawLayer.Draw drawInfo</param>
		/// <param name="frame">The frame for the animation.</param>
		/// <param name="eyePos">The position of the eye to draw.</param>
		/// <param name="socketPos">The position of the socket on the player's face.</param>
		/// <param name="eyeToDraw">The Asset Texture2D of the eye to draw.</param>
		public static void DrawATetheredEye(ref PlayerDrawSet drawInfo, ref int frame, Vector2 eyePos, Vector2 socketPos, Asset<Texture2D> eyeToDraw)
		{
			Player player = drawInfo.drawPlayer;

			if (player.miscCounter % 10 == 0) // Every 10 ticks, increase the frame counter.
			{
				frame = ++frame % 3; // Increase the frame with a range of 0 - 2.
			}

			Vector2 worldPos = eyePos - socketPos;
			Vector2 tetherPos = Vector2.Lerp(eyePos, socketPos, 0.5f); // Get the midpoint between the eye position and socket position.
			float thickness = worldPos.Length() / 32f; // 32 is a magic number that looks good for this sprite.
			float stretch = MathHelper.Clamp(thickness, 0.5f, 3f);
			thickness = MathHelper.Clamp(thickness, 0.75f, 1f); // Clamp the thickness so it doesn't get too thin or thick.

			DrawData FrontEyeDrawData = new(
				eyeToDraw.Value,
				eyePos - new Vector2(eyeToDraw.Size().X / 2f, eyeToDraw.Size().Y / 8f) - Main.screenPosition, // Move the eye to draw in the center of the position instead of the top left.
				eyeToDraw.Frame(1, 3, 0, frame),
				drawInfo.colorArmorHead,
				player.headRotation,
				Vector2.Zero,
				1f,
				drawInfo.playerEffect
				);
			DrawData EyeTetherDrawData = new(
				EyeTether.Value,
				tetherPos - Main.screenPosition,
				null,
				drawInfo.colorArmorHead,
				worldPos.ToRotation() + MathHelper.PiOver2,
				EyeTether.Size() / 2f,
				new Vector2(thickness, stretch),
				drawInfo.playerEffect
				);
			drawInfo.DrawDataCache.Add(EyeTetherDrawData);
			drawInfo.DrawDataCache.Add(FrontEyeDrawData);
			/* Debugging the locations
			DrawData socketDraw = new(
				TextureAssets.MagicPixel.Value,
				socketPos - new Vector2(2, 2) - Main.screenPosition,
				new Rectangle(0, 0, 4, 4),
				Color.Magenta,
				0f,
				Vector2.Zero,
				1f,
				drawInfo.playerEffect
				);
			DrawData eyeDraw = new(
				TextureAssets.MagicPixel.Value,
				eyePos - new Vector2(2, 2) - Main.screenPosition,
				new Rectangle(0, 0, 4, 4),
				Color.Magenta,
				0f,
				Vector2.Zero,
				1f,
				drawInfo.playerEffect
				);
			DrawData tetherDraw = new(
				TextureAssets.MagicPixel.Value,
				tetherPos - new Vector2(2, 2) - Main.screenPosition,
				new Rectangle(0, 0, 4, 4),
				Color.Magenta,
				0f,
				Vector2.Zero,
				1f,
				drawInfo.playerEffect
				);
			drawInfo.DrawDataCache.Add(socketDraw);
			drawInfo.DrawDataCache.Add(eyeDraw);
			drawInfo.DrawDataCache.Add(tetherDraw);
			*/
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MdCostumeBodypiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorGlowmaskBody.RegisterData(Item.bodySlot, Color.White);
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
			}
		}
	}

	public class MdCostumeHeadpiecePlayerDrawLayerFront : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.FrontAccFront);
		}
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "MdCostumeHeadpiece", EquipType.Head);
		}

		private int frame = 0;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;
			float playerMountYOffset = (player.MountedCenter.Y - player.Center.Y);

			// The position of the eye that is drawn in front of the player's head.
			Vector2 frontEyePos = player.Center - new Vector2((12 - player.MountXOffset) * player.direction, 28 - player.gfxOffY - playerMountYOffset);
			frontEyePos -= player.velocity; // Make the eye fall behind the player when the player is moving.
			
			// The position of the eye socket with a little bias towards the front and down.
			// The tether will connect to the top left of this position, so pushing it down and forward makes it appear more centered in the eye socket.
			Vector2 eyeSocketPos = player.Center - new Vector2((-3.5f - player.MountXOffset) * player.direction, 11 - player.gfxOffY - playerMountYOffset);

			// Make it so the Rez eye is always on the left side and the Spaz eye is always on the right side.
			Asset<Texture2D> eyeToUse = player.direction == 1 ? MdCostumeHeadpiece.RezEye : MdCostumeHeadpiece.SpazEye;

			MdCostumeHeadpiece.DrawATetheredEye(ref drawInfo, ref frame, frontEyePos, eyeSocketPos, eyeToUse);
		}
	}

	public class MdCostumeHeadpiecePlayerDrawLayerBack : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new BeforeParent(PlayerDrawLayers.BackAcc);
		}
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "MdCostumeHeadpiece", EquipType.Head);
		}

		private int frame = 1;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;

			float playerMountYOffset = (player.MountedCenter.Y - player.Center.Y);

			// The position of the eye that is drawn in behind of the player's head.
			Vector2 backEyePos = player.Center - new Vector2((-6 - player.MountXOffset) * player.direction, 28 - player.gfxOffY - playerMountYOffset);
			backEyePos -= player.velocity * 1.25f; // Make the eye fall behind the player when the player is moving.
			
			// The position of the eye socket with a little bias towards the front and down.
			// The tether will connect to the top left of this position, so pushing it down and forward makes it appear more centered in the eye socket.
			Vector2 eyeSocketPos = player.Center - new Vector2((-7f - player.MountXOffset) * player.direction, 11 - player.gfxOffY - playerMountYOffset);

			// Make it so the Rez eye is always on the left side and the Spaz eye is always on the right side.
			Asset<Texture2D> eyeToUse = player.direction == 1 ? MdCostumeHeadpiece.SpazEye : MdCostumeHeadpiece.RezEye;

			MdCostumeHeadpiece.DrawATetheredEye(ref drawInfo, ref frame, backEyePos, eyeSocketPos, eyeToUse);
		}
	}
}