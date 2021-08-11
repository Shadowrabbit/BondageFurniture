// ******************************************************************
//       /\ /|       @file       CompLayerExtension.cs
//       \ V/        @brief      
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

        public override void PostDraw()
        {
            base.PostDraw();
            if (Props.graphicLayers == null || Props.graphicLayers.Count == 0)
            {
                return;
            }

            foreach (var compDataGraphicLayer in Props.graphicLayers)
            {
                var location = GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size,
                    compDataGraphicLayer.altitudeLayer.AltitudeFor());
                compDataGraphicLayer.graphicData.Graphic.Draw(location, parent.Rotation, parent);
            }
        }
    }
}