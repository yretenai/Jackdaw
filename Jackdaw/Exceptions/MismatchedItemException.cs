using System;

namespace Jackdaw.Exceptions;

public sealed class MismatchedItemException : Exception {
	public MismatchedItemException(string message) : base(message) { }
	public MismatchedItemException() { }
	public MismatchedItemException(string message, Exception innerException) : base(message, innerException) { }
}
