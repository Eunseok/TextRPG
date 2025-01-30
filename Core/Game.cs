namespace Core;

public class Game
{
    private Player player = new Player();

    private string[] jobType = new string[] { "전사", "도적" };
    //private string strInfo = "스파르타 던전에 오신 여러분 환영합니다.";

    public void Start()
    {
        CreateName(); 
        CreateJob();
    }

    private void CreateName()
    {
        Console.Clear();
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        Console.WriteLine("원하시는 이름을 설정해 주세요.\n");
        Utility.PrintColor(">> ", ConsoleColor.Yellow);
        
        string name = Console.ReadLine();
        Utility.RemoveLine();
        Console.WriteLine($"선택하신 이름은 \"{name}\" 입니다.\n");
        
        Console.WriteLine("저장하시겠습니까? (1. 저장 / 2. 취소)");
        
        if (Confirm(2) == 1)
            player.name = name;
        else
            CreateName();

    }
    
    private void CreateJob()
    {
        Console.Clear();
        Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        Console.WriteLine("원하시는 직업을 설정해 주세요.\n");
        
        for (int i = 0; i < jobType.Length; i++)
            Console.WriteLine($"{i+1}. {jobType[i]}");
        Utility.CleanEnter();

        string job = jobType[Confirm(2)-1]; //1.전사, 2.도적
        
        Utility.RemoveLine();
        Console.WriteLine($"선택하신 직업은 \"{job}\" 입니다.");
        Utility.CleanEnter();
        Console.WriteLine("저장하시겠습니까? (1. 저장 / 2. 취소)");
        if(Confirm(2) == 1)
            player.job = job;
        else
            CreateJob();
        
    }

    private int Confirm(int range)
    {
        while (true)
        {
            Utility.PrintColor(">> ", ConsoleColor.Yellow);
            
            int num;
            if (!int.TryParse(Console.ReadLine(), out num)
                || num < 0 || num > range)
            {
                Utility.RemoveLine(1);
                ErrorMessage();
                continue;
            }

            return num;
        }
    }

    private void ErrorMessage(string message = "잘못된 입력입니다.")
    {
        
        Console.SetCursorPosition(0,Console.CursorTop+1);
        Utility.PrintColor(message);
        Console.SetCursorPosition(0,Console.CursorTop-1);
    }
}