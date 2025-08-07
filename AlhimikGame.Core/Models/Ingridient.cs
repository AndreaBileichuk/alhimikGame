namespace AlhimikGame.Core.Models;

public class Ingredient
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int BaseValue { get; set; }
        
    public Ingredient(string name, string description, int baseValue)
    {
        Name = name;
        Description = description;
        BaseValue = baseValue;
    }
}