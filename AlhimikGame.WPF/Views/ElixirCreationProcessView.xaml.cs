using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using AlhimikGame.WPF.ViewModels;

namespace AlhimikGame.WPF.Views
{
    public partial class ElixirCreationProcessView : UserControl
    {
        private Storyboard mixingAnimation;
        private DispatcherTimer countdownTimer;
        private int secondsRemaining;
        private const int MIXING_DURATION = 5; 
        
        public ElixirCreationProcessView()
        {
            InitializeComponent();
    
            CauldronContent.Fill = new SolidColorBrush(Colors.LightGreen);
    
            mixingAnimation = (Storyboard)FindResource("MixingAnimation");
            
            foreach (DoubleAnimation animation in mixingAnimation.Children)
            {
                animation.Duration = new Duration(TimeSpan.FromSeconds(MIXING_DURATION));
                animation.RepeatBehavior = new RepeatBehavior(1);
            }
    
            mixingAnimation.Completed += MixingAnimation_Completed;
            
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
        }
        private void StartMixing_Click(object sender, RoutedEventArgs e)
        {
            StartMixingBtn.IsEnabled = false;
    
            var viewModel = DataContext as ElixirCreationProcess;
            if (viewModel != null)
            {
                secondsRemaining = MIXING_DURATION;
                viewModel.SetMessage($"Змішування інгредієнтів... ({secondsRemaining} секунд залишилося)");
                
                countdownTimer.Start();
        
                SolidColorBrush mixingBrush = new SolidColorBrush(Colors.LightGreen);
                CauldronContent.Fill = mixingBrush;
        
                ColorAnimation mixingColorAnimation = new ColorAnimation();
                mixingColorAnimation.From = Colors.LightGreen;
                mixingColorAnimation.To = Colors.Purple;
                mixingColorAnimation.Duration = new Duration(TimeSpan.FromSeconds(MIXING_DURATION));
        
                mixingBrush.BeginAnimation(SolidColorBrush.ColorProperty, mixingColorAnimation);
        
                // Explicitly set the duration and begin the mixing animation
                foreach (DoubleAnimation animation in mixingAnimation.Children)
                {
                    animation.Duration = new Duration(TimeSpan.FromSeconds(MIXING_DURATION));
                }
                mixingAnimation.Begin(this, HandoffBehavior.SnapshotAndReplace);
            }
        }
        
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            secondsRemaining--;
            
            var viewModel = DataContext as ElixirCreationProcess;
            if (viewModel != null)
            {
                if (secondsRemaining > 0)
                {
                    viewModel.SetMessage($"Змішування інгредієнтів... ({secondsRemaining} секунд залишилося)");
                }
                else
                {
                    viewModel.SetMessage("Завершення створення еліксиру...");
                    countdownTimer.Stop();
                }
            }
        }

        private void MixingAnimation_Completed(object sender, EventArgs e)
        {
            countdownTimer.Stop();
            
            var viewModel = DataContext as ElixirCreationProcess;
            if (viewModel != null)
            {
                viewModel.FinishCreation();
                viewModel.SetMessage("Еліксир створено успішно!");
                StartMixingBtn.IsEnabled = true;
            }
        }

        private void Ingredient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var image = sender as Image;
                if (image != null && image.DataContext is IngredientViewModel ingredient)
                {
                    if (ingredient.RemainingNeeded > 0)
                    {
                        DataObject dragData = new DataObject();
                        dragData.SetData("IngredientViewModel", ingredient);
                        
                        DragDrop.DoDragDrop(image, dragData, DragDropEffects.Copy);
                    }
                    else
                    {
                        var viewModel = DataContext as ElixirCreationProcess;
                        if (viewModel != null)
                        {
                            viewModel.SetMessage($"Вам більше не потрібно {ingredient.Name}!");
                        }
                    }
                }
            }
        }

        private void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("IngredientViewModel"))
            {
                e.Effects = DragDropEffects.Copy;
                
                DropHighlight.Visibility = Visibility.Visible;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            
            e.Handled = true;
        }

        private void DropTarget_DragLeave(object sender, DragEventArgs e)
        {
            DropHighlight.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }

        private void DropTarget_Drop(object sender, DragEventArgs e)
        {
            DropHighlight.Visibility = Visibility.Collapsed;
            
            if (e.Data.GetDataPresent("IngredientViewModel"))
            {
                var ingredient = e.Data.GetData("IngredientViewModel") as IngredientViewModel;
                if (ingredient != null)
                {
                    var viewModel = DataContext as ElixirCreationProcess;
                    if (viewModel != null)
                    {
                        viewModel.AddIngredient(ingredient);
                        
                        ShowIngredientAddedEffect();
                    }
                }
            }
            
            e.Handled = true;
        }

        private void ShowIngredientAddedEffect()
        {
            SolidColorBrush animatableBrush = new SolidColorBrush(Colors.LightGreen);
    
            CauldronContent.Fill = animatableBrush;
    
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = Colors.Yellow;
            colorAnimation.To = Colors.LightGreen;
            colorAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
    
            animatableBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }
    }
}