using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager;
using UnityEditor.VersionControl;
using UnityEngine;

public static class SaveSystem
{
	public const string TRACK_DATA_DIR = "TrackData";
	public const string DEFAULT_TRACK_LIST = "DefaultTrackList";
	public const string JSON_EXTENSION = ".json";

	public static string GetDirectoryPath(string directory)
	{
		var fullPath = System.IO.Path.Combine(Application.persistentDataPath, directory);

		try
		{
			if (!Directory.Exists(fullPath))
				Directory.CreateDirectory(fullPath);
		}
		catch (Exception e)
		{
			GameManager.GameEventManager.OnGameMessage?.Invoke("Error creating directory: " + fullPath, true);
		}

		return System.IO.Path.Combine(Application.persistentDataPath, directory) + "/";
	}

	public static void SaveData(string directory, string fileName, object data)
	{
		try
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			string json = JsonUtility.ToJson(data);
			string path = GetDirectoryPath(directory) + fileName + JSON_EXTENSION;
			GameManager.GameEventManager.OnGameMessage?.Invoke(path, true);
			File.WriteAllText(path, json);
		}
		catch (Exception e)
		{
			GameManager.GameEventManager.OnGameMessage?.Invoke("Error saving data to directory: " + directory + ", file: " + fileName, true);
		}
	}

	public static List<T> LoadFilesInDirectory<T>(string directory)
	{
		var list = new List<T>();
		var path = GetDirectoryPath(directory);
		var filePaths = Directory.GetFiles(path);
		foreach (var filePath in filePaths)
		{
			try
			{
				var data = File.ReadAllText(filePath);
				list.Add(JsonUtility.FromJson<T>(data));
			}
			catch (Exception e)
			{
				GameManager.GameEventManager.OnGameMessage?.Invoke("Error loading data from file: " + filePath, true);
			}
		}
		return list;
	}

	public static T LoadData<T>(string directory, string fileName)
	{
		string path = GetDirectoryPath(directory) + fileName + JSON_EXTENSION;

		try
		{
			string data = File.ReadAllText(path);
			return JsonUtility.FromJson<T>(data);
		}
		catch (Exception e)
		{
			GameManager.GameEventManager.OnGameMessage?.Invoke("Error loading data from file: " + path, true);
			return default(T);
		}
	}

	public static bool DeleteFile(string directory, string fileName)
	{
		string path = GetDirectoryPath(directory) + fileName + JSON_EXTENSION;

		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
				return true;
			}
			return false;
		}
		catch (Exception e)
		{
			GameManager.GameEventManager.OnGameMessage?.Invoke("Error deleting file: " + path, true);
			return false;
		}
	}
}