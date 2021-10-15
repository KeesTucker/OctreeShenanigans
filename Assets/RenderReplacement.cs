using UnityEngine;

public class RenderReplacement : MonoBehaviour
{
    private void Start()
    {
        if (Camera.main is { }) Camera.main.targetTexture = null;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(RenderOctree.Output, (RenderTexture) null);
    }
}