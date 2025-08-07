namespace AlhimikGame.Core.Patterns;

public interface IElixirEffect
{
    void Apply(Character character);
    string GetEffectDescription();
}

public class HealingElixirEffect : IElixirEffect
{
    private int _healingPower;
    
    public HealingElixirEffect(int healingPower)
    {
        _healingPower = healingPower;
    }

    public void Apply(Character character)
    {
        Console.WriteLine("Applying healing power to the character!");
        character.Health += _healingPower;
    }

    public string GetEffectDescription()
    {
        return $"This effect heals character's health by {_healingPower} points";
    }
}

public class ManaElixirEffect : IElixirEffect
{
    private int _manaPower;
    
    public ManaElixirEffect(int manaPower)
    {
        _manaPower = manaPower;
    }
    
    public void Apply(Character character)
    {
        Console.WriteLine("Applying mana power to the character!");
        character.Mana += _manaPower;
    }

    public string GetEffectDescription()
    {        
        return $"This effect boosts character's mana by {_manaPower} points";
    }
}

public class StrengthElixirEffect : IElixirEffect
{
    private int _strengthBoost;
    
    public StrengthElixirEffect(int strengthBoost)
    {
        _strengthBoost = strengthBoost;
    }
    
    public void Apply(Character character)
    {
        Console.WriteLine("Applying strength boost to the character!");
        character.Strength += _strengthBoost;
    }

    public string GetEffectDescription()
    {        
        return $"This effect boosts character's strength by {_strengthBoost} points";
    }
}

public class MentalElixirEffect : IElixirEffect
{
    private int _mentalBoost;
    
    public MentalElixirEffect(int mentalBoost)
    {
        _mentalBoost = mentalBoost;
    }
    
    public void Apply(Character character)
    {
        Console.WriteLine("Applying intelligence boost to the character!");
        character.Intelligence += _mentalBoost;
    }

    public string GetEffectDescription()
    {        
        return $"This effect boosts character's intelligence by {_mentalBoost} points";
    }
}

public class EnhancementElixirEffect : IElixirEffect
{
    private int _defenseBoost;
    
    public EnhancementElixirEffect(int defenseBoost)
    {
        _defenseBoost = defenseBoost;
    }
    
    public void Apply(Character character)
    {
        Console.WriteLine("Applying defense boost to the character!");
        character.Defense += _defenseBoost;
    }

    public string GetEffectDescription()
    {        
        return $"This effect boosts character's defense by {_defenseBoost} points";
    }
}

public class GeneralElixirEffect : IElixirEffect
{
    private int _boostFactor;
    public GeneralElixirEffect(int boostFactor)
    {
        _boostFactor = boostFactor;
    }
    public void Apply(Character character)
    {
        Console.WriteLine("Applying defense boost to the character!");
        character.Health += _boostFactor;
        character.Mana += _boostFactor;
        character.Strength += _boostFactor;
        character.Defense += _boostFactor;
        character.Intelligence += _boostFactor;
    }

    public string GetEffectDescription()
    {        
        return $"This effect boosts each character's parameter by {_boostFactor} points";
    }
}

public class NoElixirEffect : IElixirEffect
{
    public void Apply(Character character)
    {
        Console.WriteLine("This elixir has no effect!");
    }

    public string GetEffectDescription()
    {        
        return $"This elixir has no effect";
    }
}  