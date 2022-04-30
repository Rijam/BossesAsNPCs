using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.EmpressOfLight
{
	[AutoloadEquip(EquipType.Head)]
    public class EoLCostumeHeadpiece : VanityBase
    {
    }
	[AutoloadEquip(EquipType.Body)]
    public class EoLCostumeBodypiece : VanityBase
    {
    }
	[AutoloadEquip(EquipType.Legs)]
	public class EoLCostumeLegpiece : VanityBase
	{
		//Thanks Exterminator for the help
		public int LegEquipTextureMale;
		public int LegEquipTextureFemale;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTextureMale = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs");
				LegEquipTextureFemale = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, (GetType().Namespace + "." + Name).Replace('.', '/') + "_FemaleLegs");
			}
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			if (male) equipSlot = LegEquipTextureMale;
			if (!male) equipSlot = LegEquipTextureFemale;
		}
	}
}