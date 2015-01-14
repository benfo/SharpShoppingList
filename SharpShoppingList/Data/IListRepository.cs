using SharpShoppingList.Models;
using System.Collections.Generic;

namespace SharpShoppingList.Data
{
    public interface IListRepository
    {
        IEnumerable<List> GetLists();

        int AddList(List list);

        int DeleteList(int id);
    }
}