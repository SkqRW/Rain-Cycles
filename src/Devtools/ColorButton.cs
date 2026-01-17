using DevInterface;

namespace FilesSetting;

public class SelectButton : Button
{
    private bool isSelected;
    public SelectButton(DevUI owner, string IDstring, DevUINode parentNode, Vector2 pos, float width, string text, bool isSelected) : base(owner, IDstring, parentNode, pos, width, text)
    {
        this.isSelected = isSelected;
        SetSelected();
    }

    public override void Update()
    {
        base.Update();
        SetSelected();
    }

    public override void Clicked()
    {
        base.Clicked();
        
        if (!isSelected)
        {
            if (parentNode != null)
            {
                // Who care about performance if we have few buttons
                foreach (var node in parentNode.subNodes)
                {
                    if (node is SelectButton otherButton && otherButton != this)
                    {
                        otherButton.Deselect();
                    }
                }
            }
            isSelected = true;
            SetSelected();
        }
    }

    public void Deselect()
    {
        isSelected = false;
        SetSelected();
    }

    public void Select()
    {
        isSelected = true;
        if (parentNode != null)
        {
            // Who care about performance if we have few buttons
            foreach (var node in parentNode.subNodes)
            {
                if (node is SelectButton otherButton && otherButton != this)
                {
                    otherButton.Deselect();
                }
            }
        }
        isSelected = true;
        SetSelected();
    }

    private void SetSelected()
    {
        if (isSelected)
            this.colorA = new Color(0.2f, 0.6f, 0.2f);
        else
            this.colorA = new Color(1f, 1f, 1f);
    }
}