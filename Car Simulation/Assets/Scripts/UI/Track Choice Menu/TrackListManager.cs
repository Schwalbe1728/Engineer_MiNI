using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackListManager : MonoBehaviour {

    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private float FadeInTime;

    private string NextSceneID;

    public void SetPreviewSprite(Sprite sprite)
    {
        if (!sprite.Equals(previewImage.sprite))
        {
            previewImage.color = new Color(1, 1, 1, 0f);
            previewImage.sprite = sprite;

            StopAllCoroutines();
            StartCoroutine(PreviewFadeIn());
        }
    }

    public void SetNextSceneID(string id)
    {
        NextSceneID = id;

        //odpalScenę

        SceneManager.LoadScene(NextSceneID);
    }
	
    private IEnumerator PreviewFadeIn()
    {        
        float progress = 0;
        Color start = new Color(1, 1, 1, 0f);
        Color end = Color.white;

        while(progress < FadeInTime)
        {
            progress += Time.deltaTime;
            previewImage.color = Color.Lerp(start, end, progress / FadeInTime);
            yield return new WaitForEndOfFrame();
        }
    }

}
