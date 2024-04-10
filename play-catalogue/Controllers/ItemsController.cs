using Catalogue_Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static Catalogue_Library.Dtos;

namespace play_catalogue.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(),"Potion", "Restores a small amount of Hp", 5, DateTimeOffset.UtcNow ),
            new ItemDto(Guid.NewGuid(),"Antidote", "Cures poison", 7, DateTimeOffset.UtcNow ),
            new ItemDto(Guid.NewGuid(),"Bronze sword", "Details a small amount of damage", 20, DateTimeOffset.UtcNow ),
        };

    
        [HttpGet("GetItems")]      
        public IEnumerable<ItemDto> GetItems()
        {
            return items;
        }

     
        [HttpGet("GetById/{id}")]       
        public ItemDto GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            return item;
        }

        [HttpPost("createitem")]
        public ActionResult<CreateItemDto> CreateItem(CreateItemDto createItem)
        {
            var addItem = new ItemDto(Guid.NewGuid(), createItem.Name, createItem.Description,
                createItem.Price, DateTimeOffset.UtcNow);

            items.Add(addItem);

           return CreatedAtAction(nameof(GetById), new {id = addItem.Id}, addItem);
           
        }

        [HttpPut("updateitem/{id}")]
        public IActionResult UpdateItem(UpdateItemDto updateItem, Guid id)
        {
            //  var existingItem =  GetById(id);

            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

            var updatedItem = existingItem with
            {
                Name = updateItem.Name, 
                Description = updateItem.Description, 
                Price = updateItem.Price,
            };

            var itemIndex = items.FindIndex(x => x.Id == id);
            items[itemIndex] = updatedItem;
            return NoContent();

        }

        [HttpDelete("deleteitem/{id}")]
        public IActionResult DeleteItem(Guid id)
        {
            var itemIndex = items.FindIndex(x => x.Id == id);
            items.Remove(items[itemIndex]);
            return NoContent();
        }


    }
}
