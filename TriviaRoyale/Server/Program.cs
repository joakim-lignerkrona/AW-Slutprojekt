using Microsoft.AspNetCore.ResponseCompression;
using TriviaRoyale.Client.Models;
using TriviaRoyale.Server.Hubs;
using TriviaRoyale.Server.Models;

namespace TriviaRoyale
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			builder.Services.AddTransient<QRService>();

			builder.Services.AddTransient<QuestionService>();
			builder.Services.AddSingleton<RoomService>();



			builder.Services.AddSignalR();
			builder.Services.AddResponseCompression(opts =>
			{
				opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
					new[] { "application/octet-stream" });
			});

			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			app.UseResponseCompression();

			// Configure the HTTP request pipeline.
			if(app.Environment.IsDevelopment())
			{
				app.UseWebAssemblyDebugging();

			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}


			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/v1/swagger.json", "v1");
				options.RoutePrefix = "/swagger";
			});


			app.UseHttpsRedirection();

			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseCookiePolicy();

			app.MapRazorPages();
			app.MapHub<QuizHub>(pattern: "/Quiz");
			app.MapControllers();
			app.MapFallbackToFile("index.html");

			app.Run();
		}
	}
}