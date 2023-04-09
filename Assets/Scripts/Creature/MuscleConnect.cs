using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleConnect : MonoBehaviour
{

  private GameObject bone1, bone2;
  private Rigidbody2D bone1rdb2d, bone2rdb2d;
  public float maxStrength, power;

  // The timer that moves the muscle each time
  private float currentTimer, resetTimer = 0.1f;

  public void SetBones(GameObject bone1, GameObject bone2)
  {
    this.bone1 = bone1;
    this.bone2 = bone2;
    bone1rdb2d = bone1.GetComponent<Rigidbody2D>();
    bone2rdb2d = bone2.GetComponent<Rigidbody2D>();
  }

  void ChangeShape(float strength)
  {
    Vector2 direction = (bone2.transform.position - bone1.transform.position).normalized;
    float force = (Mathf.Abs(strength) > maxStrength) ? maxStrength * Mathf.Sign(strength) : strength;
    bone1rdb2d.AddForce(force * direction);
    bone2rdb2d.AddForce(force * direction * -1);
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    currentTimer -= Time.fixedDeltaTime;

    if (bone1)
    {
      Debug.DrawLine(bone1.transform.position, bone2.transform.position,
                     Color.red);

      if (currentTimer < 0)
      {
        ChangeShape(power * 20);
        currentTimer = resetTimer;
      }
      
    }
  }
}
