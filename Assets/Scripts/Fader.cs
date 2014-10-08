using UnityEngine;
using System.Collections;
 
public class Fader : MonoBehaviour
{
    private static Fader mInstance = null;

    public static Fader Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType(typeof(Fader)) as Fader;

                if (mInstance == null)
                {
                    mInstance = new GameObject("FadeManager").AddComponent<Fader>();
                }
            }

            return mInstance;
        }
    }

    public Texture2D blackTexture;
    public float fadeAlpha = 0;
    public bool isFading = false;

    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as Fader;

            Instance.blackTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);
            Instance.blackTexture.ReadPixels(new Rect(0, 0, 32, 32), 0, 0, false);
            Instance.blackTexture.SetPixel(0, 0, Color.white);
            Instance.blackTexture.Apply();
        }
    }


    void OnGUI()
    {
        if (!this.isFading)
        {
            return;
        }
        GUI.color = new Color(0, 0, 0, this.fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.blackTexture);
        
    }

    public void LoadLevel(string scene, float interval)
    {
        StartCoroutine(TransScene(scene, interval));
    }

	  
	private IEnumerator TransScene (string scene, float interval)
	{
		this.isFading = true;
		float time = 0;
		while (time <= interval)
		{
			this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);      
			time += Time.deltaTime;
			yield return 0;
		}
	 
		Application.LoadLevel (scene);

		time = 0;
		while (time <= interval)
		{
			this.fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}
	 
		this.isFading = false;
        Die();
	}
    
    void Die()
    {
        mInstance = null;
        Destroy(gameObject);
    }

 
}