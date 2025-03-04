using Domain.Enums;
using Domain.Models;
using Domain.Models.Stats;
using Domain.Models.Stats.Combat;
using Domain.Models.WAKFUAPI;
using Newtonsoft.Json;
using System.Diagnostics;
using WakfuItemsPlayground;
using WakfuItemsPlayground.Enums;

internal class Program
{
    private static void Main(string[] args)
    {
        var files = Directory.GetFiles(@"C:\Users\juanp\OneDrive\Documentos\github\calculator_wakfu\wakfuCalculatorStats\DownloaderVersions\bin\Debug\net8.0\WakfuJsonFiles_1.86.4.31");
        var txt = "";
        var items = new List<Item>();
        var actions = new List<Actions>();
        var itemTypes = new List<ItemTypes>();
        foreach (var file in files)
        {
            var name = file.Split('\\').Last().Replace(".json", "");
            txt += $"case \"{name}\":\n\tbreak;\n";
            switch (name)
            {
                case "actions":
                    actions = JsonConvert.DeserializeObject<List<Actions>>(File.ReadAllText(file));
                    break;
                case "blueprints":
                    break;
                case "collectibleResources":
                    break;
                case "equipmentItemTypes":
                    break;
                case "harvestLoots":
                    break;
                case "itemProperties":
                    break;
                case "items":
                    items = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(file));
                    break;
                case "itemTypes":
                    itemTypes = JsonConvert.DeserializeObject<List<ItemTypes>>(File.ReadAllText(file));
                    break;
                case "jobsItems":
                    break;
                case "recipeCategories":
                    break;
                case "recipeIngredients":
                    break;
                case "recipeResults":
                    break;
                case "recipes":
                    break;
                case "resources":
                    break;
                case "resourceTypes":
                    break;
                case "states":
                    break;
                default:
                    break;
            }
        }

        foreach (var item in items)
        {
            item.Definition.EquipEffectsParsed = new List<Domain.Models.WAKFUAPI.Parsed.EquipEffectParsed>();
            item.Definition.Item.BaseParameters.ItemTypeEnum = (ItemTypesEnum)item.Definition.Item.BaseParameters.ItemTypeId;
            foreach (var equipEffect in item.Definition.EquipEffects)
            {
                var action = actions.FirstOrDefault(x => x.Definition.Id == equipEffect.Effect.Definition.ActionId);
                if (action.Description != null)
                {
                    item.Definition.EquipEffectsParsed.Add(new Domain.Models.WAKFUAPI.Parsed.EquipEffectParsed()
                    {
                        ActionId = equipEffect.Effect.Definition.ActionId,
                        ActionName = action.Description.Spanish,
                        Value = equipEffect.Effect.Definition.Params.FirstOrDefault()
                    });
                }
            }
            item.SetStatsCollection(false, false, false);
        }

        //aditional Hidden stats
        StatsCollection stats = new StatsCollection();
        stats[StatsEnum.INFLICTED_DAMAGE] += 20;//brutalidad
        stats[StatsEnum.INFLICTED_DAMAGE_MELE] += 20; //brutalidad

        stats[StatsEnum.INFLICTED_DAMAGE] += 20; // borrasca
        stats[StatsEnum.INFLICTED_DAMAGE] += 15; // hinchable
        stats[StatsEnum.INFLICTED_DAMAGE] += 20; //chute
        stats[StatsEnum.INFLICTED_DAMAGE] += 20; //ani
        stats[StatsEnum.INFLICTED_DAMAGE_MELE] += 40; //baston


        //Spell conditions
        CalculateDamage calculateReq = new CalculateDamage(
            baseDamage: 62,
            type: DomainType.FIRE,
            resistance: new Domain.Models.Stats.StatsCollection(),
            sideDamage: SideDamage.FRONT, 
            isCrit: false,
            rangeDamageEnum: RangeDamageEnum.HIGHEST,
            isBerserker: false, 
            isHealing: false, 
            isIndirect: false, 0);

