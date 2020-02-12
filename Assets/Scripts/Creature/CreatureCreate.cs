using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreate : MonoBehaviour
{

  public GameObject[][] creatureSystem;
  public GameObject jointObject, boneObject, muscleObject;
  public NeuralNetwork brain;
  public DNA genome;

  private Transform[] structurePosition;
  private GameObject[] joints, bones, muscles;
  private MuscleConnect[] connectDirectory;
  private Vector3 averagePosition;

  private int numberOfSelectedTransforms, numberOfSelectedConnections, delay;
  private int[] connections;
  private float startPositionX, timer;
  private float[] distances, muscleConnections;
  private bool isRunning;

  GameObject[] CreateRandomObjects(GameObject[] gameObjectSet, GameObject gameObjectType)
  {
    Vector2 transformPosition = transform.position;
    Vector3 scale = gameObjectType.transform.lossyScale;

    for (int i = 0; i < gameObjectSet.Length; i++)
    {
      gameObjectSet[i] = Instantiate(gameObjectType, transformPosition + Random.insideUnitCircle, transform.rotation, transform);
      gameObjectSet[i].transform.localScale = scale;
      GameObject temp = new GameObject();
      temp.transform.position = gameObjectSet[i].transform.position;
      temp.transform.rotation = gameObjectSet[i].transform.rotation;
      structurePosition[numberOfSelectedTransforms++] = temp.transform;
    }
    return gameObjectSet;
  }
  GameObject[] CreateSpecificObject(int setSize, GameObject gameObjectType)
  {
    GameObject[] newArray = new GameObject[setSize];
    for (int i = 0; i < setSize; i++)
    {
      Vector3 position = genome.structurePosition[numberOfSelectedTransforms].position;
      Quaternion rotation = genome.structurePosition[numberOfSelectedTransforms].rotation;

      newArray[i] = Instantiate(gameObjectType, position, rotation, transform);
      numberOfSelectedTransforms++;
    }

    return newArray;
  }

  private void Awake()
  {
    delay = 1;
    timer = 0;
  }

  void Begin()
  {
    joints = new GameObject[5];
    bones = new GameObject[10];
    muscles = new GameObject[8];

    brain = new NeuralNetwork(bones.Length + muscles.Length * 2, muscles.Length, 1, 60);
    connectDirectory = new MuscleConnect[muscles.Length];

    structurePosition = new Transform[joints.Length + bones.Length + muscles.Length];
    connections = new int[bones.Length * 2 + muscles.Length * 2];

    if (genome == null)
    {
      joints = CreateRandomObjects(joints, jointObject);
      bones = CreateRandomObjects(bones, boneObject);
      muscles = CreateRandomObjects(muscles, muscleObject);

      creatureSystem = new GameObject[][] { joints, bones, muscles };

      int doubleConnection = 0;

      foreach (GameObject bone in bones)
      {
        HingeJoint2D boneJoint = bone.GetComponent<HingeJoint2D>();

        int firstJoint = (doubleConnection < joints.Length) ? doubleConnection : Mathf.FloorToInt(Random.Range(0, joints.Length));
        connections[numberOfSelectedConnections++] = firstJoint;

        boneJoint.connectedBody = joints[firstJoint].GetComponent<Rigidbody2D>();

        if (doubleConnection < joints.Length || Random.value > 0.5f)
        {
          doubleConnection++;
          HingeJoint2D boneJoint2 = bone.AddComponent<HingeJoint2D>();

          int secondJoint = (doubleConnection < joints.Length) ? doubleConnection : Mathf.FloorToInt(Random.Range(0, joints.Length));
          while (firstJoint == secondJoint) { secondJoint = Mathf.FloorToInt(Random.Range(0, joints.Length)); }

          connections[numberOfSelectedConnections++] = secondJoint;
          boneJoint2.connectedBody = joints[secondJoint].GetComponent<Rigidbody2D>();
          boneJoint2.autoConfigureConnectedAnchor = false;

          boneJoint2.anchor = new Vector2(0.5f, 0);
          boneJoint2.connectedAnchor = Vector2.zero;
        }
        else
        {
          connections[numberOfSelectedConnections++] = -1;
        }
      }
      int directoryCounter = 0;
      foreach (GameObject muscle in muscles)
      {
        MuscleConnect muscleConnection = muscle.GetComponent<MuscleConnect>();
        int chosenBone1 = Mathf.FloorToInt(Random.Range(0, bones.Length));
        int chosenBone2;

        do
        {
          chosenBone2 = Mathf.FloorToInt(Random.Range(0, bones.Length));
        }
        while (chosenBone1 == chosenBone2);

        muscleConnection.SetBones(bones[chosenBone1], bones[chosenBone2]);

        connections[numberOfSelectedConnections++] = chosenBone1;
        connections[numberOfSelectedConnections++] = chosenBone2;
        connectDirectory[directoryCounter++] = muscleConnection;
      }
      genome = new DNA(brain, structurePosition, connections, joints.Length, bones.Length, muscles.Length);
    }
    else
    {

      brain = genome.brain;

      joints = CreateSpecificObject(joints.Length, jointObject);
      bones = CreateSpecificObject(bones.Length, boneObject);
      muscles = CreateSpecificObject(muscles.Length, muscleObject);

      foreach (GameObject bone in bones)
      {
        HingeJoint2D boneJoint = bone.GetComponent<HingeJoint2D>();
        boneJoint.connectedBody = joints[genome.connections[numberOfSelectedConnections++]].GetComponent<Rigidbody2D>();
        if (genome.connections[numberOfSelectedConnections] != -1)
        {
          HingeJoint2D boneJoint2 = bone.AddComponent<HingeJoint2D>();
          boneJoint2.connectedBody = joints[genome.connections[numberOfSelectedConnections]].GetComponent<Rigidbody2D>();
          boneJoint2.autoConfigureConnectedAnchor = false;

          boneJoint2.anchor = new Vector2(0.5f, 0);
          boneJoint2.connectedAnchor = Vector2.zero;
        }
        numberOfSelectedConnections++;
      }

      int directoryCounter = 0;
      foreach (GameObject muscle in muscles)
      {
        MuscleConnect muscleConnection = muscle.GetComponent<MuscleConnect>();
        GameObject bone1 = bones[genome.connections[numberOfSelectedConnections++]];
        GameObject bone2 = bones[genome.connections[numberOfSelectedConnections++]];

        muscleConnection.SetBones(bone1, bone2);
        connectDirectory[directoryCounter++] = muscleConnection;
      }
    }
  }

  public float GetFitness()
  {
    float distance = averagePosition.x;
    return distance;
  }

  Vector3 Recentre()
  {
    Vector3 averagePosition = Vector3.zero;
    foreach (GameObject bone in bones)
    {
      averagePosition += bone.transform.position;
    }
    return averagePosition / bones.Length;
  }

  float[] GetDistancesFromCentre()
  {
    float[] distances = new float[bones.Length];
    for (int i = 0; i < bones.Length; i++)
    {
      distances[i] = (bones[i].transform.position - averagePosition).magnitude;
    }
    return distances;
  }
  float[] GetmuscleConnections()
  {
    float[] muscleConnections = new float[muscles.Length * 2];
    for (int i = 0; i < muscleConnections.Length; i++)
    {
      muscleConnections[i] = genome.connections[i + bones.Length * 2];
    }
    return muscleConnections;
  }

  private void Run()
  {
    Matrix inputs = new Matrix(brain.inputs, 1);
    for (int i = 0; i < brain.inputs; i++)
    {
      inputs[i, 0] = (i >= distances.Length) ? muscleConnections[i - distances.Length] : distances[i];
    }
    Matrix outputs = brain.ForwardRun(inputs);
    for (int i = 0; i < connectDirectory.Length; i++)
    {
      connectDirectory[i].power = outputs[i, 0];
    }
  }

  private void Update()
  {
    if (isRunning)
    {
      Run();
      if (timer < delay && delay > 0)
      {
        timer++;
      }
      else if (timer > delay)
      {
        startPositionX = Recentre().x;
      }
    }
    if (!isRunning && jointObject)
    {
      Begin();
      muscleConnections = GetmuscleConnections();

      isRunning = true;
    }
  }
  private void LateUpdate()
  {
    if (isRunning)
    {
      averagePosition = Recentre();
      distances = GetDistancesFromCentre();
    }

  }

  private void OnDrawGizmos()
  {
    Vector2 beginPoint = new Vector2(startPositionX, averagePosition.y);
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(beginPoint, 0.1f);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(averagePosition, 0.1f);
    Gizmos.DrawLine(beginPoint, averagePosition);
  }
}
