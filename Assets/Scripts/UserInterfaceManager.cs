using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class UserInterfaceManager : MonoBehaviour {

    //Controller
    public GameObject GraphManager;

    //View
    public GameObject Model;

    public GameObject LoadMenu;
    public GameObject GraphMenu;

    //Graph Data Display
    public Text force;
    public Text neighbour_force;
    public Text gravity_force;
    public Text max_distance_parent;
    public Text min_distance_parent;
    public Text distance_tolerance;
    public Text max_neighbour_distance;
    public Text min_neighbour_distance;
    public Text distance_tolerance_neighbour;

    //Submission Data
    public Text subreddit;
    public Text submission_author;
    public Text submission_id;
    public Text submission_title;
    public Text submission_url;
    public Text submission_content;
    public Text submission_upvote;
    public Text submission_downvote;

    //Comment Data
    public Text comment_author;
    public Text comment_id;
    public Text comment_url;
    public Text comment_content;
    public Text comment_score;
    public Text comment_likes;
    public Text comment_upvote;
    public Text comment_downvote;
    public Text comment_reply_count;




    //Transformations
    public Text transform_depth;
    public Text transform_max_scale;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetMarkedVertices() {

    }

    public void Loaded()
    {
        LoadMenu.SetActive(false);
        GraphMenu.SetActive(true);
    }

    public void SetSubmission(Submission submission)
    {
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
