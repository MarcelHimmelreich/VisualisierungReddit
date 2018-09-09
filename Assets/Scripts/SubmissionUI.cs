using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class SubmissionUI : MonoBehaviour {

    public delegate void InterfaceSubmission(string id);
    public static event InterfaceSubmission SelectSubmission;

    public Submission submission;
    public Image background;
    public Text title;
    public Text author;
    public Text replies;

    public SubmissionUI(Submission _submission)
    {
        submission = _submission;     
    }

    public void SetText()
    {
        title.text = submission.Title;
        author.text = submission.Author;
    }

    public void SelectSub()
    {
        SelectSubmission(submission.Id);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
