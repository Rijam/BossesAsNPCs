using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.Skeletron
{
	[AutoloadEquip(EquipType.Body)]
	public class SkCostumeBodypiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Legs)]
	public class SkCostumeLegpiece : VanityBase
	{
	}

	[AutoloadEquip(EquipType.Head)]
	public class SkeletronsRedHat : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class SkShimmeredCostumeHeadpiece : VanityBase
	{
	}

	[AutoloadEquip(EquipType.Head)]
	public class SkShimmeredAltCostumeHeadpiece : VanityBase
	{
	}

	[AutoloadEquip(EquipType.Body)]
	public class SkShimmeredCostumeBodypiece : VanityBase
	{
	}

	[AutoloadEquip(EquipType.Legs)]
	public class SkShimmeredCostumeLegpiece : VanityBase
	{
	}
}