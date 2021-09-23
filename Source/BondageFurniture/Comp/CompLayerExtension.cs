// ******************************************************************
//       /\ /|       @file       CompLayerExtension.cs
//       \ V/        @brief      额外层级绘制组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-11 11:45:05
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using RimWorld;
using Verse;

namespace RabiSquare.BondageFurniture
{
    public class CompLayerExtension : ThingComp
    {
        private CompPropertiesLayerExtension Props => (CompPropertiesLayerExtension) props;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }

        public override void PostDraw()
        {
            base.PostDraw();
            Log.Warning("初始化");
            if (Props.graphicLayers == null || Props.graphicLayers.Count <= 0)
            {
                Log.Warning("没有图形层级数据");
                return;
            }

            foreach (var compDataGraphicLayer in Props.graphicLayers)
            {
                var location = GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size,
                    compDataGraphicLayer.altitudeLayer.AltitudeFor());
                if (Prefs.DevMode)
                {
                    Log.Message("绘制 坐标:" + location);
                }

                compDataGraphicLayer.graphicData.Graphic.Draw(location, parent.Rotation, parent);
            }
            Log.Warning("初始化2");
        }
    }
}