using AutoMapper;
using Escola.API.Data.Entities;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;

namespace Escola.API.Profiles
{
    public class ProfessorProfile : Profile
    {
        public ProfessorProfile()
        {
            CreateMap<ProfessorRequest, ProfessorEntity>().ReverseMap();
            CreateMap<ProfessorEntity, ProfessorResponse>().ReverseMap();
            CreateMap<ProfessorUpdateRequest, ProfessorEntity>().ReverseMap();
        }
    }
}
