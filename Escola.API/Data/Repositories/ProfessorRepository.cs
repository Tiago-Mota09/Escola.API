using Dapper;
using Escola.API.Data.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Escola.API.Data.Repositories
{
    public class ProfessorRepository : RepositoryBase
    {
        public ProfessorRepository(IConfiguration configuration)
        {
            base.configuration = configuration;
        }
        public int Insert(ProfessorEntity professor)
        {
            using var db = Connection;

            var query = @"INSERT INTO Professor
                            (nome,
                             idade,
                             data_nascimento,
                             id_unidade)
                            values( 
                            @Nome, 
                            @Idade,
                            @DataNascimento,
                            @IdUnidade)
                            RETURNING id_professor;";

            return db.ExecuteScalar<int>(query, new
            {
                professor.Nome,
                professor.Idade,
                professor.DataNascimento,
                professor.IdUnidade
            });
        }
        public int Update(ProfessorEntity professor)
        {
            using var db = Connection;

            var query = @"UPDATE Professor
                            SET nome = @Nome,
                                idade = @Idade,
                                id_unidade = @IdUnidade,
                                data_nascimento = @DataNascimento
                          WHERE id_professor = @IdProfessor AND status = 1;";

            return db.Execute(query, new
            {
                professor.Nome,
                professor.Idade,
                professor.DataNascimento,
                professor.IdUnidade,
                professor.IdProfessor
            });
        }

        public ProfessorEntity GetProfessor(int idProfessor)
        {
            using var db = Connection;

            var query = @"SELECT id_professor, 
                             nome,
                             idade,
                             data_nascimento,
                             status, 
                             id_unidade
                             FROM professor
                          WHERE id_professor = @idProfessor
                             AND status = 1;";

            return db.QueryFirstOrDefault<ProfessorEntity>(query, new { idProfessor });
        }

        public int GetStatusUnidadeById(int idUnidade)
        {
            using var db = Connection;

            var query = @"SELECT  status
                            FROM unidade
                          WHERE id_unidade = @idUnidade
                            AND status = 1;";

            return db.ExecuteScalar<int>(query, new { idUnidade });
        }

        public string GetNomeProfessorById(int idProfessor)
        {
            using var db = Connection;

            var query = @"SELECT nome 
                            FROM Professor 
                        WHERE id_professor = @idProfessor
                            AND status = 1;";

            return db.ExecuteScalar<string>(query, new { idProfessor });
        }

        public int GetIdByNome(string nome)
        {
            using var db = Connection;

            var query = @"select id_professor 
	                        from professor
                        where nome = @Nome
	                        AND status = 1";

            return db.ExecuteScalar<int>(query, new { nome });
        }
        public IEnumerable<ProfessorEntity> GetAllProfessores()
        {
            using var db = Connection;

            var query = @"Select id_professor, 
	                         nome, 
	                         idade, 
	                         data_nascimento, 
	                         status, 
	                         id_unidade
	                     From professor 
		                      Where status = 1; ";

            return db.Query<ProfessorEntity>(query);
        }
        public int Delete(int id)
        {
            using var db = Connection;

            var query = @"UPDATE Professor 
                        SET status = 2
                      WHERE id_professor = @id";

            return db.Execute(query, new { id });
        }
    }
}
