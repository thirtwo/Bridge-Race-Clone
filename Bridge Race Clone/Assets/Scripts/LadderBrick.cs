using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBrick : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Color startColor;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void AnimateBrick(Color startColor)
    {
        this.startColor = startColor;
        StartCoroutine(Co_AnimateBrick());
    }
    private IEnumerator Co_AnimateBrick()
    {
        var color = meshRenderer.material.color;
        var time = Time.time;
        while (color != startColor)
        {
            color = Color.Lerp(color, startColor, Time.deltaTime);
            meshRenderer.material.SetColor("_Color", color);
            if(time + 0.5f < Time.time)
            {
                meshRenderer.material.SetColor("_Color", startColor);
                break;
            }
            yield return null;
        }
    }
}
