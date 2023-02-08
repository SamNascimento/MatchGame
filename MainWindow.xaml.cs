using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DispatcherTimer timer = new DispatcherTimer();
    private int tenthsOfSecondsElapsed;
    private int lastTenthsOfSecondsElapsed;
    private int matchesFound;

    private TextBlock lastTextBlockClicked = new();
    private bool findingMatch = false;

    public MainWindow()
    {
        InitializeComponent();

        timer.Interval = TimeSpan.FromSeconds(.1);
        timer.Tick += Timer_Tick;

        SetUpGame();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        tenthsOfSecondsElapsed++;
        timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
        if (matchesFound == 8)
        {
            timer.Stop();

            lastTimeTextBlock.Text = timeTextBlock.Text;
            timeTextBlock.Text = "Play again?";
        }
    }

    private void SetUpGame()
    {
        var animalEmoji = new List<string>()
        {
            "🐙", "🐙",
            "🐶", "🐶",
            "🐘", "🐘",
            "🐵", "🐵",
            "🐮", "🐮",
            "🦁", "🦁",
            "🦄", "🦄",
            "🐻", "🐻"
        };

        var random = new Random();

        foreach (var textBlock in mainGrid.Children.OfType<TextBlock>())
        {
            if (textBlock.Name != "timeTextBlock" && textBlock.Name != "lastTimeTextBlock" && textBlock.Name != "lastTime")
            {
                var index            = random.Next(animalEmoji.Count);
                var nextEmoji        = animalEmoji[index];
                textBlock.Text       = nextEmoji;
                textBlock.Visibility = Visibility.Visible;

                animalEmoji.RemoveAt(index);
            }
        }
    }

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (!timer.IsEnabled)
        {
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        var textBlock = (TextBlock)sender;
        if (findingMatch == false)
        {
            textBlock.Visibility = Visibility.Hidden;
            lastTextBlockClicked = textBlock;
            findingMatch = true;
        }
        else if (textBlock.Text == lastTextBlockClicked.Text)
        {
            matchesFound++;
            textBlock.Visibility = Visibility.Hidden;
            findingMatch = false;
        }
        else
        {
            lastTextBlockClicked.Visibility = Visibility.Visible;
            findingMatch = false;
        }
    }

    private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (matchesFound == 8)
        {
            SetUpGame();
            timeTextBlock.Text = "0,0s";
        }
    }
}
