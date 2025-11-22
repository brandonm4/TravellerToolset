using System;

namespace Traveller.Toolset.Data;

public partial class TravellerDb
{
private readonly static List<Skill> _skills = new() {
    new Skill("Admin", "This skill covers bureaucracies and administration of all sorts, including the navigation of bureaucratic obstacles or disasters. It also covers tracking inventories, ship manifests and other records.", new List<string> {
        "Avoiding Close Examination of Papers: Average (8+) Admin check (1D x 10 seconds, EDU or SOC).",
        "Dealing with Police Harassment: Difficult (10+) Admin check (1D x 10 minutes, EDU or SOC)."
    }, new()),
    new Skill("Advocate", "Advocate gives a knowledge of common legal codes and practises, especially interstellar law. It also gives the Traveller experience in oratory, debate and public speaking, making it an excellent skill for lawyers and politicians.", new List<string> {
        "Arguing in Court: Opposed Advocate check (1D days, EDU or SOC).",
        "Debating an Argument: Opposed Advocate check (1D x 10 minutes, INT)."
    }, new()),
    new Skill("Animals", "This skill, rare on industrialised or technologically advanced worlds, is for the care of animals.", new(), new() {
        new Skill("Handling", "The Traveller knows how to handle an animal and ride those trained to bear a rider. Unusual animals raise the difficulty of the check.", new List<string> {
            "Riding a Horse into Battle: Difficult (10+) Animals (handling) check (1D seconds, DEX). If successful, the Traveller can control the horse for a number of minutes equal to the Effect before needing to make another check."
        }, new()),
        new Skill("Veterinary", "The Traveller is trained in veterinary medicine and animal care.", new List<string> {
            "Applying Medical Care: See the Medic skill on page 69, but use the Animals (veterinary) skill."
        }, new()),
        new Skill("Training", "The Traveller knows how to tame and train animals.", new List<string> {
            "Taming a Strange Alien Creature: Formidable (14+) Animals (training) check (1D days, INT)."
        }, new())
    }),
    new Skill("Art", "The Traveller is trained in a type of creative art.", new(), new() {
        new Skill("Performer", "The Traveller is a trained actor, dancer or singer at home on the stage, screen or holo.", new List<string> {
            "Performing a Play: Average (8+) Art (performer) check (1D hours, EDU).",
            "Convincing a Person you are Actually Someone Else: Art (performer) check (INT) opposed by Recon check (INT)."
        }, new()),
        new Skill("Holography", "Recording and producing aesthetically pleasing and clear holographic images.", new List<string> {
            "Surreptitiously Switching on Your Recorder While in a Secret Meeting: Formidable (14+) Art (holography) check (1D seconds, DEX)."
        }, new()),
        new Skill("Instrument", "Playing a particular musical instrument, such a flute, piano or organ.", new List<string> {
            "Playing a Concerto: Difficult (10+) Art (instrument) check (1D x 10 minutes, EDU)."
        }, new()),
        new Skill("Visual Media", "Making artistic or abstract paintings or sculptures in a variety of media.", new List<string> {
            "Making a Statue of Someone: Difficult (10+) Art (visual media) check (1D days, INT)."
        }, new()),
        new Skill("Write", "Composing inspiring or interesting pieces of text.", new List<string> {
            "Rousing the People of a Planet by Exposing Their Government's Corruption: Difficult (10+) Art (write) check (1D hours, INT or EDU).",
            "Writing the Update of Traveller: Formidable (14+) Art (write) check (1D months, INT)."
        }, new())
    }),
    new Skill("Astrogation", "This skill is for plotting the courses of starships and calculating accurate jumps. See the Spacecraft Operations chapter.", new List<string> {
        "Plotting Course to a Target World Using a Gas Giant for a Gravity Slingshot: Difficult (10+) Astrogation check (1D x 10 minutes, EDU).",
        "Plotting a Standard Jump: Easy (4+) Astrogation check (1D x 10 minutes, EDU), with DM- equal to the Jump distance."
    }, new()),
    new Skill("Athletics", "The Traveller is a trained athlete and is physically fit. The Athletics skill effectively augments a Traveller's physical characteristics; whatever you can do with Strength alone you can also add your Athletics (strength) DM to, for example. Athletics is also the principal skill used in adverse gravitational environments, specifically Athletics (dexterity) in low or zero-G and Athletics (strength) in high-G locations.", new(), new() {
        new Skill("Dexterity", "Climbing, Juggling, Throwing. For alien species with wings, this also includes flying.", new List<string> {
            "Climbing: Difficulty varies. Athletics (dexterity) check (1D x 10 seconds, DEX). So long as they succeed, the Traveller's Effect is usually irrelevant unless they are trying to do something while climbing, in which case the climbing is part of a task chain or multiple action.",
            "Sprinting: Average (8+) Athletics (dexterity) check (1D seconds, DEX). If the Traveller does nothing but sprint flat out they can cover 24 + Effect metres with every check. Avoiding obstacles while sprinting requires another Athletics (dexterity) check (Difficult, because they are performing a multiple action).",
            "High Jumping: Average (8+) Athletics (dexterity) check (1D seconds, DEX). The Traveller jumps a number of metres straight up equal to the Effect halved.",
            "Long Jumping: Average (8+) Athletics (dexterity) check (1D seconds, DEX). The Traveller jumps a number of metres forward equal to the Effect with a running start.",
            "Righting Yourself When Artificial Gravity Suddenly Fails on Board a Ship: Average (8+) Athletics (dexterity) check (1D seconds, DEX)."
        }, new()),
        new Skill("Endurance", "Long-distance running, hiking.", new List<string> {
            "Long-distance Running: Average (8+) Athletics (endurance) check (1D x 10 minutes, END).",
            "Long-distance Swimming: Average (8+) Athletics (endurance) check (1D x 10 minutes, END)."
        }, new()),
        new Skill("Strength", "Feats of strength, weight-lifting.", new List<string> {
            "Arm Wrestling: Opposed Athletics (strength) check (1D minutes, STR).",
            "Feats of Strength: Average (8+) Athletics (strength) check (1D x 10 seconds, STR).",
            "Performing a Complicated Task in a High Gravity Environment: Difficult (10+) Athletics (strength) check (1D seconds, STR)."
        }, new())
    }),
    new Skill("Broker", "The Broker skill allows a Traveller to negotiate trades and arrange fair deals. It is heavily used when trading (see the Trade chapter).", new List<string> {
        "Negotiating a Deal: Average (8+) Broker check (1D hours, INT).",
        "Finding a Buyer: Average (8+) Broker check (1D hours, SOC)."
    }, new()),
    new Skill("Carouse", "Carousing is the art of socialising; having fun, but also ensuring other people have fun, and infectious good humour. It also covers social awareness and subterfuge in such situations.", new List<string> {
        "Drinking Someone Under the Table: Opposed Carouse check (1D hours, END). Difficulty varies by liquor.",
        "Gathering Rumours at a Party: Average (8+) Carouse check (1D hours, SOC)."
    }, new()),
    new Skill("Deception", "Deception allows a Traveller to lie fluently, disguise themselves, perform sleight of hand and fool onlookers. Most underhanded ways of cheating and lying fall under deception.", new List<string> {
        "Convincing a Guard to let you Past Without ID: Very Difficult (12+) Deception check (1D minutes, INT). Alternatively, oppose with a Recon check.",
        "Palming a Credit Chit: Average (8+) Deception check (1D seconds, DEX).",
        "Disguising Yourself as a Wealthy Noble to Fool a Client: Difficult (10+) Deception check (1D x 10 minutes, INT or SOC). Alternatively, oppose with a Recon check."
    }, new()),
    new Skill("Diplomat", "The Diplomat skill is for negotiating deals, establishing peaceful contact and smoothing over social faux pas. It includes how to behave in high society and proper ways to address nobles. It is a much more formal skill than Persuade.", new List<string> {
        "Greeting the Emperor Properly: Difficult (10+) Diplomat check (1D minutes, SOC).",
        "Negotiating a Peace Treaty: Average (8+) Diplomat check (1D days, EDU).",
        "Transmitting a Formal Surrender: Average (8+) Diplomat check (1D x 10 seconds, INT)."
    }, new()),
    new Skill("Drive", "This skill is for controlling ground vehicles of various types. There are several specialities.", new(), new() {
        new Skill("Hovercraft", "Vehicles that rely on a cushion of air and thrusters for motion.", new List<string> {
            "Manoeuvring a Hovercraft Through a Tight Canal: Difficult (10+) Drive (hovercraft) check (1D minutes, DEX)."
        }, new()),
        new Skill("Mole", "For controlling vehicles that move through solid matter using drills or other earth-moving technologies, such as plasma torches or cavitation.", new List<string> {
            "Surfacing in the Right Place: Average (8+) Drive (mole) check (1D x 10 minutes, INT).",
            "Precisely Controlling a Dig to Expose a Vein of Minerals: Difficult (10+) Drive (mole) check (1D x 10 minutes, DEX)."
        }, new()),
        new Skill("Track", "For tanks and other vehicles that move on tracks.", new List<string> {
            "Manoeuvring (or Smashing, Depending on the Vehicle) Through a Forest: Difficult (10+) Drive (tracked) check (1D minutes, DEX).",
            "Driving a Tank into a Cargo Bay: Average (8+) Drive (tracked) check (1D x 10 seconds, DEX)."
        }, new()),
        new Skill("Walker", "Vehicles that use two or more legs to manoeuvre.", new List<string> {
            "Negotiating Rough Terrain: Difficult (10+) Drive (walker) check (1D minutes, DEX)."
        }, new()),
        new Skill("Wheel", "For automobiles and similar groundcars.", new List<string> {
            "Driving a Groundcar in a Short Race: Opposed Drive (wheeled) check (1D minutes, DEX). Longer races might use END instead of DEX.",
            "Avoiding an Unexpected Obstacle on the Road: Average (8+) Drive (wheeled) check (1D seconds, DEX)."
        }, new())
    }),
    new Skill("Electronics", "This skill is used to operate electronic devices such as computers and ship-board systems. Higher levels represent the ability to repair and create electronic devices and systems. There are several specialities.", new(), new() {
        new Skill("Comms", "The use of modern telecommunications; opening communications channels, querying computer networks, jamming signals and so on, as well as the proper protocols for communicating with starports and other spacecraft.", new List<string> {
            "Requesting Landing Privileges at a Starport: Routine (6+) Electronic (comms) check (1D minutes, EDU).",
            "Accessing Publicly Available but Obscure Data Over Comms: Average (8+) Electronic (comms) check (1D x 10 minutes, EDU).",
            "Bouncing a Signal off Orbiting Satellite to Hide Your Transmitter: Difficult (10+) Electronics (comms) check (1D x 10 minutes, INT).",
            "Jamming a Comms System: Opposed Electronics (comms) check (1D minutes, INT). Difficult (10+) for radio, Very Difficult (12+) for laser and Formidable (14+) for masers. A Traveller using a comms system with a higher Technology Level than their opponent gains DM+1 for every TL of difference."
        }, new()),
        new Skill("Computers", "Using and controlling computer systems and similar electronics and electrics.", new List<string> {
            "Accessing Publicly Available Data: Easy (4+) Electronics (computers) check (1D minutes, INT or EDU).",
            "Activating a Computer Program on a Ship's Computer: Routine (6+) Electronics (computers) check (1D x 10 seconds, INT or EDU).",
            "Searching a Corporate Database for Evidence of Illegal Activity: Difficult (10+) Electronics (computers) check (1D hours, INT).",
            "Hacking into a Secure Computer Network: Formidable (14+) Electronics (computers) check (1D x 10 hours, INT). Hacking is aided by Intrusion programs and made more difficult by Security programs. The Effect determines the amount of data retrieved; failure means the targeted system may be able to trace the hacking attempt."
        }, new()),
        new Skill("Remote Ops", "Using telepresence to remotely control drones, missiles, robots and other devices.", new List<string> {
            "Using a Mining Drone to Excavate an Asteroid: Routine (6+) Electronics (remote ops) check (1D hours, DEX)."
        }, new()),
        new Skill("Sensors", "The use and interpretation of data from electronic sensor devices, from observation satellites and remote probes to thermal imaging and densitometers.", new List<string> {
            "Making a Detailed Sensor Scan: Routine (6+) Electronics (sensors) check (1D x 10 minutes, INT or EDU).",
            "Analysing Sensor Data: Average (8+) Electronics (sensors) check (1D hours, INT)."
        }, new())
    }),
    new Skill("Explosives", "The Explosives skill covers the use of demolition charges and other explosive devices, including assembling or disarming bombs. A failed Explosives check with an Effect of -4 or less can result in a bomb detonating prematurely.", new List<string> {
        "Planting Charges to Collapse a Wall in a Building: Average (8+) Explosives check (1D x 10 minutes, EDU).",
        "Planting a Breaching Charge: Average (8+) Explosives check (1D x 10 seconds, EDU). The damage from the explosive is multiplied by the Effect.",
        "Disarming a Bomb Equipped with Anti-Tamper Trembler Detonators: Formidable (14+) Explosives check (1D minutes, DEX)."
    }, new()),
    new Skill("Flyer", "The various specialities of this skill cover different types of flying vehicles. Flyers only work in an atmosphere; vehicles that can leave the atmosphere and enter orbit generally use the Pilot skill.", new List<string> {
        "Landing Safely: Routine (6+) Flyer check (1D minutes, DEX).",
        "Racing Another Flyer: Opposed Flyer check (1D x 10 minutes, DEX)."
    }, new() {
        new Skill("Airship", "Used for airships, dirigibles and other powered lighter than air craft.", new(), new()),
        new Skill("Grav", "This covers air/rafts, grav belts and other vehicles that use gravitic technology.", new(), new()),
        new Skill("Ornithopter", "For vehicles that fly through the use of flapping wings.", new(), new()),
        new Skill("Rotor", "For helicopters, tilt-rotors and aerodynes.", new(), new()),
        new Skill("Wing", "For jets, vectored thrust aircraft and aeroplanes using a lifting body.", new(), new())
    }),
    new Skill("Gunner", "The various specialities of this skill deal with the operation of ship-mounted weapons in space combat. Most Travellers have smaller ships equipped solely with turret weapons.", new(), new() {
        new Skill("Turret", "Operating turret-mounted weapons on board a ship.", new List<string> {
            "Firing a Turret at an Enemy Ship: Average (8+) Gunner (turret) check (1D seconds, DEX)."
        }, new()),
        new Skill("Ortillery", "A contraction of Orbital Artillery – using a ship's weapons for planetary bombardment or attacks on stationary targets.", new List<string> {
            "Firing Ortillery: Average (8+) Gunner (ortillery) check (1D minutes, INT)."
        }, new()),
        new Skill("Screen", "Activating and using a ship's energy screens like Black Globe generators or meson screens.", new List<string> {
            "Activating a Screen to Intercept Enemy Fire: Difficult (10+) Gunner (screen) check (1D seconds, DEX)."
        }, new()),
        new Skill("Capital", "Operating bay or spinal mount weapons on board a ship.", new List<string> {
            "Firing a Spinal Mount Weapon: Average (8+) Gunner (capital) check (1D minutes, INT)."
        }, new())
    }),
    new Skill("Gun Combat", "The Gun Combat skill covers a variety of ranged weapons. See the Combat chapter for details on using guns in combat.", new List<string> {
        "Firing a Gun: Average (8+) Gun Combat check (1D seconds, DEX)."
    }, new() {
        new Skill("Archaic", "For primitive weapons that are not thrown, such as bows and blowpipes.", new(), new()),
        new Skill("Energy", "Using advanced energy weapons like laser pistols or plasma rifles.", new(), new()),
        new Skill("Slug", "Weapons that fire a solid projectile such as the autorifle or gauss rifle.", new(), new())
    }),
    new Skill("Gambler", "The Traveller is familiar with a wide variety of gambling games, such as poker, roulette, blackjack, horse-racing, sports betting and so on, and has an excellent grasp of statistics and probability. Gambler increases the rewards from Benefit rolls, giving the Traveller DM+1 to their cash rolls if they have Gambler 1 or better.", new List<string> {
        "A Casual Game of Poker: Opposed Gambler check (1D hours, INT).",
        "Picking the Right Horse to Bet On: Average (8+) Gambler check (1D minutes, INT)."
    }, new()),
    new Skill("Heavy Weapons", "The Heavy Weapons skill covers portable and larger weapons that cause extreme property damage, such as rocket launchers, artillery and large plasma weapons.", new List<string> {
        "Firing an Artillery Piece at a Visible Target: Average (8+) Heavy Weapons (artillery) check (1D seconds, DEX).",
        "Firing an Artillery Piece Using Indirect Fire: Difficult (10+) Heavy Weapons (artillery) check (1D x 10 seconds, INT)."
    }, new() {
        new Skill("Artillery", "Fixed guns, mortars and other indirect-fire weapons.", new(), new()),
        new Skill("Portable", "Missile launchers, flamethrowers and portable fusion and plasma guns.", new(), new()),
        new Skill("Vehicle", "Large weapons typically mounted on vehicles or strongpoints such as tank guns and autocannon.", new(), new())
    }),
    new Skill("Investigate", "The Investigate skill incorporates keen observation, forensics and detailed analysis.", new List<string> {
        "Searching a Crime Scene For Clues: Average (8+) Investigate check (1D x 10 minutes, INT).",
        "Watching a Bank of Security Monitors in a Starport, Waiting for a Specific Criminal: Difficult (10+) Investigate check (1D hours, INT)."
    }, new()),
    new Skill("Jack-of-All-Trades", "The Jack-of-All-Trades skill works differently to other skills. It reduces the unskilled penalty a Traveller receives for not having the appropriate skill by one for every level of Jack-of-All-Trades. For example, if a Traveller does not have the Pilot skill, they suffer DM-3 to all Pilot checks. If that Traveller has Jack-of-All-Trades 2, then the penalty is reduced by 2 to DM-1. With Jack-of-All-Trades 3, a Traveller can totally negate the penalty for being unskilled. There is no benefit for having Jack-of-All-Trades 0 or Jack-of-All-Trades 4 or higher.", new(), new()),
    new Skill("Language", "There are numerous different Language specialities, each one covering reading and writing in a different language. All Travellers can speak and read their native language without needing the Language skill and automated computer translator programs mean Language skills are not always needed on other worlds. Having Language 0 implies the Traveller has a smattering of simple phrases in a few common languages.", new List<string> {
        "Ordering a Meal, Asking for Basic Directions: Routine (6+) Language check (1D seconds, EDU).",
        "Holding a Simple Conversation: Average (8+) Language check (1D x 10 seconds, EDU).",
        "Understanding a Complex Technical Document or Report: Very Difficult (12+) Language check (1D minutes, EDU)."
    }, new() {
        new Skill("Galanglic", "The common trade language of the Third Imperium, derived originally from the English spoken in the Rule of Man.", new(), new()),
        new Skill("Vilani", "The language spoken by the Vilani of the First Imperium; the 'Latin' of the Third Imperium.", new(), new()),
        new Skill("Zdetl", "The Zhodani spoken language.", new(), new()),
        new Skill("Oynprith", "The Droyne ritual language.", new(), new()),
        new Skill("Trokh", "The Aslan spoken language.", new(), new()),
        new Skill("Gvegh", "The Vargr spoken language.", new(), new())
    }),
    new Skill("Leadership", "The Leadership skill is for directing, inspiring and rallying allies and comrades. A Traveller may make a Leadership action in combat, as detailed on page 74.", new List<string> {
        "Shouting an Order: Average (8+) Leadership check (1D seconds, SOC).",
        "Rallying Shaken Troops: Difficult (10+) Leadership check (1D seconds, SOC)."
    }, new()),
    new Skill("Mechanic", "The Mechanic skill allows a Traveller to maintain and repair most equipment – some advanced equipment and spacecraft components require the Engineer skill. Unlike the narrower and more focused Engineer or Science skills, Mechanic does not allow a Traveller to build new devices or alter existing ones – it is purely for repairs and maintenance but covers all types of equipment.", new List<string> {
        "Repairing a Damaged System in the Field: Average (8+) Mechanic check (1D minutes, INT or EDU)."
    }, new()),
    new Skill("Medic", "The Medic skill covers emergency first aid and battlefield triage as well as diagnosis, treatment, surgery and long-term care. See Injury and Recovery on page 82.", new List<string> {
        "First Aid: Average (8+) Medic check (1D rounds, EDU). The patient regains lost characteristic points equal to the Effect.",
        "Treat Poison or Disease: Average (8+) Medic check (1D hours, EDU).",
        "Long-term Care: Average (8+) Medic check (1 day, EDU)."
    }, new()),
    new Skill("Melee", "The Melee skill covers attacking in hand-to-hand combat and the use of suitable weapons.", new List<string> {
        "Swinging a Sword: Average (8+) Melee (blade) check (1D seconds, STR or DEX)."
    }, new() {
        new Skill("Unarmed", "Punching, kicking and wrestling; using improvised weapons in a bar brawl.", new(), new()),
        new Skill("Blade", "Attacking with swords, rapiers, blades and other edged weapons.", new(), new()),
        new Skill("Bludgeon", "Attacking with maces, clubs, staves and so on.", new(), new()),
        new Skill("Natural", "Weapons that are part of an alien or creature, such as claws or teeth.", new(), new())
    }),
    new Skill("Pilot", "The Pilot skill specialities cover different forms of spacecraft. See the Spacecraft Operations chapter for more details.", new(), new() {
        new Skill("Small Craft", "Shuttles and other craft under 100 tons.", new(), new()),
        new Skill("Spacecraft", "Trade ships and other vessels between 100 and 5,000 tons.", new(), new()),
        new Skill("Capital Ships", "Battleships and other ships over 5,000 tons.", new(), new())
    }),
    new Skill("Navigation", "Navigation is the planetside counterpart of astrogation, covering plotting courses and finding directions on the ground.", new List<string> {
        "Plotting a Course Using an Orbiting Satellite Beacon: Routine (6+) Navigation check (1D x 10 minutes, INT or EDU).",
        "Avoiding Getting Lost in Thick Jungle: Difficult (10+) Navigation check (1D hours, INT)."
    }, new()),
    new Skill("Persuade", "Persuade is a more casual, informal version of Diplomat. It covers fast talking, bargaining, wheedling and bluffing. It also covers bribery or intimidation.", new List<string> {
        "Bluffing Your Way Past a Guard: Opposed Persuade check (1D minutes, INT or SOC).",
        "Haggling in a Bazaar: Opposed Persuade check (1D minutes, INT or SOC).",
        "Intimidating a Thug: Opposed Persuade check (1D minutes, STR or SOC).",
        "Asking the Alien Space Princess to Marry You: Formidable (14+) Persuade check (1D x 10 minutes, SOC)."
    }, new()),
    new Skill("Profession", "A Traveller with a Profession skill is trained in producing useful goods or services. There are many different Profession specialities but each one works the same way – the Traveller can make a Profession check to earn money on a planet that supports that trade. The amount of money raised is Cr250 x the Effect of the check per month. Unlike other skills with specialties, levels in the Profession skill do not grant the ability to use other specialties at level 0. Each specialty must be learned individually. Someone with a Profession skill of 0 has a general grasp of working for a living but little experience beyond the most menial jobs.", new(), new() {
        new Skill("Belter", "Mining asteroids for valuable ores and minerals.", new(), new()),
        new Skill("Biologicals", "Engineering and managing artificial organisms.", new(), new()),
        new Skill("Civil Engineering", "Designing structures and buildings.", new(), new()),
        new Skill("Construction", "Building orbital habitats and megastructures.", new(), new()),
        new Skill("Hydroponics", "Growing crops in hostile environments.", new(), new()),
        new Skill("Polymers", "Designing and using polymers.", new(), new())
    }),
    new Skill("Recon", "A Traveller trained in Recon is able to scout out dangers and spot threats, unusual objects or out of place people.", new List<string> {
        "Working Out the Routine of a Trio of Guard Patrols: Average (8+) Recon check (1D x 10 minutes, INT).",
        "Spotting the Sniper Before they Shoot You: Recon check (1D x 10 seconds, INT) opposed by Stealth (DEX) check."
    }, new()),
    new Skill("Science", "The Science skill covers not just knowledge but also practical application of that knowledge where such practical application is possible.", new List<string> {
        "Remembering a Commonly Known Fact: Routine (6+) Science check (1D minutes, EDU).",
        "Researching a Problem Related to a Field of Science: Average (8+) Science check (1D days, INT)."
    }, new() {
        new Skill("Archaeology", "The study of ancient civilisations, including the previous Imperiums and Ancients. It also covers techniques of investigation and excavations.", new(), new()),
        new Skill("Astronomy", "The study of stars and celestial phenomena.", new(), new()),
        new Skill("Biology", "The study of living organisms.", new(), new()),
        new Skill("Chemistry", "The study of matter at the atomic, molecular and macromolecular levels.", new(), new()),
        new Skill("Cosmology", "The study of the universe and its creation.", new(), new()),
        new Skill("Cybernetics", "The study of blending living and synthetic life.", new(), new()),
        new Skill("Economics", "The study of trade and markets.", new(), new()),
        new Skill("Genetics", "The study of genetic codes and engineering.", new(), new()),
        new Skill("History", "The study of the past, as seen through documents and records as opposed to physical artefacts.", new(), new()),
        new Skill("Linguistics", "The study of languages.", new(), new()),
        new Skill("Philosophy", "The study of beliefs and religions.", new(), new()),
        new Skill("Physics", "The study of the fundamental forces.", new(), new()),
        new Skill("Planetology", "The study of planet formation and evolution.", new(), new()),
        new Skill("Psionicology", "The study of psionic powers and phenomena.", new(), new()),
        new Skill("Psychology", "The study of thought and society.", new(), new()),
        new Skill("Robotics", "The study of robot construction and use.", new(), new()),
        new Skill("Sophontology", "The study of intelligent living creatures.", new(), new()),
        new Skill("Xenology", "The study of alien life forms.", new(), new())
    }),
    new Skill("Seafarer", "The Seafarer skill covers all manner of watercraft and ocean travel.", new List<string> {
        "Controlling a Canoe in a Violent Storm: Formidable (14+) Seafarer (personal) check (1D hours, END)."
    }, new() {
        new Skill("Ocean Ships", "For motorised sea-going vessels.", new(), new()),
        new Skill("Personal", "Used for very small waterborne craft such as canoes and rowboats.", new(), new()),
        new Skill("Sail", "This skill is for wind-driven watercraft.", new(), new()),
        new Skill("Submarine", "For vehicles that travel underwater.", new(), new())
    }),
    new Skill("Stealth", "A Traveller trained in the Stealth skill is adept at staying unseen, unheard and unnoticed.", new List<string> {
        "Sneaking Past a Guard: Stealth check (1D x 10 seconds, DEX) opposed by Recon (INT) check.",
        "Avoiding Detection by a Security Patrol: Stealth check (1D minutes, DEX) opposed by Recon (INT) check."
    }, new()),
    new Skill("Steward", "The Steward skill allows the Traveller to serve and care for nobles and high-class passengers. It covers everything from proper address and behaviour to cooking and tailoring, as well as basic management skills. A Traveller with the Steward skill is necessary on any ship offering high passage.", new List<string> {
        "Cooking a Fine Meal: Average (8+) Steward check (1D hours, EDU).",
        "Calming Down an Angry Duke who has Just Been Told he Will not be Jumping to his Destination on Time: Difficult (10+) Steward check (1D minutes, SOC)."
    }, new()),
    new Skill("Streetwise", "A Traveller with the Streetwise skill understands the urban environment and the power structures in society. On their homeworld, and in related systems, they know criminal contacts and fixers. On other worlds, they can quickly intuit power structures and fit into local underworlds.", new List<string> {
        "Finding a Dealer in Illegal Materials or Technologies: Average (8+) Streetwise check (1D x 10 hours, INT).",
        "Evading a Police Search: Streetwise check (1D x 10 minutes, INT) opposed by Recon (INT) check."
    }, new()),
    new Skill("Survival", "The Survival skill is the wilderness counterpart of the urban Streetwise skill – the Traveller is trained to survive in the wild, build shelters, hunt or trap animals, avoid exposure and so forth. They can recognise plants and animals of their homeworld and related planets and can pick up on common clues and traits even on unfamiliar worlds.", new List<string> {
        "Gathering Supplies in the Wilderness to Survive for a Week: Average (8+) Survival check (1D days, EDU).",
        "Identifying a Poisonous Plant: Average (8+) Survival check (1D x 10 seconds, INT or EDU)."
    }, new()),
    new Skill("Tactics", "This skill covers tactical planning and decision making, from board games to squad level combat to fleet engagements. For use in combat, see Combat chapter.", new List<string> {
        "Developing a Strategy for Attacking an Enemy Base: Average (8+) Tactics (military) check (1D x 10 hours, INT)."
    }, new() {
        new Skill("Military", "Co-ordinating the attacks of foot troops or vehicles on the ground.", new(), new()),
        new Skill("Naval", "Co-ordinating the attacks of a spacecraft or fleet.", new(), new())
    }),
    new Skill("Vacc Suit", "The Vacc Suit skill allows a Traveller to wear and operate spacesuits and environmental suits. A Traveller will rarely need to make Vacc Suit checks under ordinary circumstances – merely possessing the skill is enough. If the Traveller does not have the requisite Vacc Suit skill for the suit they are wearing, they suffer DM-1 to all skill checks made while wearing a suit for each missing level. This skill also permits a Traveller to operate advanced battle armour.", new List<string> {
        "Performing a Systems Check on Battle Dress: Average (8+) Vacc Suit check (1D minutes, EDU)."
    }, new())
};

public static IReadOnlyList<Skill> Skills => _skills;
}

