using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public int objHeight;

	public GameObject prefab;
	public List<GameObject> up = new List<GameObject>();
	public List<GameObject> down = new List<GameObject>();
	public List<GameObject> back = new List<GameObject>();
	public List<GameObject> forward = new List<GameObject>();
	public List<GameObject> right = new List<GameObject>();
	public List<GameObject> left = new List<GameObject>();

	public Node(GameObject prefab, int h) {
		this.prefab = prefab;
        this.objHeight = Mathf.Max(h,1);
	}
}
