using System.Collections.Generic;
using SharpShoppingList.Models;

namespace SharpShoppingList.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();

        int Add(Product product);

        int Delete(int id);
    }
}