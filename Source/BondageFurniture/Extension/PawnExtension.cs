// ******************************************************************
//       /\ /|       @file       PawnExtension.cs
//       \ V/        @brief      角色扩展
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 04:08:16
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Linq;
using Verse;

namespace RabiSquare.BondageFurniture
{
    public static class PawnExtension
    {
        /// <summary>
        /// 是否被束缚
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        public static bool IsBound(this Pawn pawn)
        {
            var thingList = pawn?.Map.thingGrid.ThingsListAt(pawn.Position);
            if (thingList == null || thingList.Count == 0)
            {
                return false;
            }

            return thingList.Select(thing => thing.TryGetComp<CompBondage>()).Any(compBondage => compBondage != null);
        }

        /// <summary>
        /// 获取当前位置 带有束缚组件的物体
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        public static Thing GetBondageThing(this Pawn pawn)
        {
            var thingList = pawn?.Map.thingGrid.ThingsListAt(pawn.Position);
            if (thingList == null || thingList.Count == 0)
            {
                return null;
            }

            return (from thing in thingList
                let compBondage = thing.TryGetComp<CompBondage>()
                where compBondage != null
                select thing).FirstOrDefault();
        }
    }
}