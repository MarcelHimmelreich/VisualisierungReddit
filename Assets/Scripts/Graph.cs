using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickType;

public class Graph : MonoBehaviour {

    public delegate void DepthCounter();
    public static event DepthCounter count;

    public delegate void ForceVertex();
    public static event ForceVertex ApplyForce;

    //Reddit Submission Data Structure
    public Submission submission;
    public List<GameObject> Comments;

    public List<int> depth_counter;
    public List<int> depth_counter_all;

    //Property
    public bool created = false;
    public bool castGraph = false;
    public GameObject Vertex_prefab;
    public int depth = 0;



    public void SetGraph() {
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (created && castGraph) {
            CastEdge();
        }

    }

    void OnEnable() {
        Vertex.SendVerticesCount += CountVertices;
    }

    void OnDisable()
    {
        Vertex.SendVerticesCount -= CountVertices;
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
        else
        {
            depth_counter_all[depth] += value;
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
