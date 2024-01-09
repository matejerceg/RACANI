using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParticleSystem : MonoBehaviour
{

    public Transform spawnPoint;
    public float speed = 10f;
    public float dieTime = 5f;
    public int maxParticles = 100;
    public int freq = 10;
    public int currentTime = 0;
    public int currentParticles = 0;
    public bool start = true;

    System.Random random;
    public GameObject s;

    public float spawnDuration = 3f;
    private float timer = 0f;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random(2);
        currentTime = 0;
        renderer = s.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        /*timer += Time.deltaTime;

        if (timer >= spawnDuration)
        {
            return;
        }*/

        currentTime++;
        if (currentTime == freq)
        {
            currentTime = 0;
            if (transform.childCount < maxParticles)
            {
                currentParticles++;
                SpawnParticle();
            }
        }
    }

    void SpawnParticle()
    {
        //radijus plane-a
        float spawnerWidth = 1f;
        float spawnerHeight = 1f;
        float spawnerDepth = 1f;

        float offsetX = (float)(random.NextDouble() * spawnerWidth - spawnerWidth / 2);
        float offsetY = (float)(random.NextDouble() * spawnerHeight - spawnerHeight / 2);
        float offsetZ = (float)(random.NextDouble() * spawnerDepth - spawnerDepth / 2);

        Vector3 randomOffset = new Vector3(offsetX, offsetY, offsetZ);
        Vector3 randomPosition = spawnPoint.position + randomOffset;


        GameObject particle = Instantiate(s, randomPosition, Quaternion.identity);

        particle.transform.parent = transform;

        speed = Random.Range(1f, 3f);

        float randomRotation = Random.Range(-90f, 90f);
        Quaternion randomRotationQuaternion = Quaternion.Euler(0f, randomRotation, 0f);
        particle.transform.rotation = randomRotationQuaternion;

        renderer = particle.GetComponent<Renderer>();
        Material material = renderer.material;
        if (material.shader.name.Contains("Standard"))
        {
            float randomMetallic = Random.Range(0f, 1f);
            material.SetFloat("_Metallic", randomMetallic);
        }

        //simuliraj pad do smrti cestice
        StartCoroutine(MakeParticleRise(particle, speed));
    }

    IEnumerator MakeParticleRise(GameObject particle, float riseSpeed)
    {
        float elapsedTime = 0f;

        while (elapsedTime < dieTime)
        {
            particle.transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Destroy(particle);
        currentParticles--;
    }
}
