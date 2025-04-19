using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.LunaticCultist
{
	[AutoloadEquip(EquipType.Head)]
	public class LCCostumeHeadpiece : VanityBase
	{
	}

	[AutoloadEquip(EquipType.Body)]
	public class LCCostumeBodypiece : VanityBase
	{

		public int LegEquipTexture;
		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTexture = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs", EquipType.Legs, this, "LCCostumeBodypiece_Legs");
			}
		}
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = LegEquipTexture;
		}
	}
}