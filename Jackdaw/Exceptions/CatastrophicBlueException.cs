using System;

namespace Jackdaw.Exceptions;

public sealed class CatastrophicBlueException : Exception {
	public CatastrophicBlueException(string message) : base(message) { }
	public CatastrophicBlueException() { }
	public CatastrophicBlueException(string message, Exception innerException) : base(message, innerException) { }
}
