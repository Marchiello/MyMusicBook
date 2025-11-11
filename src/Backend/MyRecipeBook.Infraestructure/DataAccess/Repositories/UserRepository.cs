using Microsoft.EntityFrameworkCore;
using MyMusicBook.Domain.Entities;
using MyMusicBook.Domain.Repositories.User;

namespace MyMusicBook.Infraestructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MyMusicBookDbContext _dbContext;

    public UserRepository(MyMusicBookDbContext dbContext) => _dbContext = dbContext;

    // Métodos assincronos são mais performaticos para trabalhar com DB

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    //public async Task Add(User user)
    //{
    //    await _dbContext.Users.AddAsync(user);
    //    await _dbContext.SaveChangesAsync(); // Agora com essa linha, ele persiste os dados no DB
    //}

    /* Não quis usar dessa forma pois num cenário onde para cada ação(criar usuário, musica) eu use o
    .SaveChangesAsync para persistir no DB, pode ocorrer uma quebra na regra de negócio caso uma dessas
    ações falhe. Portanto, é uma melhor prática fazer todas as ações(criar usuário e uma musica) e no final
    persistir o BD com o uso do .SaveChangesAsync. Para isso, cria-se a classe 
     
     */

    // => já faz o return automático
    public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
  
}