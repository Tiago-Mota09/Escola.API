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
    public class AlunoBL
    {
        private readonly IMapper _mapper;
        private readonly AlunoRepository _alunoRepository;

        public AlunoBL(AlunoRepository alunoRepository, IMapper mapper)
        {
            _mapper = mapper;
            _alunoRepository = alunoRepository;
        }

        public int Insert(AlunoRequest alunoRequest) 
        {
            VerificaSeAlunoJaExiste(alunoRequest.Nome);
            VerificaSeUnidadeExiste(alunoRequest.IdUnidade);

            var alunoEntity = _mapper.Map<AlunoEntity>(alunoRequest);
            var idAluno = _alunoRepository.Insert(alunoEntity);

            return idAluno;
        }

        public int Update(AlunoUpdateRequest alunoUpdateRequest)
        {
            var nome = _alunoRepository.GetNomeAlunoById(alunoUpdateRequest.IdAluno);

            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new SignaRegraNegocioException("Nenhum aluno foi encontrado");
            }

            VerificaSeUnidadeExiste(alunoUpdateRequest.IdUnidade);

            var alunoEntity = _mapper.Map<AlunoEntity>(alunoUpdateRequest);
            var linhasafetadas = _alunoRepository.Update(alunoEntity);

            return linhasafetadas;
        }
        public IEnumerable<AlunoResponse> GetAllAlunos()
        {
            var alunoEntities = _alunoRepository.GetAllAlunos();
            var alunoResponse = alunoEntities.Select(x => _mapper.Map<AlunoResponse>(x));

            return alunoResponse;
        }

        public AlunoResponse GetAlunoById(int id)
        {
            var alunoEntity = _alunoRepository.GetAluno(id);
            var alunoResponse = _mapper.Map<AlunoResponse>(alunoEntity);

            return alunoResponse;
        }
        public int Delete(int id)
        {
            var alunoEntity = _alunoRepository.GetAluno(id);

            if(alunoEntity != null)
            {
                var linhasAfetadas = _alunoRepository.Delete(id);

                return linhasAfetadas;
            }
            else
            {
                throw new SignaRegraNegocioException("Erro ao excluir o aluno, contate o administrador");
            }
        }

        private void VerificaSeUnidadeExiste(int idUnidade)
        {
            var status = _alunoRepository.GetStatusUnidadeById(idUnidade);

            if (status != 1)
            {
                throw new SignaRegraNegocioException("A Unidade informada não existe");
            }
        }
        private void VerificaSeAlunoJaExiste(string nome)
        {
            var id = _alunoRepository.GetIdByNome(nome);

            if (id != 0)
            {
                throw new SignaRegraNegocioException("Já existe um usuário cadastrado com esse nome");
            }
        }
    }
}
