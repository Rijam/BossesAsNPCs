using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.QueenSlime
{
	[AutoloadEquip(EquipType.Head)]
	public class QSAltCostumeHeadpiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Queen Slime Alternate Costume Headpiece");
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
	public class QSCostumeBodypiece : ModItem
	{
		public int LegEquipTexture;
		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTexture = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, "BossesAsNPCs/Items/Vanity/QueenSlime/QSCostumeBodypiece_Legs");
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Queen Slime Costume Bodypiece");
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
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class QSCostumeGloves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Queen Slime Costume Gloves");
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