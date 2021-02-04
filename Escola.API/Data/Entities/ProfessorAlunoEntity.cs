using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Data.Entities
{
    public class ProfessorAlunoEntity
    {
        public int IdProfessorAluno { get; set; }
        public int IdAluno { get; set; }
        public int IdProfessor { get; set; }
    }
}
