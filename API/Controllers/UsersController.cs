using System;
using System.IO.Compression;
using API.Data;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UsersController(DataContext context) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsersAsync()
    {
        var users = await context.Users.ToListAsync();

        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> GetUserAsync(int id)
    {
        var user = await context.Users.FindAsync(id);

        if(user == null)
        {
            return NotFound();
        }

        return user;
    }

}
