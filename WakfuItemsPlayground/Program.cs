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
            baseDamage: 155,
            type: DomainType.FIRE,
            resistance: new Domain.Models.Stats.StatsCollection(),
            sideDamage: SideDamage.FRONT,
            isCrit: false,
            rangeDamageEnum: RangeDamageEnum.HIGHEST,
            isBerserker: false,
            isHealing: false,
            isIndirect: true, 0);

        //filters for gear
        var minLevel = 1;
        var maxLevel = 95;
        string buildCode = "wbz90";// or code of zenith build
        var ignoreRelic = false;
        var ignore_epic = false;
        var fillIncomplete = true;
        var alwaysResis = true;


        var bots_types = new ItemTypesEnum[] { ItemTypesEnum.Botas };
        var amu_types = new ItemTypesEnum[] { ItemTypesEnum.Amuleto };
        var capa_types = new ItemTypesEnum[] { ItemTypesEnum.Capa };
        var coraza_types = new ItemTypesEnum[] { ItemTypesEnum.Coraza };
        var casco_types = new ItemTypesEnum[] { ItemTypesEnum.Casco };
        var one_Hand_types = ItemTypesEnum.Null_751.GetOneHandTypes();
        var two_Hand_types = ItemTypesEnum.Null_751.GetTwoHandedTypes();
        var second_hand_types = ItemTypesEnum.Null_751.GetSecondHandType();
        var hombrera_types = new ItemTypesEnum[] { ItemTypesEnum.Hombreras };
        var cintu_types = new ItemTypesEnum[] { ItemTypesEnum.Cinturon };
        var emblema_types = new ItemTypesEnum[] { ItemTypesEnum.Emblema };
        var anillo_types = new ItemTypesEnum[] { ItemTypesEnum.Anillo };

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
        //Console.WriteLine("presione cualquier tecla para empezar");
        //Console.ReadLine();

        var bannedGear = new int[]
        {
            //31360,31353,31341,31354,31353,21692,31395,17322,31381,31354,//talkasha
            ////23164,23165,26750,24346//archi
            26497,26494,26495,26496,//espadas
            //27376,26288,24028,27290,27304,27410,27298,29528,29168,31555
            //,23670,23671,14884
            14663,14635,14279,14282
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

        var requirements = new Dictionary<ItemTypesEnum[], Dictionary<StatsEnum, double>>()
        {
            { bots_types, new Dictionary<StatsEnum, double>() { /*{ StatsEnum.MP, 1 }*/ } },             
            { amu_types, new Dictionary<StatsEnum, double>() { { StatsEnum.AP, 1 }, { StatsEnum.CRIT_HIT, 1 } } },            
            { capa_types, new Dictionary<StatsEnum, double>() { { StatsEnum.AP, 1 }, { StatsEnum.CRIT_HIT, 1 } } },            
            { coraza_types, new Dictionary<StatsEnum, double>() { { StatsEnum.AP, 1 } } },
            { casco_types, new Dictionary<StatsEnum, double>() { } },
            { one_Hand_types, new Dictionary<StatsEnum, double>() { { StatsEnum.AP, 1 }, { StatsEnum.CRIT_HIT, 1 } } },
            { second_hand_types, new Dictionary<StatsEnum, double>() { { StatsEnum.CRIT_HIT, 1 } } },
            { hombrera_types, new Dictionary<StatsEnum, double>() { { StatsEnum.CRIT_HIT, 1 } } },
            { cintu_types, new Dictionary<StatsEnum, double>() { { StatsEnum.CRIT_HIT, 1 } } },
            { emblema_types, new Dictionary<StatsEnum, double>() { { StatsEnum.CRIT_HIT, 0 } } },
            { anillo_types, new Dictionary<StatsEnum, double>() { { StatsEnum.CRIT_HIT, 0 } } },

            { two_Hand_types, new Dictionary<StatsEnum, double>() { { StatsEnum.AP, 1 }, { StatsEnum.CRIT_HIT, 1 } } },
        };

        foreach (var req in requirements)
        {
            req.Value.Add(StatsEnum.EARTH_RESIST, 1);
            req.Value.Add(StatsEnum.WATER_RESIST, 1);
        }

        var requirements_epic = new Dictionary<StatsEnum, double>() { { StatsEnum.AP, 1 } };
        var requirements_relic = new Dictionary<StatsEnum, double>() { { StatsEnum.AP, 1 } };

        ItemRarity[] qualities =  new ItemRarity[] { ItemRarity.LEGENDARY, ItemRarity.SOUVENIR, ItemRarity.MYTHIC, ItemRarity.RARE };
        var botas = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Botas], qualities, requirements[bots_types], alwaysResis, bannedStats, bannedGear);
        var casco = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Casco], qualities, requirements[casco_types], alwaysResis, bannedStats, bannedGear);
        var amuletos = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Amuleto], qualities, requirements[amu_types], alwaysResis, bannedStats, bannedGear);
        var capa = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Capa], qualities, requirements[capa_types], alwaysResis, bannedStats, bannedGear);
        var arma_1_mano = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetOneHandTypes(), qualities, requirements[one_Hand_types], alwaysResis, bannedStats, bannedGear);
        var arma_2_manos = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetTwoHandedTypes(), qualities, requirements[two_Hand_types], alwaysResis, bannedStats, bannedGear);
        var arma_2da_mano = items.Filter(minLevel, maxLevel, ItemTypesEnum.Null_751.GetSecondHandType(), qualities, requirements[second_hand_types], alwaysResis, bannedStats, bannedGear);
        var coraza = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Coraza], qualities, requirements[coraza_types], alwaysResis, bannedStats, bannedGear);
        var hombreras = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Hombreras], qualities, requirements[hombrera_types], alwaysResis, bannedStats, bannedGear);
        var cinturon = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Cinturon], qualities, requirements[cintu_types], alwaysResis, bannedStats, bannedGear);
        var emblema = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Emblema], [ItemRarity.LEGENDARY, ItemRarity.MYTHIC], requirements[emblema_types], alwaysResis, bannedStats, bannedGear);
        var rings = items.Filter(minLevel, maxLevel, [ItemTypesEnum.Anillo], qualities, requirements[anillo_types], alwaysResis, bannedStats, bannedGear);
        
        var epics = items.Filter(minLevel, maxLevel, null, [ItemRarity.EPIC], requirements_epic, alwaysResis, bannedStats, bannedGear);
        var relics = items.Filter(minLevel, maxLevel, null, [ItemRarity.RELIC], requirements_relic, alwaysResis, bannedStats, bannedGear);
        var weaponTypes = new List<ItemTypesEnum>();
        weaponTypes.AddRange(ItemTypesEnum.Null_751.GetOneHandTypes());
        weaponTypes.AddRange(ItemTypesEnum.Null_751.GetTwoHandedTypes());
        weaponTypes.AddRange(ItemTypesEnum.Null_751.GetSecondHandType());

        var wp = items.Filter(minLevel, maxLevel, null, [ItemRarity.MYTHIC, ItemRarity.LEGENDARY],
            new Dictionary<StatsEnum, double>() { { StatsEnum.WP, 1 }, { StatsEnum.CRIT_HIT, 0 } })
            .CalculateDamage(calculateReq, stats);

        var wpgrouped = wp.GroupBy(x => x.Key.Definition.Item.BaseParameters.ItemType.ItemTypeEnum);

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
        //equip.Add(items.Get(21150));
        //equip.Add(items.Get(9723));
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
            _arma_2da_mano, equip, ignoreRelic, ignore_epic, fillIncomplete);
            
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
            if(!ignoreRelic && !equips.HasRelic)
                equips.AddRelicEpic(relicCompared);
            if(!ignoreEpic && !equips.HasEpic)
                equips.AddRelicEpic(epicCompared);
        }
        else if(_epic != null)
        {
            if (!ignoreEpic && _epic != null && !equips.HasEpic)
                equips.AddRelicEpic(epicCompared);
            if (!ignoreRelic && _relic != null && !equips.HasRelic)
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
