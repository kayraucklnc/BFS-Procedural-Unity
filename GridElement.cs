using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridElement {
    public GameObject prefab = null;
    public Vector3Int posIn3D;

    public GridElement(Vector3Int v){
        posIn3D = v;
    }
    
    public GridElement(Vector3Int v, GameObject p){
        posIn3D = v;
        prefab = p;
    }
}
