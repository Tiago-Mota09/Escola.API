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
    public class ProfessorBL
    {
        private readonly IMapper _mapper;
        private readonly ProfessorRepository _professorRepository;

        public ProfessorBL(IMapper mapper, ProfessorRepository professorRepository)
        {
            _mapper = mapper;
            _professorRepository = professorRepository;
        }

        public int Insert(ProfessorRequest professorRequest)
        {
            VerificaSeProfessorJaExiste(professorRequest.Nome);
            VerificaSeUnidadeExiste(professorRequest.IdUnidade);

            var professorEntity = _mapper.Map<ProfessorEntity>(professorRequest);
            var idProfessor = _professorRepository.Insert(professorEntity);

            return idProfessor;
        }

        public int Update(ProfessorUpdateRequest professorUpdateRequest)
        {
            var nome = _professorRepository.GetNomeProfessorById(professorUpdateRequest.IdProfessor);

            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new SignaRegraNegocioException("Nenhum professor foi encontrado");
            }

            VerificaSeUnidadeExiste(professorUpdateRequest.IdUnidade);

            var professorEntity = _mapper.Map<ProfessorEntity>(professorUpdateRequest);
            var linhasafetadas = _professorRepository.Update(professorEntity);

            return linhasafetadas;
        }
        public IEnumerable<ProfessorResponse> GetAllProfessores()
        {
            var professorEntities = _professorRepository.GetAllProfessores();
            var professorResponse = professorEntities.Select(x => _mapper.Map<ProfessorResponse>(x));

            return professorResponse;
        }
        public ProfessorResponse GetProfessorById(int id)
        {
            var professorEntity = _professorRepository.GetProfessor(id);
            var professorResponse = _mapper.Map<ProfessorResponse>(professorEntity);

            return professorResponse;
        }

        public int Delete(int id)
        {
            var professorEntity = _professorRepository.GetProfessor(id);

            if (professorEntity != null)
            {
                var linhasAfetadas = _professorRepository.Delete(id);

                return linhasAfetadas;
            }
            else
            {
                throw new SignaRegraNegocioException("Erro ao excluir o professor, contate o administrador");
            }
        }

        private void VerificaSeUnidadeExiste(int idUnidade)
        {
            var status = _professorRepository.GetStatusUnidadeById(idUnidade);

            if (status != 1)
            {
                throw new SignaRegraNegocioException("A Unidade informada não existe");
            }
        }
        private void VerificaSeProfessorJaExiste(string nome)
        {
            var id = _professorRepository.GetIdByNome(nome);

            if (id != 0)
            {
                throw new SignaRegraNegocioException("Já existe um professor cadastrado com esse nome");
            }
        }
    }
}
