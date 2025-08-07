using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AlhimikGame.Core.Models;
using AlhimikGame.WPF.Commands;
using AlhimikGame.WPF.Views;

namespace AlhimikGame.WPF.ViewModels;

public class LocationViewModel : INotifyPropertyChanged
{
    private Location _currentLocation;
    private ObservableCollection<KeyValuePair<Ingredient, int>> _availableIngredients;
    public ObservableCollection<Location> Locations { get; } = new ObservableCollection<Location>(GameWorld.Instance.Locations);
    
    public Location CurrentLocation
    {
        get => _currentLocation;
        set
        {
            if (_currentLocation != value)
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                UpdateAvailableIngredients();
                OnPropertyChanged(nameof(HasShops));
            }
        }
    }
    
    public ObservableCollection<KeyValuePair<Ingredient, int>> AvailableIngredients
    {
        get => _availableIngredients;
        private set
        {
            _availableIngredients = value;
            OnPropertyChanged(nameof(AvailableIngredients));
        }
    }
    
    public bool HasShops => CurrentLocation?.Shops.Count > 0;
    public ICommand SelectLocationCommand { get; }
    public ICommand GatherIngredientCommand { get; }
    public ICommand VisitShopCommand { get; }
    public ICommand TalkToNPCCommand { get; }
    
    public LocationViewModel()
    {
        _availableIngredients = new ObservableCollection<KeyValuePair<Ingredient, int>>();
        if (Locations.Count > 0)
        {
            CurrentLocation = Locations[0];
        }
        
        SelectLocationCommand = new RelayCommand<Location>(location => CurrentLocation = location);
        GatherIngredientCommand = new RelayCommand<KeyValuePair<Ingredient, int>>(GatherIngredient);
        VisitShopCommand = new RelayCommand<Shop>(VisitShop);
    }
    
    private void UpdateAvailableIngredients()
    {
        AvailableIngredients = new ObservableCollection<KeyValuePair<Ingredient, int>>(
            CurrentLocation?.AvailableIngredients ?? new Dictionary<Ingredient, int>());
    }
    
    private void GatherIngredient(KeyValuePair<Ingredient, int> ingredientPair)
    {
        Player player = GameWorld.Instance.CurrentPlayer;
        player.AddIngredient(ingredientPair.Key, ingredientPair.Value);
        CurrentLocation?.AvailableIngredients.Remove(ingredientPair.Key);
        UpdateAvailableIngredients();
    }
    
    public void VisitShop(Shop shop)
    {
        var shopViewModel = new ShopViewModel(shop);
        var shopView = new ShopView
        {
            DataContext = shopViewModel
        };

        var shopWindow = new Window
        {
            Title = "Крамниця",
            Content = shopView,
            Width = 600,
            Height = 400,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ResizeMode = ResizeMode.NoResize,
            Owner = Application.Current.MainWindow 
        };

        shopWindow.ShowDialog(); 
    }
    
    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}