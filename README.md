![EcoRiftLogo](Images/NewLogoTestSolo.png)

# EcoRift
 A group project for the Software Development Practice paper at AUT.

Group members:
- Kylie Crump
- Myles Hosken
- Christopher Young
- Jared Scholz

# Description
EcoRift is an Android 2D platform game that relies heavily on real time weather data. The player will navigate through multiple levels and battle enemies. Enemies are comprised of Sun, Rain, Storm and Snow. The in-game weather reflects the retrieved weather data at your location. Enemies are buffed and enhanced depending on their type and current weather state. Players can gain stronger attacks depending on their completed progress of the levels. 

## Technologies
Project is created with :
* Unity 2021.3.8f1
* Visual Studio 2019
* Open Weather API

## Setup
* The project is under review on Google Play as on 23/10/2022. 

* To clone and run on Unity. 
User must include the API key (this is not public on GitHub for exploitation prevention).\
The API key can be found on our Trello board under Sprint 2 Documentation -> API Key.\
The API key must be pasted into the 'APIData.cs' script ( "EcoRift/Assets/Scripts/WeatherAPI/" ). A placeholder is there for you to replace under 'removed'.\
Build and compile the game. \
To play the game functionally, please run the game from the 'MainMenu' scene.\
If there are missing references from the inspector, please contact us!\
Enjoy :)\



* Install .apk locally on Android device.
Please find the .apk file in the repo. This will only work on Android devices/emulators. Once you have the .apk file, feel free to install it to your device. If your device has a security block on unrecognized .apk files, you can bypass this depending on your Android version :\
Settings > Apps > Special app access > Install unknown apps\
Settings > Apps & notifications > Advanced > Special app access > Install unknown apps\
Settings > Apps and notifications\
Settings > Security\
* If the above doesn't work, please follow : https://www.lifewire.com/install-apk-on-android-4177185

## Features
On-screen joystick movement implemented for player control.\
![MovementDemo](Images/movement.png)\
Multiple enemies of different types, flying and ground.\
![Enemies](Images/enemies.png)\
Player can shoot in any direction.\
![Shooting](Images/shooting.png)\
Player can select from multiple elemental weapons.\
![Sunbullet](Images/weapons.png)\
Weather dynamically controls the flow and feel of the game.\
![Weather](Images/weather.png)\
Player can select from 4 different levels.\
![Levels](Images/levels.png)
