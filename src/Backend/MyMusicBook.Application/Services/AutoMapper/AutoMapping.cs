using AutoMapper;
using MyMusicBook.Communication.Requests;

namespace MyMusicBook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, option => option.Ignore());
            //.ForMember(dest => dest.Password, option => option.MapFrom(source => source.Password)); // Caso o nome fosse diferente
    }


}