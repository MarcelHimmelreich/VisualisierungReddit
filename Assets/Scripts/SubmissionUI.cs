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
        replies.text = CountDepth().ToString();
    }

    public int CountDepth()
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
