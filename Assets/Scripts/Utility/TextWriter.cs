/*
 * TextWriter.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [12/09/2023] - Added delay (C137)
 */
using System.Collections;
using System.Collections.Generic;
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
    /// The text to be written
    /// </summary>
    [TextArea]
    public string text;

    /// <summary>
    /// Speed at which to write each character
    /// </summary>
    public float speed;

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
        coroutine = StartCoroutine(WriterCoroutine(delay));
    }

    private void Update()
    {
        if(Input.GetKeyDown(skipWriting))
        {
            if(coroutine != null) 
                StopCoroutine(coroutine);

            textShower.text = text;
        }
    }

    IEnumerator WriterCoroutine(float delay)
    {
        currentProgress = string.Empty;

        yield return new WaitForSeconds(delay);

        foreach (char c in text)
        {
            yield return new WaitForSeconds(speed);

            currentProgress += c;

            textShower.text = currentProgress;
        }
    }
}
