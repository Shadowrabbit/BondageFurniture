// ******************************************************************
//       /\ /|       @file       JobGiverBondageLayDown.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-13 03:47:48
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class JobGiverBondageLayDown : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            var bondageThing = pawn.GetBondageThing();
            if (bondageThing == null)
            {
                return null;
            }

            var job = JobMaker.MakeJob(JobDefOf.SrJobBondageLayDown);
            return job;
        }
    }
}