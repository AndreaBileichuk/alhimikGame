using AlhimikGame.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AlhimikGame.WPF.Commands;

namespace AlhimikGame.WPF.ViewModels;

public class ShopViewModel : INotifyPropertyChanged
{
    private readonly Player _player;
    private int _playerGold;

    public Shop CurrentShop { get; }
    public ObservableCollection<InventoryItemViewModel> InventoryItems { get; }

    public int PlayerGold
    {
        get => _playerGold;
        private set
        {
            if (_playerGold != value)
            {
                _playerGold = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand BuyCommand { get; }


    public ShopViewModel(Shop shop)
    {
        CurrentShop = shop;
        _player = GameWorld.Instance.CurrentPlayer;
        PlayerGold = _player.Gold;

        InventoryItems = new ObservableCollection<InventoryItemViewModel>(
            shop.Inventory.Select(kv => new InventoryItemViewModel(kv.Key, kv.Value.Quantity, kv.Value.Price))
        );

        BuyCommand = new RelayCommand<InventoryItemViewModel>(ExecuteBuy);
    }

    private void ExecuteBuy(InventoryItemViewModel item)
    {
        if (item == null || item.PurchaseQuantity <= 0)
            return;

        try
        {
            CurrentShop.ProcessPurchase(GameWorld.Instance.CurrentPlayer, item.Ingredient, item.PurchaseQuantity);
            ShowMessage($"Ви купили {item.PurchaseQuantity} {item.Ingredient.Name}(s)");
            item.Quantity -= item.PurchaseQuantity;
            item.PurchaseQuantity = 0;
            PlayerGold = _player.Gold;
        }
        catch (Exception e)
        {
            ShowMessage(e.Message);
        }
    }

    private void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class InventoryItemViewModel : INotifyPropertyChanged
{
    private int _quantity;
    private int _purchaseQuantity;

    public Ingredient Ingredient { get; }
    public int Price { get; }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
    }

    public int PurchaseQuantity
    {
        get => _purchaseQuantity;
        set
        {
            if (_purchaseQuantity != value)
            {
                _purchaseQuantity = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public InventoryItemViewModel(Ingredient ingredient, int quantity, int price)
    {
        Ingredient = ingredient;
        _quantity = quantity;
        Price = price;
        _purchaseQuantity = 0;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}