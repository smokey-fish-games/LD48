using UnityEngine;

public class sprinkle : MonoBehaviour
{
    private Animator animatorionar;

    void Start()
    {
        animatorionar = GetComponent<Animator>();
    }


    public void StartAnimation()
    {
        if (animatorionar != null)
        {
            animatorionar.SetBool("Go", true);
        }
    }
}
