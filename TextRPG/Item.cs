// Item.cs
namespace TextRPG;

public enum ItemType { Armor, Weapon, Accessory }

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public string Description { get; set; }
    public int Effect { get; set; }
    public int Price { get; set; }
    public bool IsEquipped { get; set; }

    public override string ToString()
    {
        string effectType = Type == ItemType.Armor ? "방어력" : Type == ItemType.Weapon ? "공격력" : "효과";
        return $"{Name} ({Type}) | {effectType} +{Effect} | {Description}";
    }
}