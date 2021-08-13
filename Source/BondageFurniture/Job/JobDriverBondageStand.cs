// ******************************************************************
//       /\ /|       @file       JobDriverBondageStand.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-13 05:08:17
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class JobDriverBondageStand : JobDriver_Wait
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => true;

        public override Vector3 ForcedBodyOffset
        {
            get
            {
                var thing = job.GetTarget(TargetIndex.A).Thing;
                if (thing == null)
                {
                    return base.ForcedBodyOffset;
                }

                var compBondage = thing.TryGetComp<CompBondage>();
                return compBondage?.Props.forcedBodyOffset.RotatedBy(thing.Rotation) ?? base.ForcedBodyOffset;
            }
        }
    }
}