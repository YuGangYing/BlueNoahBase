using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using GameEngine.IO;

//Local Auto deploy.
public class WebGLAutoDeploy  {

	//Change the path to your deploy path.
	const string LOCAL_DEPLOY_PATH = "/Applications/XAMPP/xamppfiles/htdocs/";

	static bool isAutoDeploy = true;

	[PostProcessBuild (101)]
	public static void OnPostProcessBuild (BuildTarget target, string path)
	{
		if (target != BuildTarget.WebGL)
			return;
		Debug.Log ("BuildPath : " + path);
		if(isAutoDeploy){
			string fileName = FileManager.GetFileInfo (path).Name;
			if (FileManager.DirectoryExists (LOCAL_DEPLOY_PATH)) {
				string targetDirectory = LOCAL_DEPLOY_PATH + fileName;
				FileManager.DeleteDirectory (targetDirectory);
				FileManager.CopyDirectory (path,targetDirectory,true);
			}
		}
	}
}