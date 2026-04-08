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


    IEnumerator LoadAsync()
    {
        yield return SceneManager.UnloadSceneAsync(2);
        //卸载完成
        //清除不再使用的数据

        yield return SceneManager.LoadSceneAsync(1);
        //加载完成
        //加载新的数据
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
