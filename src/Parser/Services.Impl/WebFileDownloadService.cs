using System;
using System.IO;
using System.Net;
using HtmlAnalyzer.Parser.Exceptions;
using Microsoft.Extensions.Logging;

namespace HtmlAnalyzer.Parser.Services.Impl
{
	public class WebFileDownloadService : IFileFetchService
	{
		private readonly ILogger<IFileFetchService> logger;

		public WebFileDownloadService(ILogger<IFileFetchService> logger) =>
			(this.logger) = (logger);

		public StreamReader Fetch(string filename)
		{
			logger.LogInformation("Downloading the page...");

			var localUri = new UriBuilder(filename).Uri;
			using var client = new WebClient();

			Stream stream;
			try
			{
				stream = client.OpenRead(localUri);
			}
			catch (WebException e)
			{
				throw new ParserException($"Failed to download the file: {e.Message}", e);
			}

			return new StreamReader(stream);
		}
	}

}
