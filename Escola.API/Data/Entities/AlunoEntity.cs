using System;

namespace Escola.API.Data.Entities
{
    public class AlunoEntity
    {
        public int IdAluno { get; set; }
        public  string Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Status { get; set; }
        public int IdUnidade { get; set; }
    }
}
