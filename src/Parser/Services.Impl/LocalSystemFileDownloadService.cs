using System.IO;
using HtmlAnalyzer.Parser.Exceptions;
using Microsoft.Extensions.Logging;
using System;

namespace HtmlAnalyzer.Parser.Services.Impl
{
	public class LocalSystemFileDownloadService : IFileFetchService
	{
		private readonly ILogger<IFileFetchService> logger;

		public LocalSystemFileDownloadService(ILogger<IFileFetchService> logger) =>
			(this.logger) = (logger);

		public StreamReader Fetch(string filename)
		{
			logger.LogInformation($"Fetching file '{filename}' from the file system...");

			if (!File.Exists(filename))
				throw new ParserException($"File not found: {filename}");

			StreamReader reader;
			try
			{
				reader = File.OpenText(filename);
			}
			catch (Exception e)
			{
				throw new ParserException($"Failed to open the file {filename}", e);
			}

			return reader;
		}
	}
}
