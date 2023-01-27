# Scriptable Player Prefs
Simple utility that uses scriptable objects to save and load data using player prefs, you can view, modify and clear the data saved on inspector and reference them from any script.
## Installation
Use the package manager to add the following git URL:
>`https://github.com/lucasgaspar/scriptable-player-prefs-unity.git`
## Usage
Create a new Save Data from the Project window:  
`Create` -> `Scriptable Player Pref`.  
The asset will use his GUID as the key to save the data, renaming it or moving it to another folder will not change the key used to save the information.

On a script you must first include the library:  
```csharp
using ScriptablePlayerPrefs;
```  
Expose the field to the inspector to assign the save data asset:  
```csharp
[SerializeField] private ScriptablePlayerPref _saveData = null;
```  
To check if there is any data saved:  
```csharp
_saveData.HasData();
```  
To load:  
```csharp
User user = _saveData.Get<User>();
```  
To load with a default value:
```csharp
User user = _saveData.Get(new User());
```  
To save:  
```csharp
_saveData.Set(user);
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
using ScriptablePlayerPrefs;
//The default unity library
using UnityEngine;

public class CountElapsedTime : MonoBehaviour
{
    //An invalid time
    private const float InvalidTime = -1f;
    //The reference of the save data asset
    [SerializeField] private ScriptablePlayerPref _saveData = null;
    //A time counter
    [SerializeField] private float _elapsedTime = InvalidTime;

    private void Start()
    {
        //Checks if there is any saved data
        if (_saveData.HasData())
        {
            //Load the data, if it is not possible to load it will return InvalidTime
            _elapsedTime = _saveData.Load(InvalidTime);
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
        _saveData.Save(_elapsedTime);
    }
}
```
