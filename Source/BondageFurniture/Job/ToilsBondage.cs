// ******************************************************************
//       /\ /|       @file       ToilsBondage.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-12 01:05:24
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    public static class ToilsBondage
    {
        public static Toil GetToilLayDownBondage(TargetIndex bedOrRestSpotIndex, bool lookForOtherJobs,
            bool canSleep = true, bool gainRestAndHealth = true)
        {
            var layDownBondage = new Toil();
            layDownBondage.initAction = () =>
            {
                var actor = layDownBondage.actor;
                actor.pather.StopDead();
                var curDriver = actor.jobs.curDriver;
                actor.jobs.posture = PawnPosture.LayingInBed;
                actor.mindState.lastBedDefSleptIn = null;
                curDriver.asleep = false;
            };
            layDownBondage.tickAction = () =>
            {
                var actor = layDownBondage.actor;
                var curJob = actor.CurJob;
                var curDriver = actor.jobs.curDriver;
                var thing = curJob.GetTarget(bedOrRestSpotIndex).Thing;
                actor.GainComfortFromCellIfPossible();
                //asleep
                if (!curDriver.asleep)
                {
                    if (canSleep && (actor.needs.rest != null &&
                                     (double) actor.needs.rest.CurLevel < RestUtility.FallAsleepMaxLevel(actor) ||
                                     curJob.forceSleep))
                        curDriver.asleep = true;
                }
                else if (!canSleep)
                    curDriver.asleep = false;
                else if ((actor.needs.rest == null ||
                          actor.needs.rest.CurLevel >= (double) RestUtility.WakeThreshold(actor)) &&
                         !curJob.forceSleep)
                    curDriver.asleep = false;

                //rest
                if (curDriver.asleep && gainRestAndHealth && actor.needs.rest != null)
                {
                    var restEffectiveness =
                        thing == null || !thing.def.statBases.StatListContains(StatDefOf.BedRestEffectiveness)
                            ? StatDefOf.BedRestEffectiveness.valueIfMissing
                            : thing.GetStatValue(StatDefOf.BedRestEffectiveness);
                    actor.needs.rest.TickResting(restEffectiveness);
                }

                if (actor.IsHashIntervalTick(100) && !actor.Position.Fogged(actor.Map))
                {
                    if (curDriver.asleep)
                        FleckMaker.ThrowMetaIcon(actor.Position, actor.Map, FleckDefOf.SleepZ);
                    if (gainRestAndHealth && actor.health.hediffSet.GetNaturallyHealingInjuredParts().Any())
                        FleckMaker.ThrowMetaIcon(actor.Position, actor.Map, FleckDefOf.HealingCross);
                }

                if (!lookForOtherJobs || !actor.IsHashIntervalTick(211))
                    return;
                actor.jobs.CheckForJobOverride();
            };
            layDownBondage.defaultCompleteMode = ToilCompleteMode.Never;
            layDownBondage.FailOn(() => !layDownBondage.actor.IsBound());
            layDownBondage.AddFinishAction(() =>
            {
                var actor = layDownBondage.actor;
                var curDriver = actor.jobs.curDriver;
                curDriver.asleep = false;
            });
            return layDownBondage;
        }
    }
}