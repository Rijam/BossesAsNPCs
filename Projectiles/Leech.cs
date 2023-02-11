using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace BossesAsNPCs.Projectiles
{
	public class Leech : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Leech");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 18;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			AIType = 0;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 180;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void AI()
		{
			Projectile.ai[0]++;
			if (Projectile.ai[0] == 0 || Projectile.ai[0] == 1)
			{
				SoundEngine.PlaySound(SoundID.NPCDeath13, Projectile.position);
			}
			if (Projectile.ai[0] == 7) //Update every 2 ticks
			{
				int newTarget = Projectile.FindTargetWithLineOfSight();
				if (newTarget != -1) //fly to the target
				{
					NPC nPC2 = Main.npc[newTarget];
					float speed = 10f;
					float inertia = 10f;
					Vector2 direction = nPC2.Center - Projectile.Center;
					direction.Normalize();
					direction *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
					Projectile.netUpdate = true;
				}
				Projectile.ai[0] = 5;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
            {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.Blood);
			}
			Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, 134, 1f); //Leech Head
			SoundEngine.PlaySound(SoundID.NPCDeath12, Projectile.position);
		}
	}
}