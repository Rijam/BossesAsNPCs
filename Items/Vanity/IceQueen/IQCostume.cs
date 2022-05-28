using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.IceQueen
{
	[AutoloadEquip(EquipType.Head)]
	public class IQCostumeHeadpiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Body)]
	public class IQCostumeBodypiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Legs)]
	public class IQCostumeLegpiece : VanityBase
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
	[AutoloadEquip(EquipType.Back)]
	public class IQCostumeCape : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}