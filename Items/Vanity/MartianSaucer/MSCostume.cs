using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.MartianSaucer
{
	[AutoloadEquip(EquipType.Head)]
    public class MSCostumeHeadpiece : VanityBase
    {
    }
	[AutoloadEquip(EquipType.Body)]
	public class MSCostumeBodypiece : VanityBase
	{
	}
	[AutoloadEquip(EquipType.Legs)]
	public class MSCostumeLegpiece : VanityBase
	{
	}
}