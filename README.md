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
2. Select Edit _ New view from the menu and you'll see a new, blank View. You'll also see the Edit Window on the right side, with the View displayed. You can see its name, "New view", at the bottom of the View itself, and in the "Name" field of the Edit Window.
3. Change the "Name" field to "Watch Battery". Notice as you type the new name, it appears at the bottom of the View itself.
4. Click on the "New label" button at the bottom of the Edit Window. Notice that the top of the Edit Window has changed to "label" to indicate that now you're editing a label, not the View itself.
5. Leave the "Text" field blank - it will come from the "Format" field on the Binding Window, below.
6. Click on the "..." by the the "Binding" field, and the Binding Window will open.
7. Click on the Signal K icon in the upper right of the Binding Window and a list of all Paths on your Server will pop up.
8. Click on "TWatchSK.battery" in the scrollable list and click OK. The pop-up list will close and the Path will be displayed in the Binding Window.
9. We could change any of the other fields that define how the data from this Path will be displayed, but for this example, we're going to change only the "Format" field, which determines how the values from the Binding will be displayed. Type "Battery: $$%" in that field. The "$$" represents the value that will come from the specified Path for this Binding (TWatchSK.battery), and the "Battery:" and "%" are just text that will display around the value. So the actual output will look like this: **Battery: 88%** (NOTE: If all you want to output is the value, with no other text, leave the "Format" field blank.)
10. Click on the "OK" button. The Binding Window will close, you'll see the Path displayed in the Edit Window, and the "Text" field will change to "Battery: --%". The "--" is a placeholder for where the actual data will appear.
11. Click on File _ Save, then "OK", to save this new View to disk.

You should now have a View that looks like this:
![image](https://user-images.githubusercontent.com/15186790/140117214-596edf24-9cb9-4519-a9ba-14ff82ba86d2.png)

When loaded onto your watch (in a process described later), you'll have a single DynamicView that will look just like the body of the image above, but with the current value of the watch's battery percentage displayed instead of the "--".

### 2. Watch Battery Status Only - Version 2.0
Obviously, we didn't use very much of the screen for the first one, so now we'll do another one that uses much more of the screen to display the same information. We'll have three labels on this screen: "Watch Battery" at the top, then the battery percentage in the middle, then the word "Percent" at the bottom. We'll also introduce different colors and fonts in this example.

1. Select Edit _ New view from the menu and you'll see a new, blank View.
3. Change the "Name" field to "Watch Battery 2".
4. In the "Layout" field, select "column_mid".
5. Click on the white square to the right of "Background", and the Color Window will pop up.
6. Click in the "Pick color" field, select Aqua, then click on "OK".
7. Click on the "New label" button at the bottom of the Edit Window.
8. Change the "Text" field to "Watch Battery". Notice that the text is too big to fit in the View that's being built as you go.
9. Change the "Font" field to "montserrat32" to make it fit.
10. Click on the "New label" button at the bottom of the Edit Window.
11. Leave the "Text" field blank - it will come from the "Format" field on the Binding Window, below.
12. This label is going to display only a number - an integer from 1 to 100 - so we can make it really large. Select "roboto80" in the "Font" field.
13. Click on the "..." by the the "Binding" field, then the Signal K icon, then select "TWatchSK.battery", then click OK.
14. Since this label is going to display only the value from this Binding, you don't need to specify any "Format". But you can accomplish the same thing by putting "$$" into the "Format" field.
15. Click "OK" to close the Binding Window.
16. Click on the "New label" button at the bottom of the Edit Window.
17. Change the "Text" field to "Percent". 
18. To make it consistent with the text at the top of the screen, change the "Font" to "montserrat32".
19. Click on File _ Save, then "OK", to save this new View to disk.

You should now have a second View that looks like this:
![image](https://user-images.githubusercontent.com/15186790/140144567-fbcb512c-f7c0-4b8c-b771-e66a13ace3fd.png)

### Editing Existing DynamicViews
At this point, you have two Views. If you evern want to edit an existing View, the first thing you have to do is click on the little pencil icon in the lower left corner of the View you want to edit. That will open the Edit Window if it's not already open, or it will populate the already-open Edit Window with the data for the View whose pencil icon you clicked. The drop-down list at the top of the Edit Window always shows what you're editing - the View itself, or one of its labels.

Be sure to select the correct View, and then the correct label, before you make any changes. And when you're finished, always remember to File _ Save.

### 3. Three Values on One View
Our first example did not result in a very attractive screen - just one line at the top - and you would probably never want to use a screen like that. You might want to use a layout like our second example, if you don't have very many DynamicViews, especially for data that's very important. This example will demonstrate a more commonly used layout - one that will more efficiently use the screen by displaying three different Paths: TWatchSK.battery, TWatchSK.temperature, and TWatchSK.uptime (the number of seconds since the watch was last rebooted). Each of them will be displayed slightly differently.

1. Select Edit _ New view from the menu and you'll see a new, blank View.
2. Change the "Name" field to "All Watch Info".
3. In the "Layout" field, select "column_mid".
4. Click on the white square to the right of "Background", and the Color Window will pop up.
5. Move the Red, Green, and Blue sliders around until you get a color like shown below, then click on "OK".

![image](https://user-images.githubusercontent.com/15186790/140180604-21432b42-8658-46f0-b718-3142b0a35c41.png)

6. Click on the "New label" button at the bottom of the Edit Window.
7. Change the "Text" to "Watch Stats".
8. Change the "Font" to "montserrat28".
9. Change the "Color" to "DarkOrange".
10. Create a new label and set these fields: Text = 10 asterisks; Font = montserrat28; Color = DarkGreen.
11. Create a new label and set these fields: Font = montserrat28; Color = Aqua; Binding = TWatchSK.battery (with Format = "Battery: $$%").
12. Create a new label and set these fields: Font = montserrat28; Color = Crimson; Binding = TWatchSK.temperature (with Format = "CPU temp: $$")
13. Create a new label and set these fields: Font = montserrat28; Color = Blue; Binding = TWatchSK.uptime (with Format = "$$")

You should now have a new View that looks like this: ![image](https://user-images.githubusercontent.com/15186790/140182214-c72724a2-1002-4ef8-b07a-abd29499e779.png)

## About Colors
While the rest of the TWatchSK screens have different Day and Night modes, the DynamicView screens do not. So when choosing colors, consider how those colors will look in low light, when the screen's display brightness is very low, and also in daylight, when the brightness is high. The colors used in the previous example might work for your eyes, or they might not, so experiment!



[![.NET](https://github.com/JohnySeven/TWatchSKDesigner/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JohnySeven/TWatchSKDesigner/actions/workflows/dotnet.yml)
