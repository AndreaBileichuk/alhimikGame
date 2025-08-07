using AlhimikGame.Core.Patterns.ChainOfResponsibility;

namespace AlhimikGame.Core.Models;

public class Shop
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<Ingredient, (int Quantity, int Price)> Inventory { get; private set; }

    public Shop(string name, string description)
    {
        Name = name;
        Description = description;
        Inventory = new Dictionary<Ingredient, (int Quantity, int Price)>();
    }

    public void AddInventoryItem(Ingredient ingredient, int quantity, int price)
    {
        if (Inventory.ContainsKey(ingredient))
        {
            var currentData = Inventory[ingredient];
            Inventory[ingredient] = (currentData.Quantity + quantity, price);
        }
        else
        {
            Inventory.Add(ingredient, (quantity, price));
        }
    }
    
    public void ProcessPurchase(Player player, Ingredient ingredient, int quantity)
    {
        var inventoryCheck = new InventoryCheckHandler();
        var fundsCheck = new FundsCheckHandler();
        var completePurchase = new CompletePurchaseHandler();

        inventoryCheck.SetNext(fundsCheck);
        fundsCheck.SetNext(completePurchase);

        inventoryCheck.Handle(player, this, ingredient, quantity);
    }

    public void PurchaseItem(Ingredient ingredient, int quantity)
    {
        var data = Inventory[ingredient];
        int totalCost = data.Price * quantity;
        GameWorld.Instance.CurrentPlayer.Gold -= totalCost;
        
        var currentData = Inventory[ingredient];

        int newQuantity = currentData.Quantity - quantity;

        if (newQuantity > 0)
        {
            Inventory[ingredient] = (newQuantity, currentData.Price);
        }
        else
        {
            Inventory.Remove(ingredient);
        }
    }
}