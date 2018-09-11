using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuickType;

public class NodeUI : MonoBehaviour {

    public delegate void InterfaceNode(string author);
    public static event InterfaceNode SelectAuthor;

    public delegate void NodeSend(string id);
    public static event NodeSend SendCommentID;

    public Comment comment;
    public Image background;
    public Text upvote;
    public Text downvote;
    public Text author;
    public Text replies;

    public NodeUI(Comment _comment)
    {
        comment = _comment;
    }

    public void SetText()
    {
        upvote.text = comment.Upvote.ToString();
        author.text = comment.Author;
        downvote.text = comment.Downvote.ToString();
        if (comment.Comments.CommentArray != null)
        {
            replies.text = comment.Comments.CommentArray.Length.ToString();
        }
        else
        {
            replies.text = "0";
        }

    }

    public void SelectNode()
    {
        //Zoom to Node

        //Select Author
        SelectAuthor(comment.Author);
    }

    public void SendCommentIDToNodes()
    {
        SendCommentID(comment.Id);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
