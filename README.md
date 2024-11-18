# Pacman Game

## Aesthetic Goals:
### Goal 1: Challenge

**Definition:** A game is challenging if it requires the player to use skillful decision-making, quick reflexes, and strategic thinking to overcome obstacles within the game. The difficulty should be present throughout the gameplay, providing constant tension and engagement while testing the player’s abilities.

#### Success:
- The game should maintain a high level of challenge by introducing dynamic obstacles, such as aggressive or unpredictable enemies.
- Players must balance risk and reward, making decisions that require careful planning and quick reactions to succeed.
- The game should offer strategic opportunities, such as utilizing the environment or timing to outmaneuver enemies.
- The player should feel consistently engaged, with moments of tension that challenge their reflexes and decision-making, keeping the experience exciting.

#### Failure:
- The game is too easy, with little need for skill or strategic thinking, resulting in a lack of challenge.
- The difficulty is stagnant or trivial, with no real obstacles or threats that create tension or test the player.
- The obstacles or enemies are too predictable or passive, causing the player to feel like success is guaranteed without significant effort.

### Goal 2: Replayability
**Definition:** A game is replayable if the player can enjoy playing it repeatedly, with each playthrough offering a fresh and unique experience. While the player’s skill may improve over time, the game should retain enough variation and novelty to feel engaging, preventing the experience from becoming repetitive.

#### Success:
- The game offers variations in gameplay, whether through randomized elements (such as map layout, enemy behavior, or power-up placements) or evolving challenges that encourage the player to keep coming back.
- The player can approach the game differently with each playthrough, whether by experimenting with new strategies or discovering new outcomes in gameplay.
- The player can improve their performance with each playthrough, refining their skills, learning from past attempts, and striving for better results (e.g., higher scores or faster completion times).

#### Failure:
- The game feels repetitive after a few playthroughs because the gameplay doesn’t change in meaningful ways, offering little variation or novelty.
- The player quickly becomes bored or frustrated because the game starts to feel predictable or stagnant.
- Randomness or variations don’t sufficiently affect gameplay in a way that encourages further play, making the game feel too predictable.

## Core Loop:
- Navigate the maze by turning pacman toward pellets and away from ghosts
- Eat pellets to increase your score and progress toward winning
- Avoid ghosts to prevent losing lives; lose a life if caught by a ghost
- Repeat until you either collect enough pellets to win or lose all three lives

## Devlog

### Dynamic Maze Generation - Nov. 14, 2024
A maze is randomly generated using the Depth-First Search (DFS) algorithm. The generation starts at the bottom-left corner of the grid. From there, the algorithm randomly selects an unvisited neighboring tile, marks it as visited, and updates it to the current tile. This process continues until the current tile has no unvisited neighboring tiles. At this point, the algorithm backtracks to the most recently visited tile with unvisited neighbors and continues exploring. The maze generation is complete once all tiles have been visited, ensuring that every cell in the maze can be reached from any other cell.

