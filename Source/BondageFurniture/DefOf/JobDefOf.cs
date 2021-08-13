// ******************************************************************
//       /\ /|       @file       JobDefOf.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 10:11:15
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace RabiSquare.BondageFurniture
{
    [DefOf]
    public static class JobDefOf
    {
        [UsedImplicitly] public static readonly JobDef SrJobBondageArrest;
        [UsedImplicitly] public static readonly JobDef SrJobBondageRelease;
        [UsedImplicitly] public static readonly JobDef SrJobBondageLayDown;
        [UsedImplicitly] public static readonly JobDef SrJobBondageStand;
    }
}