using DevInterface;

namespace FilesSetting;

public class RCPanel : Panel
{
    public RCPanel(DevUI owner, string IDstring, DevUINode parentNode, Vector2 pos, Vector2 size, string title) : base(owner, IDstring, parentNode, pos, size, title)
    {
        subNodes.Add(new Button(owner, "RC_1", this, new Vector2(5f, 80f), 30f, "1"));
        subNodes.Add(new Button(owner, "RC_2", this, new Vector2(40f, 80f), 30f, "2"));
        subNodes.Add(new Button(owner, "RC_Plus", this, new Vector2(75f, 80f), 30f, "+"));
        subNodes.Add(new Button(owner, "RC_save", this, new Vector2(5f, 5f), 190f, "Save"));
    }
}