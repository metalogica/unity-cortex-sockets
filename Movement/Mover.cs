using System;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
  [SerializeField] Transform target;

  void Start() {
    if (target)
    {
      GetComponent<NavMeshAgent>().destination = target.position;
    }
  }

  void Update() {
    UpdateAnimation();
  }

  void UpdateAnimation() {
    Vector3 globalVelocity = GetComponent<NavMeshAgent>().velocity;
    Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
    float speed = Math.Abs(globalVelocity.z);

    GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
  }

  public void MoveTo(Vector3 destination)
  {
    GetComponent<NavMeshAgent>().destination = destination;
  }
}
