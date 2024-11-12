using PlatyPulseAPI;
using PlatyPulseAPI.Data;
using System.Collections.ObjectModel;
namespace PlatyPulse;



public class ViewChallenge : PlatyAppComponent
{
    public Challenge Challenge;
    public ObservableCollection<Quest> Quests { get; set; }

    public ViewChallenge(Challenge challenge) 
    { 
        Challenge = challenge;
        Quests = new ObservableCollection<Quest>(challenge.Quests);
    }
}

public partial class MainPage : ContentPage
{
    ViewChallenge ViewChallenge;
    private int imageClickCount = 0;
    private bool changedImaged = false;
    int count = 0;

    public MainPage()
    {
        InitializeComponent();

        PlatyApp.Instance.LoadExample();
        ViewChallenge = new ViewChallenge(PlatyApp.Instance.DailyChallenge);
        BindingContext = ViewChallenge;
    }

    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //    count++;

    //    if (count == 1)
    //        CounterBtn.Text = $"Clicked {count} time";
    //    else
    //        CounterBtn.Text = $"Clicked {count} times";

        //SemanticScreenReader.Announce(CounterBtn.Text);
    }



    private async void OnImageTapped(object sender, EventArgs e)
    {
        imageClickCount++;
        await ClickableImage.ScaleTo(1.2, 100); // 100 ms pour l'agrandissement
                                                
        await ClickableImage.ScaleTo(1.0, 100); // 100 ms pour le rétrécissement

        if (imageClickCount >= 12 && !changedImaged)
        {
            ClickableImage.Source = "platypulse_hiden.png"; 
            imageClickCount = 0;
            changedImaged = true;
        }
    }

}
