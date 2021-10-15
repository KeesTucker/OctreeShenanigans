using UnityEngine;

public class OctreeGenerator : MonoBehaviour
{
    [SerializeField] private Bounds _bounds;
    [SerializeField] private int _resolution = 64;
    private Vector3 _voxelSize;

    [SerializeField] private GameObject _voxelRepresentation;

    private bool[,,] _voxelArray;

    private OctNode _rootNode;
    
    private void Start()
    {
        _voxelArray = new bool[_resolution, _resolution, _resolution];
        
        _voxelSize = new Vector3(
            _bounds.extents.x / _resolution, 
            _bounds.extents.y / _resolution, 
            _bounds.extents.z / _resolution);

        for (int z = 0; z < _resolution; z++)
        {
            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    var pos = new Vector3(
                        x * _voxelSize.x - (_bounds.extents.x / 2), 
                        y * _voxelSize.y - (_bounds.extents.y / 2), 
                        z * _voxelSize.z - (_bounds.extents.z / 2));

                    var colliders = Physics.OverlapBox(pos, _voxelSize / 2);
                    
                    if (colliders.Length > 0)
                    {
                        _voxelArray[x, y, z] = true;
                    }
                }
            }
        }

        _rootNode = OctNode.GenerateFullTree(_resolution);
        
        for (int z = 0; z < _resolution; z++)
        {
            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    _rootNode.SetNodeStateAtPosition(_voxelArray[x, y, z], new Vector3(x, y, z));
                }
            }
        }
        
        _rootNode.CleanTree((int)Mathf.Log(_resolution, 2));

        DebugNode(_rootNode);
    }

    private void DebugNode(OctNode node)
    {
        //Leaf
        if (node.Children == null)
        {
            if (!node.State) return;
            GameObject debug = Instantiate(_voxelRepresentation, 
                node.Pos * _voxelSize.x - (Vector3.one * (_bounds.extents.x / 2f)), 
                Quaternion.identity, transform);
            debug.transform.localScale = _voxelSize * node.Span * 2;
        }
        //Node
        else
        {
            foreach (OctNode child in node.Children)
            {
                DebugNode(child);
            }
        }
    }
}
