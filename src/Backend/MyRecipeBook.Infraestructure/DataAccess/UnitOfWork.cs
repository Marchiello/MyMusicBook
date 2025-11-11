using MyMusicBook.Domain.Repositories;

namespace MyMusicBook.Infraestructure.DataAccess;

public class UnitOfWork : IUnitOfWork
//  " : " -> Implementa  => UnitOfWork implementa IUnitOfWork
{
    private readonly MyMusicBookDbContext _dbContext;

    public UnitOfWork(MyMusicBookDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}