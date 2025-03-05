namespace Domain.Models.WAKFUAPI
{
    public class JobItemDefinition
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Rarity { get; set; }
        public int ItemTypeId { get; set; }
        public GraphicParameters GraphicParameters { get; set; }
    }

    public class JobItems
    {
        public ItemTypeDefinition Definition { get; set; }
        public Lang Title { get; set; }
        public Lang Description { get; set; }
    }

}
