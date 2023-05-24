using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MinimizeHandler : MonoBehaviour
{

    private Dictionary<string, string> map = new Dictionary<string, string>()
    {
        { "WindowWithPaint(Clone)", "Draw" },
        { "WindowWithPicture(Clone)", "Image"},
        { "WindowYellow(Clone)", "Card"},
        { "WindowWall(Clone)", "Wall"}
    };

    private List<Window> windows = new List<Window>();


    public void Minimize(Window window)
    {
        if (windows.Count >= 3) return;

        transform.GetChild(windows.Count).GetComponentInChildren<TextMeshProUGUI>().text = map[window.name];
        transform.GetChild(windows.Count).gameObject.SetActive(true);
        windows.Add(window);
        window.gameObject.SetActive(false);
    }

    public void Unminimize(int id)
    {
        windows[id].gameObject.SetActive(true);
        windows.RemoveAt(id);
        GameObject lastActiveChild = null;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf) lastActiveChild = child.gameObject;
        }
        lastActiveChild.SetActive(false);
    }


}
