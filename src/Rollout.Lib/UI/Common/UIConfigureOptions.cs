using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace Rollout.Lib.UI.Common;

public class UiConfigureOptions : IPostConfigureOptions<StaticFileOptions>
{
    private readonly IWebHostEnvironment _environment;

    public UiConfigureOptions(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public void PostConfigure(string name, StaticFileOptions options)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        // Basic initialization in case the options weren't initialized by any other component
        options.ContentTypeProvider ??= new FileExtensionContentTypeProvider();
        if (options.FileProvider == null && _environment.WebRootFileProvider == null)
        {
            throw new InvalidOperationException("Missing FileProvider.");
        }

        options.FileProvider ??= _environment.WebRootFileProvider;

        const string basePath = "wwwroot";

        var filesProvider = new ManifestEmbeddedFileProvider(GetType().Assembly, basePath);
        options.FileProvider = new CompositeFileProvider(options.FileProvider, filesProvider);
    }
}
