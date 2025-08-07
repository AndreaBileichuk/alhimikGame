using System.Windows.Controls;
using AlhimikGame.Core.Patterns.State;
using AlhimikGame.WPF.ViewModels;

namespace AlhimikGame.WPF.Views;

public partial class NpcInteractionView : UserControl
{
    public NpcInteractionView(WiseQuestioner questioner)
    {
        DataContext = new NpcInteractionViewModel(questioner);
        InitializeComponent();
    }
}