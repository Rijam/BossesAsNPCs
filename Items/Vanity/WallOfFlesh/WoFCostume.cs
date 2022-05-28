using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.WallOfFlesh
{
	[AutoloadEquip(EquipType.Head)]
	public class WoFCostumeHeadpiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Body)]
	public class WoFCostumeBodypiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Legs)]
	public class WoFCostumeLegpiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Back)]
	public class WoFCostumeBackpiece : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}