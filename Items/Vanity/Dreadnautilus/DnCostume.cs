using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.Dreadnautilus
{
	[AutoloadEquip(EquipType.Head)]
	public class DnCostumeHeadpiece : VanityBase
	{
    }
	[AutoloadEquip(EquipType.Body)]
	public class DnCostumeBodypiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Legs)]
	public class DnCostumeLegpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
		}
	}
}