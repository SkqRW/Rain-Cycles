using System;
using System.Dynamic;
using System.Linq;
using DevInterface;

namespace FilesSetting;

public class RCPanel : Panel, IDevUISignals
{
    private const float BUTTON_WIDTH = 30f;
    private const float BUTTON_SPACING = 5f;
    private const float MARGIN = 5f;
    private const float BUTTONS_ROW_Y = 80f;
    private const float MIN_PANEL_WIDTH = 200f;

    public static int buttonSelected = 1;
    
    public RCPanel(DevUI owner, string IDstring, DevUINode parentNode, Vector2 pos, Vector2 size, string title) : base(owner, IDstring, parentNode, pos, size, title)
    {
        int n = ReadStateReadFiles.CountRainStateFiles(owner.room?.abstractRoom?.name);
        int cycle = owner.room.game.GetStorySession.saveState.cycleNumber; //can't open in arena
        buttonSelected = n > 0 ? (cycle % n): 0;
        UnityEngine.Debug.Log($"[Rain Cycles] Found {n} rain state files for room {owner.room?.abstractRoom?.name}");
        for (int i = 1; i <= n; i++)
        {
            subNodes.Add(new SelectButton(owner, $"RC_{i}", this, new Vector2(MARGIN, BUTTONS_ROW_Y), BUTTON_WIDTH, i.ToString(), false));
        }
        subNodes.Add(new Button(owner, "RC_Plus", this, new Vector2(MARGIN, BUTTONS_ROW_Y), BUTTON_WIDTH, "+"));
        //subNodes.Add(new Button(owner, "RC_save", this, new Vector2(MARGIN, MARGIN), 190f, "Save"));
        
        ReorganizeButtons();
    }

    public void Signal(DevUISignalType type, DevUINode sender, string message)
    {
        if (type == DevUISignalType.ButtonClick)
        {
            if (sender.IDstring == "RC_Plus")
            {
                int buttonCount = subNodes.Count(n => n is SelectButton) + 1;
                subNodes.Add(new SelectButton(owner, $"RC_{buttonCount}", this, new Vector2(MARGIN, BUTTONS_ROW_Y), BUTTON_WIDTH, buttonCount.ToString(), false));
                string path = ReadStateReadFiles.CreateNewRainStateFile(owner.room?.abstractRoom?.name, buttonCount, owner.room);
                ReorganizeButtons();
            }

            if (sender.IDstring.StartsWith("RC_"))
            {
                int buttonCount = int.Parse(sender.IDstring.Split('_')[1]);
                owner.room.roomSettings.filePath = ReadStateReadFiles.GetRainStateSettingsFile(owner.room?.abstractRoom?.name, buttonCount);
                owner.room.roomSettings.Load((SlugcatStats.Timeline)null);
                foreach (var node in subNodes)
                {
                    node.Refresh();
                }
                parentNode?.Refresh();
                this.owner.room.game.cameras[0].ApplyEffectColorsToAllPaletteTextures(base.RoomSettings.EffectColorA, base.RoomSettings.EffectColorB);
                this.owner.room.game.cameras[0].ChangeMainPalette(base.RoomSettings.Palette);
                owner.room.game.cameras[0].ApplyFade();
                UnityEngine.Debug.Log($"[Rain Cycles] Loaded rain state file for room {owner.room?.abstractRoom?.name} at index {buttonCount}");
            }
        }   
    }
    
    private void ReorganizeButtons(bool newButton = false)
    {
        var selectButtons = subNodes.Where(n => n is SelectButton).ToList();
        var plusButton = subNodes.FirstOrDefault(n => n.IDstring == "RC_Plus");
        
        // Calculate number of buttons per row
        int buttonsPerRow = (int)((this.size.x - 2 * MARGIN + BUTTON_SPACING) / (BUTTON_WIDTH + BUTTON_SPACING));
        buttonsPerRow = Math.Max(1, buttonsPerRow);

        /*
        // Look if should expand panel height
        int totalRows = (int)Math.Ceiling((selectButtons.Count + 1) / (float)buttonsPerRow);
        float requiredHeight = MARGIN + (totalRows * (BUTTON_WIDTH + BUTTON_SPACING)) + MARGIN + BUTTON_WIDTH + MARGIN;
        if (requiredHeight > this.size.y)
        {
            this.size = new Vector2(this.size.x, requiredHeight);
            this.pos = new Vector2(this.pos.x, this.pos.y - 30f);
            this.Refresh();
        }
        */
        
        // Position the SelectButtons in rows
        int totalButtons = selectButtons.Count;
        int ButtonActive = newButton ? totalButtons : RCPanel.buttonSelected;
        for (int i = 0; i < totalButtons; i++)
        {
            int row = i / buttonsPerRow;
            int col = i % buttonsPerRow;
            
            float xPos = MARGIN + col * (BUTTON_WIDTH + BUTTON_SPACING);
            float yPos = BUTTONS_ROW_Y - row * (BUTTON_WIDTH + BUTTON_SPACING);
            
            (selectButtons[i] as PositionedDevUINode).Move(new Vector2(xPos, yPos));
            if(i == ButtonActive){
                (selectButtons[i] as SelectButton).Select();
                UnityEngine.Debug.Log($"[Rain Cycles] Selected button {i}, {ButtonActive}");  
            }
                UnityEngine.Debug.Log($"[Rain Cycles] Selected button {i}, {ButtonActive}");  

        }
        
        if (plusButton != null)
        {
            int totalIndex = totalButtons;
            int row = totalIndex / buttonsPerRow;
            int col = totalIndex % buttonsPerRow;
            
            float xPos = MARGIN + col * (BUTTON_WIDTH + BUTTON_SPACING);
            float yPos = BUTTONS_ROW_Y - row * (BUTTON_WIDTH + BUTTON_SPACING);
            
            (plusButton as PositionedDevUINode).Move(new Vector2(xPos, yPos));
        }
    }
}

