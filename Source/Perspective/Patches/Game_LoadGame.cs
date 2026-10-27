using HarmonyLib;
using Verse;

namespace Perspective;

//Refresh cache on game load
[HarmonyPatch(typeof(Game), nameof(Game.LoadGame))]
public static class Game_LoadGame
{
    public static void Prefix()
    {
        ResourceBank.offsetRegistry.Clear();
    }
}