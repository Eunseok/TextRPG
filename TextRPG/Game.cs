namespace TextRPG;

public class Game
{
    public Player player;
    private UIManager uiManager;


    public Game()
    {
        DataLoader.LoadItems();
        
        player = DataLoader.LoadPlayerData() ?? new Player();
        
        if (string.IsNullOrEmpty(player.strName))
        {
            player.CreateName();
            player.CreateJob();
        }
        uiManager = new UIManager(player);
        uiManager.ShopInventory = DataLoader.Items;
    }

    public void Start()
    {
        uiManager.MainScene();

    }
    
    

}