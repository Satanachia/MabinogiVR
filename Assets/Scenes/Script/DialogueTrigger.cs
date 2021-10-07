using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public GameObject LetterCover;
    public GameObject Letter;
    public void TriggerDialogue()
    {
        LetterCover.SetActive(false);
        Letter.SetActive(true);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        Debug.Log("click!");
        
    }
}
