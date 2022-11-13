using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private SpriteRenderer _sr;
    public SpriteRenderer SR
    {
        get
        {
            if (_sr == null)
                _sr = GetComponent<SpriteRenderer>();
            return _sr;
        }
    }

    private void Start()
    {
        ColorChange();
    }

    public void ColorChange()
    {
        SR.color = Random.ColorHSV();
    }

    public void ResetColor()
    {
        SR.color = Color.white;
    }
}
