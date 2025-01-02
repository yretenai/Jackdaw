using System;

namespace Jackdaw.Exceptions;

public sealed class UnknownBlueObjectException : Exception {
	public UnknownBlueObjectException() { }
	public UnknownBlueObjectException(string message, Exception innerException) : base(message, innerException) { }
	public UnknownBlueObjectException(string type) : base($"Unknown object type: {type}") => Type = type;

	public string Type { get; set; } = "IRoot";
}
