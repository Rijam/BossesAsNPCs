using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Audio;

namespace BossesAsNPCs.Projectiles
{
	public class FireBolt : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.alpha = 255;
			AIType = -1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.ai[0]++;
			if (Projectile.ai[0] == 0)
			{
				SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
			}
			Color dustColor = Color.Lerp(Color.Orange, Color.Blue, Projectile.ai[0] / 300f);
			int dust = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.WhiteTorch, 0, 0, 100, dustColor, 1f);
			Main.dust[dust].noLightEmittence = true;
			Lighting.AddLight(Projectile.Center, dustColor.ToVector3());
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// SpriteEffects change which direction the sprite is drawn.
			SpriteEffects spriteEffects = SpriteEffects.None;

			// Get texture of projectile
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			// Get the currently selected frame on the texture.
			Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

			// The rotation of the projectile.
			float rotation = Projectile.rotation;

			// The position of the sprite.
			Vector2 position = new(Projectile.Center.X, Projectile.Center.Y);

			// Apply lighting and draw our projectile
			Color drawColor = new(255, 255, 255, 100);

			ulong seed = Main.TileFrameSeed ^ (ulong)(((long)Projectile.position.Y << 32) | (uint)Projectile.position.X);

			// Fading trail
			for (int i = 0; i < 5; i++)
			{
				float random1 = Utils.RandomInt(ref seed, -11, 11) * 0.05f;
				float random2 = Utils.RandomInt(ref seed, -5, 5) * 0.15f;
				position += new Vector2(random1, random2);
				Vector2 moveTo = Projectile.velocity.SafeNormalize(Vector2.Zero) * (i + 3f);
				position += moveTo;
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position - Main.screenPosition, sourceRectangle, drawColor * ((i + 1f) / 10), rotation, default, Projectile.scale, spriteEffects, 0);
			}
			// Actual position
			for (int i = 0; i < 5; i++)
			{
				float random1 = Utils.RandomInt(ref seed, -11, 11) * 0.05f;
				float random2 = Utils.RandomInt(ref seed, -5, 5) * 0.15f;
				position += new Vector2(random1, random2);
				Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position - Main.screenPosition, sourceRectangle, drawColor, rotation, default, Projectile.scale, spriteEffects, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Color dustColor = Color.Lerp(Color.Orange, Color.Blue, i / 30f);
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0, 0, 100, dustColor, 1f);
				Main.dust[dust].noLightEmittence = true;
				Lighting.AddLight(Projectile.Center, dustColor.ToVector3());
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (damage > 0)
			{
				if (Main.hardMode)
				{
					target.AddBuff(BuffID.OnFire3, damage);
				}
				else
				{
					target.AddBuff(BuffID.OnFire, damage);
				}
			}
			
		}
	}
}