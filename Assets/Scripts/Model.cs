using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using QuickType;


public class Model : MonoBehaviour {

    //Controller
    public GameObject GraphManager;
    //View
    public GameObject UserInterface;

    public Subreddit subreddit { get; set; }

    public List<Submission> submissions { get; set; }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Load Json Data and stores it
    //Create a Dictionary from Json
    public void LoadJson(string filepath) {
        Debug.Log("Loading Json Data...");
        var json = File.ReadAllText(filepath);

        subreddit = Subreddit.FromJson(json);
        Debug.Log("Success!");
    }

    public void PrintAll()
    {
        Debug.Log(System.Environment.Version);
        /*foreach (Submission submission in Submissions.submission)
        {
            Debug.Log(submission);
            Debug.Log("Submission Name: " + submission.title);
            Debug.Log("Submission Author: " + submission.author);
            Debug.Log("Submission ID: " + submission.id);
            Debug.Log("Submission Comments: " + submission.comments.comment.Count);
            foreach (Comment comment in submission.comments.comment) {
                Debug.Log("Comment Author: " + comment.author);

                int value = 0;
                if (comment.replies!= null)
                {
                    value = comment.replies.comment.Count;
                }
                else {  value  =0; }
                Debug.Log("Comment Replies: " + value);
            }

        }*/
    }

    public void CreateDataObjects() {

    }
    
}
