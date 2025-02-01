namespace TextRPG;

public enum ItemType
{
    Armor,
    Weapon,
    Accessory
}

public class Item
{
    public int ID { get; set; }
    public string strName { get; set; }
    public ItemType Type { get; set; }
    public string strDescription { get; set; }
    public float iEffect { get; set; }
    public int iPrice { get; set; }
    public bool isEquipped { get; set; }

    public override string ToString()
    {
        string effectType = Type == ItemType.Armor ? "방어력" : Type == ItemType.Weapon ? "공격력" : "체력";
        return $"{strName} ({Type}) | {effectType} +{iEffect} | {strDescription}";
    }
}