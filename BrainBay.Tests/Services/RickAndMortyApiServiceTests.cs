using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using BrainBay.Core.Models;
using BrainBay.Core.Services;

namespace BrainBay.Tests.Services
{
    public class RickAndMortyApiServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly RickAndMortyApiService _service;

        public RickAndMortyApiServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://rickandmortyapi.com/api/")
            };
            _service = new RickAndMortyApiService(_httpClient);
        }

        [Fact]
        public async Task GetAliveCharactersAsync_ReturnsOnlyAliveCharacters()
        {
            // Arrange
            var response = new
            {
                Results = new[]
                {
                    new Character { Id = 1, Name = "Rick", Status = "Alive" },
                    new Character { Id = 2, Name = "Morty", Status = "Alive" },
                    new Character { Id = 3, Name = "Dead Guy", Status = "Dead" }
                }
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(response)
                });

            // Act
            var result = await _service.GetAliveCharactersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, c => Assert.Equal("Alive", c.Status));
        }

        [Fact]
        public async Task GetAliveCharactersAsync_ReturnsEmptyList_WhenApiReturnsNoResults()
        {
            // Arrange
            var response = new { Results = Array.Empty<Character>() };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(response)
                });

            // Act
            var result = await _service.GetAliveCharactersAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAliveCharactersAsync_ThrowsException_WhenApiFails()
        {
            // Arrange
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAliveCharactersAsync());
        }

        [Fact]
        public async Task GetAliveCharactersAsync_ThrowsException_WhenApiReturnsInvalidJson()
        {
            // Arrange
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("invalid json")
                });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAliveCharactersAsync());
        }
    }
} 