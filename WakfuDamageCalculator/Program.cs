using Domain.Enums;
using Domain.Models;
using Domain.Models.Stats;
using Domain.Models.Stats.Combat;
using Domain.Models.Stats.Domains;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScrapperZenith;


//await Scrapper.Get();
//Console.ReadKey();


//stats[StatsEnum.INDIRECT_DAMAGE] += 30;

//var xel = await Scrapper.GetZenithData("https://zenithwakfu.com/builder/6e185");
//var tactic = 25.0;
//var puntualidad = 50.0;
//xel[StatsEnum.INFLICTED_DAMAGE] += (tactic+puntualidad);


var mrpunchy = new StatsCollection();
mrpunchy[StatsEnum.FIRE_RESIST] = 1040;
mrpunchy[StatsEnum.WATER_RESIST] = 1040;
mrpunchy[StatsEnum.AIR_RESIST] = 1040;
mrpunchy[StatsEnum.EARTH_RESIST] = 1040;


//var sadi_elecon = await Scrapper.GetZenithData("https://www.zenithwakfu.com/builder/61240");
//var sadi_noelecon = await Scrapper.GetZenithData("https://www.zenithwakfu.com/builder/05dc0");

//sadi_elecon[StatsEnum.INFLICTED_DAMAGE] += 9; //pesos pluma
//sadi_noelecon[StatsEnum.INFLICTED_DAMAGE] += 9; //pesos pluma
//sadi_noelecon[StatsEnum.INDIRECT_DAMAGE] += 40; //constante ii

//var hupper = Hupper();
var sadi = Sadi();
while (true)
{
    //var desprendimiento = hupper.CalculateDamage(62, DomainType.EARTH, mrpunchy, SideDamage.FRONT, false, false, false, false, false);
    //var desprendimiento_crit = hupper.CalculateDamage(77, DomainType.EARTH, mrpunchy, SideDamage.FRONT, false, false, false, false, false);
    //var air = hupper.CalculateDamage(62, DomainType.AIR, mrpunchy, SideDamage.FRONT, false, false, false, false, false);


    //var ranura = hupper.CalculateDamage(57, DomainType.EARTH, mrpunchy, SideDamage.FRONT, false, false, false, false, false);
    //var ranura_crit= hupper.CalculateDamage(72, DomainType.EARTH, mrpunchy, SideDamage.FRONT, false, false, false, false, false);
    //var maldito_con_elecon = sadi_elecon.CalculateDamage(76, DomainType.AIR, mrpunchy, SideDamage.FRONT, false, true, false, false, true, 80.0);
    //var toxina_con_elecon= sadi_elecon.CalculateDamage(490, DomainType.AIR, mrpunchy, SideDamage.FRONT, false, true, false, false, true, 80.0);

    //var maldito_sin_elecon = sadi_noelecon.CalculateDamage(76, DomainType.AIR, mrpunchy, SideDamage.FRONT, false, true, false, false, true, 80.0);  
    //var toxina_sin_elecon = sadi_noelecon.CalculateDamage(490, DomainType.AIR, mrpunchy, SideDamage.FRONT, false, true, false, false, true, 80.0);
    CalculateDamage dmgNoCrit = new(72, DomainType.AIR, mrpunchy, SideDamage.FRONT, false, RangeDamageEnum.DIST, false, false, false, 0);
    CalculateDamage dmgCrit = new(90, DomainType.AIR, mrpunchy, SideDamage.FRONT, true, RangeDamageEnum.DIST, false, false, false, 0);
    var noCrit = sadi.CalculateDamage(dmgNoCrit);
   var crit = sadi.CalculateDamage(dmgCrit);

    //var poisonNoCrit = sadi.CalculateDamage(28, DomainType.AIR, puch, SideDamage.FRONT, false, true, false, false, true);//248
    //var poisonCrit = sadi.CalculateDamage(35, DomainType.AIR, puch, SideDamage.FRONT, false, true, false, false, true);//311


    //var noCrit = xel.CalculateDamage(132, DomainType.FIRE, mrpunchy, SideDamage.FRONT, false, true, false, false, false);
    //var crit = xel.CalculateDamage(166, DomainType.FIRE, mrpunchy, SideDamage.FRONT, true, true, false, false, false);

}
Console.ReadKey();


