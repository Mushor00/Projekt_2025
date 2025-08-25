using System;
using System.Threading.Tasks;
using API.ApiService;
using API.ApiService.DB;
using API.Test;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using API.Web;
using API.Web.Components.Pages;

public class ReservationServiceTests
{
    private readonly Mock<ReservationRepo> _repoMock;
    private readonly Mock<ILogger<ReservationService>> _loggerMock;
    private readonly ReservationService _service;

    public ReservationServiceTests()
    {
        _repoMock = new Mock<ReservationRepo>();
        _loggerMock = new Mock<ILogger<ReservationService>>();
        _service = new ReservationService(_repoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ZarezerwujSaleAsync_PoprawneDane_ZwracaTrue()
    {
        // Arrange
        _repoMock.Setup(r => r.ZarezerwujSale(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>()))
            .ReturnsAsync((true, "Sukces"));

        // Act
        var result = await _service.ZarezerwujSaleAsync(101, "Jan", "Kowalski", "Matematyka",
            DateOnly.FromDateTime(DateTime.Now), new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Assert
        Assert.True(result.Success);
        _repoMock.Verify(
            r => r.ZarezerwujSale(101, "Jan", "Kowalski", "Matematyka", It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(),
                It.IsAny<TimeOnly>()), Times.Once);
    }

    [Fact]
    public async Task UsunRezerwacjeAsync_PoprawneId_ZwracaTrue()
    {
        // Arrange
        _repoMock.Setup(r => r.UsunRezerwacjeAsync(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        bool result = await _service.UsunRezerwacjeAsync(1);

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.UsunRezerwacjeAsync(1), Times.Once);
    }

    [Fact]
    public async Task UsunRezerwacjeAsync_WystapilWyjatek_ZwracaFalseILogujeBlad()
    {
        // Arrange
        var exception = new Exception("Błąd bazy");
        _repoMock.Setup(r => r.UsunRezerwacjeAsync(It.IsAny<int>())).ThrowsAsync(exception);

        // Act
        bool result = await _service.UsunRezerwacjeAsync(1);

        // Assert
        Assert.False(result);
        _repoMock.Verify(r => r.UsunRezerwacjeAsync(1), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Błąd podczas usuwania rezerwacji.")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task EdytujRezerwacjeAsync_GdyRepoRzucaWyjatek_ZwracaFalseILogujeBlad()
    {
        // Arrange
        var exception = new Exception("Błąd repozytorium");
        _repoMock.Setup(r => r.EdytujRezerwacjeAsync(
                It.IsAny<int>(),
                It.IsAny<DateOnly>(),
                It.IsAny<TimeOnly>(),
                It.IsAny<TimeOnly>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _service.EdytujRezerwacjeAsync(
            1,
            DateOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now.AddHours(1)));

        // Assert
        Assert.False(result.Success);  // <-- sprawdzamy właściwość Success
        Assert.Equal("Błąd podczas edytowania rezerwacji.", result.Message);

        _repoMock.Verify(r => r.EdytujRezerwacjeAsync(
            1,
            It.IsAny<DateOnly>(),
            It.IsAny<TimeOnly>(),
            It.IsAny<TimeOnly>()), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Błąd podczas edytowania rezerwacji.")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    public class OsobyServiceSimpleTests
    {
        [Fact]
        public async Task RegisterAsync_WhenValidModel_ReturnsSuccess()
        {
            // Używamy sztucznego modelu
            var model = new Register.RegisterModel
            {
                Username = "janek",
                Password = "tajnehaslo",
                Email = "janek@example.com",
                FirstName = "Jan",
                LastName = "Kowalski",
                IndexNumber = "12345"
            };
            
            var service = TestHelpers.CreateOsobyService();

            var result = await service.RegisterAsync(model);

            Assert.True(result.Success);
            Assert.Equal("Rejestracja zakończona sukcesem.", result.Message);
        }

        [Fact]
        public async Task LoginAsync_WhenInvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var service = TestHelpers.CreateOsobyService();

            // Act
            var result = await service.LoginAsync("bad@example.com", "wrongpass");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nieprawidłowy login lub hasło", result.Message);
        }
    }
}
