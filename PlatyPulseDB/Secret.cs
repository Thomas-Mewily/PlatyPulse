using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PlatyPulseAPI;
using PlatyPulseAPI.Value;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PlatyPulseWebAPI;

public static class Secret
{
    public static string Sel() => "LeSelJaiPerduAuJeu";
    public static string TopSecretToken() => "JustForTesting&IKnowItIsOnGit4FhjKLo9ZueR1x5QWEert8&L89oP3rNu2gYrMbV5PLfU9Xc1K2DgH8RgWyEpQZuRt&";
}


