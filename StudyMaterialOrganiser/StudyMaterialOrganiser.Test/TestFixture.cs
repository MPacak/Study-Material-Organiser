using AutoMapper;
using BL.AutoMaperProfiles;
using StudyMaterialOrganiser.Mappers;
using BL.IServices;
using BL.Services;
using BL.Utilities;
using DAL.IRepositories;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyMaterialOrganiser.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialOrganiser.Test
{
    public class TestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public StudymaterialorganiserContext DbContext { get; private set; }

        public TestFixture()
        {
            var services = new ServiceCollection();

            // Load real appsettings.json configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Use current directory for appsettings.json
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Load appsettings.json
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            // Configure DbContext with InMemory Database
            services.AddDbContext<StudymaterialorganiserContext>(options =>
                options.UseInMemoryDatabase("IntegrationTestDb"));

            // repositories
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupService, GroupService>();
           services.AddScoped<IUserGroupRepository, UserGroupRepository>();
           services.AddScoped<ITagRepository, TagRepository>();
           services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IRoleApprovalStrategy, DefaultRoleApprovalStrategy>();
            services.AddScoped<IRoleApprovalStrategy, AdminRoleApprovalStrategy>();
           services.AddScoped<IMaterialTagRepository, MaterialTagRepository>();
   

            // Add Services (real implementations)
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IMaterialTagService, MaterialTagService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMaterialAccessService, MaterialAccessService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddHttpContextAccessor();
            services.AddTransient<AssignTags>();
           services.AddScoped<IMaterialFactory, MaterialFactory>();
            services.AddScoped<IMaterialAccessService, MaterialAccessService>();

            services.AddScoped<BaseFileHandler, BinaryFileHandler>();
            services.AddScoped<IBinaryFileHandler, BinaryFileHandler>();

            services.AddSingleton<IMapper>(provider =>
            {
                // BL Layer Mapper: DTO to DB Entity
                var dbMapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BL.AutoMaperProfiles.MappingProfile>();
                });

                // MVC Layer Mapper: DTO to ViewModel
                var mvcMapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<Mappers.MappingProfile>();
                });

                // Return a consolidated IMapper instance combining both configurations
                return new Mapper(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<BL.AutoMaperProfiles.MappingProfile>();
                    cfg.AddProfile<Mappers.MappingProfile>();
                }));
            });

            // Build the service provider
            ServiceProvider = services.BuildServiceProvider();

            // Resolve DbContext and ensure database is created
            DbContext = ServiceProvider.GetRequiredService<StudymaterialorganiserContext>();
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }

}
