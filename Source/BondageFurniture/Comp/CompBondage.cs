// ******************************************************************
//       /\ /|       @file       CompBondage.cs
//       \ V/        @brief      束缚组件 持有该组件的物体拥有束缚角色的能力
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
        //束缚组件的属性
        public CompPropertiesBondage Props => (CompPropertiesBondage) props;

        //第一个睡眠点
        public IntVec3 BondagePos =>
            BedUtility.GetSleepingSlotPos(0, parent.Position, parent.Rotation, parent.def.size);

        /// <summary>
        /// 绘制选项列表
        /// </summary>
        /// <param name="selPawn"></param>
        /// <returns></returns>
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            //角色无法触碰持有组件的当前物体
            if (!selPawn.CanReach((LocalTargetInfo) parent, PathEndMode.Touch, Danger.Deadly))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"NoPath".Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            //角色无法保留持有组件的当前物体
            if (!selPawn.CanReserve((LocalTargetInfo) parent))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"Reserved".Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            //角色健康无法满足操纵
            if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"Incapable".Translate()})",
                    null, MenuOptionPriority.DisabledOption);
                yield break;
            }

            //被束缚者
            var bondager = GetBondager();
            //存在被束缚者 并且 当前角色无法解除被束缚者
            if (bondager != null && !selPawn.CanReserve(bondager))
            {
                yield return new FloatMenuOption($"{FloatMenuOptionLabelCantArrest()}({"Reserved".Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            //存在被束缚者 并且 当且角色可以解除被束缚者 释放选项
            if (bondager != null && selPawn.CanReserve(bondager))
            {
                yield return new FloatMenuOption(MsicDef.SrBondageRelease.Translate(bondager.Label),
                    () => { ReleaseBondager(selPawn, bondager); });
                yield break;
            }

            //不存在被束缚者 不存在囚犯
            var prisonerList = selPawn.Map.mapPawns.PrisonersOfColony;
            if (prisonerList.Count <= 0)
            {
                yield return new FloatMenuOption(
                    $"{FloatMenuOptionLabelCantArrest()}({MsicDef.SrBondageNoPrisoner.Translate()})", null,
                    MenuOptionPriority.DisabledOption);
                yield break;
            }

            //不存在被束缚者 存在囚犯 绘制选项抓捕列表内的囚犯
            foreach (var prisoner in prisonerList.Where(prisoner => selPawn.CanReserve(prisoner)))
            {
                yield return new FloatMenuOption(
                    $"{MsicDef.SrBondageArrest.Translate(prisoner.Label)}",
                    () => { ArrestBondager(selPawn, prisoner); });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string FloatMenuOptionLabelCantArrest()
        {
            return MsicDef.SrBondageCantArrest.Translate();
        }

        /// <summary>
        /// 获取被束缚者
        /// </summary>
        /// <returns></returns>
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

            //处于第一个睡眠点上的角色列表
            var thingList = parent?.Map.thingGrid.ThingsListAt(BondagePos);
            //只取第一个角色 正常情况下无法站在睡眠点上 等同于被束缚状态
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

        /// <summary>
        /// 抓捕被束缚者
        /// </summary>
        /// <param name="selPawn"></param>
        /// <param name="target"></param>
        private void ArrestBondager(Pawn selPawn, LocalTargetInfo target)
        {
            //无法解除持有束缚组件的当前物体
            if (!selPawn.CanReserveAndReach(parent, PathEndMode.Touch, Danger.Some))
            {
                return;
            }

            //当前角色无法接触目标
            if (!selPawn.CanReserveAndReach(target, PathEndMode.Touch, Danger.Some))
            {
                return;
            }

            //当前角色执行job
            var job = JobMaker.MakeJob(JobDefOf.SrJobBondageArrest, parent, target);
            job.count = 1;
            selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }
    }
}