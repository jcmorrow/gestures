using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class GestureTest {

  [Test]
  public void Gesture_Match_ReturnsTrue() {
    var gesturePoints = new List<Vector3>(new Vector3[] { new Vector3(1f, 1f, 1f) });
    var secondGesturePoints = new List<Vector3>(new Vector3[] { new Vector3(1f, 1f, 1f),
                                                                  new Vector3(1f, 1f, 1f) });
    Gesture gesture = new Gesture(gesturePoints, 1);

    bool match = gesture.Match(secondGesturePoints);

    Assert.True(match);
  }

  [Test]
  public void Gesture_Match_ReturnsFalse() {
    var gesturePoints = new List<Vector3>(new Vector3[] { new Vector3(1f, 1f, 1f) });
    var secondGesturePoints = new List<Vector3>(new Vector3[] { new Vector3(2f, 2f, 2f) });
    Gesture gesture = new Gesture(gesturePoints, 1);

    bool match = gesture.Match(secondGesturePoints);

    Assert.False(match);
  }

  [Test]
  public void Gesture_Gesture_ReturnsCorrectCount() {
    var gesturePoints = new List<Vector3>(new Vector3[] {
         new Vector3(1f, 1f, 1f),
         new Vector3(1f, 1f, 1f),
         new Vector3(1f, 1f, 1f),
         new Vector3(2f, 2f, 2f),
         new Vector3(2f, 2f, 2f),
         new Vector3(2f, 2f, 2f),
         new Vector3(3f, 3f, 3f),
         new Vector3(3f, 3f, 3f),
         new Vector3(3f, 3f, 3f),
         new Vector3(4f, 4f, 4f)
      });
    Gesture gesture = new Gesture(gesturePoints, 3);

    List<Vector3> sampledGesturePoints = gesture.Points();

    Assert.AreEqual(3, sampledGesturePoints.Count);
  }

  [Test]
  public void Gesture_PathLength_ReturnsCorrectLength() {
    var gesture = new List<Vector3>(new Vector3[] {
         new Vector3(0f, 0f, 0f),
         new Vector3(1f, 1f, 1f),
      });

    float pathLength = new Gesture(gesture, 2).PathLength(gesture);

    Assert.AreEqual(Mathf.Sqrt(3), pathLength);
  }
}
