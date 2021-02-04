using AutoMapper;
using Escola.API.Data.Entities;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;

namespace Escola.API.Profiles
{
    public class ProfessorAlunoProfile : Profile
    {
        public ProfessorAlunoProfile()
        {
            CreateMap<ProfessorAlunoRequest, ProfessorAlunoEntity>().ReverseMap();
            CreateMap<ProfessorAlunoEntity, ProfessorAlunoResponse>().ReverseMap();
            CreateMap<ProfessorAlunoUpdateRequest, ProfessorAlunoEntity>().ReverseMap();
        }
    }
}
