using System;

public class InvalidPublicKeyException : Exception
{
    public InvalidPublicKeyException(string msg) : base(msg) { }
}