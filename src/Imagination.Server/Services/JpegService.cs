using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Imagination.Models;
using Imagination.Providers;
using Microsoft.Extensions.Logging;

namespace Imagination.Services;

public class JpegService: ImageProvider, IJpegService
{
    private readonly ILogger<JpegService> _logger;

    public JpegService(IMagickImage magickImage, ILogger<JpegService> logger) : base(magickImage)
    {
        _logger = logger;
    }

    public Task<JpegConverterResult> ConvertFormat(Stream imageStream, CancellationToken cancellationToken)
    {
        // NOTE: Setting max Quality as to not loose any details
        return ConvertFormat(imageStream, new JpegOptions( 100), cancellationToken);
    }

    // NOTE: Overload for if/when service needs to expose Quality + other options
    public async Task<JpegConverterResult> ConvertFormat(Stream imageStream, JpegOptions options, CancellationToken cancellationToken)
    {
        try
        {
            var stream = new MemoryStream();

            await base.ConvertFormat(imageStream, stream, options.Format, options.Quality, cancellationToken);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
        catch (MagickCoderErrorException e)
        {
            _logger.LogError("Invalid image exception: {ExceptionMessage}", e.Message);
            return new InvalidImageError();
        }
        catch (Exception e)
        {
            _logger.LogError("Unknown exception while converting image: {ExceptionMessage}", e.Message);
            return new ImageConversionError();
        }
    }
}