

// using Microsoft.Extensions.Logging;

// namespace Traveller.Toolset.Generators;

// public class StarGenerator(ILogger<StarGenerator> logger, DiceRoller diceRoller)
// {
//      #region Methods
//     public async Task<StarSystem> GenerateStarSystem(StarSystemGeneratorArgs? args = null)
//     {
//         if (args == null) args = new();
//         StarSystem system = new();

//         //Determine Type
//         var starTypeRoll = diceRoller.RollDiceTotal(2,6);
//         system.System = StarTypeDetermination[starTypeRoll];
//         if (system.System == "Hot")
//         {
//             system.System = StarTypeDeterminationHot[diceRoller.RollDiceTotal(2,6)];
//             system.Tags.Add("Hot");
//         }
//         else if (system.System == "Special")
//         {
            
//         }

//         return system;
//     }
//     #endregion
//     #region Data
//     static readonly Dictionary<int, string> StarTypeDetermination = new()
//         {
//             {2, "Special"},
//             {3, "M"},
//             {4, "M"},
//             {5, "M"},
//             {6, "M"},
//             {7, "K"},
//             {8, "K"},
//             {9, "G"},
//             {10, "G"},
//             {11, "F"},
//             {12, "Hot"},
//         };
//     static readonly Dictionary<int, string> StarTypeDeterminationHot = new()
//     {
//         {2, "A"},
//         {3, "A"},
//         {4, "A"},
//         {5, "A"},
//         {6, "A"},
//         {7, "A"},
//         {8, "A"},
//         {9, "A"},
//         {10, "B"},
//         {11, "B"},
//         {12, "O"},
//     };
//     #endregion



//     #region Models
//     public class StarSystem
//     {
//         public string Sector { get; set; } = string.Empty;
//         public string SubSector{ get; set; } = string.Empty;
//        public string HexLocation{ get; set; } = string.Empty;

      



//     }
//     public class StarSystemGeneratorArgs
//     {

//     }

//     #endregion

//     #region Properties
    
//     #endregion

   

//     #region Helpers

//     #endregion

//     #region Local vars

//     #endregion
// }