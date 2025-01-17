using AutoMapper;
using BL.Builder;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Moq;


namespace StudyMaterialOrganiser.Test.UnitTests
{

    public class UserSearchQueryBuilderTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly List<User> _users;

        public UserSearchQueryBuilderTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            
            _users = new List<User>
        {
            new User { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new User { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" },
            new User { FirstName = "Alice", LastName = "Johnson", Email = "alice.j@example.com" }
        };

            
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.GetAll(null, null)).Returns(_users);
            _mockUnitOfWork.Setup(u => u.User).Returns(mockUserRepository.Object);
        }

        [Fact]
        public void FilterByName_ShouldFilterUsersByName()
        {
            
            var queryBuilder = new UserSearchQueryBuilder(_mockUnitOfWork.Object, _mockMapper.Object);

            
            var result = queryBuilder.FilterByName("Jane").Build();

            Assert.Single(result);
            Assert.Contains(result, u => u.FirstName == "Jane" );
        }

        [Fact]
        public void FilterByEmail_ShouldFilterUsersByEmail()
        {
            
            var queryBuilder = new UserSearchQueryBuilder(_mockUnitOfWork.Object, _mockMapper.Object);

            
            var result = queryBuilder.FilterByEmail("jane.smith@example.com").Build();

            Assert.Single(result);
            Assert.Contains(result, u => u.Email == "jane.smith@example.com");
        }

        [Fact]
        public void BuildDto_ShouldMapFilteredUsersToDto()
        {
            
            var queryBuilder = new UserSearchQueryBuilder(_mockUnitOfWork.Object, _mockMapper.Object);

            _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                .Returns((IEnumerable<User> users) => users.Select(u => new UserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }));

            var result = queryBuilder.FilterByName("Jane").BuildDto();

            Assert.Single(result);
            var userDto = result.First();
            Assert.Equal("Jane", userDto.FirstName);
            Assert.Equal("Smith", userDto.LastName);
            Assert.Equal("jane.smith@example.com", userDto.Email);
        }

        [Fact]
        public void Build_ShouldReturnAllUsersIfNoFiltersApplied()
        {
            
            var queryBuilder = new UserSearchQueryBuilder(_mockUnitOfWork.Object, _mockMapper.Object);

            
            var result = queryBuilder.Build();

            
            Assert.Equal(_users.Count, result.Count());
        }
    }

}
