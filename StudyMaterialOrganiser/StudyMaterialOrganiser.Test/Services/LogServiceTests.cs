using System.Linq.Expressions;
using Moq;
using Xunit;
using DAL.IRepositories;
using BL.Services;
using DAL.Models;
using BL.Models;
using Microsoft.Extensions.Configuration;
using AutoMapper;

public class LogServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly LogService _logService;

    public LogServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockConfiguration = new Mock<IConfiguration>();

        _logService = new LogService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object);
    }

    [Fact]
    public void GetAll_ShouldReturnLogs()
    {
        
        var logs = new List<Log>
        {
            new Log { Id = 1, Level = "Info", Message = "Log 1" },
            new Log { Id = 2, Level = "Error", Message = "Log 2" }
        };

        _mockUnitOfWork.Setup(u => u.Log.GetAll(null, It.Is<string>(s => s == null)))
            .Returns(logs);

        
        var result = _logService.GetAll();

        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, log => log.Level == "Info" && log.Message == "Log 1");
    }

    [Fact]
    public void Log_ShouldAddLogAndSave()
    {
        
        var log = new Log { Level = "Info", Message = "Test Log" };

        
        _logService.Log("Info", "Test Log");

        
        _mockUnitOfWork.Verify(u => u.Log.Add(It.Is<Log>(l => l.Level == log.Level && l.Message == log.Message)), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }
}
