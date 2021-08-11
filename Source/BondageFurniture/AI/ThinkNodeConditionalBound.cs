// ******************************************************************
//       /\ /|       @file       ThinkNodeConditionalBound.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 07:55:41
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    public class ThinkNodeConditionalBound : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.IsBound();
        }
    }
}