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

    private async void OnImageTapped(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var targetPage = image.AutomationId;

        switch (targetPage)
        {
            case "Profile":
                await Navigation.PushAsync(new ProfilePage());
                break;
            case "Friends":
                await Navigation.PushAsync(new FriendPage());
                break;
            case "Challenges":
                await Navigation.PushAsync(new ChallengeHisto());
                break;
            case "Rank":
                await Navigation.PushAsync(new RankPage());
                break;
            case "Stats":
                await Navigation.PushAsync(new StatPage());
                break;
            case "HomePage":
                break;
            default:
                break;
        }
    }

}
