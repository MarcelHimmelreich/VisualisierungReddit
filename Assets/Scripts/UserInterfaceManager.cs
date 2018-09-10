using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class UserInterfaceManager : MonoBehaviour {

    public delegate void NodeSend(int depth, float value);
    public static event NodeSend Force;
    public static event NodeSend NeighbourForce;
    public static event NodeSend GravityForce;
    public static event NodeSend MaxDisParent;
    public static event NodeSend MinDisParent;
    public static event NodeSend DisTolerance;
    public static event NodeSend MaxNeighDisParent;
    public static event NodeSend MinNeighDisParent;
    public static event NodeSend NeighDisTolerance;
    public static event NodeSend MinForceEnable;
    public static event NodeSend MaxScale;
    public static event NodeSend DisableScale;
    public static event NodeSend NodeSize;

    public delegate void NodeComment();
    public static event NodeComment AddComment;

    public delegate void NodeCreation(int depth, string author);
    public static event NodeCreation CreateNode;
    public static event NodeCreation HighlightBool;

    public delegate void EdgeDraw(int depth);
    public static event EdgeDraw EnableEdge;
    public static event EdgeDraw DisableEdge;

    public delegate void EdgeColor(int depth, Color color);
    public static event EdgeColor SetEdgeColor;

    public delegate void Orbit(int depth);
    public static event Orbit DeleteOrbit;

    public delegate void OrbitColor(int depth, Color color, float maxdistance);
    public static event OrbitColor CreateOrbit;

    //Controller
    public GameObject GraphManager;

    public GameObject ShaderManager;

    //View
    public GameObject Model;

    public GameObject LoadMenu;
    public GameObject GraphMenu;

    //Graph Data Display
    public Text text_depth;
    public Text text_force;
    public Text text_neighbour_force;
    public Text text_gravity_force;
    public Text text_max_distance_parent;
    public Text text_min_distance_parent;
    public Text text_distance_tolerance;
    public Text text_max_neighbour_distance;
    public Text text_min_neighbour_distance;
    public Text text_distance_tolerance_neighbour;
    public Text text_min_force_apply;
    public Text text_node_size;

    public int depth;
    public float force;
    public float neighbour_force;
    public float gravity_force;
    public float max_distance_parent;
    public float min_distance_parent;
    public float distance_tolerance;
    public float max_neighbour_distance;
    public float min_neighbour_distance;
    public float distance_tolerance_neighbour;
    public float min_force_apply;
    public float min_node_size;

    public List<Text> depth_count;

    //Submission Data
    public Text text_selected_submission;
    public Submission marked_submission;
    public Text subreddit;
    public Text submission_author;
    public Text submission_id;
    public Text submission_title;
    public Text submission_url;
    public Text submission_content;
    public Text submission_upvote;
    public Text submission_downvote;
    public Text submission_replies;

    //Comment Data
    public Comment marked_comment;
    public Text comment_author;
    public Text comment_id;
    public Text comment_url;
    public Text comment_content;
    public Text comment_score;
    public Text comment_likes;
    public Text comment_upvote;
    public Text comment_downvote;
    public Text comment_reply_count;
    public Text comment_orbit;

    //Highlight
    public Text highlight_depth;

    public Text highlight_material_id;

    //Color Selection
    public Color color;
    public float red_author = 255;
    public float green_author = 255;
    public float blue_author = 255;
    public Text text_red_author;
    public Text text_green_author;
    public Text text_blue_author;
    public Image color_image;
    public int selected_material = 0;

    //Shader Selection
    public Text text_selected_shader;
    public Color shader_color;
    public float red_shader = 255;
    public float green_shader = 255;
    public float blue_shader = 255;
    public Text text_red_shader;
    public Text text_green_shader;
    public Text text_blue_shader;
    public Image shader_color_image;
    public List<GameObject> shader_list;
    public int selected_shader = 0;
    public int shader_depth = 0;
    public GameObject Sample_Node;

    //Orbit
    public Text orbit_count;
    public Color orbit_color;
    public Image orbit_color_image;
    public Text orbit_depth_value;
    public float red_orbit = 255;
    public float green_orbit = 255;
    public float blue_orbit = 255;
    public float alpha_orbit = 100;
    public Text text_red_orbit;
    public Text text_green_orbit;
    public Text text_blue_orbit;
    public Text text_alpha_orbit;

    //Edge
    public Text edge_count;
    public Color edge_color;
    public Image edge_color_image;
    public Text edge_depth_value;
    public float red_edge = 255;
    public float green_edge = 255;
    public float blue_edge = 255;
    public float alpha_edge = 255;
    public Text text_red_edge;
    public Text text_green_edge;
    public Text text_blue_edge;
    public Text text_alpha_edge;

    //Size Transformations
    public Text text_transform_depth;
    public Text text_transform_max_scale;
    public Text text_selected_attribute;

    public int size_depth = 0;
    public float size_max_scale = 3;

    //Highlight Author
    public Text text_highlight_author_depth;
    public Text text_highlight_author;

    public int author_depth = 0;
    public int edge_depth = 0;
    public int orbit_depth = 0;

    public Color author_color;
    public Image author_color_image;


    //Prefabs
    public GameObject prefab_submission;
    public GameObject prefab_author;
    public GameObject prefab_node;

    public GameObject author_content_scroll;
    public GameObject submission_content_scroll;
    public GameObject node_content_scroll;

    public List<GameObject> author_list;
    public int selected_author = 0;

    public List<GameObject> submission_list;
    public int selected_submission = 0;

    public List<GameObject> node_list;
    public int selected_node = 0;

    //Skybox
    public Color background_color;
    public Material Background;
    public Image background_color_image;
    public float red_background = 255;
    public float green_background = 255;
    public float blue_background = 255;
    public Text text_red_background;
    public Text text_green_background;
    public Text text_blue_background;



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable()
    {
        Node.SendComment += SetComment;
        Node.SendOrbit += SetCommentOrbit;
        Node.AddComment += AddAuthorToInterface;
        Node.AuthorNode += AddAuthorNodesToInterface;
        Graph.SendSubmission += SetSubmission;
        SubmissionUI.SelectSubmission += SelectSubmission;
        AuthorUI.SelectAuthor += SelectAuthor;
        NodeUI.SelectAuthor += SelectAuthor;
    }

    void OnDisable()
    {
        Node.SendComment -= SetComment;
        Node.SendOrbit -= SetCommentOrbit;
        Node.AddComment -= AddAuthorToInterface;
        Node.AuthorNode -= AddAuthorNodesToInterface;
        Graph.SendSubmission -= SetSubmission;
        SubmissionUI.SelectSubmission -= SelectSubmission;
        AuthorUI.SelectAuthor -= SelectAuthor;
        NodeUI.SelectAuthor -= SelectAuthor;
    }

    void GetMarkedVertices()
    {

    }

    public void SelectAuthor(string author)
    {
        for (int i = 0; i < author_list.Count; ++i)
        {
            if (author == author_list[i].GetComponent<AuthorUI>().comment.Author)
            {
                selected_author = i;
                text_highlight_author.text = author;
            }
        }
    }

    public void SelectSubmission(string id)
    {
        for (int i = 0; i < submission_list.Count; ++i)
        {
            if (id.Equals(submission_list[i].GetComponent<SubmissionUI>().submission.Id))
            {
                selected_submission = i;
                text_selected_submission.text = submission_list[i].GetComponent<SubmissionUI>().submission.Title;
            }
        }
    }

    //Call Event
    public void SendForce()
    {
        Force(depth, force);
    }
    public void SendNeighbourForceForce()
    {
        NeighbourForce(depth, neighbour_force);
    }
    public void SendGravityForce()
    {
        GravityForce(depth, gravity_force);
    }
    public void SendMaxDisParent()
    {
        MaxDisParent(depth, max_distance_parent);
    }
    public void SendMinDisParent()
    {
        MinDisParent(depth, min_distance_parent);
    }
    public void SendDisTolerance()
    {
        DisTolerance(depth, distance_tolerance);
    }
    public void SendMaxNeighDisParent()
    {
        MaxNeighDisParent(depth, max_neighbour_distance);
    }
    public void SendMinNeighDisParent()
    {
        MinNeighDisParent(depth, min_neighbour_distance);
    }
    public void SendNeighDisTolerance()
    {
        NeighDisTolerance(depth, distance_tolerance_neighbour);
    }
    public void SendMinForceEnable()
    {
        MinForceEnable(depth, min_force_apply);
    }
    public void SendNodeSize()
    {
        NodeSize(0, min_node_size);
    }

    //Set Value
    public void SetForce(string value)
    {
        text_force.text = value;
        force = float.Parse(value);
        SendForce();
    }
    public void SetForceNeighbour(string value)
    {
        text_neighbour_force.text = value;
        neighbour_force = float.Parse(value);
    }
    public void SetForceOrigin(string value)
    {
        text_gravity_force.text = value;
        gravity_force = float.Parse(value);
        SendGravityForce();
    }
    public void SetMaxDisParent(string value)
    {
        text_max_distance_parent.text = value;
        max_distance_parent = float.Parse(value);
        SendMaxDisParent();
    }
    public void SetMaxNeighbourParent(string value)
    {
        text_max_neighbour_distance.text = value;
        max_neighbour_distance = float.Parse(value);
        SendMaxNeighDisParent();
    }
    public void SetMinForceEnable(string value)
    {
        text_min_force_apply.text = value;
        min_force_apply = float.Parse(value);
        SendMinForceEnable();
    }
    public void SetNodeSize(string value)
    {
        text_node_size.text = value.ToString();
        min_node_size = float.Parse(value);
        SendNodeSize();
    }

    public void SetSizeDepth(string value)
    {
        text_transform_depth.text = value;
        size_depth = int.Parse(value);
    }
    public void SetSizeMaxScale(string value)
    {
        text_transform_max_scale.text = value;
        size_max_scale = float.Parse(value);
    }
    public void SetSizeAttribute(string value)
    {
        text_transform_depth.text = value;
    }
    public void SetAuthorDepth(string value)
    {
        text_highlight_author_depth.text = value;
        author_depth = int.Parse(value);
    }

    //Size Transformation
    public void EnableSizeTransformation()
    {
        MaxScale(size_depth, size_max_scale);
        GraphManager.GetComponent<GraphManager>().SendTransformToNodes(size_depth, 0);
    }
    public void DisablesizeTransformation()
    {
        DisableScale(size_depth, 1);
    }

    //Hightlight Author
    public void EnableAuthorHighlight()
    {
        HighlightBool(author_depth, text_highlight_author.text);
        ShaderManager.GetComponent<ShaderManager>().SendColorToNode(author_depth, author_list[selected_author].GetComponent<AuthorUI>().comment.Author, author_color);
        CreateNode(author_depth, text_highlight_author.text);
    }
    public void DisableAuthorHighlight()
    {
        HighlightBool(author_depth, text_highlight_author.text);
        ShaderManager.GetComponent<ShaderManager>().SendColorToNode(author_depth, author_list[selected_author].GetComponent<AuthorUI>().comment.Author, shader_color);
        DeleteAuthorNodes();
    }
    public void DisableAuthorHighlightAll()
    {
        HighlightBool(author_depth, text_highlight_author.text);
        ShaderManager.GetComponent<ShaderManager>().SendColorToNode(0, author_list[selected_author].GetComponent<AuthorUI>().comment.Author, shader_color);
        DeleteAuthorNodesAll();
    }

    //Edge
    public void ColorEdge()
    {
        SetEdgeColor(edge_depth, edge_color);
    }
    public void SendEnableEdge()
    {
        EnableEdge(edge_depth);
        SetEdgeColor(edge_depth, edge_color);
    }
    public void SendDisableEdge()
    {
        DisableEdge(edge_depth);
    }

    //Orbit
    public void SendCreateOrbit()
    {
        CreateOrbit(orbit_depth, orbit_color, max_distance_parent);
    }
    public void SendCreateAllOrbits()
    {
        int maxdepth = GraphManager.GetComponent<GraphManager>().Submission[0].GetComponent<Graph>().max_depth;
        for (int i = 1; i<=maxdepth;++i)
        {
            CreateOrbit(i, orbit_color, max_distance_parent);
        }
    }
    public void SendDeleteOrbit()
    {
        DeleteOrbit(orbit_depth);
    }
    public void SendDeleteAllOrbits()
    {
        int maxdepth = GraphManager.GetComponent<GraphManager>().Submission[0].GetComponent<Graph>().max_depth;
        for (int i = 1; i <=maxdepth;++i)
        {
            DeleteOrbit(i);
        }

    }


    public void LoadSubmission()
    {
        foreach (Submission submission in Model.GetComponent<Model>().subreddit.Submission)
        {
            AddSubmissionToInterface(submission);
        }
    }

    //Fill List
    public void AddSubmissionToInterface(Submission submission)
    {
        bool add_submission = true;
        for (int i = 0; i < submission_list.Count; ++i)
        {
            if (submission.Title == submission_list[i].GetComponent<SubmissionUI>().submission.Title)
            {
                add_submission = false;
            }
        }
        if (add_submission)
        {
            GameObject submission_ui = Instantiate(prefab_submission) as GameObject;
            submission_ui.GetComponent<SubmissionUI>().submission = submission;
            submission_ui.transform.parent = submission_content_scroll.transform;
            submission_ui.transform.localScale = Vector3.one;
            submission_ui.GetComponent<RectTransform>().localPosition = new Vector3(submission_ui.GetComponent<RectTransform>().localPosition.x,
                submission_ui.GetComponent<RectTransform>().localPosition.y,
                0);
            submission_ui.GetComponent<SubmissionUI>().SetText();
            submission_list.Add(submission_ui);
        }
    }
    public void AddAuthorToInterface(Comment comment)
    {
        bool add_author = true;
        for (int i = 0; i < author_list.Count; ++i)
        {
            if (comment.Author == author_list[i].GetComponent<AuthorUI>().comment.Author)
            {
                add_author = false;
            }
        }
        if (add_author)
        {
            GameObject author_ui = Instantiate(prefab_author) as GameObject;

            author_ui.GetComponent<AuthorUI>().comment = comment;
            author_ui.transform.parent = author_content_scroll.transform;
            author_ui.transform.localScale = Vector3.one;
            author_ui.GetComponent<RectTransform>().localPosition = new Vector3(author_ui.GetComponent<RectTransform>().localPosition.x,
                author_ui.GetComponent<RectTransform>().localPosition.y,
                0);

            author_ui.GetComponent<AuthorUI>().SetText();
            author_list.Add(author_ui);
        }
    }
    public void AddAuthorNodesToInterface(Comment comment)
    {
        bool add_author_node = true;
        for (int i = 0; i < node_list.Count; ++i)
        {
            if (comment.Id == node_list[i].GetComponent<NodeUI>().comment.Id)
            {
                add_author_node = false;
            }
        }
        if (add_author_node)
        {
            GameObject node_ui = Instantiate(prefab_node) as GameObject;
            node_list.Add(node_ui);
            node_ui.GetComponent<NodeUI>().comment = comment;
            node_ui.transform.parent = node_content_scroll.transform;
            node_ui.transform.localScale = Vector3.one;
            node_ui.GetComponent<RectTransform>().localPosition = new Vector3(node_ui.GetComponent<RectTransform>().localPosition.x,
               node_ui.GetComponent<RectTransform>().localPosition.y,
               0);
            node_ui.GetComponent<RectTransform>().localRotation = new Quaternion(0, 0, 0, 0);
            node_ui.GetComponent<NodeUI>().SetText();

        }
        //SetNodeParent();
    }

    public void SetNodeParent()
    {
        foreach (GameObject node in node_list)
        {
            node.transform.SetParent(node_content_scroll.transform);
        }
    }

    public void DeleteAuthorNodes()
    {
        for (int i = 0;i<node_list.Count; ++i)
        {
            if (node_list[i].GetComponent<NodeUI>().author.text.Equals(text_highlight_author.text))
            {
                GameObject node = node_list[i];
                node_list.Remove(node);
                Destroy(node);
                --i;
            }
        }
    }
    public void DeleteAuthorNodesAll()
    {
        for (int i = 0; i < node_list.Count; ++i)
        {
            GameObject node = node_list[i];
            node_list.Remove(node);
            Destroy(node);
            --i;
        }
    }

    //Set Color
    public void SetColorAuthor()
    {
        author_color = new Color(red_author, green_author,blue_author);
        author_color_image.color = author_color;

    }
    public void SetColorAuthorRed(float value)
    {
        text_red_author.text = red_author.ToString();
        red_author = value / 255;

        SetColorAuthor();
    }
    public void SetColorAuthorGreen(float value)
    {
        text_green_author.text = green_author.ToString();
        green_author = value / 255;

        SetColorAuthor();
    }
    public void SetColorAuthorBlue(float value)
    {
        text_blue_author.text = blue_author.ToString();
        blue_author = value / 255;

        SetColorAuthor();
    }

    public void SetColorShader()
    {
        shader_color = new Color(red_shader, green_shader,blue_shader);
        shader_color_image.color = shader_color;
        Sample_Node.GetComponent<Renderer>().material.color = shader_color;
    }
    public void SetColorShaderRed(float value)
    {
        text_red_shader.text = red_shader.ToString();
        red_shader = value / 255;

        SetColorShader();
    }
    public void SetColorShaderGreen(float value)
    {
        text_green_shader.text = green_shader.ToString();
        green_shader = value / 255;

        SetColorShader();
    }
    public void SetColorShaderBlue(float value)
    {
        text_blue_shader.text = blue_shader.ToString();
        blue_shader = value / 255;

        SetColorShader();
    }

    public void SetColorOrbit()
    {
        orbit_color = new Color(red_orbit, green_orbit, blue_orbit, alpha_orbit);
        orbit_color_image.color = orbit_color;
    }
    public void SetColorOrbitRed(float value)
    {
        text_red_orbit.text = red_orbit.ToString();
        red_orbit = value / 255;

        SetColorOrbit();
    }
    public void SetColorOrbitGreen(float value)
    {
        text_green_orbit.text = green_orbit.ToString();
        green_orbit = value / 255;

        SetColorOrbit();
    }
    public void SetColorOrbitBlue(float value)
    {
        text_blue_orbit.text = blue_orbit.ToString();
        blue_orbit = value / 255;

        SetColorOrbit();
    }
    public void SetColorOrbitAlpha(float value)
    {
        text_alpha_orbit.text = alpha_orbit.ToString();
        alpha_orbit = value / 100;

        SetColorOrbit();
    }

    public void SetColorEdge()
    {
        edge_color = new Color(red_edge, green_edge, blue_edge,alpha_edge);
        edge_color_image.color = edge_color;
    }
    public void SetColorEdgeRed(float value)
    {
        text_red_edge.text = red_edge.ToString();
        red_edge = value / 255;

        SetColorEdge();
    }
    public void SetColorEdgeGreen(float value)
    {
        text_green_edge.text = green_edge.ToString();
        green_edge = value / 255;

        SetColorEdge();
    }
    public void SetColorEdgeBlue(float value)
    {
        text_blue_edge.text = blue_edge.ToString();
        blue_edge = value / 255;

        SetColorEdge();
    }
    public void SetColorEdgeAlpha(float value)
    {
        text_alpha_edge.text = alpha_edge.ToString();
        alpha_edge = value / 255;

        SetColorEdge();
    }

    public void SetColorBackground()
    {
        background_color = new Color(red_background, green_background, blue_background);
        background_color_image.color = background_color;
        Background.SetColor("_Tint", background_color);
    }
    public void SetColorBackgroundRed(float value)
    {
        text_red_background.text = red_background.ToString();
        red_background = value / 255;

        SetColorBackground();
    }
    public void SetColorBackgroundGreen(float value)
    {
        text_green_background.text = green_background.ToString();
        green_background = value / 255;

        SetColorBackground();
    }
    public void SetColorBackgroundBlue(float value)
    {
        text_blue_background.text = blue_background.ToString();
        blue_background = value / 255;

        SetColorBackground();
    }

    //Shader Text
    public void SetShaderText(string name)
    {
        text_selected_shader.text = name;
    }
    public void SelectMaterial(int id)
    {
        Sample_Node.GetComponent<Renderer>().material = ShaderManager.GetComponent<ShaderManager>().Material[id];
        selected_material = id;
    }
    public void SetShaderDepthText(string value)
    {
        shader_depth = int.Parse(value);
    }

    //Orbit
    public void SetOrbitDepth(string value)
    {
        orbit_depth_value.text = value;
        orbit_depth = int.Parse(value);
    }

    //Edge
    public void SetEdgeDepth(string value)
    {
        edge_depth_value.text = value;
        edge_depth = int.Parse(value);

    }

    public void SendShader()
    {
        ShaderManager.GetComponent<ShaderManager>().SendMaterialToNode(shader_depth, selected_material);
    }

    public void SendColorUnHighlightedNodes()
    {
        ShaderManager.GetComponent<ShaderManager>().SendColorToNode(shader_depth, shader_color);
    }

    public void HighlightNodesByAuthor()
    {
        if (marked_comment != null)
        {
            ShaderManager.GetComponent<ShaderManager>().SendMaterialToNode(author_depth, comment_author.text, int.Parse(highlight_material_id.text));
        }

    }

    public void AllUpdateMaterial()
    {
        if (marked_comment != null)
        {
            ShaderManager.GetComponent<ShaderManager>().SendMaterialToNode(int.Parse(highlight_depth.text), int.Parse(highlight_material_id.text));
        }
    }

    public void Loaded()
    {
        LoadMenu.SetActive(false);
        GraphMenu.SetActive(true);
        LoadSubmission();
    }

    //Load Text of the Interface after loading Json Data
    public void InitializeGraph()
    {
        
        AddComment();

    }

    public void UpdateForceText()
    {
        text_force.text = force.ToString();
        text_gravity_force.text = gravity_force.ToString();
        text_neighbour_force.text = neighbour_force.ToString();

        text_max_distance_parent.text = max_distance_parent.ToString();
        text_min_distance_parent.text = min_distance_parent.ToString();
        text_distance_tolerance.text = distance_tolerance.ToString();

        text_max_neighbour_distance.text = max_neighbour_distance.ToString();
        text_min_neighbour_distance.text = min_neighbour_distance.ToString();
        text_distance_tolerance_neighbour.text = distance_tolerance_neighbour.ToString();

        text_min_force_apply.text = min_force_apply.ToString();
        text_depth.text = depth.ToString();
    }

    public void SetSubmission(Submission submission)
    {
        marked_submission = submission;
        subreddit.text = submission.Subreddit;
        submission_author.text = submission.Author;
        submission_id.text = submission.Id;
        submission_title.text = submission.Title;
        submission_url.text = submission.Url;
        submission_content.text = submission.Content;
        submission_upvote.text = submission.Upvote.ToString();
        submission_downvote.text = submission.Downvote.ToString();
        submission_replies.text = CountDepth(submission).ToString();
    }

    public void SetComment(Comment comment)
    {
        marked_comment = comment;
        comment_author.text = comment.Author;
        comment_id.text = comment.Id;
        comment_url.text = comment.Url;
        comment_content.text = comment.Content;
        comment_score.text = comment.Score.ToString();
        comment_likes.text = comment.Likes.ToString();
        comment_upvote.text = comment.Upvote.ToString();
        comment_downvote.text = comment.Downvote.ToString();
        if (comment.Comments.CommentArray.Length != null)
        {
            comment_reply_count.text = (GetDepth(comment)-1).ToString();
        }
        else
        {
            comment_reply_count.text = "0";
        }
        
        SelectAuthor(comment.Author);

    }

    public void SetCommentOrbit(int orbit)
    {
        comment_orbit.text = orbit.ToString();
    }

    public void CreateGraph()
    {
        GraphManager.GetComponent<GraphManager>().CreateGraph(selected_submission);
        SetSubmission(GraphManager.GetComponent<GraphManager>().Submission[0].GetComponent<Graph>().submission);
        orbit_count.text = GraphManager.GetComponent<GraphManager>().Submission[0].GetComponent<Graph>().max_depth.ToString();
        edge_count.text = GraphManager.GetComponent<GraphManager>().Submission[0].GetComponent<Graph>().max_depth.ToString();
    }

    public void DeleteGraph()
    {
        GraphManager.GetComponent<GraphManager>().DeleteGraph(0);
    }

    public int CountDepth(Submission submission)
    {
        int count = 0;
        if (submission.Comments != null)
        {
            foreach (Comment comment in submission.Comments)
            {
                count += GetDepth(comment);
            }
        }
        return count;
    }

    public int GetDepth(Comment _comment)
    {
        int count = 1;
        if (_comment.Comments.CommentArray != null)
        {
            foreach (Comment comment in _comment.Comments.CommentArray)
            {
                count += GetDepth(comment);
            }
        }
        return count;

    }


}
