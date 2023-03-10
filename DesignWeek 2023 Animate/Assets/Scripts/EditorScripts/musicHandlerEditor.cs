using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(musicHandler))]
public class musicHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        musicHandler handler = (musicHandler)target;

        DrawDefaultInspector();

        if (handler.musicTracks.Count > 0)
        {
            GUILayout.Label("---------------------------------------------------------------------------------------------");

            GUILayout.Space(10f);

            
            GUILayout.Label("Now Playing: " + handler.musicTracks[handler.currentSongIndex].name, EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();



            string currentSongTime = TimeSpan.FromMinutes(handler.musicSource.time).ToString().Remove(5);
            string maxSongTime = TimeSpan.FromMinutes(handler.musicTracks[handler.currentSongIndex].length).ToString().Remove(5);

            GUILayout.Label(currentSongTime +
                "/" + maxSongTime); ;
            GUILayout.HorizontalSlider(handler.musicSource.time, 0f, handler.musicTracks[handler.currentSongIndex].length);
            GUILayout.Space(24f);
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
            if (GUILayout.Button("Play Next Song"))
            {
                handler.StopCoroutine(handler.playBGMusic());
                AudioClip temp = handler.musicTracks[handler.currentSongIndex];
                handler.musicTracks.RemoveAt(handler.currentSongIndex);
                handler.musicTracks.Add(temp);
                handler.StartCoroutine(handler.playBGMusic());
            }
        }
        Repaint();

    }
}
