using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Imagination.Models;
using Imagination.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Server.Test.Unit.Services;

public class JpegServiceTest
{
    private readonly IJpegService _sut;
    private readonly IMagickImage _magicImage;

    public JpegServiceTest()
    {
        _magicImage = Substitute.For<IMagickImage>();
        ILogger logger = new Logger<JpegService>(new LoggerFactory());

        _sut = Substitute.For<JpegService>(_magicImage, logger);
    }

    [Fact]
    public async Task ConvertFormat_ReturnsStream()
    {
        var imageStream = new MemoryStream();

        _magicImage.ReadAsync(Arg.Any<Stream>()).Returns(Task.CompletedTask);

        var result = await _sut.ConvertFormat(imageStream, CancellationToken.None);

        Assert.IsAssignableFrom<Stream>(result.AsT0);
    }

    [Fact]
    public async Task ConvertFormat_Returns_InvalidImageError_On_MagickCoderErrorException()
    {
        var imageStream = new MemoryStream();

        _magicImage.ReadAsync(Arg.Any<Stream>(), CancellationToken.None).Returns(Task.FromException(new MagickCoderErrorException("")));

        var result = await _sut.ConvertFormat(imageStream, CancellationToken.None);

        Assert.Equal(new InvalidImageError(), result.AsT1);
    }
           
    [Fact]
    public async Task ConvertFormat_Returns_ImageConversionError_On_Exception()
    {
        var imageStream = new MemoryStream();

        _magicImage.ReadAsync(Arg.Any<Stream>(), CancellationToken.None).Returns(Task.FromException(new Exception("")));

        var result = await _sut.ConvertFormat(imageStream, CancellationToken.None);

        Assert.Equal(new ImageConversionError(), result.AsT2);
    }
}