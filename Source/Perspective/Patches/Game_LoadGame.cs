using HarmonyLib;
using Verse;

namespace Perspective;

//Refresh cache on game load
[HarmonyPatch(typeof(Game), nameof(Game.LoadGame))]
public class Game_LoadGame
{
    private static void Prefix()
    {
        ResourceBank.offsetRegistry.Clear();
    }
}