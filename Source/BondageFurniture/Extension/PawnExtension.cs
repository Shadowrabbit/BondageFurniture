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
using Verse.AI;

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

        public static void NotifyTuckedIntoBondageFurture(this Pawn pawn, Thing thing)
        {
            var compBondage = thing.TryGetComp<CompBondage>();
            if (compBondage == null)
            {
                return;
            }

            Log.Warning($"pawn.Position:{pawn.Position}");
            Log.Warning($"pawn.BondagePos:{compBondage.BondagePos}");
            pawn.Position = compBondage.BondagePos;
            pawn.Notify_Teleported(false);
            pawn.stances.CancelBusyStanceHard();
            var job = JobMaker.MakeJob(RimWorld.JobDefOf.LayDown);
            pawn.jobs.StartJob(job, JobCondition.InterruptForced);
        }
    }
}