using AlhimikGame.Core.Models;
using AlhimikGame.Core.Patterns;

public sealed class GameWorld
{
    private static GameWorld _instance;
    private static readonly object _lock = new object();

    public List<Location> Locations { get; private set; }
    public List<Recipe> Recipes { get; private set; }
    public Player CurrentPlayer { get; set; }

    private GameWorld()
    {
        Locations = new List<Location>();
        Recipes = new List<Recipe>();
        InitializeWorld();
    }

    public static GameWorld Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new GameWorld();
                }

                return _instance;
            }
        }
    }

    private void InitializeWorld()
    {
        // Create starting locations
        var forest = new Location("Ліс", "Густий ліс з багатьма рослинами та травами");
        var village = new Location("Село", "Маленьке село з місцевими крамницями");
        var university = new Location("Університет", "Місце навчання, де алхіміки діляться знаннями");
        var mountains = new Location("Гори", "Високі вершини з рідкісними мінералами та рослинами");
        var swamp = new Location("Болото", "Туманна заболочена місцевість з унікальною флорою");

        Locations.Add(forest);
        Locations.Add(village);
        Locations.Add(university);
        Locations.Add(mountains);
        Locations.Add(swamp);

        // Create expanded set of ingredients
        var herb = new Ingredient("Цілюща трава", "Поширена трава з невеликими лікувальними властивостями", 5);
        var mushroom = new Ingredient("Світний гриб", "Рідкісний гриб, який світиться в темряві", 12);
        var crystal = new Ingredient("Кристал мани", "Кристал, що містить магічну енергію", 25);
        var root = new Ingredient("Корінь женьшеню", "Корінь, що підвищує життєву силу та міць", 8);
        var flower = new Ingredient("Сонцецвіт", "Яскрава квітка, що поглинає сонячне світло", 10);
        var berry = new Ingredient("Ягода беладони", "Темна ягода з таємничими властивостями", 15);
        var leaf = new Ingredient("Срібний лист", "Листя з металевим блиском і відновлювальною силою", 18);
        var water = new Ingredient("Чиста джерельна вода", "Кришталево чиста вода з гірських джерел", 3);
        var salt = new Ingredient("Алхімічна сіль", "Очищена сіль з магічними властивостями", 7);
        var moss = new Ingredient("Світний мох", "Рідкісний мох, що випромінює світло", 14);
        var stone = new Ingredient("Фрагмент філософського каменю", "Крихітний шматочок легендарного каменю", 50);
        var dust = new Ingredient("Пил фей", "Магічний пил, зібраний з місць проживання фей", 30);
        var essence = new Ingredient("Есенція життя", "Концентрована життєва енергія", 35);

        // Distribute ingredients across locations
        forest.AddIngredient(herb, 10);
        forest.AddIngredient(mushroom, 3);
        forest.AddIngredient(berry, 5);
        forest.AddIngredient(leaf, 7);
        forest.AddIngredient(moss, 4);

        university.AddIngredient(crystal, 2);
        university.AddIngredient(salt, 8);
        university.AddIngredient(stone, 1);

        mountains.AddIngredient(flower, 6);
        mountains.AddIngredient(water, 15);
        mountains.AddIngredient(stone, 1);

        swamp.AddIngredient(root, 9);
        swamp.AddIngredient(moss, 8);
        swamp.AddIngredient(essence, 2);

        // Магазин в лісі
        var gnomeShop = new Shop("Магазин гнома",
            "В цьому магазині сховані магічні деталі темних лісів, в яких головними жителями залишились старі гноми з Рівії...");
        gnomeShop.AddInventoryItem(mushroom, 20, 10);
        gnomeShop.AddInventoryItem(moss, 15, 15);
        gnomeShop.AddInventoryItem(essence, 10, 20);
        forest.AddShop(gnomeShop);
        // Код для додавання в метод InitializeWorld() класу GameWorld

        // Магазин в Селі
        var villageApothecary = new Shop("Сільська аптека",
            "Затишна аптека, де місцеві травники продають свої найкращі зілля та інгредієнти.");
        villageApothecary.AddInventoryItem(herb, 25, 6); // Дешевше, бо село
        villageApothecary.AddInventoryItem(root, 12, 10);
        villageApothecary.AddInventoryItem(berry, 8, 18);
        villageApothecary.AddInventoryItem(water, 20, 4);
        village.AddShop(villageApothecary);

        // Магазин в Університеті
        var academicStore = new Shop("Академічна крамниця",
            "Елітний магазин, де продаються рідкісні компоненти для складних алхімічних експериментів.");
        academicStore.AddInventoryItem(crystal, 5, 28); // Дорожче, бо рідкісний товар
        academicStore.AddInventoryItem(salt, 15, 9);
        academicStore.AddInventoryItem(dust, 3, 35);
        academicStore.AddInventoryItem(stone, 1, 60); // Дуже рідкісний і дорогий
        university.AddShop(academicStore);

        // Магазин в Горах
        var dwarvenTrader = new Shop("Гірський торговець",
            "Загартований гном, який торгує рідкісними мінералами та ресурсами, видобутими глибоко в горах.");
        dwarvenTrader.AddInventoryItem(stone, 1, 55);
        dwarvenTrader.AddInventoryItem(crystal, 7, 27);
        dwarvenTrader.AddInventoryItem(flower, 10, 12);
        dwarvenTrader.AddInventoryItem(water, 30, 4); // Багато води в горах
        mountains.AddShop(dwarvenTrader);

        // Магазин в Болоті
        var witchHut = new Shop("Хатинка відьми",
            "Таємнича хатина на курячих ніжках, де стара відьма торгує загадковими компонентами, зібраними в найтемніших куточках болота.");
        witchHut.AddInventoryItem(moss, 12, 16);
        witchHut.AddInventoryItem(berry, 10, 17);
        witchHut.AddInventoryItem(essence, 4, 40);
        witchHut.AddInventoryItem(leaf, 8, 20);
        swamp.AddShop(witchHut);


        // Створення рецептів
        var builder = new RecipeBuilder("", "");

        // Recipe 1: Basic Health Potion
        var healthPotion = new RecipeBuilder("Мале зілля здоров'я", "Відновлює невелику кількість очок здоров'я")
            .SetDifficulty(1)
            .SetValue(15)
            .SetResultType("Лікування")
            .AddIngredient(herb, 2)
            .AddIngredient(water, 1)
            .Build();

        // Recipe 2: Mana Potion
        var manaPotion = new RecipeBuilder("Мале зілля мани", "Відновлює невелику кількість магічної енергії")
            .SetDifficulty(1)
            .SetValue(18)
            .SetResultType("Мана")
            .AddIngredient(crystal, 1)
            .AddIngredient(water, 1)
            .Build();

// Recipe 3: Greater Health Potion
        var greaterHealthPotion = new RecipeBuilder("Велике зілля здоров'я", "Відновлює значну кількість здоров'я")
            .SetDifficulty(2)
            .SetValue(35)
            .SetResultType("Лікування")
            .AddIngredient(herb, 3)
            .AddIngredient(root, 1)
            .AddIngredient(leaf, 2)
            .AddIngredient(water, 1)
            .Build();

        // Recipe 4: Strength Elixir
        var strengthElixir = new RecipeBuilder("Еліксир сили", "Тимчасово підвищує фізичну силу")
            .SetDifficulty(2)
            .SetValue(25)
            .SetResultType("Сила")
            .AddIngredient(root, 2)
            .AddIngredient(salt, 1)
            .AddIngredient(water, 1)
            .Build();

        // Recipe 5: Night Vision Potion
        var nightVisionPotion = new RecipeBuilder("Зілля нічного бачення", "Дозволяє бачити в темряві")
            .SetDifficulty(2)
            .SetValue(30)
            .SetResultType("Корисність")
            .AddIngredient(mushroom, 2)
            .AddIngredient(moss, 1)
            .AddIngredient(water, 1)
            .Build();

// Recipe 6: Invisibility Potion
        var invisibilityPotion =
            new RecipeBuilder("Зілля невидимості", "Робить користувача частково невидимим на короткий час")
                .SetDifficulty(4)
                .SetValue(80)
                .SetResultType("Корисність")
                .AddIngredient(dust, 1)
                .AddIngredient(moss, 2)
                .AddIngredient(berry, 3)
                .AddIngredient(water, 1)
                .Build();

// Recipe 7: Antidote
        var antidotePotion = new RecipeBuilder("Універсальний антидот", "Лікує більшість поширених отрут")
            .SetDifficulty(3)
            .SetValue(45)
            .SetResultType("Лікування")
            .AddIngredient(herb, 2)
            .AddIngredient(berry, 1)
            .AddIngredient(salt, 2)
            .AddIngredient(water, 1)
            .Build();

// Recipe 8: Intelligence Elixir
        var intelligenceElixir = new RecipeBuilder("Еліксир інтелекту", "Тимчасово підвищує розумові здібності")
            .SetDifficulty(3)
            .SetValue(50)
            .SetResultType("Розумовий")
            .AddIngredient(crystal, 1)
            .AddIngredient(flower, 2)
            .AddIngredient(salt, 1)
            .AddIngredient(water, 1)
            .Build();

        // Recipe 9: Alchemist's Wonder
        var alchemistWonder = new RecipeBuilder("Диво алхіміка", "Тимчасово покращує всі здібності")
            .SetDifficulty(5)
            .SetValue(150)
            .SetResultType("Посилення")
            .AddIngredient(stone, 1)
            .AddIngredient(essence, 1)
            .AddIngredient(dust, 1)
            .AddIngredient(root, 2)
            .AddIngredient(leaf, 2)
            .AddIngredient(water, 1)
            .Build();

        // Add all recipes to the game world
        Recipes.Add(healthPotion);
        Recipes.Add(manaPotion);
        Recipes.Add(greaterHealthPotion);
        Recipes.Add(strengthElixir);
        Recipes.Add(nightVisionPotion);
        Recipes.Add(invisibilityPotion);
        Recipes.Add(antidotePotion);
        Recipes.Add(intelligenceElixir);
        Recipes.Add(alchemistWonder);

        // Player creation
        CurrentPlayer = new Player("Новий Алхімік");
        CurrentPlayer.AddIngredient(herb, 3);
        CurrentPlayer.AddIngredient(mushroom, 1);
        CurrentPlayer.AddIngredient(water, 4);
        CurrentPlayer.AddIngredient(flower, 4);
        CurrentPlayer.AddIngredient(salt, 4);
        CurrentPlayer.AddIngredient(crystal, 4);
        CurrentPlayer.LearnRecipe(healthPotion);
        CurrentPlayer.LearnRecipe(manaPotion);
    }
}