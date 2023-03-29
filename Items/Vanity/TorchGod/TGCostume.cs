using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.TorchGod
{
	[AutoloadEquip(EquipType.Head)]
	public class TGCostumeHeadpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
			if (!Main.dedServ)
			{
				ArmorPlayerDrawLayerHead.RegisterData(Item.headSlot, new string[] { Texture + "_Head_Glowmask", "255", "255", "255", "flame2" });
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
    public class TGCostumeBodypiece : VanityBase
    {
    }
	[AutoloadEquip(EquipType.Legs)]
	public class TGCostumeLegpiece : VanityBase
	{
	}
}