using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using ReLogic.Content;

namespace BossesAsNPCs
{
	///	<summary>
	/// <br>Adapted from Clicker Class DrawLayers/HeadLayer.cs</br>
	/// <br>Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:</br>
	/// <br><code>ArmorUseGlowHead.RegisterData(Item.headSlot, new ArmorHeadLegsOptions(Texture + "_Head_Glowmask", Color.White, GlowMaskEffects.Flame2));</code></br>
	/// <br>The key value is the slot. Item.headSlot</br>
	/// <br>ArmorHeadLegsOptions Texture is the texture of the glowmask</br>
	/// <br>ArmorHeadLegsOptions Color is the draw color. This can be omitted and it will draw White.</br>
	/// <br>ArmorHeadLegsOptions Effects is the special effect. This can be omitted and defaults to None.</br>
	/// </summary>
	public class ArmorPlayerDrawLayerHead : PlayerDrawLayer
	{
		// slot, options
		private static Dictionary<int, ArmorHeadLegsOptions> GlowListHead { get; set; }

		/// <summary>
		/// Register the head piece to have a glow mask.
		/// </summary>
		/// <param name="headSlot">The key value is the slot. Item.headSlot</param>
		/// <param name="values"><br>ArmorHeadLegsOptions Texture is the texture of the glowmask</br>
		/// <br>ArmorHeadLegsOptions Color is the draw color. This can be omitted and it will draw White.</br>
		/// <br>ArmorHeadLegsOptions Effects is the special effect. This can be omitted and defaults to None.</br></param>
		public static void RegisterData(int headSlot, ArmorHeadLegsOptions values)
		{
			if (!GlowListHead.ContainsKey(headSlot))
			{
				GlowListHead.Add(headSlot, values);
			}
		}

		// Returning true in this property makes this layer appear on the minimap player head icon.
		public override bool IsHeadLayer => false;

		public override void Load()
		{
			GlowListHead = new Dictionary<int, ArmorHeadLegsOptions>();
		}

		public override void Unload()
		{
			GlowListHead.Clear();
			GlowListHead = null;
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead || drawPlayer.invis || drawPlayer.head == -1)
			{
				return false;
			}

			return true;
		}

		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;

			if (!GlowListHead.TryGetValue(drawPlayer.head, out ArmorHeadLegsOptions values))
			{
				return;
			}
			Asset<Texture2D> glowmask = ModContent.Request<Texture2D>(values.Texture);

			int numTimesToDraw = 1;

			ulong seed = 0;

			if (values.Effects == GlowMaskEffects.Flame || values.Effects == GlowMaskEffects.Flame2)
			{
				numTimesToDraw = 5;
				seed = Main.TileFrameSeed ^ (ulong)(((long)drawPlayer.position.Y << 32) | (uint)drawPlayer.position.X);
			}

			for (int i = 0; i < numTimesToDraw; i++)
			{
				Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
				Vector2 headVect = drawInfo.headVect;

				if (values.Effects == GlowMaskEffects.Flame)
				{
					float random1 = Utils.RandomInt(ref seed, -5, 6) * 0.05f;
					float random2 = Utils.RandomInt(ref seed, -5, 1) * 0.15f;
					drawPos += new Vector2(random1, random2);
				}
				if (values.Effects == GlowMaskEffects.Flame2)
				{
					float random1 = Utils.RandomInt(ref seed, -11, 11) * 0.05f;
					float random2 = Utils.RandomInt(ref seed, -5, 5) * 0.15f;
					drawPos += new Vector2(random1, random2);
				}

				Color color = drawPlayer.GetImmuneAlphaPure(new Color(values.Color.R, values.Color.G, values.Color.B), drawInfo.shadow);

				if (values.Effects == GlowMaskEffects.Flame2)
				{
					color = drawPlayer.GetImmuneAlphaPure(new Color(values.Color.R, values.Color.G, values.Color.B, 100), drawInfo.shadow);
				}

				DrawData drawData = new(
					glowmask.Value, // The texture to render.
					drawPos.Floor() + headVect, // Position to render at.
					new Rectangle(drawPlayer.bodyFrame.X, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), // Source rectangle.
					color * drawPlayer.stealth, // Color.
					drawPlayer.headRotation, // Rotation.
					headVect, // Origin. Uses the texture's center.
					1f, // Scale.
					drawInfo.playerEffect, // SpriteEffects.
					0) // 'Layer'. This is always 0 in Terraria.
					{
						shader = drawInfo.cHead //Shader applied aka dyes
					};

				// Queues a drawing of a sprite. Do not use SpriteBatch in drawlayers!
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public readonly struct ArmorHeadLegsOptions
	{
		public ArmorHeadLegsOptions(string texture)
		{
			Texture = texture;
			Color = Color.White;
			Effects = GlowMaskEffects.None;
		}
		public ArmorHeadLegsOptions(string texture, Color color)
		{
			Texture = texture;
			Color = color;
			Effects = GlowMaskEffects.None;
		}

		public ArmorHeadLegsOptions(string texture, Color color, GlowMaskEffects effects)
		{
			Texture = texture;
			Color = color;
			Effects = effects;
		}

		public string Texture { get; }
		public Color Color { get; }
		public GlowMaskEffects Effects { get; }
	}
	public enum GlowMaskEffects
	{
		/// <summary>No effect (default)</summary>
		None = 0,
		/// <summary>Draws a few times each with a slight offset to make it looks like fire</summary>
		Flame = 1,
		/// <summary>Similar to Flame, but with 100 alpha and slightly different randomness.</summary>
		Flame2 = 2
	}
}