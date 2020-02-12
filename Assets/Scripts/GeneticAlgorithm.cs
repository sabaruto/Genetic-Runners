using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{

  private CreatureCreate newCreature;
  private LineManager[] graphs;

  private List<CreatureCreate> creatures;
  private List<float> allTimeBestFitness;
  private List<float> allTimeAverageFitness;
  private List<float> allTimeWorstFitness;
  private List<DNA> collectedGenomes;

  private List<float> currentProbability;
  private List<GameObject> collectedGameObjects;

  private float mutateRate;
  private int populationSize;

  public GameObject jointObject, boneObject, muscleObject, lines;
  public int generation;
  public float greatestFitness, averageFitness, worstFitness, timer;

  private void Awake()
  {
    // The list of creatures in a run
    creatures = new List<CreatureCreate>();

    // All the different attributes for the creatures
    collectedGenomes = new List<DNA>();
    collectedGameObjects = new List<GameObject>();
    currentProbability = new List<float>();

    // The graph values for the fitness eac generation
    allTimeBestFitness = new List<float>() { 0 };
    allTimeAverageFitness = new List<float>() { 0 };
    allTimeWorstFitness = new List<float>() { 0 };

    GameObject bestLine = Instantiate(lines, new Vector3(1, 3, 0), 
                                      transform.rotation, null);
    GameObject averageLine = Instantiate(lines, new Vector3(1, 3, 0), 
                                         transform.rotation, null);
    GameObject worstLine = Instantiate(lines, new Vector3(1, 3, 0), 
                                       transform.rotation, null);

    graphs = new LineManager[3];
    graphs[0] = bestLine.GetComponent<LineManager>();
    graphs[1] = averageLine.GetComponent<LineManager>();
    graphs[2] = worstLine.GetComponent<LineManager>();


    // The hyperparameters
    Random.InitState(1);
    mutateRate = 0.1f;
    populationSize = 50;

    // Initialising the values for the timer and generation
    timer = 0;
    generation = 1;


    // Starting the algorithm
    StartPopulation(populationSize);
  }

  CreatureCreate CreateSpecies(DNA genome = null)
  {
    GameObject newSpecies = new GameObject();
    collectedGameObjects.Add(newSpecies);
    newCreature = newSpecies.AddComponent<CreatureCreate>();
    bool v = (genome == null);
    newCreature.genome = v ? null : genome;
    newCreature.jointObject = jointObject;
    newCreature.boneObject = boneObject;
    newCreature.muscleObject = muscleObject;

    return newCreature;
  }

  void StartPopulation(int populationSize)
  {
    for (int populationIndex = 0; populationIndex < populationSize;
         populationIndex++)
    {
      creatures.Add(CreateSpecies());
    }
  }
  void CheckPopulation(int liveTime)
  {
    if (timer < liveTime)
    {
      timer += Time.deltaTime;
    }

    else
    {
      // Find the greatest and worst fitness;
      greatestFitness = Mathf.NegativeInfinity;
      worstFitness = Mathf.Infinity;
      float totalFitness = 0;
      foreach (DNA genome in collectedGenomes) { genome.Clear(); }
      collectedGenomes.Clear();
      currentProbability.Clear();

      // Looping through all the creatures in the population
      for (int i = 0; i < populationSize; i++)
      {
        // Having the current fitness
        float currentFitness = creatures[0].GetFitness();

        // Updates the greatest and worst fitness lines
        if (greatestFitness < currentFitness) 
        { 
          greatestFitness = currentFitness; 
        }
        else if (worstFitness > currentFitness) 
        { 
          worstFitness = currentFitness; 
        }
        
        totalFitness += currentFitness;

        // Add the genome to the list of collected genomes
        collectedGenomes.Add(creatures[0].genome);

        // Add the creatures's fitness to the set of fitness
        collectedGenomes[i].SetFitness(currentFitness);

        // Removes the creature from the current list of running creatures
        creatures.Remove(creatures[0]);
        GameObject temp = collectedGameObjects[0];
        collectedGameObjects.Remove(temp);
        Destroy(temp);
      }

      averageFitness = totalFitness / populationSize;

      Debug.Log("Greatest Fitness " + greatestFitness);
      Debug.Log("Average Fitness " + averageFitness);
      Debug.Log("Lowest Fitness " + worstFitness);

      allTimeBestFitness.Add(greatestFitness);
      allTimeAverageFitness.Add(averageFitness);
      allTimeWorstFitness.Add(worstFitness);
      
      // Sorts the collected genomes
      collectedGenomes.Sort();
      collectedGenomes.Reverse();

      // Getting the total value for the scaled fitness
      float totalScaledFitness = 0;

      // Finds the probabilites for the different genomes
      foreach(DNA genome in collectedGenomes)
      {
        // Getting the scaled difference between the current genome and the 
        // lowest genome
        float diff = Mathf.Exp(genome.GetFitness() - 
                     collectedGenomes[populationSize - 1].GetFitness());
        currentProbability.Add(diff);
        totalScaledFitness += diff;
      }

      // Divides each value of the probabilites by the total value
      for (int genomeIndex = 0; genomeIndex < populationSize; genomeIndex++)
      {
        currentProbability[genomeIndex] /= totalScaledFitness;
      }

      GeneticRepopulation();
      generation++;

      graphs[0].CreateGraph(allTimeBestFitness, Color.blue, 0.1f);
      graphs[1].CreateGraph(allTimeAverageFitness, Color.red, 0.1f);
      graphs[2].CreateGraph(allTimeWorstFitness, Color.white, 0.1f);
    }
  }

  int ChooseGenomeFromProbability()
  {
    int chosenIndex = 0;
    float currentValue = Random.value;

    for (int index = 0; index < populationSize; index++)
    {
      currentValue -= currentProbability[index];

      if (currentValue <= 0)
      {
        Debug.Log("Chosen Index: " + index);
        break;
      }
    }

    if (currentValue > 0)
    {
      Debug.Log("Auto Choosing with: " + currentValue);
    }

    return chosenIndex;
  }
  void GeneticRepopulation()
  {
    for (int i = 0; i < collectedGenomes.Count; i++)
    {
      DNA firstGenome = collectedGenomes[ChooseGenomeFromProbability()];
      DNA secondGenome = collectedGenomes[ChooseGenomeFromProbability()];

      DNA newGene = DNA.Crossover(firstGenome, secondGenome);
      creatures.Add(CreateSpecies(newGene));
    }

    for (int i = 0; i < collectedGenomes.Count; i++)
    {
      creatures[i].genome.Mutate(mutateRate);
    }


    timer = 0;
  }

  private void Update()
  {
    CheckPopulation(30);
  }
}
