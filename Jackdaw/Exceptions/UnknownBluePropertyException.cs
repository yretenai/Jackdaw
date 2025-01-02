using System;

namespace Jackdaw.Exceptions;

public sealed class UnknownBluePropertyException : Exception {
	public UnknownBluePropertyException(string message) : base(message) { }
	public UnknownBluePropertyException() { }
	public UnknownBluePropertyException(string message, Exception innerException) : base(message, innerException) { }

	public UnknownBluePropertyException(string property, string type) : base($"Unknown property: {property} for type {type}") {
		Property = property;
		Type = type;
	}

	public string Property { get; set; } = "Unknown";
	public string Type { get; set; } = "IRoot";
}
