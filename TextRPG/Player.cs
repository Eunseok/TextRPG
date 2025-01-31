// Player.cs

namespace TextRPG;

// 플레이어의 스탯을 구조체로 관리
public struct Stats
{
    public int Atk { get; set; } // 공격력
    public int Def { get; set; } // 방어력
    public int Hp { get; set; } // 체력

    // 생성자 (초기값 설정)
    public Stats(int atk, int def, int hp)
    {
        this.Atk = atk;
        this.Def = def;
        this.Hp = hp;
    }
}

public class Player
{
    public string Name { get; private set; }
    public string Job { get; private set; }
    public int Gold { get;  set; } = 1500;
    public int Level { get; private set; } = 1;
    public Stats Stats { get; private set; } = new Stats(10, 5, 100);
    public Stats AddStats { get; private set; } = new Stats(0, 0, 0);
    public List<Item> Inventory { get; private set; } = new List<Item>();

    public void CreateName()
    {
        Console.Clear();
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        Console.WriteLine("이름을 설정해 주세요.\n");

        Utility.PrintColor(">> ", ConsoleColor.Yellow);
        string name = Console.ReadLine();

        if (!string.IsNullOrEmpty(name) &&
            Utility.Save($"선택하신 이름은 \"{name}\" 입니다.\n"))
            Name = name;
        else
            CreateName();
    }

    public void CreateJob()
    {
        string[] jobType = { "전사", "도적" };
        Console.Clear();
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        Console.WriteLine("직업을 선택해 주세요.\n");
        for (int i = 0; i < jobType.Length; i++)
            Console.WriteLine($"{i + 1}. {jobType[i]}");
        Console.WriteLine();

        int choice = Utility.Confirm(jobType.Length);
        if (Utility.Save($"선택하신 직업은 \"{jobType[choice - 1]}\" 입니다.\n"))
            Job = jobType[choice - 1];
        else
            CreateJob();
    }
    
    public void EquipItem(Item item)
    {
        int effectModifier = item.IsEquipped ? -1 : 1; // 장착이면 해제후 스텟 감소, 해제면 장착 후 스탯 증가
    
        item.IsEquipped = !item.IsEquipped; // 장착 여부 토글
    
        ApplyItemEffect(item, effectModifier);
    }

    private void ApplyItemEffect(Item item, int modifier)
    {
        switch (item.Type)
        {
            case ItemType.Armor:
                AddStats = new Stats(AddStats.Atk, AddStats.Def + (modifier * item.Effect), AddStats.Hp);
                break;
            case ItemType.Weapon:
                AddStats = new Stats(AddStats.Atk + (modifier * item.Effect), AddStats.Def, AddStats.Hp);
                break;
            case ItemType.Accessory:
                AddStats = new Stats(AddStats.Atk, AddStats.Def, AddStats.Hp + (modifier * item.Effect));
                break;
        }
    }
}