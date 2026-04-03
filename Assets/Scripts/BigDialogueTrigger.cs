using System;
using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered)
        {
            return;
        }
        
        player.StopMoving(react);
        player.state = Player.State.NoMove;
        BigDialogue newBigDialogue = Instantiate(bigDialogue);
        newBigDialogue.lines = lines;
        newBigDialogue.talkChanges = changes;
        if (talker)
        {
            newBigDialogue.character1.isActiveSpeaker = true;
        }
        else
        {
            newBigDialogue.character2.isActiveSpeaker = true;
        }
        triggered = true;
    }
}
