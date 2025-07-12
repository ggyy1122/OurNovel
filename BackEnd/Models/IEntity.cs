namespace OurNovel.Models
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
