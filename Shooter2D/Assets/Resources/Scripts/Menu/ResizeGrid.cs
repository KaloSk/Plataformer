using UnityEngine;

public class ResizeGrid : MonoBehaviour {

    public UnityEngine.UI.GridLayoutGroup gridLayoutView;

    bool reActive;
    int frameCount = 0;

	// Update is called once per frame
	void Update () {
        if (frameCount <= 0){
            frameCount++;
        } else {
            if (!reActive)
            {
                RectTransform rectTransform = (RectTransform)gameObject.transform;
                gridLayoutView.cellSize = new Vector2(rectTransform.rect.width - (rectTransform.rect.width*0.05f), rectTransform.rect.width / 3.0f);
                reActive = true;
            }
        }
	}
}
