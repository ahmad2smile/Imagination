using Imagination.Models;
using Xunit;

namespace Server.Test.Unit.Models;

public class JpegOptionsTest
{
    [Fact]
    public void Assigns_Valid_Quality()
    {
        const uint expectedQuality = 75;
        var opts = new JpegOptions(expectedQuality);

        Assert.Equal(expectedQuality, opts.Quality);
    }

    [Theory]
    [InlineData(101)]
    [InlineData(uint.MaxValue)]
    public void Throws_Exception_On_OutOfRange(uint quality)
    {
        Assert.Throws<InvalidJpegQualityException>(() => new JpegOptions(quality));
    }
}