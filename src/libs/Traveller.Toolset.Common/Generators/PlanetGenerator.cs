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
        var law = DetermineLawLevel(government.Code);
        var starport = DetermineStarPort(population.Code);
        var tech = DetermineTechLevel(starport.Code, size.Code, atmosphere.Code, hydro.Code, population.Code, government.Code);
        var travel = DetermineTravelCodes(population.Code, law.Code, government.Code, starport.Code);
        var trade = DetermineTradeCodes(size.Code, atmosphere.Code, hydro.Code, population.Code, government.Code, law.Code, tech.Code);

        world.SizeRating = size.Code;
        world.AtmosphereRating = atmosphere.Code;
        world.HydrogrpahicRating = hydro.Code;
        world.PopulationRating = population.Code;
        world.GovernmentRating = government.Code;
        world.LawLevelRating = law.Code;
        world.StarPortRating = starport.Code;
        world.TechLevelRating = tech.Code;

        return world;
    }

    public (string Code, string Quality, string BerthingCost, string Fuel, string Facilities, string Bases) DetermineStarPort(string populationCode)
    {
        var pop = populationCode.CodeToInt();
        var modifier = 0;
        modifier = pop switch
        {
            0 or 1 or 2 => -2,
            3 or 4 => -1,
            5 or 6 or 7 => 0,
            8 or 9 => 1,
            >= 10 => 2,
            _ => 0
        };

        var roll = dice.RollDiceTotal(2, 6, 0) + modifier;
        string code, quality, fuel, facilities, bases;
        switch (roll)
        {
            case <= 2: code = "X"; break;
            case <= 4: code = "E"; break;
            case <= 6: code = "D"; break;
            case <= 8: code = "C"; break;
            case <= 10: code = "B"; break;
            case >= 11: code = "A"; break;
        }

        // Map code -> quality, berthing cost multiplier, fuel & facilities
        int berthRoll;
        int berthCostValue = 0;
        string berthCostString;
        switch (code)
        {
            case "A":
                quality = "Excellent";
                berthRoll = dice.RollDiceTotal(1, 6, 0);
                berthCostValue = berthRoll * 1000;
                fuel = "Refined";
                facilities = "Refined Shipyard (all), Repair";
                break;
            case "B":
                quality = "Good";
                berthRoll = dice.RollDiceTotal(1, 6, 0);
                berthCostValue = berthRoll * 500;
                fuel = "Refined";
                facilities = "Refined Shipyard (spacecraft), Repair";
                break;
            case "C":
                quality = "Routine";
                berthRoll = dice.RollDiceTotal(1, 6, 0);
                berthCostValue = berthRoll * 100;
                fuel = "Unrefined";
                facilities = "Unrefined Shipyard (small craft), Repair";
                break;
            case "D":
                quality = "Poor";
                berthRoll = dice.RollDiceTotal(1, 6, 0);
                berthCostValue = berthRoll * 10;
                fuel = "Unrefined";
                facilities = "Unrefined Limited Repair";
                break;
            case "E":
                quality = "Frontier";
                berthCostValue = 0;
                fuel = "None";
                facilities = "None";
                break;
            case "X":
            default:
                quality = "No Starport";
                berthCostValue = 0;
                fuel = "None";
                facilities = "None";
                break;
        }

        berthCostString = berthCostValue > 0 ? $"Cr{berthCostValue}" : "None";

        // Roll for bases (2D6) and determine which are present based on thresholds from the chart
        var baseRoll = dice.RollDiceTotal(2, 6, 0);
        var baseList = new List<string>();

        // thresholds per code (inclusive)
        // A: Highport 6+, Military 8+, Naval 8+, Scout 10+
        // B: Highport 8+, Military 8+, Naval 8+, Scout 9+
        // C: Highport 10+, Military 10+, Scout 9+
        // D: Highport 12+, Scout 8+, Corsair 12+
        // E/X: Corsair 10+
        if (code == "A")
        {
            if (baseRoll >= 6) facilities += ", Highport";
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 8) baseList.Add("Military");
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 8) baseList.Add("Naval");
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 10) baseList.Add("Scout");
        }
        else if (code == "B")
        {
            if (baseRoll >= 8) facilities += ", Highport";
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 8) baseList.Add("Military");
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 8) baseList.Add("Naval");
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 9) baseList.Add("Scout");
        }
        else if (code == "C")
        {
            if (baseRoll >= 10) facilities += ", Highport";
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 10) baseList.Add("Military");
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 9) baseList.Add("Scout");
        }
        else if (code == "D")
        {
            if (baseRoll >= 12) facilities += ", Highport";
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 8) baseList.Add("Scout");
            baseRoll = dice.RollDiceTotal(2, 6, 0);
            if (baseRoll >= 12) baseList.Add("Corsair");
        }
        else // E or X
        {
            if (baseRoll >= 10) baseList.Add("Corsair");
        }

        bases = baseList.Count > 0 ? string.Join(", ", baseList) : "None";

        return (code, quality, berthCostString, fuel, facilities, bases);
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
            1 => "Few",
            2 => "Hundreds",
            3 => "Thousands",
            4 => "Tens of thousands",
            5 => "Hundreds of thousands",
            6 => "Millions",
            7 => "Tens of millions",
            8 => "Hundreds of millions",
            9 => "Billions",
            10 => "Tens of billions",
            11 => "Hundreds of billions",
            12 => "Trillions",
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

    private (string Code, string WeaponsBanned, string ArmourBanned) DetermineLawLevel(string governmentCode)
    {
        // Roll 2D6-7 + government modifier
        int roll = dice.RollDiceTotal(2, 6, -7) + governmentCode.CodeToInt();

        // Clamp roll to sensible range (0 - 15)
        if (roll < 0) roll = 0;
        if (roll > 15) roll = 15;

        string code = roll.RollToCode();

        // Map law level to banned weapons and armour
        string weaponsBanned;
        string armourBanned;

        if (roll <= 0)
        {
            weaponsBanned = "None";
            armourBanned = "None";
        }
        else if (roll == 1)
        {
            weaponsBanned = "Poison gas, explosives, undetectable weapons, WMD";
            armourBanned = "Battle dress";
        }
        else if (roll == 2)
        {
            weaponsBanned = "Portable energy and laser weapons";
            armourBanned = "Combat armour";
        }
        else if (roll == 3)
        {
            weaponsBanned = "Military weapons";
            armourBanned = "Flak";
        }
        else if (roll == 4)
        {
            weaponsBanned = "Light assault weapons and submachine guns";
            armourBanned = "Cloth";
        }
        else if (roll == 5)
        {
            weaponsBanned = "Personal concealable weapons";
            armourBanned = "Mesh";
        }
        else if (roll == 6)
        {
            weaponsBanned = "All firearms except shotguns & stunners; carrying weapons discouraged";
            armourBanned = "None";
        }
        else if (roll == 7)
        {
            weaponsBanned = "Shotguns";
            armourBanned = "None";
        }
        else if (roll == 8)
        {
            weaponsBanned = "All bladed weapons, stunners";
            armourBanned = "All visible armour";
        }
        else // 9 and up
        {
            weaponsBanned = "All weapons";
            armourBanned = "All armour";
        }

        return (code, weaponsBanned, armourBanned);
    }

    private (string Code, string Description) DetermineTechLevel(string starportCode, string sizeCode, string atmosphereCode, string hydrographicsCode, string populationCode, string governmentCode)
    {
        // Base roll 1d6
        int tech = dice.RollDiceTotal(1, 6, 0);
        int modifier = 0;

        // Starport modifier
        var sp = string.IsNullOrEmpty(starportCode) ? "" : starportCode.Trim().ToUpper();
        if (sp == "A") modifier += 6;
        else if (sp == "B") modifier += 4;
        else if (sp == "C") modifier += 2;
        else if (sp == "X") modifier -= 4;

        // Size modifier
        int size = sizeCode.CodeToInt();
        if (size == 0 || size == 1) modifier += 2;
        else if (size >= 2 && size <= 4) modifier += 1;

        // Atmosphere modifier (0,1,2,3,10-15 => +1)
        int atm = atmosphereCode.CodeToInt();
        if (atm == 0 || atm == 1 || atm == 2 || atm == 3 || (atm >= 10 && atm <= 15)) modifier += 1;

        // Hydrographics modifier (0 or 9 => +1, 10 => +2)
        int hydro = hydrographicsCode.CodeToInt();
        if (hydro == 0 || hydro == 9) modifier += 1;
        else if (hydro == 10) modifier += 2;

        // Population modifier (1-5,8 => +1; 9 => +2; 10 => +2)
        int pop = populationCode.CodeToInt();
        if ((pop >= 1 && pop <= 5) || pop == 8) modifier += 1;
        if (pop == 9 || pop == 10) modifier += 2;

        // Government modifier (0,5 => +1; 7 => +2; 13,14 => -2)
        int gov = governmentCode.CodeToInt();
        if (gov == 0 || gov == 5) modifier += 1;
        if (gov == 7) modifier += 2;
        if (gov == 13 || gov == 14) modifier -= 2;

        tech += modifier;

        // Clamp to sensible code range 0..15
        if (tech < 0) tech = 0;
        if (tech > 15) tech = 15;

        string code = tech.RollToCode();
        string description = $"Tech Level {code} (TL {tech})";

        return (code, description);
    }
    private (string Codes, string Description) DetermineTravelCodes(string populationCode, string lawLevelCode, string governmentCode, string starportCode)
    {
        // Simple travel recommendation codes based on population, law and government.
        // Returns short code list and a human readable description.
        var pop = populationCode.CodeToInt();
        var law = lawLevelCode.CodeToInt();
        var gov = governmentCode.CodeToInt();
        var sp = string.IsNullOrEmpty(starportCode) ? "" : starportCode.Trim().ToUpper();

        var codes = new List<string>();
        var notes = new List<string>();

        if (pop <= 0)
        {
            codes.Add("Uninhabited");
            notes.Add("No permanent population — travel usually unnecessary.");
            return (string.Join(", ", codes), string.Join(" ", notes));
        }

        // Law-driven restrictions
        if (law >= 9)
        {
            codes.Add("Closed");
            notes.Add("High law level — heavy restrictions, travel mostly prohibited.");
        }
        else if (law >= 6)
        {
            codes.Add("Restricted");
            notes.Add("Moderate law level — travel possible with permits; caution advised.");
        }
        else
        {
            codes.Add("Open");
            notes.Add("Low law level — travel generally allowed.");
        }

        // Government instability / danger
        if (gov == 7 || gov == 13 || gov == 14 || gov == 15)
        {
            codes.Add("Unstable");
            notes.Add("Political instability or harsh regime — expect checkpoints and possible danger.");
        }

        // Starport availability influences access
        if (sp == "X")
        {
            codes.Add("DifficultAccess");
            notes.Add("No starport — requires specialist transport or orbital transfer.");
        }
        else if (sp == "E" || sp == "D")
        {
            codes.Add("LimitedAccess");
            notes.Add("Limited starport facilities; reduced scheduled traffic.");
        }
        else
        {
            codes.Add("GoodAccess");
            notes.Add("Starport supports normal traffic.");
        }

        return (string.Join(", ", codes), string.Join(" ", notes));
    }

    private string DetermineTradeCodes(string sizeCode, string atmosphereCode, string hydrographicsCode, string populationCode, string governmentCode, string lawLevelCode, string techLevelCode)
    {
        // Build trade classification codes using commonly used Traveller heuristics.
        // Uses CodeToInt extension to interpret hex-like UWP codes.
        var size = sizeCode.CodeToInt();
        var atm = atmosphereCode.CodeToInt();
        var hydro = hydrographicsCode.CodeToInt();
        var pop = populationCode.CodeToInt();
        var gov = governmentCode.CodeToInt();
        var law = lawLevelCode.CodeToInt();
        var tech = techLevelCode.CodeToInt();

        var codes = new List<string>();

        // Barren / Asteroid / Vacuum
        if (pop == 0)
        {
            if (size == 0 && atm == 0 && hydro == 0) codes.Add("As"); // Asteroid
            else if (gov == 0 && law == 0) codes.Add("Ba"); // Barren
        }

        // Agricultural (Ag) — needs moderate atmosphere/hydro and moderate population
        if (pop >= 5 && pop <= 7 && atm >= 4 && atm <= 9 && hydro >= 4 && hydro <= 8)
            codes.Add("Ag");

        // Desert (De) — very low hydrographics but inhabited
        if (hydro == 0 && pop > 0 && atm >= 2 && atm <= 9)
            codes.Add("De");

        // Fluid Oceans / Waterworld (Fl)
        if (hydro >= 1 && atm >= 10)
            codes.Add("Fl");

        //Garden (Ga)
        if (size >= 6 && size <= 8 && new List<int> { 5, 6, 8 }.Contains(atm) && hydro >= 5 && hydro <= 7)
            codes.Add("Ga");

        //High Population(Hi)
        if (pop >= 9)
            codes.Add("Hi");

        //High Tech
        if (tech >= 12)
            codes.Add("Ht");

        // Ice-capped (Ic) — very low temperature is not available here, approximate by low atm/hydro with population
        if ((atm == 0 || atm == 1) && hydro == 0)
            codes.Add("Ic");

        // Industrial (In) — high population and production capability
        if (new List<int> { 0, 1, 2, 4, 7, 9, 10, 11, 12 }.Contains(atm) && pop >= 9)
            codes.Add("In");

        // Low population (Lo)
        if (pop >= 1 && pop <= 3)
            codes.Add("Lo");

        // Low Tech (Lt)
        if (pop >= 1 && tech <= 5)
            codes.Add("Lt");

        //Non-Agraculture(Na)
        if (atm >= 0 && atm <= 3 && hydro >= 0 && hydro <= 3 && pop >= 6)
            codes.Add("Na");

        // Non-Industrial (Ni) — low tech / low production worlds
        if (pop >= 4 && pop <= 6)
            codes.Add("Ni");

        // Poor (Po) — limited resources / low hydro/atm
        if (atm >= 3 && atm <= 5 && hydro <= 3)
            codes.Add("Po");

        // Rich (Ri) — moderate population, good hydro and favourable government/law
        if ((atm == 6 || atm == 8) && pop >= 6 && pop <= 8 && gov >= 4 && gov <= 9)
            codes.Add("Ri");

        // Vacuum (Va)
        if (atm == 0)
            codes.Add("Va");

        //Waterworld (Wa)
        if (((atm >= 3 && atm <= 9) || atm >= 13) && hydro >= 10)
            codes.Add("Wa");








        return string.Join(", ", codes.Distinct());
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
