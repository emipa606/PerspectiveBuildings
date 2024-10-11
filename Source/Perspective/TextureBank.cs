using UnityEngine;
using Verse;

namespace Perspective;

[StaticConstructorOnStartup]
internal static class TextureBank
{
    public static readonly Texture2D iconMirror = ContentFinder<Texture2D>.Get("UI/Owl_Mirror");
    public static readonly Texture2D iconAdjust = ContentFinder<Texture2D>.Get("UI/Owl_Adjust");
}