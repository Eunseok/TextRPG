// UIManager.cs

using System.Runtime.InteropServices;

namespace TextRPG;

public class UIManager
{
    private Player player;
    private Game game;

    public List<Item> ShopInventory { get; set; }


    private string[] dungeonDiff = new string[] { "쉬운 던전", "일반 던전", "어려운 던전" };
    int[] dungeonDef = { 5, 11, 17 }; //요구 방어력

    public UIManager(Player player, Game game)
    {
        this.player = player;
        this.game = game;
        ShopInventory = ItemLoader.Items;
    }

    public void MainScene()
    {
        Console.Clear();
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

        string[] menu = { "상태보기", "인벤토리", "상점", "던전입장", "휴식하기" };
        for (int i = 0; i < menu.Length; i++)
        {
            Utility.PrintColor($"{i + 1}. ", ConsoleColor.Magenta);
            Console.WriteLine(menu[i]);
        }

        Console.WriteLine();

        int choice = Utility.Confirm(menu.Length);
        switch (choice)
        {
            case 1:
                ShowStatus();
                break;
            case 2:
                ShowInventory();
                break;
            case 3:
                ShowShop();
                break;
            case 4:
                DungeonEntry();
                break;
            case 5:
                ShowRest();
                break;
        }
    }

    private void ShowStatus() //스탯 보기
    {
        Console.Clear();
        Utility.PrintColorLine("상태 보기", ConsoleColor.Yellow);
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

        Console.WriteLine($"{player.strName}: ({player.strJob})\nLv: {player.iLevel}");
        Console.Write($"공격력: {player.Stats.Atk}");
        if (player.AddStats.Atk > 0)
            Utility.PrintColor($" +({player.AddStats.Atk})", ConsoleColor.Yellow);
        Console.Write($"\n방어력: {player.Stats.Def}");
        if (player.AddStats.Def > 0)
            Utility.PrintColor($" +({player.AddStats.Def})", ConsoleColor.Yellow);
        Console.Write($"\n체력: {player.Stats.Hp}");
        if (player.AddStats.Hp > 0)
            Utility.PrintColor($" +({player.AddStats.Hp})", ConsoleColor.Yellow);

        Console.WriteLine("\n");
        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);

