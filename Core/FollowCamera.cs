using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
  [SerializeField] Transform target;

  void Start() {
  }

  // Update is called once per frame
  void LateUpdate()
  {
    if (!target)
    {
      return;
    }

    transform.position = target.position;
  }
}
