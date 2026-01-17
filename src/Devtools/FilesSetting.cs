using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DevInterface;
using Plugin;
using UnityEngine;

namespace FilesSetting;

public class RainStateFiles
{
    public static void Init()
    {
        RCDEVTools.Init();
        ReadStateReadFiles.Init();
    }
}

public class ReadStateReadFiles
{
    public static void Init()
    {
        On.RoomSettings.ctor_Room_string_Region_bool_bool_Timeline_RainWorldGame += RoomSettings_ctor;
    }

    private static void RoomSettings_ctor(On.RoomSettings.orig_ctor_Room_string_Region_bool_bool_Timeline_RainWorldGame orig, RoomSettings self, Room room, string name, Region region, bool template, bool firstTemplate, SlugcatStats.Timeline timelinePoint, RainWorldGame game)
    {
        orig(self, room, name, region, template, firstTemplate, timelinePoint, game);
        string rainStatePath = null;
        if (room != null && room.game != null && room.game.GetStorySession != null && room.game.GetStorySession.saveState != null)
        {
            int cycle = room.game.GetStorySession.saveState.cycleNumber;
            rainStatePath = GetRainStateFilePath(name, cycle);
            if (rainStatePath != null)
            {
                self.filePath = rainStatePath;
                self.Load((SlugcatStats.Timeline)null);
                RSPlugin.log.LogInfo($"[Rain States] Loaded rain state file for room {name} at cycle {cycle}: {rainStatePath}");
            }
            else
            {
            RSPlugin.log.LogInfo($"[Rain States] No rain state file found for room {name} at cycle {cycle}. Using default settings.");
            }
        }
        else
        {
            RSPlugin.log.LogInfo($"[Rain States] {room == null} | {room?.game == null} | {room?.game?.GetStorySession == null} | {room?.game?.GetStorySession?.saveState == null}");
            RSPlugin.log.LogInfo($"[Rain States] No cycle number found for room. Skipping rain state file loading.");
        }
    }

    public static string GetRainStateFilePath(string roomName, int cycle)
    {
        string text = AssetManager.ResolveFilePath(string.Concat(new string[]
        {
            "World",
            Path.DirectorySeparatorChar.ToString(),
            Regex.Split(roomName, "_")[0],
            "-Rooms",
            Path.DirectorySeparatorChar.ToString(),
            "RainCycles",
            Path.DirectorySeparatorChar.ToString(),
            roomName,
            "_settings_",
            ((cycle % 2) + 1).ToString() + ".txt"
        }));

        if (File.Exists(text))
        {
            UnityEngine.Debug.Log($"[Rain States] Found rain state file for {roomName} at cycle {cycle}: {text}");
            RSPlugin.log.LogInfo($"[Rain States] Found rain state file for {roomName} at cycle {cycle}: {text}");
            return text;
        }

        return null;
    }
}

