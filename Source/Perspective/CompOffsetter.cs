using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using static Perspective.ResourceBank;
using static Perspective.TextureBank;

namespace Perspective;

public class CompOffsetter : ThingComp
{
    private Command_Action adjustGizmo, mirrorGizmo;
    public Vector3 currentOffset, cachedTrueCenter;
    public bool isOffset, isMirrored;
    private Offsetter Props;

    public override void Initialize(CompProperties props)
    {
        base.Initialize(props);

        //Modextensions will be impersonating CompProperties, basically
        if (parent.def.HasModExtension<Offsetter>())
        {
            Props = parent.def.GetModExtension<Offsetter>();
        }

        //Cache position
        cachedTrueCenter = parent.TrueCenter();

        adjustGizmo = new Command_Action
        {
            defaultLabel = "Owl_Adjust".Translate(),
            defaultDesc = "Owl_AdjustDesc".Translate(),
            icon = iconAdjust,
            action = setCurrentOffset
        };
        mirrorGizmo = new Command_Action
        {
            defaultLabel = "Owl_Mirror".Translate(),
            defaultDesc = "Owl_MirrorDesc".Translate(),
            icon = iconMirror,
            action = setMirroredState
        };
    }

    public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
    {
        offsetRegistry.Remove(parent.thingIDNumber);
        base.PostDeSpawn(map, mode);
    }

    public override void PostExposeData()
    {
        if (Props == null)
        {
            return;
        }

        Scribe_Values.Look(ref isMirrored, "mirrored");
        Scribe_Values.Look(ref currentOffset, "currentOffset", new Vector3(0, 0, 0));

        updateRegistry();
    }

    private void updateRegistry()
    {
        isOffset = currentOffset != zero;
        cachedTrueCenter = parent.TrueCenter();

        if (parent.def.drawerType is DrawerType.MapMeshOnly or DrawerType.MapMeshAndRealTime)
        {
            parent.Map?.mapDrawer.MapMeshDirty(parent.Position, MapMeshFlagDefOf.Things);
        }

        if (isMirrored || currentOffset != zero)
        {
            if (!offsetRegistry.ContainsKey(parent.thingIDNumber))
            {
                offsetRegistry.Add(parent.thingIDNumber, this);
            }
        }
        else
        {
            offsetRegistry.Remove(parent.thingIDNumber);
        }
    }

    private void setCurrentOffset()
    {
        SoundDefOf.Click.PlayOneShotOnCamera();

        var index = Props.offsets.FindIndex(x => x == currentOffset);
        currentOffset = Props.offsets.Count - 1 == index ? zero : Props.offsets[index == -1 ? 0 : ++index];
        updateRegistry();
    }

    private void setMirroredState()
    {
        SoundDefOf.Click.PlayOneShotOnCamera();
        isMirrored ^= true;
        updateRegistry();
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        if (Props == null || !(parent?.Faction?.IsPlayer ?? false))
        {
            yield break;
        }

        // Adjust
        yield return adjustGizmo;

        // Mirror
        if (!parent.def.rotatable && Props.mirror != Offsetter.Override.False ||
            parent.def.rotatable && Props.mirror == Offsetter.Override.True)
        {
            yield return mirrorGizmo;
        }
    }
}