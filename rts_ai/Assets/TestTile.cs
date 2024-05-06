using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestTile : MonoBehaviour
{
    TMP_Text text;
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    public void setText(int weight)
    {
        text.text = weight.ToString();
    }

    
}
