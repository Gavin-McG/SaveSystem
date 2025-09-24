# **Save System**

This documentation describes how to install, set up, and extend the Save System package. It is designed to guide developers from first-time setup through advanced customization. The Save system is useful for saving and restoring game states between scenes or between sessions, allowing the developer to customize which data is saved and restored.

The package is designed for Unity 6000.2+, but would likely work on older versions as well.

# **Installation**

The Save System is distributed as a Unity package. You can install it in one of two ways using the **Unity Package Manager**:

Repo: [https://github.com/Gavin-McG/SaveSystem.git](https://github.com/Gavin-McG/SaveSystem.git)

### **Option 1: Install from Local Zip**

1. Download the package contents from the repo as a .zip file.  
2. In Unity, go to **Window \> Package Manager**.  
3. Click the **\+** icon and choose **Install package from disk...**.  
4. Select the .zip file.

### **Option 2: Install from Git Repository**

*This method makes it easier to pull updates.*

1. In Unity, go to **Window \> Package Manager**.  
2. Click the **\+** icon and choose **Add package from Git URL...**.  
3. Enter the repository HTTPS URL:

# **Included Samples**

The Dialogue System package ships with a **Sample** for usage examples that can be imported through the Unity Package Manager:

### **1\. Example Usage**

This sample provides a set of Scripts and assets for demonstrating how the system can be used:

* RigidbodySaver \- Example script for saving and restoring a rigidbody data  
* ObjectManagerSaver \- Example script for a manager of an object set  
* Autosaver \- Example script allowing periodic autosaving  
* ExampleScene \- URP scene with example scripts being used  
* ExampleSetting/ExampleFile \- Save System assets used for ExampleScene

Importing this sample is recommended if you want to quickly learn how the save system can be used and applied or learning the Save system Editor tools (Save Window).

# **Save Manager**

The save Manager is the primary Component of the Save System. Every scene where the save system should be used and applied should always contain a SaveManager. In fact, it’s recommended to use a SaveManager as a DontDestoryOnLoad object so that data loaded into the manager can persist between scenes without needing to reload from file.

## **Inspector Fields**

* **Is Primary** \- The SaveManager contains a Singleton structure. The SaveManager marked with this toggle will be accessible via the SaveManager.Instance within your script.  
  * Only one SaveManager should ever be marked with Is Primary.  
  * Any other SaveManagers (e.g. for settings) should be used via direct reference.  
* Manager ID \- Used to identify which objects should be saved and loaded by this manager.  
  * It’s recommended to keep blank for your main/default Save Manager.  
  * ISaveData\<T\> objects can define a “managerID” property to target a specific managerID.  
* Settings \- SaveSystemSettings object for the manager to use.

## **Primary Methods**

* SaveData() \- Triggers all the data of all ISaveData\<T\> objects in the scene to be saved onto the Save file assigned to the manager’s settings in addition to all data loaded into the manager.  
  * Updates the data saved internally within the SaveManager.  
  * Individual object’s data identified by ISaveData\<T\>’s “identifier” property. No two objects in the same scene should contain the same identifier.  
  * To avoid losing data for identifiers not present in a scene when saving, you should run a load without restoring prior to saving.

* LoadData(bool restore=true) \- Loads the data from the save file into the SaveManager internally.  
  * Removes data previously loaded into the SaveManager  
  * “restore” parameter determines whether to restore data immediately after loading.

* RestoreData() \- Uses the data loaded into the Save Manager to instruct ISaveData\<T\> objects to restore their state  
  * If no save data for a given object’s identifier is available then it will Restore to Default

## **Other Methods**

* RestoreToDefault() \- instructs all ISaveData\<T\> objects to restore themselves to their default state.  
  * Does not affect or use SaveManager’s internally stored data.

* ClearData(bool restore=true) \- Clears the data stored by SaveManager.  
  * “restore” parameter determines whether to restore to default immediately after clearing.

* TryGetSaveData\<T\>(string identifier, out T data) \- manually query for the data of an identifier  
  * outputs bool based on success of data retrieval.  
  * queries from SaveManager’s stored data.  
  * Will return false if either no such identifier exists within the data or the type T is incorrect.

## **Tools**

The SaveManager inspector comes with several tools that are meant for the developer’s convenience while testing.

* The 4 “Save Tools” buttons are exactly what they describe, making calls to the previously described Primary Methods. All buttons except “Save Data” require to be in play mode.  
  * Save Data \- calls SaveData()  
  * Load Data \- calls LoadData(restore: false)  
  * Restore Data \- calls RestoreData()  
  * Load and Restore Data \- calls LoadData(restore: true)  
* The 2 “Save File Tools” are for clearing the data from the save file completely. Using these tools will not clear the data from the SaveManager, only the file. To clear data from SaveManager, load immediately after clearing.  
  * Clear All Save Data \- removes temp and non-temp save file  
  * Clear Temp Save Data \- removed only the temporary save file, leaving the main file untouched  
    * Attempting to Load in the same session after the Temp file has been deleted will create a new temp copy.

# **Save System Settings**

ScriptableObject for storing the settings for a SaveManager to use, including the Save File.

Create → Save System → Settings

## **Inspector Fields**

* Save File \- Save file for any SaveManager with these settings to interact with.  
* Use Temporary Save \- Whether to use a temp save when running in the editor.  
  * SaveManager creates copy of save on Start to use.  
  * Saves and Loads use the temporary copy file instead for the duration of the session.  
  * Useful to ensure the original save isn’t affected when testing.  
* Show Logs \- Whether to log operations run by the manager.  
* Show Warnings \- Whether to log Warnings coming from operations.

## **Usage**

Other than its basic required usage of configuring your SaveManager, you can also modify the save file you want to use through SaveSystemSettings.saveFile. This is useful for if you want to have multiple save files, selecting a certain file can assign that file to your settings object.

# **Save File**

ScriptableObject representing a Unique save file.

Create → Save System → Save File

## **Inspector Fields**

* File Name \- Unique name that the File will use in its file path  
  * Two save files with identical File Names will just overwrite the same file.  
  * Do not create a file ending in “-temp” as this could interfere with the temporary file of the same name (e.g. myFile and myFile-temp would interfere)

## **Methods**

* ClearSave() \- Clears all save data associated with this file  
  * Data stored in any SaveManager will not be affected

## **Tools**

Contains the same button tools to clear save data as SaveManger.

# **ISaveData\<T\>**

ISaveData\<T\> is the interface that your MonoBehaviors should inherit from if you want them to be able to save data. The type parameter T is the type of data that you want your MonoBehavior to save to and restore from.

## **Required Definitions**

* string Identifier → Property corresponding to the unique identifier of the object that the save system uses to identify what data should belong to which objects.  
* T GetSaveData() → Method used by the SaveManager to retrieve data from your Monobehaviors.  
  * Called on SaveManager.SaveData()  
* void RestoreFromSaveData(T data) → Method to get your object to set its state from saved data.  
  * Called on SaveManager.RestoreData()  
* void RestoreToDefault() → Method to get your object to restore itself to its default state.  
  * If you only intent to Restore data on scene loads then this method can be empty, assuming your scene/components are configured to its intended default state already.  
  * Called on SaveManager.RestoreData() when do data is present for its identifier.

## **Optional Definitions**

* string ManagerID → optional identifier for what SaveManager to correspond to.  
  * Will only interact with SaveManager of matching managerID.  
  * Default value is “”, hence SaveManager.managerID recommended to be empty for main your primary SaveManager.

# **UnityEngine.Objects**

The Save system is not able to serialize any references to UnityEngine.Objects. This includes any GaemObjects, ScriptableObjects, textures, etc. Any attempt to include these types as or within your chosen data type T will not function as intended. In such cases it is recommended that you resolve these references using an ID/Database system. Here’s a great example on how you might set that up:

[https://www.youtube.com/watch?v=3dRTFgm9-Tc](https://www.youtube.com/watch?v=3dRTFgm9-Tc)

Note: a non-generic ISaveData interface also exists within the package. It has its own set of functions which use System.Object rather than a specified type. You could theoretically use this for objects which might save/load from multiple different types dynamically, but its generally not recommended. Its mostly meant as the base class of the generic ISaveData\<T\> to reduce the need for reflection within the System’s internals.

# **Save Window**

The Save Window is the developer’s all-in-one interface to interact with the Save System. It provides direct access to the fields of any SaveManager and its assigned settings as well as the same tools all without needed to dig through components and gameObjects to open it up.

By default the targeted SaveManager is whatever SaveManager is marked with the IsPrimary bool. However, you can manually select which SaveManager to edit through the “Override Manager” field.

To open the Window to to Window → WolverineSoft → Save System

# **Developer Notes**

Author: Gavin McGinness

Some ways that this package could be improved upon in the future:

* New Sample or addition to existing sample demonstrating the Save System being used for scene transitions. Could also include demonstration on using multiple SaveManagers for different data.  
* Currently the package has a Singleton Monobehavior class for the SaveManager that is only used once. This script could potentially be moved into its own Singleton package and be added as a dependency for use elsewhere within a project for developer convenience.  
* I’m not currently too happy with the ManagerID system for multiple SaveManagers. Using strings properties which might very well be hidden within scripts definitions could lead to debugging issues. Perhaps this system could be later improved some form.  
  * The same applies to ISaveData\<T\> identifiers, although to a lesser extent as these only need to remain consistent with itself rather than match a SaveManager.  
* As mentioned in the ISaveData\<T\> section, The system can not serialize UnityEngine.Object references. The documentation currently advises a database system for storing and resolving these references, although perhaps someone could later figure out a way to configure NewtonSoft or some other serialization tool to resolve these references.

