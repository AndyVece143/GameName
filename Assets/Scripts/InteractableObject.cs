using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Player player;
    public string[] dialogueLines;
    public Dialogue dialogue;

    public bool interactable;
    public BoxCollider2D boxCollider;
    public int react;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
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
                    //player.thoughtBubble.enabled = false;
                    interactable = false;
                    player.StopMoving(react);
                    player.state = Player.State.NoMove;

                    Dialogue newDialogue = Instantiate(dialogue);
                    newDialogue.lines = dialogueLines;
                    newDialogue.interactableObject = this;
                }
            }
        }
    }
}
