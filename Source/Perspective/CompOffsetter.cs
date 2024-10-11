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
    public Offsetter Props;

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
            action = SetCurrentOffset
        };
        mirrorGizmo = new Command_Action
        {
            defaultLabel = "Owl_Mirror".Translate(),
            defaultDesc = "Owl_MirrorDesc".Translate(),
            icon = iconMirror,
            action = SetMirroredState
        };
    }

    public override void PostDeSpawn(Map map)
    {
        offsetRegistry.Remove(parent.thingIDNumber);
        base.PostDeSpawn(map);
    }

    public override void PostExposeData()
    {
        if (Props == null)
        {
            return;
        }

        Scribe_Values.Look(ref isMirrored, "mirrored");
        Scribe_Values.Look(ref currentOffset, "currentOffset", new Vector3(0, 0, 0));

        UpdateRegistry();
    }

    public void UpdateRegistry()
    {
        isOffset = currentOffset != zero;
        cachedTrueCenter = parent.TrueCenter();

        if (parent.def.drawerType == DrawerType.MapMeshOnly || parent.def.drawerType == DrawerType.MapMeshAndRealTime)
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

    public void SetCurrentOffset()
    {
        SoundDefOf.Click.PlayOneShotOnCamera();

        var index = Props.offsets.FindIndex(x => x == currentOffset);
        currentOffset = Props.offsets.Count - 1 == index ? zero : Props.offsets[index == -1 ? 0 : ++index];
        UpdateRegistry();
    }

    public void SetMirroredState()
    {
        SoundDefOf.Click.PlayOneShotOnCamera();
        isMirrored ^= true;
        UpdateRegistry();
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