using AutoMapper;
using BL.AutoMaperProfiles;
using BL.IServices;
using BL.Services;
using BL.Utilities;
using DAL.IRepositories;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // Add Repositories
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Services (real implementations)
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IMaterialTagService, MaterialTagService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMaterialAccessService, MaterialAccessService>();
            services.AddScoped<ITagService, TagService>();

            // Add BinaryFileHandler
            services.AddScoped<BaseFileHandler, BinaryFileHandler>();

            // Add AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());

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
