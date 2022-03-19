using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossesAsNPCs.Items
{
	public class PlanterasAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plantera's Axe");
			Tooltip.SetDefault("[c/403638:Used by the Plantera Town NPC]\n[c/403638:because the normal The Axe is upside-down]");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 56;
			Item.height = 56;
			Item.value = 0;
			Item.rare = ItemRarityID.White;
		}
	}
}