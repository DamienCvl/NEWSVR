using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagListGameObject : MonoBehaviour
{
    public NewsGameObject newsGameObject;

    private GameObject tagPreFab;

    private void Awake()
    {
        tagPreFab = (GameObject)Resources.Load("Prefabs/Tag", typeof(GameObject));
    }

    private void OnEnable()
    {
        foreach (string tag in newsGameObject.Tags)
        {
            Color color = StaticClass.tagPrefColorList[tag];
            color.a = 100f / 255f;
            GameObject tagGO = Instantiate(tagPreFab, transform);
            tagGO.GetComponent<Image>().color = color;
            tagGO.GetComponentInChildren<Text>().text = tag;
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
