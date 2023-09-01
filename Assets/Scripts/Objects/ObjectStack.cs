//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObjectStack : MonoBehaviour
//{
//    public List<GameObject> metalList = new List<GameObject>();
    
    
//    private GameObject lastMetalObject;
//    void Start()
//    {
//        UpdateLastMetalObject();
//    }

//    public void Stacking(GameObject _gameObject)
//    {
//        lastMetalObject = _gameObject;
        
//        lastMetalObject.transform.position = new Vector3(lastMetalObject.transform.position.x, lastMetalObject.transform.position.y+ 0.304f, lastMetalObject.transform.position.z);
//        lastMetalObject.transform.SetParent(transform);
//        metalList.Add(_gameObject);
//        UpdateLastMetalObject();
//    }



//    public void DecreaseStacking(GameObject _gameObject)
//    {
//        lastMetalObject= _gameObject;
//        lastMetalObject.transform.parent = null;
//        Destroy(lastMetalObject);
//        UpdateLastMetalObject();
//    }
//    private void UpdateLastMetalObject()
//    {
//        lastMetalObject = metalList[metalList.Count - 1];

//    }
//}
