using System;

namespace TextRPG;

class Program
{
    static Game game;

    static void Main(string[] args)
    {
        // 프로그램이 종료될 때 자동으로 SavePlayerData() 실행
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        game = new Game();
        game.Start();
    }
    
    static void OnProcessExit(object sender, EventArgs e)
    {
        DataLoader.SavePlayerData(game.player);
        Console.WriteLine("게임 데이터가 자동 저장되었습니다.");
    }
}