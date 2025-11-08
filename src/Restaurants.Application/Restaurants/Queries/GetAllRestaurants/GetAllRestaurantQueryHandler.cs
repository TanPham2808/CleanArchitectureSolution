using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantQueryHandler : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
    {
        private readonly IRestaurantsRepository _restaurantsRepository;
        private readonly ILogger<GetAllRestaurantQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllRestaurantQueryHandler(
            IRestaurantsRepository restaurantsRepository,
            ILogger<GetAllRestaurantQueryHandler> logger,
            IMapper mapper)
        {
            _restaurantsRepository = restaurantsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantQuery request, CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("Get all restaurants.");

        //    //var restaurants = await _restaurantsRepository.GetAllAsync();

        //    var restaurants = await _restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase);
        //    var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        //    return restaurantDtos;
        //}

        public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var (restaurants, totalCount) = await _restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase,
                request.PageSize,
                request.PageNumber,
                request.SortBy,
                request.SortDirection);

            var restaurantsDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            var result = new PagedResult<RestaurantDto>(restaurantsDtos, totalCount, request.PageSize, request.PageNumber);
            return result;
        }
    }
}
