using System;
using UnityEngine;

public class RenderOctree : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    
    public static RenderTexture Output;
    [SerializeField] private Renderer _renderer;
    private Texture2D _texture;
    
    private Vector2Int _res;
    
    private void Start()
    {
        _res = new Vector2Int(Screen.width, Screen.height);
        Output = new RenderTexture(_res.x, _res.y, 24, RenderTextureFormat.R8);
        Output.Create();
        _texture = new Texture2D (_res.x, _res.y);
        _renderer.material.mainTexture = _texture;
    }

    private void Update()
    {
        RenderTexture.active = Output;
        for (int x = 0; x < _res.x; x++)
        {
            for (int y = 0; y < _res.y; y++)
            {
                _texture.SetPixel(x, y, Color.magenta);
            }
        }
        _texture.Apply();
        RenderTexture.active = null;
    }
}
