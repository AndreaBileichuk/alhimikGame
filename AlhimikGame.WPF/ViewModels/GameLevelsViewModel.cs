using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AlhimikGame.Core.Patterns;
using AlhimikGame.WPF.Commands;

namespace AlhimikGame.WPF.ViewModels;

public class GameLevelsViewModel : INotifyPropertyChanged
{
    private readonly GameLevelFacade _gameLevelFacade;
    private ObservableCollection<LevelViewModel> _levels;
    private LevelViewModel _selectedLevel;
    private SizeChangeDecorator _currentCharacter;
    private ObservableCollection<ElixirBase> _availableElixirs;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<LevelViewModel> Levels 
    { 
        get => _levels; 
        set 
        { 
            _levels = value;
            OnPropertyChanged();
        }
    }

    public LevelViewModel SelectedLevel 
    { 
        get => _selectedLevel; 
        set 
        { 
            _selectedLevel = value;
            OnPropertyChanged();
            LoadSelectedLevel();
        }
    }

    public SizeChangeDecorator CurrentCharacter 
    { 
        get => _currentCharacter; 
        set 
        { 
            _currentCharacter = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ElixirBase> AvailableElixirs 
    { 
        get => _availableElixirs; 
        set 
        { 
            _availableElixirs = value;
            OnPropertyChanged();
        }
    }

    public ICommand UseElixirCommand { get; }

    public GameLevelsViewModel(GameLevelFacade gameLevelFacade)
    {
        _gameLevelFacade = gameLevelFacade;
        _levels = new ObservableCollection<LevelViewModel>();
        _availableElixirs = new ObservableCollection<ElixirBase>(GameWorld.Instance.CurrentPlayer.Elixirs);

        for (int i = 1; i <= 6; i++)
        {
            CreateLevel(i);
        }

        UseElixirCommand = new RelayCommand<ElixirBase>(UseElixir, CanUseElixir);
    }

    private void CreateLevel(int levelNumber)
    {
        var levelProxy = _gameLevelFacade.CreateLevel(levelNumber);
        var levelViewModel = new LevelViewModel(levelProxy);
        Levels.Add(levelViewModel);
    }

    private void LoadSelectedLevel()
    {
        if (SelectedLevel == null) return;

        try 
        {
            var character = _gameLevelFacade.LoadLevel(SelectedLevel.LevelNumber);
            CurrentCharacter = character;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading level: {ex.Message}");
        }
    }

    private void UseElixir(ElixirBase elixir)
    {
        if (SelectedLevel == null || elixir == null) return;

        try 
        {
            if (!_gameLevelFacade.UseElixirForCharacter(SelectedLevel.LevelNumber, elixir))
            {
                MessageBox.Show("Цей еліксир не є обов'язковим для цього рівня");
                return;
            }
            AvailableElixirs.Remove(elixir);
            SelectedLevel.Refresh();
            
            if (SelectedLevel.IsCompleted)
            {
                GameWorld.Instance.CurrentPlayer.Level++;
                MessageBox.Show($"Рівень {SelectedLevel.LevelNumber} завершено!");
            }
            OnPropertyChanged(nameof(CurrentCharacter));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка під час використання еліксиру: {ex.Message}");
        }
    }

    private bool CanUseElixir(ElixirBase elixir)
    {
        return SelectedLevel != null && elixir != null;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class LevelViewModel : INotifyPropertyChanged
{
    private readonly LevelProxy _levelProxy;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int LevelNumber => _levelProxy.LevelNumber;
    public string Name => _levelProxy.Name;
    public bool IsCompleted => _levelProxy.IsCompleted;

    public ObservableCollection<ElixirBase> RequiredElixirs { get; }

    public LevelViewModel(LevelProxy levelProxy)
    {
        _levelProxy = levelProxy;
        RequiredElixirs = new ObservableCollection<ElixirBase>(levelProxy.RequiredElixirs);
    }

    public void Refresh()
    {
        OnPropertyChanged(nameof(IsCompleted));
        RequiredElixirs.Clear();
        foreach (var elixir in _levelProxy.RequiredElixirs)
        {
            RequiredElixirs.Add(elixir);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}