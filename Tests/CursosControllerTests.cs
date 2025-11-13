using Xunit;
using MinhaApiOracle.Controllers;
using MinhaApiOracle.Data;
using MinhaApiOracle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MinhaApiOracle.Tests.Controllers
{
    public class CursosControllerTests
    {
        private AppDb GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDb>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDb(options);
        }

        [Fact]
        public void GetAll_ReturnsOkResult()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new CursosController(context);

            // Act
            var result = controller.GetAll().Result;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Create_ReturnsCreatedResult()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new CursosController(context);
            var curso = new Curso
            {
                NomeCurso = "Curso de Teste",
                Descricao = "Descrição do curso",
                QtHoras = 40
            };

            // Act
            var result = controller.Create(curso).Result;

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}

