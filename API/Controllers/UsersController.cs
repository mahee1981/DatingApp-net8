using System;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsersAsync()
    {
        var users = await userRepository.GetMembersAsync();


        return Ok(users);
    }


    [HttpGet("{username}")] // Example: api/users/ursula
    public async Task<ActionResult<MemberDTO>> GetUserAsync(string username)
    {
        var user = await userRepository.GetMemberByUsernameAsync(username);

        if(user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(username == null) return BadRequest("No username found in token!");

        var user = await userRepository.GetUserByUsernameAsync(username);

        if(user == null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDTO, user);

        if(await userRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Failed to update the user");

    }

}
