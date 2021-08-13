// ******************************************************************
//       /\ /|       @file       JobDriverBondageLayDown.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-12 01:02:09
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class JobDriverBondageLayDown : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed) => true;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return ToilsBondage.GetToilLayDownBondage(TargetIndex.A, false);
        }

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