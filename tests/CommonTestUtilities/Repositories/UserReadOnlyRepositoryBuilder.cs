using Moq;
using MyMusicBook.Domain.Repositories.User;
using System.Data.SqlTypes;

namespace CommonTestUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{

    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();

    public void ExistActiveUserWithEmail(string email)
    {
        //_repository.Setup(repository => repository.ExistActiveUserWithEmail(It.IsAny<string>());
        _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }
    public IUserReadOnlyRepository Build() => _repository.Object;

}
