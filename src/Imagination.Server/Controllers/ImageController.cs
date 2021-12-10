using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Imagination.Services;
using Microsoft.AspNetCore.Mvc;

namespace Imagination.Controllers;

[ApiController]
[Route("convert")]
public class ImageController: ControllerBase
{
    private readonly IJpegService _jpegService;

    public ImageController(IJpegService jpegService)
    {
        _jpegService = jpegService;
    }

    [HttpPost]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        await using var fileStream = new MemoryStream();

        await Request.Body.CopyToAsync(fileStream, cancellationToken);

        fileStream.Seek(0, SeekOrigin.Begin);

        var result = await _jpegService.ConvertFormat(fileStream, cancellationToken);

        return result.Match<IActionResult>(stream =>
        {
            Response.RegisterForDisposeAsync(stream);

            return File(stream, MediaTypeNames.Image.Jpeg);
        }, BadRequest, error => Problem(error.Message));
    }
}