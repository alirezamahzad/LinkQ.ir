using Xunit;
using Moq;
using System.Net;
using Domain.Entities;
using Shared;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.Logging;

namespace Test
{
    public class LinkServiceTests
    {
        private readonly Mock<ICrudRepository<Link>> _mockRepository;
        private readonly Mock<ILinkRepository> _mockLinkRepository;
        private readonly Mock<IVisitService> _mockVisitService;
        private readonly Mock<ILogger<LinkService>> _mockLogger;
        private readonly ILinkService _linkService;

        public LinkServiceTests()
        {
            _mockRepository = new Mock<ICrudRepository<Link>>();
            _mockLinkRepository = new Mock<ILinkRepository>();
            _mockVisitService = new Mock<IVisitService>();
            _mockLogger = new Mock<ILogger<LinkService>>();
            _linkService = new LinkService(_mockRepository.Object, _mockLinkRepository.Object, _mockVisitService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Add_Link_AddedSuccessfully()
        {
            //Arrange
            var link = new Link() { OriginalUrl = new Url("https://alirezamahzad.ir"), ShortUrl = "TestUrl" };
            _mockRepository.Setup(r => r.Add(It.IsAny<Link>())).ReturnsAsync(new OperationResult(true, HttpStatusCode.Created));

            // Act
            var result = await _linkService.Add(link);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.Created, result.ErrorCode);
        }
    }
}
