using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Interactivity;
using Avalonia.Media.Immutable;
using System;
using System.Threading.Tasks;
using System.Data;                 // for DataTable.Compute
using System.Text.RegularExpressions;
using Avalonia.Platform;

namespace HorseCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Find the button and hook its Click event
            var button = this.FindControl<Button>("AddButton");
            if (button != null)
            {
                button.Click += OnAddButtonClicked;
            }
            else
            {
                Console.WriteLine("AddButt not found!");
            }

        }
        
        private async Task RunProgressAsync(int rpa)
        {
            if (this.FindControl<ProgressBar>("WorkBar") is not ProgressBar bar)
                return;

            if (this.FindControl<Image>("HorseImage") is not Image hrse)
                return;

            //var horseDefault = new Bitmap("Assets/hrse/horse_still.png");
            //var horseUp = new Bitmap("Assets/hrse/horse_up.png");
            //var horseDown = new Bitmap("Assets/hrse/horse_down.png");

            var uri = new Uri("avares://HorseCalculator/Assets/hrse/horse_still.png");
            var uriUp = new Uri("avares://HorseCalculator/Assets/hrse/horse_up.png");
            var uriDown = new Uri("avares://HorseCalculator/Assets/hrse/horse_down.png");
            var uriSad = new Uri("avares://HorseCalculator/Assets/hrse/horse_sad.png");
            using var stream = AssetLoader.Open(uri);
            var horseDefault = new Bitmap(stream);
            using var stream2 = AssetLoader.Open(uriUp);
            var horseUp = new Bitmap(stream2);
            using var stream3 = AssetLoader.Open(uriDown);
            var horseDown = new Bitmap(stream3);
            using var stream4 = AssetLoader.Open(uriSad);
            var horseSad = new Bitmap(stream4);

            bar.Value = 0;
            if (rpa == 0)
                bar.IsVisible = true;
            if (rpa == 1)
                bar.IsVisible = false;

            
            for (int i = 0; i <= 100; i++)
            {
                bar.Value = i;

                if (rpa == 0)
                {
                    if (i % 3 == 0)
                        hrse.Source = horseUp;
                    else
                        hrse.Source = horseDown;
                }
                if (rpa == 1)
                {
                    hrse.Source = horseSad;
                }
                await Task.Delay(30);
            }

            hrse.Source = horseDefault;
            bar.IsVisible = false;
        }

        private async void OnAddButtonClicked(object? sender, RoutedEventArgs e)
        {
            

            if (this.FindControl<ProgressBar>("WorkBar") is not ProgressBar bar)
                return;

            
            if (this.FindControl<Image>("HorseImage") is not Image hrse)
                return;

            var box = this.FindControl<TextBox>("ExprBox");
            var outText = this.FindControl<TextBlock>("ResultText");
            var horseText = this.FindControl<TextBlock>("HorseText");
            if (box is null || outText is null || horseText is null) return;

            if (sender is Button button)
            {
                outText.IsVisible = false;
                button.IsEnabled = false;
                horseText.Text = "Horse Calculating...";
                await RunProgressAsync(0);
                horseText.Text = "Horse Calculator";
                outText.IsVisible = true;
                button.IsEnabled = true;
            }

            var expr = (box.Text ?? "").Trim();

            // 1) Allow only digits, whitespace, + - * / . and parentheses
            //    (prevents weird inputs; extend as needed)
            if (!Regex.IsMatch(expr, @"^[0-9\.\s\+\-\*\/\(\)]+$"))
            {
                //horseText.IsVisible = false;
                horseText.Text = " ";
                outText.IsVisible = false;
                box.IsVisible = false;
                if (sender is Button button2)
                {
                    button2.IsEnabled = false;
                    button2.IsVisible = false;
                }

                await RunProgressAsync(1);

                if (expr == "Agnes Tachyon")
                {
                    horseText.IsVisible = true;
                    horseText.Text = "Passphrase Accepted. Contacting HORSE";

                    int totalDuration = 735000;
                    int stepDelay = 500;
                    int elapsed = 0;

                    string baseText = "Passphrase Accepted. Contacting HORSE";
                    string[] dots = { ".", "..", "..." };
                    int h = 0;

                    while (elapsed < totalDuration)
                    {
                        horseText.Text = $"{baseText}{dots[h]}";
                        h = (h + 1) % dots.Length;
                        await Task.Delay(stepDelay);
                        elapsed += stepDelay;
                    }

                    outText.IsVisible = true;
                    horseText.Text = "HORSE Connection Established";
                    int year = DateTime.Now.Year;
                    string yearSuffix = (year % 100).ToString("00");
                    string zyrusCode = $"4830878064408710241543903220435531024156111215{yearSuffix}";
                    int i = 0;
                    while (true)
                    {
                        if (i >= zyrusCode.Length)
                            i = 0;

                        outText.Text = zyrusCode[i].ToString();

                        i++;

                        await Task.Delay(1000);
                    }
                }
                else
                {
                    //horseText.IsVisible = true;
                    //outText.IsVisible = true;
                    horseText.Text = "horse calculator";
                    box.IsVisible = true;
                    if (sender is Button button3)
                    {
                        button3.IsEnabled = true;
                        button3.IsVisible = true;
                    }
                }
            }

            try
            {
                // 2) Evaluate (DataTable.Compute handles + - * / and parentheses)
                var dt = new DataTable();
                // Optional: make Compute safer by turning off expression columns features (we're not adding any)
                var result = dt.Compute(expr, null);
                outText.Text = $"{expr} = {result}";
            }
            catch (Exception)
            {
                outText.Text = "what";
            }

        }
    }
}