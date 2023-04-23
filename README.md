![project_name (3)](https://user-images.githubusercontent.com/66903296/233828533-91c2a683-75b4-4c3d-8c41-af0bedbab1dd.png)
# Sketch Transfer
![image](https://user-images.githubusercontent.com/66903296/233837617-7da3399d-3ede-4838-9b88-89fafb3372ab.png)


## Description
This project aims to help tattoo artists and their clients overcome the challenge of visualizing how a tattoo design will look on the body before actually getting it done. With the use of Unity 3D app and a Spring Boot server, app provides a platform where artists and clients can collaborate in real-time to bring their tattoo ideas to life. The goal is to make the tattooing experience more accessible, convenient, and enjoyable for both parties involved, ultimately leading to better communication and greater satisfaction with the end result.

## Build
**To build the app you will need to instal Unity hub and Unity client itself: https://unity.com/download**

## Instructions
First, you will need to login
- If the server(https://github.com/node-value/sketch_transfer_server) is running locally on localhost:8080, then just sign up/in
- If there is no local server, then uncomit the baseUrl var in GlobalParams script. It will allow you to login to remote server, but client-client option is not available right now

## Modes
**Modes Available: Master and Client**
- In Master mode, you can use the app in single mode without any client connected or in co-working mode if a client is connected to you. Once the client is connected, they will receive all your project data and can manipulate it. Changes made by both parties can be seen in real-time.
- In Client mode, you can connect to a provided master if they are online. To start your own project, switch to Master mode.
- For now connecton available only if the server runs on local machine.

### Controls
The camera can be controlled using either the WASD/Arrow keys or the mouse. To zoom in, hold Shift + W/Arrow Up, and to zoom out, hold Shift + S/Arrow Down. If using the mouse, hold and pull the left button to adjust the camera.

### ToolBar 
- The "Pick a Point" dropdown panel allows the user to select a specific point on the body for the camera to focus on.
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/AddIcon.svg" alt="alt text" width="40" height="40"> - Add new sketch from the device
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/Delete_Icon.PNG" alt="alt text" width="40" height="40"> - Delete sketch from the scene
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/IMG_3986.PNG" alt="alt text" width="40" height="40"> - Pick sketch to edit
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/IMG_3987.PNG" alt="alt text" width="40" height="40"> - Relocate sketch
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/ZoomInIcon.svg" alt="alt text" width="40" height="40"> - Zoom in
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/ZoomOutIcon.svg" alt="alt text" width="40" height="40"> - Zoom out
- <img src="https://github.com/node-value/sketch_transfer/blob/master/Assets/Resources/Icons/Save_Icon.PNG" alt="alt text" width="40" height="40"> - Save progress
- The "Edit a Sketch" dropdown panel allows you to adjust the rotation, scale, and depth of the sketch. The depth value controls how far the sketch is projected, so if you see a sketch in an unexpected place, such as the back when it is supposed to be in the front, try lowering the depth value.

## Note for Challange Reviwers
This app is fully functional, but it still needs some polish. There are currently no error messages in case something goes wrong, the dropdown menus are not as smooth as they could be, and some features are not yet implemented. This is because in the last four days, I decided to expand the challenge by adding a server and connection service, which required a lot of work. While the basic functionality is working, there are still some improvements that can be made.

My plans for future improvements include debugging the Heroku deployment, adding error messages, adding a chat feature for communication between the artist and client, and adding different poses and locations to choose from. Thank you for taking the time to review my app! 
