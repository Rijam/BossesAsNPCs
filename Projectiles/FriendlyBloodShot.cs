using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace BossesAsNPCs.Projectiles
{
	public class FriendlyBloodShot : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BloodShot;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Friendly Blood Shot");
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			AIType = ProjectileID.BloodShot;
			Projectile.timeLeft = 300;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = TextureAssets.Projectile[Projectile.type].Height() / Main.projFrames[Projectile.type];
			int frame = frameHeight * Projectile.frame;
			Rectangle rectangle = new(0, frame, texture.Width, frameHeight);
			Vector2 origin = rectangle.Size() / 2f;
			int num147 = 0;
			int incrementAmount = -1;
			int startLoop = 18;
			float minScale = 1f;
			float scaleMod = 15f;
			Rectangle sourceRectangle = rectangle;

			Color color = lightColor;

			for (int i = startLoop; incrementAmount < 0 && i > num147; i += incrementAmount)
			{
				color = Color.Lerp(color, Color.Crimson * 0.5f, i / (float)num147);

				color = Color.Lerp(color, color * 0.5f, i / (float)num147);

				color = Projectile.GetAlpha(color);

				float num157 = num147 - i;
				if (incrementAmount < 0)
				{
					num157 = startLoop - i;
				}
				color *= num157 / (ProjectileID.Sets.TrailCacheLength[Projectile.type] * 1.5f);
				float rotation = Projectile.rotation;
				Vector2 position = Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				Main.EntitySpriteDraw(
					texture, 
					position, 
					sourceRectangle, 
					color, 
					rotation, 
					origin, 
					MathHelper.Lerp(Projectile.scale, minScale, i / scaleMod),
					SpriteEffects.None, 
					0);
			}

			return false;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood)];
				dust.scale = 1.25f + Main.rand.NextFloat();
				Dust dust2 = dust;
				dust2.velocity *= 2f;
			}
		}
	}
}