# TWatchSKDesigner
Watch face designer and utility program for TWatchSK project.

[TWatchSK](https://github.com/JohnySeven/TWatchSK#readme) is a program that displays data and notifications from [Signal K](https://signalk.org/) on the [LILYGO T-WATCH-2020  wristwatch](http://www.lilygo.cn/prod_view.aspx?Id=1290). TWatchSKDesigner (called simply "Designer" throughout this document) is a program that runs on your personal computer that makes it easy to install TWatchSK on the watch, and to design and install the customizable Signal K data screens for TWatchSK. It runs on Windows, Mac, and Linux.

## Installing Designer
BAS: write this after Jan gets the first version ready to release.

## Installing TWatchSK
After you install Designer, the first thing you'll want to use it for is to install TWatchSK onto your T-WATCH-2020. (If you think you'd like to contribute to the development of TWatchSK, [click here] BAS: link to the document that tells, briefly, how to fork and clone the project.) The following steps will guide you through the process.
1. Do the first step.
2. Do the second step.
3. Finish the last step.

Your T-WATCH-2020 now has TWatchSK installed, and whenever it's turned on, it will be running TWatchSK. (That's the only program that it will run, unless you install some other program, which will overwrite TWatchSK. That is, you can install and run only one program on T-WATCH-2020 at any time.) [Click here](https://github.com/JohnySeven/TWatchSK#readme) to see everything that TWatchSK can do, and to learn how to use it.

## Updating TWatchSK
Periodically, new versions of TWatchSK will be released. You can check for the lastest release version [here](https://github.com/JohnySeven/TWatchSK/releases), and if there is a newer version than the version on your watch, you can follow these steps to install the latest version. (You can check the version from the TWatchSK *Watch Info* menu. It is also briefly displayed whenever TWatchSK is started, on the startup screen.)
1. Step one.
2. Step two.
3. Step last.

## DynamicViews - Custom Screens for Displaying Signal K Data
Out of the box, the only information from Signal K that TWatchSK displays is (a) any Notifications* sent by Signal K, and (b) basic data about your T-WATCH-2020, such as its battery life and internal temperature. (These are on the watch only to demonstrate the DynamicViews feature. Battery life is always visible on the top of the TWatchSK home screen, and you don't need to monitor the internal temperature.) (BAS: We may decide not to install ANY DynamicViews out of the box, so the previous may not be accurate.) The DynamicViews feature of TWatchSK allows you to define additional screens for the watch, each of which can display the data from one, two, three, or four Signal K Paths.

![image](https://user-images.githubusercontent.com/15186790/139879628-1e463149-4694-4368-9102-70d2bc9fd57e.png)

*A DynamicView screen showing vessel speed, outside temperature, and barometric pressure* (BAS: Show a couple more, with color, and labels, etc.)

DynamicViews are defined with JSON, so it would not be practical to create them through a user interface on the watch itself. The original reason for writing Designer was to allow you to easily define DynamicViews on your computer, which would then be installed onto the watch.

(* Notifications: TWatchSK will automatically display any notifications from Signal K. Signal K sends very few notifications out of the box - one example is a notification when a new version is released. But there are many Signal K plugins that send notifications, one of the simplest being the Simple Notifications Plugin, which allows you to set notification thresholds for any Signal K Path. For example, you can set up an Alert notification if your engine coolant temperature is hotter than normal, and an Alarm notification if it gets into the dangerously hot range. Both of these would automatically be displayed by TWatchSK.)

## Creating Your First DynamicView
As soon as you install Designer and connect it to your Signal K Server ("Server" from now on), it will have access to every active Signal K Path on the Server. As soon as you install TWatchSK on your watch and connect it to your Server, it will start sending data to three Paths: TWatchSK.battery, TWatchSK.temperature, and TWatchSK.uptime. (If you have given your watch a name other than "TWatchSK" while setting it up, your watch's name will replace "TWatchSK" in these three Paths.) So even if you have no other data being sent to the Server, you will have these three Paths, so that's what we're going to use for all of the DynamicViews in this tutorial. The steps will be identical when creating DynamicViews for "real" data.
![image](https://user-images.githubusercontent.com/15186790/139891245-1c3758b2-373c-4f1e-ab57-99f8db7bc135.png)

*The three Paths sent to the Signal K Server by every TWatchSK*

### 1. Watch Battery Status Only
This will be about as simple as a DynamicView can be: just one data field with a single label, using the default colors for everything.
1. Start Designer and connect to your Server.
2. Select Edit _ New view from the menu and you'll see a new, blank view. You'll also see the Edit Window on the right side, with the View displayed. You can see its name, "New view", at the bottom of the View itself, and in the "Name" field of the Edit Window.
3. Change the "Name" field to "Watch Battery". Notice as you type the new name, it appears at the bottom of the View itself.
4. Click on the "New label" button at the bottom of the Edit Window. Notice that the top of the Edit Window has changed to "label" to indicate that now you're editing a label, not the View itself.
5. Click on the ... by the the "Binding" field, and the Binding Window will open.
6. Click on the Signal K icon in the upper right of the Binding Window and a list of all Paths on your Server will pop up.
7. Click on "TWatchSK.battery" and click OK. The pop-up list will close and the Path will be displayed in the Binding Window.
8. We could change any of the other fields that define how the data from this Path will be displayed, but we're not going to, so click on the "OK" button. The Binding Window will close, you'll see the Path displayed in the Edit Window, and the "Text" field will change to "--". The -- is a placeholder for where the actual data will appear.
9. Change the "Text" field to "Battery -- %". As you type, the text will appear on the View itself, because Designer is a real-time WYSIWYG editor as much as possible.

[![.NET](https://github.com/JohnySeven/TWatchSKDesigner/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JohnySeven/TWatchSKDesigner/actions/workflows/dotnet.yml)
