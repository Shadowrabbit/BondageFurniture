// ******************************************************************
//       /\ /|       @file       JobDriverLayDownBondage.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-12 01:02:09
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class JobDriverLayDownBondage : JobDriver_LayDown
    {
        public override bool LookForOtherJobs => false;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return LayDownToil(false);
        }

        public override Vector3 ForcedBodyOffset
        {
            get
            {
                var thing = job.GetTarget(TargetIndex.A).Thing;
                return thing != null && thing.def.Size.z > 1
                    ? new Vector3(0.0f, 0.0f, 0.5f).RotatedBy(thing.Rotation)
                    : base.ForcedBodyOffset;
            }
        }

        public override Toil LayDownToil(bool hasBed)
        {
            return ToilsBondage.GetToilLayDownBondage(TargetIndex.A, LookForOtherJobs, CanSleep, CanRest);
        }
    }
}