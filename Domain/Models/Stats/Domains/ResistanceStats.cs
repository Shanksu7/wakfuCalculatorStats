using static System.Formats.Asn1.AsnWriter;

namespace Domain.Models.Stats.Domains
{
    public class ResistanceStats(double water, double eart, double air, double fire) : DomainBase(water, eart, air, fire)
    {
        public double WaterPercentage => GetPercentage(Water);
        public double EarthPercentage => GetPercentage(Earth);
        public double AirPercentage => GetPercentage(Air);
        public double FirePercentage => GetPercentage(Fire);

        double GetPercentage(double score)
        {
            return (1 - Math.Pow(0.8, score / 100.0))*100;
        }
    }
}