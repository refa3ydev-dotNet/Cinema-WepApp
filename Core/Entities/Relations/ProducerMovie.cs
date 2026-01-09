namespace Core.Entities.Relations
{
    public class ProducerMovie
    {
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
