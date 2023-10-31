![project_name (3)](https://user-images.githubusercontent.com/66903296/233828533-91c2a683-75b4-4c3d-8c41-af0bedbab1dd.png)
# Sketch Transfer
![image](https://user-images.githubusercontent.com/66903296/233837617-7da3399d-3ede-4838-9b88-89fafb3372ab.png)


## Description
This project aims to help tattoo artists and their clients overcome the challenge of visualizing how a tattoo design will look on the body before actually getting it done. With the use of Unity 3D app and a Spring Boot server, app provides a platform where artists and clients can collaborate in real-time to bring their tattoo ideas to life. The goal is to make the tattooing experience more accessible, convenient, and enjoyable for both parties involved, ultimately leading to better communication and greater satisfaction with the end result.

## Download
[Windows 10 build](https://drive.google.com/file/d/1095uhFa4xTRlUk_pIBQrNdBc4G9QDigC/view?usp=sharing)

## Build
### Prerequisites
- [Unity Hub](https://unity.com/download) installed
- Unity client installed
### Steps
- Clone the repository to your local machine.
- Open Unity Hub and click on the `Projects` tab.
- Click the `Add` button and select the cloned repository folder.
- Once the project is added, click on it to open it in Unity.
- In Unity, navigate to the `Scenes` folder and double-click on the "Menu" file to open it.
- Click on the `Play` button to start the app and test it out.
- To make an executable click on `File` -> `Build Settings` or `Buid and Run`

## Instructions
First, you will need to login or register

## Modes
**Modes Available: Master and Client**
- In Master mode, you can use the app in single mode without any client connected or in co-working mode if a client is connected to you. Once the client is connected, they will receive all your project data and can manipulate it. Changes made by both parties can be seen in real-time.
- In Client mode, you can connect to a provided master if they are online. To start your own project, switch to Master mode.

### Controls
The camera can be controlled using either the WASD/Arrow keys or the mouse. To zoom in, hold Shift + W/Arrow Up, and to zoom out, hold Shift + S/Arrow Down. If using the mouse, hold and pull the left button to adjust the camera.

### ToolBar 
- The `Pick a Point` dropdown panel allows the user to select a specific point on the body for the camera to focus on.
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/AddIcon.svg" alt="alt text" width="40" height="40"> - Add new sketch from the device
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/Delete_Icon.PNG" alt="alt text" width="40" height="40"> - Delete sketch from the scene
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/IMG_3986.PNG" alt="alt text" width="40" height="40"> - Pick sketch to edit
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/IMG_3987.PNG" alt="alt text" width="40" height="40"> - Relocate sketch
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/ZoomInIcon.svg" alt="alt text" width="40" height="40"> - Zoom in
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/ZoomOutIcon.svg" alt="alt text" width="40" height="40"> - Zoom out
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/Save_Icon.PNG" alt="alt text" width="40" height="40"> - Save progress
- The `Edit a Sketch` dropdown panel allows you to adjust the rotation, scale, and depth of the sketch. The depth value controls how far the sketch is projected, so if you see a sketch in an unexpected place, such as the back when it is supposed to be in the front, try lowering the depth value.
