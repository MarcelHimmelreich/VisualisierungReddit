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

    public delegate void InterfaceSender(Submission submission);
    public static event InterfaceSender SendSubmission;


    //Reddit Submission Data Structure
    public Submission submission;
    public List<GameObject> CommentNode = new List<GameObject>();
    public List<GameObject> Nodes = new List<GameObject>();
    public List<List<GameObject>> NodesPerDepth = new List<List<GameObject>>();

    public List<int> depth_counter = new List<int>();
    public List<int> depth_counter_done = new List<int>();
    public List<int> depth_counter_all = new List<int>();

    public GameObject prefab_orbit;
    public List<GameObject> orbit_list;

    //Property
    public bool created = false;
    public bool castGraph = false;
    public GameObject Vertex_prefab;
    public float max_distance = 50;
    public int depth = 0;
    public int max_depth = 0;

    // Use this for initialization
    void Start() {
        depth_counter_done.Add(0);
        NodesPerDepth.Add(new List<GameObject>());
    }

    // Update is called once per frame
    void Update() {
        if (created && castGraph) {
            CastEdge();
        }

    }

    void OnEnable() {
        Node.SendVerticesCount += CountVertices;
        Node.SendDepth += AddDepthCounter;
        UserInterfaceManager.CreateOrbit += CreateOrbit;
        UserInterfaceManager.DeleteOrbit += DeleteOrbit;
    }

    void OnDisable()
    {
        Node.SendVerticesCount -= CountVertices;
        Node.SendDepth -= AddDepthCounter;
        UserInterfaceManager.CreateOrbit -= CreateOrbit;
        UserInterfaceManager.DeleteOrbit -= DeleteOrbit;
    }

    void OnMouseDown()
    {
        SendSubmission(submission);
    }

    public void CheckDepth(int depth) {
        Debug.Log("Send Depth Check:" + depth);
        if (depth_counter.Count > depth && depth_counter_done.Count > depth && depth > 0)
        {
            if (depth_counter[depth] == depth_counter_done[depth])
            {
                Debug.Log("Calculate next Layer");
                //ApplyForce(depth+1);
            }

        }

    }

    public void AddNodes(int depth, GameObject node)
    {
        if (depth > max_depth)
        {
            for (int i = 0; i < depth - max_depth; ++i)
            {
                NodesPerDepth.Add(new List<GameObject>());
            }
        }
        Debug.Log("NodesPerDepth Count: " + NodesPerDepth.Count);
        if (!Nodes.Contains(node))
        {
            Nodes.Add(node);
        }
        if (depth < NodesPerDepth.Count)
        {
            NodesPerDepth[depth].Add(node);
        }
    }

    public List<GameObject> GetNodesPerDepth(int depth)
    {
        if (depth == 0)
        {
            return Nodes;
        }
        else if (depth < NodesPerDepth.Count)
        {
            return NodesPerDepth[depth];
        }
        else
        {
            return Nodes;
        }
    }

    public void ApplyForcePerDepth(int depth) {
        ApplyForce(depth);
    }

    public void AddDepthDone(int depth)
    {
        if (depth_counter_done.Count <= depth)
        {
            depth_counter_done.Add(1);
        }
        else if (depth > 0)
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

    //Count Nodes for a single depth
    public void CountVertices(int depth, int value) {
        if (depth > depth_counter_all.Count)
        {
            depth_counter_all.Add(value);
        }
        else if (depth > 0)
        {
            Debug.Log("Depth: " + (depth));
            Debug.Log("Depth Count:" + depth_counter_all.Count);
            depth_counter_all[depth - 1] += value;
        }

    }

    //Counts nodes for every depth by calling a event to all nodes
    public void CountAllVerticesDepth() {
        depth_counter_all.Clear();
        count();

    }

    public int getDepthCount(int depth) {
        int count = 0;

        return count;
    }

    public void getDepthNodes() {
        foreach (GameObject comment in CommentNode) {
            comment.GetComponent<Node>().getVerticesDepthCount();

        }
    }

    public void CheckMaxdepth(int depth)
    {
        if (depth > max_depth)
        {
            max_depth = depth;
        }
    }

    //Set the parent of a node  to create a tree like structure in the game scene
    public void MakeParents() {
        foreach (GameObject comment in CommentNode) {
            comment.GetComponent<Node>().SetParent(this.transform);
            comment.GetComponent<Node>().MakeParent();
        }
    }

    //Create a graph from a submission with every comment
    public void CreateGraph(Submission submission) {
        foreach (Comment comment in submission.Comments) {
            Debug.Log("comment: " + comment);
            GameObject new_comment = Instantiate(Vertex_prefab) as GameObject;
            new_comment.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * max_distance;
            CommentNode.Add(new_comment);
            new_comment.GetComponent<Node>().comment = comment;
            new_comment.GetComponent<Node>().Parent = this.gameObject;
            new_comment.GetComponent<Node>().Origin = this.gameObject;
            new_comment.GetComponent<Node>().depth = 1;
            //Calls a recursive function for every comment
            new_comment.GetComponent<Node>().CommentNode = new_comment.GetComponent<Node>().CreateComment(comment, new_comment, this.gameObject);

        }

    }

    public float GetMaxValue(string attribute)
    {
        Debug.Log("Attribute " + attribute);
        float maxvalue = 0;
        if (CommentNode.Count == 0)
        {
            return maxvalue;
        }
        else
        {
            foreach (GameObject _comment in CommentNode)
            {
                float commentvalue = _comment.GetComponent<Node>().GetMaxValue(attribute); ;
                if (commentvalue > maxvalue)
                {
                    maxvalue = commentvalue;
                }

            }
            return maxvalue;
        }
    }

    public void CastEdge() {
    }

    public void CreateOrbit(int _depth, Color color, float maxdistance)
    {
        bool overwrite_orbit = false;
        foreach(GameObject orbit in orbit_list)
        {
            if (depth == orbit.GetComponent<Orbit>().depth)
            {
                overwrite_orbit = true;
            }
        }
        if (overwrite_orbit)
        {
            DeleteOrbit(_depth);
            GameObject temp_orbit = Instantiate(prefab_orbit, transform.position, transform.rotation) as GameObject;
            temp_orbit.GetComponent<Orbit>().depth = _depth;
            temp_orbit.GetComponent<Renderer>().material.color = color;
            temp_orbit.transform.localScale = new Vector3(maxdistance* _depth * 2, maxdistance* _depth * 2, maxdistance* _depth * 2);
            orbit_list.Add(temp_orbit);
        }
        else
        {
            GameObject temp_orbit = Instantiate(prefab_orbit, transform.position, transform.rotation) as GameObject;
            temp_orbit.GetComponent<Orbit>().depth = _depth;
            temp_orbit.GetComponent<Renderer>().material.color = color;
            temp_orbit.transform.localScale = new Vector3(maxdistance * _depth * 2, maxdistance * _depth * 2, maxdistance * _depth * 2);
            orbit_list.Add(temp_orbit);
        }
    }

    public void DeleteOrbit(int depth)
    {
        foreach (GameObject orbit in orbit_list)
        {
            if (orbit.GetComponent<Orbit>().depth == depth || depth == 0)
            {
                GameObject temp_orbit = orbit;
                orbit_list.Remove(temp_orbit);
                Destroy(temp_orbit);
                DeleteOrbit(depth);
                break;
            }
            

        }

    }
}
