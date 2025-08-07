using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AlhimikGame.Core.Models;
using AlhimikGame.Core.Patterns;

namespace AlhimikGame.WPF.ViewModels
{
    public class ElixirCreationProcess : INotifyPropertyChanged
    {
        private Recipe _recipe;
        private GameWorld _gameWorld;
        private int _spendAmount;
        private ElixirFactory _currentFactory;
        private string _currentInstruction;
        private string _currentMessage;
        private bool _canStartMixing;
        
        private ObservableCollection<IngredientViewModel> _ingredients;
        public ObservableCollection<IngredientViewModel> Ingredients
        {
            get => _ingredients;
            set
            {
                _ingredients = value;
                OnPropertyChanged();
            }
        }
        
        public string CurrentInstruction
        {
            get => _currentInstruction;
            set
            {
                _currentInstruction = value;
                OnPropertyChanged();
            }
        }
        
        public bool CanStartMixing
        {
            get => _canStartMixing;
            set
            {
                _canStartMixing = value;
                OnPropertyChanged();
            }
        }

        public ElixirCreationProcess(Recipe recipe, int spendAmount, ElixirFactory currentFactory)
        {
            _recipe = recipe;
            _gameWorld = GameWorld.Instance;
            _spendAmount = spendAmount;
            _currentFactory = currentFactory;
            
            InitializeIngredients();
            
            UpdateCurrentInstructionMessage();
            
            CanStartMixing = false;
        }
        
        private void InitializeIngredients()
        {
            _ingredients = new ObservableCollection<IngredientViewModel>();
            
            foreach (var kvp in _recipe.Ingredients)
            {
                _ingredients.Add(new IngredientViewModel(kvp.Key, kvp.Value));
            }
        }
        
        private void UpdateCurrentInstructionMessage()
        {
            foreach (var ingredient in _ingredients)
            {
                if (ingredient.RemainingNeeded > 0)
                {
                    CurrentInstruction = $"Додай {ingredient.Name} в котел";
                    return;
                }
            }
            
            CurrentInstruction = "Всі інгредієнти додано! Почніть змішувати\n!";
            CanStartMixing = true;
        }
        
        public void AddIngredient(IngredientViewModel ingredient)
        {
            if (ingredient.RemainingNeeded > 0)
            {
                ingredient.Added++;
                
                SetMessage($"Додано {ingredient.Name}!");
                
                UpdateCurrentInstructionMessage();
                
                OnPropertyChanged(nameof(Ingredients));
            }
        }
        
        public void SetMessage(string message)
        {
            CurrentInstruction = message;
        }

        public void FinishCreation()
        {
            
            foreach (var kvp in _recipe.Ingredients)
            {
                var ingredient = kvp.Key;
                var requiredAmount = kvp.Value;
        
                _gameWorld.CurrentPlayer.Inventory[ingredient] -= requiredAmount;
        
                if (_gameWorld.CurrentPlayer.Inventory[ingredient] <= 0)
                {
                    _gameWorld.CurrentPlayer.Inventory.Remove(ingredient);
                }
            }
    
            _gameWorld.CurrentPlayer.Gold -= _spendAmount;
            
            ElixirBase craftedElixir = _recipe.CreateElixir(_spendAmount, _currentFactory);
            _gameWorld.CurrentPlayer.AddElixir(craftedElixir);
            
            ElixirCreated?.Invoke(this, craftedElixir);
        }
        
        public event System.EventHandler<ElixirBase> ElixirCreated;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IngredientViewModel : INotifyPropertyChanged
    {
        private Ingredient _ingredient;
        private int _quantity;
        private int _added;    

        public Ingredient Ingredient => _ingredient;
        public string Name => _ingredient.Name;
        public string Description => _ingredient.Description;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayText));
                    OnPropertyChanged(nameof(RemainingNeeded));
                }
            }
        }
        
        public int Added
        {
            get => _added;
            set
            {
                if (_added != value)
                {
                    _added = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayText));
                    OnPropertyChanged(nameof(RemainingNeeded));
                    OnPropertyChanged(nameof(IsComplete));
                }
            }
        }
        
        public int RemainingNeeded => _quantity - _added;
        
        public bool IsComplete => _added >= _quantity;

        public string DisplayText => $"{Name} × {Quantity} (Added: {Added})";

        public IngredientViewModel(Ingredient ingredient, int quantity)
        {
            _ingredient = ingredient;
            _quantity = quantity;
            _added = 0;  // Initially none added
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}