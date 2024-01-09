using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{

    public Vector2 turn;
    public float sensitivity = 10f;

    public Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayMyAnimations();
        RotateCharacter();
    }

    private void RotateCharacter()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;

            turn.x += mouseX;
            transform.localRotation = Quaternion.Euler(0, -turn.x, 0);
            transform.localRotation *= Quaternion.Euler(0, 180, 0);
        }
    }

    public void PlayMyAnimations()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            myAnimator.SetBool("isChopping", false);
            myAnimator.SetBool("isRunning", true);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isChopping", true);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            myAnimator.SetBool("isRunning", false);
            myAnimator.SetBool("isChopping", false);
        }
    }

    public void StartSwing()
    {
        
    }

    public void DealDamage()
    {

    }
}
