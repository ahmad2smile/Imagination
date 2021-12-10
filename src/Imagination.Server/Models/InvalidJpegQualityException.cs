using System;

namespace Imagination.Models;

public class InvalidJpegQualityException : Exception
{
    public InvalidJpegQualityException(): base("Quality should be between 0 and 100")
    {
    }
}