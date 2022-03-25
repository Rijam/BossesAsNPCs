using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.Golem
{
	[AutoloadEquip(EquipType.Body)]
	public class GolemCostumeBodypiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golem Costume Bodypiece");
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
	[AutoloadEquip(EquipType.Legs)]
	public class GolemCostumeLegpiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golem Costume Legpiece");
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