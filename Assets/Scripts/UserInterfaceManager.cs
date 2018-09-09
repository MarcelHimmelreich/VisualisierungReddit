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

    public delegate void NodeComment();
    public static event NodeComment AddComment;

    public delegate void NodeCreation(int depth, string author);
    public static event NodeCreation CreateNode;

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

    public int depth { get; set; }
    public float force { get; set; }
    public float neighbour_force { get; set; }
    public float gravity_force { get; set; }
    public float max_distance_parent { get; set; }
    public float min_distance_parent { get; set; }
    public float distance_tolerance { get; set; }
    public float max_neighbour_distance { get; set; }
    public float min_neighbour_distance { get; set; }
    public float distance_tolerance_neighbour { get; set; }
    public float min_force_apply { get; set; }

    public List<Text> depth_count;

    //Submission Data
    public Submission marked_submission;
    public Text subreddit;
    public Text submission_author;
    public Text submission_id;
    public Text submission_title;
    public Text submission_url;
    public Text submission_content;
    public Text submission_upvote;
    public Text submission_downvote;

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
    public Image color_image;
    public int selected_material = 0;

    //Shader Selection
    public Color shader_color;
    public Image shader_color_image;
    public List<GameObject> shader_list;
    public int selected_shader = 0;
    public int shader_depth = 0;
    public GameObject Sample_Node;

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




    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable()
    {
        Node.SendComment += SetComment;
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
            }
        }
    }

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

    public void SetForce(string value)
    {
        text_force.text = value;
        force = float.Parse(value);
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
    }
    public void SetMaxDisParent(string value)
    {
        text_max_distance_parent.text = value;
        max_distance_parent = float.Parse(value);
    }
    public void SetMaxNeighbourParent(string value)
    {
        text_max_neighbour_distance.text = value;
        max_neighbour_distance = float.Parse(value);
    }
    public void SetMinForceEnable(string value)
    {
        text_min_force_apply.text = value;
        min_force_apply = float.Parse(value);
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
        ShaderManager.GetComponent<ShaderManager>().SendColorToNode(author_depth, author_list[selected_author].GetComponent<AuthorUI>().comment.Author, author_color);
        CreateNode(author_depth, text_highlight_author.text);
    }
    public void DisableAuthorHighlight()
    {
        DeleteAuthorNodes();
        ShaderManager.GetComponent<ShaderManager>().SendColorToNode(author_depth, author_list[selected_author].GetComponent<AuthorUI>().comment.Author, shader_color);
        
    }
    public void DisableAuthorHighlightAll()
    {
        DeleteAuthorNodesAll();
        ShaderManager.GetComponent<ShaderManager>().SendColorToNode(0, author_list[selected_author].GetComponent<AuthorUI>().comment.Author, shader_color);
        
    }


    public void LoadSubmission()
    {
        foreach (Submission submission in Model.GetComponent<Model>().subreddit.Submission)
        {
            AddSubmissionToInterface(submission);
        }
    }

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
            if (comment.Id.Equals(node_list[i].GetComponent<NodeUI>().comment.Id))
            {
                add_author_node = false;
            }
        }
        if (add_author_node)
        {
            GameObject node_ui = Instantiate(prefab_node) as GameObject;
            node_ui.GetComponent<NodeUI>().comment = comment;
            node_ui.transform.parent = node_content_scroll.transform;
            node_ui.transform.localScale = Vector3.one;
            node_ui.GetComponent<RectTransform>().localPosition = new Vector3(node_ui.GetComponent<RectTransform>().localPosition.x,
               node_ui.GetComponent<RectTransform>().localPosition.y,
               0);
            node_ui.GetComponent<NodeUI>().SetText();
            node_list.Add(node_ui);
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
    public void SetColorAuthorRed(float value)
    {
        author_color.r = value / 255;
        author_color_image.color = color;
    }
    public void SetColorAuthorGreen(float value)
    {
        author_color.g = value / 255;
        author_color_image.color = color;
    }
    public void SetColorAuthorBlue(float value)
    {
        author_color.b = value / 255;
        author_color_image.color = color;
    }
    public void SetColorAuthorAlpha(float value)
    {
        author_color.a = value / 100;
        author_color_image.color = color;
    }

    public void SetColorShaderRed(float value)
    {
        shader_color.r = value / 255;
        shader_color_image.color = color;
        Sample_Node.GetComponent<Renderer>().material.color = color;
    }
    public void SetColorShaderGreen(float value)
    {
        shader_color.g = value / 255;
        shader_color_image.color = color;
        Sample_Node.GetComponent<Renderer>().material.color = color;
    }
    public void SetColorShaderBlue(float value)
    {
        shader_color.b = value / 255;
        shader_color_image.color = color;
        Sample_Node.GetComponent<Renderer>().material.color = color;
    }
    public void SetColorShaderAlpha(float value)
    {
        shader_color.a = value / 100;
        shader_color_image.color = color;
        Sample_Node.GetComponent<Renderer>().material.color = color;
    }

    public void SelectMaterial(int id)
    {
        Sample_Node.GetComponent<Renderer>().material = ShaderManager.GetComponent<ShaderManager>().Material[id];
        selected_material = id;
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
        comment_reply_count.text = comment.Comments.CommentArray.Length.ToString();
    }

    public void CreateGraph()
    {
        GraphManager.GetComponent<GraphManager>().CreateGraph(selected_submission);
    }

    public void DeleteGraph()
    {
        GraphManager.GetComponent<GraphManager>().DeleteGraph(0);
    }


}
