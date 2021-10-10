using System;
using System.IO;
using HtmlAnalyzer.Parser.Exceptions;
using Microsoft.Extensions.Logging;

namespace HtmlAnalyzer.Parser.Services.Impl
{
	public class FileFetchService : IFileFetchService
	{
		private readonly IFileFetchService localFileService;
		private readonly IFileFetchService webFileService;
		private readonly ILogger<FileFetchService> logger;

		public FileFetchService(LocalSystemFileDownloadService localFileService, WebFileDownloadService webFileService, ILogger<FileFetchService> logger) =>
			(this.localFileService, this.webFileService, this.logger) =
			(localFileService, webFileService, logger);

		public StreamReader Fetch(string filename)
		{
			if (Uri.TryCreate(filename, UriKind.RelativeOrAbsolute, out var uri))
				return !uri.IsAbsoluteUri || uri.IsFile ? localFileService.Fetch(filename) : webFileService.Fetch(filename);

			throw new ParserException($"Given URI could not be recognized: {uri}");
		}
	}
}
