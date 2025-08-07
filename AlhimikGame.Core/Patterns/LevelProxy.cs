using AlhimikGame.Core.Patterns;

public abstract class LevelBase
{
    public abstract void Load();
    List<ElixirBase> RequiredElixirs { get; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
}

public class Level : LevelBase
{
    public List<ElixirBase> RequiredElixirs { get; private set; }

    public Level(string name, List<ElixirBase> requiredElixirs)
    {
        Name = name;
        RequiredElixirs = requiredElixirs;
        IsCompleted = false;
    }

    public override void Load()
    {
        Console.WriteLine($"Loading level: {Name}");
        Console.WriteLine("Required Elixirs:");
        foreach (var elixir in RequiredElixirs)
        {
            Console.WriteLine($"- {elixir.Name} (Type: {elixir.Type})");
        }
    }
}

public class LevelProxy : LevelBase
{
    private Level _realLevel;
    private bool _isLoaded = false;
    public string Name { get; private set; }
    public List<ElixirBase> RequiredElixirs { get; private set; }
    public bool IsCompleted { get; set; }
    public int LevelNumber { get; set; }

    public LevelProxy(string name, List<ElixirBase> requiredElixirs, int levelNmber)
    {
        Name = name;
        RequiredElixirs = requiredElixirs;
        IsCompleted = false;
        LevelNumber = levelNmber;
    }

    public override void Load()
    {
        if (!_isLoaded)
        {
            _realLevel = new Level(Name, RequiredElixirs);
            _realLevel.Load();
            _isLoaded = true;
        }
        else
        {
            Console.WriteLine($"Level {Name} already loaded");
        }
    }
}