using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class GestureRecorder : MonoBehaviour {
  private bool stopRecording;
  private Vector3 gestureStartingPoint;
  private List<Vector3> gesturePoints = new List<Vector3>();
  private Gesture gesture;

  public GameObject marker;
  public int sampleLength;

  void Start() {
    var events = GetComponent<VRTK.VRTK_ControllerEvents>();
    events.TriggerClicked += HandleTriggerClicked;
    events.TriggerUnclicked += HandleTriggerUnclicked;
  }

  void HandleTriggerClicked(object sender, ControllerInteractionEventArgs e) {
    stopRecording = false;
    gesturePoints = new List<Vector3>();
    gestureStartingPoint = transform.position;
    StartCoroutine(RecordGesture(0.01f));
  }

  void HandleTriggerUnclicked(object sender, ControllerInteractionEventArgs e) {
    SetStopRecording();
  }

  void DrawGesture(List<Vector3> points, Color color) {
    foreach (Vector3 vec in points) {
      GameObject newMarker = (GameObject)Instantiate(marker, null);
      newMarker.transform.position = vec + Vector3.up * 1f;
      newMarker.GetComponent<Renderer>().material.color = color;
    }
  }

  void SetStopRecording() {
    stopRecording = true;
  }

  void StopRecording() {
    // string summaryString = "{ \"points\": [";
    // foreach(Vector3 vec in gesture) {
    // summaryString = summaryString + JsonUtility.ToJson(vec) + ",\n ";
    // }
    // summaryString = summaryString + "]}";
    if (gesture == null) {
      gesture = new Gesture(gesturePoints, sampleLength);
      DrawGesture(gesture.Points(), Color.blue);
    }
    else {
      Gesture candidateGesture = new Gesture(gesturePoints, sampleLength);
      if (gesture.Match(candidateGesture.Points())) {
        DrawGesture(candidateGesture.Points(), Color.green);
      }
      else {
        DrawGesture(candidateGesture.Points(), Color.red);
      }
    }
  }

  void RecordGesturePoint(Vector3 position) {
    gesturePoints.Add(position - gestureStartingPoint);
  }

  private IEnumerator RecordGesture(float interval) {
    while (!stopRecording) {
      RecordGesturePoint(transform.position);
      yield return new WaitForSeconds(interval);
    }
    StopRecording();
    yield break;
  }
}