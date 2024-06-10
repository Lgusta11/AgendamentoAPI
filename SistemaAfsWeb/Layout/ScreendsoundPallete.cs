using MudBlazor;
using MudBlazor.Utilities;

namespace SistemaAfsWeb.Layout;

public sealed class AFSPallete : PaletteLight
{
    private AFSPallete()
    {
        Primary = new MudColor("#008000"); // Branco
        Secondary = new MudColor("#ff8c00"); // Verde
        Tertiary = new MudColor("#FFA500"); // Laranja
        White = new MudColor("#fff"); // Laranja



    }

    public static AFSPallete CreatePallete => new();
}
