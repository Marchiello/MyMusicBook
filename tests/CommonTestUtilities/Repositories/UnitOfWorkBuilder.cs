using Moq;
using MyMusicBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        // o .Object é a "implementação fake" em si, por isso retornando isso tudo ocorre perfeitamente.
        return mock.Object;
    }
}
