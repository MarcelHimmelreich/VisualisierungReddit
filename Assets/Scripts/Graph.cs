using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickType;

public class Graph : MonoBehaviour {

    public delegate void DepthCounter();
    public static event DepthCounter count;
    public static event DepthCounter CheckVelocity;

    public delegate void ForceVertex(int depth);
    public static event ForceVertex ApplyForce;

    //Reddit Submission Data Structure
    public Submission submission;
    public List<GameObject> Comments;

    public List<int> depth_counter = new List<int>();
    public List<int> depth_counter_done = new List<int>();
    public List<int> depth_counter_all = new List<int>();

    //Property
    public bool created = false;
    public bool castGraph = false;
    public GameObject Vertex_prefab;
    public float maxDistance = 50;
    public int depth = 0;



    public void SetGraph() {
    }

    // Use this for initialization
    void Start() {
        depth_counter_done.Add(0);

    }

    // Update is called once per frame
    void Update() {
        if (created && castGraph) {
            CastEdge();
        }

    }

    void OnEnable() {
        Vertex.SendVerticesCount += CountVertices;
        Vertex.SendDepth += AddDepthCounter; 
    }

    void OnDisable()
    {
        Vertex.SendVerticesCount -= CountVertices;
        Vertex.SendDepth -= AddDepthCounter;
    }

    public void CheckDepth(int depth) {
        Debug.Log("Send Depth Check:" + depth);
        if (depth_counter.Count > depth && depth_counter_done.Count > depth && depth > 0)
        {
            if (depth_counter[depth] == depth_counter_done[depth])
            {
                Debug.Log("Calculate next Layer");
                ApplyForce(depth+1);
            }

        }

    }

    public void Apply(int depth) {
        ApplyForce(depth);
    }

    public void AddDepthDone(int depth)
    {
        if (depth_counter_done.Count <= depth)
        {
            depth_counter_done.Add(1);
        }
        else if(depth > 0)
        {
            ++depth_counter_done[depth];
        }
        CheckDepth(depth);
    }

    public void AddDepthCounter(int depth)
    {
        if (depth_counter.Count <= depth)
        {
            depth_counter.Add(1);
        }
        else if (depth > 0)
        {
            ++depth_counter[depth];
        }
    }

    public void SetDepth() {
        foreach (GameObject comment in Comments) {
            comment.GetComponent<Vertex>().SetDepth(depth+1);
        }
    }

    public void CountVertices(int depth, int value) {
        if (depth > depth_counter_all.Count)
        {
            depth_counter_all.Add(value);
        }
        else if(depth > 0)
        {
            Debug.Log("Depth: " + (depth));
            Debug.Log("Depth Count:" + depth_counter_all.Count);
            depth_counter_all[depth-1] += value;
        }

    }

    public void CountAllVerticesDepth() {
        depth_counter_all.Clear();
        count();

    }

    public int getDepthCount(int depth) {
        int count = 0;

        return count;
    }

    public void getDepthNodes() {
        foreach (GameObject comment in Comments) {
            comment.GetComponent<Vertex>().getVerticesDepthCount();

        }
    }

    public void MakeParents() {
        foreach (GameObject comment in Comments) {
            comment.GetComponent<Vertex>().SetParent(this.transform);
            comment.GetComponent<Vertex>().MakeParent();
        }
    }

    public void CreateGraph(Submission submission) {
        foreach (Comment comment in submission.Comments) {
            Debug.Log("comment: " + comment);
            GameObject new_comment = Instantiate(Vertex_prefab) as GameObject;
            new_comment.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * maxDistance;
            Comments.Add(new_comment);
            new_comment.GetComponent<Vertex>().comment = comment;
            new_comment.GetComponent<Vertex>().Parent = this.gameObject;
            new_comment.GetComponent<Vertex>().Origin = this.gameObject;
            new_comment.GetComponent<Vertex>().Comments = new_comment.GetComponent<Vertex>().CreateComment(comment, new_comment, this.gameObject);

        }

    }

    public void CastEdge() {
    }
}
