using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AlhimikGame.Core.Patterns.State;
using AlhimikGame.WPF.Commands;

namespace AlhimikGame.WPF.ViewModels
{
    public abstract class QuizContent
    {
        public NpcInteractionViewModel Parent { get; set; }
    }

    public class GreetingContent : QuizContent
    {
        public string NpcName => Parent.NpcName;
        public string GreetingText => Parent.QuestionerGreeting;
        public ICommand StartCommand => Parent.StartQuizCommand;
    }

    public class QuestionContent : QuizContent
    {
        public string QuestionText => Parent.CurrentQuestion.Text;
        
        public List<AnswerOption> Answers { get; } = new List<AnswerOption>();
        
        public ICommand SubmitCommand => Parent.AnswerCommand;
    }

    public class AnswerOption
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
    }

    public class ResultContent : QuizContent
    {
        public string ResultText => Parent.ResultMessage;
        public string NextButtonText => Parent.QuizCompleted ? "Finish" : "Next Question";
        public ICommand NextCommand => Parent.QuizCompleted ? Parent.CompleteQuizCommand : Parent.NextQuestionCommand;
    }

    public class NpcInteractionViewModel : INotifyPropertyChanged
{
    private readonly WiseQuestioner _questioner;
    private int _earnedGold;
    private QuizContent _currentContent;

    public NpcInteractionViewModel(WiseQuestioner questioner)
    {
        _questioner = questioner;
        _questioner.UpdateState(GameWorld.Instance.CurrentPlayer.Level);
        
        StartQuizCommand = new RelayCommand(StartQuiz);
        AnswerCommand = new RelayCommand(SubmitAnswer);
        NextQuestionCommand = new RelayCommand(NextQuestion);
        CompleteQuizCommand = new RelayCommand(CompleteQuiz);
        
        ShowAppropriateContent();
    }

    private void ShowAppropriateContent()
    {
        if (_questioner.IsQuizCompleted() || _questioner.GetCurrentQuestionIndex() == 0 && _questioner.GetCorrectAnswers() == 0)
        {
            ShowGreeting();
        }
        else
        {
            ShowQuestion();
        }
    }

    public string NpcName => _questioner.GetName();
    public string QuestionerGreeting => _questioner.Greet();
    
    public Question CurrentQuestion => _questioner.GetCurrentQuestion();
    public string ResultMessage { get; private set; }
    public int CorrectAnswers => _questioner.GetCorrectAnswers();
    public int TotalQuestions => _questioner.GetQuestions().Count;
    public int EarnedGold => _earnedGold;
    public bool QuizCompleted => _questioner.IsQuizCompleted();

    public QuizContent CurrentContent
    {
        get => _currentContent;
        set
        {
            _currentContent = value;
            OnPropertyChanged();
        }
    }

    public ICommand StartQuizCommand { get; }
    public ICommand AnswerCommand { get; }
    public ICommand NextQuestionCommand { get; }
    public ICommand CompleteQuizCommand { get; }

    private void ShowGreeting()
    {
        CurrentContent = new GreetingContent { Parent = this };
    }

    private void ShowQuestion()
    {
        var questionContent = new QuestionContent { Parent = this };
        
        foreach (var answer in CurrentQuestion.PossibleAnswers)
        {
            questionContent.Answers.Add(new AnswerOption { Text = answer });
        }
        
        CurrentContent = questionContent;
    }

    private void ShowResult()
    {
        CurrentContent = new ResultContent { Parent = this };
    }

    private void StartQuiz()
    {
        _questioner.ResetQuiz();
        ShowQuestion();
    }

    private void SubmitAnswer()
    {
        var questionContent = CurrentContent as QuestionContent;
        if (questionContent == null) return;

        var selectedIndex = questionContent.Answers.FindIndex(a => a.IsSelected);
        if (selectedIndex == -1)
        {
            ResultMessage = "Будь ласка, оберіть відповідь!";
            ShowResult();
            return;
        }

        bool isCorrect = selectedIndex == CurrentQuestion.CorrectAnswerIndex;
    
        bool hasMoreQuestions = _questioner.MoveToNextQuestion(isCorrect);
    
        ResultMessage = isCorrect
            ? "Правильно!"
            : $"Неправильно. Правильна відповідь: {CurrentQuestion.PossibleAnswers[CurrentQuestion.CorrectAnswerIndex]}";

        if (_questioner.IsQuizCompleted())
        {
            bool passedTest = CorrectAnswers >= TotalQuestions / 2;
            _earnedGold = _questioner.CalculateReward(CorrectAnswers);
            GameWorld.Instance.CurrentPlayer.Gold += _earnedGold;
        
            ResultMessage += $"\n\n{_questioner.Farewell(passedTest)}";
            ResultMessage += $"\nВи заробили {_earnedGold} золота!";
        }
    
        ShowResult();
    }

    private void NextQuestion()
    {
        ShowQuestion();
    }

    private void CompleteQuiz()
    {
        ShowGreeting();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
}