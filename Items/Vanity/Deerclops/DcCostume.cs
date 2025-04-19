using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.Deerclops
{
	[AutoloadEquip(EquipType.Body)]
	public class DcCostumeBodypiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = true;
			ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class DcCostumeLegpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
		}
	}
}