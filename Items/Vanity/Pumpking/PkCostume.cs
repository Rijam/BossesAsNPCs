using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity.Pumpking
{
	[AutoloadEquip(EquipType.Head)]
	public class PkCostumeHeadpiece : VanityBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				ArmorPlayerDrawLayerHead.RegisterData(Item.headSlot, new string[] { Texture + "_Head_Glowmask", "255", "255", "255", "flame" });
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
	public class PkCostumeBodypiece : VanityBase
	{
		//Thanks Exterminator for the help
		public int LegEquipTexture;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				LegEquipTexture = EquipLoader.AddEquipTexture(Mod, (GetType().Namespace + "." + Name).Replace('.', '/') + "_Legs_Complete", EquipType.Legs, this);
			}
		}
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = LegEquipTexture;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PkCostumeLegpiece : VanityBase
	{
		public override bool IsLoadingEnabled(Mod mod) => false;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// Tooltip.SetDefault("[c/403638:Unobtainable]");
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
		}
	}
	[AutoloadEquip(EquipType.Shoes)]
	public class PkCostumeShoes : VanityBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}