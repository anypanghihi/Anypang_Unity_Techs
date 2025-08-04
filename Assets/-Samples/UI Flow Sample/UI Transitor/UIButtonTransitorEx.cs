using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIButtonTransitorEx : UIButtonTransitor
{
    [Space(5)]
    [Header("내부 확장속성")]
    [SerializeField] List<ImageEx> ExtendList;

    [Serializable]
    public class ImageEx
    {
        public Image Image;
        public Color[] Color;
    }


    void Awake()
    {
        OnChildExCall += OnButtonChangedExtended;
    }
    void OnDestory()
    {
        OnChildExCall -= OnButtonChangedExtended;
    }

    void OnButtonChangedExtended(int id)
    {
        foreach (var item in ExtendList)
        {
            if (id < item.Color.Length)
                item.Image.color = item.Color[id];
        }
    }



}
