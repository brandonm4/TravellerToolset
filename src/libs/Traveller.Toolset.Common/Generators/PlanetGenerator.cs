using System;
using Traveller.Toolset.Extensions;

namespace Traveller.Toolset.Generators;

public class PlanetGenerator()
{
    DiceRoller dice = new DiceRoller();
    public class PlanetGeneratorArgs
    {
        public int TemperatureModifier { get; set; }
        public int PopulationModifier { get; set; }
        public int GovernmentModifier { get; set; }
    }

    public World GenerateWorld(PlanetGeneratorArgs? args)
    {
        if (args == null) args = new();

        World world = new();
        var size = DetermineSize();
        var atmosphere = DetermineAtmosphere(size.Code);
        var temperature = DetermineTemperature(atmosphere.Code, args.TemperatureModifier);
        var hydro = DetermineHydrographics(size.Code, atmosphere.Code, temperature.Type);
        var population = DeterminePopulation(args.PopulationModifier);
        var government = DetermineGovernment(args.GovernmentModifier);

        world.SizeRating = size.Code;
        world.AtmosphereRating = atmosphere.Code;
        world.HydrogrpahicRating = hydro.Code;
        world.PopulationRating = population.Code;
        world.GovernmentRating = government.Code;
        return world;
    }


    private (string Code, int KM, float Gs) DetermineSize()
    {
        var roll = dice.RollDiceTotal(2, 6, -2);
        switch (roll)
        {
            case <= 0: return ("0", 100 * dice.RollDiceTotal(1, 10), 0);
            case 1: return ("1", 1600, 0.05f);
            case 2: return ("2", 3200, 0.15f);
            case 3: return ("3", 4800, 0.25f);
            case 4: return ("4", 6400, 0.35f);
            case 5: return ("5", 8000, 0.45f);
            case 6: return ("6", 9600, 0.7f);
            case 7: return ("7", 11200, 0.9f);
            case 8: return ("8", 12800, 1f);
            case 9: return ("9", 14400, 1.25f);
            case 10: return ("A", 16000, 1.4f);
        }

        return ("0", 100, 0);
    }

    private (string Code, string Composition, string Pressure) DetermineAtmosphere(string size)
    {
        var iSize = size.CodeToInt();
        

        //Roll 2D6-7 + size
        var roll = dice.RollDiceTotal(2, 6, -7) + iSize;

        // Clamp to 0-15 range (0-9, A-F)
        if (roll < 0) roll = 0;
        if (roll > 15) roll = 15;

        string code = roll.RollToCode();
       
        string composition;
        string pressure;

        switch (roll)
        {
            case 0:
                composition = "None (e.g., Moon)";
                pressure = "0.00";
                break;
            case 1:
                composition = "Trace (e.g., Mars)";
                pressure = "0.001 - 0.09";
                break;
            case 2:
                composition = "Very Thin, Tainted";
                pressure = "0.1 - 0.42";
                break;
            case 3:
                composition = "Very Thin";
                pressure = "0.1 - 0.42";
                break;
            case 4:
                composition = "Thin, Tainted";
                pressure = "0.43 - 0.7";
                break;
            case 5:
                composition = "Thin";
                pressure = "0.43 - 0.7";
                break;
            case 6:
                composition = "Standard (Earth-like)";
                pressure = "0.71 - 1.49";
                break;
            case 7:
                composition = "Standard, Tainted";
                pressure = "0.71 - 1.49";
                break;
            case 8:
                composition = "Dense";
                pressure = "1.5 - 2.49";
                break;
            case 9:
                composition = "Dense, Tainted";
                pressure = "1.5 - 2.49";
                break;
            case 10:
                composition = "Exotic";
                pressure = "Varies";
                break;
            case 11:
                composition = "Corrosive (e.g., Venus)";
                pressure = "Varies";
                break;
            case 12:
                composition = "Insidious";
                pressure = "Varies";
                break;
            case 13:
                composition = "Very Dense";
                pressure = "2.5+";
                break;
            case 14:
                composition = "Low";
                pressure = "0.5 or less";
                break;
            case 15:
            default:
                composition = "Unusual";
                pressure = "Varies";
                break;
        }

        return (code, composition, pressure);
    }

