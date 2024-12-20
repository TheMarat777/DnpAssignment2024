﻿using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=/Users/maratcolesnic/Documents/GitHub/DnpAssignment3/Server/WebAPI/EfcRepositories.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId);
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p =>p.Comments)
            .HasForeignKey(c => c.PostId);
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u =>u.Comments)
            .HasForeignKey(c => c.UserId);
    }
}