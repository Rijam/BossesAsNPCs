using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.KingSlime
{
	[AutoloadEquip(EquipType.Head)]
    public class KSCostumeHeadpiece : VanityBase
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
}