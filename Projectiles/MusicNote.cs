using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class DoubleEighthNote : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.TiedEighthNote;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Double Eighth Note");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 18;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.alpha = 100;
			AIType = 0;
			Projectile.penetrate = 6;
			Projectile.timeLeft = 300;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.Kill();
		}
		public static readonly SoundStyle Guitar1 = new("Terraria/Sounds/Item_47", 47, 1)
		{
			Volume = 1f,
			PitchRange = (1f, 2f)
		};
		//private readonly float[] pitches = {1f, 1.25f, 1.5f, 1.75f, 2f};
		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
			{
				//SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 47, 1, pitches[Main.rand.Next(0, 4)]);
				SoundEngine.PlaySound(Guitar1, Projectile.Center);
			}
			if (Projectile.ai[0] == 7) //Update every 2 ticks
			{
				int newTarget = Projectile.FindTargetWithLineOfSight();
				if (newTarget != -1) //fly to the target
				{
					NPC nPC2 = Main.npc[newTarget];
					float speed = 6f;
					float inertia = 6f;
					Vector2 direction = nPC2.Center - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
					Projectile.netUpdate = true;
				}
				Projectile.ai[0] = 5;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() * 0.1f;
		}

		public static readonly SoundStyle Guitar2 = new("Terraria/Sounds/Item_47", 47, 1)
		{
			Volume = 0.5f,
			PitchRange = (0.5f, 0.5f)
		};
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
            {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.RedTorch);
			}
			//SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 47, 0.5f, 0.5f);
			SoundEngine.PlaySound(Guitar2, Projectile.Center);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity * 0.5f, ModContent.ProjectileType<SingleEighthNote>(), Projectile.damage / 2, Projectile.knockBack / 2, 0);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity * -0.5f, ModContent.ProjectileType<SingleEighthNote>(), Projectile.damage / 2, Projectile.knockBack / 2, 0);
		}
		public override Color? GetAlpha(Color lightColor) => Color.Red; //Fullbright
	}
	public class SingleEighthNote : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.EighthNote;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Single Eighth Note");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 18;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.alpha = 100;
			AIType = 0;
			Projectile.penetrate = 6;
			Projectile.timeLeft = 300;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
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
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.Kill();
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() * 0.1f;
		}

        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.RedTorch);
			}
		}
		public override Color? GetAlpha(Color lightColor) => Color.Red; //Fullbright
	}
}