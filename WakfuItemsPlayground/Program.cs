﻿using Domain.Enums;
using Domain.Models;
using Domain.Models.Stats;
using Domain.Models.Stats.Combat;
using Domain.Models.WAKFUAPI;
using Newtonsoft.Json;
using System.Diagnostics;
using WakfuItemsPlayground;
using WakfuItemsPlayground.Enums;
using ZenithWebHandler.Extensions;
using ZenithWebHandler.Handler;
using ZenithWebHandler.Models.Responses;

internal class Program
{
    private static async Task Main(string[] args)
    {

        var files = Directory.GetFiles(@"C:\Users\juanp\OneDrive\Documentos\github\calculator_wakfu\wakfuCalculatorStats\DownloaderVersions\bin\Debug\net8.0\WakfuJsonFiles_1.86.4.31");
        var txt = "";
        var items = new List<Item>();
        var actions = new List<Actions>();
        var itemTypes = new List<ItemTypes>();
        var equipmentItemTypes = new List<EquipmentItemTypes>();
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
                    equipmentItemTypes = JsonConvert.DeserializeObject<List<EquipmentItemTypes>>(File.ReadAllText(file));
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
            var itemType = equipmentItemTypes.FirstOrDefault(x => x.Definition.Id == item.Definition.Item.BaseParameters.ItemTypeId);

            item.Definition.Item.BaseParameters.ItemType = itemType ?? new EquipmentItemTypes();
            if (itemType != null)
            {
                item.Definition.Item.BaseParameters.ItemType.ItemTypeEnum = (ItemTypesEnum)item.Definition.Item.BaseParameters.ItemTypeId;
            }

            item.SetStatsCollection(false, false, false, false);
        }

        items = items.Where(X => X.Definition.Item.BaseParameters.ItemType.ItemTypeEnum != ItemTypesEnum.EscudoSegundaMano).ToList();
        //aditional Hidden stats
        StatsCollection stats = new StatsCollection();
        //stats[StatsEnum.INFLICTED_DAMAGE] += 40 + 25 + 12;


        //Spell conditions
        CalculateDamage calculateReq = new CalculateDamage(
            baseDamage: 70,
            type: DomainType.FIRE,
            resistance: new Domain.Models.Stats.StatsCollection(),
            sideDamage: SideDamage.FRONT,
            isCrit: false,
            rangeDamageEnum: RangeDamageEnum.HIGHEST,
            isBerserker: true,
            isHealing: false,
            isIndirect: true, 0);

