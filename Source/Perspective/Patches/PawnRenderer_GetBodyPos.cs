using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using static Perspective.ResourceBank;

namespace Perspective;

//Pawns in bed follow position
[HarmonyPatch(typeof(PawnRenderer), nameof(PawnRenderer.GetBodyPos))]
public class PawnRenderer_GetBodyPos
{
    private static void Postfix(ref Vector3 __result, Pawn ___pawn)
    {
        var bed = ___pawn.CurrentBed();
        if (bed != null && offsetRegistry.TryGetValue(bed.thingIDNumber, out var compBuffer) && compBuffer.isOffset)
        {
            __result += compBuffer.currentOffset;
        }
    }
}