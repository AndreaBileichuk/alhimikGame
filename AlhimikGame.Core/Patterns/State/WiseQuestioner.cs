using AlhimikGame.Core.Models;

namespace AlhimikGame.Core.Patterns.State;

using System;
using System.Collections.Generic;

// Інтерфейс стану
public interface IQuestionerState
{
    string GetGreeting();
    List<Question> GetQuestions();
    string GetFarewellMessage(bool answeredCorrectly);
    int GetRewardAmount(int correctAnswers);
}

public class Question
{
    public string Text { get; }
    public string[] PossibleAnswers { get; }
    public int CorrectAnswerIndex { get; }

    public Question(string text, string[] possibleAnswers, int correctAnswerIndex)
    {
        Text = text;
        PossibleAnswers = possibleAnswers;
        CorrectAnswerIndex = correctAnswerIndex;
    }
}
public abstract class QuestionerState : IQuestionerState
{
    protected List<Question> Questions { get; set; }
    protected int CurrentQuestionIndex { get; set; }
    protected int CorrectAnswers { get; set; }
    protected bool QuizCompleted { get; set; }

    public abstract string GetGreeting();
    public abstract string GetFarewellMessage(bool answeredCorrectly);
    public abstract int GetRewardAmount(int correctAnswers);

    public List<Question> GetQuestions()
    {
        return Questions;
    }

    public void ResetQuiz()
    {
        CurrentQuestionIndex = 0;
        CorrectAnswers = 0;
        QuizCompleted = false;
    }

    public Question GetCurrentQuestion()
    {
        return Questions[CurrentQuestionIndex];
    }

    public bool MoveToNextQuestion(bool wasCorrect)
    {
        if (wasCorrect) CorrectAnswers++;
    
        if (CurrentQuestionIndex >= Questions.Count - 1)
        {
            QuizCompleted = true;
            return false;
        }
    
        CurrentQuestionIndex++;
        return true; 
    }

    public int GetCurrentQuestionIndex() => CurrentQuestionIndex;
    public int GetCorrectAnswers() => CorrectAnswers;
    public bool IsQuizCompleted() => QuizCompleted;
}

public class BeginnerQuestionerState : QuestionerState
{
    public BeginnerQuestionerState()
    {
        Questions = new List<Question>
        {
            new Question(
                "Яка основна складова еліксиру здоров'я?",
                new string[] { "Червона трава", "Блакитний гриб", "Зелений корінь", "Жовта ягода" },
                0
            ),
            new Question(
                "Скільки інгредієнтів потрібно для базового зілля маги?",
                new string[] { "Один", "Два", "Три", "Чотири" },
                1
            ),
            new Question(
                "Який інструмент використовується для збору лікарських трав?",
                new string[] { "Молот", "Кирка", "Ніж", "Серп" },
                3
            )
        };
        ResetQuiz();
    }

    public override string GetGreeting()
    {
        return "Вітаю, новачку! У мене є кілька простих запитань для тебе.";
    }

    public override string GetFarewellMessage(bool answeredCorrectly)
    {
        return answeredCorrectly
            ? "Непогано для початківця! Приходь, коли досягнеш вищого рівня, і я приготую складніші питання."
            : "Не засмучуйся, новачку. Практика зробить тебе кращим. Спробуй ще раз пізніше.";
    }

    public override int GetRewardAmount(int correctAnswers)
    {
        return correctAnswers * 5;
    }
}

// Конкретний стан - середній рівень питань (для гравців рівня 3-4)
public class IntermediateQuestionerState : IQuestionerState
{
    public string GetGreeting()
    {
        return "Вітаю, учень! Бачу, ти вже маєш певні знання. Спробуй відповісти на ці питання.";
    }

    public List<Question> GetQuestions()
    {
        return new List<Question>
        {
            new Question(
                "Який інгредієнт підсилює дію еліксиру невидимості?",
                new string[] { "Рубін", "Нічний пил", "Місячна роса", "Тіньовий кристал" },
                2
            ),
            new Question(
                "Яка комбінація трав створює протиотруту?",
                new string[] { "Червона і біла", "Зелена і синя", "Жовта і фіолетова", "Чорна і золота" },
                0
            ),
            new Question(
                "Де можна знайти рідкісну траву мрійників?",
                new string[] { "У глибоких печерах", "На високих горах", "Біля водоспадів", "У старих руїнах" },
                1
            ),
            new Question(
                "Який символ використовується для активації магічного зілля?",
                new string[] { "Коло", "Трикутник", "Пентаграма", "Спіраль" },
                3
            )
        };
    }

