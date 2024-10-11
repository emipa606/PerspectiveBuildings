using HarmonyLib;
using UnityEngine;
using Verse;

namespace Perspective;

//Dynamic shadows (non-static). This should probably be replaced with a transpiler one of these days...
[HarmonyPatch(typeof(Graphic_Shadow), nameof(Graphic_Shadow.Print))]
public class Graphic_Shadow_Print
{
    private static bool Prefix(Thing thing, ShadowData ___shadowInfo, SectionLayer layer,
        float ___GlobalShadowPosOffsetX, float ___GlobalShadowPosOffsetZ)
    {
        if (!ResourceBank.offsetRegistry.TryGetValue(thing.thingIDNumber, out var compBuffer) || !compBuffer.isOffset)
        {
            return true;
        }

        var center = compBuffer.cachedTrueCenter + compBuffer.currentOffset + (___shadowInfo.offset +
                                                                               new Vector3(___GlobalShadowPosOffsetX,
                                                                                       0f, ___GlobalShadowPosOffsetZ)
                                                                                   .RotatedBy(thing.Rotation));
        center.y = AltitudeLayer.Shadows.AltitudeFor();
        Printer_Shadow.PrintShadow(layer, center, ___shadowInfo, thing.Rotation);
        return false;
    }
}