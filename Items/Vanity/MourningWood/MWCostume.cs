using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.MourningWood
{
	[AutoloadEquip(EquipType.Head)]
	public class MWCostumeHeadpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorPlayerDrawLayerHead.RegisterData(Item.headSlot, new string[] { Texture + "_Head_Glow", "255", "255", "255", "flame" });
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
			}
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class MWCostumeBodypiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = true;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class MWCostumeLegpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
		}
	}
}