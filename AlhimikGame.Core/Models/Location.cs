namespace AlhimikGame.Core.Models;
public class Location
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<Ingredient, int> AvailableIngredients { get; private set; }
    public List<Shop> Shops { get; private set; }
        
    public Location(string name, string description)
    {
        Name = name;
        Description = description;
        AvailableIngredients = new Dictionary<Ingredient, int>();
        Shops = new List<Shop>();
    }
        
    public void AddIngredient(Ingredient ingredient, int quantity)
    {
        if (AvailableIngredients.ContainsKey(ingredient))
        {
            AvailableIngredients[ingredient] += quantity;
        }
        else
        {
            AvailableIngredients.Add(ingredient, quantity);
        }
    }
        
    public void AddShop(Shop shop)
    {
        Shops.Add(shop);
    }
}