    private (string Type, int AverageTemp, string Description) DetermineTemperature(string atmosphere, int optionalModifier)
    {

        var baseRoll = dice.RollDiceTotal(2, 6, 0);

        // Determine atmosphere DM
        int atmDM = 0;
        var atm = string.IsNullOrEmpty(atmosphere) ? "" : atmosphere.Trim().ToUpper();

        if (atm == "0" || atm == "1")
        {
            atmDM = 0; // no modifier, but extreme swings
        }
        else if (atm == "2" || atm == "3") atmDM = -2;
        else if (atm == "4" || atm == "5" || atm == "E") atmDM = -1;
        else if (atm == "6" || atm == "7") atmDM = 0;
        else if (atm == "8" || atm == "9") atmDM = 1;
        else if (atm == "A" || atm == "D" || atm == "F") atmDM = 2;
        else if (atm == "B" || atm == "C") atmDM = 6;
        else atmDM = 0;

        var modifiedRoll = baseRoll + atmDM + optionalModifier;

        string type;
        int avgTemp;
        string desc;

        if (modifiedRoll <= 2)
        {
            type = "Frozen";
            avgTemp = -60; // -51 or less
            desc = "Frozen world. No liquid water, very dry atmosphere.";
        }
        else if (modifiedRoll <= 4)
        {
            type = "Cold";
            avgTemp = -25; // -51 to 0
            desc = "Icy world. Little liquid water, extensive ice caps, few clouds.";
        }
        else if (modifiedRoll <= 9)
        {
            type = "Temperate";
            avgTemp = 15; // 0 to 30
            desc = "Temperate world. Earth-like. Liquid and vaporised water are common, moderate ice caps.";
        }
        else if (modifiedRoll <= 11)
        {
            type = "Hot";
            avgTemp = 55; // 31 to 80
            desc = "Hot world. Small or no ice caps, little liquid water. Most water in vapor/cloud form.";
        }
        else
        {
            type = "Boiling";
            avgTemp = 90; // 81+
            desc = "Boiling world. No ice caps, little liquid water.";
        }

        // Special note for very thin atmospheres
        if (atm == "0" || atm == "1")
        {
            desc += " Temperature swings from roasting during the day to frozen at night.";
        }

        return (type, avgTemp, desc);

    }

    private (string Code, float Percentage, string Description) DetermineHydrographics(string size, string atmosphere, string temperature)
    {
        
        int sizeVal = size.CodeToInt();
        

        // Roll 2D6-7
        int roll = dice.RollDiceTotal(2, 6, -7);

        // Modifiers
        if (sizeVal == 0 || sizeVal == 1)
        {
            roll = 0;
        }
        else
        {
            var atm = string.IsNullOrEmpty(atmosphere) ? "" : atmosphere.Trim().ToUpper();
            if (atm == "0" || atm == "1" || atm == "A" || atm == "B" || atm == "C" || atm == "D" || atm == "E" || atm == "F")
                roll -= 4;

            if (!string.IsNullOrEmpty(temperature))
            {
                var temp = temperature.Trim().ToUpper();
                if (temp == "HOT") roll -= 2;
                else if (temp == "BOILING") roll -= 4;
            }
        }

        // Clamp roll to 0-A (10)
        if (roll < 0) roll = 0;
        if (roll > 10) roll = 10;

        string code = roll.RollToCode();
        float percentage = 0f;
        string description = "";

        var rand = new Random();
        switch (roll)
        {
            case 0:
                percentage = rand.Next(0, 6); // 0-5
                description = "Desert world";
                break;
            case 1:
                percentage = rand.Next(6, 16); // 6-15
                description = "Dry world";
                break;
            case 2:
                percentage = rand.Next(16, 26); // 16-25
                description = "A few small seas";
                break;
            case 3:
                percentage = rand.Next(26, 36); // 26-35
                description = "Small seas and oceans";
                break;
            case 4:
                percentage = rand.Next(36, 46); // 36-45
                description = "Wet world";
                break;
            case 5:
                percentage = rand.Next(46, 56); // 46-55
                description = "A large ocean";
                break;
            case 6:
                percentage = rand.Next(56, 66); // 56-65
                description = "Large oceans";
                break;
            case 7:
                percentage = rand.Next(66, 76); // 66-75
                description = "Earth-like world";
                break;
            case 8:
                percentage = rand.Next(76, 86); // 76-85
                description = "Only a few islands and archipelagos";
                break;
            case 9:
                percentage = rand.Next(86, 96); // 86-95
                description = "Almost entirely water";
                break;
            case 10:
                percentage = rand.Next(96, 101); // 96-100
                description = "Waterworld";
                break;
        }

        return (code, percentage, description);
    }

