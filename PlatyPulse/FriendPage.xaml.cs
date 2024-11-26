namespace PlatyPulse;

public partial class FriendPage : ContentPage
{
	public FriendPage()
	{
		InitializeComponent();
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
            case "HomePage":
                await Navigation.PushAsync(new MainPage());
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
            case "Friends":
                break;
            default:
                break;
        }
    }
}
