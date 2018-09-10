using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class GraphManager : MonoBehaviour {

    public delegate void ForceGraph();
    public static event ForceGraph ApplyForce;
    public static event ForceGraph ApplyForceDepth;
    public static event ForceGraph CreateParentComments;
    public static event ForceGraph PrintData;
    public static event ForceGraph GetMaxDepth;
    public static event ForceGraph AddNodesToGraph;
    public static event ForceGraph EnableForce;
    public static event ForceGraph DisableForce;
    public static event ForceGraph PushNode;

    public delegate void GraphTransform(int depth, string attribute, float maxvalue);
    public static event GraphTransform SendTransform;

    public delegate void MeshSpawn(int depth, GameObject mesh);
    public static event MeshSpawn Spawn;

    public delegate void MeshDestroy(int depth);
    public static event MeshDestroy DestroyMesh;

    public delegate void Highlight(string author, Material highlight);
    public static event Highlight HighlightAuthor;

    // View
    public GameObject UserInterface;
    public UserInterfaceManager UIComponent;
    // Model
    public GameObject Model;
    public Model ModelComponent;
    // Shader Manager
    public GameObject ShaderManager;
    public ShaderManager ShaderManagerComponent;

    // Graph
    public List<GameObject> Submission;
    public int createdGraph = 0;

    public GameObject submission_prefab;

    //Mesh Objects
    public List<GameObject> Mesh;

    //Dimension Variables

    public string attribute_transform = "upvote";
    public Text attribute_transform_text;
    public string attribute_color = "upvote";

    public int transform_depth = 0;
    public int color_depth = 0;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ForceEnable()
    {
        EnableForce();
    }

    public void ForceDisable()
    {
        DisableForce();
    }

    public void PushNodes()
    {
        PushNode();
    }

    public void SendPrintData()
    {
        PrintData();
    }

    public void SendSpawn(int depth)
    {
        Spawn(depth, Mesh[0]);
    }

    public void SendDestroyMesh(int depth)
    {
        DestroyMesh(depth);
    }

    public void MaxDepth()
    {
        GetMaxDepth();
    }

    public void CreateNodesList()
    {
        AddNodesToGraph();
    }


    public void SendTransformToNodes(int depth, int submission)
    {
        float maxvalue = 0;
        if (Submission.Count > 1)
        {
            maxvalue = Submission[submission].GetComponent<Graph>().GetMaxValue(attribute_transform); 
        }
        else
        {
            maxvalue = Submission[0].GetComponent<Graph>().GetMaxValue(attribute_transform);
        }
        Debug.Log("Max Value: "  + maxvalue);
        SendTransform(depth, attribute_transform, maxvalue);
    }
    public void SetTransformAttribute(string value)
    {
        attribute_transform_text.text = value;
        attribute_transform = value;
    }

    public void MakeParents() {
        foreach (GameObject submission in Submission) {
            submission.GetComponent<Graph>().MakeParents();
        }
    }


    //Create a single graph by id in a array or all submissions
    public void CreateGraph(int value = -1) {
        if (Submission.Count == 1)
        {
            DeleteGraph(0);
        }
        Debug.Log("Creating Force Graph...");
        createdGraph = value;
        if (value >= 0)
        {
            Submission submission = Model.GetComponent<Model>().subreddit.Submission[value];
            GameObject new_submission = Instantiate(submission_prefab);
            new_submission.GetComponent<Graph>().submission = submission;
            new_submission.GetComponent<Graph>().CreateGraph(submission);
            Submission.Add(new_submission);
        }
        else
        {
            createdGraph = 0;
            foreach (Submission submission in Model.GetComponent<Model>().subreddit.Submission)
            {
                Debug.Log("submission: " + submission);
                GameObject new_submission = Instantiate(submission_prefab);
                new_submission.GetComponent<Graph>().submission = submission;
                new_submission.GetComponent<Graph>().CreateGraph(submission);
                Submission.Add(new_submission);
            }
        }
        Debug.Log("Force Graph Creation complete!");
        Debug.Log("Configure Force Graph...");
        MakeParents();
        CreateParentComments();
        MaxDepth();
        CreateNodesList();
        //SendSpawn(0);
        UserInterface.GetComponent<UserInterfaceManager>().InitializeGraph();
        foreach (GameObject submission in Submission)
        {
            
            submission.GetComponent<Graph>().getDepthNodes();
            //submission.GetComponent<Graph>().CountAllVerticesDepth();
            //submission.GetComponent<Graph>().Apply(1);
        }
        Debug.Log("Force Graph Configuration complete!");

    }

    public void DeleteGraph(int submission)
    {
        //submission is in list
        if (submission < Submission.Count && submission >= 0)
        {
            GameObject submission_die = Submission[submission];
            Submission.Remove(submission_die);
            Destroy(submission_die.gameObject);
        }
    }


}
