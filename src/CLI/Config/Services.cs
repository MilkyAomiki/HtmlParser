using HtmlAnalyzer.Parser.Services;
using HtmlAnalyzer.Parser.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlAnalyzer.CLI.Config
{
	public static class Services
	{
		public static void AddParserServices(this IServiceCollection services)
		{
			services.AddScoped<LocalSystemFileDownloadService>();
			services.AddScoped<WebFileDownloadService>();
			services.AddScoped<IFileFetchService, FileFetchService>();
			services.AddScoped<IHtmlParserService, HtmlParserService>();
		}
	}
}
