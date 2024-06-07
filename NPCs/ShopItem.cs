using System.Collections.Generic;
using Terraria;

namespace BossesAsNPCs.NPCs
{
	public class ShopItem(int itemType, int price, List<Condition> condition)
	{
		public int ItemType { get; set; } = itemType;
		public int Price { get; set; } = price;
		public List<Condition> Condition { get; set; } = condition;
	}
}