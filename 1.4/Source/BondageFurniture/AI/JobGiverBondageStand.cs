// ******************************************************************
//       /\ /|       @file       JobGiverBondageStand.cs
//       \ V/        @brief      行为节点 站立状态被束缚
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-13 05:13:46
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class JobGiverBondageStand : ThinkNode_JobGiver
    {
        /// <summary>
        /// 分配行为
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        protected override Job TryGiveJob(Pawn pawn)
        {
            var bondageThing = pawn.GetBondageThing();
            //当前位置存在带有束缚组件的物体
            if (bondageThing == null)
            {
                return null;
            }

            //被束缚的角色执行 躺着被束缚行为
            var job = JobMaker.MakeJob(JobDefOf.SrJobBondageStand);
            return job;
        }
    }
}