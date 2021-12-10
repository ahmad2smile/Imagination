using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Imagination.Models;

namespace Imagination.Services;

public interface IJpegService
{
    Task<JpegConverterResult> ConvertFormat(Stream imageStream, CancellationToken cancellationToken);
    Task<JpegConverterResult> ConvertFormat(Stream imageStream, JpegOptions options, CancellationToken cancellationToken);
}