using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextKill : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(FadeTextToZeroAlpha(2f, GetComponent<TextMeshProUGUI>()));    
    }
    
    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        yield return new WaitForSeconds(2f);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
