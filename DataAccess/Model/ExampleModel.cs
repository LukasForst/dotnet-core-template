namespace DataAccess.Model
{
    /// <summary>
    ///     Example model entity, will be deleted in the future. TODO - delete in the future.
    /// </summary>
    public class ExampleModel : AbstractEntity
    {
        public ExampleModel(string something)
        {
            Something = something;
        }

        public string Something { get; set; }
    }
}