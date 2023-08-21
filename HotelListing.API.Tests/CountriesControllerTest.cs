using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Controllers;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Tests
{
    public class CountriesControllerTest
    {
        private readonly Mock<ICountriesRepository> countriesRepository;
        private readonly IMapper mapper;
        private readonly CountriesController countriesController;

        public CountriesControllerTest()
        {
            countriesRepository = new Mock<ICountriesRepository>();

            // Create an instance of MapperConfiguration to provide an IMapper instance
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Country, CreateCountryDto>().ReverseMap();
                cfg.CreateMap<Country, GetCountryDto>().ReverseMap();
                cfg.CreateMap<Country, UpdateCountryDto>().ReverseMap();
                cfg.CreateMap<Country, CountryDto>().ReverseMap();


                cfg.CreateMap<Hotel, CreateHotelDto>().ReverseMap();
                cfg.CreateMap<Hotel, GetHotelDto>().ReverseMap();
                cfg.CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
                cfg.CreateMap<Hotel, HotelDto>().ReverseMap();
            });

            // Create an instance of IMapper
            mapper = config.CreateMapper();
            countriesController = CreateController();
        }

        private CountriesController CreateController()
        {
            return new CountriesController(mapper, countriesRepository.Object);
        }

        [Fact]
        public async Task GetCountries_WhenCountriesExist_ReturnsListOfCountries()
        {
            // Arrange

            countriesRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(GetTestCountries());

            // Act
            var actionResult = await countriesController.GetCountries();

            // Assert
            var resultCountries = Assert.IsAssignableFrom<List<GetCountryDto>>(actionResult.Value);
            Assert.Equal(2, resultCountries.Count);
        }

        [Fact]
        public async Task GetCountries_WhenNoCountries_ReturnsEmptyList()
        {
            // Arrange
            countriesRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Country>());

            var controller = new CountriesController(mapper, countriesRepository.Object);

            // Act
            var actionResult = await countriesController.GetCountries();

            // Assert
            var resultCountries = Assert.IsAssignableFrom<List<GetCountryDto>>(actionResult.Value);
            Assert.Empty(resultCountries);
        }

        [Fact]
        public async Task GetCountry_WhenCountryExists_ReturnsThatCountry()
        {
            //Arrange

            var originalCountry = new Country
            {
                Id = 1,
                Name = "Poland",
                ShortName = "PL",
                Hotels = null // Initialize or populate hotels as needed
            };

            var id = 1;

            countriesRepository.Setup(repo => repo.GetDetails(id))
                .ReturnsAsync(originalCountry);

            //Act

            var actionResult = await countriesController.GetCountry(id);

            //Assert

            Assert.IsAssignableFrom<CountryDto>(actionResult.Value);
            Assert.Equal(actionResult.Value.Id, originalCountry.Id);
            Assert.Equal(actionResult.Value.Name, originalCountry.Name);
            Assert.Equal(actionResult.Value.ShortName, originalCountry.ShortName);

        }
        private List<Country> GetTestCountries()
        {
            var countries = new List<Country>
            {
                new Country { Id = 1, Name = "Poland",ShortName ="PL" },
                new Country { Id = 2, Name = "Germany",ShortName ="DE" },
            };

            return countries;
        }

    }
}
