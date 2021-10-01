using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShadowEffect : MonoBehaviour
{
    public Vector3 offset = new Vector3(-0.1f, -0.1f);
    public Material material;

    public GameObject shadow;

    void Start()
    {
        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;
        shadow.transform.localPosition = offset;
        //shadow.transform.localPosition = Quaternion.identity;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = shadow.GetComponent<SpriteRenderer>();

        sr.sprite = renderer.sprite;
        sr.material = material;
        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        shadow.transform.localPosition = offset;
    }
}