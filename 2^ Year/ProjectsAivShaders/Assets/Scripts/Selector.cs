using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField] private string shaderOutlineName;
    [SerializeField] private Transform[] objects;

    private Transform selected;
    private Shader textureShader;
    private Shader outlineShader;
    private int currentIndex;


    void Start () {
        textureShader = Shader.Find("Custom/textureSingleOutlineH");
        outlineShader = Shader.Find(shaderOutlineName);
    }
    private void Awake()
    {
        textureShader = Shader.Find("Custom/textureSingleOutlineH");
        outlineShader = Shader.Find(shaderOutlineName);
        currentIndex = 0;
        selected = objects[currentIndex];
        selected.GetComponent<Renderer>().material.shader = outlineShader;
        selected.GetComponent<Renderer>().material.shader = textureShader;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectNextObject();
        }
    }

    private void SelectNextObject()
    {
        selected.GetComponent<Renderer>().material.shader = textureShader;
        selected.GetComponent<Renderer>().material.SetFloat("_Border", 0f);
        

        currentIndex = ++currentIndex % objects.Length == 0 ? 0 : currentIndex;
        selected = objects[currentIndex];
        selected.GetComponent<Renderer>().material.shader = outlineShader;
        selected.GetComponent<Renderer>().material.shader = textureShader;
        selected.GetComponent<Renderer>().material.SetFloat("_Border", 0.2f);
        selected.GetComponent<Renderer>().material.SetColor("_BorderColor", Color.cyan);
    }
}
