namespace BookExchange.Application.DTOs.Subjects
{
    public class SubjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int TotalBooks { get; set; } // Número de libros asociados (opcional y útil)
    }
}
