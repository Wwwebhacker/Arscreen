using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyBoardHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _InputField;

    private bool shift = false;

    public void OnButtonClick(string letter)
    {
        if (shift == false)
            _InputField.text += letter;
        else
            _InputField.text += letter.ToLower();
    }

    public void OnBackSpaceClick()
    {
        if (_InputField.text.Length > 0)
        {
            _InputField.text = _InputField.text.Remove(_InputField.text.Length - 1);
        }
    }

    public void CloseKeyBoardClick()
    {
        gameObject.transform.Find("KeyBoard Canvas").gameObject.SetActive(false);
    }

    public void OnShiftClick()
    {
        shift = !shift;
    }
}
