using System;

namespace Jackdaw.Exceptions;

public sealed class UnknownStaticDataTypeException : Exception {
	public UnknownStaticDataTypeException(string message) : base(message) { }
	public UnknownStaticDataTypeException() { }
	public UnknownStaticDataTypeException(string message, Exception innerException) : base(message, innerException) { }

	public UnknownStaticDataTypeException(ulong high, ulong low) : base($"Unknown static data type: {high:X16}{low:X16}") {
		High = high;
		Low = low;
	}

	public ulong High { get; set; }
	public ulong Low { get; set; }
}
