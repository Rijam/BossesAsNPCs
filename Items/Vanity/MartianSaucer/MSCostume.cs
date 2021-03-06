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
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorPlayerDrawLayerHead.RegisterData(Item.headSlot, new string[] { Texture + "_Head_Glowmask", "255", "255", "255", "none" });
			}
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class MSCostumeBodypiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorGlowmaskBody.RegisterData(Item.bodySlot, Color.White);
			}
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class MSCostumeLegpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorPlayerDrawLayerLegs.RegisterData(Item.legSlot, new string[] { Texture + "_Legs_Glowmask", "255", "255", "255", "none" });
			}
		}
	}
}