// ******************************************************************
//       /\ /|       @file       CompDataGraphicLayer.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 02:21:51
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using JetBrains.Annotations;
using Verse;

namespace RabiSquare.BondageFurniture
{
    [UsedImplicitly]
    public class CompDataGraphicLayer
    {
        [UsedImplicitly] public GraphicData graphicData; //额外的图形
        [UsedImplicitly] public AltitudeLayer altitudeLayer; //对应的layer层
    }
}