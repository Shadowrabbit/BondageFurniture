// ******************************************************************
//       /\ /|       @file       CompPropertiesBondage.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 11:48:45
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using Verse;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class CompPropertiesBondage : CompProperties
    {
        public CompPropertiesBondage()
        {
            compClass = typeof(CompBondage);
        }
    }
}