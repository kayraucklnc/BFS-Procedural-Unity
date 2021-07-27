using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Linq;


//Kayra Uckilinc 2021

[CustomEditor(typeof(relationScript))]
public class GameManagerEditor : Editor {


    private ReorderableList list;
	private GameObject selected;
	private int selectedHeight;
	private bool isInCreateMode = false;
	GameObject currentGOtoEdit;
	List<GameObject> currentGONeighbour = new List<GameObject>();
	List<GameObject> createModePrefabList = new List<GameObject>();

	private SerializedProperty modelAssetToEdit;

	private bool isRandomizeStarted = false;
	private int editorLoopCount = 0;

	private List<SerializedProperty> rotateBetween = new List<SerializedProperty>();


	private Vector3Int sizeToFill;

	private bool showPos;

	private ReorderableList unfinishedModelList;
	private bool makeCustomStart;
	private GameObject customItem;
	private GameObject customItemPlaced;
	private Vector3Int customItemPos;
	private bool isAdded = false;
	private bool isValidToCustom = true;
	private List<GameObject> addedList = new List<GameObject>();

	private void OnEnable() {
		relationScript myScript = (relationScript)target;
		myScript.size2Fill = new Vector3Int(5,1,5);
		sizeToFill = myScript.size2Fill;

		modelAssetToEdit = serializedObject.FindProperty("modelAss");

		list = new ReorderableList(serializedObject,
				serializedObject.FindProperty("models"),
				true, true, false, true);

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
		var element = list.serializedProperty.GetArrayElementAtIndex(index);
		rect.y += 2;
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("modelName"), GUIContent.none);
        EditorGUI.PropertyField(
			new Rect(rect.x + rect.width * 0.3f + 5, rect.y, rect.width * 0.7f, EditorGUIUtility.singleLineHeight),
			element.FindPropertyRelative("Prefab"), GUIContent.none);
        };

        list.onAddCallback = (ReorderableList l) => {
			var index = l.serializedProperty.arraySize;
			l.serializedProperty.arraySize++;
			l.index = index;
			var element = l.serializedProperty.GetArrayElementAtIndex(index);
			element.FindPropertyRelative("modelName").stringValue = "New Model";
			element.FindPropertyRelative("Prefab").objectReferenceValue = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Cube.prefab",
			typeof(GameObject)) as GameObject; ;
		};

		list.onSelectCallback = (ReorderableList l) => {
			relationScript myScript = (relationScript)target;

			var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;

			myScript.removeLastCreated(true);
			myScript.buildObject();
			myScript.changeObject(prefab);

			rotateBetween.Clear();
			rotateBetween.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("up"));
			rotateBetween.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("down"));
			rotateBetween.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("forward"));
			rotateBetween.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("back"));
			rotateBetween.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("left"));
			rotateBetween.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("right"));

			currentGONeighbour.Clear();
			if(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("up").arraySize != 0) {
				currentGONeighbour.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("up").GetArrayElementAtIndex(0).objectReferenceValue as GameObject);
			}
			else {
				currentGONeighbour.Add(null);
			}
			if(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("down").arraySize != 0) {
				currentGONeighbour.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("down").GetArrayElementAtIndex(0).objectReferenceValue as GameObject);
			}
			else {
				currentGONeighbour.Add(null);
			}
			if (l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("forward").arraySize != 0) {
				currentGONeighbour.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("forward").GetArrayElementAtIndex(0).objectReferenceValue as GameObject);
			}
			else {
				currentGONeighbour.Add(null);
			}
			if (l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("back").arraySize != 0) {
				currentGONeighbour.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("back").GetArrayElementAtIndex(0).objectReferenceValue as GameObject);
			}
			else {
				currentGONeighbour.Add(null);
			}
			if (l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("left").arraySize != 0) {
				currentGONeighbour.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("left").GetArrayElementAtIndex(0).objectReferenceValue as GameObject);
			}
			else {
				currentGONeighbour.Add(null);
			}
			if (l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("right").arraySize != 0) {
				currentGONeighbour.Add(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("right").GetArrayElementAtIndex(0).objectReferenceValue as GameObject);
			}
			else {
				currentGONeighbour.Add(null);
			}
			myScript.visualizeWithPrefab(currentGONeighbour, l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("objHeight").intValue);


			EditorGUIUtility.PingObject(prefab.gameObject);
			selected = prefab;
			currentGOtoEdit = prefab;
			selectedHeight = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("objHeight").intValue;
		};

		//---------------------------------------------------------------------------------------------

		unfinishedModelList = new ReorderableList(serializedObject,
				serializedObject.FindProperty("unfinishedModelList"),
				true, true, false, true);

		unfinishedModelList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = unfinishedModelList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("prefab"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width * 0.3f + 5, rect.y, rect.width * 0.7f, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("pos"), GUIContent.none);
        };
    }



	private void OnDisable() {
		//Clicking outside, resets everything
		relationScript myScript = (relationScript)target;
		EditorApplication.update -= randomizeCoroutine;

		isInCreateMode = false;
		selected = null;
        myScript.removeLastCreated(true);
        //deleteStructure();
    }

    public override void OnInspectorGUI() {
		relationScript myScript = (relationScript)target;
		var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, fontSize = 20, stretchHeight = true, fixedHeight = 50};


		//If you want to see the normal way uncomment this line
        //DrawDefaultInspector();

        EditorGUILayout.LabelField("Building settings", style, GUILayout.ExpandWidth(true));
		EditorGUI.BeginDisabledGroup(makeCustomStart);
		DrawUILine(Color.grey, 3, 25);
		sizeToFill = EditorGUILayout.Vector3IntField("Size to Fill:", sizeToFill);
		myScript.size2Fill = sizeToFill;
        EditorGUILayout.Space(10f);



		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Percent of filled area");
		myScript.fillPercent = EditorGUILayout.Slider(myScript.fillPercent, 0, 1);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUI.BeginDisabledGroup(list.count < 0);
		if (GUILayout.Button("Build the Castle!")) {
			buildTheCastle();
		}

		if (GUILayout.Button("Delete Structure")) {
			deleteStructure();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUI.EndDisabledGroup();
		EditorGUI.EndDisabledGroup();

		EditorGUILayout.LabelField("Block Generation", style, GUILayout.ExpandWidth(true));
		DrawUILine(Color.grey, 3, 25);

		EditorGUI.BeginDisabledGroup(makeCustomStart);
		myScript.editOn = selected;

        if (isInCreateMode) {
			if(currentGONeighbour.Count == 6) {
				myScript.visualizeWithPrefab(currentGONeighbour, modelAssetToEdit.FindPropertyRelative("objHeight").intValue);
            }
        }


		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Create Model")) {
			deleteStructure();
			selected = null;
			EditorApplication.update -= randomizeCoroutine;

			myScript.removeLastCreated(true);
			myScript.buildObject();

            isInCreateMode = true;
			currentGOtoEdit = modelAssetToEdit.FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
		}


		//Enables after you create model
		EditorGUI.BeginDisabledGroup(isInCreateMode == false);
		if (GUILayout.Button("Add This Model")) {
			isInCreateMode = false;
			myScript.saveModel();
		}
		EditorGUI.EndDisabledGroup();
		GUILayout.EndHorizontal();

		if (isInCreateMode) {
			//We dont need to clear because we only update the content not re-create it again and again
			//createModePrefabList.Clear();
			createUpdater(createModePrefabList, "up", 0);
			createUpdater(createModePrefabList, "down", 1);
			createUpdater(createModePrefabList, "forward", 2);
			createUpdater(createModePrefabList, "back", 3);
			createUpdater(createModePrefabList, "left", 4);
			createUpdater(createModePrefabList, "right", 5);
			myScript.visualizeWithPrefab(createModePrefabList, modelAssetToEdit.FindPropertyRelative("objHeight").intValue);
            serializedObject.Update();

            EditorGUILayout.PropertyField(modelAssetToEdit, new GUIContent("New Model"), true);
			if (modelAssetToEdit.FindPropertyRelative("Prefab").objectReferenceValue != currentGOtoEdit) {
				myScript.changeObject(modelAssetToEdit.FindPropertyRelative("Prefab").objectReferenceValue as GameObject);
				currentGOtoEdit = modelAssetToEdit.FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
				if (currentGOtoEdit) {
					modelAssetToEdit.FindPropertyRelative("modelName").stringValue = currentGOtoEdit.name;
				}
			}
			serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginDisabledGroup(selected == null);
		if (GUILayout.Button(isRandomizeStarted ? "Stop Randomize" : "Start Randomize")) {
			//This event subscribing needs some attention, unwanted behaviour might occur if we forget to unsubscribe
			if (isRandomizeStarted) {
				EditorApplication.update -= randomizeCoroutine;
			}
			else {
				EditorApplication.update += randomizeCoroutine;
			}
			isRandomizeStarted = !isRandomizeStarted;
		}
		EditorGUI.EndDisabledGroup();

		showPos = EditorGUILayout.Foldout(showPos, showPos ? "Hide List⤵" : "Show List⤴", true);
        if (showPos) {
			list.DoLayoutList();
        }
		EditorGUI.EndDisabledGroup();

        EditorGUILayout.LabelField("Unfinished Model Generation", style, GUILayout.ExpandWidth(true));
        DrawUILine(Color.grey, 3, 25);


        makeCustomStart = EditorGUILayout.Toggle("Use Unfinished Model:", makeCustomStart);
        if (makeCustomStart) {
            DrawUILine(Color.gray, 1, 5, 30);
            if (!isAdded) {
                deleteStructure();
                EditorApplication.update -= randomizeCoroutine;
                addedList.Clear();
                foreach (unfinishedModelElement uME in myScript.unfinishedModelList) {
                    addedList.Add(Instantiate(uME.prefab, uME.pos, uME.prefab.transform.rotation, myScript.transform));
                }
                isAdded = true;
            }

            customItem = (GameObject)EditorGUILayout.ObjectField("Item:", customItem, typeof(GameObject), false);
            Vector3Int tempInt = EditorGUILayout.Vector3IntField("Pos of the item", customItemPos);
            if (Mathf.Abs(tempInt.x) <= (sizeToFill.x - 1) / 2) {
                if (Mathf.Abs(tempInt.y) <= (sizeToFill.y - 1) / 2) {
                    if (Mathf.Abs(tempInt.z) <= (sizeToFill.z - 1) / 2) {
                        customItemPos = tempInt;
                    }
                }
            }

            if (GUILayout.Button("Add Unfinished Model")) {
                unfinishedModelElement tempU = new unfinishedModelElement();
				if(tempU.prefab != null) {
					tempU.prefab = customItem;
					tempU.pos = customItemPos;
					myScript.unfinishedModelList.Add(tempU);
					isAdded = false;
					isValidToCustom = isValid2Custom();
					if (!isValidToCustom) {
						myScript.unfinishedModelList.RemoveAt(myScript.unfinishedModelList.Count - 1);
					}
                }
            }

            EditorGUILayout.Space(20f);

            unfinishedModelList.DoLayoutList();
            if (customItem != null) {
                if (customItemPlaced == null) {
                    DestroyImmediate(customItemPlaced);
                    customItemPlaced = Instantiate(customItem, customItemPos, customItem.transform.rotation, myScript.transform);
                }
                else if (!customItemPlaced.name.Substring(0, customItemPlaced.name.Length - 7).Equals(customItem.name)) {
                    DestroyImmediate(customItemPlaced);
                    customItemPlaced = Instantiate(customItem, customItemPos, customItem.transform.rotation, myScript.transform);
                }
                customItemPlaced.transform.position = customItemPos;
            }

            if (GUILayout.Button("Finish the building!")) {
				//If the size is changed this makes sure we are working on "at least" the correct size
				int xMax = myScript.unfinishedModelList.Max(o => Mathf.Abs(o.pos.x));
				int yMax = myScript.unfinishedModelList.Max(o => Mathf.Abs(o.pos.y));
				int zMax = myScript.unfinishedModelList.Max(o => Mathf.Abs(o.pos.z));

				int xMaxU = Mathf.Max(myScript.size2Fill.x, xMax * 2 + 1);
				int yMaxU = Mathf.Max(myScript.size2Fill.y, yMax * 2 + 1);
				int zMaxU = Mathf.Max(myScript.size2Fill.z, zMax * 2 + 1);

				myScript.size2Fill = new Vector3Int(xMaxU, yMaxU, zMaxU);
				sizeToFill = myScript.size2Fill;

				buildTheCastle(myScript.unfinishedModelList);
            }


        }
        else {
			//If custom start checkbox is deactivated
            DestroyImmediate(customItemPlaced);
            for (int i = addedList.Count - 1; i >= 0; i--) {
                DestroyImmediate(addedList[i]);
            }
            addedList.Clear();
            isAdded = false;

			//You can delete the structure if you want
            //deleteStructure();
        }

        DrawUILine(Color.grey, 3, 25);


        if (sizeToFill.x % 2 == 0) sizeToFill.x++;
		if (sizeToFill.y % 2 == 0) sizeToFill.y++;
		if (sizeToFill.z % 2 == 0) sizeToFill.z++;

		serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
	}

    private bool isValid2Custom() {
		//If you are trying to add an custom object item but the grid is already filled this is the way to know it
		relationScript myScript = (relationScript)target;

		Dictionary<GameObject, Node> allItems = new Dictionary<GameObject, Node>();
		fillAllItems(allItems);

		GameObject[,,] test3D = new GameObject[sizeToFill.x, sizeToFill.y, sizeToFill.z];
		foreach (unfinishedModelElement uME in myScript.unfinishedModelList) {
			//Look if any item is colliding with any other if not, with these given start the procedure!!!
			for (int i = 0; i < allItems[uME.prefab].objHeight; i++) {
				Vector3Int nowTest = world2grid(uME.pos);
				if (test3D[nowTest.x, nowTest.y + i, nowTest.z] != null) {
					return false;
                }
                else {
					test3D[nowTest.x, nowTest.y + i, nowTest.z] = uME.prefab;
				}
			}
		}
		return true;
	}
    private void createUpdater(List<GameObject> lst, string str, int idx) {
		//TODO: Could be optimized, if the same => dont recreate the object
		if(lst.Count == 6) {
			if (modelAssetToEdit.FindPropertyRelative(str).arraySize > 0) {
				lst[idx] = (modelAssetToEdit.FindPropertyRelative(str).GetArrayElementAtIndex(modelAssetToEdit.FindPropertyRelative(str).arraySize - 1).objectReferenceValue as GameObject);
			}
			else {
				lst[idx] = null;
			}
		}
        else {
			if(modelAssetToEdit.FindPropertyRelative(str).arraySize > 0) {
				lst.Add(modelAssetToEdit.FindPropertyRelative(str).GetArrayElementAtIndex(modelAssetToEdit.FindPropertyRelative(str).arraySize - 1).objectReferenceValue as GameObject);
			}
			else {
				lst.Add(null);
			}
        }
	}
	private void deleteStructure() {
		relationScript myScript = (relationScript)target;
		myScript.removeLastCreated(true);

		for (int i = myScript.transform.childCount; i > 0; --i) {
			DestroyImmediate(myScript.transform.GetChild(0).gameObject);
        }
	}
	private void randomizeCoroutine() {
		//TODO: This script is called more if you randomly move your mouse on inspector, can be improved
		relationScript myScript = (relationScript)target;

		editorLoopCount++;
		if(editorLoopCount == 200) {
			editorLoopCount = 0;
            List<GameObject> toRet = new List<GameObject>();

            for (int i = 0; i < 6; i++) {
				int randNo = Random.Range(0, rotateBetween[i].arraySize);

				if(rotateBetween[i].arraySize == 0) {
					toRet.Add(null);
                }
                else {
					toRet.Add(rotateBetween[i].GetArrayElementAtIndex(randNo).objectReferenceValue as GameObject);
                }
            }

            myScript.visualizeWithPrefab(toRet, selectedHeight);
        }
    }
	private void buildTheCastle(List<unfinishedModelElement> optionalStart = null) {
		relationScript myScript = (relationScript)target;
        deleteStructure();

        Dictionary<GameObject, Node> allItems = new Dictionary<GameObject, Node>();
		//TODO: This "fillAllItems()" doesn't actually needs to be called in every "buildTheCastle()" call, only if the content is changed
		fillAllItems(allItems);


        GridElement[,,] list3D = new GridElement[sizeToFill.x, sizeToFill.y, sizeToFill.z];
        for (int i = 0; i < sizeToFill.x; i++) {
            for (int j = 0; j < sizeToFill.y; j++) {
                for (int k = 0; k < sizeToFill.z; k++) {
					Vector3Int realV = grid2world(new Vector3Int(i, j, k));
					list3D[i, j, k] = new GridElement(realV);
                }
            }
        }


		Queue<GridElement> que = new Queue<GridElement>();
		
		if(optionalStart == null) {
			GameObject randObj = list.serializedProperty.GetArrayElementAtIndex(Random.Range(0, list.serializedProperty.arraySize)).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
			//Debug purposes if you only want to spawn from first element change 0⤵
			//GameObject randObj = list.serializedProperty.GetArrayElementAtIndex(0).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;

			que.Enqueue(new GridElement(new Vector3Int(0, 0, 0), randObj));
			Node nd = allItems[randObj];
            int objHeight = nd.objHeight;
            Instantiate(randObj, myScript.transform.position, randObj.transform.rotation, myScript.transform);

			Vector3Int gridCenter = world2grid(new Vector3Int(0, 0, 0));
			list3D[gridCenter.x , gridCenter.y, gridCenter.z] = new GridElement(new Vector3Int(0,0,0), randObj);
		
			//For objects with height
			try {
				for (int i = 1; i < objHeight; i++) {
					list3D[0, 0 + i, 0] = new GridElement(new Vector3Int(0, 0 + i, 0), randObj);
				}
			}
			catch {
			}
        }
        else {
			foreach (unfinishedModelElement uME in optionalStart) {
				Vector3Int tempPos = world2grid(uME.pos);
				
				que.Enqueue(new GridElement(uME.pos, uME.prefab));
                Instantiate(uME.prefab, new Vector3(uME.pos.x, uME.pos.y, uME.pos.z), uME.prefab.transform.rotation, myScript.transform);

				list3D[tempPos.x, tempPos.y, tempPos.z] = new GridElement(new Vector3Int(uME.pos.x, uME.pos.y, uME.pos.z), uME.prefab);

				//For objects with height
				try {
					for (int i = 1; i < allItems[uME.prefab].objHeight; i++) {
						list3D[tempPos.x, tempPos.y + i, tempPos.z] = new GridElement(new Vector3Int(uME.pos.x, uME.pos.y + i, uME.pos.z), uME.prefab);
					}
				}
				catch {
				}
			}
		}


        while (que.Count > 0) {
            recursiveBuild(que, list3D, allItems);
        }

	}
	public void recursiveBuild(Queue<GridElement> q, GridElement[,,] list3D, Dictionary<GameObject, Node> allItems) {
		relationScript myScript = (relationScript)target;
		GridElement ge = q.Dequeue();
		Node n = allItems[ge.prefab];

		recursiveInside(n.up, ge.posIn3D.y, ge.posIn3D, sizeToFill.y, Vector3Int.up, myScript, q, list3D, n.objHeight);
		recursiveInside(n.down, ge.posIn3D.y, ge.posIn3D, sizeToFill.y, Vector3Int.down, myScript, q, list3D, n.objHeight);
		recursiveInside(n.right, ge.posIn3D.x, ge.posIn3D, sizeToFill.x, Vector3Int.right, myScript, q, list3D, n.objHeight);
		recursiveInside(n.left, ge.posIn3D.x, ge.posIn3D, sizeToFill.x, Vector3Int.left, myScript, q, list3D, n.objHeight);
		recursiveInside(n.forward, ge.posIn3D.z, ge.posIn3D, sizeToFill.z, Vector3Int.forward, myScript, q, list3D, n.objHeight);
		recursiveInside(n.back, ge.posIn3D.z, ge.posIn3D, sizeToFill.z, Vector3Int.back, myScript, q, list3D, n.objHeight);
	}
    private void recursiveInside(List<GameObject> randList, int limit, Vector3Int gePos, int limitTo, Vector3Int nextVector, relationScript myScript, Queue<GridElement> q ,GridElement[,,] list3D, int objHeight) {
		Vector3Int checkInt = world2grid(gePos + nextVector);

        try {
			if (list3D[checkInt.x, checkInt.y, checkInt.z].prefab != null) {
				//If the block we are about to fill is already filled
				return;
			}
        }
        catch {
			//If the block we are about to fill is out of bounds
			return;
        }

        if (Random.Range(0f, 1f) > myScript.fillPercent) {
			//If fillPercent==1 it fills entire grid
			list3D[checkInt.x, checkInt.y, checkInt.z].prefab = list3D[(sizeToFill.x - 1) / 2, (sizeToFill.y - 1) / 2, (sizeToFill.z - 1) / 2].prefab;
			return;
        }
        else {
			if (randList.Count > 0) {
				GameObject rand = randList[Random.Range(0, randList.Count - 1)];

				//If we are not on the sides
				if (Mathf.Abs(limit) <= (limitTo - 1) / 2) {
					try {
						for (int i = 0; i < objHeight; i++) {
							if(list3D[checkInt.x, checkInt.y + i, checkInt.z].prefab != null) {
								return;
							}
						}
					}
					catch {
						return;
					}

					GameObject placed = Instantiate(rand, gePos + nextVector + Vector3Int.up * (objHeight - 1), rand.transform.rotation, myScript.transform);
					for (int i = 0; i < objHeight; i++) {
						list3D[checkInt.x, checkInt.y + i, checkInt.z].prefab = placed;
					}
                    q.Enqueue(new GridElement(gePos + nextVector + Vector3Int.up * (objHeight - 1), rand));
                }
			}
        }

	}
	private void fillAllItems(Dictionary<GameObject, Node> allItems) {
		for (int i = 0; i < list.serializedProperty.arraySize; i++) {
			GameObject pf = list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
			int pHeight = list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("objHeight").intValue;

			Node node;
			if (!allItems.ContainsKey(pf)) {
				node = new Node(pf, pHeight);
				allItems.Add(pf, node);
			}
			else {
				node = allItems[pf];
				allItems[pf].objHeight = pHeight;
			}
			iterateOver(list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("up"), node.up, allItems);
			iterateOver(list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("down"), node.down, allItems);
			iterateOver(list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("forward"), node.forward, allItems);
			iterateOver(list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("back"), node.back, allItems);
			iterateOver(list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("left"), node.left, allItems);
			iterateOver(list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("right"), node.right, allItems);
		}

		//A's left has B but B's right doesn't have A, this fixes that issue
		syncData(allItems);
	}
	public void iterateOver(SerializedProperty sList, List<GameObject> toAdd, Dictionary<GameObject, Node> dict) {
		for(int i = 0; i < sList.arraySize; i++) {
			GameObject elm = sList.GetArrayElementAtIndex(i).objectReferenceValue as GameObject;

			if (dict.ContainsKey(elm)) {
				toAdd.Add(dict[elm].prefab);
			}
            else {
				Node tempNode = new Node(elm, 1);
				dict.Add(elm, tempNode);
				toAdd.Add(elm);
            }
        }
    }
    public Vector3Int grid2world(Vector3Int input) {
		return new Vector3Int(input.x - ((sizeToFill.x - 1) / 2), input.y - ((sizeToFill.y - 1) / 2), input.z - ((sizeToFill.z - 1) / 2));
	}
    public Vector3Int world2grid(Vector3Int input) {
		return new Vector3Int(input.x + ((sizeToFill.x - 1) / 2), input.y + ((sizeToFill.y - 1) / 2), input.z + ((sizeToFill.z - 1) / 2));
	}
	public void syncData(Dictionary<GameObject, Node> allItems) {
		//A's left has B but B's right doesn't have A, this fixes that issue
		foreach (Node n in allItems.Values) {
			foreach (GameObject go in n.left) {
				if (!allItems[go].right.Contains(n.prefab)) {
					//Debug.Log(n.prefab + " doesnt have " + allItems[go].prefab);
					allItems[go].right.Add(n.prefab);
				}
			}


			foreach (GameObject go in n.right) {
				if (!allItems[go].left.Contains(n.prefab)) {
					//Debug.Log(n.prefab + " doesnt have " + allItems[go].prefab);
					allItems[go].left.Add(n.prefab);
				}
			}

			foreach (GameObject go in n.up) {
				if (n.objHeight == 1 && !allItems[go].down.Contains(n.prefab)) {
					allItems[go].down.Add(n.prefab);
				}
			}

			foreach (GameObject go in n.left) {
				if (!allItems[go].right.Contains(n.prefab)) {
					allItems[go].right.Add(n.prefab);
				}
			}

			foreach (GameObject go in n.down) {
				if (!allItems[go].up.Contains(n.prefab)) {
					allItems[go].up.Add(n.prefab);
				}
			}

			foreach (GameObject go in n.forward) {
				if (!allItems[go].back.Contains(n.prefab)) {
					allItems[go].back.Add(n.prefab);
				}
			}

			foreach (GameObject go in n.back) {
				if (!allItems[go].forward.Contains(n.prefab)) {
					allItems[go].forward.Add(n.prefab);
				}
			}
		}
	}
	public void DrawUILine(Color color, int thick = 2, int padding = 10, int widthMinus = 6) {
		Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thick));
		r.height = thick;
		r.y += padding / 2;
		r.x -= 8 - widthMinus;
        r.width -= widthMinus;
        EditorGUI.DrawRect(r, color);
    }
}