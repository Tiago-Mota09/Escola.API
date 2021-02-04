using AutoMapper;
using Escola.API.Business;
using Escola.API.Data.Entities;
using Escola.API.Data.Repositories;
using Escola.API.Domain.Models.Request;
using Escola.API.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Escola.API.Controllers
{
    [Route("api/unidades")]

    public class UnidadesController : ControllerBase
    {
        private readonly UnidadeBL _unidadeBL;

        public UnidadesController(UnidadeBL unidadeBL)
        {
            _unidadeBL = unidadeBL;
        }

        /// <summary>
        /// Cadastrar unidade
        /// </summary>
        /// <param name="unidadeReq"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] UnidadeRequest unidadeReq)
        {
            var idUnidade = _unidadeBL.Insert(unidadeReq);

            return CreatedAtAction(nameof(GetById), new { id = idUnidade }, unidadeReq);
        }

        /// <summary>
        /// Atualizar Unidades
        /// </summary>
        /// <param name="unidadeUpdateRequest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] UnidadeUpdateRequest unidadeUpdateRequest)
        {
            var linhasAfetadas = _unidadeBL.Update(unidadeUpdateRequest);

            if (linhasAfetadas == 1)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new Response { Message = "Erro ao atualizar o cadastro da unidade, contate o administrador" });
            }
        }

        /// <summary>
        /// Buscar Unidade po ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var unidadeResponse = _unidadeBL.GetUnidadeById(id);

            if (unidadeResponse != null)
            {
                return Ok(unidadeResponse);
            }
            else
            {
                return NotFound(new Response { Message = "Nenhuma unidade encontrada." });
            }
        }

        /// <summary>
        /// Buscar todas as Unidades
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        [ProducesResponseType(typeof(IEnumerable<UnidadeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var unidadeResponse = _unidadeBL.GetAllUnidades();

            if (unidadeResponse.Any())
            {
                return Ok(unidadeResponse);
            }
            else
            {
                return NotFound(new { message = "Nenhuma unidade foi encontrada." });
            }
        }

        /// <summary>
        /// Deletar Unidades por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(UnidadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)

        {
            var linhasAfetadas = _unidadeBL.Delete(id);

            if (linhasAfetadas == 1)
            {
                return Ok(new { message = "unidade excluida com sucesso" });
            }
            else
            {
                return NotFound(new { message = "Nenhuma unidade foi encontrado." });
            }
        }
    }
}