        //filters for gear
        var minLevel = 1;
        var maxLevel = 155;
        var bannedGear = new int[] 
        {
            31344, 31381,31354,31353, 31360,31359//talkasha drops
            ,30684
            ,26750//coraza apaciguador 155
            ,31235,31234

        };
        var bannedStats = new List<StatsEnum>() 
        {
            //StatsEnum.REAR_DOMAIN,
            //StatsEnum.BERSERKER_DOMAIN,
            //StatsEnum.DISTANCE_DOMAIN,
            //StatsEnum.MELE_DOMAIN,
            //StatsEnum.CRIT_DOMAIN, 
            //StatsEnum.HEAL_DOMAIN
        };
        ItemRarity[] qualities =  new ItemRarity[] {  ItemRarity.LEGENDARY, ItemRarity.MYTHIC, ItemRarity.SOUVENIR };
        var botas = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Botas], qualities, new Dictionary<StatsEnum, double>()
        {
        }, bannedStats, bannedGear);
        var casco = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Casco], qualities, new Dictionary<StatsEnum, double>()
        {
            //{StatsEnum.RANGE, 1}
        }, bannedStats, bannedGear);
        var amuletos = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Amuleto], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 },
            { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var capa = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Capa], qualities, new Dictionary<StatsEnum, double>() 
        {
            {StatsEnum.CRIT_HIT, 1 },
            { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var arma_1_mano = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetOneHandTypes(), qualities, new Dictionary<StatsEnum, double>()
        {
          // {StatsEnum.CRIT_HIT, 1 },

            {StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);

        var arma_2_manos = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetTwoHandedTypes(), qualities, new Dictionary<StatsEnum, double>()
        {
           { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);


        var coraza = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Coraza], qualities, new Dictionary<StatsEnum, double>()
        {
            //{ StatsEnum.BERSERKER_DOMAIN, 0 },
            //{ StatsEnum.CRIT_HIT, 0 },
            { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var hombreras = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Hombreras], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 }
        }, bannedStats, bannedGear);
        var cinturon = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Cinturon], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 }
        }, bannedStats, bannedGear);        
        var emblema = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Emblema], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 }
        }, bannedStats, bannedGear);
        var rings = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Anillo], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 0 }
        }, bannedStats, bannedGear);

        var epics = items.Filter(minLevel, maxLevel, null, [ItemRarity.EPIC], new Dictionary<StatsEnum, double>()
        {
            //{StatsEnum.CRIT_HIT, 0 }
        }, bannedStats, bannedGear);

        var relics = items.Filter(minLevel, maxLevel, null, [ItemRarity.RELIC], new Dictionary<StatsEnum, double>()
        {
            //{StatsEnum.CRIT_HIT, 0 }
        }, bannedStats, bannedGear);


        var casco_Dmg = casco.CalculateDamage(calculateReq, stats);
        var amuleto_dmg = amuletos.CalculateDamage(calculateReq, stats);
        var coraza_dmg = coraza.CalculateDamage(calculateReq, stats);
        var botas_dmg = botas.CalculateDamage(calculateReq, stats);
        var capa_dmg = capa.CalculateDamage(calculateReq, stats);
        var hombreras_dmg = hombreras.CalculateDamage(calculateReq, stats);
        var cinturon_dmg = cinturon.CalculateDamage(calculateReq, stats);
        var arma_dmg = arma_1_mano.CalculateDamage(calculateReq, stats);
        var arma_2_dmg = arma_2_manos.CalculateDamage(calculateReq, stats);
        var emblema_dmg = emblema.CalculateDamage(calculateReq, stats);
        var anillos_dmg = rings.CalculateDamage(calculateReq, stats);
        var anillo = items.FirstOrDefault(x => x.Definition.Item.Id == 9723).CalculateDamage(calculateReq, stats);
        var anillo2 = items.FirstOrDefault(x => x.Definition.Item.Id == 27281).CalculateDamage(calculateReq, stats);
        var epics_dmg = epics.CalculateDamage(calculateReq, stats);
        var relic_dmg = relics.CalculateDamage(calculateReq, stats);
        CharacterEquipments equip = new CharacterEquipments();

        equip.Add(
            casco_Dmg, 
            amuleto_dmg,
            coraza_dmg,
            botas_dmg,
            capa_dmg,
            hombreras_dmg,
            cinturon_dmg,
            arma_dmg, 
            emblema_dmg,
            anillos_dmg,
            anillos_dmg.Skip(1).ToDictionary()
            );

        var equpstats = equip.Stats.CalculateDamage(calculateReq, stats);//38408
        //equip.Add(anillo.Item1, anillo.Item2);
        //equip.Add(anillo2.Item1, anillo2.Item2);

        //openUrls(equip.Urls.Split('\n', StringSplitOptions.RemoveEmptyEntries));
        //var act = actions.Where(x => x.Description != null && x.Description.Spanish.Contains("Dominio"));
    }

    static void openUrls(string[] urls)
    {
        foreach (var url in urls)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
