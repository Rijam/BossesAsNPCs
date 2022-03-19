using BossesAsNPCs.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossesAsNPCs.Items
{
	public class TownNPCWeapon : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.RottenEgg;
		public override bool IsLoadingEnabled(Mod mod)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Town NPC Weapon");
		}

		public override void SetDefaults()
		{
			Item.damage = 99999;
			Item.width = 20;
			Item.height = 20;
			Item.useTime = 20;
			Item.knockBack = 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 16f;
			Item.useAnimation = 20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TownNPCKiller>();
			Item.value = 0;
		}
	}
}
