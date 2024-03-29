﻿// ******************************************************************
//       /\ /|       @file       CompPropertiesLayerExtension.cs
//       \ V/        @brief      额外层级绘制属性
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 11:48:30
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace RabiSquare.BondageFurniture
{
    public class CompPropertiesLayerExtension : CompProperties
    {
        public CompPropertiesLayerExtension()
        {
            compClass = typeof(CompLayerExtension);
        }

        [UsedImplicitly]
        public List<CompDataGraphicLayer> graphicLayers;
    }
}