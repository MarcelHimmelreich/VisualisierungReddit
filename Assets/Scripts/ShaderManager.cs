using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour {

    //Send Events to Nodes in Graph
    public delegate void MaterialEvent(int depth, string author, Material material);
    public static event MaterialEvent SendMaterial;

    public delegate void ShaderEvent(int depth, string author, Shader shader);
    public static event ShaderEvent SendShader;

    public delegate void ColorEvent(int depth, string author, Color color);
    public static event ColorEvent SendColor;

    public delegate void ColorGradientEvent(int depth, string attribute, Color startcolor, Color endcolor);
    public static event ColorGradientEvent SendColorGradient;

    public List<Material> Material;
    public List<Shader> Shader;
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
        SendMaterial(depth,author,Material[id]);
    }

    public void SendShaderToNode(int depth, string author, int id)
    {
        SendShader(depth,author, Shader[id]);
    }

    public void SendColorToNode(int depth, string author, int id)
    {
        SendColor(depth,author, Colors[id]);
    }

    public void SendColorGradientToNode()
    {
        SendColorGradient(depth_gradient, attribute_gradient, start_color, end_color);
    }
}
