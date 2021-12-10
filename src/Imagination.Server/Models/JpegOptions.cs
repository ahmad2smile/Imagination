namespace Imagination.Models;

public class JpegOptions
{
    public JpegOptions(uint quality)
    {
        Quality = quality is >= 0 and <= 100 ? quality : throw new InvalidJpegQualityException();
        Format = ImageFormat.Jpeg;
    }

    public ImageFormat Format { get; }
    public uint Quality { get; }
}