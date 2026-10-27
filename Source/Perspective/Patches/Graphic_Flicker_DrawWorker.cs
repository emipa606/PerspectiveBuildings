using HarmonyLib;
using UnityEngine;
using Verse;

namespace Perspective;

//Fire graphics follow offset
[HarmonyPatch(typeof(Graphic_Flicker), nameof(Graphic_Flicker.DrawWorker))]
public static class Graphic_Flicker_DrawWorker
{
    public static void Prefix(ref Vector3 loc, Thing thing)
    {
        if (ResourceBank.offsetRegistry.TryGetValue(thing?.thingIDNumber ?? 0, out var compBuffer))
        {
            loc += compBuffer.currentOffset;
        }
    }
}