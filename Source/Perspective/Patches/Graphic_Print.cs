using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Perspective;

[HarmonyPatch(typeof(Graphic), nameof(Graphic.Print))]
public static class Graphic_Print
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var count = codes.Count;
        var drawPerspectiveMirror =
            AccessTools.Method(typeof(Graphic_Print), nameof(DrawPerspectiveMirror));

        for (var i = 0; i < count; ++i)
        {
            if (codes[i].opcode != OpCodes.Call || codes[i].operand as MethodInfo !=
                AccessTools.Method(typeof(Graphic), nameof(Graphic.DrawOffset)))
            {
                continue;
            }

            codes.InsertRange(i + 3, new List<CodeInstruction>
            {
                new(OpCodes.Ldloca_S, 3),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Call, typeof(Graphic_Print).GetMethod(nameof(DrawPerspectiveOffset))),

                new(OpCodes.Ldloc_0),
                new(OpCodes.Ldarg_2),
                new(OpCodes.Call, drawPerspectiveMirror),
                new(OpCodes.Stloc_0)
            });
            ++ResourceBank.transpilerRan;
            break;
        }

        if (ResourceBank.transpilerRan < 2)
        {
            Log.Error(
                "[Perspective: Buildings] Transpiler failed to compile. There may be a mod conflict, or RimWorld updated?");
        }

        return codes.AsEnumerable();
    }

    public static void DrawPerspectiveOffset(ref Vector3 pos, Thing thing)
    {
        if (ResourceBank.offsetRegistry.TryGetValue(thing?.thingIDNumber ?? 0, out var compBuffer))
        {
            pos += compBuffer.currentOffset;
        }
    }

    public static bool DrawPerspectiveMirror(bool flag, Thing thing)
    {
        return ResourceBank.offsetRegistry.TryGetValue(thing?.thingIDNumber ?? 0, out var compBuffer) &&
            compBuffer.isMirrored || flag;
    }
}