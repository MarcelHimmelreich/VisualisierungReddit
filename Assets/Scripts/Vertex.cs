using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickType;

public class Vertex : MonoBehaviour {

    public delegate void vertexsender(int depth, int value);
    public static event vertexsender SendVerticesCount;

    //Reddit Comment Data Structure
    public Comment comment;
    public List<GameObject> Comments;
    public List<GameObject> ParentComments;

    //Depth for Graph in Commentforest
    public int depth = 0;
    public int comment_count = 0;

    //Component Properties
    public Material mat;
    public Transform vertextransform;
    public Vector3 Position;

    //Force Graph
    public Rigidbody rigidbody;
    public GameObject Origin;
    public GameObject Parent;
    public Vector3 velocity;
    public Vector3 velocity_neighbor;
    public float force = 1;
    public float force_neighbor = 1;
    public float maxDistance = 5;
    public float rangeDistance = 1;
    public bool apply = true;
    public bool checkforce = true;

    public GameObject Vertex_prefab;

    // Use this for initialization
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        mat = this.GetComponent<Material>();

    }

    // Update is called once per frame
    void Update() {
        if (apply)
        {
            CalculateVelocity();
            //CalculateNeighborVelocity();
            if (checkforce) {
                ApplyForce();
                //ApplyForceNeighbour();

            }

        }


    }

    void OnEnable()
    {
        Graph.count += sendVerticesCount;
        GraphManager.CreateParentComments += GetParentComments;
    }

    void OnDisable()
    {
        Graph.count -= sendVerticesCount;
        GraphManager.CreateParentComments -= GetParentComments;
    }

    public void CalculateVelocity() {
        float distance = Vector3.Distance(Parent.transform.position, transform.position);
        if (distance < maxDistance - rangeDistance)
        {
            velocity = Parent.transform.position + transform.position;
            checkforce = true;
        }
        else if(distance > maxDistance + rangeDistance)
        {
            velocity = Parent.transform.position - transform.position;
            checkforce = true;
        }
        //Is in range
        else { checkforce = false; }
        velocity = velocity.normalized;
    }

    public void CalculateNeighborVelocity() {
        Vector3 n_velocity = new Vector3();
        if (depth == 1)
        {
            foreach (GameObject transform_neighbor in Parent.GetComponent<Graph>().Comments)
            {
                if (transform_neighbor.GetComponent<Vertex>().comment.Id != comment.Id)
                {
                    Vector3 direction = transform_neighbor.transform.position + transform.position;
                    Debug.Log("Direction: " + direction);
                    n_velocity += direction;
                    Debug.Log("Add Neighbour direction Graph");
                }
                
                
            }
        }
        else
        {
            foreach (GameObject transform_neighbor in ParentComments)
            {
                if (transform_neighbor.GetComponent<Vertex>().comment.Id != comment.Id)
                {
                    Vector3 direction = transform_neighbor.transform.position + transform.position;
                    Debug.Log("Direction: " + direction);
                    n_velocity += direction;
                    Debug.Log("Add Neighbour direction");
                }
            }
        }
        velocity_neighbor = n_velocity.normalized;

    }



    public void ApplyForce() {
        rigidbody.AddForce(velocity * force, ForceMode.Force);
    }

    public void ApplyForceNeighbour() {
        rigidbody.AddForce(velocity_neighbor * force_neighbor, ForceMode.Force);
    }

    public void sendVerticesCount() {
            int vertices_count = getVerticesCount();
            SendVerticesCount(depth, vertices_count);
    }

    public void SetDepth(int new_depth) {
        depth = new_depth;
        foreach (GameObject comment in Comments){
            comment.GetComponent<Vertex>().SetDepth(depth+1);
        }

    }

    //rekursiv function
    public int getVerticesCount(int value = 0) {
        int count = value;
        if (Comments.Count == 0) {
            return 1;
        }
        else if (Comments.Count > 0) {
            foreach (GameObject vertex in Comments) {
                count += vertex.GetComponent<Vertex>().getVerticesCount();
            }
        }
        comment_count = count;
        return ++count;
    }

    public List<GameObject> CreateComment(Comment comment, GameObject parent, GameObject origin)
    {
        List<GameObject> comments = new List<GameObject>();
        if (comment.Comments.CommentArray != null)
        {
            foreach (Comment new_comment in comment.Comments.CommentArray)
            {
                GameObject new_comment_object = Instantiate(Vertex_prefab) as GameObject;
                comments.Add(new_comment_object);
                new_comment_object.GetComponent<Vertex>().comment = new_comment;
                new_comment_object.GetComponent<Vertex>().Origin = origin;
                new_comment_object.GetComponent<Vertex>().Parent = parent;
                new_comment_object.GetComponent<Vertex>().Comments = CreateComment(new_comment_object.GetComponent<Vertex>().comment, 
                    new_comment_object, 
                    new_comment_object.GetComponent<Vertex>().Origin);             
                
            }
        }
        else
        {

        }
        return comments;

    }
    public void MakeParent() {
        Debug.Log("Make Parents");
        if (Comments != null || Comments.Count == 0)
        {
            foreach (GameObject comment in Comments)
            {
                comment.GetComponent<Vertex>().SetParent(this.transform);
                comment.GetComponent<Vertex>().MakeParent();
            }
        }

            
    }

    public void SetParent(Transform transform) {
        this.transform.SetParent(transform);
    }

    public void GetParentComments() {
        if (depth > 1)
        {
            ParentComments = Parent.GetComponent<Vertex>().Comments;
        }
        else
        {
            ParentComments = Parent.GetComponent<Graph>().Comments;
        }
        

    }




    public void Move(Vector3 position) {
    }

    public void CastEdge() {
    }
}
