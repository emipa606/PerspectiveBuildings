using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using static Perspective.ResourceBank;
using static Perspective.Offsetter.Override;

namespace Perspective;

public class Mod_Perspective : Mod
{
    public Mod_Perspective(ModContentPack content) : base(content)
    {
        new Harmony(Content.PackageIdPlayerFacing).PatchAll();
        LongEventHandler.QueueLongEvent(Setup, null, false, null);
    }

    private void Setup()
    {
        //Give standard offsets to the following defs:
        var dd = DefDatabase<ThingDef>.AllDefs.Where(x =>
                x.HasModExtension<Offsetter>() &&
                x.GetModExtension<Offsetter>().ignore == False || //Mod extension forces inclusion?
                x.category == ThingCategory.Building && //Is a building?
                (!x.graphicData?.Linked ?? false) &&
                x.graphicData.linkFlags == LinkFlags.None && //Isn't a linked graphic like a wall?
                x.useHitPoints && //Has hit points?
                x.altitudeLayer != AltitudeLayer.DoorMoveable && //Not a door?
                x.building.sowTag == null && //Not a pot?
                (x.GetCompProperties<CompProperties_Power>() == null ||
                 x.GetCompProperties<CompProperties_Power>().basePowerConsumption > 0) //Isn't a power plant?
        );

        foreach (var def in dd)
        {
            //Has a pre-defined offsetter?
            if (def.HasModExtension<Offsetter>())
            {
                var modX = def.GetModExtension<Offsetter>();
                //Check if the properties declare to force-ignore offsets, and move the extension if true
                if (modX.offsets == null && modX.mirror == Normal && modX.ignore == Normal)
                {
                    def.modExtensions.Remove(modX);
                    continue;
                }

                if (modX.offsetType == Offsetter.OffsetType.Eight)
                {
                    var tmp = modX.offsets.FirstOrFallback();
                    modX.offsets =
                    [
                        new Vector3(tmp.x, tmp.y, tmp.z),
                        new Vector3(0, tmp.y, tmp.z),
                        new Vector3(-tmp.x, tmp.y, tmp.z),
                        new Vector3(tmp.x, tmp.y, 0),
                        new Vector3(-tmp.x, tmp.y, 0),
                        new Vector3(tmp.x, tmp.y, -tmp.z),
                        new Vector3(0, tmp.y, -tmp.z),
                        new Vector3(-tmp.x, tmp.y, -tmp.z)
                    ];
                }
                else if (modX.offsetType == Offsetter.OffsetType.Four)
                {
                    var tmp = modX.offsets.FirstOrFallback();
                    modX.offsets =
                    [
                        new Vector3(0, tmp.y, tmp.z),
                        new Vector3(0, tmp.y, -tmp.z),
                        new Vector3(tmp.x, tmp.y, 0),
                        new Vector3(-tmp.x, tmp.y, 0)
                    ];
                }
                else if (modX.offsets == null)
                {
                    modX.offsets = standardOffsets;
                }
            }
            else
            {
                //Add modX list if missing
                if (def.modExtensions == null)
                {
                    def.modExtensions = [];
                }

                def.modExtensions.Add(new Offsetter { offsets = standardOffsets });
            }

            //Add component
            if (def.comps == null)
            {
                def.comps = [];
            }

            def.comps.Add(new CompProperties { compClass = typeof(CompOffsetter) });
        }
    }
}