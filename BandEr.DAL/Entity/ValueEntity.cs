namespace BandEr.DAL.Entity
{
    public class ValueEntity : AppEntity
    {
        public string Value { get; set; }
        public int OwnerId { get; set; }
        public AppUser Owner { get; set; }
    }
}
