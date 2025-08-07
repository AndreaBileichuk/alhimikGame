using AlhimikGame.Core.Models;

namespace AlhimikGame.Core.Patterns;

public class RecipeBuilder
{
    private Recipe _recipe;

    public RecipeBuilder(string name, string description)
    {
        Reset();
        _recipe.Name = name;
        _recipe.Description = description;
    }

    public void Reset()
    {
        _recipe = new Recipe();
    }

    public RecipeBuilder AddIngredient(Ingredient ingredient, int quantity)
    {
        _recipe.AddIngredient(ingredient, quantity);
        return this;
    }

    public RecipeBuilder SetDifficulty(int difficulty)
    {
        _recipe.Difficulty = difficulty;
        return this;
    }
    
    public RecipeBuilder SetValue(int value)
    {
        _recipe.Value = value;
        return this;
    }

    public RecipeBuilder SetResultType(string resultType)
    {
        _recipe.ResultType = resultType;
        return this;
    }

    public Recipe Build()
    {
        Recipe result = _recipe;
        Reset();
        return result;
    }
}