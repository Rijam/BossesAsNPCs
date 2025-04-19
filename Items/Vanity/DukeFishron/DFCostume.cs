using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.DukeFishron
{
	[AutoloadEquip(EquipType.Head)]
	public class DFCostumeHeadpiece : VanityBase
	{
    }
	[AutoloadEquip(EquipType.Body)]
	public class DFCostumeBodypiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Legs)]
	public class DFCostumeLegpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
		}
	}
}