        if (Utility.Confirm(0, 0) == 0)
            MainScene();
    }

    private void ShowInventory() // 인벤토리
    {
        Console.Clear();
        Utility.PrintColorLine("인벤토리 - 장착관리", ConsoleColor.Yellow);
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

        for (int i = 0; i < player.Inventory.Count; i++)
        {
            Utility.PrintColor($"{i + 1}. ", ConsoleColor.Magenta);
            if (player.Inventory[i].isEquipped)
                Console.Write("[E] ");
            Console.WriteLine(player.Inventory[i]);
        }

        Console.WriteLine();

        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);
        int input = Utility.Confirm(player.Inventory.Count, 0);
        if (input == 0)
        {
            MainScene();
        }
        else
        {
            player.EquipItem(player.Inventory[input - 1]);
            ShowInventory();
        }
    }

    private void ShowShop(string info = "") //상점
    {
        Console.Clear();
        Utility.PrintColorLine("상점", ConsoleColor.Yellow);
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine("[보유 골드]");
        Utility.PrintColorLine($"{player.iGold} G\n", ConsoleColor.Yellow);

        int index = 0;
        foreach (Item item in ShopInventory)
        {
            Console.Write("- ");
            Utility.PrintColor($"{index + 1}. ", ConsoleColor.Magenta);
            Console.Write(item);
            if (player.Inventory.Find(x => x.ID == item.ID) != null)
                Console.WriteLine(" [구매완료]");
            else if (player.iGold < item.iPrice)
                Utility.PrintColorLine($" ({item.iPrice} G)");
            else
                Utility.PrintColorLine($" ({item.iPrice} G)", ConsoleColor.Yellow);
            index++;
        }

        Console.WriteLine();
        Utility.PrintColorLine("1. 아이템 구매", ConsoleColor.Magenta);
        Utility.PrintColorLine("2. 아이템 판매", ConsoleColor.Magenta);
        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);

        if (!string.IsNullOrEmpty(info))
            Utility.InfoMessage("\n" + info, ConsoleColor.Blue);
        switch (Utility.Confirm(2, 0))
        {
            case 0:
                MainScene();
                break;
            case 1:
                ShowShopPurchase();
                break;
            case 2:
                ShowShopSell();
                break;
        }

        if (Utility.Confirm(2, 0) == 1)
            ShowShopPurchase();
        else
            MainScene();
    }

    private void ShowShopPurchase() //상점 구매
    {
        Console.Clear();
        Utility.PrintColorLine("상점 - 아이템 구매", ConsoleColor.Yellow);
        Console.WriteLine("구매하실 아이템을 선택하세요.\n");
        
        Console.WriteLine("[보유 골드]");
        Utility.PrintColorLine($"{player.iGold} G\n", ConsoleColor.Yellow);

        int index = 0;
        foreach (Item item in ShopInventory)
        {
            Console.Write("- ");
            Utility.PrintColor($"{index + 1}. ", ConsoleColor.Magenta);
            Console.Write(item);
            if (player.Inventory.Find(x => x.ID == item.ID) != null)
                Console.WriteLine(" [구매완료]");
            else if (player.iGold < item.iPrice)
                Utility.PrintColorLine($" ({item.iPrice} G)");
            else
                Utility.PrintColorLine($" ({item.iPrice} G)", ConsoleColor.Yellow);
            index++;
        }

        Console.WriteLine();

        Utility.PrintColorLine("0. 취소\n", ConsoleColor.Red);

        while (true)
        {
            int input = Utility.Confirm(ShopInventory.Count, 0);
            if (input == 0)
                ShowShop();
            else
            {
                Item item = ShopInventory[input - 1];
                if (player.Inventory.Find(x => x.ID == item.ID) != null)
                {
                    Utility.InfoMessage("이미 보유한 아이템 입니다.");
                }
                else if (player.iGold < item.iPrice)
                {
                    Utility.InfoMessage("보유 골드가 부족합니다.");
                }
                else
                {
                    player.AddGold(-item.iPrice);
                    player.Inventory.Add(item);
                    ShowShop("구매를 완료하였습니다.");
                    break;
                }
            }
        }
    }

    private void ShowShopSell() //상점 판매
    {
        Console.Clear();
        Utility.PrintColorLine("상점 - 아이템 판매", ConsoleColor.Yellow);
        if (player.Inventory.Count == 0)
            Console.WriteLine("보유하신 아이템이 없습니다.");
        else
            Console.WriteLine("판매하실 아이템을 선택하세요.\n");
        
        Console.WriteLine("[보유 골드]");
        Utility.PrintColorLine($"{player.iGold} G\n", ConsoleColor.Yellow);

        int index = 0;
        foreach (Item item in player.Inventory)
        {
            Console.Write("- ");
            Utility.PrintColor($"{index + 1}. ", ConsoleColor.Magenta);
            Console.Write(item);
            Utility.PrintColorLine($" (+{(int)(item.iPrice * 0.85f)} G)", ConsoleColor.Yellow);

            index++;
        }


        Console.WriteLine();

        Utility.PrintColorLine("0. 취소\n", ConsoleColor.Red);

        while (true)
        {
            int input = Utility.Confirm(player.Inventory.Count, 0);
            if (input == 0)
                ShowShop();
            else
            {
                Item item = player.Inventory[input - 1];

                if (item.isEquipped)
                    player.EquipItem(item);
                player.AddGold((int)(item.iPrice * 0.85f));
                player.Inventory.Remove(item);
                ShowShop($"판매를 완료하였습니다.(+{(int)(item.iPrice * 0.85f)} G)");
                break;
            }
        }
    }


    private void DungeonEntry()
    {
        Console.Clear();
        Utility.PrintColorLine("던전 입장", ConsoleColor.Yellow);
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");


        for (int i = 0; i < 3; i++)
        {
            Utility.PrintColor($"{i + 1}. ", ConsoleColor.Magenta);
            if (player.GetStats().Def >= dungeonDef[i])
                Console.WriteLine($"{dungeonDiff[i]} | 방어력 {dungeonDef[i]} 이상 권장");
            else
                Utility.PrintColorLine($"{dungeonDiff[i]} | 방어력 {dungeonDef[i]} 이상 권장");
        }

        Console.WriteLine();


        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);

        while (true)
        {
            int input = Utility.Confirm(3, 0);

            if (input == 0)
                MainScene();
            else
            {
                if (player.iCurHp <= 0)
                {
                    Utility.InfoMessage("체력이 부족합니다. 휴식 후 도전하세요.");
                    continue;
                }

                if (player.GetStats().Def < dungeonDef[input - 1])
                {
                    int rand = new Random().Next(0, 100);
                    if (rand < 40)
                    {
                        DungeonDefeated();
                        return;
                    }
                }

                DungeonClear(input - 1);
            }
        }
    }

    private int ResultHpCalc(int diffculty)
    {
        int min = 20 + (dungeonDef[diffculty] - player.GetStats().Def);
        int max = 35 + (dungeonDef[diffculty] - player.GetStats().Def);
        int rand = new Random().Next(min, max);
        return -rand;
    }

    private int ResultGoldCalc(int diffculty)
    {
        int[] reward = new int[] { 1000, 1700, 2500 };

        int min = player.GetStats().Atk; //(공격력 ~ 공겨력*2)% 추가보상
        int max = player.GetStats().Atk * 2;
        float rand = new Random().Next(min, max) / 100.0f;
        return reward[diffculty] + (int)(reward[diffculty] * rand);
    }

    private void DungeonClear(int diffculty = 0)
    {
        Console.Clear();
        Utility.PrintColorLine("던전 클리어", ConsoleColor.Yellow);
        Console.WriteLine($" 축하합니다!!\n{dungeonDiff[diffculty]}을 클리어 하였습니다.\n");

        Utility.PrintColorLine("[탐험 결과]", ConsoleColor.Yellow);

        int preHp = player.iCurHp;
        player.AddHp(ResultHpCalc(diffculty));
        Console.WriteLine($"체력 {preHp} -> {player.iCurHp}");

        int preGold = player.iGold;
        player.AddGold(ResultGoldCalc(diffculty));
        Console.WriteLine($"Gold {preGold} G -> {player.iGold} G");
        Console.WriteLine();

        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);
        int input = Utility.Confirm(3, 0);

        if (input == 0)
            MainScene();
    }

    private void DungeonDefeated()
    {
        Console.Clear();
        Utility.PrintColorLine("던전 실패", ConsoleColor.Red);
        Console.WriteLine("아쉽게도 실패 했습니다\n다음에 다시 도전하세요.\n");

        Utility.PrintColorLine("[탐험 결과]", ConsoleColor.Yellow);

        int preHp = player.iCurHp;
        player.AddHp(-(player.GetStats().Hp / 2)); //최대의 체력 절반 감소
        Console.WriteLine($"체력 {preHp} -> {player.iCurHp}");

        Console.WriteLine();

        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);
        int input = Utility.Confirm(3, 0);

        if (input == 0)
            MainScene();
    }

    private bool ClearRand(int diffculty) // 클리어 계산
    {
        return false;
    }

    private void ShowRest(bool rested = false)
    {
        Console.Clear();
        Utility.PrintColorLine("휴식하기", ConsoleColor.Yellow);
        Console.Write("500 G 를 내면 체력을 회복할 수 있습니다.\n");
        Utility.PrintColorLine($"(보유 골드 : {player.iGold} G)\n", ConsoleColor.Yellow);

        Utility.PrintColorLine("1. 휴식하기", ConsoleColor.Magenta);
        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);
        if (rested)
            Utility.InfoMessage("\n휴식을 완료했습니다. ", ConsoleColor.Blue);
        while (true)
        {
            if (Utility.Confirm(1, 0) == 0)
                MainScene();
            else
            {
                if (player.iCurHp == player.GetStats().Hp)
                {
                    Utility.InfoMessage("이미 최대 체력입니다.");
                }
                else if (player.iGold >= 500)
                {
                    player.AddGold(-500);
                    player.AddHp(100);
                    ShowRest(true);
                    break;
                }
                else
                {
                    Utility.InfoMessage("보유 골드가 부족합니다.");
                }
            }
        }
    }
}