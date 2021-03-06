﻿using UnityEngine;
using System.Collections;
using System.IO;

public class Share : MonoBehaviour
{
	
	public  static string imagePath;
	static AndroidJavaClass sharePluginClass;
	static AndroidJavaClass unityPlayer;
	static AndroidJavaObject currActivity;
	private static Share mInstance;
	
	public static Share instance {
		get{ return mInstance;}
	}
	
	void Awake ()
	{
		mInstance = this;
	}
	
	void Start ()
	{
		imagePath = Application.persistentDataPath + "/HKeyGame.png";
		sharePluginClass = new AndroidJavaClass ("com.ari.tool.UnityAndroidTool");
		if (sharePluginClass == null) {
			Debug.Log ("sharePluginClass is null");
		} else {
			Debug.Log ("sharePluginClass is not null");
		}
		unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		currActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
	}
	
	public void CallShare (string handline, string subject, string text, bool image)
	{
		Debug.Log ("share call start : " + imagePath);
		if (image) {
			sharePluginClass.CallStatic ("share", new object[] {
				handline,
				subject,
				text,
				imagePath
			});
		} else {
			sharePluginClass.CallStatic ("share", new object[] {
				handline,
				subject,
				text,
				""
			});
		}
		Debug.Log ("share call end");
	}
	
	public void ScreenShot ()
	{
		StartCoroutine (GetCapture ());
	}
	
	IEnumerator GetCapture ()
	{
		
		yield return new WaitForEndOfFrame ();
		
		int width = Screen.width;
		
		int height = Screen.height;
		
		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);
		
		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0, true);
		
		byte[] imagebytes = tex.EncodeToPNG ();//转化为png图
		
		tex.Compress (false);//对屏幕缓存进行压缩
		
		//		image.mainTexture = tex;//对屏幕缓存进行显示（缩略图）
		
		File.WriteAllBytes (Application.persistentDataPath + "/HKeyGame.png", imagebytes);//存储png图
		
		Debug.Log (Application.persistentDataPath);
	}
}
