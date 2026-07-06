using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs
{
	/// <summary>
	/// <br>Adapted from Clicker Class Core/BodyGlowmaskPlayer.cs</br>
	/// <br>Usage: In the item's SetStaticDefaults(), Check for !Main.dedServ first, then add:</br>
	/// <br><code>ArmorGlowmaskBody.RegisterData(Item.bodySlot, Color.White);</code></br>
	/// <br>The key value is the slot. Item.bodySlot</br>
	/// <br>The second value can be any color</br>
	/// </summary>
	public class ArmorGlowmaskBody : ModPlayer
	{
		//slot, color
		private static Dictionary<int, Color> GlowListBody { get; set; }

		/// <summary>
		/// Register this body piece to have a glow mask.
		/// </summary>
		/// <param name="bodySlot">The key value is the slot. Item.bodySlot</param>
		/// <param name="color">The color is the color to draw</param>
		public static void RegisterData(int bodySlot, Color color)
		{
			GlowListBody.TryAdd(bodySlot, color);
		}

		public override void Load()
		{
			GlowListBody = new();
		}

		public override void Unload()
		{
			GlowListBody.Clear();
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			if (!GlowListBody.TryGetValue(drawInfo.drawPlayer.body, out Color color))
			{
				return;
			}
			if (drawInfo.hideEntirePlayer || (drawInfo.drawPlayer.mount.Active && drawInfo.drawPlayer.mount.Type == MountID.Wolf))
			{
				return;
			}
			drawInfo.bodyGlowColor = drawInfo.drawPlayer.GetImmuneAlphaPure(color * drawInfo.drawPlayer.stealth, drawInfo.shadow);
			drawInfo.armGlowColor = drawInfo.drawPlayer.GetImmuneAlphaPure(color * drawInfo.drawPlayer.stealth, drawInfo.shadow);
		}
	}
}