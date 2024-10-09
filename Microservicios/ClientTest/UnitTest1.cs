using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using MicroservicioClientePersona.Controllers; 
using Microservicio.Shared;
using MicroservicioClientePersona.Models; 
using MicroservicioClientePersona.RepositoriesClientPerson.IRepositoryClientPerson;

namespace ClientTest
{
    [TestClass]
    public class ClienteControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IClienteRepository> _mockClienteRepository;
        private readonly ClienteController _controller;

        public ClienteControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockClienteRepository = new Mock<IClienteRepository>();
            _controller = new ClienteController(_mockMapper.Object, _mockClienteRepository.Object);
        }

        [TestMethod]
        public async Task Crear_Cliente_Correcto()
        {
            // Arrange
            var clienteDTO = new ClienteDTO
            {
                PersonaId = 2, 
                Clave = "8787878787",
                Estado = true
            };

            var cliente = new CLIENTE
            {
                PersonaId = clienteDTO.PersonaId,
                Clave = clienteDTO.Clave,
                Estado = clienteDTO.Estado
            };

            _mockMapper.Setup(m => m.Map<CLIENTE>(It.IsAny<ClienteDTO>())).Returns(cliente);
            _mockClienteRepository.Setup(m => m.Crear(It.IsAny<CLIENTE>())).ReturnsAsync(cliente);

            // Act
            var result = await _controller.Crear(clienteDTO);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode);

            var responseDTO = objectResult.Value as ResponseDTO<ClienteDTO>;
            Assert.IsNotNull(responseDTO);
            Assert.IsTrue(responseDTO.status); 
            Assert.AreEqual("Cliente creado exitosamente", responseDTO.msg);
            Assert.IsNotNull(responseDTO.status); 
            Assert.AreEqual(clienteDTO.PersonaId, responseDTO.value.PersonaId); 
        }




        [TestMethod]
        public async Task Crear_Cliente_Fallido()
        {
            // Arrange
            var clienteDTO = new ClienteDTO
            {
                PersonaId = 99, 
                Clave = "11111111",
                Estado = true
            };
            _mockMapper.Setup(m => m.Map<CLIENTE>(It.IsAny<ClienteDTO>())).Returns(new CLIENTE());
            _mockClienteRepository.Setup(m => m.Crear(It.IsAny<CLIENTE>())).ThrowsAsync(new Exception("Cliente no válido"));

            // Act
            var result = await _controller.Crear(clienteDTO);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

            var responseDTO = objectResult.Value as ResponseDTO<ClienteDTO>;
            Assert.IsNotNull(responseDTO);
            Assert.IsFalse(responseDTO.status);
            Assert.AreEqual("Cliente no válido", responseDTO.msg); 
        }
    }
}
