# Scriptable Player Prefs
Simple utility that uses scriptable objects to save and load data using player prefs, you can view, modify and clear the data saved on inspector and reference them from any script.
## Installation
Use the package manager to add the following git URL:
>`https://github.com/lucasgaspar/scriptable-player-prefs-unity.git`
## Usage
Create a new Save Data from the Project window:  
`Create` -> `Scriptable Player Prefs` -> `Scriptable Player Pref`.  

On a script you must first include the library:  
```csharp
using LG.ScriptablePlayerPrefs;
```  
Expose the field to the inspector to assign the save data asset:  
```csharp
[SerializeField] private ScriptablePlayerPref _saveData = null;
```  
To check if there is any data saved:  
```csharp
_saveData.HasData();
```  
To load you must determine what type the data is and a default value if it is not possible to convert to it:  
```csharp
_saveData.Load<User>(new User());
```  
To save:  
```csharp
_saveData.Save<User>(user);
```  
To clear data:  
```csharp
_saveData.Clear();
```  
## Supported data types
- string
- int
- float
- long
- bool
- Any serializable class  
## Example
This basic script counts and saves the elapsed seconds while unity is on play mode:
```csharp
//The library of this plugin
using LG.ScriptablePlayerPrefs;
//The default unity library
using UnityEngine;

public class CountElapsedTime : MonoBehaviour
{
    //An invalid time
    private const float InvalidTime = -1f;
    //The save data asset
    [SerializeField] private ScriptablePlayerPref _saveData = null;
    //A time counter
    [SerializeField] private float _elapsedTime = InvalidTime;

    private void Start()
    {
        //Checks if there is any saved data
        if (_saveData.HasData())
        {
            //Load the data and if it was not possible to load will return InvalidTime
            _elapsedTime = _saveData.Load<float>(InvalidTime);
        }

        //If loaded time is invalid
        if (_elapsedTime == InvalidTime)
        {
            //Clear the data
            _saveData.Clear();
            //Then resets the time to zero
            _elapsedTime = default;
        }
    }

    private void Update()
    {
        //Add this frame delta time to the elapsed time
        _elapsedTime += Time.deltaTime;
    }

    private void OnDestroy()
    {
        //Saves the elapsed time
        _saveData.Save<float>(_elapsedTime);
    }
}
```  
Create new save data asset by right clicking the Project window:  
<img width="674" alt="Create new save data asset" src="https://user-images.githubusercontent.com/7684147/206793500-458788ff-3bca-4e29-8f38-477b314e9a4d.png">  
Assign save data asset:  
![Assign save data](https://user-images.githubusercontent.com/7684147/206793813-78d3ae6b-db11-44ed-bfd4-ea762bd92e2a.gif)  
The data is saved when the application stops and you can change the value trough the inspector:  
![Save and modify data](https://user-images.githubusercontent.com/7684147/206793908-f4751bd1-71c7-4bea-a91f-a5596abc36f3.gif)  
And you can clear the data any time  
![Clear data](https://user-images.githubusercontent.com/7684147/206793975-a9130d21-28ff-44cc-b62a-704fff308067.gif)
