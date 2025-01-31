namespace TextRPG;

public class Game
{
    private Player player;
    private UIManager uiManager;


    public Game()
    {
        player = new Player();
        uiManager = new UIManager(player, this);
    }

    public void Start()
    {
        ItemLoader.LoadItems();
        uiManager.ShopInventory = ItemLoader.Items;

        player.CreateName();
        player.CreateJob();
        uiManager.MainScene();
    }
}