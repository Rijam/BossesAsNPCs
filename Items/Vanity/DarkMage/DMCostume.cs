using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.DarkMage
{
	[AutoloadEquip(EquipType.Head)]
	public class DMCostumeHeadpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorPlayerDrawLayerHead.RegisterData(Item.headSlot, new ArmorHeadLegsOptions(Texture + "_Head_Glow"));
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
	public class DMCostumeBodypiece : VanityBase
	{

	}
	[AutoloadEquip(EquipType.Legs)]
	public class DMCostumeLegpiece : VanityBase
	{

	}
}