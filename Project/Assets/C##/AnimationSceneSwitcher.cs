using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AnimationSceneSwitcher : MonoBehaviour
{
    private Animator animator;
    private bool animationEnded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Credits Animation") && stateInfo.normalizedTime >= 1.0f && !animationEnded)
            {
                animationEnded = true;

                StartCoroutine(SwitchScene());
            }
        }
    }


    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(1f); 
        SceneManager.LoadScene("MainMenu"); 
    }
}
