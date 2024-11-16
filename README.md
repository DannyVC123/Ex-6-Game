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

## Devlog

### Dynamic Maze Generation - Nov. 14, 2024
I implemented a randomly generated maze using the Depth-First Search (DFS) algorithm. The generation starts at the bottom-left corner of the grid. From there, the algorithm randomly selects an unvisited neighboring tile, marks it as visited, and updates it to the current tile. This process continues until the current tile has no unvisited neighboring tiles. At this point, the algorithm backtracks to the most recently visited tile with unvisited neighbors and continues exploring. The maze generation is complete once all tiles have been visited, ensuring that every cell in the maze can be reached from any other cell.

![Alt text](https://github.com/DannyVC123/Ex-6-Game/blob/main/Screenshots/maze.png "maze.png")

### Pacman and Ghost Spawning - Nov. 15 2024
Once the maze is generated, Pacman's starting position is randomly selected from an open spot that isn't a wall. The same process is used for spawning the four ghosts, but to prevent Pacman from immediately losing a life when the game starts, the ghosts cannot spawn within a certain radius around Pacman's starting tile. This reduces the chance of an immediate death and gives the player a fairer start to the game.
