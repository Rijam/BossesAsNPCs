using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

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