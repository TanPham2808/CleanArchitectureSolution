using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
    {
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly ILogger<CreateRestaurantCommandHandler> _logger;
        private readonly AutoMapper.IMapper _mapper;
        private IUserContext _userContext;

        public CreateRestaurantCommandHandler(
            IRestaurantsRepository restaurantsRepository,
            ILogger<CreateRestaurantCommandHandler> logger,
            IUserContext userContext,
            AutoMapper.IMapper mapper)
        {
            _restaurantsRepository = restaurantsRepository;
            _logger = logger;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            _logger.LogInformation("{UserEmail} {UserId} Creating a new restaurant {@Restaurant}",
                currentUser?.Email,
                currentUser?.Id,
                request);


            var restaurant = _mapper.Map<Restaurant>(request);
            restaurant.OwnerId = currentUser!.Id;

            int id = await _restaurantsRepository.Create(restaurant);
            return id;
        }
    }
}
