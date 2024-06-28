using Azure.Storage.Blobs;
using BandLabTestAssignment;
using BandLabTestAssignment.Data;
using BandLabTestAssignment.Managers;
using BandLabTestAssignment.Managers.Interfaces;
using BandLabTestAssignment.Repository;
using BandLabTestAssignment.Repository.Interfaces;
using BandLabTestAssignment.Services;
using BandLabTestAssignment.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        string connectionString = context.Configuration[Constants.DatabaseConnectionString];

        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.SetMinimumLevel(LogLevel.None))));

        services.AddScoped(x => new BlobServiceClient(context.Configuration[Constants.StorageConnectionString]));
        services.AddScoped<IBlobService, BlobService>();

        services.AddScoped<IBlobService, BlobService>();
        services.AddScoped<IImageService, ImageService>();

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<IPostManager, PostManager>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<ICommentManager, CommentManager>();
    })
    .Build();

host.Run();
