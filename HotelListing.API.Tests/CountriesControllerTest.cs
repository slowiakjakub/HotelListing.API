using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Controllers;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
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

        public CountriesControllerTest()
        {
            countriesRepository = new Mock<ICountriesRepository>();

            // Create an instance of MapperConfiguration to provide an IMapper instance
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Country, GetCountryDto>();
            });

            // Create an instance of IMapper
            mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetCountries_ReturnsListOfCountries()
        {
            // Arrange

            countriesRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(GetTestCountries());

            var controller = new CountriesController(mapper, countriesRepository.Object);

            // Act
            var actionResult = await controller.GetCountries();

            // Assert
            var resultCountries = Assert.IsAssignableFrom<List<GetCountryDto>>(actionResult.Value);
            Assert.Equal(2, resultCountries.Count);
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
