// ******************************************************************
//       /\ /|       @file       ToilsBondage.cs
//       \ V/        @brief      工作流程 束缚相关
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-12 01:05:24
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using RimWorld;
using Verse;
using Verse.AI;

namespace RabiSquare.BondageFurniture
{
    public static class ToilsBondage
    {
        /// <summary>
        /// 获取躺下束缚状态的工作流程
        /// </summary>
        /// <param name="bedOrRestSpotIndex"></param>
        /// <param name="lookForOtherJobs"></param>
        /// <param name="canSleep"></param>
        /// <param name="gainRestAndHealth"></param>
        /// <returns></returns>
        public static Toil GetToilLayDownBondage(TargetIndex bedOrRestSpotIndex, bool lookForOtherJobs = false,
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
                var thing = curJob.GetTarget(bedOrRestSpotIndex).Thing as Building_Bed;
                if (!curDriver.asleep)
                {
                    if (canSleep && (RestUtility.CanFallAsleep(actor) || curJob.forceSleep) &&
                        (actor.ageTracker.CurLifeStage.canVoluntarilySleep || curJob.startInvoluntarySleep))
                    {
                        curDriver.asleep = true;
                        curJob.startInvoluntarySleep = false;
                    }
                }
                else if (!canSleep || RestUtility.ShouldWakeUp(actor) && !curJob.forceSleep)
                    curDriver.asleep = false;

                ApplyBedRelatedEffects(actor, thing, curDriver.asleep, gainRestAndHealth);
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

        private static void ApplyBedRelatedEffects(
            Pawn p,
            Building_Bed bed,
            bool asleep,
            bool gainRest)
        {
            p.GainComfortFromCellIfPossible();
            if (asleep & gainRest && p.needs.rest != null)
            {
                var restEffectiveness =
                    bed == null || !bed.def.statBases.StatListContains(StatDefOf.BedRestEffectiveness)
                        ? StatDefOf.BedRestEffectiveness.valueIfMissing
                        : bed.GetStatValue(StatDefOf.BedRestEffectiveness);
                p.needs.rest.TickResting(restEffectiveness);
            }

            Thing spawnedParentOrMe;
            if (p.IsHashIntervalTick(100) && (spawnedParentOrMe = p.SpawnedParentOrMe) != null &&
                !spawnedParentOrMe.Position.Fogged(spawnedParentOrMe.Map))
            {
                if (asleep && !p.IsColonyMech)
                {
                    var fleckDef = FleckDefOf.SleepZ;
                    var velocitySpeed = 0.42f;
                    switch (p.ageTracker.CurLifeStage.developmentalStage)
                    {
                        case DevelopmentalStage.Baby:
                        case DevelopmentalStage.Newborn:
                            fleckDef = FleckDefOf.SleepZ_Tiny;
                            velocitySpeed = 0.25f;
                            break;
                        case DevelopmentalStage.Child:
                            fleckDef = FleckDefOf.SleepZ_Small;
                            velocitySpeed = 0.33f;
                            break;
                    }

                    FleckMaker.ThrowMetaIcon(spawnedParentOrMe.Position, spawnedParentOrMe.Map, fleckDef,
                        velocitySpeed);
                }

                if (gainRest && p.health.hediffSet.GetNaturallyHealingInjuredParts().Any<BodyPartRecord>())
                    FleckMaker.ThrowMetaIcon(spawnedParentOrMe.Position, spawnedParentOrMe.Map,
                        FleckDefOf.HealingCross);
            }

            if (p.mindState.applyBedThoughtsTick != 0 && p.mindState.applyBedThoughtsTick <= Find.TickManager.TicksGame)
            {
                ApplyBedThoughts(p, bed);
                p.mindState.applyBedThoughtsTick += 60000;
                p.mindState.applyBedThoughtsOnLeave = true;
            }

            if (!ModsConfig.IdeologyActive || bed == null || (!p.IsHashIntervalTick(2500) || p.Awake()) ||
                !p.IsFreeColonist && !p.IsPrisonerOfColony || p.IsSlaveOfColony)
                return;
            var room = bed.GetRoom();
            if (room.PsychologicallyOutdoors)
                return;
            var flag = false;
            foreach (var containedBed in room.ContainedBeds)
            {
                foreach (var curOccupant in containedBed.CurOccupants)
                {
                    if (curOccupant == p || curOccupant.Awake() || (!curOccupant.IsSlave || LovePartnerRelationUtility
                        .LovePartnerRelationExists(p, curOccupant))) continue;
                    p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInRoomWithSlave);
                    flag = true;
                    break;
                }

                if (flag)
                    break;
            }
        }

        private static void ApplyBedThoughts(Pawn actor, Building_Bed bed)
        {
            if (actor.needs.mood == null)
                return;
            actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBedroom);
            actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInBarracks);
            actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOutside);
            actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptOnGround);
            actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInCold);
            actor.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.SleptInHeat);
            if ((double) actor.AmbientTemperature <
                actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin))
                actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInCold);
            if (actor.AmbientTemperature >
                (double) actor.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax))
                actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptInHeat);
            if (!actor.IsWildMan())
            {
                if (actor.GetRoom().PsychologicallyOutdoors)
                    actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOutside);
                if (bed == null || bed.CostListAdjusted().Count == 0)
                    actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleptOnGround);
            }

            if (bed != null && bed == actor.ownership.OwnedBed && !bed.ForPrisoners &&
                !actor.story.traits.HasTrait(TraitDefOf.Ascetic))
            {
                var def = (ThoughtDef) null;
                if (bed.GetRoom().Role == RoomRoleDefOf.Bedroom)
                    def = ThoughtDefOf.SleptInBedroom;
                else if (bed.GetRoom().Role == RoomRoleDefOf.Barracks)
                    def = ThoughtDefOf.SleptInBarracks;
                if (def != null)
                {
                    var scoreStageIndex =
                        RoomStatDefOf.Impressiveness.GetScoreStageIndex(bed.GetRoom()
                            .GetStat(RoomStatDefOf.Impressiveness));
                    if (def.stages[scoreStageIndex] != null)
                        actor.needs.mood.thoughts.memories.TryGainMemory(
                            ThoughtMaker.MakeThought(def, scoreStageIndex));
                }
            }

            actor.Notify_AddBedThoughts();
        }
    }
}