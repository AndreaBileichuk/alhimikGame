using System.Windows.Media.Imaging;
using AlhimikGame.Core.Models;

namespace AlhimikGame.Core.Patterns;

public class GameLevelFacade
{
    private Dictionary<int, SizeChangeDecorator> _levelCharacters;
    private List<LevelProxy> _levelProxies;
    private Random _random;
    private List<Recipe> _availableRecipes;

    public GameLevelFacade()
    {
        _levelCharacters = new Dictionary<int, SizeChangeDecorator>();
        _levelProxies = new List<LevelProxy>();
        _random = new Random();
        _availableRecipes = GameWorld.Instance.Recipes;
    }

    public LevelProxy CreateLevel(int levelNumber)
    {
        if (levelNumber <= _levelProxies.Count)
        {
            return _levelProxies[levelNumber - 1];
        }

        var requiredElixirs = GenerateRequiredElixirsForLevel(levelNumber);
        var levelProxy = new LevelProxy($"Level_{levelNumber}", requiredElixirs, levelNumber);

        _levelProxies.Add(levelProxy);
        Console.WriteLine($"Created Level Proxy: {levelProxy.Name}");

        return levelProxy;
    }
    public SizeChangeDecorator LoadLevel(int levelNumber)
    {
        EnsureLevelExists(levelNumber);
        _levelProxies[levelNumber - 1].Load();
        return LoadCharacterForLevel(levelNumber);
    }
    public bool UseElixirForCharacter(int levelNumber, ElixirBase elixir)
    {
        EnsureLevelExists(levelNumber);

        _levelProxies[levelNumber - 1].Load();

        if (!_levelCharacters.ContainsKey(levelNumber))
        {
            throw new InvalidOperationException(
                "Character not loaded for this level. Call LoadCharacterForLevel first.");
        }

        
        var character = _levelCharacters[levelNumber];
        character.UseElixirWithSizeChange(elixir);

        var currentLevel = _levelProxies[levelNumber - 1];
        var usedElixirMatch = currentLevel.RequiredElixirs
            .FirstOrDefault(e => e.GetType() == elixir.GetType() && e.Type == elixir.Type && e.Name == elixir.Name);

        if (usedElixirMatch != null)
        {
            currentLevel.RequiredElixirs.Remove(usedElixirMatch);
            GameWorld.Instance.CurrentPlayer.Elixirs.Remove(elixir);            

            if (currentLevel.RequiredElixirs.Count == 0)
            {
                currentLevel.IsCompleted = true;
            }
        }
        else
        {
            return false;
        }

        return true;
    }
    private List<ElixirBase> GenerateRequiredElixirsForLevel(int levelNumber)
    {
        var requiredElixirs = new List<ElixirBase>();

        var availableRecipes = levelNumber switch
        {
            <= 3 => _availableRecipes.Where(r => r.Difficulty <= 2).ToList(),
            <= 6 => _availableRecipes.Where(r => r.Difficulty <= 4).ToList(),
            _ => _availableRecipes.ToList()
        };

        int elixirCount = _random.Next(2, Math.Min(4, availableRecipes.Count + 1));

        var selectedRecipes = availableRecipes
            .OrderBy(x => _random.Next())
            .Take(elixirCount)
            .ToList();

        foreach (var recipe in selectedRecipes)
        {
            ElixirBase elixir = CreateElixirFromRecipe(recipe);
            requiredElixirs.Add(elixir);
        }

        return requiredElixirs;
    }
    private ElixirBase CreateElixirFromRecipe(Recipe recipe)
    {
        ElixirFactory factory = recipe.ResultType switch
        {
            "Лікування" => new HealingElixirFactory(),
            "Мана" => new ManaElixirFactory(),
            "Сила" => new StrengthElixirFactory(),
            "Розумовий" => new MentalElixirFactory(),
            "Корисність" => new UtilityElixirFactory(),
            "Посилення" => new EnhancementElixirFactory(),
            _ => throw new ArgumentException($"Unknown elixir type: {recipe.ResultType}")
        };

        ElixirType elixirType = recipe.Difficulty switch
        {
            <= 2 => ElixirType.Base,
            <= 4 => ElixirType.Advanced,
            _ => ElixirType.Master
        };

        return elixirType switch
        {
            ElixirType.Base => factory.CreateBasicElixir(recipe.Name, recipe.Description, recipe.Value),
            ElixirType.Advanced => factory.CreateAdvancedElixir(recipe.Name, recipe.Description, recipe.Value),
            ElixirType.Master => factory.CreateMasterElixir(recipe.Name, recipe.Description, recipe.Value),
            _ => throw new ArgumentException($"Invalid elixir type: {elixirType}")
        };
    }
    private SizeChangeDecorator LoadCharacterForLevel(int levelNumber)
    {
        EnsureLevelExists(levelNumber);

        if (_levelCharacters.TryGetValue(levelNumber, out var existingCharacter))
        {
            Console.WriteLine(
                $"Reusing existing Character for {_levelProxies[levelNumber - 1].Name}: {existingCharacter.Name}");
            return existingCharacter;
        }

        string imagePath = $"/AlhimikGame.WPF;component/Assets/level{levelNumber}.jpg";
        var characterImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        characterImage.CacheOption = BitmapCacheOption.OnLoad;


        string name = "";
        switch (levelNumber)
        {
            case 1:
                name = "Mashmes";
                break;
            case 2:
                name = "Vinland cat";
                break;
            case 3:
                name = "Papus Jordan";
                break;
            case 4:
                name = "Bombini Gusini";
                break;
            case 5:
                name = "Yaroslav Mudriy";
                break;
            case 6:
                name = "Vidmakini Lapshini";
                break;
            default:
                name = "Tralalelo tralala";
                break;
        }
        
        var baseCharacter = new Character(
            name,
            100 + (levelNumber * 10),
            50 + (levelNumber * 5),
            characterImage
        );
        var character = new SizeChangeDecorator(baseCharacter);

        _levelCharacters[levelNumber] = character;

        Console.WriteLine($"Loaded Character for {_levelProxies[levelNumber - 1].Name}: {character.Name}");

        return character;
    }
    private void EnsureLevelExists(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > _levelProxies.Count)
        {
            throw new ArgumentException("Invalid level number. Create the level first using CreateLevel method.");
        }
    }
}