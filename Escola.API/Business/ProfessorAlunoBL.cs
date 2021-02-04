using AutoMapper;
using Escola.API.Data.Entities;
using Escola.API.Data.Repositories;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;
using Signa.Library.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Escola.API.Business
{
    public class ProfessorAlunoBL
    {
        private readonly IMapper _mapper;
        private readonly ProfessorAlunoRepository _professorAlunoRepository;

        public ProfessorAlunoBL(IMapper mapper, ProfessorAlunoRepository professorAlunoRepository)
        {
            _mapper = mapper;
            _professorAlunoRepository = professorAlunoRepository;
        }

        public int Insert(ProfessorAlunoRequest professorAlunoRequest)
        {
            var professorAlunoEntity = _mapper.Map<ProfessorAlunoEntity>(professorAlunoRequest);
            var idProfessorAluno = _professorAlunoRepository.Insert(professorAlunoEntity);

            return idProfessorAluno;
        }

        public int Update(ProfessorAlunoUpdateRequest professorAlunoUpdateRequest)
        {
            var professorAlunoEntity = _mapper.Map<ProfessorAlunoEntity>(professorAlunoUpdateRequest);
            var idProfessorAluno = _professorAlunoRepository.Update(professorAlunoEntity);

            if (idProfessorAluno == 0)
            {
                throw new SignaRegraNegocioException("Nenhuma referência entre professor e aluno foi encontrada");
            }

            return idProfessorAluno;
        }
        public ProfessorAlunoResponse GetProfessorAlunoById(int id)
        {
            var professorAlunoEntity = _professorAlunoRepository.GetProfessorAlunoById(id);
            var professorAlunoResponse = _mapper.Map<ProfessorAlunoResponse>(professorAlunoEntity);

            return professorAlunoResponse;
        }
        public IEnumerable<ProfessorAlunoResponse> GetAllProfessorAlunoById(int id)
        {
            var professorAlunoEntities = _professorAlunoRepository.GetAllProfessorAlunoById(id);
            var professorAlunoResponse = professorAlunoEntities.Select(x => _mapper.Map<ProfessorAlunoResponse>(x));

            return professorAlunoResponse;
        }

        public int Delete(int id)
        {
            var status = _professorAlunoRepository.GetStatusProfessorAlunoById(id);

            if(status != 1)
            {
                throw new SignaRegraNegocioException("Nenhum relação entre professor e aluno foi encontrada.");
            }
            else
            {
                var linhasAfetadas = _professorAlunoRepository.Delete(id);

                return linhasAfetadas;
            }
        }

        //private void VerificaSeUnidadeExiste(int idUnidade)
        //{
        //    var status = _professorAlunoRepository.GetProfessorAlunoById();

        //    if (status != 1)
        //    {
        //        throw new SignaRegraNegocioException("A Unidade informada não existe");
        //    }
        //}
        //private void VerificaSeProfessorAlunoJaExiste(string nome)
        //{
        //    var id = _professorAlunoRepository.GetProfessorAlunoById(nome);

        //    if (id != 0)
        //    {
        //        throw new SignaRegraNegocioException("Já existe um professor cadastrado com esse nome");
        //    }
        //}

        private void VerificaSeProfessorAlunoJaExistePorId(int id)
        {
            var nome = _professorAlunoRepository.GetProfessorAlunoById(id);

            if (nome != null)
            {
                throw new SignaRegraNegocioException("Já existe um professor cadastrado com esse nome");
            }
        }
    }
}
    

