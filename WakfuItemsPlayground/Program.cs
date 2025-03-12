using Domain.Enums;
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
            baseDamage: 125,
            type: DomainType.FIRE,
            resistance: new Domain.Models.Stats.StatsCollection(),
            sideDamage: SideDamage.FRONT,
            isCrit: false,
            rangeDamageEnum: RangeDamageEnum.DIST,
            isBerserker: false,
            isHealing: false,
            isIndirect: false, 0);

        //filters for gear
        var minLevel = 1;
        var maxLevel = 125;
        string buildCode = null;// or code of zenith build
        var ignoreRelic = false;
        var ignore_epic = false;
        var fillIncomplete = true;

        Console.WriteLine("PARAMETROS:");
        Console.WriteLine("Level minimo de los objetos: " + minLevel);
        Console.WriteLine("Level maximo de los objetos: " + maxLevel);
        Console.WriteLine("Tipo: " + calculateReq.RangeDamageType.ToString());
        Console.WriteLine("Se tiene en cuenta el critico?: " + calculateReq.IsCrit.YesNo());
        Console.WriteLine("Se tiene en cuenta el berserker?: " + calculateReq.IsBerserker.YesNo());
        Console.WriteLine("Se tiene en cuenta las curas?: " + calculateReq.IsHealing.YesNo());
        Console.WriteLine("Side Damage: " + calculateReq.Side.ToString());
        Console.WriteLine("Elemento: " + calculateReq.DomainType.ToString());
        Console.WriteLine(buildCode  == null ? "Se creará un zenith builder nuevo." : "Se actualizara sobre el builder: " + buildCode);
        Console.WriteLine("presione cualquier tecla para empezar");
        Console.ReadLine();

        var bannedGear = new int[]
        {
            19450,14707,17498,23110,23056,24222
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

        List<Dictionary<StatsEnum, double>> requiremets = new List<Dictionary<StatsEnum, double>>();

        var requirements = new Dictionary<StatsEnum, double>()
        {
            { StatsEnum.RANGE, 5 }
        };

        ItemRarity[] qualities = new ItemRarity[] { ItemRarity.LEGENDARY, ItemRarity.SOUVENIR, ItemRarity.MYTHIC };
        var botas = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Botas], qualities, new Dictionary<StatsEnum, double>()
        {
         //   {StatsEnum.CRIT_DOMAIN, 0 },
        }, bannedStats, bannedGear);
        var casco = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Casco], qualities, new Dictionary<StatsEnum, double>()
        {
         //  {StatsEnum.CRIT_HIT, 0 },
            {StatsEnum.RANGE, 1 }
        }, bannedStats, bannedGear);
        var amuletos = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Amuleto], qualities, new Dictionary<StatsEnum, double>()
        {
            { StatsEnum.AP, 1 },
          {StatsEnum.RANGE, 1 },
            {StatsEnum.CRIT_HIT, 1 }
        }, bannedStats, bannedGear);
        var capa = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Capa], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 },
            { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var arma_1_mano = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetOneHandTypes(), qualities, new Dictionary<StatsEnum, double>()
        {
       //     {StatsEnum.CRIT_HIT, 0 },
          { StatsEnum.AP, 1 },
          { StatsEnum.WP, 1 },
        }, bannedStats, bannedGear);

        var arma_2_manos = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetTwoHandedTypes(), qualities, new Dictionary<StatsEnum, double>()
        {
      //      {StatsEnum.CRIT_HIT, 0 },
        }, bannedStats, bannedGear);

        var arma_2da_mano = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetSecondHandType(), qualities, new Dictionary<StatsEnum, double>()
        {
      //      {StatsEnum.CRIT_HIT, 0 },
        }, bannedStats, bannedGear);

        var coraza = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Coraza], qualities, new Dictionary<StatsEnum, double>()
        {
            { StatsEnum.AP, 1 },
        }, bannedStats, bannedGear);
        var hombreras = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Hombreras], qualities, new Dictionary<StatsEnum, double>()
        {
            {StatsEnum.CRIT_HIT, 1 },

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
             {StatsEnum.CRIT_HIT, 0 },
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

        var items2 = botas.Concat(casco).Concat(amuletos).Concat(capa).Concat(coraza).Concat(hombreras).Concat(cinturon).Concat(arma_1_mano).Concat(arma_2_manos)
            .Concat(emblema).Concat(rings).ToList().CalculateDamage(calculateReq, stats)
            .GroupBy(x => x.Key.Definition.Item.BaseParameters.ItemType.Definition.EquipmentPositionsData)
            .Select(x => new { type = x.Key, item = x.OrderByDescending(y => y.Value).FirstOrDefault() });

        var cat = items.Get(24162);
        var pet = items.Get(28371);


        CharacterEquipments equip = new CharacterEquipments();


        var equips = new CharacterEquipments();
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
            _arma_2da_mano, equips, ignoreRelic, ignore_epic, fillIncomplete);
            
        __equiOptimized.Stats += stats;
        var equpstats = __equiOptimized.Stats.CalculateDamage(calculateReq, stats);//38408
        Console.WriteLine(__equiOptimized.Stats);
        Console.WriteLine("Enter to export" + (buildCode  == null ? " to new build page" : " to buildcoide : " + buildCode ));
        Console.ReadLine();
        await GenerateZenith(maxLevel,
            $"Build {DateTimeOffset.Now.ToUnixTimeSeconds()}",
            pet,
            cat,
            buildCode,
            __equiOptimized.Items.Select(x => x.Value).ToArray()
            );
    }

    private static async Task GenerateZenith(int lvl, string name, Item pet, Item mount, string buildcode = null, params Item[] items)
    {
        List<ItemZenith> items_zenith = new List<ItemZenith>();
        bool isFirstRing = true;
        foreach (var item in items)
        {
            var _item = await ZenithHandler.GetSingleItem(item);
            _item.AddMetaData(
                item,
                ItemTypesEnum.Null_751.GetOneHandTypes().Concat(ItemTypesEnum.Null_751.GetTwoHandedTypes()).ToArray(),
                ItemTypesEnum.Null_751.GetSecondHandType(),
                isFirstRing
                );
            _item.Elements();
            items_zenith.Add(_item);

            if(item.Definition.Item.BaseParameters.ItemType.ItemTypeEnum == ItemTypesEnum.Anillo)
                isFirstRing = false;
        }

        var build = buildcode ??  await ZenithHandler.Create(lvl, name);
        Console.WriteLine("Created Build in zenith: " + build);
        var info = await ZenithHandler.GetBuild(build);
        Console.WriteLine($"Fetched data from build [{build}]: {info.NameBuild} - {info.IdBuild} ");
        var pet_item = await ZenithHandler.GetSingleItem(pet);
        var mount_item = await ZenithHandler.GetSingleItem(mount);
        await ZenithHandler.Add(pet_item.AddMetaData(pet), info.IdBuild);
        await ZenithHandler.Add(mount_item.AddMetaData(mount), info.IdBuild);
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
                                 CharacterEquipments equips,
                                 bool ignoreRelic = false, bool ignoreEpic = true, bool fillIncomplete = false)
    {
        //Dictionary<Item,  Dictionary<Item, double>> relicCompared = new Dictionary<Item, Dictionary<Item, double>>();
        Dictionary<Item, (Item, double)> relicCompared = new Dictionary<Item, (Item, double)>();
        Dictionary<Item, (Item, double)> epicCompared = new Dictionary<Item, (Item, double)>();

        if (!ignoreRelic)
        {
            foreach (var (relic, value) in relics)
            {
                Dictionary<Item, double> comparedDict = DeterminateDictionaryToCompare(relic.Definition.Item.BaseParameters.ItemType.ItemTypeEnum, cascos, amuletos, corazas, botas, capas, hombreras, cinturones, armas_1_mano, emblemas, anillos, armas_2_manos, armas_2da_manos);
                var compared = CompareOne(relic, value, comparedDict);
                relicCompared.Add(compared.Item1, (compared.Item2, compared.Item3));
            }
            relicCompared = relicCompared.OrderByDescending(x => x.Value.Item2).ToDictionary();
        }

        if (!ignoreEpic)
        {
            foreach (var (epic, value) in epics)
            {
                Dictionary<Item, double> comparedDict = DeterminateDictionaryToCompare(epic.Definition.Item.BaseParameters.ItemType.ItemTypeEnum, cascos, amuletos, corazas, botas, capas, hombreras, cinturones, armas_1_mano, emblemas, anillos, armas_2_manos, armas_2da_manos);
                var compared = CompareOne(epic, value, comparedDict);
                epicCompared.Add(compared.Item1, (compared.Item2, compared.Item3));
            }
            epicCompared = epicCompared.OrderByDescending(x => x.Value.Item2).ToDictionary();
        }
        List<ItemTypesEnum> neededItems = new List<ItemTypesEnum>();

        if (!fillIncomplete)
        {
            CheckNeeded(cascos, neededItems, ItemTypesEnum.Casco);
            CheckNeeded(amuletos, neededItems, ItemTypesEnum.Amuleto);
            CheckNeeded(corazas, neededItems, ItemTypesEnum.Coraza);
            CheckNeeded(botas, neededItems, ItemTypesEnum.Botas);
            CheckNeeded(capas, neededItems, ItemTypesEnum.Capa);
            CheckNeeded(hombreras, neededItems, ItemTypesEnum.Hombreras);
            CheckNeeded(cinturones, neededItems, ItemTypesEnum.Cinturon);
            CheckNeeded(armas_1_mano, neededItems, ItemTypesEnum.ArmaUnaMano);
            CheckNeeded(emblemas, neededItems, ItemTypesEnum.Emblema);
            CheckNeeded(anillos, neededItems, ItemTypesEnum.Anillo);
            CheckNeeded(armas_2da_manos, neededItems, ItemTypesEnum.SegundaMano);
            CheckNeeded(armas_2_manos, neededItems, ItemTypesEnum.ArmaDosManos);

            if (neededItems.Count > 0) throw new Exception("Can't create full build");
            if (relics.Count == 0) throw new Exception("No relics");
            if (epics.Count == 0) throw new Exception("No epics");
        }
        var (_relic, _relic_value) = relicCompared.FirstOrDefault();
        var (_epic, _epic_value) = epicCompared.FirstOrDefault();
        if (_relic != null && _relic_value.Item2 >= _epic_value.Item2)
        {
            if(!ignoreRelic)
                equips.AddRelicEpic(relicCompared);
            if(!ignoreEpic)
                equips.AddRelicEpic(epicCompared);
        }
        else if(_epic != null)
        {
            if (!ignoreEpic && _epic != null)
                equips.AddRelicEpic(epicCompared);
            if (!ignoreRelic && _relic != null)
                equips.AddRelicEpic(relicCompared);
        }
        equips.AddSingle(cascos);
        equips.AddSingle(amuletos);
        equips.AddSingle(corazas);
        equips.AddSingle(botas);
        equips.AddSingle(capas);
        equips.AddSingle(hombreras);
        equips.AddSingle(cinturones);
        var added_1st = equips.AddSingle(armas_1_mano);
        equips.AddSingle(emblemas);
        equips.AddSingle(anillos);
        var skipped = anillos.Skip(1).ToDictionary();
        equips.AddSingle(skipped);
        if (added_1st)
            equips.AddSingle(armas_2da_manos);
        else
            equips.AddSingle(armas_2_manos);
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
