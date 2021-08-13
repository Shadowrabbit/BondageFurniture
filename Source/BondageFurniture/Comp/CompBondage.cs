// ******************************************************************
//       /\ /|       @file       CompBondage.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 11:42:32
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    public class CompBondage : ThingComp
    {
        public CompPropertiesBondage Props => (CompPropertiesBondage) props;

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            if (!selPawn.CanReach((LocalTargetInfo) parent, PathEndMode.Touch, Danger.Deadly))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"NoPath".Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            if (!selPawn.CanReserve((LocalTargetInfo) parent))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"Reserved".Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"Incapable".Translate()})",
                    null, MenuOptionPriority.DisabledOption);
                yield break;
            }

            var bondager = GetBondager();
            if (bondager != null && !selPawn.CanReserve(bondager))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"Reserved".Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            //release
            if (bondager != null && selPawn.CanReserve(bondager))
            {
                yield return new FloatMenuOption(MsicDef.SrBondageRelease.Translate(bondager.Label),
                    () => { ReleaseBondager(selPawn, bondager); });
                yield break;
            }

            var prisonerList = selPawn.Map.mapPawns.PrisonersOfColony;
            if (prisonerList.Count <= 0)
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabelCantArrest()}({MsicDef.SrBondageNoPrisoner.Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            //arrest
            foreach (var prisoner in prisonerList.Where(prisoner => selPawn.CanReserve(prisoner)))
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabelCantArrest()}({MsicDef.SrBondageArrest.Translate(prisoner)})",
                    () => { ArrestBondager(selPawn, prisoner); });
            }
        }

        public IntVec3 BondagePos =>
            BedUtility.GetSleepingSlotPos(0, parent.Position, parent.Rotation, parent.def.size);

        private static string FloatMenuOptionLabelCantArrest()
        {
            return MsicDef.SrBondageCantArrest.Translate();
        }

        private Pawn GetBondager()
        {
            if (parent == null)
            {
                return null;
            }

            if (!parent.Spawned)
            {
                return null;
            }

            var thingList = parent?.Map.thingGrid.ThingsListAt(BondagePos);
            foreach (var thing in thingList)
            {
                if (thing is Pawn p)
                {
                    return p;
                }
            }

            return null;
        }

        private void ReleaseBondager(Pawn selPawn, LocalTargetInfo target)
        {
            if (!selPawn.CanReserveAndReach(parent, PathEndMode.Touch, Danger.Some))
            {
                return;
            }

            //prisoner
            if (!selPawn.CanReserveAndReach(target, PathEndMode.Touch, Danger.Some))
            {
                return;
            }

            //分配job
            var job = JobMaker.MakeJob(JobDefOf.SrJobBondageRelease, parent, target);
            job.count = 1;
            selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }

        private void ArrestBondager(Pawn selPawn, LocalTargetInfo target)
        {
            //building
            if (!selPawn.CanReserveAndReach(parent, PathEndMode.Touch, Danger.Some))
            {
                return;
            }

            //prisoner
            if (!selPawn.CanReserveAndReach(target, PathEndMode.Touch, Danger.Some))
            {
                return;
            }

            //分配job
            var job = JobMaker.MakeJob(JobDefOf.SrJobBondageArrest, parent, target);
            job.count = 1;
            selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }
    }
}