using Microsoft.AspNetCore.Mvc;
using PlatyPulseAPI.Data;

namespace PlatyPulseWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestsController : GenericController<Quest>
{
    public QuestsController(DataBaseCtx dbContext) : base(dbContext) { }
}

[Route("api/[controller]")]
[ApiController]
public class ChallengesController : GenericController<Challenge>
{
    public ChallengesController(DataBaseCtx dbContext) : base(dbContext) { }
}
