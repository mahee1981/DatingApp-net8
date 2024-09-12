using System;
using System.IO.Compression;
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
public class UsersController(IUserRepository userRepository) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsersAsync()
    {
        var users = await userRepository.GetMemberssAsync();


        return Ok(users);
    }


    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDTO>> GetUserAsync(string username)
    {
        var user = await userRepository.GetMemberByUsernameAsync(username);

        if(user == null)
        {
            return NotFound();
        }

        return user;
    }

}
