using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.QueenSlime
{
	[AutoloadEquip(EquipType.Head)]
	public class QSAltCostumeHeadpiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Body)]
	public class QSCostumeBodypiece : VanityBase
	{

		public int LegEquipTexture;
		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTexture = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs", EquipType.Legs, this, "QSCostumeBodypiece_Legs");
			}
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = LegEquipTexture;
		}
	}
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class QSCostumeGloves : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}