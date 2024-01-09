using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 3f;
    public float moveSpeed = 5f;

    public GameObject cloud;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    void Update()
    {
        MoveSpawnerWithMouse();
    }

    void SpawnObject()
    {
        Instantiate(objectToSpawn, cloud.transform.position, Quaternion.identity);
    }

    void MoveSpawnerWithMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 20f;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * moveSpeed);
    }
}

public class ObjectFalling : MonoBehaviour
{
    private Vector3 targetPosition;

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f);
        if (transform.position.y <= 0f)
        {
            Debug.Log("destroy");
            Destroy(gameObject);
        }
    }
}
