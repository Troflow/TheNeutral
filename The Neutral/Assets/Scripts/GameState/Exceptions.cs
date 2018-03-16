namespace Neutral
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions
    /// Class containing all Exceptions within Neutral game. Each exception needs four constructors
    /// From docs.microsoft.com:
    /// "The derived classes should define at least four constructors:
    /// one default constructor, one that sets the message property,
    /// and one that sets both the Message and InnerException properties.
    /// The fourth constructor is used to serialize the exception.
    /// New exception classes should be serializable"
    /// </summary>
    [System.Serializable]
    public class InvalidSplineBoxCountException : System.Exception
    {
        public InvalidSplineBoxCountException() : base() { }
        public InvalidSplineBoxCountException(string message) : base(message) { }
        public InvalidSplineBoxCountException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected InvalidSplineBoxCountException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}