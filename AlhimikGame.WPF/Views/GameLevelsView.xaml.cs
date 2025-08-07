using System.Windows.Controls;
using AlhimikGame.Core.Patterns;
using AlhimikGame.WPF.ViewModels;

namespace AlhimikGame.WPF.Views;

public partial class GameLevelsView : UserControl
{
    public GameLevelsView(GameLevelFacade gameLevelFacade)
    {
        DataContext = new GameLevelsViewModel(gameLevelFacade);
        InitializeComponent();
    }
}