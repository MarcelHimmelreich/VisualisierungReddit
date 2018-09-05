using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickType;

public class Vertex : MonoBehaviour {

    public delegate void vertexsender(int depth, int value);
    public static event vertexsender SendVerticesCount;
    public static event vertexsender SendVerticesDepthCount;

    public delegate void vertexdepthsender(int depth);
    public static event vertexdepthsender SendDepth;

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
    public Vector3 velocity_origin;
    public float velocity_magnitude;
    public List<Vector3> velocity_neighbor;
    public List<float> neighbourdistance;
    public float force = 1;
    public float force_neighbor = 1;
    public float force_sphere = 1;
    public float maxvelocity = 5;
    public float maxDistance = 10;
    public float minDistance = 10;
    public float rangeDistance = 1;
    public float maxneighbourdistance = 5;
    public float minneighbourdistance = 5;
    public float neighbourrangeDistance = 1;
    public float minvelocity = 0.1f;

    public float parentdistance = 0;
    public float origindistance = 0;

    public bool applyphysics = true;
    public bool applyforce = false;
    public bool applyforceneighbour = false;
    public bool applyforceorigin = true;

    public bool checkforce = true;
    public bool drawline = true;

    public Vector3 neighbourdirection;
    public int closestneighbour = 0;
    public float closestneighbourdistance = 0;
    public List<GameObject> NearNeighbour;

    public GameObject Vertex_prefab;

    public int counter = 0;

    public bool marked = false;

    // Use this for initialization
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        mat = this.GetComponent<Material>();

    }

    // Update is called once per frame
    void Update() {
        velocity_magnitude = rigidbody.velocity.magnitude;
        if (applyphysics)
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
            /*if (applyforce)
            {
                ApplyForce();
                CheckVelocity();
                //Debug.Log("Apply Force");
            }
            if (applyforceneighbour)
            {
                ApplyForceNeighbour();
                CheckParentDistance();
                //Debug.Log("Apply Neighbour Force");
            }
            if (applyforceorigin)
            {
                ApplySphereForce();
                CheckNeighbourDistance();
                //Debug.Log("Apply Sphere Force");

            }*/

            CheckVelocityDirection();
        }

        if (drawline) {
            SetLine();

        }
    }

    void OnEnable()
    {
        Graph.count += sendVerticesCount;
        Graph.ApplyForce += SetApplyByDepth;
        GraphManager.CreateParentComments += GetParentComments;
        ShaderManager.SendMaterial += SetMaterial;

    }

    void OnDisable()
    {
        Graph.count -= sendVerticesCount;
        Graph.ApplyForce -= SetApplyByDepth;
        GraphManager.CreateParentComments -= GetParentComments;
        ShaderManager.SendMaterial -= SetMaterial;
    }

    public void SetApplyByDepth(int _depth)
    {
        if (depth == _depth)
        {
            if (applyphysics)
            {
                applyphysics = false;
            }
            else
            {
                applyphysics = true;
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

            maxDistance = ParentComments.Count/3;
        }
        else
        {
            maxDistance = distance;
        }

    }

    public void CheckVelocityDirection()
    {
        if (velocity == new Vector3(-1, -1, -1))
        {
            velocity_magnitude = 0;
            applyphysics = false;
        }
    }

    public void CheckPositionComplete()
    {
        if (!applyforce && !applyforceneighbour && !applyforceorigin)
        {
            //Origin.GetComponent<Graph>().AddDepthDone(depth);
            applyphysics = false;
        }
    }

    public void CheckVelocity() {
        float distance = Vector3.Distance(Parent.transform.position, transform.position);

        //Check if Distance is in MaxDistance or velocity is decreasing
        if (velocity_magnitude < minvelocity &&  distance < maxDistance - rangeDistance || velocity_magnitude < minvelocity && distance > maxDistance + rangeDistance)
        {
            applyforce = false;
            //applyforceneighbour = true;
            velocity = new Vector3(-1, -1, -1);
            
        }
    }

    public void CheckNeighbourDistance(){
        float distance = Vector3.Distance(ParentComments[closestneighbour].transform.position, transform.position);

        if (distance > minneighbourdistance && velocity.magnitude < minvelocity)
        {
            applyforceneighbour = true;
        }
        else
        {
            applyforceneighbour = false;
        }

    }

    public void CheckParentDistance() {
        float distanceparent = Vector3.Distance(Parent.transform.position, transform.position);
        float distanceorigin = Vector3.Distance(Origin.transform.position, Parent.transform.position);

        if (distanceparent > distanceorigin && distanceparent < maxDistance*2)
        {
            //applyforceorigin = true;
        }
        else
        {
            //ApplySphereForce(true);
            applyforceorigin = false;
        }

    }

    public void CalculateVelocity() {
        parentdistance = Vector3.Distance(Parent.transform.position, transform.position);
        
        if (parentdistance < maxDistance - rangeDistance)
        {
            velocity = Parent.transform.position - transform.position;
            velocity = -velocity;
            checkforce = true;
        }
        else if(parentdistance > maxDistance + rangeDistance)
        {
            velocity = Parent.transform.position - transform.position;
            
            checkforce = true;
        }
        //Is in range
        else {
            //velocity = new Vector3(0,0,0);
            rigidbody.AddRelativeForce(-velocity * force, ForceMode.Force);
            checkforce = false; }
        velocity = velocity.normalized;
    }

    public void CalculateSphereVelocity()
    {
        origindistance = Vector3.Distance(Origin.transform.position, transform.position);
        float parentorigindistance = Vector3.Distance(Origin.transform.position, Parent.transform.position);
        if (origindistance < maxDistance * depth)
        {
            velocity_origin = Origin.transform.position - transform.position;
            velocity_origin = -velocity_origin;

        }
        else if (origindistance > maxDistance * depth + maxDistance)
        {
            velocity_origin = Origin.transform.position - transform.position;
            

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
        if (closestneighbourdistance > 0)
        {
            if (closestneighbourdistance < maxneighbourdistance - rangeDistance)
            {
                neighbourdirection = -(ParentComments[closestneighbour].transform.position - transform.position).normalized;

            }
            else if (closestneighbourdistance > maxneighbourdistance + rangeDistance)
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
        if (ParentComments.Count > 1)
        {
            if (!float.IsNaN(neighbourdirection.x) && !float.IsNaN(neighbourdirection.y) && !float.IsNaN(neighbourdirection.z))
            {
                rigidbody.AddRelativeForce(neighbourdirection * force_neighbor, ForceMode.Force);
            }

        }
    }

    public void GetClosestNeighbour() {
        for(int i = 0;i<ParentComments.Count;++i){
            if (ParentComments[i].GetComponent<Vertex>().comment.Id != comment.Id)
            {
                float distance = Vector3.Distance(ParentComments[i].transform.position, transform.position);
                if (distance < closestneighbourdistance && distance > 0 ||closestneighbourdistance == 0)
                {
                    closestneighbourdistance = distance;
                    closestneighbour = i;
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

    public void getVerticesDepthCount()
    {
            sendVerticesDepthCount();
            foreach (GameObject vertex in Comments)
            {
                vertex.GetComponent<Vertex>().getVerticesDepthCount();
            }
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
