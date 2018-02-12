using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackListEntry : MonoBehaviour {

    private Text entryNameText;

    [SerializeField]
    private string sceneId;

    [SerializeField]
    private string entryName;

    [SerializeField]
    private Sprite imagePreview;

    [SerializeField]
    private TrackListManager trackManager;

    void Awake()
    {
        OnValidate();        
    }

    void OnValidate()
    {
        if(entryNameText == null)
        {
            entryNameText = gameObject.GetComponent<Text>();

            if(entryNameText == null)
            {
                entryNameText = gameObject.GetComponentInChildren<Text>();
            }
        }

        entryNameText.text = entryName;
    }

    public void PointerHighlight()
    {               
        trackManager.SetPreviewSprite(imagePreview);
    }

    public void ChooseEntry()
    {
        trackManager.SetNextSceneID(sceneId);
    }
}
