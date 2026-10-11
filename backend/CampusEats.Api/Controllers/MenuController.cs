using Microsoft.AspNetCore.Authorization;
using CampusEats.Api.Dtos;
using CampusEats.Api.Services;
using Microsoft.AspNetCore.Mvc;


namespace CampusEats.Api.Controllers;

[ApiController]                         // auto 400 on invalid body
[Route("api/[controller]")]             // -> /api/menu
public class MenuController : ControllerBase
{
    private readonly IMenuService _svc;
    public MenuController(IMenuService svc)
        => _svc = svc;                  // injected by DI

    [HttpGet]                           // GET /api/menu
    public ActionResult<IEnumerable<MenuItemDto>> GetAll()
        => Ok(_svc.GetAll());

    [HttpGet("{id}")]                   // GET /api/menu/1
    public ActionResult<MenuItemDto> GetById(int id)
    {
        var item = _svc.GetById(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost] 
    [Authorize(Roles = "Admin")]                         // POST /api/menu
    public ActionResult<MenuItemDto> Create(
        [FromBody] CreateMenuItemDto dto)
    {
        var created = _svc.Create(dto);
        return CreatedAtAction(nameof(GetById), 
            new { id = created.Id }, created);  // 201
    }



    [HttpPut("{id}")]      
    [Authorize(Roles = "Admin")]                     // PUT /api/menu/1
    public IActionResult Update(int id, 
        [FromBody] CreateMenuItemDto dto)
    {
        var ok = _svc.Update(id, dto);
        return ok ? NoContent() : NotFound();   // 204/404
    }

    [HttpDelete("{id}")]   
    [Authorize(Roles = "Admin")]                     // DELETE /api/menu/1
    public IActionResult Delete(int id)
    {
        var ok = _svc.Delete(id);
        return ok ? NoContent() : NotFound();   // 204/404
    }

    
}