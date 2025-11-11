using AutoMapper;
using MyMusicBook.Application.Services.AutoMapper;
using MyMusicBook.Application.Services.Criptography;
using MyMusicBook.Communication.Requests;
using MyMusicBook.Communication.Responses;
using MyMusicBook.Domain.Repositories;
using MyMusicBook.Domain.Repositories.User;
using MyMusicBook.Exceptions;
using MyMusicBook.Exceptions.ExceptionsBase;
using System.Threading.Tasks;

namespace MyMusicBook.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    // Por ser uma variavel privada, coloca-se o _ antes do nome
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordEncripter _passwordEncripter;

    public RegisterUserUseCase(
        IUserWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork,
        PasswordEncripter passwordEncripter,
        IMapper mapper)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    // o valor entre <> indica o que deve ser retornado
    {
        //var autoMapper = new AutoMapper.MapperConfiguration(options =>
        //{
        //    options.AddProfile(new AutoMapping());
        //}).CreateMapper();

        // Validar a request

        await Validate(request);

        // Mapear a request em uma entidade

        var user = _mapper.Map<Domain.Entities.User>(request);

        // Criptografia da senha

        user.Password = _passwordEncripter.Encrypt(request.Password);

        // Salvar no banco de dados

        await _writeOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        // o que deve ser retornado.
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
        };
    }
    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if(emailExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }


        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }


}

