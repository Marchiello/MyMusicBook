using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusicBook.Application.UseCases.User.Register;
using MyMusicBook.Communication.Requests;
using MyMusicBook.Communication.Responses;

namespace MyMusicBook.API.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        // Duas formas de fazer a injeção de dependencia

        //1a: Por construtor
        //public UserController(IRegisterUseCase useCase) { }

        //2a: Veja no endpoint/função a seguir:
        // Injete nos parametros

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase, // Injetei aqui | FromServices = Pegar isso dos serviços de injeção de dependencia.
            [FromBody] RequestRegisterUserJson request) // FromBody = Pegar do body da request.
        {
            var result = await useCase.Execute(request);

            return Created(string.Empty , result);
        }
    }
}
