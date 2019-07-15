﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagListGameObject : MonoBehaviour
{
    public NewsGameObject newsGameObject = null;

    private GameObject tagPreFab;

    private void Awake()
    {
        tagPreFab = (GameObject)Resources.Load("Prefabs/Tag", typeof(GameObject));
    }

    private void OnEnable()
    {
        if (newsGameObject != null)
        {
            foreach (string tag in newsGameObject.Tags)
            {
                Color color;
                if (StaticClass.tagPrefColorList.ContainsKey(tag))
                {
                    color = StaticClass.tagPrefColorList[tag];
                }
                else
                {
                    color = Color.white;
                }
                color.a = 100f / 255f;
                GameObject tagGO = Instantiate(tagPreFab, transform);
                tagGO.GetComponent<Image>().color = color;
                tagGO.GetComponentInChildren<Text>().text = tag;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