        //filters for gear
        var minLevel = 1;
        var maxLevel = 2450;
        var bannedGear = new int[]
        {
            31344, 31381,31354,31353, 31360,31359,31340,31339,31341,31395

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

        //var requirements = new Dictionary<StatsEnum, double>()
        //{
        //    { StatsEnum.WP, 1 }
        //};

        ItemRarity[] qualities = new ItemRarity[] { ItemRarity.LEGENDARY, ItemRarity.SOUVENIR/*, ItemRarity.MYTHIC*/ };
        var botas = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Botas], qualities, new Dictionary<StatsEnum, double>()
        {
            //{StatsEnum.AP, 1 }
            //{StatsEnum.CRIT_DOMAIN, 0 },
            //{ StatsEnum.MP, 1 }
        }, bannedStats, bannedGear);
        var casco = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Casco], qualities, new Dictionary<StatsEnum, double>()
        {
           {StatsEnum.RANGE, 1},
           //{StatsEnum.CRIT_HIT, 1 }
        }, bannedStats, bannedGear);
        var amuletos = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Amuleto], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.RANGE, 1 },
           {StatsEnum.CRIT_HIT, 1 },
          //  { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var capa = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Capa], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 },
            { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var arma_1_mano = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetOneHandTypes(), qualities, new Dictionary<StatsEnum, double>()
        {
               { StatsEnum.RANGE, 1 },
           { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);

        var arma_2_manos = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetTwoHandedTypes(), qualities, new Dictionary<StatsEnum, double>()
        {
        }, bannedStats, bannedGear);

        var arma_2da_mano = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetSecondHandType(), qualities, new Dictionary<StatsEnum, double>()
        {
        }, bannedStats, bannedGear);

        var coraza = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Coraza], qualities, new Dictionary<StatsEnum, double>()
        {

            { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var hombreras = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Hombreras], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 },
              //  {StatsEnum.RANGE, 1 }
        }, bannedStats, bannedGear);
        var cinturon = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Cinturon], qualities, new Dictionary<StatsEnum, double>()
        {
               {StatsEnum.CRIT_HIT, 1 }
        }, bannedStats, bannedGear);
        var emblema = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Emblema], [ItemRarity.LEGENDARY, ItemRarity.MYTHIC], new Dictionary<StatsEnum, double>()
        {
               {StatsEnum.CRIT_HIT, 0 }
        }, bannedStats, bannedGear);
        var rings = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Anillo], qualities, new Dictionary<StatsEnum, double>()
        {
        }, bannedStats, bannedGear);

        var epics = items.Filter(minLevel, maxLevel, null, [ItemRarity.EPIC], new Dictionary<StatsEnum, double>()
        {
                    {StatsEnum.CRIT_HIT, 0 },
            {StatsEnum.AP, 1 }
        }, bannedStats, bannedGear);

        var relics = items.Filter(minLevel, maxLevel, null, [ItemRarity.RELIC], new Dictionary<StatsEnum, double>()
        {
             {StatsEnum.AP , 1},
                {StatsEnum.CRIT_HIT, 0 }
        }, bannedStats, bannedGear);

        var weaponTypes = new List<ItemTypesEnum>();
        weaponTypes.AddRange(ItemTypesEnum.Null_751.GetOneHandTypes());
        weaponTypes.AddRange(ItemTypesEnum.Null_751.GetTwoHandedTypes());
        weaponTypes.AddRange(ItemTypesEnum.Null_751.GetSecondHandType());

        var _cascos = casco.CalculateDamage(calculateReq, stats);
        var _amuletos = amuletos.CalculateDamage(calculateReq, stats);
        var _corazas = coraza.CalculateDamage(calculateReq, stats);
        var _botas = botas.CalculateDamage(calculateReq, stats);
        var _capas = capa.CalculateDamage(calculateReq, stats);
        var _hombreras = hombreras.CalculateDamage(calculateReq, stats);
        var _cinturones = cinturon.CalculateDamage(calculateReq, stats);
        var _armas_1_mano = arma_1_mano.CalculateDamage(calculateReq, stats);
        var _arma_2_manos = arma_2_manos.CalculateDamage(calculateReq, stats);
        var _emblemas = emblema.CalculateDamage(calculateReq, stats);
        var _anillos = rings.CalculateDamage(calculateReq, stats);
        var anillo = items.Get(9723).CalculateDamage(calculateReq, stats);
        var anillo_fab = items.Get(27281).CalculateDamage(calculateReq, stats);
        var _arma_2da_mano = arma_2da_mano.CalculateDamage(calculateReq, stats);

        var _epics = epics
            .Where(x => x.GiveExtra(StatsEnum.AP, weaponTypes))
            .ToList().CalculateDamage(calculateReq, stats);
        var _relics = relics
            .Where(x => x.GiveExtra(StatsEnum.AP, weaponTypes))
            .ToList().CalculateDamage(calculateReq, stats);

        CharacterEquipments equip = new CharacterEquipments();


        var equips = new CharacterEquipments();
        //equips.Add(items.Get(29635));
        //equips.Add(items.Get(31904));
        //equips.Add(items.Get(25938));
        var __equiOptimized = Optimize(_cascos,
            _amuletos,
            _corazas,
            _botas,
            _capas,
            _hombreras,
            _cinturones,
            _armas_1_mano,
            _emblemas,
            _anillos,
            _relics,
            _epics,
            _arma_2_manos,
            _arma_2da_mano, equips);

        __equiOptimized.Stats += stats;
        var equpstats = __equiOptimized.Stats.CalculateDamage(calculateReq, stats);//38408

        await GenerateZenith(maxLevel, $"Build {DateTimeOffset.Now.ToUnixTimeSeconds()}", __equiOptimized.Items.Select(x => x.Value).ToArray());
    }

    private static async Task GenerateZenith(int lvl, string name, params Item[] items)
    {
        List<ItemZenith> items_zenith = new List<ItemZenith>();
        bool isFirstRing = true;
        foreach (var item in items)
        {
            var _item = await ZenithHandler.GetSingleItem(item);
            var itemType = item.Definition.Item.BaseParameters.ItemType.ItemTypeEnum;
            ZenithSideEquipEnum metaSide;
            switch (itemType)
            {
                case ItemTypesEnum.Casco: metaSide = ZenithSideEquipEnum.HELM; break;
                case ItemTypesEnum.Amuleto: metaSide = ZenithSideEquipEnum.NECKLACE; break;
                case ItemTypesEnum.Coraza: metaSide = ZenithSideEquipEnum.BREATSPLACE; break;
                case ItemTypesEnum.Anillo:
                    metaSide = isFirstRing ? ZenithSideEquipEnum.LEFT_RING : ZenithSideEquipEnum.RIGHT_RING;
                    isFirstRing = false;
                    break;
                case ItemTypesEnum.Botas: metaSide = ZenithSideEquipEnum.BOOTS; break;
                case ItemTypesEnum.Capa: metaSide = ZenithSideEquipEnum.CAPE; break;
                case ItemTypesEnum.Hombreras: metaSide = ZenithSideEquipEnum.SHOULDERS; break;
                case ItemTypesEnum.Cinturon: metaSide = ZenithSideEquipEnum.BELT; break;
                case ItemTypesEnum.Emblema: metaSide = ZenithSideEquipEnum.EMBLEM; break;
                default:
                    {
                        var oneHandType = ItemTypesEnum.Null_751.GetOneHandTypes();
                        var secondHandType = ItemTypesEnum.Null_751.GetSecondHandType();
                        var twoHandType = ItemTypesEnum.Null_751.GetTwoHandedTypes();

                        if (oneHandType.Contains(itemType) || twoHandType.Contains(itemType))
                        {
                            metaSide = ZenithSideEquipEnum.FIRST_HAND;
                            break;
                        }

                        if (secondHandType.Contains(itemType))
                        {
                            metaSide = ZenithSideEquipEnum.SECOND_HAND;
                            break;
                        }

                        throw new Exception("Not handled: " + itemType);
                    }
            }
            _item.Metadata = new Metadata() { Side = (int)metaSide };
            _item.Elements();
            items_zenith.Add(_item);
        }

        var build = await ZenithHandler.Create(lvl, name);
        Console.WriteLine("Created Build in zenith: " + build);
        var info = await ZenithHandler.GetBuild(build);
        Console.WriteLine($"Fetched data from build [{build}]: {info.NameBuild} - {info.IdBuild} ");
        foreach (var item in items_zenith)
            await ZenithHandler.Add(item, info.IdBuild);
        Console.WriteLine($"Opening url: {build.GetZenithLink()}");
        Process.Start(new ProcessStartInfo()
        {
            FileName = build.GetZenithLink(),
            UseShellExecute = true
        });
    }

    private static CharacterEquipments Optimize(Dictionary<Item, double> cascos,
                                 Dictionary<Item, double> amuletos,
                                 Dictionary<Item, double> corazas,
                                 Dictionary<Item, double> botas,
                                 Dictionary<Item, double> capas,
                                 Dictionary<Item, double> hombreras,
                                 Dictionary<Item, double> cinturones,
                                 Dictionary<Item, double> armas_1_mano,
                                 Dictionary<Item, double> emblemas,
                                 Dictionary<Item, double> anillos,
                                 Dictionary<Item, double> relics,
                                 Dictionary<Item, double> epics,
                                 Dictionary<Item, double> armas_2_manos,
                                 Dictionary<Item, double> armas_2da_manos,
                                 CharacterEquipments equips)
    {
        //Dictionary<Item,  Dictionary<Item, double>> relicCompared = new Dictionary<Item, Dictionary<Item, double>>();
        Dictionary<Item, (Item, double)> relicCompared = new Dictionary<Item, (Item, double)>();
        Dictionary<Item, (Item, double)> epicCompared = new Dictionary<Item, (Item, double)>();


        foreach (var (relic, value) in relics)
        {
            Dictionary<Item, double> comparedDict = DeterminateDictionaryToCompare(relic.Definition.Item.BaseParameters.ItemType.ItemTypeEnum, cascos, amuletos, corazas, botas, capas, hombreras, cinturones, armas_1_mano, emblemas, anillos, armas_2_manos, armas_2da_manos);
            var compared = CompareOne(relic, value, comparedDict);
            relicCompared.Add(compared.Item1, (compared.Item2, compared.Item3));
        }
        relicCompared = relicCompared.OrderByDescending(x => x.Value.Item2).ToDictionary();
        foreach (var (epic, value) in epics)
        {
            Dictionary<Item, double> comparedDict = DeterminateDictionaryToCompare(epic.Definition.Item.BaseParameters.ItemType.ItemTypeEnum, cascos, amuletos, corazas, botas, capas, hombreras, cinturones, armas_1_mano, emblemas, anillos, armas_2_manos, armas_2da_manos);
            var compared = CompareOne(epic, value, comparedDict);
            epicCompared.Add(compared.Item1, (compared.Item2, compared.Item3));
        }
        epicCompared = epicCompared.OrderByDescending(x => x.Value.Item2).ToDictionary();

        List<ItemTypesEnum> neededItems = new List<ItemTypesEnum>();

        CheckNeeded(cascos, neededItems, ItemTypesEnum.Casco);
        CheckNeeded(amuletos, neededItems, ItemTypesEnum.Amuleto);
        CheckNeeded(corazas, neededItems, ItemTypesEnum.Coraza);
        CheckNeeded(botas, neededItems, ItemTypesEnum.Botas);
        CheckNeeded(capas, neededItems, ItemTypesEnum.Capa);
        CheckNeeded(hombreras, neededItems, ItemTypesEnum.Hombreras);
        CheckNeeded(cinturones, neededItems, ItemTypesEnum.Cinturon);
        CheckNeeded(armas_1_mano, neededItems, ItemTypesEnum.ArmaDosManos);
        CheckNeeded(emblemas, neededItems, ItemTypesEnum.Emblema);
        CheckNeeded(anillos, neededItems, ItemTypesEnum.Anillo);
        CheckNeeded(armas_2da_manos, neededItems, ItemTypesEnum.SegundaMano);
        CheckNeeded(armas_2_manos, neededItems, ItemTypesEnum.ArmaDosManos);

        if (neededItems.Count > 0) throw new Exception("Can't create full build");
        if (relics.Count == 0) throw new Exception("No relics");
        if (epics.Count == 0) throw new Exception("No epics");

        if (relicCompared.First().Value.Item2 >= epicCompared.First().Value.Item2)
        {
            equips.AddRelicEpic(relicCompared);
            equips.AddRelicEpic(epicCompared);
        }
        else
        {
            equips.AddRelicEpic(epicCompared);
            equips.AddRelicEpic(relicCompared);
        }
        equips.AddSingle(cascos);
        equips.AddSingle(amuletos);
        equips.AddSingle(corazas);
        equips.AddSingle(botas);
        equips.AddSingle(capas);
        equips.AddSingle(hombreras);
        equips.AddSingle(cinturones);
        equips.AddSingle(armas_1_mano);
        equips.AddSingle(emblemas);
        equips.AddSingle(anillos);
        var skipped = anillos.Skip(1).ToDictionary();
        equips.AddSingle(skipped);
        equips.AddSingle(armas_2da_manos);
        return equips;
        //equips.AddSingle(armas_2da_manos);
        //equips.AddSingle(amuletos);
    }

    private static void CheckNeeded(Dictionary<Item, double> cascos, List<ItemTypesEnum> neededItems, ItemTypesEnum type)
    {
        if (cascos.Count == 0) neededItems.Add(type);
    }

    static (Item, Item, double) CompareOne(Item item, double value, Dictionary<Item, double> comparableDict)
    {
        if (comparableDict.Any())
        {
            var first = comparableDict.First();
            return (item, first.Key, value - first.Value);
        }

        return (item, null, value);
    }

    static (Item, Dictionary<Item, double>) Compare(Item item, double value, Dictionary<Item, double> comparableDict)
    {
        var result = new Dictionary<Item, double>();
        foreach (var (comparable, comp_value) in comparableDict)
        {
            result.Add(comparable, value - comp_value);
        }
        return (item, result);
    }

    static Dictionary<Item, double> DeterminateDictionaryToCompare(ItemTypesEnum type, Dictionary<Item, double> cascos,
                                 Dictionary<Item, double> amuletos,
                                 Dictionary<Item, double> corazas,
                                 Dictionary<Item, double> botas,
                                 Dictionary<Item, double> capas,
                                 Dictionary<Item, double> hombreras,
                                 Dictionary<Item, double> cinturones,
                                 Dictionary<Item, double> armas_1_mano,
                                 Dictionary<Item, double> emblemas,
                                 Dictionary<Item, double> anillos,
                                 Dictionary<Item, double> armas_2_manos,
                                 Dictionary<Item, double> armas_2da_manos)
    {
        return type switch
        {
            ItemTypesEnum.Hombreras => hombreras,
            ItemTypesEnum.Casco => cascos,
            ItemTypesEnum.Amuleto => amuletos,
            ItemTypesEnum.Coraza => corazas,
            ItemTypesEnum.Botas => botas,
            ItemTypesEnum.Capa => capas,
            ItemTypesEnum.Cinturon => cinturones,
            ItemTypesEnum.AgujaUnaMano => armas_1_mano,
            ItemTypesEnum.ArmaUnaMano => armas_1_mano,
            ItemTypesEnum.BastonUnaMano => armas_1_mano,
            ItemTypesEnum.CartaUnaMano => armas_1_mano,
            ItemTypesEnum.EspadaUnaMano => armas_1_mano,
            ItemTypesEnum.VaritaUnaMano => armas_1_mano,
            ItemTypesEnum.Emblema => emblemas,
            ItemTypesEnum.Anillo => anillos,
            ItemTypesEnum.ArcoDosManos => armas_2_manos,
            ItemTypesEnum.ArmaDosManos => armas_2_manos,
            ItemTypesEnum.BastonDosManos => armas_2_manos,
            ItemTypesEnum.EspadaDosManos => armas_2_manos,
            ItemTypesEnum.HachaDosManos => armas_2_manos,
            ItemTypesEnum.MartilloDosManos => armas_2_manos,
            ItemTypesEnum.PalaDosManos => armas_2_manos,
            ItemTypesEnum.EscudoSegundaMano => new Dictionary<Item, double>(),
            ItemTypesEnum.DagaSegundaMano => armas_2da_manos,
            _ => throw new Exception($"Not handled {type}")
        };
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
