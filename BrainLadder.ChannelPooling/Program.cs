using BrainLadder.ChannelPooling.Services;
using RabbitMq.Extensions;
using RabbitMq.Models;

namespace BrainLadder.ChannelPooling
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            
            builder.Services.AddOptions();
            builder.Services.Configure<RabbitOptions>(builder.Configuration.GetSection("RabbitOptions"));
            
            builder.Services.AddRabbitMq();

            builder.Services.AddHostedService<MessagePublihserHostedService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}