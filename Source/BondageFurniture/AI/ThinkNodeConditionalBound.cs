// ******************************************************************
//       /\ /|       @file       ThinkNodeConditionalBound.cs
//       \ V/        @brief      条件节点 是否被束缚中
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 07:55:41
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class ThinkNodeConditionalBound : ThinkNode_Conditional
    {
        /// <summary>
        /// 是否满足条件
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.IsBound();
        }
    }
}