![Alt text](https://github.com/DannyVC123/Ex-6-Game/blob/main/Screenshots/maze.png "maze.png")

### Pacman and Ghost Spawning - Nov. 15, 2024
Once the maze is generated, Pacman's starting position is randomly selected from an open spot that isn't a wall. The same process is used for spawning the four ghosts, but to prevent Pacman from immediately losing a life when the game starts, the ghosts cannot spawn within a certain radius around Pacman's starting tile. This reduces the chance of an immediate death and gives the player a fairer start to the game.

![Alt text](https://github.com/DannyVC123/Ex-6-Game/blob/main/Screenshots/spawning.png "spawning.png")

### Pacman Movement - Nov. 15, 2024
Pacman continuously moves in the direction he is facing. The player can change Pacman's orientation using the W, A, S, and D keys or the arrow keys.

### Ghost Movement - Nov. 16, 2024
Every ghost has a unique target tile to create a distinct challenge for each ghost. In the original game, each ghost checks its neighboring tiles and moves to the tile with the smallest euclidean distance to its target tile. However, because the generated mazes are unique each time and may contain dead ends, this approach is not feasible. Instead, a Breadth-First Search (BFS) algorithm is used to find the shortest path from the ghost's current position to its target tile and moves one tile along the path. The path is recalculated every frame.

#### Target Tiles
- <span style="color:red">**Blinky:**</span> Blinky’s target tile is always Pacman’s current position, constantly chasing him down. <br>
![Alt text](https://media.gameinternals.com/pacman-ghosts/blinky-targeting.png "blinky target")
- <span style="color:pink">**Pinky:**</span> Pinky’s target tile is four tiles in front of Pacman, attempting to anticipate his movements. <br>
![Alt text](https://media.gameinternals.com/pacman-ghosts/pinky-targeting.png "pinky target")
- <span style="color:cyan">**Inky:**</span> Inky's target is determined by creating a vector from Blinky’s current tile to a point two tiles in front of Pacman. This vector is then doubled in length, and the endpoint becomes Inky’s target. This strategy allows Inky to work with Blinky and trap Pacman on both sides. <br>
![Alt text](https://media.gameinternals.com/pacman-ghosts/inky-targeting.png "inky target")
- <span style="color:orange">**Clyde:**</span> Clyde’s behavior depends on his distance from Pacman. If he is more than eight tiles away, his target is Pacman’s current position. If he’s within eight tiles, Clyde's target switches to the bottom left corner of the maze. This makes Clyde appear threatening at first, but his sudden changes in behavior can be exploited by experienced players. <br>
![Alt text](https://media.gameinternals.com/pacman-ghosts/clyde-targeting2.png "clyde target 1")
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
![Alt text](https://media.gameinternals.com/pacman-ghosts/clyde-targeting.png "clyde target 1")

If any of the target tiles are out of bounds of the maze, then the closest valid tile is selected for the target tile instead.

*Images  from [GameInternals: Understanding Pac-Man Ghost Behavior](https://gameinternals.com/understanding-pac-man-ghost-behavior)*.

### Maze Generation Update - Nov. 16, 2024

Generating the maze using DFS results in a perfect, loop-free structure, where each tile has only one unique path. While this ensures a challenging maze, it also makes it nearly impossible to avoid the ghosts. If you reach a dead end, the only way out is to backtrack, but since the ghosts follow the same path, they can trap you in these dead ends.

To address this issue, I modified the maze generation process. I am still generating the maze with DFS, but afterward I delete random walls. This introduces more paths, improving the chances of avoiding the ghosts by providing alternative escape routes.

### Chase Mode & Scatter Mode - Nov. 17, 2024

In Pacman, the ghost behavior is divided into two primary modes: Chase Mode and Scatter Mode.

The ghost behavior described above is known as Chase Mode. In Chase Mode, the ghosts actively pursue Pacman, working together to capture him. While Chase Mode keeps the game thrilling, having the ghosts constantly in pursuit can make the game feel overwhelmingly difficult for the player.

To balance this, Scatter Mode is introduced. Every few seconds, the ghosts switch from Chase Mode to Scatter Mode. In Scatter Mode, instead of targeting Pac-Man, each ghost moves towards a designated corner of the maze. This provides brief moments of relief, as the ghosts spread out and become less aggressive, allowing the player to evade them more easily. The alternating pattern between Chase and Scatter Modes creates a fairer challenge and makes gameplay more engaging, offering a tactical rhythm that skilled players can use to their advantage.

### Lives System - Nov. 17, 2024

The player starts out with three lives. When Pacman collides with a ghost, they lose a life. Each time they lose a life, the maze resets, giving Pacman and the ghosts new starting positions. When all three lives are lost, a "Game Over" message is displayed, and the game ends.

### Pellets and Scoring - Nov. 17, 2024

When the game starts, all non-wall tiles contain a pellet. If Pacman eats a pellet, his score increases by one point. In the original game, Pacman must collect all the pellets to complete the level. However, because my game uses dynamic maze generation, some pellets can end up in challenging spots, like dead ends. To account for this, the player only needs to collect 300 pellets to win the game. Upon reaching this goal, a "You Win" message is displayed, but the player can continue playing the game to achieve a higher score until all three lives are lost.

If the player continues playing after winning, they will eventually see both a "You Win" and a "Game Over" message simultaneously. This is intentional, indicating that the player has achieved the set goal but has now exhausted all their lives.

![Alt text](https://github.com/DannyVC123/Ex-6-Game/blob/main/Screenshots/pellets.png "spawning.png")

### Sound - Nov. 17, 2024

My game features a variety of sound effects:

- **Pacman Eating:** This classic sound plays continuously throughout the game until Pacman loses all three lives.
- **Pacman Death:** A death sound is triggered each time Pacman loses a life, providing an auditory cue and a brief pause before Pacman respawns.
- **Score Milestones:** A short sound effect is played whenever the player's score reaches a multiple of 100 (e.g., 100, 200, 300). This audio cue helps the player track their progress without needing to constantly check the score.













