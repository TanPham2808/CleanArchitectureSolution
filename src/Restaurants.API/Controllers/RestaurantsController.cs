using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Contant;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    //[Authorize]
    public class RestaurantsController : ControllerBase
    {
        public readonly IRestaurantsService _restaurantsService;
        public readonly IMediator _mediator;

        //public RestaurantsController(IRestaurantsService restaurantsService)
        //{
        //    _restaurantsService = restaurantsService;
        //}

        public RestaurantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
        {
            //var restaurants = await _restaurantsService.GetAllRestaurants();
            var restaurants = await _mediator.Send(query);
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = PolicyNames.HasNationality)] // Chỉ người dùng có quốc tịch mới có quyền truy cập 
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //var restaurant = await _restaurantsService.GetById(id);
            var restaurant = await _mediator.Send(new GetRestaurantByIdQuery(id));
            if (restaurant is null)
                return NotFound();

            return Ok(restaurant);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto)
        //{
        //    int id = await _restaurantsService.Create(createRestaurantDto);
        //    return CreatedAtAction(nameof(GetById), new { id }, null);
        //}

        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)] // Chỉ chủ nhà hàng (Owner) mới có quyền tạo nhà hàng
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
        {
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            var isDeleted = await _mediator.Send(new DeleteRestaurantComand(id));

            if (isDeleted)
                return NoContent();

            return NotFound();
        }

        [HttpPost("{id}/logo")]
        public async Task<IActionResult> UploadLogo([FromRoute] int id, IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var command = new UploadRestaurantLogoCommand()
            {
                RestaurantId = id,
                FileName = file.FileName,
                File = stream
            };

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
