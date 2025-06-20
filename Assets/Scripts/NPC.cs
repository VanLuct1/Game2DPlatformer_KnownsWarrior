using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;

    void Update()
    {
        if (dialogueText != null && index < dialogue.Length && dialogueText.text == dialogue[index])
        {
            if (contButton != null)
                contButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        if (dialogueText != null)
            dialogueText.text = "";

        index = 0;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        if (dialogueText == null || index >= dialogue.Length)
            yield break;

        dialogueText.text = "";

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (contButton != null)
            contButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            if (dialogueText != null)
                dialogueText.text = "";

            if (this.isActiveAndEnabled)
                StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;

            if (dialoguePanel != null && !dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);

                if (dialogueText != null)
                    dialogueText.text = "";

                index = 0;

                if (this.isActiveAndEnabled)
                    StartCoroutine(Typing());
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
