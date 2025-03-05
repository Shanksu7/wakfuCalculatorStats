namespace Domain.Models.WAKFUAPI
{
    public class ResourcesDefinition
    {
        public int Id { get; set; }
        public int ResourceType { get; set; }
        public bool IsBlocking { get; set; }
        public int IdealRainRangeMin { get; set; }
        public int IdealRainRangeMax { get; set; }
        public int IdealTemperatureRangeMin { get; set; }
        public int IdealTemperatureRangeMax { get; set; }
        public int IconGfxId { get; set; }
        public int LastEvolutionStep { get; set; }
        public bool UsableByHeroes { get; set; }
        public int IdealRain { get; set; }
    }

    public class Resources
    {
        public Definition Definition { get; set; }
        public Lang Title { get; set; }
    }

}
