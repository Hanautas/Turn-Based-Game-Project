using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public GameObject[] items;
    private GameObject itemPrefab;

    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            GameObject shopItem = Instantiate(itemPrefab, transform.position, Quaternion.identity) as GameObject;
        }
    }

    void Update()
    {
        
    }

    public void OpenShop()
    {
        Debug.Log("Test");
    }
}