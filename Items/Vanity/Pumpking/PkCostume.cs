using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.Pumpking
{
	[AutoloadEquip(EquipType.Head)]
	public class PkCostumeHeadpiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpking Costume Headpiece");
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
	[AutoloadEquip(EquipType.Body)]
	public class PkCostumeBodypiece : ModItem
	{
		//Thanks Exterminator for the help
		public int LegEquipTexture;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTexture = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, "BossesAsNPCs/Items/Vanity/Pumpking/PkCostumeBodypiece_Legs_Complete");
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpking Costume Bodypiece");
			ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = LegEquipTexture;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PkCostumeLegpiece : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpking Costume Legpiece");
			Tooltip.SetDefault("[c/403638:Unobtainable]");
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
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
	[AutoloadEquip(EquipType.Shoes)]
	public class PkCostumeShoes : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumpking Costume Shoes");
			Tooltip.SetDefault("Vanity Accessory");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
			Item.accessory = true;
		}
	}
}