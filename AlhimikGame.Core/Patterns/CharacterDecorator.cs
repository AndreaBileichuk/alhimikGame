using AlhimikGame.Core.Patterns;
using System;
using System.Windows.Media.Imaging; // For WPF image handling

public class Character
{
    public string Name { get; protected set; }
    public float Health { get; set; }
    public float Mana { get; set; }
    public float Strength { get; set; }
    public float Intelligence { get; set; }
    public float Defense { get; set; }
    public BitmapImage CharacterImage { get; protected set; }

    public Character(string name, float health, float mana, BitmapImage characterImage)
    {
        Name = name;
        Health = health;
        Mana = mana;
        CharacterImage = characterImage;
    }
}

public abstract class CharacterDecorator : Character
{
    public CharacterDecorator(Character character) : base(character.Name, character.Health, character.Health, character.CharacterImage)
    {
        Health = character.Health;
        Mana = character.Mana;
        Strength = character.Strength;
        Intelligence = character.Intelligence;
        Defense = character.Defense;
    }
}

public class SizeChangeDecorator : CharacterDecorator
{
    public float Size { get; private set; } = 1.0f;

    public SizeChangeDecorator(Character character) : base(character)
    {
    }

    private void IncreaseSize(float sizeIncrease)
    {
        Size += sizeIncrease;
        Console.WriteLine($"{Name}'s size increased to {Size}x");
    }

    public void UseElixirWithSizeChange(ElixirBase elixir)
    {
        elixir.Use(this);

        float sizeIncrease = elixir.Type switch
        {
            ElixirType.Base => 0.2f,
            ElixirType.Advanced => 0.5f,
            ElixirType.Master => 1.0f,
            _ => 0.1f
        };

        IncreaseSize(sizeIncrease);
    }
}