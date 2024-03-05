using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tello_Demo.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tello_Demo.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CardList> CardLists => Set<CardList>();
    public DbSet<Card> Cards => Set<Card>();
}
