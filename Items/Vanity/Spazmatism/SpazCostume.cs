using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.Spazmatism
{
	[AutoloadEquip(EquipType.Head)]
	public class SpazCostumeHeadpiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazmatism Costume Headpiece");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}