using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour {

    //Send Events to Nodes in Graph
    public delegate void MaterialEvent(int depth, Material material);
    public static event MaterialEvent SendMaterial;

    public delegate void MaterialEventAuthor(int depth, string author, Material material);
    public static event MaterialEventAuthor SendMaterialAuthor;

    public delegate void ColorEvent(int depth, Color color);
    public static event ColorEvent SendColor;
    public static event ColorEvent SendColorShader;

    public delegate void ColorEventAuthor(int depth, string author, Color color);
    public static event ColorEventAuthor SendColorAuthor;

    public delegate void ColorGradientEvent(int depth, string attribute, Color startcolor, Color endcolor);
    public static event ColorGradientEvent SendColorGradient;

    public List<Material> Material;
    public List<Color> Colors;

    //Color Gradient
    public int depth_gradient;
    public string attribute_gradient;
    public Color start_color;
    public Color end_color;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SendMaterialToNode(int depth, string author, int id)
    {
        SendMaterialAuthor(depth,author,Material[id]);
    }

    public void SendMaterialToNode(int depth, int id)
    {
        SendMaterial(depth, Material[id]);
    }

    public void SendColorToNode(int depth, string author, Color color)
    {
        SendColorAuthor(depth, author, color);
    }

    public void SendColorToNode(int depth, int id)
    {
        SendColor(depth, Colors[id]);
    }

    public void SendColorToNode(int depth, Color color)
    {
        SendColorShader(depth, color);
    }

    public void SendColorGradientToNode()
    {
        SendColorGradient(depth_gradient, attribute_gradient, start_color, end_color);
    }
}
