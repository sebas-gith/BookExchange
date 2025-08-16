public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public string Edition { get; set; }
    public string Description { get; set; }
    public BookCondition Condition { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public int OwnerId { get; set; }
    public Student Owner { get; set; }
}
