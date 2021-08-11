// ******************************************************************
//       /\ /|       @file       JobDriverUseBondageRelease.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 11:56:44
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    public class JobDriverUseBondageRelease : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            throw new System.NotImplementedException();
        }
    }
}
