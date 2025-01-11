using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatyPulseAPI.Data;

namespace PlatyPulseWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenericController<T> : PlatyController where T : IdentifiableByID
{
    private DataBaseCtx _dbContext => Db;
    private readonly DbSet<T> _dbSet;

    public GenericController(DataBaseCtx db, IConfiguration config) : base (db, config)
    {
        _dbSet = _dbContext.Set<T>();
    }

    /// <summary>
    /// Récupère tous les éléments de type T
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        var items = (await _dbSet.ToListAsync()).Where(s => s.IsPublicData);
        return Ok(items);
    }

    /// <summary>
    /// Récupère un élément spécifique par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<T>> GetById(ID id)
    {
        var item = await _dbSet.FindAsync(id);
        if (item == null) { return NotFound(); }
        if (item.IsPrivateData) { return Unauthorized(); }

        return Ok(item);
    }

    /*
    /// <summary>
    /// Ajoute un nouvel élément
    /// </summary>
    [Authorize]
    [HttpPost("{value}")]
    public async Task<ActionResult<T>> CreateValue([FromBody] T value)
    {
        if (value == null) { return BadRequest("Item cannot be null"); }

        _dbSet.Add(value);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = GetItemId(value) }, value);
    }
    */

    /// <summary>
    /// Ajoute un nouvel élément
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<T>> Create([FromBody] T item)
    {
        if (item == null) { return BadRequest("Item cannot be null"); }

        _dbSet.Add(item);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = GetItemId(item) }, item);
    }




    /// <summary>
    /// Supprime un élément par son ID
    /// </summary>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(ID id)
    {
        var item = await _dbSet.FindAsync(id);
        if (item == null) { return NotFound(); }

        _dbSet.Remove(item);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Met à jour un élément
    /// </summary>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(ID id, [FromBody] T item)
    {
        try 
        {
            CheckSameID(item, id);

            var will_be_updated = _dbSet.Find(item.ID).Unwrap();
            will_be_updated.UpdateFrom(item, CurrentUser);

            _dbSet.Update(will_be_updated);
            await _dbContext.SaveChangesAsync();
            return NoContent();

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Obtient l'ID d'un élément
    /// </summary>
    private ID GetItemId(T item) => item.ID;
}
