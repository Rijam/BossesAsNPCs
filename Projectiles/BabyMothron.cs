using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class BabyMothron : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Baby Mothron");
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 600;
			DrawOffsetX = -9;
			DrawOriginOffsetY = -10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
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
					Projectile.velocity.X = oldVelocity.X * -0.5f;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = oldVelocity.Y * -0.5f;
				}
			}
			return false;
        }
		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);

			Projectile.ai[0]++;
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

			Projectile.velocity.Y = Projectile.velocity.Y + 0.01f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 6;
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
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
            {
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.TintableDust, 0, 0, 200);
			}
			SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
			Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, ModContent.Find<ModGore>(Mod.Name + "/" + Name + "_Gore").Type, 1f);
		}
	}
}