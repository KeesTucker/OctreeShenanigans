using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class OctNode
{
    private OctNode _parent;
    public OctNode[] _children;

    public bool _state;

    public int _span;

    public Vector3 _pos;

    public static OctNode GenerateFullTree(int resolution)
    {
        var root = new OctNode();
        root.AddChildren(resolution, Vector3.zero, root);
        return root;
    }

    private void AddChildren(int span, Vector3 pos, OctNode parent)
    {
        _span = span;
        _pos = pos;
        _parent = parent;
        if (span <= 1) return;
        _children = new OctNode[8];

        for (int z = 0, i = 0; z < 2; z++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++, i++)
                {
                    var child = new OctNode();
                    _children[i] = child;
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
        if (_children == null && _parent._children?[0] == this)
        {
            _parent.CleanNode();
        }
        else
        {
            if (_children == null) return;
            foreach (OctNode child in _children) child.CleanLevel();
        }
    }

    private void CleanNode()
    {
        if (_children == null) return;
        if (_children.Any(child => child._children != null)) return;

        bool mergeable = _children.All(child => child._state == _children[0]._state);
        
        if (!mergeable) return;
        
        _state = _children[0]._state;
        _children = null;
    }
    
    public void SetNodeStateAtPosition(bool state, Vector3 pos)
    {
        //Leaf
        if (_children == null)
        {
            _state = state;
        }
        //Node
        else
        {
            int target = 0;
            if (pos.z > _pos.z)
            {
                target += 4;
            }

            if (pos.y > _pos.y)
            {
                target += 2;
            }
            
            if (pos.x > _pos.x)
            {
                target += 1;
            }
            
            _children[target].SetNodeStateAtPosition(state, pos);
        }
    }
}
