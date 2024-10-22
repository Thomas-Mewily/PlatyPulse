using PlatyPulseAPI;
using System.Collections.ObjectModel;
namespace PlatyPulse;

public class ViewChallenge : PlatyAppComponent
{
    public Challenge Challenge;
    public ObservableCollection<Goal> Goals { get; set; }

    public ViewChallenge(Challenge challenge) 
    { 
        Challenge = challenge;
        Goals = new ObservableCollection<Goal>(challenge.Goals);
    }
}

public partial class MainPage : ContentPage
{
    ViewChallenge ViewChallenge;
    int count = 0;

    public MainPage()
    {
        InitializeComponent();

        PlatyApp.Instance.LoadExample();
        ViewChallenge = new ViewChallenge(PlatyApp.Instance.DailyChallenge);
        BindingContext = ViewChallenge;
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        //SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
