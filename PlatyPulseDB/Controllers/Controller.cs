using Microsoft.AspNetCore.Mvc;
using PlatyPulseAPI.Data;

namespace PlatyPulseWebAPI.Controllers;

// They need to have the same name than the classe. See the ServerUpload() method

// Controller pour Quest
[Route("api/[controller]")]
[ApiController]
public class QuestController(DataBaseCtx db, IConfiguration config) : GenericController<Quest>(db, config)
{
}

// Controller pour Challenge
[Route("api/[controller]")]
[ApiController]
public class ChallengeController(DataBaseCtx db, IConfiguration config) : GenericController<Challenge>(db, config)
{
}

// Controller pour User
[Route("api/[controller]")]
[ApiController]
public class UserController(DataBaseCtx db, IConfiguration config) : GenericController<User>(db, config)
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