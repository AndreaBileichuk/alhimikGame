using System.Diagnostics;
using AlhimikGame.Core.Patterns;

namespace AlhimikGame.Core.Models;

public class Recipe
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<Ingredient, int> Ingredients { get; private set; }
    public List<string> Steps { get; private set; }
    public int Difficulty { get; set; }
    public int Value { get; set; }
    public string ResultType { get; set; }
        
    public Recipe()
    {
        Ingredients = new Dictionary<Ingredient, int>();
        Steps = new List<string>();
    }
        
    public Recipe(string name, string description)
    {
        Name = name;
        Description = description;
        Ingredients = new Dictionary<Ingredient, int>();
    }
        
    public void AddIngredient(Ingredient ingredient, int quantity)
    {
        if (Ingredients.ContainsKey(ingredient))
        {
            Ingredients[ingredient] += quantity;
        }
        else
        {
            Ingredients.Add(ingredient, quantity);
        }
    }

    public void AddSteps(string step)
    {
        Steps.Add(step);
    } 
        
    public ElixirBase CreateElixir(int spendAmount, ElixirFactory factory)
    {
        ElixirBase elixir;
        if (spendAmount == 100)
        {
            elixir = factory.CreateMasterElixir(Name, Description, Value);
        }
        else if (spendAmount == 50)
        {
            elixir = factory.CreateAdvancedElixir(Name, Description, Value);
        }
        else
        {
            elixir = factory.CreateBasicElixir(Name, Description, Value);
        }
        
        return elixir;
    }
}
