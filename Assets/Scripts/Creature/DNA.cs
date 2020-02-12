using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : System.IComparable<DNA>
{

  public NeuralNetwork brain;
  public Matrix[] synapseBP, biasesBP;
  public Transform[] structurePosition;
  public int[] connections;
  public readonly int jointNumber;
  public readonly int boneNumber;
  public readonly int muscleNumber;

  // The current fitness of this DNA
  private float fitness;

  public DNA(NeuralNetwork brain, Transform[] structurePosition, int[] connections, int jointNumber, int boneNumber, int muscleNumber)
  {
    this.brain = brain;
    synapseBP = brain.synapses;
    biasesBP = brain.biases;
    this.muscleNumber = muscleNumber;
    this.boneNumber = boneNumber;
    this.jointNumber = jointNumber;
    this.structurePosition = structurePosition;
    this.connections = connections;
  }

  private static T[] PickFromTwo<T>(T[] a, T[] b)
  {
    T[] newArray = new T[a.Length];

    for (int i = 0; i < newArray.Length; i++)
    {
      newArray[i] = (Random.value > 0.5f) ? a[i] : b[i];
    }
    return newArray;
  }

  private static Transform[] TransformCopy(Transform[] transform)
  {
    Transform[] newTransform = new Transform[transform.Length];

    for (int transformIndex = 0; transformIndex < transform.Length; 
         transformIndex++)
    {
      newTransform[transformIndex] = 
                          MonoBehaviour.Instantiate(transform[transformIndex]);
    }

    return newTransform;
  }

  public static DNA Crossover(DNA genome, DNA otherGenome)
  {
    Matrix[] newSynpases = Matrix.PickFromTwoMatrix(genome.synapseBP, otherGenome.synapseBP);
    Matrix[] newBiases = Matrix.PickFromTwoMatrix(genome.biasesBP, otherGenome.biasesBP);

    Transform[] newPositions = PickFromTwo(genome.structurePosition, 
                                           otherGenome.structurePosition);

    int[] newConnections = PickFromTwo(genome.connections,
                                       otherGenome.connections);

    NeuralNetwork newBrain = new NeuralNetwork(genome.brain.inputs, genome.brain.outputs, genome.brain.numberOfHiddenLayers, genome.brain.numberOfHiddenNodes)
    {
      synapses = newSynpases,
      biases = newBiases
    };

    return new DNA(newBrain, TransformCopy(newPositions), newConnections, 
                   genome.jointNumber, genome.boneNumber, genome.muscleNumber);

  }

  public void Mutate(float mutationRate)
  {
    biasesBP = Matrix.MutateArray(biasesBP, mutationRate / 4);
    synapseBP = Matrix.MutateArray(synapseBP, mutationRate / 4);


    foreach (Transform placement in structurePosition)
    {
      Vector2 position = placement.position;
      if (Random.value < mutationRate) 
      { 
        placement.position = position + Random.insideUnitCircle * 0.1f;
      }
    }


    for (int i = jointNumber * 2; i < boneNumber * 2; i++)
    {
      if (Random.value < mutationRate / 4)
      {
        if (i % 2 == 0)
        {
          do { connections[i] = Mathf.FloorToInt(Random.Range(0, jointNumber)); }
          while (connections[i] == connections[i + 1]);
        }
        else
        {
          do { connections[i] = Mathf.FloorToInt(Random.Range(-1, jointNumber)); }
          while (connections[i] == connections[i - 1]);
        }
      }
    }

    for (int i = boneNumber * 2; i < connections.Length; i++)
    {
      connections[i] = (Random.value < mutationRate / 4) ? Mathf.FloorToInt(Random.Range(0, boneNumber)) : connections[i];
    }
  }

  // Being able to compare between two DNA copies
  public int Compare(object firstDNA, object secondDNA)
  {
    return ((DNA)firstDNA).CompareTo((DNA)secondDNA);
  }

  public int CompareTo(DNA other)
  {
    return fitness.CompareTo(other.fitness);
  }

  public override bool Equals(object obj)
  {
    if (obj == typeof(DNA))
    {
      return fitness.CompareTo((DNA)obj) == 0;
    }
    else
    {
      return base.Equals(obj);
    }
  }

  

  // Getting the fitness value
  public float GetFitness()
  {
    return fitness;
  }

  // Setting the fitness for the dna
  public void SetFitness(float fitness)
  {
    this.fitness = fitness;
  }

  // Removes all the transforms connected to the DNA object
  public void Clear()
  {
    for (int index = 0; index < structurePosition.Length; index++)
    {
      MonoBehaviour.Destroy(structurePosition[index].gameObject);
    }
  }

  public DNA Copy
  {
    get
    {
      DNA copy = new DNA(brain.Copy(), TransformCopy(structurePosition), 
                         (int[])connections.Clone(), jointNumber, boneNumber, muscleNumber);
      return copy;
    }
  }
}
