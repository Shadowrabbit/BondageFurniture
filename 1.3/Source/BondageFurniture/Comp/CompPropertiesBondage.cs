// ******************************************************************
//       /\ /|       @file       CompPropertiesBondage.cs
//       \ V/        @brief      束缚组件属性
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 11:48:45
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class CompPropertiesBondage : CompProperties
    {
        [UsedImplicitly] public BondageType bondageType; //束缚类型
        [UsedImplicitly] public Vector3 forcedBodyOffset; //身体偏移

        public CompPropertiesBondage()
        {
            compClass = typeof(CompBondage);
        }
    }
}