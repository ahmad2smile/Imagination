using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Imagination.Controllers;
using Imagination.Models;
using Imagination.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Server.Test.Unit.Controllers;

public class ImageControllerTest
{
    private readonly ImageController _sut;
    private readonly IJpegService _jpegService;

    public ImageControllerTest()
    {
        _jpegService = Substitute.For<IJpegService>();
        _sut = new ImageController(_jpegService)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    Request = { Body = new MemoryStream() }
                }
            }
        };
    }

    [Fact]
    public async Task Endpoint_ConvertPost_ReturnsStreamResult()
    {
        var returnStream = Substitute.For<MemoryStream>();
        _jpegService.ConvertFormat(Arg.Any<Stream>(), CancellationToken.None).Returns(returnStream);

        var result = await _sut.Index(CancellationToken.None);

        Assert.IsAssignableFrom<FileStreamResult>(result);
    }

        
    [Fact]
    public async Task Endpoint_ConvertPost_Returns_BadRequest()
    {
        _jpegService.ConvertFormat(Arg.Any<Stream>(), CancellationToken.None).Returns(new InvalidImageError());

        var result = await _sut.Index(CancellationToken.None);

        Assert.IsAssignableFrom<BadRequestObjectResult>(result);
    }

                
    [Fact]
    public async Task Endpoint_ConvertPost_Returns_Problem()
    {
        _jpegService.ConvertFormat(Arg.Any<Stream>(), CancellationToken.None).Returns(new ImageConversionError());

        var result = await _sut.Index(CancellationToken.None);

        Assert.IsAssignableFrom<ObjectResult>(result);
    }
}