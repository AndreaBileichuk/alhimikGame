using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AlhimikGame.Core.Models;
using AlhimikGame.Core.Patterns;
using AlhimikGame.WPF.Commands;

namespace AlhimikGame.WPF.ViewModels;

public class ProfileViewModel : INotifyPropertyChanged
{
    private Player _player;
    public Player Player
    {
        get => _player;
        private set
        {
            if (_player != value)
            {
                _player = value;
                OnPropertyChanged();
            }
        }
    }
    public ICommand UseElixirCommand { get; private set; }
    public ProfileViewModel()
    {
        Player = GameWorld.Instance.CurrentPlayer;

        UseElixirCommand = new RelayCommand<ElixirBase>(UseElixir);

        // Subscribe to player property changes to keep the UI updated
        if (Player is INotifyPropertyChanged observable)
        {
            observable.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Player));
        }
    }

    private void UseElixir(ElixirBase elixir)
    {
        if (elixir != null && Player.Elixirs.Contains(elixir))
        {
            //elixir.Use();
            Player.Elixirs.Remove(elixir);

            // Notify UI that player data has changed
            OnPropertyChanged(nameof(Player));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}