// ******************************************************************
//       /\ /|       @file       JobDriverUseBondageArrest.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 11:50:56
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class JobDriverUseBondageArrest : JobDriver
    {
        private Thing Thing => job.GetTarget(TargetIndex.A).Thing; //building
        private Thing Target => job.GetTarget(TargetIndex.B).Thing; //target

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Thing, job, 1, -1, null, errorOnFailed) &&
                   pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            this.FailOnDestroyedOrNull(TargetIndex.B);
            this.FailOnAggroMentalStateAndHostile(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch)
                .FailOnForbidden(TargetIndex.A);
            if (!(Target is Pawn prisoner))
            {
                yield break;
            }

            if (prisoner.Dead)
            {
                yield break;
            }

            yield return Toils_General.WaitWith(TargetIndex.A, 60, true, true);
            //release building for next job
            yield return Toils_Reserve.Release(TargetIndex.A);
            yield return new Toil
            {
                initAction = delegate
                {
                    //fail
                    if (Thing.Destroyed)
                    {
                        pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
                        return;
                    }

                    pawn.carryTracker.TryDropCarriedThing(Thing.Position, ThingPlaceMode.Direct,
                        out _); //drop prisoner
                    var compBondage = Thing.TryGetComp<CompBondage>();
                    if (compBondage == null)
                    {
                        return;
                    }

                    pawn.Position = compBondage.BondagePos;
                    pawn.Notify_Teleported(false);
                    pawn.stances.CancelBusyStanceHard();
                    //TuckedIntoBed
                    pawn.jobs.StartJob(
                        compBondage.Props.bondageType == BondageType.LayDown
                            ? JobMaker.MakeJob(JobDefOf.SrJobBondageLayDown, Thing)
                            : JobMaker.MakeJob(RimWorld.JobDefOf.Wait),
                        JobCondition.InterruptForced);
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}