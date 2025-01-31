// ItemLoader.cs
using Newtonsoft.Json;

namespace TextRPG;

public static class ItemLoader
{
    private static string filePath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "items.json");
   
    public static List<Item> Items { get; private set; } = new List<Item>();

    public static void LoadItems()
    {
       // Console.WriteLine($"현재 작업 디렉터리: {Directory.GetCurrentDirectory()}");
        //Console.WriteLine($"JSON 파일 예상 경로: {Path.GetFullPath(filePath)}");
        if (!File.Exists(filePath))
        {
            Console.WriteLine("아이템 데이터 파일이 없습니다.");
            return;
        }
        
        string json = File.ReadAllText(filePath);
        Items = JsonConvert.DeserializeObject<List<Item>>(json);
        Console.WriteLine("아이템 데이터 로드 완료!");
        
    }
}