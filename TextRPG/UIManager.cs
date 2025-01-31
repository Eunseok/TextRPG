// UIManager.cs
namespace TextRPG;

public class UIManager
{
    private Player player;
    private Game game;
    
    public List<Item> ShopInventory { get; set; }

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
        
        string[] menu = { "상태보기", "인벤토리", "상점" };
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
        }
    }

    private void ShowStatus()
    {
        Console.Clear(); 
        Utility.PrintColorLine("상태 보기",ConsoleColor.Yellow);
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
        
        Console.WriteLine($"이름: {player.Name}\n직업: {player.Job}\n레벨: {player.Level}");
        Console.Write($"공격력: {player.Stats.Atk}");
        if(player.AddStats.Atk > 0)
            Utility.PrintColor($" +({player.AddStats.Atk})", ConsoleColor.Yellow);
        Console.Write($"\n방어력: {player.Stats.Def}");
        if(player.AddStats.Def > 0)
            Utility.PrintColor($" +({player.AddStats.Def})", ConsoleColor.Yellow);
        Console.Write($"\n체력: {player.Stats.Hp}");
        if(player.AddStats.Hp > 0)
            Utility.PrintColor($" +({player.AddStats.Hp})", ConsoleColor.Yellow);
        
        Console.WriteLine("\n");
        Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);
        Utility.Confirm(0, 0);
        MainScene();
    }

    private void ShowInventory()
    {
        Console.Clear();
        Utility.PrintColorLine("인벤토리 - 장착관리",ConsoleColor.Yellow);
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
        
        for (int i = 0; i < player.Inventory.Count; i++)
        {
            Utility.PrintColor($"{i + 1}. ", ConsoleColor.Magenta);
            if (player.Inventory[i].IsEquipped)
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

    private void ShowShop(bool choose = false, bool buy = false)
    {
        Console.Clear();
        Utility.PrintColorLine("상점", ConsoleColor.Yellow);
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

        Console.WriteLine("[보유 골드]");
        Utility.PrintColorLine($"{player.Gold} G\n", ConsoleColor.Yellow);

        int index = 0;

        foreach (Item item in ShopInventory)
        {
            Console.Write("- ");
            if (choose)
                Utility.PrintColor($"{index + 1}. ", ConsoleColor.Magenta);
            Console.Write(item);
            if (player.Inventory.Find(x => x.Id == item.Id) != null)
                Console.WriteLine(" [구매완료]");
            else if (player.Gold < item.Price)
                Utility.PrintColorLine($" ({item.Price} G)");
            else
                Utility.PrintColorLine($" ({item.Price} G)", ConsoleColor.Yellow);
            index++;
        }

        Console.WriteLine();
        if (!choose)
        {

            Utility.PrintColorLine("1. 아이템 구매", ConsoleColor.Magenta);
            Utility.PrintColorLine("0. 나가기\n", ConsoleColor.Magenta);
            if (buy)
                Utility.InfoMessage("\n구매를 완료 하였습니다.", ConsoleColor.Blue);
            if (Utility.Confirm(1, 0) == 1)
                ShowShop(true);
            else
                MainScene();
        }
        else
        {
            Utility.PrintColorLine("구매하실 아이템을 선택하세요.\n", ConsoleColor.Blue);
            
            Utility.PrintColorLine("0. 취소\n", ConsoleColor.Red);
            while (true)
            {
                int input = Utility.Confirm(ShopInventory.Count, 0);
                if (input == 0)
                    ShowShop();
                else
                {
                    Item item = ShopInventory[input - 1];
                    if (player.Inventory.Find(x => x.Id == item.Id) != null)
                    {
                        Utility.InfoMessage("이미 보유한 아이템 입니다..");
                    }
                    else if (player.Gold < item.Price)
                    {
                        Utility.InfoMessage("보유 골드가 부족합니다.");
                    }
                    else
                    {
                        player.Gold -= item.Price;
                        player.Inventory.Add(item);
                        ShowShop(false, true);
                        break;

                    }
                }
            }

            ShowShop();
        }
    }
}
