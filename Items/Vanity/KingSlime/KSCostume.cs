using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.KingSlime
{
	[AutoloadEquip(EquipType.Head)]
    public class KSCostumeHeadpiece : VanityBase
    {
    }
	[AutoloadEquip(EquipType.Body)]
	public class KSCostumeBodypiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Legs)]
	public class KSCostumeLegpiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class KSCostumeGloves : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class KSAltCostumeGloves : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
	[AutoloadEquip(EquipType.Front, EquipType.Back)]
	public class KSCostumeCape : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}