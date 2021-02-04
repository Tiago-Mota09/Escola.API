using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Domain.Models.Request
{
    public class ProfessorAlunoUpdateRequest : ProfessorAlunoRequest
    {
        public int? IdProfessorAluno{ get; set; }
    }
}
