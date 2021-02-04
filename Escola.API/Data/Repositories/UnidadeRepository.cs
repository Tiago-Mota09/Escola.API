using Dapper;
using Escola.API.Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Data.Repositories
{
    public class UnidadeRepository : RepositoryBase
    {
        public UnidadeRepository(IConfiguration configuration)
        {
            base.configuration = configuration;
        }
        public int Insert(UnidadeEntity unidade)
        {
            using var db = Connection;

            var query = @"INSERT INTO UNIDADE
                            (nome,
                             endereco)
                            values( 
                            @Nome, 
                            @Endereco)
                            RETURNING id_unidade;";

            return db.ExecuteScalar<int>(query, new
            {
                unidade.Nome,
                unidade.Endereco
            });
        }
        public int GetStatusById(int idUnidade)
        {
            using var db = Connection;

            var query = @"SELECT  status
                            FROM unidade
                          WHERE id_unidade = @idUnidade
                            AND status = 1;";

            return db.ExecuteScalar<int>(query, new { idUnidade });
        }

        public string GetNomeUnidadeById(int idUnidade)
        {
            using var db = Connection;

            var query = @"SELECT nome 
	                        FROM unidade
                        WHERE id_unidade = @idUnidade
	                        AND status = 1";

            return db.ExecuteScalar<string>(query, new { idUnidade });
        }

        public int Update(UnidadeEntity unidade)
        {
            using var db = Connection;

            var query = @"UPDATE Unidade 
                            SET nome = @Nome,
                            endereco = @Endereco
                         WHERE id_unidade = @IdUnidade AND status = 1";

            return db.Execute(query, new
            {
                unidade.Nome,
                unidade.Endereco,
                unidade.IdUnidade
            });
        }

        public UnidadeEntity GetUnidade(int idUnidade)
        {
            using var db = Connection;

            var query = @"SELECT id_unidade,
                                 nome,
		                         endereco, 
		                         status
                               FROM unidade
                           WHERE id_unidade = @idUnidade
                               AND status = 1;";

            return db.QueryFirstOrDefault<UnidadeEntity>(query, new { idUnidade });
        }
        public IEnumerable<UnidadeEntity> GetAllUnidades()
        {
            using var db = Connection;

            var query = @"SELECT id_unidade,
                             endereco, 
                             nome,
		                     status
                        FROM unidade
                            WHERE status = 1; ";

            return db.Query<UnidadeEntity>(query);
        }

        public int GetUnidadeIdByNome(string nome)
        {
            using var db = Connection;

            var query = @"SELECT id_unidade
                            FROM unidade
                          WHERE nome = @nome
                            AND status = 1; ";

            return db.ExecuteScalar<int>(query, new { nome });
        }
        public int Delete(int id)
        {
            using var db = Connection;

            var query = @"UPDATE unidade
                        SET status = 2
                      WHERE id_unidade = @id";

            return db.Execute(query, new { id });
        }
    }
}
