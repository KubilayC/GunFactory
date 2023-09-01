using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class xd : MonoBehaviour
{
    public Material adas;
    public float x;
    private void Update()
    {
        x += .0021f;
        adas.SetTextureOffset("_MainTex", new Vector2(x, 0));
    }
}
