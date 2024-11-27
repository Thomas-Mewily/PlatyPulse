using Microsoft.AspNetCore.Mvc;
using PlatyPulseAPI.Data;

namespace PlatyPulseWebAPI.Controllers;

// Controller pour Quest
[Route("api/[controller]")]
[ApiController]
public class QuestsController(DataBaseCtx db, IConfiguration config) : GenericController<Quest>(db, config)
{
}

// Controller pour Challenge
[Route("api/[controller]")]
[ApiController]
public class ChallengesController(DataBaseCtx db, IConfiguration config) : GenericController<Challenge>(db, config)
{
}

// Controller pour User
[Route("api/[controller]")]
[ApiController]
public class UsersController(DataBaseCtx db, IConfiguration config) : GenericController<User>(db, config)
{
}

// Controller pour ChallengeEntry
[Route("api/[controller]")]
[ApiController]
public class ChallengeEntriesController(DataBaseCtx db, IConfiguration config) : GenericController<ChallengeEntry>(db, config)
{
}

// Controller pour QuestEntry
[Route("api/[controller]")]
[ApiController]
public class QuestEntriesController(DataBaseCtx db, IConfiguration config) : GenericController<QuestEntry>(db, config)
{
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