    private (string Code, string Inhabitants) DeterminePopulation(int optionalModifier)
    {
        // Roll 2D6-2 + optionalModifier
        int roll = dice.RollDiceTotal(2, 6, -2 + optionalModifier);
        if (roll < 0) roll = 0;
        if (roll > 12) roll = 12;

        string code = roll.RollToCode();

        string inhabitants = roll switch
        {
            0 => "None (0)",
            1 => "Few (1+) - A tiny farmstead or a single family",
            2 => "Hundreds (100+) - A village",
            3 => "Thousands (1,000+) - Small settlement",
            4 => "Tens of thousands (10,000+) - Small town",
            5 => "Hundreds of thousands (100,000+) - Average city",
            6 => "Millions (1,000,000+)",
            7 => "Tens of millions (10,000,000+) - Large city",
            8 => "Hundreds of millions (100,000,000+)",
            9 => "Billions (1,000,000,000+) - Present day Earth",
            10 => "Tens of billions (10,000,000,000+)",
            11 => "Hundreds of billions (100,000,000,000+) - Incredibly crowded world",
            12 => "Trillions (1,000,000,000,000+) - World-city",
            _ => "Unknown"
        };

        return (code, inhabitants);
    }

    private (string Code, string Government) DetermineGovernment(int optionalModifier)
    {
        // Roll 2D6-7 + optionalModifier
        int roll = dice.RollDiceTotal(2, 6, -7 + optionalModifier);
        if (roll < 0) roll = 0;
        if (roll > 15) roll = 15;

        string code = roll.RollToCode();

        string government = roll switch
        {
            0 => "None",
            1 => "Company/Corporation",
            2 => "Participating Democracy",
            3 => "Self-Perpetuating Oligarchy",
            4 => "Representative Democracy",
            5 => "Feudal Technocracy",
            6 => "Captive Government",
            7 => "Balkanization",
            8 => "Civil Service Bureaucracy",
            9 => "Impersonal Bureaucracy",
            10 => "Charismatic Dictator",
            11 => "Non-Charismatic Leader",
            12 => "Charismatic Oligarchy",
            13 => "Religious Dictatorship",
            14 => "Religious Autocracy",
            15 => "Totalitarian Oligarchy",
            _ => "Unknown"
        };

        return (code, government);
    }
}
public class World
{
    public string StarPortRating { get; set; } = string.Empty;
    public string StarPortType { get; set; } = string.Empty;

    public string SizeRating { get; set; } = string.Empty;
    public int SizeKm { get; set; }

    public string AtmosphereRating { get; set; } = string.Empty;
    public string Atmosphere { get; set; } = string.Empty;

    public string HydrogrpahicRating { get; set; } = string.Empty;
    public string Hydrographic { get; set; } = string.Empty;

    public string PopulationRating { get; set; } = string.Empty;
    public int Population { get; set; }

    public string GovernmentRating { get; set; } = string.Empty;
    public string Government { get; set; } = string.Empty;

    public string LawLevelRating { get; set; } = string.Empty;
    public string LawLevel { get; set; } = string.Empty;

    public string TechLevelRating { get; set; } = string.Empty;
    public string TechLevel { get; set; } = string.Empty;

    //         public string Bases{ get; set; } = string.Empty;
    //         public string TravelCode{ get; set; } = string.Empty;
    //         public string TradesCodes{ get; set; } = string.Empty;
    //         public string System{ get; set; } = string.Empty;
    //         public string SystemSubType {get;set; } = "Class V";
    //         public string Allegiances{ get; set; } = string.Empty;
    //         public string Domain{ get; set; } = string.Empty;

    //         public string PortType{ get; set; } = string.Empty;
    //         public string BerthingCost{ get; set; } = string.Empty;
    //         public string Fuel{ get; set; } = string.Empty;
    //         public string Facilities{ get; set; } = string.Empty;
    //         public string Extras{ get; set; } = string.Empty;
    //         public HashSet<string> Tags = new();
}
