using AutoMapper;
using Escola.API.Data.Entities;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;

namespace Escola.API.Profiles
{
    public class UnidadeProfile : Profile
    {
        public UnidadeProfile()
        {
            CreateMap<UnidadeEntity, UnidadeResponse>().ReverseMap();

            CreateMap<UnidadeRequest, UnidadeEntity>().ReverseMap();

            CreateMap<UnidadeUpdateRequest, UnidadeEntity>().ReverseMap();
        }
    }
}
