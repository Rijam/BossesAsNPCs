using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.LunaticCultist
{
	[AutoloadEquip(EquipType.Body)]
	public class LCCostumeBodypiece : ModItem
	{
		public int LegEquipTexture;
		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTexture = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, "BossesAsNPCs/Items/Vanity/LunaticCultist/LCCostumeBodypiece_Legs");
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunatic Cultist Costume Bodypiece");
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
}