using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScreenShotScript : MonoBehaviour {
    
    private RectTransform UIPanelToCapture;

    [SerializeField]
    private string targetPath;

    private static uint ScreenShotCount = 0;

    void Awake()
    {
        UIPanelToCapture = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TakeScreenshot());
        }
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Vector2 temp = UIPanelToCapture.transform.position;
        var startX = temp.x - UIPanelToCapture.rect.width / 2;
        var startY = temp.y - UIPanelToCapture.rect.height / 2;

        int w = System.Convert.ToInt32(UIPanelToCapture.rect.width);
        int h = System.Convert.ToInt32(UIPanelToCapture.rect.height);

        Texture2D texture = new Texture2D(w, h, TextureFormat.RGB24, false);
        texture.ReadPixels(
            new Rect(
                startX, 
                startY, 
                UIPanelToCapture.rect.width, 
                UIPanelToCapture.rect.height), 
            0, 0);

        texture.Apply();        

        var bytes = texture.EncodeToPNG();
        Destroy(texture);

        string basePath = Application.dataPath + targetPath;
        string tryPath = null;

        while(File.Exists( (tryPath = basePath + "Screenshot" + ScreenShotCount.ToString() + " " + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png" ) ))
        {
            ScreenShotCount++;
        }

        yield return null;

        File.WriteAllBytes(tryPath, bytes);
    }
}
