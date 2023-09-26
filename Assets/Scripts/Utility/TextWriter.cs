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
 *  [26/09/2023] - Removed animation handling (C137)
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
        if(Input.GetKeyDown(skipWriting) && autoSkip)
        {
            Skip();
        }
    }

    /// <summary>
    /// Skips the writing animation
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

}
