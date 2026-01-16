using System;
using System.Security;
using System.Security.Permissions;
using BepInEx;
using BepInEx.Logging;


#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace Plugin;

[BepInPlugin(ID, NAME, VER)]
public partial class RSPlugin : BaseUnityPlugin 
{
    public const string ID = "skeq.raincycles";
    public const string NAME = "Rain Cycles";
    public const string VER = "0.4";

    public static ManualLogSource log;

    private void OnDisable()
    {
        JsonGet.PaletteManager.Cleanup();
    }

    private void OnEnable()
    {
        log = base.Logger;
        On.RainWorld.OnModsInit += RainWorldOnOnModsInit;
        On.RainWorld.PostModsInit += RainWorld_PostModsInit;
        On.RainWorldGame.Update += RainWorldGame_Update;
        FilesSetting.RainStateFiles.Init();

    }

    private void RainWorldGame_Update(On.RainWorldGame.orig_Update orig, RainWorldGame self)
    {
        orig(self);
        
        // Revisar y aplicar cambios en archivos de paletas cada frame
        JsonGet.PaletteManager.ReloadChangedFiles();
    }

    private void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
    {
        orig(self);
        JsonGet.CycleStateManager.LoadCycleStates();
        JsonGet.PaletteManager.LoadPalettes(); // Fast or dev mode, more priority
        Logger.LogInfo($"[{NAME}] {VER} Json loaded successfully!");
    }



    private bool IsInit;
    private void RainWorldOnOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        if (IsInit) return;

        try
        {
            IsInit = true;
            RoomChange.PaletteDrive.Init();
            DevTools.Init();
            Logger.LogInfo($"[{NAME}] {VER} loaded successfully!");

        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

}
