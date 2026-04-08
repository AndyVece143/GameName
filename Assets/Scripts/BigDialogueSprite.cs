using UnityEngine;
using UnityEngine.UI;

public class BigDialogueSprite : MonoBehaviour
{
    public bool isActiveSpeaker;
    public Image image;
    public Animator anim;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ChangeEmotion(int i)
    {
        anim.SetInteger("emotion", i);
    }
}
