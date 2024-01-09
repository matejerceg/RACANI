using System.Collections;
using UnityEngine;

public class TreeController : MonoBehaviour
{

    private Vector3 respawnPosition;
    public GameObject bigTree;
    public GameObject smallTree;
    public GameObject stump;

    public int currentTree;
    public int hp;

    private RegenerateTrees regenerateTrees;

    public GameObject coinPrefab;
    private SFX sfx;

    void Start()
    {
        respawnPosition = transform.position;
        regenerateTrees = transform.parent.GetComponent<RegenerateTrees>();
    }

    public void chopTree()
    {
        GameObject SFX = GameObject.Find("ObjectSFX");
        sfx = SFX.GetComponent<SFX>();
        sfx.PlayChopSound();

        Debug.Log("destroyed");
        if(currentTree == 3)
        {
            hp--;
            if(hp == 0)
            {
                regenerateTrees.StopAllCoroutines();
                regenerateTrees.StartRegenCorutine(bigTree, smallTree, stump, 3);
                smallTree.SetActive(true);
                bigTree.SetActive(false);
                stump.SetActive(false);
                currentTree = 2;
                hp = 2;
                SpawnCoin(stump.gameObject.transform.position);
                SpawnCoin(stump.gameObject.transform.position);

            }
        }
        else if (currentTree == 2)
        {
            hp--;
            if (hp == 0)
            {
                regenerateTrees.StopAllCoroutines();
                regenerateTrees.StartRegenCorutine(bigTree, smallTree, stump, 2);
                stump.SetActive(true);
                smallTree.SetActive(false);
                bigTree.SetActive(false);
                currentTree = 1;
                hp = 1;
                SpawnCoin(stump.gameObject.transform.position);
            }
        }
        else if (currentTree == 1)
        {
            hp--;
            if (hp == 0)
            {
                regenerateTrees.StopAllCoroutines();
                regenerateTrees.StartRegenCorutine(bigTree, smallTree, stump, 1);
                stump.SetActive(false);
                smallTree.SetActive(false);
                bigTree.SetActive(false);
                currentTree = 0;
                hp = 3;
                SpawnCoin(stump.gameObject.transform.position);
                SpawnCoin(stump.gameObject.transform.position);
                SpawnCoin(stump.gameObject.transform.position);
            }
        }
        //gameObject.SetActive(false);
    }

    public void SpawnCoin(Vector3 position)
    {
        float x = Random.Range(-2.5f, 2.5f);
        float z = Random.Range(-2.5f, 2.5f);

        while (Mathf.Abs(x) < 1.0f || Mathf.Abs(z) < 1.0f)
        {
            x = Random.Range(-2.5f, 2.5f);
            z = Random.Range(-2.5f, 2.5f);
        }

        Instantiate(coinPrefab, position + new Vector3(x,0.2f,z), Quaternion.identity);
    }
}
