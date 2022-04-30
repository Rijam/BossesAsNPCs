using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.LunaticCultist
{
	[AutoloadEquip(EquipType.Body)]
	public class LCCostumeBodypiece : VanityBase
	{

		public int LegEquipTexture;
		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTexture = Mod.AddEquipTexture(new EquipTexture(), this, EquipType.Legs, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs");
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