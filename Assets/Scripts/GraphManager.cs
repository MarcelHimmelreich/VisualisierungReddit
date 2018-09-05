using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickType;

public class GraphManager : MonoBehaviour {

    public delegate void ForceGraph();
    public static event ForceGraph ApplyForce;
    public static event ForceGraph ApplyForceDepth;
    public static event ForceGraph CreateParentComments;

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

    // Vertex
    public List<GameObject> Vertices;

    // 
    public GameObject submission_prefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetDepthCounter() {

    }

    public void SetDepth() {
        foreach (GameObject submission in Submission) {
            submission.GetComponent<Graph>().SetDepth();
        }
    }

    public void MakeParents() {
        foreach (GameObject submission in Submission) {
            submission.GetComponent<Graph>().MakeParents();
        }
    }

    public void CreateGraph(int value = -1) {
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
        SetDepth();
        MakeParents();
        CreateParentComments();
        foreach (GameObject submission in Submission)
        {
            
            submission.GetComponent<Graph>().getDepthNodes();
            submission.GetComponent<Graph>().CountAllVerticesDepth();
            //submission.GetComponent<Graph>().Apply(1);
        }
        Debug.Log("Force Graph Configuration complete!");

    }


}
