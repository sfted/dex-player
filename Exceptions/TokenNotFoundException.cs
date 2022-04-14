namespace DexPlayer.Exceptions;

using System;

internal class TokenNotFoundException : Exception
{
    public TokenNotFoundException() { }

    public TokenNotFoundException(string message)
        : base(message) { }

    public TokenNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}
