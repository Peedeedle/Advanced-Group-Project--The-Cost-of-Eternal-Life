using System.IO; //allows use of files on the OS
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;  // allows use to serialize to binary


public static class SaveSystem { // static class to stop instantiation of multiple save systems

	public static void SavePlayer(PlayerController playerController) {

		BinaryFormatter formatter = new BinaryFormatter(); // allows conversion to a binary format
		string sFilePath = Application.persistentDataPath + "/player.sdat"; // creates a persistant path to the save file meaning it doesn't get moved around the system
	
		FileStream stream = new FileStream(sFilePath, FileMode.Create); // create the save file on the system

		PlayerData playerData = new PlayerData(playerController); // get the player information defined in the PlayerData.cs script

		formatter.Serialize(stream, playerData); // serialize our data into a binary format
		stream.Close(); // close our stream to stop leaks
	}

	public static PlayerData LoadPlayer() {
		string path = Application.persistentDataPath + "/player.sdat"; // keeps a constant path to the save file
		if (File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter(); // allows conversion to a binary format
			FileStream stream = new FileStream(path, FileMode.Open);// opens the save file on the system

			PlayerData data = formatter.Deserialize(stream) as PlayerData;// unserializes the save data
			stream.Close(); // close our data stream to stop leaks

			return data;
		} else {
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}

	public static void SaveSMG(SMG smg) {

		BinaryFormatter formatter = new BinaryFormatter(); // allows conversion to a binary format
		string sFilePath = Application.persistentDataPath + "/SMG.sdat"; // creates a persistant path to the save file meaning it doesn't get moved around the system

		FileStream stream = new FileStream(sFilePath, FileMode.Create); // create the save file on the system

		SMGData smgData = new SMGData(smg); // get the player information defined in the PlayerData.cs script

		formatter.Serialize(stream, smgData); // serialize our data into a binary format
		stream.Close(); // close our stream to stop leaks
	}

	public static SMGData LoadSMG() {
		string path = Application.persistentDataPath + "/SMG.sdat"; // keeps a constant path to the save file
		if (File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter(); // allows conversion to a binary format
			FileStream stream = new FileStream(path, FileMode.Open);// opens the save file on the system

			SMGData data = formatter.Deserialize(stream) as SMGData;// unserializes the save data
			stream.Close(); // close our data stream to stop leaks

			return data;
		} else {
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}

	public static void SaveShotgun(Shotgun shotgun) {

		BinaryFormatter formatter = new BinaryFormatter(); // allows conversion to a binary format
		string sFilePath = Application.persistentDataPath + "/SMG.sdat"; // creates a persistant path to the save file meaning it doesn't get moved around the system

		FileStream stream = new FileStream(sFilePath, FileMode.Create); // create the save file on the system

		ShotgunSaveData shotgunData = new ShotgunSaveData(shotgun); // get the player information defined in the PlayerData.cs script

		formatter.Serialize(stream, shotgunData); // serialize our data into a binary format
		stream.Close(); // close our stream to stop leaks
	}

	public static ShotgunSaveData LoadShotgun() {
		string path = Application.persistentDataPath + "/Shotgun.sdat"; // keeps a constant path to the save file
		if (File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter(); // allows conversion to a binary format
			FileStream stream = new FileStream(path, FileMode.Open);// opens the save file on the system

			ShotgunSaveData data = formatter.Deserialize(stream) as ShotgunSaveData;// unserializes the save data
			stream.Close(); // close our data stream to stop leaks

			return data;
		} else {
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}

	public static PlayerData DeleteFiles() {
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
		foreach (string filePath in filePaths) {
			File.Delete(filePath);
		}
		return null;
	}
}
