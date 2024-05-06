using MudBlazor;
using MudBlazor.Utilities;

namespace AgendamentosWEB.Layout;

public sealed class AFSPallete : PaletteLight
{
    private AFSPallete()
    {
        Primary = new MudColor("#FFFFFF"); // Branco
        Secondary = new MudColor("#008000"); // Verde
        Tertiary = new MudColor("#FFA500"); // Laranja
    }

    public static AFSPallete CreatePallete => new();
}
