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
using JetBrains.Annotations;
using RimWorld;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class CompBondage : ThingComp
    {
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
                //todo release
                yield return new FloatMenuOption("SrBondageRelease".Translate(bondager.Label), ReleaseBondager);
                yield break;
            }

            var prisonerList = selPawn.Map.mapPawns.PrisonersOfColony;
            if (prisonerList.Count <= 0)
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabelCantArrest()}({"SrBondageNoPrisoner".Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            //arrest
            foreach (var prisoner in prisonerList.Where(prisoner => selPawn.CanReserve(prisoner)))
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabelCantArrest()}({"SrBondageArrest".Translate(selPawn, prisoner)})",
                    ArrestBondager);
            }
        }

        private static string FloatMenuOptionLabelCantArrest()
        {
            return "SrBondageCantArrest".Translate();
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

            var bondagePos = BedUtility.GetSleepingSlotPos(1, parent.Position, parent.Rotation, parent.def.size);
            var thingList = parent?.Map.thingGrid.ThingsListAt(bondagePos);
            foreach (var thing in thingList)
            {
                if (thing is Pawn p)
                {
                    return p;
                }
            }

            return null;
        }

        private static void ReleaseBondager()
        {
            Log.Warning("111");
        }

        private static void ArrestBondager()
        {
            Log.Warning("222");
        }
    }
}