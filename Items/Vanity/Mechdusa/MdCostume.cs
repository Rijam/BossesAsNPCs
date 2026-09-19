using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.Mechdusa
{
	[AutoloadEquip(EquipType.Head)]
	public class MdCostumeHeadpiece : VanityBase
	{
		internal static Asset<Texture2D> RezEye;
		internal static Asset<Texture2D> SpazEye;
		internal static Asset<Texture2D> EyeTether;

		public override void Load()
		{
			base.Load();
			RezEye = ModContent.Request<Texture2D>($"{GetType().Namespace.Replace('.', '/')}/RezEye");
			SpazEye = ModContent.Request<Texture2D>($"{GetType().Namespace.Replace('.', '/')}/SpazEye");
			EyeTether = ModContent.Request<Texture2D>($"{GetType().Namespace.Replace('.', '/')}/EyeTether");
		}

		public override void Unload()
		{
			base.Unload();
			RezEye.Dispose();
			SpazEye.Dispose();
			EyeTether.Dispose();
		}

		public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
		{
			Player player = drawInfo.drawPlayer;
			float playerMountYOffset = (player.MountedCenter.Y - player.Center.Y);

			// Back eye

			// The position of the eye that is drawn in behind of the player's head.
			Vector2 backEyePos = player.Center - new Vector2((-6 - player.MountXOffset) * player.direction, (28 * player.gravDir) - player.gfxOffY - playerMountYOffset);
			backEyePos -= player.velocity * 1.25f; // Make the eye fall behind the player when the player is moving.

			// The position of the eye socket with a little bias towards the front and down.
			// The tether will connect to the top left of this position, so pushing it down and forward makes it appear more centered in the eye socket.
			Vector2 backEyeSocketPos = player.Center - new Vector2((-7f - player.MountXOffset) * player.direction, (11 * player.gravDir) - player.gfxOffY - playerMountYOffset);

			// Make it so the Rez eye is always on the left side and the Spaz eye is always on the right side.
			Asset<Texture2D> eyeToUse = player.direction == 1 ? MdCostumeHeadpiece.SpazEye : MdCostumeHeadpiece.RezEye;

			MdCostumeHeadpiece.DrawATetheredEye(ref drawInfo, frameTimerStart: 10, backEyePos, backEyeSocketPos, eyeToUse);

			// Draw the normal headpiece
			drawInfo.DrawDataCache.Add(drawData);

			// Front eye

			// The position of the eye that is drawn in front of the player's head.
			Vector2 frontEyePos = player.Center - new Vector2((12 - player.MountXOffset) * player.direction, (28 * player.gravDir) - player.gfxOffY - playerMountYOffset);
			frontEyePos -= player.velocity; // Make the eye fall behind the player when the player is moving.

			// The position of the eye socket with a little bias towards the front and down.
			// The tether will connect to the top left of this position, so pushing it down and forward makes it appear more centered in the eye socket.
			Vector2 frontEyeSocketPos = player.Center - new Vector2((-3.5f - player.MountXOffset) * player.direction, (11 * player.gravDir) - player.gfxOffY - playerMountYOffset);

			// Make it so the Rez eye is always on the left side and the Spaz eye is always on the right side.
			eyeToUse = player.direction == 1 ? MdCostumeHeadpiece.RezEye : MdCostumeHeadpiece.SpazEye;

			MdCostumeHeadpiece.DrawATetheredEye(ref drawInfo, frameTimerStart: 0, frontEyePos, frontEyeSocketPos, eyeToUse);

			return false; // Return false because we already drew the regular headpiece.
		}

		/*
		private static readonly MethodInfo Method_UICharacter_GetPlayerPosition = typeof(Terraria.GameContent.UI.Elements.UICharacter).GetMethod("GetPlayerPosition", BindingFlags.NonPublic | BindingFlags.Instance)!;

		internal static Vector2 Invoke_UICharacter_GetPlayerPosition(UICharacter instance, ref CalculatedStyle dimensions)
		{
			// MethodInfo Method_ShopHelper_AddHappinessReportText = instance.GetType().GetMethod("AddHappinessReportText", BindingFlags.NonPublic | BindingFlags.Instance);
			return (Vector2)Method_UICharacter_GetPlayerPosition.Invoke(instance, [dimensions]);
		}

		public static UICharacterSelect Get_Main__characterSelectMenu()
		{
			FieldInfo field = typeof(Terraria.Main).GetField("_characterSelectMenu", BindingFlags.NonPublic | BindingFlags.Static);
			return (UICharacterSelect)field.GetValue(Main.instance);
		}

		public static UIList Get_UICharacterSelect__playerList(UICharacterSelect instance)
		{
			FieldInfo field = typeof(Terraria.GameContent.UI.States.UICharacterSelect).GetField("_playerList", BindingFlags.NonPublic | BindingFlags.Instance);
			return (UIList)field.GetValue(instance);
		}

		public static UICharacter Get_UICharacterListItem__playerPanel(UICharacterListItem instance)
		{
			FieldInfo field = typeof(Terraria.GameContent.UI.Elements.UICharacterListItem).GetField("_playerPanel", BindingFlags.NonPublic | BindingFlags.Instance);
			return (UICharacter)field.GetValue(instance);
		}
		*/

		/// <summary>
		/// Creates the DrawData and adds them to the draw cache for one floating eye with a tether.
		/// </summary>
		/// <param name="drawInfo">PlayerDrawLayer.Draw drawInfo</param>
		/// <param name="frameTimerStart">The frame offset to give variation (so both eyes aren't on the same frame).</param>
		/// <param name="eyePos">The position of the eye to draw.</param>
		/// <param name="socketPos">The position of the socket on the player's face.</param>
		/// <param name="eyeToDraw">The Asset Texture2D of the eye to draw.</param>
		public static void DrawATetheredEye(ref PlayerDrawSet drawInfo, int frameTimerStart, Vector2 eyePos, Vector2 socketPos, Asset<Texture2D> eyeToDraw)
		{
			Player player = drawInfo.drawPlayer;

			// This works, but Mannequins and hat racks increase the frame in addition to the player.
			// The framerate gets super fast with just a couple of mannequins around.
			// Every 10 ticks, increase the frame counter.
			//if (player.miscCounter % 10 == 0/* && !player.isDisplayDollOrInanimate && !player.isHatRackDoll*/)
			//{
			//	frame = ++frame % 3; // Increase the frame with a range of 0 - 2.
			//}

			// Choose a frame directly from miscCounter.
			// miscCounter counts from 0 to 299.
			// Divide by ten to make the number increase every 10 frames.
			// Modulo 3 to get 0, 1, or 2 for the frames.
			// frameTimerStart is an offset to give variation (so both eyes aren't on the same frame).
			// For some reason the hat racks animate faster than mannequins and players, but whatever.
			Rectangle eyeFrame = eyeToDraw.Frame(1, 3, 0, (player.miscCounter + frameTimerStart) / 10 % 3);

			/*
			if (Main.menuMode == 888) // Character select menu.
			{
				UIList uiList = Get_UICharacterSelect__playerList(Get_Main__characterSelectMenu());
				foreach (UICharacterListItem charListItem in uiList.Cast<UICharacterListItem>())
				{
					UICharacter uiCharacter = Get_UICharacterListItem__playerPanel(charListItem);
					CalculatedStyle dim = uiCharacter.GetDimensions();
					Vector2 dollPos = Invoke_UICharacter_GetPlayerPosition(uiCharacter, ref dim);
					//Vector2 menuOffset = new(100, 100);
					eyePos += dollPos;
					socketPos += dollPos;
				}
			}
			*/

			Vector2 worldPos = eyePos - socketPos;
			Vector2 tetherPos = Vector2.Lerp(eyePos, socketPos, 0.5f); // Get the midpoint between the eye position and socket position.
			float thickness = worldPos.Length() / 32f; // 32 is a magic number that looks good for this sprite.
			float stretch = MathHelper.Clamp(thickness, 0.5f, 3f);
			thickness = MathHelper.Clamp(thickness, 0.75f, 1f); // Clamp the thickness so it doesn't get too thin or thick.

			DrawData FrontEyeDrawData = new(
				eyeToDraw.Value,
				eyePos - new Vector2(eyeToDraw.Size().X / 2f, eyeToDraw.Size().Y / 8f) - Main.screenPosition, // Move the eye to draw in the center of the position instead of the top left.
				eyeFrame,
				drawInfo.colorArmorHead,
				player.headRotation,
				Vector2.Zero,
				1f,
				drawInfo.playerEffect
				)
			{
				shader = drawInfo.cHead
			};
			DrawData EyeTetherDrawData = new(
				EyeTether.Value,
				tetherPos - Main.screenPosition,
				null,
				drawInfo.colorArmorHead,
				worldPos.ToRotation() + MathHelper.PiOver2,
				EyeTether.Size() / 2f,
				new Vector2(thickness, stretch),
				drawInfo.playerEffect
				)
			{
				shader = drawInfo.cHead
			};
			drawInfo.DrawDataCache.Add(EyeTetherDrawData);
			drawInfo.DrawDataCache.Add(FrontEyeDrawData);
			//Debugging the locations
			/*
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

	// Superseded by ModItem.ModifyEquipTextureDraw
	/*
	public class MdCostumeHeadpiecePlayerDrawLayerFront : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.FrontAccBack);
		}
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || (drawPlayer.invis && !drawPlayer.isHatRackDoll) || drawPlayer.head == -1)
			{
				return false;
			}
			if (drawInfo.hideEntirePlayer || drawPlayer.mount.Active && MountID.Sets.PlayerIsHidden[drawPlayer.mount.Type])
			{
				return false;
			}
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "MdCostumeHeadpiece", EquipType.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;
			float playerMountYOffset = (player.MountedCenter.Y - player.Center.Y);

			// The position of the eye that is drawn in front of the player's head.
			Vector2 frontEyePos = player.Center - new Vector2((12 - player.MountXOffset) * player.direction, (28 * player.gravDir) - player.gfxOffY - playerMountYOffset);
			frontEyePos -= player.velocity; // Make the eye fall behind the player when the player is moving.

			// The position of the eye socket with a little bias towards the front and down.
			// The tether will connect to the top left of this position, so pushing it down and forward makes it appear more centered in the eye socket.
			Vector2 eyeSocketPos = player.Center - new Vector2((-3.5f - player.MountXOffset) * player.direction, (11 * player.gravDir) - player.gfxOffY - playerMountYOffset);

			// Make it so the Rez eye is always on the left side and the Spaz eye is always on the right side.
			Asset<Texture2D> eyeToUse = player.direction == 1 ? MdCostumeHeadpiece.RezEye : MdCostumeHeadpiece.SpazEye;

			MdCostumeHeadpiece.DrawATetheredEye(ref drawInfo, frameTimerStart: 0, frontEyePos, eyeSocketPos, eyeToUse);
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
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || (drawPlayer.invis && !drawPlayer.isHatRackDoll) || drawPlayer.head == -1)
			{
				return false;
			}
			if (drawInfo.hideEntirePlayer || drawPlayer.mount.Active && MountID.Sets.PlayerIsHidden[drawPlayer.mount.Type])
			{
				return false;
			}
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "MdCostumeHeadpiece", EquipType.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;

			float playerMountYOffset = (player.MountedCenter.Y - player.Center.Y);

			// The position of the eye that is drawn in behind of the player's head.
			Vector2 backEyePos = player.Center - new Vector2((-6 - player.MountXOffset) * player.direction, (28 * player.gravDir) - player.gfxOffY - playerMountYOffset);
			backEyePos -= player.velocity * 1.25f; // Make the eye fall behind the player when the player is moving.

			// The position of the eye socket with a little bias towards the front and down.
			// The tether will connect to the top left of this position, so pushing it down and forward makes it appear more centered in the eye socket.
			Vector2 eyeSocketPos = player.Center - new Vector2((-7f - player.MountXOffset) * player.direction, (11 * player.gravDir) - player.gfxOffY - playerMountYOffset);

			// Make it so the Rez eye is always on the left side and the Spaz eye is always on the right side.
			Asset<Texture2D> eyeToUse = player.direction == 1 ? MdCostumeHeadpiece.SpazEye : MdCostumeHeadpiece.RezEye;

			MdCostumeHeadpiece.DrawATetheredEye(ref drawInfo, frameTimerStart: 10, backEyePos, eyeSocketPos, eyeToUse);
		}
	}
	*/
}