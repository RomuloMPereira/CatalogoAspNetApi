using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly ILogger _logger;
        public CategoriasController(IUnityOfWork unityOfWork, ILogger<CategoriasController> logger)
        {
            _unityOfWork = unityOfWork;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _unityOfWork.CategoriaRepository.Get().ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound($"A categoria com id={id} não foi encontrada");
            }
            return categoria;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _unityOfWork.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _unityOfWork.CategoriaRepository.Add(categoria);
            _unityOfWork.Commit();

            //Retorna um header location
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria); 
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest($"Não foi possível alterar a categoria com id={id}");
            }
            _unityOfWork.CategoriaRepository.Update(categoria);
            _unityOfWork.Commit();

            return Ok($"A categoria com id={id} foi alterada com sucesso");
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound($"A categoria com id={id} não foi encontrada");
            }
            _unityOfWork.CategoriaRepository.Delete(categoria);
            _unityOfWork.Commit();
            return categoria;
        }
    }
}
