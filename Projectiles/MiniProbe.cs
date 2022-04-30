using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class MiniProbe : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Probe");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			AIType = 0;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
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
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = oldVelocity.Y * -1f;
				}
			}
			return false;
		}
		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 51;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			}
			if (Projectile.ai[0] == 20) //Update every 15 ticks
			{
				int newTarget = FindTargetWithLineOfSight();
				if (newTarget != -1) //shoot target
				{
					NPC nPC2 = Main.npc[newTarget];
					float speed = 12f;
					Vector2 direction = nPC2.Center - Projectile.Center;
					direction.Normalize();
					Projectile.rotation = direction.ToRotation() + MathHelper.PiOver2;
					direction *= speed;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, direction, ProjectileID.MiniRetinaLaser, 40, 2f, Projectile.owner);
					Projectile.netUpdate = true;
				}
				else
                {
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				}
				Projectile.ai[0] = 5;
			}
		}
		//Copied from vanilla (1.4) Projectiles.cs
		public int FindTargetWithLineOfSight(float maxRange = 800f)
		{
			float newMaxRange = maxRange;
			int result = -1;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				bool nPCCanBeChased = nPC.CanBeChasedBy(this);
				if (Projectile.localNPCImmunity[i] != 0)
				{
					nPCCanBeChased = false;
				}
				if (nPCCanBeChased)
				{
					float projDist = Projectile.Distance(Main.npc[i].Center);
					if (projDist < newMaxRange && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, nPC.position, nPC.width, nPC.height))
					{
						newMaxRange = projDist;
						result = i;
					}
				}
			}
			return result;
		}
		public override void Kill(int timeLeft)
		{
			Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * 0.5f, GoreID.Smoke1, 1f);
			Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * 0.4f, GoreID.Smoke2, 1f);
			Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * 0.6f, GoreID.Smoke3, 1f);
			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.position);
		}
	}
}