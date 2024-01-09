using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimation : MonoBehaviour
{

    Animator myAnimator;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("running");
            myAnimator.SetBool("isChopping", false);
            myAnimator.SetBool("isRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            myAnimator.SetBool("isRunning", false);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isChopping", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            myAnimator.SetBool("isChopping", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isChopping", false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // TODO-druge animacije
        }
    }
}
