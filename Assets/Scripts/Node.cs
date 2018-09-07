﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickType;

public class Node : MonoBehaviour {

    public delegate void NodeSender(int depth, int value);
    public static event NodeSender SendVerticesCount;
    public static event NodeSender SendVerticesDepthCount;

    public delegate void NodeDepthSender(int depth);
    public static event NodeDepthSender SendDepth;

    public delegate void InterfaceSender(Comment comment);
    public static event InterfaceSender SendComment;

    //Reddit Comment Data Structure
    public Comment comment;
    public List<GameObject> CommentNode;
    public List<GameObject> ParentCommentNode;

    //Depth for Graph in Commentforest
    public int depth = 0;
    public int comment_count = 0;

    //Component Properties
    public Material mat;
    public Transform Node_transform;
    public Vector3 Position;

    //Force Graph
    public Rigidbody rigidbody;
    public GameObject Origin;
    public GameObject Parent;

    public Vector3 velocity;
    public Vector3 velocity_origin;


    public List<Vector3> velocity_neighbor;
    public List<float> neighbour_distance;

    public float force = 1;
    public float force_neighbor = 1;
    public float force_sphere = 1;

    public float max_velocity = 5;
    public float min_velocity = 0.1f;
    public float velocity_magnitude;

    public float max_parent_distance = 10;
    public float min_parent_distance = 10;
    public float range_distance = 1;

    public float max_neighbour_distance = 5;
    public float min_neighbour_distance = 5;
    public float neighbour_range_distance = 1;



    public float parent_distance = 0;
    public float origin_distance = 0;

    public bool apply_physics = true;
    public bool apply_force = false;
    public bool apply_force_neighbour = false;
    public bool apply_force_origin = true;

    public bool check_force = true;
    public bool draw_line = true;

    public Vector3 neighbour_direction;
    public int closest_neighbour = 0;
    public float closest_neighbour_distance = 0;
    public List<GameObject> Near_Neighbour;

    public GameObject Node_prefab;

    public int counter = 0;

    //Vertex Mesh
    public GameObject NodeMesh;

    //Dimension Change
    public bool marked = false;
    public float maxscale = 3;

