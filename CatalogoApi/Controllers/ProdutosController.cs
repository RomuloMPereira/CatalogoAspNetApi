using CatalogoApi.Filters;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CatalogoApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        public ProdutosController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _unityOfWork.ProdutoRepository.Get().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _unityOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _unityOfWork.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            //Validação do modelo é feita automaticamente a partir do ASP.NET 2.0 com a anotação ApiController
            _unityOfWork.ProdutoRepository.Add(produto);
            _unityOfWork.Commit();

            //Retorna um header location
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }
            //Alterar o estado da entidade para Modified, fazer as alterações e persistir no banco
            _unityOfWork.ProdutoRepository.Update(produto);
            _unityOfWork.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _unityOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            _unityOfWork.ProdutoRepository.Delete(produto);
            _unityOfWork.Commit();
            return produto;
        }
    }
}
