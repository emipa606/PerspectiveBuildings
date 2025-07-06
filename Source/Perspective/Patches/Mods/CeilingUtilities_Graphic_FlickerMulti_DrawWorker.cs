using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;
using static Perspective.ResourceBank;

namespace Perspective;

[HarmonyPatch]
internal static class CeilingUtilities_Graphic_FlickerMulti_DrawWorker
{
    private static MethodBase target;

    private static bool Prepare()
    {
        var mod = LoadedModManager.RunningMods.FirstOrDefault(m => m.Name.Contains("Simple Utilities: Ceiling"));
        if (mod == null)
        {
            return false;
        }

        var type = mod.assemblies.loadedAssemblies.FirstOrDefault(a => a.GetName().Name == "CeilingUtilities")
            ?.GetType("CeilingUtilities.Graphic_FlickerMulti");

        if (type == null)
        {
            Log.Warning(
                "[Perspective: Buildings] Failed to integrate with Simple Utilities: Ceiling. Class not found.");
            return false;
        }

        target = AccessTools.DeclaredMethod(type, "DrawWorker");

        if (target != null)
        {
            return true;
        }

        Log.Warning(
            "[Perspective: Buildings] Failed to integrate with Simple Utilities: Ceiling. Method not found.");
        return false;
    }

    private static MethodBase TargetMethod()
    {
        return target;
    }

    private static void Prefix(ref Vector3 loc, Thing thing)
    {
        if (offsetRegistry.TryGetValue(thing?.thingIDNumber ?? 0, out var compBuffer))
        {
            loc += compBuffer.currentOffset;
        }
    }
}