using Catalogue_Library;
using Catalogue_Library.Entities;
using Catalogue_Library.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Catalogue_Library.Dtos;

namespace play_catalogue.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        /*  public static readonly List<ItemDto> items = new()
          {
              new ItemDto(Guid.NewGuid(),"Potion", "Restores a small amount of Hp", 5, DateTimeOffset.UtcNow ),
              new ItemDto(Guid.NewGuid(),"Antidote", "Cures poison", 7, DateTimeOffset.UtcNow ),
              new ItemDto(Guid.NewGuid(),"Bronze sword", "Details a small amount of damage", 20, DateTimeOffset.UtcNow ),
          };*/

        private readonly IItemRepository _itemsRepository;

        public ItemsController(IItemRepository itemsRepository) 
        {
            _itemsRepository = itemsRepository;
        }

    
        [HttpGet("GetItems")]      
        public async Task<IEnumerable<ItemDto>> GetItems()
        {
            var items = (await _itemsRepository.GetAllAsync()).Select(x => x.AsDto());
            return items;
        }

     
        [HttpGet("GetById/{id}")]       
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
           // var item = items.Where(item => item.Id == id).SingleOrDefault();
           var item = await _itemsRepository.GetAsync(id);
            if(item == null)
                return NotFound();

            return Ok(item.AsDto());
        }

        [HttpPost("createitem")]
        public async Task<ActionResult<CreateItemDto>> CreateItem(CreateItemDto createItem)
        {
            var addItem = new Item
            {
              Id =   Guid.NewGuid(), 
              Name = createItem.Name, 
              Description =  createItem.Description,
              Price =   createItem.Price,
              CreatedDate =  DateTimeOffset.UtcNow 
            };

           // items.Add(addItem);
           await _itemsRepository.CreateItemAsync(addItem);

           return CreatedAtAction(nameof(GetById), new {id = addItem.Id}, addItem); //CreatedAtAction is similar to 201

        }

        [HttpPut("updateitem/{id}")]
        public async Task<IActionResult> UpdateItem(UpdateItemDto updateItem, Guid id)
        {
            //  var existingItem =  GetById(id);

            // var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

             var existingItem = await _itemsRepository.GetAsync(id);

            if (existingItem == null)
                return NotFound();


            existingItem.Name = updateItem.Name;
            existingItem.Description = updateItem.Description;
            existingItem.Price = updateItem.Price;
          

            await _itemsRepository.UpdateItemAsync(existingItem);

            return NoContent();

        }

        [HttpDelete("deleteitem/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);

            if (item == null)
                return NotFound();

           
            await _itemsRepository.RemoveItemAsync(item.Id);

            return NoContent();
        }


    }
}
