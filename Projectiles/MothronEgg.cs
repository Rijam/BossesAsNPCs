using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class MothronEgg : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Mothron Egg");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			AIType = ProjectileID.OrnamentFriendly;
			Projectile.timeLeft = 300;
		}
		public override void AI()
		{
			Projectile.rotation += 0.2f;
		}
		public override void OnKill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 1, 1, DustID.TintableDust, 0, 0, 200);
			SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<BabyMothron>(), Projectile.damage * 5, Projectile.knockBack, Projectile.owner);
		}
	}
}