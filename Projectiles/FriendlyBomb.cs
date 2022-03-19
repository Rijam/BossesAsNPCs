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
			DisplayName.SetDefault("Friendly Bomb");
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			AIType = ProjectileID.HappyBomb;
			Projectile.timeLeft = 300;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.Kill();
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
		public override void Kill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.position);
			Projectile.position = Projectile.Center;
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.Center = Projectile.position;
			Projectile.damage = 40;
			Projectile.knockBack = 4f;
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