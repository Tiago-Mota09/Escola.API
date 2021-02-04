using Escola.API.Business;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Escola.API.Controllers
{
    [Route("api/professoresAlunos")]
    public class ProfessorAlunoController : ControllerBase
    {
        private readonly ProfessorAlunoBL _professorAlunoBL;
        public ProfessorAlunoController(ProfessorAlunoBL professorAlunoBL)
        {
            _professorAlunoBL = professorAlunoBL;
        }

        /// <summary>
        /// Cadastrar relação de professor e aluno
        /// </summary>
        /// <param name="professorAlunoReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] ProfessorAlunoRequest professorAlunoReq)
        {
            var idProfessorAluno = _professorAlunoBL.Insert(professorAlunoReq);

            return CreatedAtAction(nameof(GetById), new { id = idProfessorAluno }, professorAlunoReq);
        }

        /// <summary>
        /// Atualiza relação de Professor e Aluno por ID
        /// </summary>
        /// <param name="professorAlunoUpdateRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] ProfessorAlunoUpdateRequest professorAlunoUpdateRequest)
        {
            var linhasAfetadas = _professorAlunoBL.Update(professorAlunoUpdateRequest);

            if (linhasAfetadas == 1)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new { message = "Erro ao atualizar o a relação entre professor e aluno, contate o administrador" });
            }
        }

        /// <summary>
        /// Retorna todos os alunos de acordo com o id do professor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ProfessorAlunoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult GetAllById(int id)
        {
            var professorResponse = _professorAlunoBL.GetAllProfessorAlunoById(id);

            if (professorResponse != null)
            {
                return Ok(professorResponse);
            }
            else
            {
                return NotFound(new Response { Message = "Nenhum Aluno foi encontrado para esse professor." });
            }
        }

        private IActionResult Delete(int id)
        {
            var linhasAfetadas = _professorAlunoBL.Delete(id);

            if (linhasAfetadas == 1)
            {
                return Ok(new { message = "Excluido com sucesso" });
            }
            else
            {
                return NotFound(new { message = "Nenhum relação entre professor e aluno foi encontrada." });
            }
        }

        private IActionResult GetById(int id)
        {
            var professorAlunosResponse = _professorAlunoBL.GetProfessorAlunoById(id);

            if (professorAlunosResponse != null)
            {
                return Ok(professorAlunosResponse);
            }
            else
            {
                return NotFound(new { message = "Nenhuma Relação entre professor e aluno foi encontrada." });
            }
        }
    }
}
