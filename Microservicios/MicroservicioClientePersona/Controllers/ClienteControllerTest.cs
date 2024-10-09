using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MicroservicioClientePersona.Controllers;
using Microservicio.Shared;
using MicroservicioClientePersona.Models;
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;
using AutoMapper;
public class ClienteControllerTests
{
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ClienteController _clienteController;

    public ClienteControllerTests()
    {
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _mapperMock = new Mock<IMapper>();
    }

    [Fact]
    public async Task Crear_ClienteExitoso_DeberiaDevolverOkConCliente()
    {
        // Arrange
        var clienteDto = new ClienteDTO { PersonaId = 2, Clave = "ooo", Estado = true };
        var cliente = new CLIENTE { ClienteId = 1, PersonaId = 2, Clave = "ooo", Estado = true };

        _mapperMock.Setup(m => m.Map<CLIENTE>(clienteDto)).Returns(cliente);
        _clienteRepositoryMock.Setup(r => r.Crear(It.IsAny<CLIENTE>())).ReturnsAsync(cliente);

        // Act
        var result = await _clienteController.Crear(clienteDto) as ObjectResult;
        var response = result.Value as ResponseDTO<ClienteDTO>;

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.True(response.status);
        Assert.Equal("ok", response.msg);
        Assert.Equal(cliente.PersonaId, response.value.PersonaId);
    }

    [Fact]
    public async Task Crear_ClienteFallo_DeberiaDevolverError()
    {
        // Arrange
        var clienteDto = new ClienteDTO { PersonaId = 2, Clave = "ooo", Estado = true };
        var cliente = new CLIENTE { ClienteId = 0, PersonaId = 2, Clave = "ooo", Estado = true };

        _mapperMock.Setup(m => m.Map<CLIENTE>(clienteDto)).Returns(cliente);
        _clienteRepositoryMock.Setup(r => r.Crear(It.IsAny<CLIENTE>())).ReturnsAsync(cliente);

        // Act
        var result = await _clienteController.Crear(clienteDto) as ObjectResult;
        var response = result.Value as ResponseDTO<ClienteDTO>;

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.False(response.status);
        Assert.Equal("No se pudo crear el cliente", response.msg);
    }

    [Fact]
    public async Task Crear_Excepcion_DeberiaDevolverError500()
    {
        // Arrange
        var clienteDto = new ClienteDTO { PersonaId = 2, Clave = "ooo", Estado = true };

        _mapperMock.Setup(m => m.Map<CLIENTE>(clienteDto)).Throws(new System.Exception("Error inesperado"));

        // Act
        var result = await _clienteController.Crear(clienteDto) as ObjectResult;
        var response = result.Value as ResponseDTO<ClienteDTO>;

        // Assert
        Assert.Equal(500, result.StatusCode);
        Assert.False(response.status);
        Assert.Equal("Error inesperado", response.msg);
    }
}
