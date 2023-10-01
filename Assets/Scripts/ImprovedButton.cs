/*
 * AudioHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 01/10/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [01/10/2023] - Initial Implementation (C137)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImprovedButton : Selectable, IPointerClickHandler, IPointerExitHandler /*, ISubmitHandler*/
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable())
            return;

        m_OnClick.Invoke();

    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        InstantClearState();
        DoStateTransition(SelectionState.Normal, true);
    }

    // Trigger all registered callbacks.
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Press();
    }

    //public virtual void OnSubmit(BaseEventData eventData)
    //{
    //    Press();

    //    // if we get set disabled during the press
    //    // don't run the coroutine.
    //    if (!IsActive() || !IsInteractable())
    //        return;

    //    DoStateTransition(SelectionState.Pressed, false);
    //    StartCoroutine(OnFinishSubmit());
    //}

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }
}
