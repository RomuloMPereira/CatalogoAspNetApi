using CatalogoApi.Context;
using CatalogoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _context.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            //Validação do modelo é feita automaticamente a partir do ASP.NET 2.0 com a anotação ApiController
            _context.Produtos.Add(produto);
            _context.SaveChanges();

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
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(produto == null)
            {
                return NotFound();
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }
    }
}
