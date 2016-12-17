using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gesture {
  public int sampleLength;

  private List<Vector3> gesturePoints;

  public Gesture(List<Vector3> initialGesturePoints) {
    gesturePoints = ResampleGestureToLength(
      initialGesturePoints,
      sampleLength
    );
  }

  public Gesture(List<Vector3> initialGesturePoints, int sampleLength) {
    this.sampleLength = sampleLength;
    gesturePoints = ResampleGestureToLength(
      initialGesturePoints,
      sampleLength
    );
  }

  public List<Vector3> Points() {
    return gesturePoints;
  }

  public bool Match(List<Vector3> otherGesturePoints) {
    otherGesturePoints = ResampleGestureToLength(
      otherGesturePoints,
      sampleLength
    );
    float error = 0f;
    float totalMeasurement = 0f;
    // this is naive
    for (int i = 0; i < gesturePoints.Count; i++) {
      error += Mathf.Abs(gesturePoints[i].x - otherGesturePoints[i].x);
      error += Mathf.Abs(gesturePoints[i].y - otherGesturePoints[i].y);
      error += Mathf.Abs(gesturePoints[i].z - otherGesturePoints[i].z);
      totalMeasurement += Mathf.Abs(gesturePoints[i].x);
      totalMeasurement += Mathf.Abs(gesturePoints[i].y);
      totalMeasurement += Mathf.Abs(gesturePoints[i].z);
    }
    return error / totalMeasurement < 0.5f;
  }

  public float PathLength(List<Vector3> gesturePointsToMeasure) {
    float pathLength = 0f;

    for (int i = 1; i < gesturePointsToMeasure.Count; i++) {
      pathLength += (gesturePointsToMeasure[i] - gesturePointsToMeasure[i - 1]).magnitude;
    }
    return pathLength;
  }

  private List<Vector3> ResampleGestureToLength(List<Vector3> gesturePoints, int newLength) {
    float pathLength = PathLength(gesturePoints);
    float interval = pathLength / (newLength - 1);
    float distance = 0f;

    List<Vector3> newGesturePoints = new List<Vector3>();
    newGesturePoints.Add(gesturePoints[0]);

    for (int i = 1; i < gesturePoints.Count; i++) {
      if (i > 1000) {
        Debug.Log("Breaking out of an infinite loop");
        break;
      }
      Vector3 lastSampledPoint = gesturePoints[i - 1];
      Vector3 move = gesturePoints[i] - lastSampledPoint;

      if (distance + move.magnitude >= interval) {
        Vector3 newMove = move * ((interval - distance) / move.magnitude);
        Vector3 newGesturePoint = lastSampledPoint + newMove;
        gesturePoints.Insert(i, newGesturePoint);
        newGesturePoints.Add(newGesturePoint);
        distance = 0f;
      }
      else {
        distance += (gesturePoints[i] - lastSampledPoint).magnitude;
      }
    }
    return newGesturePoints;
  }
}