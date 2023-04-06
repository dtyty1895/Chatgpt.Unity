using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Text;
using System.Net;
using TMPro;
using UnityEngine.Windows;
using System.Collections.Generic;

public class VITS : MonoBehaviour
{
    [SerializeField] private AudioClip m_AudioClip;
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] string myHost = "http://10.13.200.96";
    private void Awake()
    {
        m_AudioSource = gameObject.AddComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;

    }
    public IEnumerator Input(string message)
    {

        //UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/data", message);
        UnityWebRequest www = UnityWebRequest.Post($"{myHost}/data", message);
        yield return www.SendWebRequest();
        print("333");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Data sent successfully!");
            StartCoroutine(Download());
        }

    }
    public IEnumerator Download()
    {
        m_AudioSource = GetComponent<AudioSource>();

        //UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/output.wav");
        UnityWebRequest www = UnityWebRequest.Get($"{myHost}/output.wav");
        yield return www.SendWebRequest();
        print("444");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string filePath = Application.dataPath + "/sample.wav";
            System.IO.File.WriteAllBytes(filePath, www.downloadHandler.data);
            Debug.Log("File downloaded!");
        }
        UnityEditor.AssetDatabase.Refresh();
        m_AudioSource.clip = m_AudioClip;
        m_AudioSource.Play();
        print("Play");
    }

}
