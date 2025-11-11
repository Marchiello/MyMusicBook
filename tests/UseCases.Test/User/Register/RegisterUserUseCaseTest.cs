using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyMusicBook.Application.UseCases.User.Register;
using MyMusicBook.Exceptions;
using MyMusicBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(); 

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        // ------ Criando uma variável que armazena uma função

        Func<Task> act = async () => await useCase.Execute(request);

        // ---------------------------------------------------

        /* 
         Coloca-se () numa expressão para, como na matemática, realizá-la primeiro. Assim
         trabalhamos com o resultado dessa expressão, que é o que fazemos com o "." abaixo
         da expressão. 
       */
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        // o where diz que esperamos algo além do que foi proposto na expressão. Nele
        // adicionamos e, portanto, esperamos mais uma condição.

    }

    // A variavel que armazena uma função teria uma sitaxe "bruta" assim: --

    //public async Task Act()
    //{
    //    await UseCases.Execute(request)
    //}

    // ----------------------------------------------------------------------

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
    }

    // Colocando o tipo? nomeParam = null, faço com que esse parametro seja opcional.
    private RegisterUserUseCase CreateUseCase(string? email = null)
    {

        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var writeRepostory = UserWriteOnlyRepositoryBuilder.Build();
        var unityOfWork = UnitOfWorkBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if(string.IsNullOrEmpty(email) == false)
            readRepositoryBuilder.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(writeRepostory, readRepositoryBuilder.Build(), unityOfWork, passwordEncripter, mapper);

    }

}