    // Use this for initialization
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        mat = this.GetComponent<Material>();

    }

    // Update is called once per frame
    void Update()
    {
        velocity_magnitude = rigidbody.velocity.magnitude;
        if (apply_physics)
        {
            CalculateVelocity();
            ApplyForce();
            CheckVelocity();

            CalculateNeighborVelocity();
            ApplyForceNeighbour();
            CheckNeighbourDistance();

            CalculateSphereVelocity();
            ApplySphereForce();
            CheckParentDistance();

            CheckVelocityDirection();
            //CheckPositionComplete();
        }
        if (draw_line) {
            SetLine();
        }
    }

    void OnEnable()
    {
        Graph.count += sendVerticesCount;
        Graph.ApplyForce += SetApplyByDepth;
        GraphManager.CreateParentComments += GetParentComments;
        GraphManager.PrintData += Print;
        GraphManager.SendTransform += SetTransformScale;
        GraphManager.Spawn += CreateMesh;
        GraphManager.Destroy += DestroyMesh;
        ShaderManager.SendMaterial += SetMaterial;

    }

    void OnDisable()
    {
        Graph.count -= sendVerticesCount;
        Graph.ApplyForce -= SetApplyByDepth;
        GraphManager.CreateParentComments -= GetParentComments;
        GraphManager.PrintData -= Print;
        GraphManager.SendTransform -= SetTransformScale;
        GraphManager.Spawn -= CreateMesh;
        GraphManager.Destroy -= DestroyMesh;
        ShaderManager.SendMaterial -= SetMaterial;
    }

    void OnMouseDown()
    {
        SendComment(comment);
    }

    public void SetApplyByDepth(int _depth)
    {
        if (depth == _depth)
        {
            if (apply_physics)
            {
                apply_physics = false;
            }
            else
            {
                apply_physics = true;
            }
            ApplyForce(10);
        }
    }

    public void SetLine(){
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,Parent.transform.position);
    }

    public void SetColliderRadius(float radius) {
        GetComponent<SphereCollider>().radius = radius;

    }

    public void SetMaxDistance(float distance) {
        if (distance == 0) {

            max_parent_distance = ParentCommentNode.Count/3;
        }
        else
        {
            max_parent_distance = distance;
        }

    }

    public void CheckVelocityDirection()
    {
        if (velocity == new Vector3(-1, -1, -1))
        {
            velocity_magnitude = 0;
            apply_physics = false;
        }
    }

    public void CheckPositionComplete()
    {
        if (!apply_force && !apply_force_neighbour && !apply_force_origin)
        {
            //Origin.GetComponent<Graph>().AddDepthDone(depth);
            apply_physics = false;
        }
    }

    public void CheckVelocity() {
        float distance = Vector3.Distance(Parent.transform.position, transform.position);

        //Check if Distance is in MaxDistance or velocity is decreasing
        if (velocity_magnitude < min_velocity && distance < max_parent_distance - range_distance || velocity_magnitude < min_velocity && distance > max_parent_distance + range_distance)
        {
            apply_force = false;
            //applyforceneighbour = true;
            velocity = new Vector3(-1, -1, -1);

        }
        else
        {
            apply_force = true;
        }
    }

    public void CheckNeighbourDistance(){
        float distance = Vector3.Distance(ParentCommentNode[closest_neighbour].transform.position, transform.position);

        if (distance > min_neighbour_distance && velocity.magnitude < min_velocity)
        {
            apply_force_neighbour = true;
        }
        else
        {
            apply_force_neighbour = false;
            
        }

    }

    public void CheckParentDistance() {
        float distanceparent = Vector3.Distance(Parent.transform.position, transform.position);
        float distanceorigin = Vector3.Distance(Origin.transform.position, Parent.transform.position);

        if (distanceparent > distanceorigin)
        {
            //applyforceorigin = true;
        }
        else
        {
            //ApplySphereForce(true);
            //applyforceorigin = false;
        }

    }

    public void CalculateVelocity() {
        parent_distance = Vector3.Distance(Parent.transform.position, transform.position);
        
        if (parent_distance < max_parent_distance - range_distance)
        {
            velocity = Parent.transform.position - transform.position;
            velocity = -velocity;
            check_force = true;
        }
        else if(parent_distance > max_parent_distance + range_distance)
        {
            velocity = Parent.transform.position - transform.position;

            check_force = true;
        }
        //Is in range
        else {
            //velocity = new Vector3(0,0,0);
            rigidbody.AddRelativeForce(-velocity.normalized * force, ForceMode.Force);
            check_force = false;
        }
        velocity = velocity.normalized;
    }

    public void CalculateSphereVelocity()
    {
        origin_distance = Vector3.Distance(Origin.transform.position, transform.position);
        float parentorigindistance = Vector3.Distance(Origin.transform.position, Parent.transform.position);
        if (origin_distance < max_parent_distance * depth)
        {
            velocity_origin = Origin.transform.position - transform.position;
            velocity_origin = -velocity_origin;

        }
        else if (origin_distance > max_parent_distance * depth + max_parent_distance)
        {
            velocity_origin = Origin.transform.position - transform.position;
            //rigidbody.AddRelativeForce(velocity_origin * force/2, ForceMode.Force);


        }
        //Is in range
        else
        {
            //velocity = new Vector3(0,0,0);
            rigidbody.AddRelativeForce(-velocity_origin * force, ForceMode.Force);
            
        }
        velocity_origin = velocity_origin.normalized;
    }

    public void CalculateNeighborVelocity()
    {
        GetClosestNeighbour();
        if (closest_neighbour_distance > 0)
        {
            if (closest_neighbour_distance < max_neighbour_distance - range_distance)
            {
                neighbour_direction = -(ParentCommentNode[closest_neighbour].transform.position - transform.position).normalized;

            }
            else if (closest_neighbour_distance > max_neighbour_distance + range_distance)
            {
                //neighbourdirection = (ParentComments[closestneighbour].transform.position - transform.position);
            }
        }
    }



    public void ApplyForce(float power = 1) {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        rigidbody.AddRelativeForce(velocity * force, ForceMode.Force);
    }

    public void ApplySphereForce(bool inverse = false) {
        if (!inverse)
        {
            rigidbody.AddRelativeForce(velocity_origin * force_sphere, ForceMode.Force);
        }
        else
        {
            rigidbody.AddRelativeForce(-velocity_origin * force_sphere, ForceMode.Force);
        }

    }

    //Apply Force from Every Neighbour
    public void ApplyForceNeighbour()
    {
        if (ParentCommentNode.Count > 1)
        {
            if (!float.IsNaN(neighbour_direction.x) && !float.IsNaN(neighbour_direction.y) && !float.IsNaN(neighbour_direction.z))
            {
                rigidbody.AddRelativeForce(neighbour_direction * force_neighbor, ForceMode.Force);
            }

        }
    }

    public void GetClosestNeighbour() {
        for(int i = 0;i< ParentCommentNode.Count;++i){
            if (ParentCommentNode[i].GetComponent<Node>().comment.Id != comment.Id)
            {
                float distance = Vector3.Distance(ParentCommentNode[i].transform.position, transform.position);
                if (distance < closest_neighbour_distance && distance > 0 || closest_neighbour_distance == 0)
                {
                    closest_neighbour_distance = distance;
                    closest_neighbour = i;
                }
                
            }
            else
            {
                
            }
            
        }
    }

    public void sendVerticesCount() {
            int vertices_count = getVerticesCount();
            SendVerticesCount(depth, vertices_count);
    }

    public void sendVerticesDepthCount() {
        Debug.Log("Send Depth" + depth);
        //SendDepth(depth);
    }

    public void SetDepth(int new_depth) {
        depth = new_depth;
        foreach (GameObject comment in CommentNode)
        {
            comment.GetComponent<Node>().SetDepth(depth+1);
        }

    }


    //rekursiv function
    public int getVerticesCount(int value = 0) {
        int count = value;
        if (CommentNode.Count == 0) {
            return 1;
        }
        else if (CommentNode.Count > 0) {
            foreach (GameObject node in CommentNode) {
                count += node.GetComponent<Node>().getVerticesCount();
            }
        }
        comment_count = count;
        return ++count;
    }

    public void getVerticesDepthCount()
    {
            sendVerticesDepthCount();
            foreach (GameObject node in CommentNode)
            {
                node.GetComponent<Node>().getVerticesDepthCount();
            }
    }

    public List<GameObject> CreateComment(Comment comment, GameObject parent, GameObject origin)
    {
        List<GameObject> comments = new List<GameObject>();
        if (comment.Comments.CommentArray != null)
        {
            foreach (Comment new_comment in comment.Comments.CommentArray)
            {
                GameObject new_comment_object = Instantiate(Node_prefab) as GameObject;
                new_comment_object.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * max_parent_distance;
                comments.Add(new_comment_object);
                new_comment_object.GetComponent<Node>().comment = new_comment;
                new_comment_object.GetComponent<Node>().Origin = origin;
                new_comment_object.GetComponent<Node>().Parent = parent;
                new_comment_object.GetComponent<Node>().CommentNode = CreateComment(new_comment_object.GetComponent<Node>().comment, 
                    new_comment_object, 
                    new_comment_object.GetComponent<Node>().Origin);             
                
            }
        }
        else
        {

        }
        return comments;

    }
    public void MakeParent() {
        Debug.Log("Make Parents");
        if (CommentNode != null || CommentNode.Count == 0)
        {
            foreach (GameObject comment in CommentNode)
            {
                comment.GetComponent<Node>().SetParent(this.transform);
                comment.GetComponent<Node>().MakeParent();
            }
        }

            
    }

    public void SetParent(Transform transform) {
        this.transform.SetParent(transform);
    }

    public void GetParentComments() {
        if (depth > 1)
        {
            ParentCommentNode = Parent.GetComponent<Node>().CommentNode;
        }
        else
        {
            ParentCommentNode = Parent.GetComponent<Graph>().CommentNode;
        }
        

    }

    public void SetTransformScale(int _depth, string attribute, float maxvalue)
    {
        float value = 1+GetAttributeValue(attribute) * maxscale / maxvalue;
        if (!float.IsNaN(value) && NodeMesh != null)
        {
            if (depth > 0 && depth == _depth)
            {
                NodeMesh.transform.localScale = new Vector3(value, value, value);
            }
            else if(_depth == 0)
            {
                NodeMesh.transform.localScale = new Vector3(value, value, value);
            }
        }
    }

    public float GetAttributeValue(string attribute)
    {
        if (attribute.Equals("score"))
        {
            return comment.Score;          
        }
        else if (attribute.Equals("upvote"))
        {
            return comment.Upvote;
        }
        else if (attribute.Equals("downvote"))
        {
            return comment.Downvote;
        }
        else if (attribute.Equals("likes"))
        {
            return comment.Likes;
        }
        else
        {
            return comment.Upvote;
        }

    }

    public void Print()
    {
        Debug.Log("Likes: " + comment.Likes);
        Debug.Log("Score: " + comment.Score);
        Debug.Log("Upvote: " + comment.Upvote);
        Debug.Log("Downvote: " + comment.Downvote);
    }

    public float GetMaxValue(string attribute)
    {
        float maxvalue = GetAttributeValue(attribute);
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

    public void SetLineColour(int _depth, string author, Color startcolor, Color endcolor)
    {
        if (depth == _depth && _depth > 0)
        {
            if (author == comment.Author)
            {
                GetComponent<LineRenderer>().SetColors(startcolor, endcolor);
            }
        }
        else if (_depth == 0)
        {
            if (author == comment.Author)
            {
                GetComponent<LineRenderer>().SetColors(startcolor, endcolor);
            }
        }
    }
    public void SetLineWidth(int _depth, string author, float startwidth, float endwidth)
    {
        if (depth == _depth && _depth > 0)
        {
            if (author == comment.Author)
            {
                GetComponent<LineRenderer>().SetWidth(startwidth, endwidth);
            }
        }
        else if (_depth == 0)
        {
            if (author == comment.Author)
            {
                GetComponent<LineRenderer>().SetWidth(startwidth, endwidth);
            }
        }
    }
    public void SetMaterial(int _depth, string author, Material material)
    {
        if (depth == _depth && _depth > 0)
        {
            if (author == comment.Author)
            {
                NodeMesh.GetComponent<Renderer>().material = material;
            }
        }
        else if (_depth == 0)
        {
            if (author == comment.Author)
            {
                NodeMesh.GetComponent<Renderer>().material = material;
            }
        }
    }
    public void CreateMesh(int _depth, GameObject sphere)
    {
        if (depth == _depth && _depth > 0 && NodeMesh == null)
        {
            Debug.Log("Spawn Mesh");
            NodeMesh = Instantiate(sphere);
            NodeMesh.transform.SetParent(this.transform);
            NodeMesh.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if(_depth == 0 && NodeMesh == null)
        {
            Debug.Log("Spawn Mesh");
            NodeMesh = Instantiate(sphere);
            NodeMesh.transform.SetParent(this.transform);
            NodeMesh.transform.localPosition = new Vector3(0,0,0);
        }

    }

    public void DestroyMesh(int _depth)
    {
        if (depth == _depth && _depth > 0 && NodeMesh != null)
        {
            Destroy(NodeMesh);
        }
        else if (_depth == 0 && NodeMesh != null)
        {
            Destroy(NodeMesh);
        }

    }
}