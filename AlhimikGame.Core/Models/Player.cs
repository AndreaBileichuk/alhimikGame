using AlhimikGame.Core.Patterns;

namespace AlhimikGame.Core.Models;

public class Player
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Gold { get; set; }
    public Dictionary<Ingredient, int> Inventory { get; private set; }
    public List<ElixirBase> Elixirs { get; private set; }
    public List<Recipe> KnownRecipes { get; private set; }
    public Player(string name)
    {
        Name = name;
        Level = 1;
        Experience = 0;
        Gold = 500;

        Inventory = new Dictionary<Ingredient, int>();
        Elixirs = new List<ElixirBase>();
        KnownRecipes = new List<Recipe>();
    }

    public void AddIngredient(Ingredient ingredient, int quantity)
    {
        if (Inventory.ContainsKey(ingredient))
        {
            Inventory[ingredient] += quantity;
        }
        else
        {
            Inventory.Add(ingredient, quantity);
        }
    }
    public void AddElixir(ElixirBase elixir)
    {
        Elixirs.Add(elixir);
    }
    public void LearnRecipe(Recipe recipe)
    {
        if (!KnownRecipes.Contains(recipe))
        {
            KnownRecipes.Add(recipe);
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }
}