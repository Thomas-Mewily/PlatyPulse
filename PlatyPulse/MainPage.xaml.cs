using PlatyPulseAPI;
using PlatyPulseAPI.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace PlatyPulse;



public class ViewChallenge : PlatyAppComponent
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string nom;
    public string Nom
    {
        get => nom;
        set
        {
            nom = value;
            OnPropertyChanged(nameof(Nom));
        }
    }

    private Guid id;
    public Guid ID
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged(nameof(ID));
        }
    }

    private int xp;
    public int XP
    {
        get => xp;
        set
        {
            xp = value;
            OnPropertyChanged(nameof(XP));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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
        Thread.Sleep(2000);
        InitializeComponent();

        PlatyApp.Instance.LoadExample();
        ViewChallenge = new ViewChallenge(PlatyApp.Instance.DailyChallenge);

        InitializeAsync();
        BindingContext = ViewChallenge;
    }

    private async void InitializeAsync()
    {
        try
        {
            var user = await GetHomeUser();
            ViewChallenge.ID = user.ID;
            ViewChallenge.Nom = user.Pseudo.ToString();
            ViewChallenge.XP = user.XP.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during initialization: " + ex.Message);
        }
    }

    private async Task<User> GetHomeUser()
    {
        var user = new User
        {
            ID = Guid.Parse("ad1ab29d-7097-41ab-b67c-5a3cc9c2fdf0")
        };

        try
        {
            await user.ServerDownload();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in ServerDownload: " + ex.Message);
            throw;
        }

        return user;
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
