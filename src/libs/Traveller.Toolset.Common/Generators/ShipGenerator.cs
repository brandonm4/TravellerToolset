namespace Traveller.Toolset.Generators;

public class ShipGenerator
{
    DiceRoller dice = new DiceRoller();
    private static readonly Random sysRandom = new();

    public record Ship(
        string Name,
        string Type,
        int HullTons,
        int Jump,
        int Maneuver,
        int PowerPlant,
        int Crew,
        int Passengers,
        int CargoTons,
        int FuelTons,
        long PriceCr,
        string Notes
    );

    public class ShipGeneratorArgs
    {
        // Optional role to bias generation: "Trader","Scout","Warship","Passenger","Luxury"
        public string? Role { get; set; }
    }

    public Ship GenerateShip(ShipGeneratorArgs? args = null)
    {
        args ??= new ShipGeneratorArgs();

        // Roll a hull template index: 2D6-2 -> range roughly 0..10
        int hullRoll = dice.RollDiceTotal(2, 6, -2);
        var templates = GetHullTemplates();

        // Clamp to template bounds
        hullRoll = Math.Clamp(hullRoll, 0, templates.Count - 1);
        var template = templates[hullRoll];

        // Determine jump: base roll 2D6-7 (range -5..5) + size modifier
        int sizeMod = template.HullTons switch
        {
            <= 50 => -2,
            <= 200 => -1,
            <= 800 => 0,
            <= 3200 => 1,
            _ => 2
        };
        int jump = dice.RollDiceTotal(2, 6, -7) + sizeMod;
        if (jump < 1) jump = 1;
        if (jump > 6) jump = 6;

        // Maneuver: 1D6-3 clamped 0..6
        int maneuver = dice.RollDiceTotal(1, 6, -3);
        maneuver = Math.Clamp(maneuver, 0, 6);

        // Power plant: scaled to hull & jump (simple approximation)
        int powerPlant = Math.Max(1, (int)Math.Ceiling(template.HullTons / 200.0)) * jump;

        // Fuel (tons) - rough: base fuel = jump * 10, scaled by hull size
        int fuelTons = Math.Max(0, jump * Math.Max(1, template.HullTons / 200));

        // Crew: base crew = hullTons / 200 rounded + small random
        int crew = Math.Max(1, (template.HullTons / 200) + dice.RollDiceTotal(1, 6, -3));

        // Passengers: small randomized factor influenced by role
        int passengerMultiplier = args.Role?.ToLower() switch
        {
            "passenger" or "liner" or "luxury" => 2,
            "scout" => 0,
            "warship" => 0,
            _ => 1
        };
        int passengers = Math.Max(0, template.HullTons / 100 * Math.Clamp(dice.RollDiceTotal(1, 6, -3), 0, 4) * passengerMultiplier);

        // Cargo: heuristic: 50-70% of hull space available for cargo depending on ship type
        double cargoRatio = template.DefaultCargoRatio;
        int cargoTons = Math.Max(0, (int)Math.Round(template.HullTons * cargoRatio));

        // Price: base price per ton + jump & powerplant premiums (very approximate)
        long priceCr = template.BasePricePerTon * template.HullTons;
        priceCr += jump * 50_000;
        priceCr += powerPlant * 20_000;
        // adjust for role: traders slightly cheaper per ton, liners more expensive
        if (args.Role?.ToLower() == "trader") priceCr = (long)(priceCr * 0.95);
        if (args.Role?.ToLower() == "luxury") priceCr = (long)(priceCr * 1.25);

        // Name generator
        string name = GenerateName(template.Type);

        // Notes summarising generator choices
        string notes = $"{template.Type} template used. Role bias: {args.Role ?? "None"}.";

        return new Ship(
            Name: name,
            Type: template.Type,
            HullTons: template.HullTons,
            Jump: jump,
            Maneuver: maneuver,
            PowerPlant: powerPlant,
            Crew: crew,
            Passengers: passengers,
            CargoTons: cargoTons,
            FuelTons: fuelTons,
            PriceCr: priceCr,
            Notes: notes
        );
    }

    private static string GenerateName(string type)
    {
        // Simple procedural name generator: Prefix + Number/Suffix
        var prefixes = new[]
        {
            "Nova","Aurora","Starlight","Endeavour","Venture","Mercury","Orion","Sagitta","Pioneer","Galileo","Aquila","Daedalus","Corsair"
        };
        var suffixes = new[]
        {
            "I","II","III","Prime","Explorer","Runner","Serpent","Horizon","Spirit","Wing","Reach","Voyager"
        };

        var p = prefixes[sysRandom.Next(prefixes.Length)];
        var s = suffixes[sysRandom.Next(suffixes.Length)];
        var num = sysRandom.Next(1, 9999);
        return $"{p} {s}-{num}";
    }

    private List<(string Type, int HullTons, int BasePricePerTon, double DefaultCargoRatio)> GetHullTemplates()
    {
        // Ordered from smallest to largest; values chosen to be plausible for MGT2-like ships.
        return new List<(string, int, int, double)>
        {
            ("Shuttle", 10, 1000, 0.30),
            ("Launch", 20, 900, 0.25),
            ("Light Scout", 50, 800, 0.30),
            ("Light Freighter", 100, 700, 0.55),
            ("Small Trader", 200, 650, 0.60),
            ("Trader", 400, 600, 0.62),
            ("Free Trader", 800, 550, 0.60),
            ("Small Liner", 1600, 520, 0.50),
            ("Liner", 3200, 480, 0.45),
            ("Cruiser", 6400, 450, 0.30),
            ("Battleship", 10000, 420, 0.20)
        };
    }
}