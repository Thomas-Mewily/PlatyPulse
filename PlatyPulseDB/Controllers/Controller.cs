using Microsoft.AspNetCore.Mvc;
using PlatyPulseAPI.Data;

namespace PlatyPulseWebAPI.Controllers;

// Controller pour Quest
[Route("api/[controller]")]
[ApiController]
public class QuestsController : GenericController<Quest>
{
    public QuestsController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}

// Controller pour Challenge
[Route("api/[controller]")]
[ApiController]
public class ChallengesController : GenericController<Challenge>
{
    public ChallengesController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}

// Controller pour User
[Route("api/[controller]")]
[ApiController]
public class UsersController : GenericController<User>
{
    public UsersController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}

// Controller pour ChallengeEntry
[Route("api/[controller]")]
[ApiController]
public class ChallengeEntriesController : GenericController<ChallengeEntry>
{
    public ChallengeEntriesController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}

// Controller pour QuestEntry
[Route("api/[controller]")]
[ApiController]
public class QuestEntriesController : GenericController<QuestEntry>
{
    public QuestEntriesController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}

/*
// Si Owned ou IdentifiableOwnedByData nécessitent des contrôleurs spécifiques
[Route("api/[controller]")]
[ApiController]
public class OwnedsController : GenericController<Owned<IdentifiableOwnedByData>>
{
    public OwnedsController(DataBaseCtx db, IConfiguration config) : base(db, config) { }
}
*/
