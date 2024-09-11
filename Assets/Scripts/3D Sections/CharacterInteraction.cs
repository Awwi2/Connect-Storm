using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField] CharacterMovement3D player;
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] GameObject textBox;

    public string[] firstEncounterLine;
    public string[] rematchLine;

    public float textSpeed;
    public string[] currentLines;

    [SerializeField] private int index;

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && player.lockMovement)
        {
            if (textComponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = currentLines[index];
            }
        }
    }

    public void PlayDialogue()
    {
        player.lockMovement = true;
        Cursor.lockState = CursorLockMode.None;
        textBox.SetActive(true);
        textComponent.text = string.Empty;
        switch (0) //INSERT HERE THE DIALOGUE CHECK
        {
            case 0:
                currentLines = firstEncounterLine;
                break;
            case 1:
                currentLines = rematchLine;
                break;
        }
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        string currentLine = currentLines[index];
        foreach (char c in currentLine.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
            if (new List<char>(currentLine.ToCharArray()).IndexOf(c) == currentLine.ToCharArray().Length - 1)
            {
                textComponent.text = currentLine;
            }
        }
    }

    void NextLine()
    {
        if (index < currentLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            player.lockMovement = false;
            Cursor.lockState = CursorLockMode.Locked;
            textBox.SetActive(false);
        }
    }
}
