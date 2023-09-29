using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class FriendlyBomb : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.BombSkeletronPrime;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Friendly Bomb");
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.HappyBomb;
			Projectile.timeLeft = 300;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.timeLeft > 3)
			{
				Projectile.timeLeft = 3; // Set the timeLeft to 3 so it can get ready to explode.
			}

			// Set the direction of the projectile so the knockback is always in the correct direction.
			Projectile.direction = (target.Center.X > Projectile.Center.X).ToDirectionInt();
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = oldVelocity.X * -0.5f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = oldVelocity.Y * -0.5f;
			}
			return false;
		}
		public override void AI()
        {
			base.AI();

			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				PrepareBombToBlow();
				return;
			}

			int frameSpeed = 3;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
			if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
			{
				Projectile.velocity.X = Projectile.velocity.X * 0.97f;
				{
					Projectile.velocity.X = Projectile.velocity.X * 0.99f;
				}
				if ((double)Projectile.velocity.X > -0.01 && (double)Projectile.velocity.X < 0.01)
				{
					Projectile.velocity.X = 0f;
					Projectile.netUpdate = true;
				}
			}
			Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
			// Rotation increased by velocity.X 
			Projectile.rotation += Projectile.velocity.X* 0.1f;
		}

		/// <summary> Resizes the projectile for the explosion blast radius. </summary>
		private void PrepareBombToBlow()
		{
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the bomb explodes on slopes.
			Projectile.alpha = 255; // Make the bomb invisible.

			// Resize the hitbox of the projectile for the blast "radius".
			// Rocket I: 128, Rocket III: 200, Mini Nuke Rocket: 250
			// Measurements are in pixels, so 128 / 16 = 8 tiles.
			Projectile.Resize(60, 60);
			// Set the knockback of the blast.
			// Rocket I: 8f, Rocket III: 10f, Mini Nuke Rocket: 12f
			Projectile.knockBack = 4f;
			Projectile.damage = 40;
		}

		public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.position);

			// Resize the projectile again so the explosion dust and gore spawn from the middle
			Projectile.Resize(22, 22);

			// Smoke Dust spawn
			for (int i = 0; i < 20; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
				Main.dust[dustIndex].velocity *= 1.4f;
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.White; //Fullbright
	}
}