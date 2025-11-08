using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Contant;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantComandHandler : IRequestHandler<DeleteRestaurantComand, bool>
    {
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly ILogger<DeleteRestaurantComandHandler> _logger;
        private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;

        public DeleteRestaurantComandHandler(IRestaurantsRepository restaurantsRepository, 
            ILogger<DeleteRestaurantComandHandler> logger,
            IRestaurantAuthorizationService restaurantAuthorizationService)
        {
            _restaurantsRepository = restaurantsRepository;
            _logger = logger;
            _restaurantAuthorizationService = restaurantAuthorizationService;
        }

        public async Task<bool> Handle(DeleteRestaurantComand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Deleting restaurant with id : {request.Id}");
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);
            if (restaurant is null)
                //return false;
                throw new NotFoundException($"Restaurant", request.Id.ToString());

            if(!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            {
                _logger.LogWarning("Authorization failed for deleting restaurant with id : {RestaurantId}", request.Id);
                throw new ForbiException();
            }

            await _restaurantsRepository.Delete(restaurant);
            return true;
        }
    }
}
