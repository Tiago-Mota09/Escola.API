using Dapper;
using Escola.API.Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Escola.API.Data.Repositories
{
    public class AlunoRepository : RepositoryBase
    {
        public AlunoRepository(IConfiguration configuration)
        {
            base.configuration = configuration;
        }

        public int Insert(AlunoEntity aluno)
        {
            using var db = Connection;

            var query = @"INSERT INTO ALUNO
                            (nome,
                             idade,
                             data_nascimento,
                             id_unidade)
                            values( 
                            @Nome, 
                            @Idade,
                            @DataNascimento,
                            @IdUnidade)
                            RETURNING id_aluno;";

            return db.ExecuteScalar<int>(query, new
            {
                aluno.Nome,
                aluno.Idade,
                aluno.DataNascimento,
                aluno.IdUnidade
            });
        }
        public int Update(AlunoEntity aluno)
        {
            using var db = Connection;

            var query = @"UPDATE Aluno
                            SET nome = @Nome,
                                idade = @Idade,
                                data_nascimento = @DataNascimento
                            WHERE id_aluno = @IdAluno AND status = 1;";

            return db.Execute(query, new
            {
                aluno.Nome,
                aluno.Idade,
                aluno.DataNascimento,
                aluno.IdUnidade,
                aluno.IdAluno
            });
        }
        public AlunoEntity GetAluno(int idAluno)
        {
            using var db = Connection;

            var query = @"SELECT id_aluno,
                              nome,
                              idade,
                              data_nascimento,
                              status,
                              id_unidade
                              FROM Aluno
                          WHERE id_aluno = @idAluno
                              AND status = 1 ;";

            return db.QueryFirstOrDefault<AlunoEntity>(query, new { idAluno });
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

        public string GetNomeAlunoById(int idAluno)
        {
            using var db = Connection;

            var query = @"SELECT nome 
                            FROM aluno 
                        WHERE id_aluno = @idAluno 
                            AND status = 1;";

            return db.ExecuteScalar<string>(query, new { idAluno });
        }

        public int GetIdByNome(string nome)
        {
            using var db = Connection;

            var query = @"select id_aluno 
	                        from aluno
                        WHERE nome = @Nome
	                        AND status = 1";

            return db.ExecuteScalar<int>(query, new { nome });
        }
        public IEnumerable<AlunoEntity> GetAllAlunos()
        {
            using var db = Connection;

            var query = @"SELECT id_aluno,
                             nome,
                             idade,
                             data_nascimento,
                             status,
                             id_unidade
                        FROM Aluno
                            WHERE status = 1; ";

            return db.Query<AlunoEntity>(query);
         }
        public int Delete(int id)
        {
        using var db = Connection;

        var query = @"UPDATE Aluno 
                        SET status = 2
                      WHERE id_aluno = @id";

            return db.Execute(query, new { id });
        }
    }
}
