using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class StatBarController : MonoBehaviour
{
    int Max;
    int Current;

    public Vector3 Offset_Horizontal;
    public Vector3 Offset_Vertical;

    public Image prefab;
    public Sprite sprite;
    public Color color;

    List<Image> Sprites;

    public void Initialize(int _max)
    {
        Max = Current = _max;
        Sprites = new List<Image>();
        Set(Max);
    }

    public void Set(int _current)
    {
        Current = _current;
        if (Current != Sprites.Count)
        {
            if (Current > Sprites.Count)
            {
                Add();
            }
            else
            {
                Remove();
            }
        }
    }

    public void Highlight(int _num) {
        throw new NotImplementedException();
    }

    void Add()
    {
        int diff = Current - Sprites.Count;
        int n, j;
        n = j = 0;
        while (n < diff)
        {
            for (int i = 0; i < Max; i++)
            {
                Image newSprite = Instantiate(prefab, this.transform);
                newSprite.sprite = sprite;
                newSprite.color = color;
                newSprite.transform.localPosition = (Offset_Horizontal * i) + (Offset_Vertical * j);
                Sprites.Add(newSprite);
                n++;
                if (n >= diff) break;
            }
            j++;
        }
    }

    void Remove()
    {
        int diff = Sprites.Count - Current;
        for (int n = 0; n < diff; n++)
        {
            Image temp = Sprites[Sprites.Count - 1];
            Sprites.RemoveAt(Sprites.Count - 1);
            Destroy(temp);
        }
    }
}
