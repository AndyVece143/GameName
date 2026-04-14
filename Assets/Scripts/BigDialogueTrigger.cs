using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

public class BigDialogueTrigger : MonoBehaviour
{
    public BigDialogue bigDialogue;
    public BoxCollider2D boxCollider;
    public Player player;
    public bool triggered;
    public string[] lines;
    public bool[] changes;
    public bool talker;
    public int react;
    public int[] emotionChanges;

    public BigDialogueSprite char1;
    public BigDialogueSprite char2;

    public CameraController mainCamera;

    private float dialogueTimer;
    public bool isBossFightTrigger;
    public bool afterBossFight;
    public bool changeCameraState;
    public bool sceneTransition;
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        triggered = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggered)
        {
            return;
        }

        if (player.IsGrounded() && afterBossFight == false)
        {
            dialogueTimer += Time.deltaTime;

            if (dialogueTimer > 0.1f)
            {
                mainCamera.state = CameraController.State.StayStill;

                player.StopMoving(react);
                player.state = Player.State.NoMove;
                BigDialogue newBigDialogue = Instantiate(bigDialogue);
                newBigDialogue.lines = lines;
                newBigDialogue.talkChanges = changes;
                newBigDialogue.emotionChanges = emotionChanges;


                newBigDialogue.character1.anim = newBigDialogue.character1.GetComponent<Animator>();
                newBigDialogue.character1.anim.runtimeAnimatorController = char1.anim.runtimeAnimatorController;
                newBigDialogue.character1.image = char1.image;


                newBigDialogue.character2.anim = newBigDialogue.character2.GetComponent<Animator>();
                newBigDialogue.character2.anim.runtimeAnimatorController = char2.anim.runtimeAnimatorController;
                newBigDialogue.character2.image = char2.image;

                //newBigDialogue.character2.transform.localScale = new Vector3(1, 0, 0);
                if (talker)
                {
                    newBigDialogue.character1.isActiveSpeaker = true;
                }
                else
                {
                    newBigDialogue.character2.isActiveSpeaker = true;
                }
                newBigDialogue.isBossFightTrigger = isBossFightTrigger;
                newBigDialogue.changeCameraState = changeCameraState;
                newBigDialogue.sceneTransition = sceneTransition;
                newBigDialogue.sceneName = sceneName;
                triggered = true;
            }

        }

    }
}
