using Microsoft.EntityFrameworkCore;
using MyMusicBook.Domain.Entities;

namespace MyMusicBook.Infraestructure.DataAccess;

public class MyMusicBookDbContext : DbContext
{
    public MyMusicBookDbContext(DbContextOptions options) : base(options){}

    public DbSet<User> Users { get; set; } // Configuração

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyMusicBookDbContext).Assembly);
        // Definindo que o EntityFramework deve utilizar as configurações que estão no projeto.
    }
}
