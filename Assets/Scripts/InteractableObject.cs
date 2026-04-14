using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool isDialogue;
    public Player player;
    public string[] dialogueLines;
    public Dialogue dialogue;

    [TextArea]
    public string paperLine;
    public Paper paper;

    public bool interactable;
    public BoxCollider2D boxCollider;
    public int react;
    public CameraController mainCamera;
    public bool sceneTransition;
    public string sceneName;
    public bool gameUIRemover;
    //public bool isCutscene;
    public bool isAutomatic;
    private bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.IsTouching(player.boxCollider) && player.IsGrounded())
        {
            //player.thoughtBubble.enabled = true;
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (interactable == true)
                {
                    mainCamera.state = CameraController.State.StayStill;
                    //player.thoughtBubble.enabled = false;
                    interactable = false;
                    player.StopMoving(react);
                    player.state = Player.State.NoMove;

                    switch (isDialogue) 
                    {
                        case true:
                            Dialogue newDialogue = Instantiate(dialogue);
                            newDialogue.lines = dialogueLines;
                            newDialogue.interactableObject = this;
                            if (sceneTransition == true)
                            {
                                newDialogue.sceneTransition = sceneTransition;
                                newDialogue.sceneName = sceneName;
                            }
                            newDialogue.gameUIRemover = gameUIRemover;
                            break;

                        case false:
                            Paper newPaper = Instantiate(paper);
                            newPaper.text = paperLine;
                            newPaper.interactableObject = this;
                            break;
                    
                    }


                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAutomatic && triggered)
        {
            return;
        }

        if (interactable && player.IsGrounded() && isAutomatic && !triggered)
        {
            interactable = false;
            mainCamera.state = CameraController.State.StayStill;
            //player.thoughtBubble.enabled = false;
            interactable = false;
            player.StopMoving(react);
            player.state = Player.State.NoMove;
            Dialogue newDialogue = Instantiate(dialogue);
            newDialogue.lines = dialogueLines;
            newDialogue.interactableObject = this;
            if (sceneTransition == true)
            {
                newDialogue.sceneTransition = sceneTransition;
                newDialogue.sceneName = sceneName;
            }
            newDialogue.gameUIRemover = gameUIRemover;

            triggered = true;
        }

    }
}
