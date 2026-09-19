using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class SpikedSlimeProjectile : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SpikedSlimeSpike;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gel Spike");
		}

		public override void SetDefaults()
		{
			Projectile.arrow = false;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.friendly = true;
			AIType = ProjectileID.WoodenArrowFriendly;
			if (!Main.hardMode)
			{
				Projectile.penetrate = 1;
			}
			else
			{
				Projectile.penetrate = 2;
			}
			Projectile.timeLeft = 600;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			return true;
		}
	}
}