using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Domain.Models.Response
{
    public class UnidadeResponse
    {
        public int IdUnidade { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public int Status { get; set; }
    }
}
