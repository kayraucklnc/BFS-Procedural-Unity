using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct modelAsset {
	public string modelName;
    public int objHeight;
	public GameObject Prefab;

    public List<GameObject> up;
    public List<GameObject> down;
    public List<GameObject> back;
    public List<GameObject> forward;
    public List<GameObject> right;
    public List<GameObject> left;
}