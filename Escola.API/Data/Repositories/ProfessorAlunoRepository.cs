using Dapper;
using Escola.API.Data.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Escola.API.Data.Repositories
{
    public class ProfessorAlunoRepository : RepositoryBase
    {
        public ProfessorAlunoRepository(IConfiguration configuration)
        {
            base.configuration = configuration;
        }
        public int Insert(ProfessorAlunoEntity professorAluno)
        {
            using var db = Connection;

            var query = @"insert into professor_aluno 
                            (id_professor,
                             id_aluno)
		                  values (@IdProfessor, @IdAluno) returning id_professor_aluno";

            return db.ExecuteScalar<int>(query, new
            {
              professorAluno.IdProfessor,  
              professorAluno.IdAluno
            });
        }
        public int Update(ProfessorAlunoEntity professorAluno)
        {
            using var db = Connection;

            var query = @"UPDATE Professor_aluno
                            SET id_professor = @IdProfessor,
                                id_aluno = @IdAluno
                          WHERE id_professor_Aluno = @IdProfessorAluno AND status = 1;";

            return db.Execute(query, new
            {
                professorAluno.IdProfessor,
                professorAluno.IdAluno,
                professorAluno.IdProfessorAluno
            });
        }

        //public ProfessorAlunoEntity GetProfessor_Aluno(int idProfessorAluno)
        //{
        //    using var db = Connection;

        //    var query = @"SELECT id_professor_Aluno, 
        //                     nome,
        //                     status, 
        //                     id_unidade
        //                     FROM professor_Aluno
        //                  WHERE id_professor_Aluno = @idProfessor_Aluno
        //                     AND status = 1;";

        //    return db.QueryFirstOrDefault<ProfessorAlunoEntity>(query, new { idProfessorAluno });
        //}

        //public int GetStatusUnidadeById(int idUnidade)
        //{
        //    using var db = Connection;

        //    var query = @"SELECT  status
        //                    FROM unidade
        //                  WHERE id_unidade = @idUnidade
        //                    AND status = 1;";

        //    return db.ExecuteScalar<int>(query, new { idUnidade });
        //}

        public IEnumerable<ProfessorAlunoEntity> GetAllProfessorAlunoById(int idProfessor)
        {
            using var db = Connection;

            var query = @"SELECT  pa.id_professor_aluno,
                                  al.id_aluno, 
		                          al.nome nome_aluno
	                        FROM professor_aluno pa
		                          INNER join aluno al
			                        ON pa.id_aluno = al.id_aluno
	                        WHERE pa.id_professor = @idProfessor AND pa.status = 1";

            return db.Query<ProfessorAlunoEntity>(query, new { idProfessor });
        }

        public ProfessorAlunoEntity GetProfessorAlunoById(int idProfessorAluno)
        {
            using var db = Connection;

            var query = @"SELECT  pa.id_professor_aluno,
                                  al.id_aluno, 
		                          al.nome nome_aluno
	                        FROM professor_aluno pa
		                          INNER join aluno al
			                        ON pa.id_aluno = al.id_aluno
	                        WHERE pa.id_professor_aluno = @idProfessorAluno AND pa.status = 1";

            return db.QueryFirstOrDefault<ProfessorAlunoEntity>(query, new { idProfessorAluno });
        }

        public int GetStatusProfessorAlunoById(int idProfessorAluno)
        {
            using var db = Connection;

            var query = @"Select status 
                               FROM professor_aluno
                          WHERE id_professor_aluno = @idProfessorAluno";

            return db.ExecuteScalar<int>(query, new { idProfessorAluno });
        }

        //public int GetIdByNome(string nome)
        //{
        //    using var db = Connection;

        //    var query = @"select al.nome nome_aluno
        //                 from professor_aluno pa
        //                where nome = @Nome
        //                 AND status = 1";

        //    return db.ExecuteScalar<int>(query, new { nome });
        //}
        //public IEnumerable<ProfessorAlunoEntity> GetAllProfessoresAlunos()
        //{
        //    using var db = Connection;

        //    var query = @"Select id_professor_Aluno, 
        //                  nome, 
        //                  idade, 
        //                  data_nascimento, 
        //                  status, 
        //                  id_unidade
        //              From professor_Aluno 
        //                Where status = 1; ";

        //    return db.Query<ProfessorAlunoEntity>(query);
        //}
        public int Delete(int id)
        {
            using var db = Connection;

            var query = @"UPDATE Professor_Aluno 
                        SET status = 2
                      WHERE id_professor_Aluno = @id";

            return db.Execute(query, new { id });
        }
    }
}
