using AutoMapper;
using Escola.API.Data.Entities;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;

namespace Escola.API.Profiles
{
    public class AlunoProfile : Profile
    {
        public AlunoProfile()
        {
           CreateMap<AlunoRequest, AlunoEntity>().ReverseMap();
           CreateMap<AlunoEntity, AlunoResponse>().ReverseMap();
           CreateMap<AlunoUpdateRequest, AlunoEntity>().ReverseMap();
        }
    }
}
