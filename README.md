# RunSantaRun

The programm is designed to be changed and expanded in an easy, comfort and safe maneer. 

The game starts from a single object on a scene with the GameEntryPoint script on it. This grants us full control over game entities' instantiation and lifecycle. 

To control  game initialization process state machine design pattern was used. States are presented as single classes. 

Game starts with InitState, where all internal game services are initialized and binded to services' container. 

To manage asynchronous code UniTasks were used, since they are the most optimized desicion for Unity among Tasks and Coroutines. 

Then comes LoadState, where all game units are created by WorldBuilder. For player objects' creation there is an individual PlayerBuilder class. 

To manage game resources addressables were used. This grants us better memory management, less build size and an ability to update content without publishing new version to store.

WorldBuilder gets all necessary data in constructor, loads prefabs from addressables, adds components if they are missing on prefab, provides it with all dependencies they must have and spawns them to game world if needed. 

Components are modular and  are accessed by interfaces, so all you need to do if you wnat to add new or replace existing behaviour is change objects' building method at WorldBuilder.

To control frequently spawned game objects - boosters and obstacles - instantiation process and lifecycle factories derived from abstract factory and  pools were used.

Boosters and obstacles are subchildren of PoolableItem class. To add new booster or obstacle you don't need to change any existing logic - just create new class, make sure it derives from Obstacl or Booster class and realizes IInterractable interface. Add it's instantiation logic to factory's construct method, label your prefab with Booster or Obstacle in addressables settings, add spawning weight to random weight table in Boosters or Obstacles settings scriptable object file and add your settings there if needed. 

The ingame logic is an event-driven system, which supports components' modularity. 

To manage some movement, animation and delayed calls DoTween was used, due to it is a convenient way to work it up. 

To change game rules go to settings folder and set your values at settings scriptable objects files. 

Some classes are provided with summary, the other code is self-documented - classes, fields and methods' names ensure reader's understanding of what their purpose is.
