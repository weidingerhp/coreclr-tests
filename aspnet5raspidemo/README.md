# Raspberry Pi / Windows 10 IoT Demo

Makes use of Asp.Net 5 and Websockets to run something on Windows 10 IoT on RasPi

**Note that you need to use 1.0.0-rc2-16319 (dnvm add 1.0.0-rc2-16319 -a (arm|x64) -u)**

#### Short Description

This project launces an Asp.Net 5 server service on port 5000 either via Self-Hosted (*dnx web*) or in kestrel (*dnx kestrel*)

The Startup.cs specifies the Startup-Settings (wohooo) which initializes static file handline (*wwwroot*) and Websockets.

Websocket Handling is done in SocketHandler.cs.

