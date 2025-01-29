using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Helper;

public class DebugLabelManager
{
    private Dictionary<string, Label> labels;

    public DebugLabelManager(Node parent)
    {
        labels = new Dictionary<string, Label>();

        foreach (Node child in parent.GetChild(0).GetChildren())
        {
            if (child is Label label)
            {
                labels[child.Name] = label;
                GD.Print($"Found debug label {child.Name}");
            }
        }

    }

    public void UpdateLabel(string key, string value)
    {
        if (labels.ContainsKey(key))
        {
            labels[key].Text = value;
            return;
        }
        string keyPlusLabel = key+"Label";
        if (labels.ContainsKey(keyPlusLabel)) {
            labels[keyPlusLabel].Text = value;
        }
    }
}

