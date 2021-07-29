using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class relationScript : MonoBehaviour
{
    public float size;

    public List<modelAsset> models = new List<modelAsset>();
    public GameObject editOn;

    public bool previewIsOn = false;



    private GameObject lastCreated;

    private GameObject lastCreatedUp; 
    private GameObject lastCreatedDown;
    private GameObject lastCreatedLeft;
    private GameObject lastCreatedRight;
    private GameObject lastCreatedBack;
    private GameObject lastCreatedForward;

    public modelAsset modelAss;

    public Vector3Int size2Fill;

    public float fillPercent;

    public List<unfinishedModelElement> unfinishedModelList = new List<unfinishedModelElement>();


    public void buildObject() {
        removeLastCreated(true);

        lastCreated = GameObject.CreatePrimitive(PrimitiveType.Cube);
        editOn = lastCreated;

        modelAss = new modelAsset();
    }

    public void changeObject(GameObject go) {
        if (go) {
            GameObject newObject = Instantiate(go, Vector3.zero, go.transform.rotation, this.transform);
            newObject.transform.position = lastCreated.transform.position;
            newObject.transform.parent = go.transform.parent;

            DestroyImmediate(lastCreated);
            //This line is not really necessary but i still keep it
            //DestroyImmediate(editOn);

            lastCreated = newObject;
            editOn = newObject;
        }
    }

    public void saveModel() {
        if(modelAss.Prefab != null) {
            modelAss.objHeight = Mathf.Max(1, modelAss.objHeight);
            models.Add(modelAss);
            modelAss = new modelAsset();
        }
    }

    public void removeLastCreated(bool isSelfDestroy) {
        if (isSelfDestroy) {
            DestroyImmediate(lastCreated);
        }
        //This is for returning back to a known state
        DestroyImmediate(lastCreatedUp);
        DestroyImmediate(lastCreatedDown);
        DestroyImmediate(lastCreatedLeft);
        DestroyImmediate(lastCreatedRight);
        DestroyImmediate(lastCreatedBack);
        DestroyImmediate(lastCreatedForward);
    }

    public void visualizeWithModel() {
        if(modelAss.up == null || modelAss.down == null || modelAss.left == null ||
            modelAss.right == null || modelAss.back == null || modelAss.forward == null) {
            return;
        }
        removeLastCreated(false);
        //Because we have 6 different lists this is the way i clear, this could be improved with one two dimensional array but at this time
        //of the implementation I've used this structure, can be improved further with static arrays project wise
        if (modelAss.up.Count > 0) {
            DestroyImmediate(lastCreatedUp);
            lastCreatedUp = Instantiate(modelAss.up[0], transform.position + Vector3.up * Mathf.Max(modelAss.objHeight, 1), modelAss.up[0].transform.rotation, this.transform);
        }
        if(modelAss.down.Count > 0) {
            DestroyImmediate(lastCreatedDown);
            lastCreatedDown = Instantiate(modelAss.down[0], transform.position + Vector3.down, modelAss.down[0].transform.rotation, this.transform);
        }
        if(modelAss.forward.Count > 0) {
            DestroyImmediate(lastCreatedForward);
            lastCreatedForward = Instantiate(modelAss.forward[0], transform.position + Vector3.forward, modelAss.forward[0].transform.rotation, this.transform);
        }
        if(modelAss.back.Count > 0) {
            DestroyImmediate(lastCreatedBack);
            lastCreatedBack = Instantiate(modelAss.back[0], transform.position + Vector3.back, modelAss.back[0].transform.rotation, this.transform);
        }
        if(modelAss.left.Count > 0) {
            DestroyImmediate(lastCreatedLeft);
            lastCreatedLeft = Instantiate(modelAss.left[0], transform.position + Vector3.left, modelAss.left[0].transform.rotation, this.transform);
        }
        if(modelAss.right.Count > 0) {
            DestroyImmediate(lastCreatedRight);
            lastCreatedRight = Instantiate(modelAss.right[0], transform.position + Vector3.right, modelAss.right[0].transform.rotation, this.transform);
        }
    }

    public void visualizeWithPrefab(List<GameObject> l) {
        visualizeWithPrefab(l, 1);
    }

    public void visualizeWithPrefab(List<GameObject> l, int height) {
        if(l.Count != 6) {
            Debug.Log("Not 6");
            return;
        }
        removeLastCreated(false);

        if (l[0] != null) {
            lastCreatedUp = Instantiate(l[0], transform.position + Vector3.up * Mathf.Max(height,1), l[0].transform.rotation, this.transform);
        }
        if (l[1] != null) {
            lastCreatedDown = Instantiate(l[1], transform.position + Vector3.down, l[1].transform.rotation, this.transform);
        }
        if (l[2] != null) {
            lastCreatedForward = Instantiate(l[2], transform.position + Vector3.forward, l[2].transform.rotation, this.transform);
        }
        if (l[3] != null) {
            lastCreatedBack = Instantiate(l[3], transform.position + Vector3.back, l[3].transform.rotation, this.transform);
        }
        if (l[4] != null) {
            lastCreatedLeft = Instantiate(l[4], transform.position + Vector3.left, l[4].transform.rotation, this.transform);
        }
        if (l[5] != null) {
            lastCreatedRight = Instantiate(l[5], transform.position + Vector3.right, l[5].transform.rotation, this.transform);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color32(255, 255, 255, 100);
        Gizmos.color = new Color32(0, 255, 0, 255);
        Gizmos.DrawWireCube(transform.position, size2Fill);
    }
}
