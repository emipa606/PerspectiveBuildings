using System.Collections.Generic;
using UnityEngine;

namespace Perspective;

internal static class ResourceBank
{
    public static int transpilerRan = 0;

    public static readonly List<Vector3> standardOffsets = [new(0, 0, 0.2f), new(0, 0, -0.2f)];

    public static Vector3 zero = new(0, 0, 0);

    public static readonly Dictionary<int, CompOffsetter>
        offsetRegistry = new(); //Used to prefetch the ThingComp to avoid using the laggy TryGetComp. Key is the thingID
}