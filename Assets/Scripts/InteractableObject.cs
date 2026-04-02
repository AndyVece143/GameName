using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Player player;
    public string[] dialogueLines;
    public Dialogue dialogue;

    public bool interactable;
    public BoxCollider2D boxCollider;

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
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (interactable == true)
                {
                    interactable = false;
                    player.StopMoving();
                    player.state = Player.State.NoMove;

                    Dialogue newDialogue = Instantiate(dialogue);
                    newDialogue.lines = dialogueLines;
                    newDialogue.interactableObject = this;
                }
            }
        }
    }
}
