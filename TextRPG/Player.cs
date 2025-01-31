namespace TextRPG;

// 플레이어의 스탯을 구조체로 관리
public struct Stats
{
    public float Atk { get; set; } // 공격력
    public float Def { get; set; } // 방어력
    public float Hp { get; set; } // 체력

    // 생성자 (초기값 설정)
    public Stats(float atk, float def, float hp)
    {
        this.Atk = atk;
        this.Def = def;
        this.Hp = hp;
    }
}

public class Player
{
    public string strName { get; private set; }
    public string strJob { get; private set; }
    public int iGold { get;  private set; } = 1500;
    public int iLevel { get; private set; } = 1;
    public float fCurHp { get; private set; } = 100;
    public int iExp { get; private set; } = 0;
    public int iMaxExp { get; private set; } = 1;
    public Stats PlayerStats { get; private set; } = new Stats(10, 5, 100);
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
            strName = name;
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
            strJob = jobType[choice - 1];
        else
            CreateJob();
    }
    
    public void EquipItem(Item item)
    {
        int effectModifier = item.isEquipped ? -1 : 1; // 장착이면 해제후 스텟 감소, 해제면 장착 후 스탯 증가
        if (effectModifier > 0) // 이미 창작한 기본의 같은 타입 아이템 해제
        {
            foreach (Item equItem in Inventory)
            {
                if (equItem.Type == item.Type && equItem.isEquipped)  //같은 타입의 아이템이 있고, 창착 중이라면
                    EquipItem(equItem);
            }
            
        }
        
        item.isEquipped = !item.isEquipped; // 장착 여부 토글
        
    
        ApplyItemEffect(item, effectModifier);
    }

    private void ApplyItemEffect(Item item, int modifier)
    {
        switch (item.Type)
        {
            case ItemType.Armor:
                AddStats = new Stats(AddStats.Atk, AddStats.Def + (modifier * item.iEffect), AddStats.Hp);
                break;
            case ItemType.Weapon:
                AddStats = new Stats(AddStats.Atk + (modifier * item.iEffect), AddStats.Def, AddStats.Hp);
                break;
            case ItemType.Accessory:
                AddStats = new Stats(AddStats.Atk, AddStats.Def, AddStats.Hp + (modifier * item.iEffect));
                break;
        }
    }
    
    public void AddGold(int gold)
    {
        iGold += gold;
    }

    public void AddHp(float hp)
    {
        fCurHp += hp;
        if (fCurHp > GetStats().Hp)
            fCurHp = GetStats().Hp;
        else if (fCurHp < 0)
            fCurHp = 0;
    }
    

    public bool LevelUp()   //레벨업
    {
        if (++iExp >= iMaxExp)
        {
            iExp -= iMaxExp++;
            iLevel++;
            PlayerStats = new Stats(PlayerStats.Atk + 0.5f, PlayerStats.Def + 1.0f, PlayerStats.Hp + 5.0f);
            return true;
        }
        return false;
    }


    public Stats GetStats()
    {
        Stats stats = new Stats(PlayerStats.Atk + AddStats.Atk, PlayerStats.Def + AddStats.Def, PlayerStats.Hp + AddStats.Hp);
        return stats;
    }
}