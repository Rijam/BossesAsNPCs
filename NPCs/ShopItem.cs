using System.Collections.Generic;
using Terraria;

namespace BossesAsNPCs.NPCs
{
	public class ShopItem(int itemType, int price, List<Condition> condition)
	{
		/// <summary> Item ID Type. Item.type </summary>
		public int ItemType { get; set; } = itemType;
		/// <summary> The price of the item in copper coins. </summary>
		public int Price { get; set; } = price;
		/// <summary> A list of all of the Conditions for the shop item. </summary>
		public List<Condition> Condition { get; set; } = condition;
	}
}