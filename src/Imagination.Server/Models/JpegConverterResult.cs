using System.IO;
using OneOf;

namespace Imagination.Models;

public class JpegConverterResult: OneOfBase<Stream, InvalidImageError, ImageConversionError>
{
    private JpegConverterResult(OneOf<Stream, InvalidImageError, ImageConversionError> input) : base(input)
    {
    }

    public static implicit operator JpegConverterResult(Stream _) => new(_);
    public static implicit operator JpegConverterResult(InvalidImageError _) => new(_);
    public static implicit operator JpegConverterResult(ImageConversionError _) => new(_);
}