using AutoMapper;
using CatalogoApi.DTOs;
using CatalogoApi.Filters;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CatalogoApi.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [EnableCors("PermitirApiRequest")]
    [Route("[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;
        public ProdutosController(IUnityOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _unityOfWork.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _unityOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return produtoDto;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _unityOfWork.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);
            _unityOfWork.ProdutoRepository.Add(produto);
            _unityOfWork.Commit();
            var produtoDtoAdded = _mapper.Map<ProdutoDTO>(produto);

            //Retorna um header location
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDtoAdded);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }
            var produto = _mapper.Map<Produto>(produtoDto);
            _unityOfWork.ProdutoRepository.Update(produto);
            _unityOfWork.Commit();
            var produtoDtoAdded = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDtoAdded);
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _unityOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            _unityOfWork.ProdutoRepository.Delete(produto);
            _unityOfWork.Commit();
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return produtoDto;
        }
    }
}
