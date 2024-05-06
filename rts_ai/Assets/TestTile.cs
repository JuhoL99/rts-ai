using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestTile : MonoBehaviour
{
    TMP_Text text;
    private Grid<int> grid;
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    public void setText(string text_)
    {
        text.text = $"{text_}";
    }

    
}
