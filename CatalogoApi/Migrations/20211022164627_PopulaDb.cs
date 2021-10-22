using Microsoft.EntityFrameworkCore.Migrations;

namespace CatalogoApi.Migrations
{
    public partial class PopulaDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Bebidas', 'http://www.macoratti.net/Imagens/1.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Lanches', 'http://www.macoratti.net/Imagens/2.jpg')");
            migrationBuilder.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Sobremesas', 'http://www.macoratti.net/Imagens/3.jpg')");

            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('Coca-Cola Diet', 'Refrigerante de cola 350ml', 5.45, 'http://maccoretti.net/Imagens/coca.jpg'," +
                "50, now(), (Select CategoriaId from Categorias Where Nome='Bebidas'))");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('Lanche de atum', 'Lanche de atum com maionese', 8.50, 'http://maccoretti.net/Imagens/atum.jpg'," +
                "10, now(), (Select CategoriaId from Categorias Where Nome='Lanches'))");
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('Pudim 100g', 'Pudim de leite condensado 100g', 6.75, 'http://maccoretti.net/Imagens/pudim.jpg'," +
                "20, now(), (Select CategoriaId from Categorias Where Nome='Sobremesas'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
