using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Imagination.Models;
using Imagination.Providers;
using NSubstitute;
using Xunit;

namespace Server.Test.Unit.Services;

public class ImageProviderMock: ImageProvider
{
    public ImageProviderMock(IMagickImage magickImage) : base(magickImage)
    {
    }

    public new Task ConvertFormat(Stream imageStream, Stream outStream, ImageFormat format, uint quality,
        CancellationToken cancellationToken)
    {
        return base.ConvertFormat(imageStream, outStream, format, quality, cancellationToken);
    }


}
public class ImageProviderTest
{
    private readonly ImageProviderMock _sut;
    private readonly IMagickImage _magicImage;

    public ImageProviderTest()
    {
        _magicImage = Substitute.For<IMagickImage>();

        _sut = new ImageProviderMock(_magicImage);
    }

    [Fact]
    public async Task ConvertForm_WritesTo_OutStream()
    {
        const string content = "mock stream content";
        var contentBytes = Encoding.UTF8.GetBytes(content);
        var imageStream = new MemoryStream(contentBytes);
        var outStream = new MemoryStream();

        _magicImage.ReadAsync(Arg.Any<Stream>()).Returns(Task.CompletedTask);
        _magicImage.WriteAsync(Arg.Any<Stream>(), CancellationToken.None).Returns(Task.CompletedTask)
            .AndDoes(x => outStream.Write(contentBytes));

        await _sut.ConvertFormat(imageStream, outStream, ImageFormat.Jpeg, 75, CancellationToken.None);
            
        Assert.Equal(content, Encoding.UTF8.GetString(outStream.ToArray()));
        ;        }
}