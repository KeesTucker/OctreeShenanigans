using System.Linq;
using UnityEngine;

public class OctNode
{
    private OctNode _parent;
    public OctNode[] Children;

    public bool State;

    public int Span;

    public Vector3 Pos;

    public static OctNode GenerateFullTree(int resolution)
    {
        var root = new OctNode();
        root.AddChildren(resolution, Vector3.zero, root);
        return root;
    }

    private void AddChildren(int span, Vector3 pos, OctNode parent)
    {
        Span = span;
        Pos = pos;
        _parent = parent;
        if (span <= 1) return;
        Children = new OctNode[8];

        for (int z = 0, i = 0; z < 2; z++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++, i++)
                {
                    var child = new OctNode();
                    Children[i] = child;
                    child.AddChildren(span / 2, pos + new Vector3(
                        (x - 0.5f) * 2f * span / 2f,
                        (y - 0.5f) * 2f * span / 2f, 
                        (z - 0.5f) * 2f * span / 2f),
                        this);
                }
            }
        }
    }

    public void CleanTree(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            CleanLevel();
        }
    }

    private void CleanLevel()
    {
        if (Children == null)
        {
            _parent.CleanNode();
        }
        else
        {
            foreach (OctNode child in Children)
            {
                child.CleanLevel();
            }
        }
    }

    private void CleanNode()
    {
        if (Children == null) return;
        
        bool mergeable = Children.All(child => child.State == Children[0].State);

        //Should remove the !Children[0].State but it breaks :(.
        if (!mergeable || !Children[0].State) return;
        
        State = Children[0].State;
        Children = null;
    }
    
    public void SetNodeStateAtPosition(bool state, Vector3 pos)
    {
        //Leaf
        if (Children == null)
        {
            State = state;
        }
        //Node
        else
        {
            int target = 0;
            if (pos.z > Pos.z)
            {
                target += 4;
            }

            if (pos.y > Pos.y)
            {
                target += 2;
            }
            
            if (pos.x > Pos.x)
            {
                target += 1;
            }
            
            Children[target].SetNodeStateAtPosition(state, pos);
        }
    }
}
