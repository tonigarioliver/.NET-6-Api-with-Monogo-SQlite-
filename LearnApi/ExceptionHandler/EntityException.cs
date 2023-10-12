namespace LearnApi.ExceptionHandler
{
    public class EntityException : Exception
    {
        public EntityException()
        {
        }

        public EntityException(string message)
            : base(message)
        {
        }

        public EntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class DataNotFoundException : EntityException
    {
        public DataNotFoundException()
        {
        }

        public DataNotFoundException(string message)
            : base(message)
        {
        }

        public DataNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class ValidationException : EntityException
    {
        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }


}
