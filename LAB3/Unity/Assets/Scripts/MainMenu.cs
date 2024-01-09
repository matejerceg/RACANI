using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public float timeBetweenAnimations = 5f;
    public string[] animationNames;

    void Start()
    {
        StartCoroutine(PlayAnimationCycle());
    }

    IEnumerator PlayAnimationCycle()
    {
        int i = 0;
        while (i <3)
        {
            foreach (string animationName in animationNames)
            {
                animator.Play(animationName);

                yield return new WaitForSeconds(timeBetweenAnimations);
            }
            i++;
        }
    }

    public void MainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ViewAnimations()
    {
        SceneManager.LoadScene("Animations");
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