StatsCollection Sadi()
{
    var sadi = new StatsCollection();
    sadi[StatsEnum.INFLICTED_DAMAGE] = 35 + 12;
    sadi[StatsEnum.WATER_DOMAIN] = 1752;
    sadi[StatsEnum.EARTH_DOMAIN] = 578;
    sadi[StatsEnum.AIR_DOMAIN] = 4645;
    //stats[StatsEnum.AIR_DOMAIN] = 3681;
    sadi[StatsEnum.FIRE_DOMAIN] = 2300;
    sadi[StatsEnum.DISTANCE_DOMAIN] = 1934;
    sadi[StatsEnum.INDIRECT_DAMAGE] = 50;

    //sadi[StatsEnum.WATER_RESIST] = 896;
    //sadi[StatsEnum.EARTH_RESIST] = 917;
    //sadi[StatsEnum.AIR_RESIST] = 890;
    //sadi[StatsEnum.FIRE_RESIST] = 909;

    //sadi[StatsEnum.BERSERKER_DOMAIN] = -159;
    //sadi[StatsEnum.CRIT_DOMAIN] = 34;
    //sadi[StatsEnum.HEAL_DOMAIN] = 34;
    //sadi[StatsEnum.MELE_DOMAIN] = 34;
    //sadi[StatsEnum.REAR_DOMAIN] = -413;

    //sadi[StatsEnum.CRIT_RESISTANCE] = 89;
    //sadi[StatsEnum.GIVEN_ARMOR] = 0;
    //sadi[StatsEnum.INDIRECT_DAMAGE] = 30;
    //sadi[StatsEnum.REAR_RESISTANCE] = 81;
    //sadi[StatsEnum.RECEIVED_ARMOR] = 8;

    //sadi[StatsEnum.CRIT_HIT] = 49;
    //sadi[StatsEnum.INI] = 89;
    //sadi[StatsEnum.DODGE] = 559;
    //sadi[StatsEnum.CONTROL] = 1;
    //sadi[StatsEnum.FINAL_HEAL] = -22;
    //sadi[StatsEnum.BLOCK] = 6;
    //sadi[StatsEnum.RANGE] = 4;
    //sadi[StatsEnum.LOCK] = 214;
    //sadi[StatsEnum.PP] = 10;
    //sadi[StatsEnum.WILL] = 10;
    return sadi;
}

StatsCollection Xel()
{

    var result = new StatsCollection();
    result[StatsEnum.INFLICTED_DAMAGE] = 167+12;
    result[StatsEnum.CRIT_HIT] = 49;
    //result[StatsEnum.INI] = 89;
    //result[StatsEnum.DODGE] = 559;
    //result[StatsEnum.CONTROL] = 1;
    //result[StatsEnum.FINAL_HEAL] = -22;
    //result[StatsEnum.BLOCK] = 6;
    //result[StatsEnum.RANGE] = 4;
    //result[StatsEnum.LOCK] = 214;
    //result[StatsEnum.PP] = 10;
    //result[StatsEnum.WILL] = 10;
    result[StatsEnum.WATER_DOMAIN] = 2870;
    result[StatsEnum.EARTH_DOMAIN] = 658;
    result[StatsEnum.AIR_DOMAIN] = 2219;
    result[StatsEnum.FIRE_DOMAIN] = 6921;

    result[StatsEnum.WATER_RESIST] = 937;
    result[StatsEnum.EARTH_RESIST] = 920;
    result[StatsEnum.AIR_RESIST] = 917;
    result[StatsEnum.FIRE_RESIST] = 1001;

    result[StatsEnum.CRIT_RESISTANCE] = 72;
    result[StatsEnum.GIVEN_ARMOR] = 0;
    result[StatsEnum.INDIRECT_DAMAGE] = 0;
    result[StatsEnum.REAR_RESISTANCE] = 0;
    result[StatsEnum.RECEIVED_ARMOR] = 0;
    return result;
}

StatsCollection Hupper()
{
    var s = new StatsCollection();
    s[StatsEnum.INFLICTED_DAMAGE] = 87.0+ 20.0 -10.0 + 12.0;
    s[StatsEnum.EARTH_DOMAIN] = 7071.0;
    s[StatsEnum.AIR_DOMAIN] = 1056;
    return s;
}