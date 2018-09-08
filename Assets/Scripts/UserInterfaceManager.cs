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

    //Controller
    public GameObject GraphManager;

    public GameObject ShaderManager;

    //View
    public GameObject Model;

    public GameObject LoadMenu;
    public GameObject GraphMenu;

    //Graph Data Display
    public Text depth;
    public Text force;
    public Text neighbour_force;
    public Text gravity_force;
    public Text max_distance_parent;
    public Text min_distance_parent;
    public Text distance_tolerance;
    public Text max_neighbour_distance;
    public Text min_neighbour_distance;
    public Text distance_tolerance_neighbour;
    public Text min_force_apply;

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

    //Highlight
    public Text highlight_depth;
    public Text highlight_author;
    public Text highlight_material_id;

    public Color color;
    public Image color_image;
    public int selected_material = 0;


    //Transformations
    public Text transform_depth;
    public Text transform_max_scale;

    //Prefabs
    public GameObject prefab_submission;
    public GameObject prefab_author;

    public GameObject author_content_scroll;
    public GameObject submission_content_scroll;

    public List<GameObject> author_list;
    public int selected_author = 0;
    public List<GameObject> submission_list;
    public int selected_submission = 0;

    public GameObject Sample_Node;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        Node.SendComment += SetComment;
        Graph.SendSubmission +=SetSubmission;
    }

    void OnDisable()
    {
        Node.SendComment -= SetComment;
        Graph.SendSubmission -= SetSubmission;
    }

    void GetMarkedVertices()
    {

    }

    public void SelectAuthor(string author)
    {
        for (int i = 0; i < author_list.Count;++i)
        {
            if (author == author_list[i].GetComponent<NodeUI>().comment.Author)
            {
                selected_author = i;
            }
        }
    }

    public void SendForce()
    {
        Force(int.Parse(depth.text), float.Parse(force.text));
    }
    public void SendNeighbourForceForce()
    {
        NeighbourForce(int.Parse(depth.text), float.Parse(neighbour_force.text));
    }
    public void SendGravityForce()
    {
        GravityForce(int.Parse(depth.text), float.Parse(gravity_force.text));
    }
    public void SendMaxDisParent()
    {
        MaxDisParent(int.Parse(depth.text), float.Parse(max_distance_parent.text));
    }
    public void SendMinDisParent()
    {
        MinDisParent(int.Parse(depth.text), float.Parse(min_distance_parent.text));
    }
    public void SendDisTolerance()
    {
        DisTolerance(int.Parse(depth.text), float.Parse(distance_tolerance.text));
    }
    public void SendMaxNeighDisParent()
    {
        MaxNeighDisParent(int.Parse(depth.text), float.Parse(max_neighbour_distance.text));
    }
    public void SendMinNeighDisParent()
    {
        MinNeighDisParent(int.Parse(depth.text), float.Parse(min_neighbour_distance.text));
    }
    public void SendNeighDisTolerance()
    {
        NeighDisTolerance(int.Parse(depth.text), float.Parse(distance_tolerance_neighbour.text));
    }
    public void SendMinForceEnable()
    {
        MinForceEnable(int.Parse(depth.text), float.Parse(min_force_apply.text));
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
            submission_list.Add(submission_ui);
        }
    }

    public void AddAuthorToInterface(Comment comment)
    {
        bool add_author = true;
        for (int i = 0; i<author_list.Count;++i)
        {
            if (comment.Author == author_list[i].GetComponent<NodeUI>().comment.Author)
            {
                add_author = false;
            }
        }
        if (add_author)
        {
            GameObject author_ui = Instantiate(prefab_author) as GameObject;
            author_ui.GetComponent<NodeUI>().comment = comment;
            author_ui.transform.parent = author_content_scroll.transform;
            author_ui.transform.localScale = Vector3.one;
            author_list.Add(author_ui);
        }
    }

    public void SetColorRed(float value)
    {
        color.r = value/255;
        Sample_Node.GetComponent<Renderer>().material.color = color;
    }
    public void SetColorGreen(float value)
    {
        color.g = value/255;
        Sample_Node.GetComponent<Renderer>().material.color = color;
    }
    public void SetColorBlue(float value)
    {
        color.b = value/255;
        Sample_Node.GetComponent<Renderer>().material.color = color;
    }
    public void SetColorAlpha(float value)
    {
        color.a = value/100;
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
            ShaderManager.GetComponent<ShaderManager>().SendMaterialToNode(int.Parse(highlight_depth.text), comment_author.text, int.Parse(highlight_material_id.text));
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
    }

    //Todo
    public void UpdateSettingsText()
    {
        
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


}
