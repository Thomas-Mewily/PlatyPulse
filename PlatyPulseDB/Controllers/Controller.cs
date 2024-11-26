using Microsoft.AspNetCore.Mvc;
using PlatyPulseAPI.Data;

namespace PlatyPulseWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestsController : GenericController<Quest>
{
    public QuestsController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}

[Route("api/[controller]")]
[ApiController]
public class ChallengesController : GenericController<Challenge>
{
    public ChallengesController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}
