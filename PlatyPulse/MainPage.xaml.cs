using PlatyPulseAPI;
using PlatyPulseAPI.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

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
    private ViewChallenge ViewChallenge;
    private int imageClickCount = 0;
    private bool changedImaged = false;
    private int count = 0;

    public MainPage()
    {
        InitializeComponent();

        // Chargement initial
        PlatyApp.Instance.LoadExample();

        // Initialisation de ViewChallenge avant de définir le BindingContext
        ViewChallenge = new ViewChallenge(PlatyApp.Instance.DailyChallenge);

        // Démarrage de l'initialisation utilisateur de manière asynchrone
        InitializeUserAsync();
    }

    private async Task InitializeUserAsync()
    {
        try
        {
            var user = await InitializeAsync();

            // Mise à jour des propriétés de ViewChallenge
            ViewChallenge.ID = user.ID;
            ViewChallenge.Nom = user.Pseudo.ToString(); // Assurez-vous que le Pseudo n'est pas null
            ViewChallenge.XP = user.XP.Value; // Assurez-vous que XP n'est pas null

            // Mettre à jour le BindingContext sur le thread principal
            // Utiliser SynchronizationContext pour s'assurer que le code est exécuté sur le thread principal
            var synchronizationContext = SynchronizationContext.Current;
            if (synchronizationContext != null)
            {
                synchronizationContext.Post(_ =>
                {
                    // Redéfinir BindingContext pour appliquer les changements à l'interface utilisateur
                    BindingContext = ViewChallenge;
                }, null);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during user initialization: " + ex.Message);
        }
    }

    private async Task<User> InitializeAsync()
    {
        User user;
        try
        {
            // Récupérer l'utilisateur depuis le serveur
            user = await GetHomeUser();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during initialization: " + ex.Message);
            user = new User(); // Retourner un utilisateur par défaut en cas d'erreur
        }
        return user;
    }

    private async Task<User> GetHomeUser()
    {
        var user = new User
        {
            ID = Guid.Parse("ad1ab29d-7097-41ab-b67c-5a3cc9c2fdf0")
        };

        try
        {
            // Simuler un téléchargement de données utilisateur depuis un serveur
            await user.ServerDownload();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in ServerDownload: " + ex.Message);
            throw; // Relancer l'exception pour qu'elle soit gérée ailleurs
        }

        return user;
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var targetPage = image.AutomationId;

        // Navigation en fonction de la page cible
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
