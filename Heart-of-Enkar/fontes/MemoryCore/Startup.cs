using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MemoryCore.Models;
using MemoryCore.Services;
using System;
using MemoryCore.Services.Agents;

namespace MemoryCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Console.WriteLine("1");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure Database Settings
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            // Register encryption service
            services.AddSingleton<EncryptionService>(sp => 
                new EncryptionService(Configuration["EncryptionKey"] ?? "YourDefaultKey32CharsLongForAES256!"));
            
            // Register core services
            services.AddSingleton<EmbeddingService>();
            services.AddSingleton<ZeyraService>();
            services.AddSingleton<TermService>();
            services.AddSingleton<RalkService>();
            services.AddSingleton<MessageBusService>();
            
            // Register agent services
            services.AddSingleton<RalkGuardianAgent>();
            
            services.AddControllers();
            
            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MemoryCore API", Version = "v1" });
            });
            
            // In the ConfigureServices method, add:
            services.AddSingleton<SensitiveWordDetector>();
        }

        // Configure method remains unchanged
    }
}