using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Helper;

public class LabelManager
{
    private Dictionary<string, Label> labels;

    public LabelManager(Node parent)
    {
        labels = new Dictionary<string, Label>();

        foreach (Node child in Helper.GetAllDescendantsEndingIn(parent, "Label"))
        {
            if (child is Label label)
            {
                labels[child.Name] = label;
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
public static class Helper {
    public static List<Node> GetAllDescendants(Node commonAncestor, List<Node> list = null) {
        if (list == null) {
            list = new List<Node>();
        }
        
        foreach(Node child in commonAncestor.GetChildren()) {
            list.Add(child);
            list = GetAllDescendants(child, list);
        }
        return list;
    }

    public static List<T> GetAllDescendants<T>(Node commonAncestor, List<T> list = null) where T : Node {
        if (list == null) {
            list = new List<T>();
        }
        
        foreach(Node child in commonAncestor.GetChildren()) {
            if (child is T subType) {
                list.Add(subType);
            }            
            list = GetAllDescendants(child, list);
        }
        return list;
    }

    public static List<Node> GetAllDescendantsWhere(Node commonAncestor, Predicate<Node> shouldInclude, List<Node> list = null)
    {
        if (list == null)
            list = new List<Node>();    

        foreach (Node child in commonAncestor.GetChildren())
        {
            if (shouldInclude(child)) list.Add(child);
            GetAllDescendantsWhere(child, shouldInclude, list);
        }

        return list;
    }

    public static List<Node> GetAllDescendantsEndingIn(Node commonAncestor, string nameEnding) {
        return GetAllDescendantsWhere(commonAncestor, node => node.Name.ToString().EndsWith(nameEnding));
    }
}
