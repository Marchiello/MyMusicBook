using MyMusicBook.Communication.Requests;
using MyMusicBook.Communication.Responses;

namespace MyMusicBook.Application.UseCases.User.Register;
public interface IRegisterUserUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);

}

