using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class EyeOrnament : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Eye Ornament");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			AIType = ProjectileID.OrnamentFriendly;
			if (!Main.hardMode)
			{
				Projectile.penetrate = 3;
			}
			if (Main.hardMode)
			{
				Projectile.penetrate = 6;
			}
			Projectile.timeLeft = 300;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else //bounce off of tiles
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = oldVelocity.X * -1f;
					Projectile.penetrate--;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = oldVelocity.Y * -1f;
					Projectile.penetrate--;
				}
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.Blood);
			SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
		}
	}
}