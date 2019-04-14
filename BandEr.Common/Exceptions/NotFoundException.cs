namespace BandEr.Common.Exceptions
{
    public class NotFoundException: ApplicationException
    {
        public NotFoundException(string name, object key)
          : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }

    public class NotFoundException<T> : NotFoundException
    {
        public NotFoundException(object key)
            :base(nameof(T),key)
        {
        }
    }
}
