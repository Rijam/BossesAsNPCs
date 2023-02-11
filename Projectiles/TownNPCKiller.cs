using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Projectiles
{
	public class TownNPCKiller : ModProjectile
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.RottenEgg;
		public override bool IsLoadingEnabled(Mod mod)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Town NPC Killer");
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.RottenEgg);
			Projectile.hostile = true;
			Projectile.timeLeft = 30;
		}
	}
}