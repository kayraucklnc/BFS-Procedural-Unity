###UNITY BLOCK-BASED PROCUDERAL GENERATION TOOL DOCUMENTATION

**PART I – External Sources**

`    `There are a few external apps/add-ons/tools you might want to use during the development of this project that I highly recommend in order to have a flawless transition.

`    `- Visual Studio 2019

`    `- ProGrids – Unity Addon

`    `- Unity 2020.3.1f1

These are not crucial per say fort his Project to work but I highly recommend having the same environment that it is developed with.

##**PART II – Installation**

The installation is quite simple. Download the scripts. Create a “Editor” folder and put “GameManagerEditor.cs” in it. Create a folder called “Scripts” and put the rest of the files in it**, the naming is very important**:

![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.001.png?raw=true)
![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.002.png)

![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.003.png?raw=true)



Add an Empty Object give it a name add the “RelationShipScript” on that object set the Empty Object’s coordiantes to zero-zero-zero. Add models and build structure to test. Start reading Part III for more info. If you see any trouble with installation feel free to email me as the last chance to fix. Also don’t forget to set the fill percent to “1” when first initialized it comes with “0” value this might be the issue for the first starting errors you might encounter. You may use the [fbx.] Collections given in the repository. You need to separate the prefabs and save the prefabs into its own folders. Do it by Unpack Completely option.








**PART III – End-user Usage**

This project is actually created as a tool for artists to use in unity editor.![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.004.png?raw=true)

This is the view you will see after adding the “Relation Script” into any game-object, a master game manager is encouraged.






After adding the script the Scene View should have something similar to this:
![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.005.png?raw=true)

*5x1x5* size wireframe box. This is the area we will try to fill in.

After the artist completes his/her design adding all the prefabs into List simply click into the button after setting all the settings such as fill-percent and fill-size. Fill percent = 0 means don’t fill, Fill percent = 1, fill all the area possible.
![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.006.png?raw=true)
![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.007.png?raw=true)

![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.008.png?raw=true)



**PART III.a – Adding Model**

To add a new model simply press then at the window you will see opened fill the correct information, Model name is only for you to see and will update as the prefab’s name automatically if not filled in, Obj Height is the block height and will be set to 1 automatically if not set. It is important for prefabs with height more than 1. Prefab is the model’s prefab we want to add. The lists are the objects that are allowed next to the prefab, you will also see a visual representation in the Scene View with the last added item to up/down/left/right/back/forward list. Keep in mind that “up” list is instantiated at height “ObjHeight - 1”. When you are satisfied click .
![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.009.png?raw=true)
![](Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.010.png)![](Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.011.png?raw=true)

The model can be easily deleted after clicking show list selecting the model and clicking minus button at the bottom of the list. Currently only way to edit an existing model is to change into debug mode( ) in the inspector and find/edit or delete/re-add.![](Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.012.png)



This might not the best place to start working on since you need to have a good understanding of the structure beforehand.

**PART III.b – Previewing Model![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.013.png?raw=true)**

The model can be re-viewed by clicking on it and further enabling  button. It will select a random item in each up/down/left/right/back/forward and show in small intervals. It also has a small bug waiting to be fixed documented in the code itself.

**PART III.c – Creating Unfinished Model**

After countless hours of research I’ve decided that a fully random system is not enough to have an organic output. This is the reason we have this module. If you enable   button you will see some settings are not ![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.014.png?raw=true)

blocked out and cannot be modified until we disable it. This is necessary for code to work properly. Under that you can add a prefab select its position and add it to the list, three important things here:

- Never add a prefab that doesn’t have a definition in the list above
- Don’t try to add two prefabs at the same place
- This module is not entirely fool-proof, if you see some bug, enable-disable it will solve.![](https://github.com/kayraucklnc/BFS-Procedural-Unity/blob/main/Documentation/Aspose.Words.7fc4461b-33bb-4c81-a099-e9686cd1e903.015.png?raw=true)

If you click  it will use your starting position and build on that part to have a more 

organic look on your builds.


**PART IV.a – Code**

The code is nowhere near perfect, neither best practice in all terms. It works, it works fast, reliable and in an expected way. We’ve found that WFC is the best algorithm for our needs, but we needed a faster approach, so I found this instead.

A breadth-first-search based procedural generation algorithm. Adds each node into the queue and fills from up down left right back front. Looks up for the valid prefabs to add with a Dictionary.

Code has some parts that are commented as “TODO:” these are the parts best to start for future improvements in my opinion. The structure is quite loosely connected so modules can easily be added and removed. I’ve used CamelCase coding standard. If you don’t know you should definitely look up to *“Unity Custom Editor”* and first get to know it. I heavily used it in this project so lack of understanding its features will be a huge obstacle in your way.


**PART IV.b – FAQS(Frequently Asked Question’s Solutions)**

- Grid space and world space has different coordinates, use existing functions accordingly in your needs
- Yes, it can be done in a better and more efficient way but this was already enough.
- Yes, the viewport is being refreshed A LOT and it can be fixed but we are not using such big tile sizes that it slows our work down.
- Don’t change some hardcoded string values of properties of the serialized objects, they work in that way.
- You cannot put an item into a model with no prefab.
- You can put the same prefab into a model more than once, it makes its chance to be spawned higher.
- You have to create a model for every item in your list in practice but with “syncData()” function solves some of the unwanted stuff.
- Yes, you can only use odd number of cells, it is to be able to find the middle cell everytime.









**PART V – Possible Further Work**

- Fool-proof the input system for user caused errors
- Output to [.fbx] file with button
- Editor Frame-rate related bug fixes
- Rotation is not supported
- Objects with height more than one block is supported but it can’t work with prefabs which has a width more than one block.
- At some modes, objects are generated and destroyed every frame, this can be improved for bigger working areas.
- Some items are can be set to be spawned only on certain heights.
- Some item’s up might completely empty, currently there is no restriction for that
- Mirrored objects could be easier to work with.
- BFS starts from middle, a new way of creating can be started from wanted cells but nor pre-determined like “customStart”. Therefore starting can be done in more than one cell as it is currently in the exact middle cell.
- DFS can also be tried, using stack instead of queue
- Now you need to manually import an fbx file and set the correct size for every item this could be automatically done.
- If you are working in blender correct block size is exactly one blender default cube size, which is one meter high. This size could be flexible.

**PART VII – Useful websites**

- <http://graphics.stanford.edu/~pmerrell/thesis.pdf>
- <http://graphics.stanford.edu/~pmerrell/floorplan-final.pdf>
- <http://graphics.stanford.edu/~pmerrell/model_synthesis.pdf>
- <https://bolddunkley.itch.io/wfc-mixed>
- <https://bolddunkley.itch.io/wave-function-collapse>
- <https://github.com/mxgmn/WaveFunctionCollapse>
- <http://oskarstalberg.com/game/wave/wave.html>
- <https://github.com/sylefeb/VoxModSynth>
- <http://graphics.stanford.edu/~pmerrell/continuous.pdf>
- <http://graphics.stanford.edu/~pmerrell/tvcg.pdf>
- <http://oskarstalberg.com/game/house/>
- <https://stalcup.github.io/static-files/posts/wfc-vs-ipm/>
- <https://marian42.itch.io/wfc>
- <https://www.minecraft.net/en-us/article/the-road-dungeons>

