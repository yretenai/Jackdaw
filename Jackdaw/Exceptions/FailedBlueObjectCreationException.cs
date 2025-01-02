using System;

namespace Jackdaw.Exceptions;

public sealed class FailedBlueObjectCreationException : Exception {
	public FailedBlueObjectCreationException() { }
	public FailedBlueObjectCreationException(string message, Exception innerException) : base(message, innerException) { }
	public FailedBlueObjectCreationException(string type) : base($"Failed to create object of type {type}") => Type = type;

	public string Type { get; set; } = "IRoot";
}
