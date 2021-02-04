using System;

namespace Escola.API.Domain.Models.Response
{
    public class AlunoResponse
    {
        public int IdAluno { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public DateTime DataNascimento { get; set; }
        public int IdUnidade { get; set; }
    }
}
