// ******************************************************************
//       /\ /|       @file       PawnExtension.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 04:08:16
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Linq;
using Verse;

namespace RabiSquare.BondageFurniture
{
    public static class PawnExtension
    {
        public static bool IsBound(this Pawn pawn)
        {
            var thingList = pawn?.Map.thingGrid.ThingsListAt(pawn.Position);
            if (thingList == null || thingList.Count == 0)
            {
                return false;
            }

            return thingList.Select(thing => thing.TryGetComp<CompBondage>()).Any(compBondage => compBondage != null);
        }
    }
}