    public string GetFarewellMessage(bool answeredCorrectly)
    {
        return answeredCorrectly
            ? "Чудово! Ти вже багато знаєш. Продовжуй вдосконалюватися, і скоро ти зможеш відповісти на мої найскладніші питання."
            : "Хмм, тобі ще є чому повчитися. Не здавайся, і повертайся, коли будеш готовий знову спробувати.";
    }

    public int GetRewardAmount(int correctAnswers)
    {
        return correctAnswers * 15; // 15 золота за кожну правильну відповідь
    }
}

// Конкретний стан - складний рівень питань (для гравців рівня 5-6)
public class AdvancedQuestionerState : IQuestionerState
{
    public string GetGreeting()
    {
        return
            "Вітаю, майстре! Готовий перевірити свої обширні знання? Ці питання перевірять твою справжню майстерність.";
    }

    public List<Question> GetQuestions()
    {
        return new List<Question>
        {
            new Question(
                "Який рідкісний інгредієнт потрібен для приготування легендарного еліксиру безсмертя?",
                new string[]
                    { "Сльоза фенікса", "Корінь древнього дерева", "Пил зі старовинної гробниці", "Кров дракона" },
                3
            ),
            new Question(
                "Яка правильна послідовність змішування для створення зілля всезнання?",
                new string[]
                {
                    "Нагрівання, змішування, охолодження", "Змішування, настоювання, фільтрація",
                    "Подрібнення, настоювання, дистиляція", "Розчинення, кристалізація, активація"
                },
                2
            ),
            new Question(
                "Який інгредієнт нейтралізує токсичні ефекти кореня темряви?",
                new string[] { "Срібний пилок", "Кристал світла", "Есенція життя", "Морська сіль" },
                1
            ),
            new Question(
                "Як правильно зберігати зілля перетворення, щоб воно не втратило силу?",
                new string[]
                {
                    "У скляній колбі під сонячним світлом", "У керамічному горщику в прохолодному місці",
                    "У металевій посудині поблизу вогню", "У кришталевому флаконі під місячним світлом"
                },
                3
            ),
            new Question(
                "Яка рідкісна рослина росте лише раз на 100 років і може зцілити будь-яку хворобу?",
                new string[] { "Золота орхідея", "Кривавий лотос", "Зоряна квітка", "Місячний корінь" },
                2
            )
        };
    }

    public string GetFarewellMessage(bool answeredCorrectly)
    {
        return answeredCorrectly
            ? "Вражаюче! Твої знання справді виняткові. Ти довів, що гідний називатися справжнім майстром."
            : "Навіть майстри іноді помиляються. Повертайся, коли будеш готовий знову випробувати свої знання.";
    }

    public int GetRewardAmount(int correctAnswers)
    {
        return correctAnswers * 30; // 30 золота за кожну правильну відповідь
    }
}

// Контекст - NPC, який задає питання
public class WiseQuestioner
{
    private QuestionerState _currentState;
    private readonly string _name;

    public WiseQuestioner(string name)
    {
        _name = name;
        _currentState = new BeginnerQuestionerState();
    }

    public void UpdateState(int playerLevel)
    {
        if (playerLevel >= 5)
        {
            if (!(_currentState is AdvancedQuestionerState)) ;
            //_currentState = new AdvancedQuestionerState();
        }
        else if (playerLevel >= 3)
        {
            if (!(_currentState is IntermediateQuestionerState)) ;
            //_currentState = new IntermediateQuestionerState();
        }
        else
        {
            if (!(_currentState is BeginnerQuestionerState))
                _currentState = new BeginnerQuestionerState();
        }
    }

    public string GetName() => _name;
    public string Greet() => _currentState.GetGreeting();
    public List<Question> GetQuestions() => _currentState.GetQuestions();
    public string Farewell(bool answeredCorrectly) => _currentState.GetFarewellMessage(answeredCorrectly);
    public int CalculateReward(int correctAnswers) => _currentState.GetRewardAmount(correctAnswers);
    
    public void ResetQuiz() => _currentState.ResetQuiz();
    public Question GetCurrentQuestion() => _currentState.GetCurrentQuestion();
    public bool MoveToNextQuestion(bool wasCorrect) => _currentState.MoveToNextQuestion(wasCorrect);
    public int GetCurrentQuestionIndex() => _currentState.GetCurrentQuestionIndex();
    public int GetCorrectAnswers() => _currentState.GetCorrectAnswers();
    public bool IsQuizCompleted() => _currentState.IsQuizCompleted();
}