using AutoMapper;
using CatalogoApi.Context;
using CatalogoApi.DTOs;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly IMapper _mapper;
        public CategoriasController(IUnityOfWork unityOfWork, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _unityOfWork.CategoriaRepository.GetCategorias(categoriasParameters);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound($"A categoria com id={id} não foi encontrada");
            }
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            var categorias = _unityOfWork.CategoriaRepository.GetCategoriasProdutos().ToList();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _unityOfWork.CategoriaRepository.Add(categoria);
            _unityOfWork.Commit();
            var categoriaDtoAdded = _mapper.Map<CategoriaDTO>(categoria);

            //Retorna um header location
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDtoAdded); 
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest($"Não foi possível alterar a categoria com id={id}");
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _unityOfWork.CategoriaRepository.Update(categoria);
            _unityOfWork.Commit();

            return Ok($"A categoria com id={id} foi alterada com sucesso");
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unityOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound($"A categoria com id={id} não foi encontrada");
            }
            _unityOfWork.CategoriaRepository.Delete(categoria);
            _unityOfWork.Commit();
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }
    }
}
