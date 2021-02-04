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
    public class UnidadeBL
    {
        private readonly IMapper _mapper;
        private readonly UnidadeRepository _unidadeRepository;

        public UnidadeBL(IMapper mapper, UnidadeRepository unidadeRepository)
        {
            _mapper = mapper;
            _unidadeRepository = unidadeRepository;
        }

        public int Insert(UnidadeRequest unidadeRequest)
        {
            VerificaSeUnidadeExistePorNome(unidadeRequest.Nome);

            var unidadeEntity = _mapper.Map<UnidadeEntity>(unidadeRequest);
            var idUnidade = _unidadeRepository.Insert(unidadeEntity);

            return idUnidade;
        }

        public int Update(UnidadeUpdateRequest unidadeUpdateRequest)
        {
            var nome = _unidadeRepository.GetNomeUnidadeById(unidadeUpdateRequest.IdUnidade);

            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new SignaRegraNegocioException("Nenhuma unidade foi encontrada");
            }

            VerificaSeUnidadeExiste(unidadeUpdateRequest.IdUnidade);

            var unidadeEntity = _mapper.Map<UnidadeEntity>(unidadeUpdateRequest);
            var linhasafetadas = _unidadeRepository.Update(unidadeEntity);

            return linhasafetadas;
        }
        public IEnumerable<UnidadeResponse> GetAllUnidades()
        {
            var unidadeEntities = _unidadeRepository.GetAllUnidades();
            var unidadeResponse = unidadeEntities.Select(x => _mapper.Map<UnidadeResponse>(x));

            return unidadeResponse;
        }

        public UnidadeResponse GetUnidadeById(int id)
        {
            var unidadeEntity = _unidadeRepository.GetUnidade(id);
            var unidadeResponse = _mapper.Map<UnidadeResponse>(unidadeEntity);

            return unidadeResponse;
        }
        public int Delete(int id)
        {
            var unidadeEntity = _unidadeRepository.GetUnidade(id);

            if (unidadeEntity != null)
            {
                var linhasAfetadas = _unidadeRepository.Delete(id);

                return linhasAfetadas;
            }
            else
            {
                throw new SignaRegraNegocioException("Erro ao excluir o aluno, contate o administrador");
            }
        }

        public void VerificaSeUnidadeExiste(int idUnidade)
        {
            var status = _unidadeRepository.GetStatusById(idUnidade);

            if (status != 1)
            {
                throw new SignaRegraNegocioException("A Unidade informada não existe");
            }
        }

        public void VerificaSeUnidadeExistePorNome(string nome)
        {
            var id = _unidadeRepository.GetUnidadeIdByNome(nome);

            if (id != 0)
            {
                throw new SignaRegraNegocioException("Já existe uma unidade cadastrada com esse nome");
            }
        }
    }
}
