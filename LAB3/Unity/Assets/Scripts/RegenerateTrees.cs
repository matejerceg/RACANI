using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateTrees : MonoBehaviour
{
    private Coroutine regenerationCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRegenCorutine(GameObject bigTree, GameObject smallTree, GameObject stump, int i)
    {
        StartCoroutine(RegenCorutine(bigTree, smallTree, stump, i));
    }

    private IEnumerator RegenCorutine(GameObject bigTree, GameObject smallTree, GameObject stump, int i)
    {

        if(i == 3)
        {
            yield return new WaitForSeconds(10f);
        }
        else if(i == 2)
        {
            yield return new WaitForSeconds(20f);
        }
        else if(i == 1)
        {
            yield return new WaitForSeconds(30f);
        } 
        else
        {
            yield return new WaitForSeconds(10f);
        }

        regen(bigTree, smallTree, stump);
    }

    public void StopRegenerationTimer()
    {
        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
            regenerationCoroutine = null;
        }
    }

    public void regen(GameObject bigTree, GameObject smallTree, GameObject stump)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            TreeController treeController = child.GetComponent<TreeController>();

            if(treeController != null)
            {
                if(i == 0)
                {
                    treeController.currentTree = 3;
                    treeController.hp = 2;
                }
                else if (i == 1)
                {
                    treeController.currentTree = 2;
                    treeController.hp = 1;
                }
                if (i == 2)
                {
                    treeController.currentTree = 1;
                    treeController.hp = 3;
                }
            }
        }
        smallTree.SetActive(false);
        bigTree.SetActive(true);
        stump.SetActive(false);

    }
}
