using Catalogue_Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue_Library.Repository
{
    public interface IItemRepository
    {
       Task CreateItemAsync(Item entity);
       Task UpdateItemAsync(Item existingEntity);

       Task<IReadOnlyCollection<Item>> GetAllAsync();
       Task<Item> GetAsync(Guid id);
        Task RemoveItemAsync(Guid id);




    }
}
