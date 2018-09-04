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
    public List<Vector3> velocity_neighbor;
    public List<float> neighbourdistance;
    public float force = 1;
    public float force_neighbor = 1;
    public float force_sphere = 1;
    public float maxDistance = 10;
    public float minDistance = 10;
    public float rangeDistance = 1;
    public float maxneighbourdistance = 5;
    public float neighbourrangeDistance = 1;
    public bool apply = true;
    public bool checkforce = true;
    public bool drawline = true;

    public Vector3 neighbourdirection;
    public int closestneighbour = 0;
    public List<GameObject> NearNeighbour;

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
            if (checkforce)
            {
                ApplyForce();
            }
            if (CheckNeighbourDistance() && !checkforce)
            {
                ApplyForceNeighbour();

            }

            if (CheckParentDistance()) {
                ApplySphereForce();
            }

        }
        if (drawline) {
            SetLine();

        }


    }

    void OnEnable()
    {
        Graph.count += sendVerticesCount;
        GraphManager.CreateParentComments += GetParentComments;
        ShaderManager.SendMaterial += SetMaterial;
    }

    void OnDisable()
    {
        Graph.count -= sendVerticesCount;
        GraphManager.CreateParentComments -= GetParentComments;
        ShaderManager.SendMaterial -= SetMaterial;
    }

    void OnTriggerEnter(Collider collider) {
        NearNeighbour.Add(collider.gameObject);

    }

    void OnTriggerExit(Collider collider) {
        NearNeighbour.Remove(collider.gameObject);


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

            maxDistance = ParentComments.Count/3;
        }
        else
        {
            maxDistance = distance;
        }

    }

    public bool CheckNeighbourDistance(){
        float distance = Vector3.Distance(NearNeighbour[closestneighbour].transform.position, transform.position);
        if (distance < (minDistance - rangeDistance))
        {
            return true;
        }

        return false ;
    }

    public bool CheckParentDistance() {
        if (Vector3.Distance(Parent.transform.position, transform.position) < Vector3.Distance(Origin.transform.position, Parent.transform.position)) {
            return true;
        }
        return false;

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
        List<Vector3> n_velocity = new List<Vector3>();
        if (depth == 1)
        {
            foreach (GameObject transform_neighbor in Parent.GetComponent<Graph>().Comments)
            {
                if (transform_neighbor.GetComponent<Vertex>().comment.Id != comment.Id)
                {
                    Vector3 direction = transform_neighbor.transform.position + transform.position;
                    Debug.Log("Direction: " + direction);
                    n_velocity.Add(direction);
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
                    n_velocity.Add(direction);
                    Debug.Log("Add Neighbour direction");
                }
            }
        }

    }



    public void ApplyForce() {
        rigidbody.AddRelativeForce(velocity * force, ForceMode.Force);
    }

    public void ApplySphereForce() {
        rigidbody.AddForce(Origin.transform.position + Parent.transform.position * force_sphere, ForceMode.Force);
    }

    //Apply Force from Every Neighbour
    public void ApplyForceNeighbour() {
        if (ParentComments.Count > 0) {
            neighbourdistance = GetClosestNeighbour();
            if (neighbourdistance.Count > 0)
            {
                float maxdistance = 0;
                closestneighbour = 0;
                for (int i = 0; i < neighbourdistance.Count; ++i)
                {
                    if (maxdistance < neighbourdistance[i]) {
                        maxdistance = neighbourdistance[i];
                    }
                    if (neighbourdistance[i] < neighbourdistance[closestneighbour] && neighbourdistance[i] > 0)
                    {
                        closestneighbour = i;
                    }
                }
                //for (int i = 0; i < ParentComments.Count; ++i)
                //{
                    float forcedrop;
                    //Check if Last Element is 0
                    if (maxdistance > 0)
                    {
                        forcedrop = neighbourdistance[closestneighbour] / maxdistance;
                    }
                    else
                    {
                        forcedrop = 1;
                    }

                    Vector3 direction = (ParentComments[closestneighbour].transform.position - transform.position).normalized;
                    Debug.Log(direction);
                    if (!float.IsNaN(direction.x) && !float.IsNaN(direction.y) && !float.IsNaN(direction.z))
                    {
                        neighbourdirection = direction;
                        rigidbody.AddForce(-direction * force_neighbor , ForceMode.Force);
                    Debug.Log("Apply  Neighbour Force");
                    }
                //}
            }

        }

        
    }

    public List<float> GetClosestNeighbour() {
        List<float> list = new List<float>();
        for(int i = 0;i<NearNeighbour.Count;++i){
            if (NearNeighbour[i].GetComponent<Vertex>().comment.Id != comment.Id)
            {
                list.Add(Vector3.Distance(NearNeighbour[i].transform.position, transform.position));
            }
            else
            {
                list.Add(-1);
            }
            
        }
        return list;
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
                new_comment_object.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * maxDistance;
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

    public void SetLineColour(string author, Color startcolor,  Color endcolor)
    {
        if (author == comment.Author)
        {
            GetComponent<LineRenderer>().SetColors(startcolor, endcolor);
        }
    }

    public void SetLineWidth(string author, float startwidth, float endwidth)
    {
        if (author == comment.Author)
        {
            GetComponent<LineRenderer>().SetWidth(startwidth, endwidth);
        }
    }

    public void SetMaterial(string author, Material material)
    {
        if (author == comment.Author)
        {
            GetComponent<LineRenderer>().material = material;
        }
    }

    public void Move(Vector3 position) {
    }

    public void CastEdge() {
    }
}
