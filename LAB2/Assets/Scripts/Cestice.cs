using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cestice : MonoBehaviour
{
    public Transform spawnPoint;  
    public float fallSpeed = 10f; 
    public float dieTime = 5f; 
    public int maxSnowflakes = 100;
    public int freq = 10;
    public int currentTime = 0;
    public int currentSnowFlakes = 0;
    public bool start = true;

    System.Random random;
    GameObject s;

    void Start()
    {
        random = new System.Random(2);
        currentTime = 0;
        s = createSnowflake(new Vector3(100f, 100f, 100f));
        s.name = "mySnowflake";
        s.transform.position = new Vector3(100f, 100f, 100f);
    }

    void Update()
    {
        currentTime++;
        if(currentTime == freq)
        {
            currentTime = 0;
            if (transform.childCount < maxSnowflakes)
            {
                //Debug.Log("Stvori novu pahulju");
                currentSnowFlakes++;
                SpawnSnowflake();
            }
        }
    }

    void SpawnSnowflake()
    {
        //radijus oblaka
        float spawnerWidth = 5f;
        float spawnerHeight = 2f;
        float spawnerDepth = 1f;

        //random pozicija unutar radijusa oblaka
        float offsetX = (float)(random.NextDouble() * spawnerWidth - spawnerWidth / 2);
        float offsetY = (float)(random.NextDouble() * spawnerHeight - spawnerHeight / 2);
        float offsetZ = (float)(random.NextDouble() * spawnerDepth - spawnerDepth / 2);

        Vector3 randomOffset = new Vector3(offsetX, offsetY, offsetZ);
        Vector3 randomPosition = spawnPoint.position + randomOffset;

        //Vector3 directionToCamera = Camera.main.transform.position - randomPosition;
        //Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);

        //Debug.Log(randomPosition);
        GameObject snowflake = Instantiate(s, randomPosition, Quaternion.identity);

        snowflake.transform.parent = transform;

        //promjeni scale i fallSpeed za svaki particle
        float randomScaleFactor = Random.Range(1f, 5f);
        Vector3 randomScale = new Vector3(randomScaleFactor, randomScaleFactor, randomScaleFactor);
        snowflake.transform.localScale = randomScale;
        fallSpeed = Random.Range(5f, 12f);

        //simuliraj pad do smrti cestice
        StartCoroutine(MakeSnowflakeFall(snowflake, fallSpeed));
    }

    IEnumerator MakeSnowflakeFall(GameObject snowflake, float fallSpeed)
    {
        float elapsedTime = 0f;

        while (elapsedTime < dieTime)
        {
            snowflake.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Destroy(snowflake);
        currentSnowFlakes--;
    }

    GameObject createSnowflake(Vector3 randomPosition)
    {
        GameObject snowflake;
        snowflake = GameObject.CreatePrimitive(PrimitiveType.Quad);
        snowflake.name = "snowflake";

        Material myMaterial = Resources.Load<Material>("materials/snowMaterial");
        if (myMaterial != null)
        {
            Renderer renderer = snowflake.GetComponent<Renderer>();
            renderer.material = myMaterial;
        }
        else
        {
            Debug.LogError("Materijal ne postoji");
        }

        Vector3 newScale = new Vector3(3f, 3f, 3f);
        snowflake.transform.localScale = newScale;
        
        return snowflake;
    }
}
