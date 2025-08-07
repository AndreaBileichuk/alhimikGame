// AlchemyViewModel.cs
using System.Collections.ObjectModel;
using System.Windows.Input;
using AlhimikGame.Core.Models;
using AlhimikGame.Core.Patterns;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using AlhimikGame.WPF.Commands;
using AlhimikGame.WPF.Views;

namespace AlhimikGame.WPF.ViewModels;

public class AlchemyViewModel : INotifyPropertyChanged
{
    private readonly GameWorld _gameWorld;
    private ElixirFactory _currentFactory;
    private string _craftingResult;
    private string _currentFactoryType;
    private RecipeViewModel _selectedRecipe;
    private int _spendAmount;
    
    public Dictionary<Ingredient, int> Inventory => _gameWorld.CurrentPlayer.Inventory;
    
    public ObservableCollection<RecipeViewModel> RecipeVMs => new ObservableCollection<RecipeViewModel>(
        _gameWorld.Recipes.Select(r => new RecipeViewModel(r, Inventory))
    );
    
    public ObservableCollection<RecipeViewModel> FilteredRecipeVMs => new ObservableCollection<RecipeViewModel>(
        _gameWorld.Recipes
            .Where(r => r.ResultType == CurrentFactoryType)
            .Select(r => new RecipeViewModel(r, Inventory))
    );
    
    // Properties
    public RecipeViewModel SelectedRecipe
    {
        get => _selectedRecipe;
        set
        {
            if (_selectedRecipe != value)
            {
                _selectedRecipe = value;
                OnPropertyChanged();
                _selectedRecipeFactory = _currentFactory;
            }
        }
    }

    private ElixirFactory _selectedRecipeFactory;
    public int SpendAmount
    {
        get => _spendAmount;
        set
        {
            if (_spendAmount != value)
            {
                _spendAmount = value;
                OnPropertyChanged();
            }
        }
    }
    public string CurrentFactoryType
    {
        get => _currentFactoryType;
        set
        {
            if (_currentFactoryType != value)
            {
                _currentFactoryType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilteredRecipeVMs));
            }
        }
    }
    public string CraftingResult
    {
        get => _craftingResult;
        set
        {
            if (_craftingResult != value)
            {
                _craftingResult = value;
                OnPropertyChanged();
            }
        }
    }
    
    // Commands
    public ICommand CraftCommand { get; private set; }
    public ICommand SwitchFactoryCommand { get; private set; }
    public ICommand SelectRecipeCommand { get; private set; }
    public ICommand SetSpendAmountCommand { get; private set; }

    public AlchemyViewModel()
    {
        _gameWorld = GameWorld.Instance;
        _currentFactory = new HealingElixirFactory();
        _currentFactoryType = "Лікування"; 
        _craftingResult = string.Empty;
        _spendAmount = 0;
        
        CraftCommand = new RelayCommand(CraftElixir, CanCraftElixir);
        SwitchFactoryCommand = new RelayCommand<string>(SwitchElixirFactory);
        SelectRecipeCommand = new RelayCommand<object>(SelectRecipe);
        SetSpendAmountCommand = new RelayCommand<string>(SetSpendAmount);
    }

    private void SetSpendAmount(string amount)
    {
        if (int.TryParse(amount, out int value))
        {
            SpendAmount = value;
        }
    }
    private void SelectRecipe(object parameter)
    {
        if (parameter is RecipeViewModel recipeVM)
        {
            SelectedRecipe = recipeVM;
        }
    }
    private bool CanCraftElixir()
    {
        if (SelectedRecipe == null) return false;
        
        // Check if player has enough gold
        if (_gameWorld.CurrentPlayer.Gold < SpendAmount)
            return false;
        
        // Check if player has enough ingredients
        foreach (var kvp in SelectedRecipe.Recipe.Ingredients)
        {
            var ingredient = kvp.Key;
            var requiredAmount = kvp.Value;
            
            if (!Inventory.ContainsKey(ingredient) || Inventory[ingredient] < requiredAmount)
            {
                return false;
            }
        }
        
        return true;
    }
    private void CraftElixir()
    {
        if (!CanCraftElixir()) return;
        ShowElixirCreationProcess(SpendAmount);
        SpendAmount = 0;
        NotifyInventoryChanged();
    }

    private void ShowElixirCreationProcess(int spendAmount)
    {
        var elixirCreationProcessViewModel = new ElixirCreationProcess(SelectedRecipe.Recipe,spendAmount, _selectedRecipeFactory);
        var elixirCreationView = new ElixirCreationProcessView
        {
            DataContext = elixirCreationProcessViewModel
        };

        Window elixirCreationWindow = new Window
        {
            Title = "Створення зілля",
            Content = elixirCreationView,
            Width = 800,
            Height = 500,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ResizeMode = ResizeMode.NoResize,
            Owner = Application.Current.MainWindow
        };
        elixirCreationProcessViewModel.ElixirCreated += (sender, @base) =>
        {
            elixirCreationWindow.Close();
            MessageBox.Show("Успішно створено еліксир, та додано його до вашого інвентаря");
        };
    
        elixirCreationWindow.ShowDialog();
    }
    public void SwitchElixirFactory(string factoryType)
    {
        switch (factoryType)
        {
            case "Healing":
                _currentFactory = new HealingElixirFactory();
                CurrentFactoryType = "Лікування";
                break;
            case "Mana":
                _currentFactory = new ManaElixirFactory();
                CurrentFactoryType = "Мана";
                break;
            case "Strength":
                _currentFactory = new StrengthElixirFactory();
                CurrentFactoryType = "Сила";
                break;
            case "Enhancement":
                _currentFactory = new EnhancementElixirFactory();
                CurrentFactoryType = "Посилення";
                break;
            case "Utility":
                _currentFactory = new UtilityElixirFactory();
                CurrentFactoryType = "Корисність";
                break;
            case "Mental":
                _currentFactory = new MentalElixirFactory();
                CurrentFactoryType = "Розумовий";
                break;
            default:
                _currentFactory = new HealingElixirFactory();
                CurrentFactoryType = "Лікування";
                break;
        }
    }
    private void NotifyInventoryChanged()
    {
        OnPropertyChanged(nameof(Inventory));
        OnPropertyChanged(nameof(RecipeVMs));
        OnPropertyChanged(nameof(FilteredRecipeVMs));
        OnPropertyChanged(nameof(_gameWorld.CurrentPlayer.Gold));
    }
    
    // Event for notifying the ui
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RecipeViewModel
{
    public Recipe Recipe { get; }
    public string InventoryStatus { get; }

    public RecipeViewModel(Recipe recipe, Dictionary<Ingredient, int> inventory)
    {
        Recipe = recipe;
        InventoryStatus = GenerateStatus(recipe, inventory);
    }

    private string GenerateStatus(Recipe recipe, Dictionary<Ingredient, int> inventory)
    {
        var status = "";
        foreach (var kvp in recipe.Ingredients)
        {
            var ingredient = kvp.Key;
            var required = kvp.Value;
            var available = inventory.ContainsKey(ingredient) ? inventory[ingredient] : 0;

            status += $"{ingredient.Name}: {available}/{required}\n";
        }
        return status.TrimEnd();
    }
}