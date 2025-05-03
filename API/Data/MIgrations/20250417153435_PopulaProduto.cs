using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.MIgrations
{
    /// <inheritdoc />
    public partial class PopulaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" + 
            "Values('Coca-cola', 'Refrigerante de Cola 350ml', 5.45, 'cocacola.jpg', 50,GETDATE(), 1)");

            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" + 
            "Values('Lanche de Atum','Lanche de atum com maionese', 8.50, 'atum.jpg', 10,GETDATE(), 2)");

            migrationBuilder.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)" + 
            "Values('Pudim  100g', 'Pudim de leite condensado 100g', 6.75, 'pudim.jpg', 20,GETDATE(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
