using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items.Vanity.TheDestroyer
{
	[AutoloadEquip(EquipType.Head)]
	public class DeCostumeHeadpiece : VanityBase
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
	public class DeCostumeBodypiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorGlowmaskBody.RegisterData(Item.bodySlot, Color.White);
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
	[AutoloadEquip(EquipType.Legs)]
	public class DeCostumeLegpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
			if (!Main.dedServ)
			{
				ArmorPlayerDrawLayerLegs.RegisterData(Item.legSlot, new ArmorHeadLegsOptions(Texture + "_Legs_Glow"));
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
}