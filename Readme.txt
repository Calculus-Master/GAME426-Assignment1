Group Members: Sap Mallick, Levi Taylor, Grace Peltan

Note: Project was done using a custom git repo at https://github.com/Calculus-Master/GAME426-Assignment1 because we had two GitHub classroom teams, and couldn't merge them. We spoke with the professor and he said it was fine for this assignment.


1-Navigating a Labyrinth
	Left clicking a square will highlight the shortest path by marking the tiles in green.
	As the player moves across the tiles, the color will revert. If the tile is unreachable, then the player will stop moving. We used a Euclidean distance heuristic for this.

2-Area Effect Attacks.
	Using the hero's magic attack will highlight all tiles within the AoE in cyan, and once the animation ends the tiles will revert back to their default color. All enemies in the radius will be immediately removed.

3-Pursuit and Evasion.
	These steering behaviors were implemented in the top right room.
	Spiked turtles pursue the hero, red slimes evade when the hero enters the room.
	Enemies remain idle until the hero enters their room.
	Enemies do not leave their assigned room.

4-Hero Walk Animation.
	We skipped this part.

5-Wandering Enemies
	The bottom left room contains enemies that wander while remaining within the bounds of the room.

6-Personal Space.
	That same room (bottom left) contains enemies that avoid each other while wandering.


______________________________________________________
DUNGEON CRAWLER (Starter Project) - v. 4.0 
School of Information, University of Arizona 
January 22, 2024

This project may modified freely for GAME 426 / 526 (Game AI) students in their assignments. 

To install and run this code in your project, you must follow
these steps:

1) Create a blank 3D project in Unity Hub. Use the 3D Core  
   template; Do NOT use 3D URP template as this will result
   in lighting issues (Objects will be pink).
2) Allow Unity editor to initialize the project.
3) Save your blank 3D project and exit Unity.
4) Navigate to project directory in file system.
5) Drop the "Assets" and "Project Settings" folders into
   this directory.
6) Open and reload the Unity project you created.
7) In the project navigator, go to the assets directory
   and find the "Scenes" folder.
8) Click on either the scene called "DungeonCrawler".
7) Go to Unity->File->Build Settings and verify that the
   scene is in the "Scenes in Build" list.
8) If the scene is not in the build list, click "Add Open 
   Scenes". DungeonCrawler should appear checked. 
9) The game should be ready for building and running.

Key Mapping:
Mouse Left Click:  Move to given location
Mouse Right Click: Use Sword (Area Effect)

Credits:
 
Sample code by Leonard D. Brown, University of Arizona.
This program was developed for educational purposes only. 
Media assets contained within this program may not be 
redistributed without written permission of authors.

Licensed media assets were used from the following sources:
(1) https://alexkim0415.wixsite.com/dungeonmason
(2) https://coderespawn.com

