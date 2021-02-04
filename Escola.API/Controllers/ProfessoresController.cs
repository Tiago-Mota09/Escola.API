using Escola.API.Business;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Escola.API.Controllers
{
    [Route("api/professores")]

    public class ProfessoresController : ControllerBase
    {
        private readonly ProfessorBL _professorBL;
        public ProfessoresController(ProfessorBL professorBL)
        {
            _professorBL = professorBL;
        }

        /// <summary>
        /// Cadastrar professores
        /// </summary>
        /// <param name="professorReq">JSON</param>
        /// <returns>JSON</returns>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] ProfessorRequest professorReq)
        {
            var idProfessor = _professorBL.Insert(professorReq);

            return CreatedAtAction(nameof(GetById), new { id = idProfessor }, professorReq);
        }

        /// <summary>
        /// Atualizar Professor
        /// </summary>
        /// <param name="professorUpdateRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] ProfessorUpdateRequest professorUpdateRequest)
        {
            var linhasAfetadas = _professorBL.Update(professorUpdateRequest);

            if (linhasAfetadas == 1)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new { message = "Erro ao atualizar o cadastro de professor, contate o administrador" });
            }
        }

        /// <summary>
        /// Buscar Professor por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ProfessorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var professorResponse = _professorBL.GetProfessorById(id);

            if (professorResponse != null)
            {
                return Ok(professorResponse);
            }
            else
            {
                return NotFound(new Response { Message = "Nenhum professor foi encontrado." });
            }
        }

        /// <summary>
        /// Buscar todos os Professores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        [ProducesResponseType(typeof(IEnumerable<ProfessorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var professorResponse = _professorBL.GetAllProfessores();

            if (professorResponse.Any())
            {
                return Ok(professorResponse);
            }
            else
            {
                return NotFound(new { message = "Nenhum professor foi encontrado." });
            }
        }

        /// <summary>
        /// Deletar Professores por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var linhasAfetadas = _professorBL.Delete(id);

            if (linhasAfetadas == 1)
            {
                return Ok(new { message = "Professor excluido com sucesso" });
            }
            else
            {
                return NotFound(new { message = "Nenhum professor foi encontrado." });
            }
        }
    }
}

