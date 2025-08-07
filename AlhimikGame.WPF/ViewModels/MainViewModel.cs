using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using AlhimikGame.Core.Patterns;
using AlhimikGame.Core.Patterns.State;
using AlhimikGame.WPF.Commands;
using AlhimikGame.WPF.Views;

namespace AlhimikGame.WPF.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private GameLevelFacade _gameLevelFacade;
    private WiseQuestioner _questioner;
    public ICommand NavigateCommand { get; set; }

    private UserControl _currentView;
    public UserControl CurrentView
    {
        get => _currentView;
        set { _currentView = value; OnPropertyChanged(); }
    }
    public MainViewModel()
    {
        _gameLevelFacade = new GameLevelFacade();
        _questioner = new WiseQuestioner("Npc questioner");
        NavigateCommand = new RelayCommand<string>(param => Navigate(param));
        Navigate("Alchemy"); // стартова сторінка
    }
    private void Navigate(string? destination)
    {
        switch (destination)
        {
            case "Alchemy":
                CurrentView = new AlchemyView();
                break;
            case "Location":
                CurrentView = new LocationView();
                break;
            case "Profile":
                CurrentView = new ProfileView();
                break;
            case "Levels":
                CurrentView = new GameLevelsView(_gameLevelFacade);
                break;
            case "NPCs":
                CurrentView = new NpcInteractionView(_questioner);
                break;
            default:
                break;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
