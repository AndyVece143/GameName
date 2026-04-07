using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    public Animator anim;

    public void DoTransition()
    {
        anim.Play("trans");
    }
}
