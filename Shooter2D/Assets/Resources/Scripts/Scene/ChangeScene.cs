using UnityEngine.SceneManagement;

public class ChangeScene {

    public static int SceneCount;

    public void CustomChangeScene(int scene)
    {
        try
        {
            SceneCount = scene;
            SceneManager.LoadScene(SceneCount);
        }
        catch
        {
            SceneManager.LoadScene(0);
        }
    }

}
