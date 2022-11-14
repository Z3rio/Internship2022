using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class UserContext : DbContext
{
    public DbSet<User> Blogs { get; set; }
    public DbSet<User> Posts { get; set; }

    public string DbPath { get; }

    public UserContext()
    {
        DbPath = "Data Source=DESKTOP-V55FTTI\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;Password=Sdf33b??;TrustServerCertificate=False;MultipleActiveResultSets=True";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(DbPath);
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public List<Message> Messages { get; } = new();
}

public class Message
{
    public int UserId { get; set; }
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTime timestamp { get; set; }
}