using System;
using System.IO.Compression;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext dataContext, IMapper mapper) : IUserRepository
{
    public async Task<MemberDTO?> GetMemberByUsernameAsync(string username)
    {
        return await dataContext.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<MemberDTO>> GetMemberssAsync()
    {
        return await dataContext.Users
            .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await dataContext.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await dataContext.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(user => user.UserName == username);

    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await dataContext.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await dataContext.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        dataContext.Entry(user).State = EntityState.Modified;
    }
}
