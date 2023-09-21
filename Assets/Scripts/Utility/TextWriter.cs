/*
 * TextWriter.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [12/09/2023] - Added delay + auto start & skip + auto set text shower (C137)
 *  [21/09/2023] - Added html tag support (C137)
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    /// <summary>
    /// The text shower used to display the text
    /// </summary>
    public TextMeshProUGUI textShower;

    /// <summary>
    /// Key used to skip the writing effect
    /// </summary>
    public KeyCode skipWriting = KeyCode.Return;

    /// <summary>
    /// Whether there is currently a writing animation
    /// </summary>
    public bool writing = false;

    /// <summary>
    /// The text to be written
    /// </summary>
    [TextArea]
    public string text;

    /// <summary>
    /// Speed at which to write each character
    /// </summary>
    public float speed;

    /// <summary>
    /// Whether to allow for auto start
    /// </summary>
    public bool autoStart = true;

    /// <summary>
    /// Whether to allow for auto skip
    /// </summary>
    public bool autoSkip = true;

    /// <summary>
    /// The delay before starting to write
    /// </summary>
    public float delay = 1f;

    /// <summary>
    /// The coroutine used for the writing effect
    /// </summary>
    public Coroutine coroutine;

    /// <summary>
    /// The progress of writing the text
    /// </summary>
    string currentProgress;

    public void Start()
    {
        textShower = textShower == null ? GetComponent<TextMeshProUGUI>() : textShower;

        if (autoStart)
            Write();
    }

    public void Write()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(WriterCoroutine(delay));
    }

    private void Update()
    {
        AnimateText();
        if(Input.GetKeyDown(skipWriting) && autoSkip)
        {
            Skip();
        }
    }

    /// <summary>
    /// Skip the writing animation
    /// </summary>
    public void Skip()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        textShower.text = text;
        writing = false;
    }

    IEnumerator WriterCoroutine(float delay)
    {
        currentProgress = string.Empty;

        yield return new WaitForSeconds(delay);

        writing = true;

        bool writingTag = false;
        foreach (char c in text)
        {
            if (c == '<')
                writingTag = true;

            if (!writingTag)
                yield return new WaitForSeconds(speed);
            else
            {
                if (c == '>')
                    writingTag = false;
            }

            currentProgress += c;

            textShower.text = currentProgress;
        }

        writing = false;
    }

    void AnimateText()
    {
        int movementSpeed = 5;
        int rainbowStrength = 10;
        Vector2 movementStrength = new(0.1f, 0.1f);

        textShower.ForceMeshUpdate();

        // Loops each link tag
        foreach (TMP_LinkInfo link in textShower.textInfo.linkInfo)
        {

            // Is it a rainbow tag? (<link="rainbow"></link>)
            if (link.GetLinkID() == "wavy")
            {

                // Loops all characters containing the rainbow link.
                for (int i = link.linkTextfirstCharacterIndex; i < link.linkTextfirstCharacterIndex + link.linkTextLength; i++)
                {
                    TMP_CharacterInfo charInfo = textShower.textInfo.characterInfo[i]; // Gets info on the current character
                    int materialIndex = charInfo.materialReferenceIndex; // Gets the index of the current character material

                    Color32[] newColors = textShower.textInfo.meshInfo[materialIndex].colors32;
                    Vector3[] newVertices = textShower.textInfo.meshInfo[materialIndex].vertices;

                    // Loop all vertexes of the current characters
                    for (int j = 0; j < 4; j++)
                    {
                        if (charInfo.character == ' ') continue; // Skips spaces
                        int vertexIndex = charInfo.vertexIndex + j;

                        // Offset and Rainbow effects, replace it with any other effect you want.
                        Vector3 offset = new Vector2(Mathf.Sin((Time.realtimeSinceStartup * movementSpeed) + (vertexIndex * movementStrength.x)), Mathf.Cos((Time.realtimeSinceStartup * movementSpeed) + (vertexIndex * movementStrength.y))) * 10f;
                        //Color32 rainbow = Color.HSVToRGB(((Time.realtimeSinceStartup * movementSpeed) + (vertexIndex * (0.001f * rainbowStrength))) % 1f, 1f, 1f);

                        // Sets the new effects
                        //newColors[vertexIndex] = rainbow;
                        newVertices[vertexIndex] += offset;
                    }
                }
            }
        }

        textShower.UpdateVertexData(TMP_VertexDataUpdateFlags.All); // IMPORTANT! applies all vertex and color changes.
    }
}
