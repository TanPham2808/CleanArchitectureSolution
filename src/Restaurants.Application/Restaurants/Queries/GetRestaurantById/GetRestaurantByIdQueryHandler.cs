using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
    {
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly ILogger<GetRestaurantByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        public GetRestaurantByIdQueryHandler(
            IRestaurantsRepository restaurantsRepository,
            ILogger<GetRestaurantByIdQueryHandler> logger,
            IMapper mapper,
            IBlobStorageService blobStorageService)
        {
            _restaurantsRepository = restaurantsRepository;
            _logger = logger;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
        }

        public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting restaurant {request.Id}");
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);

            var restaurantDto = _mapper.Map<RestaurantDto?>(restaurant);
            restaurantDto.LogoSaaUrl = _blobStorageService.GetBlobSasUrl(restaurant?.LogoUrl);

            return restaurantDto;
        }
    }
}
