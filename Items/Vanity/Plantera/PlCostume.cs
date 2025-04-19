using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.Plantera
{
	[AutoloadEquip(EquipType.Body)]
	public class PlCostumeBodypiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PlCostumeLegpiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Back)]
	public class PlCostumeBackpiece : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}