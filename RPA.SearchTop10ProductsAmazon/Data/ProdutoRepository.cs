using RPA.SearchTop10ProductsAmazon.Models;

namespace RPA.SearchTop10ProductsAmazon.Data
{
    public class ProdutoRepository
    {
        private readonly DatabaseContext _context;

        public ProdutoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddProdutosAsync(List<Produto> produtos)
        {
            await _context.Produtos.AddRangeAsync(produtos);
            await _context.SaveChangesAsync();
        }
    }
}
