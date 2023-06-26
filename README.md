# Genetic-Runners

![image](https://github.com/sabaruto/Genetic-Runners/assets/26776805/d13f0c9b-66fd-4914-ab58-60b99c344f87)

![image](https://github.com/sabaruto/Genetic-Runners/assets/26776805/2fac4b06-53e7-44fb-8d07-40290ce2f1bc)


A demonstration of the genetic algorithm through evolving creatures learning to run. The simulation creates 50 agents, and given 30 seconds to travel as far right as possible. After time is complete, we score each agent on the positive distance from the starting point. This is then repeated with a new group of agents, slightly altered from the inital generation, either from directly copying the structure or mutating certain elements.

## Agent Construction
Each agent has a body and brain. The body is how the agent interacts with the environment and the brain tells the agent what to do given it's obervation. Each agent's body is made of bones, nodes and muscles. The nodes are dark circles that have a collision with only the ground. Bones are connectors of nodes, having no collision with other bones or nodes. Muscles connect different muscles apart and are able to contract or expand depending on the brain's input The brain is a neural network, with inputs being the position of each bone and muscle and the output being the contraction power of each muscle. The node, muscle and bone positions and connections can all be mutated between generations but the main improvement from each agent comes from the mutations of the agent's brain.

## Run
To run the program, download the Build folder and run `Genetic Algorithm` executable. 