public record Skill(string Name, string Description, List<string> Examples, List<Skill> Specialties);

/*
Admin
This skill covers bureaucracies and administration of all
sorts, including the navigation of bureaucratic obstacles
or disasters. It also covers tracking inventories, ship
manifests and other records.
Avoiding Close Examination of Papers: Average (8+)
Admin check (1D x 10 seconds, EDU or SOC).
Dealing with Police Harassment: Difficult (10+) Admin
check (1D x 10 minutes, EDU or SOC).
Advocate
Advocate gives a knowledge of common legal codes
and practises, especially interstellar law. It also gives the
Traveller experience in oratory, debate and public speaking,
making it an excellent skill for lawyers and politicians.
Arguing in Court: Opposed Advocate check (1D days,
EDU or SOC).
Debating an Argument: Opposed Advocate check (1D x
10 minutes, INT).
skills and tasks

Animals
This skill, rare on industrialised or technologically
advanced worlds, is for the care of animals.
Specialities
• Handling: The Traveller knows how to handle
an animal and ride those trained to bear a rider.
Unusual animals raise the difficulty of the check.
Riding a Horse into Battle: Difficult (10+) Animals
(handling) check (1D seconds, DEX). If successful,
the Traveller can control the horse for a number of
minutes equal to the Effect before needing to make
another check.
Veterinary: The Traveller is trained in veterinary
medicine and animal care.
Applying Medical Care: See the Medic skill on page
69, but use the Animals (veterinary) skill.
• Training: The Traveller knows how to tame and
train animals.
Taming a Strange Alien Creature: Formidable (14+)
Animals (training) check (1D days, INT).

Art
The Traveller is trained in a type of creative art.
Specialities
• Performer: The Traveller is a trained actor, dancer
or singer at home on the stage, screen or holo.
Performing a Play: Average (8+) Art (performer)
check (1D hours, EDU).
Convincing a Person you are Actually Someone
Else: Art (performer) check (INT) opposed by Recon
check (INT).
• Holography: Recording and producing aesthetically
pleasing and clear holographic images.
Surreptitiously Switching on Your Recorder
While in a Secret Meeting: Formidable (14+) Art
(holography) check (1D seconds, DEX).
• Instrument: Playing a particular musical
instrument, such a flute, piano or organ.
Playing a Concerto: Difficult (10+) Art (instrument)
check (1D x 10 minutes, EDU).
• Visual Media: Making artistic or abstract paintings
or sculptures in a variety of media.
Making a Statue of Someone: Difficult (10+) Art
(visual media) check (1D days, INT).
• Write: Composing inspiring or interesting pieces
of text.
Rousing the People of a Planet by Exposing Their
Government’s Corruption: Difficult (10+) Art (write)
check (1D hours, INT or EDU).
Writing the Update of Traveller: Formidable (14+)
Art (write) check (1D months, INT).

Astrogation
This skill is for plotting the courses of starships and
calculating accurate jumps. See the Spacecraft
Operations chapter.
Plotting Course to a Target World Using a Gas Giant
for a Gravity Slingshot: Difficult (10+) Astrogation check
(1D x 10 minutes, EDU).
Plotting a Standard Jump: Easy (4+) Astrogation
check (1D x 10 minutes, EDU), with DM- equal to the
Jump distance.

Athletics
The Traveller is a trained athlete and is physically
fit. The Athletics skill effectively augments a
Traveller’s physical characteristics; whatever you
can do with Strength alone you can also add your
Athletics (strength) DM to, for example. Athletics is
also the principal skill used in adverse gravitational
environments, specifically Athletics (dexterity) in low or
zero-G and Athletics (strength) in high-G locations.
Specialities
• Dexterity: Climbing, Juggling, Throwing. For alien
species with wings, this also includes flying.
Climbing: Difficulty varies. Athletics (dexterity)
check (1D x 10 seconds, DEX). So long as they
succeed, the Traveller’s Effect is usually irrelevant
unless they are trying to do something while
climbing, in which case the climbing is part of a task
chain or multiple action.
Sprinting: Average (8+) Athletics (dexterity) check
(1D seconds, DEX). If the Traveller does nothing
but sprint flat out they can cover 24 + Effect metres
with every check. Avoiding obstacles while sprinting
requires another Athletics (dexterity) check (Difficult,
because they are performing a multiple action).
High Jumping: Average (8+) Athletics (dexterity) check
(1D seconds, DEX). The Traveller jumps a number of
metres straight up equal to the Effect halved.
Long Jumping: Average (8+) Athletics (dexterity)
check (1D seconds, DEX). The Traveller jumps a
number of metres forward equal to the Effect with a
running start.
Righting Yourself When Artificial Gravity Suddenly
Fails on Board a Ship: Average (8+) Athletics
(dexterity) check (1D seconds, DEX).
• Endurance: Long-distance running, hiking
Long-distance Running: Average (8+) Athletics
(endurance) check (1D x 10 minutes, END).
Long-distance Swimming: Average (8+) Athletics
(endurance) check (1D x 10 minutes, END).
• Strength: Feats of strength, weight-lifting.
Arm Wrestling: Opposed Athletics (strength) check
(1D minutes, STR).
Feats of Strength: Average (8+) Athletics (strength)
check (1D x 10 seconds, STR).
Performing a Complicated Task in a High Gravity
Environment: Difficult (10+) Athletics (strength)
check (1D seconds, STR).

Broker
The Broker skill allows a Traveller to negotiate trades
and arrange fair deals. It is heavily used when trading
(see the Trade chapter).
Negotiating a Deal: Average (8+) Broker check (1D
hours, INT).
Finding a Buyer: Average (8+) Broker check (1D
hours, SOC).

Carouse
Carousing is the art of socialising; having fun, but also
ensuring other people have fun, and infectious good
humour. It also covers social awareness and subterfuge
in such situations.
Drinking Someone Under the Table: Opposed Carouse
check (1D hours, END). Difficulty varies by liquor.
Gathering Rumours at a Party: Average (8+) Carouse
check (1D hours, SOC).
skills and tasks

Deception
Deception allows a Traveller to lie fluently, disguise
themselves, perform sleight of hand and fool
onlookers. Most underhanded ways of cheating and
lying fall under deception.
Convincing a Guard to let you Past Without ID: Very
Difficult (12+) Deception check (1D minutes, INT).
Alternatively, oppose with a Recon check.
Palming a Credit Chit: Average (8+) Deception check
(1D seconds, DEX).
Disguising Yourself as a Wealthy Noble to Fool a Client:
Difficult (10+) Deception check (1D x 10 minutes, INT
or SOC). Alternatively, oppose with a Recon check.

Diplomat
The Diplomat skill is for negotiating deals, establishing
peaceful contact and smoothing over social faux pas.
It includes how to behave in high society and proper
ways to address nobles. It is a much more formal skill
than Persuade.
Greeting the Emperor Properly: Difficult (10+) Diplomat
check (1D minutes, SOC).
Negotiating a Peace Treaty: Average (8+) Diplomat
check (1D days, EDU).
Transmitting a Formal Surrender: Average (8+)
Diplomat check (1D x 10 seconds, INT).

Drive
This skill is for controlling ground vehicles of various
types. There are several specialities.
Specialities
• Hovercraft: Vehicles that rely on a cushion of air
and thrusters for motion.
Manoeuvring a Hovercraft Through a Tight
Canal: Difficult (10+) Drive (hovercraft) check (1D
minutes, DEX).
• Mole: For controlling vehicles that move through
solid matter using drills or other earth-moving
technologies, such as plasma torches or cavitation.
Surfacing in the Right Place: Average (8+) Drive
(mole) check (1D x 10 minutes, INT).
Precisely Controlling a Dig to Expose a Vein of
Minerals: Difficult (10+) Drive (mole) check (1D x 10
minutes, DEX).
• Track: For tanks and other vehicles that move
on tracks.
Manoeuvring (or Smashing, Depending on the
Vehicle) Through a Forest: Difficult (10+) Drive
(tracked) check (1D minutes, DEX).
Driving a Tank into a Cargo Bay: Average (8+)
Drive (tracked) check (1D x 10 seconds, DEX).
• Walker: Vehicles that use two or more legs to
manoeuvre.
Negotiating Rough Terrain: Difficult (10+) Drive
(walker) check (1D minutes, DEX).
• Wheel: For automobiles and similar groundcars.
Driving a Groundcar in a Short Race: Opposed Drive
(wheeled) check (1D minutes, DEX). Longer races
might use END instead of DEX.
Avoiding an Unexpected Obstacle on the Road: Average
(8+) Drive (wheeled) check (1D seconds, DEX).

Electronics
This skill is used to operate electronic devices such
as computers and ship-board systems. Higher levels
represent the ability to repair and create electronic
devices and systems. There are several specialities.
Specialities
• Comms: The use of modern telecommunications;
opening communications channels, querying
computer networks, jamming signals and so on, as
well as the proper protocols for communicating with
starports and other spacecraft.
Requesting Landing Privileges at a Starport:
Routine (6+) Electronic (comms) check (1D
minutes, EDU).
Accessing Publicly Available but Obscure Data
Over Comms: Average (8+) Electronic (comms)
check (1D x 10 minutes, EDU).
Bouncing a Signal off Orbiting Satellite to Hide Your
Transmitter: Difficult (10+) Electronics (comms)
check (1D x 10 minutes, INT).
Jamming a Comms System: Opposed Electronics
(comms) check (1D minutes, INT). Difficult (10+) for
radio, Very Difficult (12+) for laser and Formidable
(14+) for masers. A Traveller using a comms
system with a higher Technology Level than their
opponent gains DM+1 for every TL of difference.
• Computers: Using and controlling computer
systems and similar electronics and electrics.
Accessing Publicly Available Data: Easy (4+)
Electronics (computers) check (1D minutes, INT
or EDU).
Activating a Computer Program on a Ship’s
Computer: Routine (6+) Electronics (computers)
check (1D x 10 seconds, INT or EDU).
Searching a Corporate Database for Evidence
of Illegal Activity: Difficult (10+) Electronics
(computers) check (1D hours, INT).
Hacking into a Secure Computer Network:
Formidable (14+) Electronics (computers) check
(1D x 10 hours, INT). Hacking is aided by Intrusion
programs and made more difficult by Security
programs. The Effect determines the amount of
data retrieved; failure means the targeted system
may be able to trace the hacking attempt.
• Remote Ops: Using telepresence to remotely
control drones, missiles, robots and other devices.
Using a Mining Drone to Excavate an Asteroid:
Routine (6+) Electronics (remote ops) check (1D
hours, DEX).
• Sensors: The use and interpretation of data
from electronic sensor devices, from observation
satellites and remote probes to thermal imaging
and densitometers.
Making a Detailed Sensor Scan: Routine (6+)
Electronics (sensors) check (1D x 10 minutes, INT
or EDU).
Analysing Sensor Data: Average (8+) Electronics
(sensors) check (1D hours, INT).

Explosives
The Explosives skill covers the use of demolition
charges and other explosive devices, including
assembling or disarming bombs. A failed Explosives
check with an Effect of -4 or less can result in a bomb
detonating prematurely.
Planting Charges to Collapse a Wall in a Building:
Average (8+) Explosives check (1D x 10 minutes, EDU).
Planting a Breaching Charge: Average (8+) Explosives
check (1D x 10 seconds, EDU). The damage from the
explosive is multiplied by the Effect.
Disarming a Bomb Equipped with Anti-Tamper Trembler
Detonators: Formidable (14+) Explosives check (1D
minutes, DEX).

Flyer
The various specialities of this skill cover different types
of flying vehicles. Flyers only work in an atmosphere;
vehicles that can leave the atmosphere and enter orbit
generally use the Pilot skill.
Specialities
• Airship: Used for airships, dirigibles and other
powered lighter than air craft.
• Grav: This covers air/rafts, grav belts and other
vehicles that use gravitic technology.
• Ornithopter: For vehicles that fly through the use
of flapping wings.
• Rotor: For helicopters, tilt-rotors and aerodynes.
• Wing: For jets, vectored thrust aircraft and
aeroplanes using a lifting body.
Landing Safely: Routine (6+) Flyer check (1D minutes, DEX).
Racing Another Flyer: Opposed Flyer check (1D x 10
minutes, DEX).

Gunner
The various specialities of this skill deal with the
operation of ship-mounted weapons in space combat.
Most Travellers have smaller ships equipped solely with
turret weapons.
Specialities
• Turret: Operating turret-mounted weapons on
board a ship.
Firing a Turret at an Enemy Ship: Average (8+)
Gunner (turret) check (1D seconds, DEX).
• Ortillery: A contraction of Orbital Artillery – using
a ship’s weapons for planetary bombardment or
attacks on stationary targets.
Firing Ortillery: Average (8+) Gunner (ortillery)
check (1D minutes, INT).
• Screen: Activating and using a ship’s energy screens
like Black Globe generators or meson screens.
Activating a Screen to Intercept Enemy Fire: Difficult
(10+) Gunner (screen) check (1D seconds, DEX).
• Capital: Operating bay or spinal mount weapons
on board a ship.
Firing a Spinal Mount Weapon: Average (8+)
Gunner (capital) check (1D minutes, INT).

Gun Combat
The Gun Combat skill covers a variety of ranged
weapons. See the Combat chapter for details on using
guns in combat.
Specialities
• Archaic: For primitive weapons that are not
thrown, such as bows and blowpipes.
• Energy: Using advanced energy weapons like
laser pistols or plasma rifles.
• Slug: Weapons that fire a solid projectile such as
the autorifle or gauss rifle.
Firing a Gun: Average (8+) Gun Combat check (1D
seconds, DEX).

Gambler
The Traveller is familiar with a wide variety of gambling
games, such as poker, roulette, blackjack, horse-racing,
sports betting and so on, and has an excellent grasp
of statistics and probability. Gambler increases the
rewards from Benefit rolls, giving the Traveller DM+1 to
their cash rolls if they have Gambler 1 or better.
A Casual Game of Poker: Opposed Gambler check (1D
hours, INT).
Picking the Right Horse to Bet On: Average (8+)
Gambler check (1D minutes, INT).
skills and tasks

Heavy Weapons
The Heavy Weapons skill covers portable and larger
weapons that cause extreme property damage, such as
rocket launchers, artillery and large plasma weapons.
Specialities
• Artillery: Fixed guns, mortars and other indirect-
fire weapons.
• Portable: Missile launchers, flamethrowers and
portable fusion and plasma guns.
• Vehicle: Large weapons typically mounted on
vehicles or strongpoints such as tank guns and
autocannon.
Firing an Artillery Piece at a Visible Target: Average (8+)
Heavy Weapons (artillery) check (1D seconds, DEX).
Firing an Artillery Piece Using Indirect Fire: Difficult
(10+) Heavy Weapons (artillery) check (1D x 10
seconds, INT).

Investigate
The Investigate skill incorporates keen observation,
forensics and detailed analysis.
Searching a Crime Scene For Clues: Average (8+)
Investigate check (1D x 10 minutes, INT).
Watching a Bank of Security Monitors in a Starport,
Waiting for a Specific Criminal: Difficult (10+)
Investigate check (1D hours, INT).

Jack-of-All-Trades
The Jack-of-All-Trades skill works differently to other
skills. It reduces the unskilled penalty a Traveller
receives for not having the appropriate skill by one
for every level of Jack-of-All-Trades. For example, if
a Traveller does not have the Pilot skill, they suffer
DM-3 to all Pilot checks. If that Traveller has Jack-
of-All-Trades 2, then the penalty is reduced by 2 to
DM-1. With Jack-of-All-Trades 3, a Traveller can totally
negate the penalty for being unskilled.
There is no benefit for having Jack-of-All-Trades 0 or
Jack-of-All-Trades 4 or higher.

Language
There are numerous different Language specialities,
each one covering reading and writing in a different
language. All Travellers can speak and read their
native language without needing the Language skill
and automated computer translator programs mean
Language skills are not always needed on other worlds.
Having Language 0 implies the Traveller has a smattering
of simple phrases in a few common languages.
Specialities
There are, of course, as many specialities of Language
as there are actual languages. Those presented here
are examples from the Third Imperium setting.
• Galanglic: The common trade language of the
Third Imperium, derived originally from the English
spoken in the Rule of Man.
• Vilani: The language spoken by the Vilani of the
First Imperium; the ‘Latin’ of the Third Imperium.
• Zdetl: The Zhodani spoken language.
• Oynprith: The Droyne ritual language.
• Trokh: The Aslan spoken language.
• Gvegh: The Vargr spoken language.
Ordering a Meal, Asking for Basic Directions: Routine
(6+) Language check (1D seconds, EDU).
Holding a Simple Conversation: Average (8+)
Language check (1D x 10 seconds, EDU).
Understanding a Complex Technical Document or
Report: Very Difficult (12+) Language check (1D
minutes, EDU).

Leadership
The Leadership skill is for directing, inspiring and
rallying allies and comrades. A Traveller may make a
Leadership action in combat, as detailed on page 74.
Shouting an Order: Average (8+) Leadership check (1D
seconds, SOC).
Rallying Shaken Troops: Difficult (10+) Leadership
check (1D seconds, SOC).

Mechanic
The Mechanic skill allows a Traveller to maintain and
repair most equipment – some advanced equipment and
spacecraft components require the Engineer skill. Unlike
the narrower and more focused Engineer or Science
skills, Mechanic does not allow a Traveller to build new
devices or alter existing ones – it is purely for repairs and
maintenance but covers all types of equipment.
Repairing a Damaged System in the Field: Average
(8+) Mechanic check (1D minutes, INT or EDU).

Medic
The Medic skill covers emergency first aid and
battlefield triage as well as diagnosis, treatment,
surgery and long-term care. See Injury and Recovery
on page 82.
First Aid: Average (8+) Medic check (1D rounds, EDU).
The patient regains lost characteristic points equal to
the Effect.
Treat Poison or Disease: Average (8+) Medic check
(1D hours, EDU).
Long-term Care: Average (8+) Medic check (1 day, EDU).

Melee
The Melee skill covers attacking in hand-to-hand
combat and the use of suitable weapons.
Specialities
• Unarmed: Punching, kicking and wrestling; using
improvised weapons in a bar brawl.
• Blade: Attacking with swords, rapiers, blades and
other edged weapons.
• Bludgeon: Attacking with maces, clubs, staves and
so on.
• Natural: Weapons that are part of an alien or
creature, such as claws or teeth.
Swinging a Sword: Average (8+) Melee (blade) check
(1D seconds, STR or DEX).

Pilot
The Pilot skill specialities cover different forms of
spacecraft. See the Spacecraft Operations chapter for
more details.
Specialities
• Small Craft: Shuttles and other craft under 100 tons.
• Spacecraft: Trade ships and other vessels
between 100 and 5,000 tons.
• Capital Ships: Battleships and other ships over
5,000 tons.

Navigation
Navigation is the planetside counterpart of
astrogation, covering plotting courses and finding
directions on the ground.
Plotting a Course Using an Orbiting Satellite Beacon:
Routine (6+) Navigation check (1D x 10 minutes, INT
or EDU).
Avoiding Getting Lost in Thick Jungle: Difficult (10+)
Navigation check (1D hours, INT).
skills and tasks

Persuade
Persuade is a more casual, informal version of
Diplomat. It covers fast talking, bargaining, wheedling
and bluffing. It also covers bribery or intimidation.
Bluffing Your Way Past a Guard: Opposed Persuade
check (1D minutes, INT or SOC).
Haggling in a Bazaar: Opposed Persuade check (1D
minutes, INT or SOC).
Intimidating a Thug: Opposed Persuade check (1D
minutes, STR or SOC).
Asking the Alien Space Princess to Marry You:
Formidable (14+) Persuade check (1D x 10 minutes, SOC).

Profession
A Traveller with a Profession skill is trained in producing
useful goods or services. There are many different
Profession specialities but each one works the same
way – the Traveller can make a Profession check to
earn money on a planet that supports that trade. The
amount of money raised is Cr250 x the Effect of the
check per month. Unlike other skills with specialties,
levels in the Profession skill do not grant the ability to
use other specialties at level 0. Each specialty must be
learned individually. Someone with a Profession skill
of 0 has a general grasp of working for a living but little
experience beyond the most menial jobs.
There are a huge range of potential specialities for this
skill, one for every possible profession in the universe.
Some examples suitable to a science fiction setting
are given here. Also note that on some worlds other
skills, such as Animals or Electronics (computers),
may be used to earn a living in the same manner as
Profession skills.
Specialities
• Belter: Mining asteroids for valuable ores and
minerals.
• Biologicals: Engineering and managing artificial
organisms.
• Civil Engineering: Designing structures and
buildings.
• Construction: Building orbital habitats and
megastructures.
• Hydroponics: Growing crops in hostile
environments.
• Polymers: Designing and using polymers.

Recon
A Traveller trained in Recon is able to scout out
dangers and spot threats, unusual objects or out of
place people.
Working Out the Routine of a Trio of Guard Patrols:
Average (8+) Recon check (1D x 10 minutes, INT).
Spotting the Sniper Before they Shoot You: Recon
check (1D x 10 seconds, INT) opposed by Stealth
(DEX) check.

Science
The Science skill covers not just knowledge but also
practical application of that knowledge where such
practical application is possible. There are a large
range of specialities.
Specialities
• Archaeology: The study of ancient civilisations,
including the previous Imperiums and Ancients. It also
covers techniques of investigation and excavations.
• Astronomy: The study of stars and celestial
phenomena.
• Biology: The study of living organisms.
• Chemistry: The study of matter at the atomic,
molecular and macromolecular levels.
• Cosmology: The study of the universe and its
creation.
• Cybernetics: The study of blending living and
synthetic life.
• Economics: The study of trade and markets.
• Genetics: The study of genetic codes and
engineering.
• History: The study of the past, as seen through
documents and records as opposed to physical
artefacts.
• Linguistics: The study of languages.
• Philosophy: The study of beliefs and religions.
• Physics: The study of the fundamental forces.
• Planetology: The study of planet formation and
evolution.
• Psionicology: The study of psionic powers and
phenomena.
• Psychology: The study of thought and society.
• Robotics: The study of robot construction and use.
• Sophontology: The study of intelligent living
creatures.
• Xenology: The study of alien life forms.
Remembering a Commonly Known Fact: Routine (6+)
Science check (1D minutes, EDU).
Researching a Problem Related to a Field of Science:
Average (8+) Science check (1D days, INT).

Seafarer
The Seafarer skill covers all manner of watercraft and
ocean travel.
Specialities
• Ocean Ships: For motorised sea-going vessels.
• Personal: Used for very small waterborne craft
such as canoes and rowboats.
• Sail: This skill is for wind-driven watercraft.
• Submarine: For vehicles that travel underwater.
Controlling a Canoe in a Violent Storm: Formidable
(14+) Seafarer (personal) check (1D hours, END).

Stealth
A Traveller trained in the Stealth skill is adept at staying
unseen, unheard and unnoticed.
Sneaking Past a Guard: Stealth check (1D x 10
seconds, DEX) opposed by Recon (INT) check.
Avoiding Detection by a Security Patrol: Stealth check
(1D minutes, DEX) opposed by Recon (INT) check.

Steward
The Steward skill allows the Traveller to serve and
care for nobles and high-class passengers. It covers
everything from proper address and behaviour to
cooking and tailoring, as well as basic management
skills. A Traveller with the Steward skill is necessary on
any ship offering high passage.
Cooking a Fine Meal: Average (8+) Steward check (1D
hours, EDU).
Calming Down an Angry Duke who has Just Been Told
he Will not be Jumping to his Destination on Time:
Difficult (10+) Steward check (1D minutes, SOC).

Streetwise
A Traveller with the Streetwise skill understands the urban
environment and the power structures in society. On their
homeworld, and in related systems, they know criminal
contacts and fixers. On other worlds, they can quickly
intuit power structures and fit into local underworlds.
Finding a Dealer in Illegal Materials or Technologies:
Average (8+) Streetwise check (1D x 10 hours, INT).
Evading a Police Search: Streetwise check (1D x 10
minutes, INT) opposed by Recon (INT) check.

Survival
The Survival skill is the wilderness counterpart of
the urban Streetwise skill – the Traveller is trained to
survive in the wild, build shelters, hunt or trap animals,
avoid exposure and so forth. They can recognise
plants and animals of their homeworld and related
planets and can pick up on common clues and traits
even on unfamiliar worlds.
Gathering Supplies in the Wilderness to Survive for a
Week: Average (8+) Survival check (1D days, EDU).
Identifying a Poisonous Plant: Average (8+) Survival
check (1D x 10 seconds, INT or EDU).

Tactics
This skill covers tactical planning and decision making,
from board games to squad level combat to fleet
engagements. For use in combat, see Combat chapter.
Specialities
• Military: Co-ordinating the attacks of foot troops or
vehicles on the ground.
• Naval: Co-ordinating the attacks of a spacecraft
or fleet.
Developing a Strategy for Attacking an Enemy Base:
Average (8+) Tactics (military) check (1D x 10 hours, INT).

Vacc Suit
The Vacc Suit skill allows a Traveller to wear and
operate spacesuits and environmental suits. A Traveller
will rarely need to make Vacc Suit checks under
ordinary circumstances – merely possessing the skill
is enough. If the Traveller does not have the requisite
Vacc Suit skill for the suit they are wearing, they suffer
DM-1 to all skill checks made while wearing a suit for
each missing level. This skill also permits a Traveller to
operate advanced battle armour.
Performing a Systems Check on Battle Dress: Average
(8+) Vacc Suit check (1D minutes, EDU).
*/