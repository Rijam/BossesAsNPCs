using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.Betsy
{
	[AutoloadEquip(EquipType.Body)]
    public class BeCostumeBodypiece : VanityBase
    {
    }
	[AutoloadEquip(EquipType.Legs)]
	public class BeCostumeLegpiece : VanityBase
	{
		//Thanks Exterminator for the help
		public int LegEquipTextureMale;
		public int LegEquipTextureFemale;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTextureMale = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs", EquipType.Legs, this);
				LegEquipTextureFemale = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_FemaleLegs", EquipType.Legs, this);
			}
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (male) equipSlot = LegEquipTextureMale;
			if (!male) equipSlot = LegEquipTextureFemale;
		}
	}
}