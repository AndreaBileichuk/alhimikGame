using AlhimikGame.Core.Models;
using System;

namespace AlhimikGame.Core.Patterns;

public enum ElixirType
{
    Base,
    Advanced,
    Master
};

public abstract class ElixirBase
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int Value { get; protected set; }
    public ElixirType Type { get; set; }
    
    protected IElixirEffect _elixirEffect;

    public ElixirBase(string name, string description, int value, ElixirType type, IElixirEffect elixirEffect)
    {
        Name = name;
        Description = description;
        Value = value;
        Type = type;
        _elixirEffect = elixirEffect;
        
        // Also putting elixir effect description to the general description
        Description = _elixirEffect.GetEffectDescription();
    }

    public void Use(Character character)
    {
        _elixirEffect.Apply(character);
    }
}

// Concrete Products
public class HealingElixir : ElixirBase
{
    public int HealingPower { get; private set; }

    public HealingElixir(string name, string description, int value, int healingPower, ElixirType type) : base(name,description, value, type, new HealingElixirEffect(healingPower))
    {
        HealingPower = healingPower;
    }
}

public class ManaElixir : ElixirBase
{
    public int ManaPower { get; private set; }

    public ManaElixir(string name, string description, int value, int manaPower, ElixirType type) : base(name,description, value, type, new ManaElixirEffect(manaPower))
    {
        ManaPower = manaPower;
    }
}

public class StrengthElixir : ElixirBase
{
    public int StrengthBoost { get; private set; }
    public int Duration { get; private set; }

    public StrengthElixir(string name, string description, int value, int strengthBoost, int duration, ElixirType type) : base(name,description, value, type, new StrengthElixirEffect(strengthBoost))
    {
        StrengthBoost = strengthBoost;
        Duration = duration;
    }
}

public class MentalElixir : ElixirBase
{
    public int IntelligenceBoost { get; private set; }
    public int Duration { get; private set; }

    public MentalElixir(string name, string description, int value, int intelligenceBoost, int duration, ElixirType type) : base(name,description, value, type, new MentalElixirEffect(intelligenceBoost))

    {
        IntelligenceBoost = intelligenceBoost;
        Duration = duration;
    }
}

public class EnhancementElixir : ElixirBase
{
    public int DefenseBoost { get; private set; }
    public int Duration { get; private set; }

    public EnhancementElixir(string name, string description, int value, int defenseBoost, int duration,
        ElixirType type) : base(name, description, value, type, new EnhancementElixirEffect(defenseBoost))
    {
        DefenseBoost = defenseBoost;
        Duration = duration;
    }
}

public class UtilityElixir : ElixirBase
{
    public int Duration { get; private set; }

    public UtilityElixir(string name, string description, int value, int duration, ElixirType type) : base(name,description, value, type, new GeneralElixirEffect(2))
    {
        Duration = duration;
    }
}

// Abstract Factory
public abstract class ElixirFactory
{
    public abstract ElixirBase CreateBasicElixir(string name, string description, int value);
    public abstract ElixirBase CreateAdvancedElixir(string name, string description, int value);
    public abstract ElixirBase CreateMasterElixir(string name, string description, int value);
}

// Concrete Factories
public class HealingElixirFactory : ElixirFactory
{
    public override ElixirBase CreateBasicElixir(string name, string description, int value)
    {
        return new HealingElixir(name, description, value, 20, ElixirType.Base);
    }
    
    public override ElixirBase CreateAdvancedElixir(string name, string description, int value)
    {
        return new HealingElixir(name, description, value, 50, ElixirType.Advanced);
    }
    
    public override ElixirBase CreateMasterElixir(string name, string description, int value)
    {
        return new HealingElixir(name, description, value, 100, ElixirType.Master);
    }
}

public class ManaElixirFactory : ElixirFactory
{
    public override ElixirBase CreateBasicElixir(string name, string description, int value)
    {
        return new ManaElixir(name, description, value, 15, ElixirType.Base);
    }
    
    public override ElixirBase CreateAdvancedElixir(string name, string description, int value)
    {
        return new ManaElixir(name, description, value, 40, ElixirType.Advanced);
    }
    
    public override ElixirBase CreateMasterElixir(string name, string description, int value)
    {
        return new ManaElixir(name, description, value, 80, ElixirType.Master);
    }
}

public class StrengthElixirFactory : ElixirFactory
{
    public override ElixirBase CreateBasicElixir(string name, string description, int value)
    {
        return new StrengthElixir(name, description, value, 2, 3, ElixirType.Base);
    }
    
    public override ElixirBase CreateAdvancedElixir(string name, string description, int value)
    {
        return new StrengthElixir(name, description, value, 4, 5, ElixirType.Advanced);
    }
    
    public override ElixirBase CreateMasterElixir(string name, string description, int value)
    {
        return new StrengthElixir(name, description, value, 8, 7, ElixirType.Master);
    }
}

public class MentalElixirFactory : ElixirFactory
{
    public override ElixirBase CreateBasicElixir(string name, string description, int value)
    {
        return new MentalElixir(name, description, value, 2, 3, ElixirType.Base);
    }
    
    public override ElixirBase CreateAdvancedElixir(string name, string description, int value)
    {
        return new MentalElixir(name, description, value, 4, 5, ElixirType.Advanced);
    }
    
    public override ElixirBase CreateMasterElixir(string name, string description, int value)
    {
        return new MentalElixir(name, description, value, 8, 7, ElixirType.Master);
    }
}

public class EnhancementElixirFactory : ElixirFactory
{
    public override ElixirBase CreateBasicElixir(string name, string description, int value)
    {
        return new EnhancementElixir(name, description, value, 2, 3, ElixirType.Base);
    }
    
    public override ElixirBase CreateAdvancedElixir(string name, string description, int value)
    {
        return new EnhancementElixir(name, description, value, 4, 5, ElixirType.Advanced);
    }
    
    public override ElixirBase CreateMasterElixir(string name, string description, int value)
    {
        return new EnhancementElixir(name, description, value, 8, 7, ElixirType.Master);
    }
}

public class UtilityElixirFactory : ElixirFactory
{
    public override ElixirBase CreateBasicElixir(string name, string description, int value)
    {
        return new UtilityElixir(name, description, value, 2, ElixirType.Base);
    }
    
    public override ElixirBase CreateAdvancedElixir(string name, string description, int value)
    {
        return new UtilityElixir(name, description, value, 4, ElixirType.Advanced);
    }
    
    public override ElixirBase CreateMasterElixir(string name, string description, int value)
    {
        return new UtilityElixir(name, description, value, 6, ElixirType.Master);
    }
}