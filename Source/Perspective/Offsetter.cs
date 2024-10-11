using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Perspective;

public class Offsetter : DefModExtension
{
    public enum OffsetType
    {
        Normal,
        Eight,
        Four
    }

    public enum Override
    {
        Normal,
        True,
        False
    }

    public readonly Override ignore = Override.Normal;
    public readonly Override mirror = Override.Normal;
    public List<Vector3> offsets;
    public OffsetType offsetType;
}