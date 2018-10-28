using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class WreckageManager : MonoBehaviour {
 
    void Start () 
    {
        string Name = "NoData";
 
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0: Name = "Cube";     break;
                case 1: Name = "Cylinder"; break;
                case 2: Name = "Capsule";  break;
                case 3: Name = "Sphere";   break;
            }
            ObjManager.Call().SetObject(Name);
        }
    }
}
