

using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

public class RestaurantsService : IRestaurantsService
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<RestaurantsService> _logger;
    private readonly IMapper _mapper;

    public RestaurantsService(IRestaurantsRepository restaurantsRepository,
        ILogger<RestaurantsService> logger,
        IMapper mapper)
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    //public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    //{
    //    _logger.LogInformation("Fetching all restaurants from the repository.");

    //    var restaurants = await _restaurantsRepository.GetAllAsync();

    //    //var restaurantDtos = restaurants.Select(RestaurantDto.FromEntity);
    //    var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

    //    return restaurantDtos;

    //}

    //public async Task<RestaurantDto?> GetById(int id)
    //{
    //    _logger.LogInformation($"Getting restaurant {id}");
    //    var restaurant = await _restaurantsRepository.GetByIdAsync(id);
        
    //    //var restaurantDto = RestaurantDto.FromEntity(restaurant);
    //    var restaurantDto = _mapper.Map<RestaurantDto?>(restaurant);

    //    return restaurantDto;
    //}

    //public async Task<int> Create(CreateRestaurantDto dto)
    //{
    //    _logger.LogInformation("Creating a new restaurant");

    //    var restaurant = _mapper.Map<Restaurant>(dto);

    //    int id = await _restaurantsRepository.Create(restaurant);
    //    return id;
    //}
}

