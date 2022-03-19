using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.EmpressOfLight
{
	[AutoloadEquip(EquipType.Head)]
	public class EoLCostumeHeadpiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empress Costume Headpiece");
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
	public class EoLCostumeBodypiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empress Costume Bodypiece");
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
	public class EoLCostumeLegpiece : ModItem
	{
		//Thanks Exterminator for the help
		public int LegEquipTextureMale;
		public int LegEquipTextureFemale;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTextureMale = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, "BossesAsNPCs/Items/Vanity/EmpressOfLight/EoLCostumeLegpiece_Legs");
				LegEquipTextureFemale = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, "BossesAsNPCs/Items/Vanity/EmpressOfLight/EoLCostumeLegpiece_FemaleLegs");
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empress Costume Legpiece");
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
			if (male) equipSlot = LegEquipTextureMale;
			if (!male) equipSlot = LegEquipTextureFemale;
		}
	}
}