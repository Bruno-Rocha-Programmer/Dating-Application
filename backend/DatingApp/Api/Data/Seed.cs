using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Api.DTOs;
using Api.Entities;
using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class Seed
{
    public static async Task SeedUsers(DbAppContext context)
    {
        if (await context.Users.AnyAsync()) return; // se já tiver data, é para retornar

        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

        if (members == null)
        {
            Console.WriteLine("No members in seed data!");
            return;
        }

        using var hmac = new HMACSHA512();

        foreach (var member in members)
        {
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email,
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Zyhy@5000")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    Description = member.Description,
                    DateOfBirth = member.DateOfBirth,
                    City = member.City,
                    Country = member.Country,
                    Created = member.Created,
                    ImageUrl = member.ImageUrl,
                    LastActive = member.LastActive,
                    Gender = member.Gender
                }
            };

            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id
            });

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
