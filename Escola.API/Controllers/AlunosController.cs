using AutoMapper;
using Escola.API.Business;
using Escola.API.Data.Entities;
using Escola.API.Data.Repositories;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Signa.Library.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Escola.API.Controllers
{
    [Route("api/alunos")]

    public class AlunosController : ControllerBase
    {
        private readonly AlunoBL _alunoBL;
        public AlunosController(AlunoBL alunoBL)
        {
            _alunoBL = alunoBL;
        }

        /// <summary>
        /// Cadastrar alunos
        /// </summary>
        /// <param name="alunoReq">JSON</param>
        /// <returns>JSON</returns>
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] AlunoRequest alunoReq)
        {
            var idAluno = _alunoBL.Insert(alunoReq);

            return CreatedAtAction(nameof(GetById), new { id= idAluno}, alunoReq);
        }

        /// <summary>
        /// Atualizar Alunos
        /// </summary>
        /// <param name="alunoUpdateRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] AlunoUpdateRequest alunoUpdateRequest)
        {
            var linhasAfetadas = _alunoBL.Update(alunoUpdateRequest);

            if (linhasAfetadas == 1)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new { message = "Erro ao atualizar o cadastro de aluno, contate o administrador" });
            }
        }

        /// <summary>
        /// Busca Aluno por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(AlunoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var alunoResponse = _alunoBL.GetAlunoById(id);

            if (alunoResponse != null)
            {
                return Ok(alunoResponse);
            }
            else
            {
                return NotFound(new Response { Message = "Nenhum aluno foi encontrado." });
            }
        }

        /// <summary>
        /// Busca todos os Alunos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        [ProducesResponseType(typeof(IEnumerable<AlunoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var alunoResponse = _alunoBL.GetAllAlunos();

            if (alunoResponse.Any ())
            {
                return Ok(alunoResponse);
            }
            else
            {
                return NotFound(new Response { Message = "Nenhum aluno foi encontrado." });
            }
        }

        /// <summary>
        /// Deleta os Alunos por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var linhasAfetadas = _alunoBL.Delete(id);

            if (linhasAfetadas == 1)
            {
                return Ok(new Response { Message = "Aluno excluido com sucesso" });
            }
            else
            {
                return NotFound(new Response{ Message = "Nenhum aluno foi encontrado." });
            }
        }
    }
}

