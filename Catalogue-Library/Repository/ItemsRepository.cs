using Catalogue_Library.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Catalogue_Library.Repository
{


    public class ItemsRepository : IItemRepository
    {
        private const string CollectionName = "items"; //table Name 

        private readonly IMongoCollection<Item> dbCollection;

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;


        public ItemsRepository(IMongoDatabase database)
        {
           /* var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalogue");*/
            dbCollection = database.GetCollection<Item>(CollectionName);

        }


        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter =  filterBuilder.Eq(entity => entity.Id, id);

            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateItemAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
          await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateItemAsync(Item existingEntity)
        {
            if (existingEntity == null)
            {
                throw new ArgumentNullException(nameof(existingEntity));
            }

            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, existingEntity.Id);

            await dbCollection.ReplaceOneAsync(filter, existingEntity);
        }

        public async Task RemoveItemAsync(Guid id)
        {
           FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);

           await dbCollection.DeleteOneAsync(filter);
        }


    }
}
