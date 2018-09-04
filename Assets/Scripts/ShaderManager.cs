using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour {

    public delegate void MaterialEvent(string author, Material material);
    public static event MaterialEvent SendMaterial;
    public delegate void ShaderEvent(string author, Shader material);
    public static event ShaderEvent SendShader;
    public delegate void ColorEvent(string author, Color material);
    public static event ColorEvent SendColor;

    public List<Material> Material;
    public List<Shader> Shader;
    public List<Color> Colors;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SendMaterialToNode(string author, int id){
        SendMaterial(author,Material[id]);
    }

    public void SendShaderToNode(string author, int id)
    {
        SendShader(author, Shader[id]);
    }

    public void SendColorToNode(string author, int id)
    {
        SendColor(author, Colors[id]);
    }
}
