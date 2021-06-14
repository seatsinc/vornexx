# vornehud

Each of the below programs were made to enhance Vorne's capabilities utilizing its programming API.
They each are contained in a folder with the same name. It will have a text file "DEPLOYMENT INSTRUCTIONS" on how to deploy the programs out into production.
Each program may also have a "CONSTANTS" text file that will need to be modified on deployment.***ONLY EDIT INFORMATION ON THE RIGHT SIDE OF THE COLON (:) SYBMOL, DO NOT EDIT ANYTHING ELSE AND KEEP ORDER THE SAME

Below are descriptions of each program with relevant information about them


------------
VorneAPITest
------------
The main control program that the user sets, starts, and stops the takt timer. It also controls the light system via serial communication through a USB port.


CONSTANTS

WORK CENTER: the work center number where the program will be deployed at
VORNE IP ADDRESS: the ip address of the vorne machine (model XL-810-1)
TAKT TIMER PORT: arbitrary port that the program will send information to its client programs (VorneAPITestC)
LIGHTS USB PORT: serial communication port where the lights arduino microcontroller is plugged into (will be in format "COM#")



-------------
VorneAPITestC
-------------
The typical operator program that exists on each operators computers, displaying the current takt time, production information, and also a scoreboard.

CONSTANTS

WORK CENTER: the work center number where the program will be deployed at
VORNE IP ADDRESS: the ip address of the vorne machine (model XL-810-1)
TAKT TIMER HOSTNAME: the host name of the master control program (VorneAPITest)
TAKT TIMER PORT: arbitrary port that the program will receive information from the master control program (VorneAPITest)
LIGHT PORT: serial communication port where the lights arduino microcontroller is plugged into (will be in format "COM#")


--------------
BarcodeScanner
--------------
A program that runs on the computer where serial labels are scanned out.
The program runs in the background listening for serial numbers entered from key presses or a barcode scanner.
Once a correct number is detected, it automatically tells vorne to +1 the good count.

CONSTANTS

VORNE IP: the ip address of the vorne machine (model XL-810-1)
WORK CENTER: the work center number where the program will be deployed at



-----------
vorneLights
-----------
A program written for the arduino microcontroller that continuously receives input from the program "VorneAPITest" and relays that data to RGB lights connected to the arduino.

