namespace MyMusicBook.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit();
}
