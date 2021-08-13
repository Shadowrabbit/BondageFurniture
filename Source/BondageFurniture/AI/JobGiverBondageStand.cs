// ******************************************************************
//       /\ /|       @file       JobGiverBondageStand.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-13 05:13:46
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class JobGiverBondageStand : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            var bondageThing = pawn.GetBondageThing();
            if (bondageThing == null)
            {
                return null;
            }

            var job = JobMaker.MakeJob(JobDefOf.SrJobBondageStand);
            return job;
        }
    }
}