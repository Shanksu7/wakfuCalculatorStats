namespace Domain.Models.WAKFUAPI
{
    public class ItemTypeDefinition
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public List<string> EquipmentPositions { get; set; }
        public List<string> EquipmentDisabledPositions { get; set; }
        public bool IsRecyclable { get; set; }
        public bool IsVisibleInAnimation { get; set; }
    }

    public class ItemTypes
    {
        public ItemTypeDefinition Definition { get; set; }
        public Lang Title { get; set; }
        public override string ToString()
        {
            return Definition.Id + "_" + Title?.Es;
        }
    }

}
