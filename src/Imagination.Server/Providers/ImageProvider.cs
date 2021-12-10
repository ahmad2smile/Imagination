using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Imagination.Models;

namespace Imagination.Providers;

public abstract class ImageProvider
{
    private readonly IMagickImage _magickImage;

    protected ImageProvider(IMagickImage magickImage)
    {
        _magickImage = magickImage;
    }

    protected async Task ConvertFormat(Stream imageStream, Stream outStream, ImageFormat format, uint quality,
        CancellationToken cancellationToken)
    {
        _magickImage.Quality = (int)quality;
        await _magickImage.ReadAsync(imageStream, cancellationToken);

        _magickImage.Format = (MagickFormat)format;

        await _magickImage.WriteAsync(outStream, cancellationToken);
    }
}