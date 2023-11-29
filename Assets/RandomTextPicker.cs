using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTextPicker : MonoBehaviour
{
    public string[] Texts;
    public TMP_Text _text;
    // Start is called before the first frame update
    void OnEnable()
    {
        _text.text = Texts[Random.Range(1, Texts.Length)];
        
    }


}
