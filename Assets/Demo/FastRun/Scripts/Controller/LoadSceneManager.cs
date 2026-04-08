using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void LoadSceneAsync(int unLoad, int load)
    {
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(load);
        asyncOperation.completed += e =>
        {
			AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(unLoad);
		};


		
	}
}
