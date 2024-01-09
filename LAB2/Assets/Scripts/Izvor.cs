using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Izvor : MonoBehaviour
{
    public GameObject cloudPrefab;

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = 18f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //pomicanje izvora cestica
        transform.position = new Vector3(worldPosition.x, worldPosition.y, 18f);
    }
}