using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;
using static Perspective.ResourceBank;

namespace Perspective;

[HarmonyPatch(typeof(Graphic), nameof(Graphic.DrawWorker))]
public static class Graphic_DrawWorker
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var count = codes.Count;
        for (var i = 0; i < count; ++i)
        {
            if (codes[i].opcode != OpCodes.Call || codes[i].operand as MethodInfo !=
                AccessTools.Method(typeof(Graphic), nameof(Graphic.DrawOffset)))
            {
                continue;
            }

            codes.InsertRange(i + 3, new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Ldarga_S, 1),
                new CodeInstruction(OpCodes.Ldarg_S, 4),
                new CodeInstruction(OpCodes.Call,
                    typeof(Graphic_Print).GetMethod(nameof(Graphic_Print.DrawPerspectiveOffset)))
            });
            ++transpilerRan;
            break;
        }

        return codes.AsEnumerable();
    }
}