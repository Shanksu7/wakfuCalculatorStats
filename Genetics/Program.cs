using System;
using System.Collections.Generic;
using System.Linq;

// Equipment and Character Definition
public class Equipment
{
    public string Type { get; set; } // Helmet, Cape, Amulet, etc.
    public Dictionary<string, int> Stats { get; set; } // E.g., {"AP": 1, "MP": 2}
    public bool IsTwoHandedWeapon { get; set; } = false;

    public override string ToString() => $"{Type} | Stats: {string.Join(", ", Stats.Select(s => s.Key + ": " + s.Value))} | {(IsTwoHandedWeapon ? "Two-Handed Weapon" : "")}";
}

public static class Inventory
{
    public static List<Equipment> Items = GenerateRandomInventory(100); // Generates 100 random items

    public static List<Equipment> GenerateRandomInventory(int itemCount)
    {
        var types = new[] { "Helmet", "Cape", "Amulet", "Belt", "Ring", "Emblem", "Shoulders", "One-Handed Weapon", "Two-Handed Weapon", "Off-Hand Weapon" };
        var stats = new[] { "AP", "MP", "Range", "Mastery", "Fire Damage", "Air Damage" };
        var random = new Random();

        var inventory = new List<Equipment>();
        for (int i = 0; i < itemCount; i++)
        {
            var itemType = types[random.Next(types.Length)];
            var itemStats = new Dictionary<string, int>();

            foreach (var stat in stats)
            {
                if (random.NextDouble() < 0.5) // 50% chance to include each stat
                {
                    itemStats[stat] = random.Next(1, 5); // Random stat value between 1-4
                }
            }

            inventory.Add(new Equipment
            {
                Type = itemType,
                Stats = itemStats,
                IsTwoHandedWeapon = itemType == "Two-Handed Weapon"
            });
        }
        return inventory;
    }
}

public class Character
{
    public string Name { get; set; }
    public Dictionary<string, double> StatPriorities { get; set; } // E.g., {"AP": 2.0, "MP": 1.5}
}

public class Requirements
{
    public Dictionary<string, int> RequiredStats { get; set; } = new Dictionary<string, int>();
}

// Equipment Generator (for mutations)
public static class EquipmentGenerator
{
    static Random rand = new Random();
    public static Equipment CreateRandomEquipment()
    {
        return Inventory.Items[rand.Next(Inventory.Items.Count)];
    }
}

// Fitness Evaluation
public static class Evaluation
{
    public static double CalculateFitness(List<Equipment> equipment, Character character, Requirements requirements)
    {
        double score = 0.0;
        var totalStats = new Dictionary<string, int>();
        bool hasTwoHandedWeapon = false;
        int oneHandedWeapons = 0;

        foreach (var item in equipment)
        {
            //Console.WriteLine($"Item Stats: {item}");

            if (item.IsTwoHandedWeapon)
            {
                if (hasTwoHandedWeapon || oneHandedWeapons > 0) return 0; // Penalty for weapon conflict
                hasTwoHandedWeapon = true;
            }
            else if (item.Type.Contains("One-Handed Weapon"))
            {
                oneHandedWeapons++;
                if (hasTwoHandedWeapon || oneHandedWeapons > 2) return 0; // Penalty for exceeding two one-handed weapons
            }

            if (equipment.Count(e => e.Type == item.Type) > 1)
                return 0; // Penalty for duplicate equipment types

            foreach (var stat in item.Stats)
            {
                if (!totalStats.ContainsKey(stat.Key))
                    totalStats[stat.Key] = 0;

                totalStats[stat.Key] += stat.Value;

                if (character.StatPriorities.TryGetValue(stat.Key, out double weight))
                {
                    score += stat.Value * weight;
                }
            }
        }

        // Penalty for unmet requirements
        foreach (var req in requirements.RequiredStats)
        {
            if (totalStats.TryGetValue(req.Key, out int currentValue))
            {
                if (currentValue < req.Value) return 0; // Reject if requirements are not met
            }
            else
            {
                return 0; // Reject if stat is missing
            }
        }

        //Console.WriteLine($"Stats in current individual: {string.Join(", ", totalStats.Select(s => s.Key + ": " + s.Value))}");

        return score;
    }

    public static void DisplayTotalStats(List<Equipment> equipment)
    {
        var totalStats = new Dictionary<string, int>();
        foreach (var item in equipment)
        {
            foreach (var stat in item.Stats)
            {
                if (!totalStats.ContainsKey(stat.Key))
                    totalStats[stat.Key] = 0;
                totalStats[stat.Key] += stat.Value;
            }
        }
        Console.Write("\nTotal Stats:");
        foreach (var stat in totalStats)
        {
            Console.Write($"{stat.Key}: {stat.Value} ");
        }
    }
}

// Genetic Algorithm - Selection, Crossover, and Mutation
public static class GeneticAlgorithm
{
    static Random rand = new Random();

    public static List<Equipment> Run(Character character, Requirements requirements, int generations = 5)
    {
        var population = new List<List<Equipment>>();
        for (int i = 0; i < 50; i++)
        {
            var individual = new List<Equipment>();
            while (individual.Count < 10)
            {
                var newItem = EquipmentGenerator.CreateRandomEquipment();
                if (!individual.Any(e => e.Type == newItem.Type))
                    individual.Add(newItem);
            }
            population.Add(individual);
            Evaluation.DisplayTotalStats(individual); // Display total stats for each population
        }

        return population.OrderByDescending(x => Evaluation.CalculateFitness(x, character, requirements)).First();
    }
}

// Entry Point - Main()
class Program
{
    static void Main(string[] args)
    {
        var character = new Character
        {
            Name = "Warrior",
            StatPriorities = new Dictionary<string, double> { { "AP", 2.0 }, { "MP", 1.5 } }
        };

        var requirements = new Requirements
        {
            RequiredStats = new Dictionary<string, int> { { "AP", 18 }, { "MP", 5 }, { "Range", 3 }, { "Mastery", 2000 } }
        };

        var bestEquipment = GeneticAlgorithm.Run(character, requirements);
        Console.WriteLine("Best equipment found:");
        bestEquipment.ForEach(e => Console.WriteLine(e));
        Evaluation.DisplayTotalStats(bestEquipment);
    }
}
