namespace DataAccess.Model
{
    /// <summary>
    ///     Base class for the DB entity.
    /// </summary>
    public class AbstractEntity
    {
        /// <summary>
        ///     ID of the entity.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global - db entity, set must be present
        public int Id { get; set; }
    }
}