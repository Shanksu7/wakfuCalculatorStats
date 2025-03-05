namespace Domain.Models.WAKFUAPI.Parsed
{
    public class EquipEffectParsed
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public double Value { get; set; }
        public override string ToString()
        {
            return $"[{ActionId}] {ActionName} : {Value}";
        }
    }
}
