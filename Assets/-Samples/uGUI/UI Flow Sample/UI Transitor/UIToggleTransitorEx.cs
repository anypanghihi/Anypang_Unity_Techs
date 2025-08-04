using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIToggleTransitorEx : UIToggleTransitor
{
    [Space(5)]
    [Header("내부 확장속성")]
    [SerializeField] List<ImageEx>      ExtendList;

    [Serializable]
    public class ImageEx
    {
        public Image              Image;
        public Color[]            Color;
    }


    void Awake()
    {
        OnChildExCall += OnToggleChangedExtended;
    }
    void OnDestory()
    {
        OnChildExCall -= OnToggleChangedExtended;
    }

    void OnToggleChangedExtended(int id)
    {
        foreach (ImageEx each in ExtendList)
        {
            if( each.Image != null )
                each.Image.color = each.Color[id];
        }
    }

    

}
