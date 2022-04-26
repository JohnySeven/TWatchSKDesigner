# TWatchSKDesigner
Watch screen designer and utility program for TWatchSK project.

[TWatchSK](https://github.com/JohnySeven/TWatchSK#readme) is a program that displays data and notifications from [Signal K](https://signalk.org/) on the [LILYGO T-WATCH-2020  wristwatch](http://www.lilygo.cn/prod_view.aspx?Id=1290). TWatchSKDesigner (called simply "Designer" throughout this document) is a program that runs on your personal computer that makes it easy to install TWatchSK on the watch, and to design and install the customizable Signal K data screens for TWatchSK. It runs on Windows, Mac, and Linux.

## Installing Designer

### Linux Installation
1. Download the latest TWatchSKDesigner.x.x.linux-x64.deb (or .rpm) from [Releases](https://github.com/JohnySeven/TWatchSKDesigner/releases/).
2. When the download is complete, double-click on the downloaded file to run the installation. 
3. When the installation is finished, open a terminal window and run `TWatchSKDesigner`.

### Windows Installation
1. Download the latest TWatchDesignerSetup.msi from [Releases](https://github.com/JohnySeven/TWatchSKDesigner/releases/).
2. When the download is complete, double-click on the downloaded file to run the installation. 
3. When the installation is finished, open Windows Start menu and type "twatchskdesigner" - it should appear as a found application - hit the "Enter" key to start it.

### MacOS installation
1. Download the latest TWatchSKDesignerMacOS.zip [Releases](https://github.com/JohnySeven/TWatchSKDesigner/releases/).
2. When the download is complete, double-click on the downloaded file to extract application from zip file.
3. At download directory level new application "TWatchSK Designer" should appear.
4. Drag and drop "TWatch Designer" applications to "Applications" in Finder to move designer into your applications
5. Go to launch pad and click on "TWatch Designer" app
6. MacOS will show you "“TWatchSK Designer” cannot be opened because the developer cannot be verified." message. Click "Cancel".
7. Open "System Preferences" by clicking on Apple logo (upper left), go into "Security & Privacy".
8. Click on "Click the lock to make changes" and switch to "General" tab
9. In section "Allow apps downloaded from" click on "Open anyway" next to text "TWatchDesigner" was blocked...
10. Confirm message "macOS cannot verify the developer of “TWatchSK Designer”. Are you sure you want to open it?" by clicking on Open and you're good to go.

## Installing TWatchSK
After you install Designer, the first thing you'll want to use it for is to install TWatchSK onto your T-WATCH-2020. The following steps will guide you through the process.

NOTE: You must be connected to the Internet for this to work, as everything that's installed is coming from somewhere online, not from your computer.

1. Start Designer. You don't need to login to the SK Server.
2. Connect your T-WATCH-2020 to your computer with the USB cable that came with the watch.
3. In Designer, select Firmware _ Update from the main menu. You should see something like this:  
![image](https://user-images.githubusercontent.com/15186790/140220269-4b245355-5df9-414a-816b-585a63f2d34e.png)
4. You should see the port that the watch is plugged into in the "Upload port" field. If you don't, disconnect and re-connect both ends of the USB cable, then click the "refresh" icon to the right of the field.
5. If you have previously installed anything on this watch, and you want it to be COMPLETELY erased, check the box by "Reset ALL settings to defaults". WARNING: this will erase everything that has been loaded onto this watch since you took it out of its original packaging. If you have previously installed TWatchSK on this watch, EVERYTHING you may have configured will be reset to the program defaults: the watch name, the date and time, the wifi credentials, the SK server address and port, and all the settings for the display, and for waking up, and everything else. (It will erase any DynamicViews you have installed, but those are easily restored with Designer.)
6. Click "Upload". If this is the first time you've done this, some tools will first be downloaded that enable your computer to communicate with the watch. Then the latest release version of the TWatchSK firmware will be downloaded from the [TWatchSK Github repository](https://github.com/JohnySeven/TWatchSK/releases), and it will be copied to your watch.

Your T-WATCH-2020 now has TWatchSK installed, and whenever it's turned on, it will be running TWatchSK. (That's the only program that it will run, unless you install some other program, which will overwrite TWatchSK. That is, you can install and run only one program on T-WATCH-2020 at any time.) [Click here](https://github.com/JohnySeven/TWatchSK#readme) to see everything that TWatchSK can do, and to learn how to use it.

## Updating TWatchSK
Periodically, new versions of TWatchSK will be released. You can check for the lastest release version [here](https://github.com/JohnySeven/TWatchSK/releases), and you can check the version that's on your watch from the TWatchSK *Watch Info* menu. It is also briefly displayed whenever TWatchSK is started, on the startup screen. If there is a newer version, just follow the steps in the "Installing TWatchSK" section above - an update is the same as the initial installation.

## DynamicViews - Custom Screens for Interacting with Signal K
Out of the box, the only interaction between Signal K and TWatchSK is that TWatchSK displays any Notifications* sent by Signal K. However, the DynamicViews feature of TWatchSK allows you to define additional screens for the watch, each of which can have different ways to display data from Signal K, and send data to Signal K. A DynamicView can display the text value of a Signal K Path as text; it can display the numeric value of a Signal K Path as text or in a graphical gauge; it can toggle the value of any Boolean Signal K Path, and it can send a PUT request to Signal K (most likely to a plug-in) to do something like change the autopilot heading by 5 degrees to port.

![image](https://user-images.githubusercontent.com/15186790/139879628-1e463149-4694-4368-9102-70d2bc9fd57e.png) ![image](https://user-images.githubusercontent.com/15186790/140789761-97ee13f6-3feb-4521-9606-0127ba0ffb9a.png)

*A basic DynamicView showing vessel speed, outside temperature, and barometric pressure, and a more advanced View showing information about the watch itself* 

DynamicViews are defined with JSON, so it would not be practical to create them through a user interface on the watch itself. Designer was created to allow you to easily define DynamicViews on your computer that can then be installed onto the watch.

You probably won't want to put every Path from Signal K into a DynamicView - a typical Signal K user might have dozens of Paths, and surely not all of them are important enough to be viewable from your watch. But you can easily fit four, or even six, Paths on a single View, and there's no limit to the number of Views you can have, so if you want to have twenty or thirty Paths available on the watch, you can do it. Just remember - in order to see all this information, you have to wake up the watch, then swipe one or more times to see the View you want, then find the specific data you want on that view, and then decide if that data is within its normal range or not. So for data that's REALLY important to you, we strongly suggest you set up Notifications* in Signal K for when that data is out of normal range, so you'll get a Notification on the watch, in addition to having it in a DynamicView.

Here's a simple example: set up a Notification* in Signal K for engine oil pressure below 40 psi, and create a DynamicView that includes engine oil pressure. If oil pressure drops below 40 psi, you'll get a Notification, and then you can bring up the DynamicView on your watch to see if it's staying stable or continuing to drop. If it's maintaining 39 psi, you might decide it's OK to keep running the engine, but if you see that it's continuing to drop, you'll know to shut off the engine.

(* Notifications: TWatchSK will automatically display any notifications from Signal K. Signal K sends very few notifications out of the box - one example is a notification when a new version is released. But there are many Signal K plugins that send notifications, one of the simplest being the Simple Notifications Plugin, which allows you to set notification thresholds for any Signal K Path. For example, you can set up an Alert notification if your engine coolant temperature is hotter than normal, and an Alarm notification if it gets into the dangerously hot range. Both of these would automatically be displayed by TWatchSK.)

## Creating Your First DynamicView
As soon as you install Designer and connect it to your Signal K Server ("Server" from now on), it will have access to every active Signal K Path on the Server. As soon as you install TWatchSK on your watch and connect it to your Server, it will start sending data to three Paths: TWatchSK.battery, TWatchSK.temperature, and TWatchSK.uptime. NOTE: These three bits of watch data are sent only when the watch is in low power mode (the screen is off), and even then, it's only every 10 seconds. (If you have given your watch a name other than "TWatchSK" while setting it up, your watch's name will replace "TWatchSK" in these three Paths.) So even if you have no other data being sent to the Server, you will have these three Paths, so that's what we're going to use for all of the DynamicViews in this tutorial. The steps will be identical when creating DynamicViews for your real boat data.

![image](https://user-images.githubusercontent.com/15186790/139891245-1c3758b2-373c-4f1e-ab57-99f8db7bc135.png)

*The three Paths sent to the Signal K Server by every TWatchSK*

### Example 1. Watch Battery Status Only
This will be about as simple as a DynamicView can be: just one data field with a single label, using the default colors for everything.

1. Start Designer and connect to your Server.
2. Select Edit _ New view from the menu and you'll see a new, blank View. You'll also see the Edit Window on the right side, with the View displayed. You can see its name, "New view", at the bottom of the View itself, and in the "Name" field of the Edit Window.
3. Change the "Name" field to **Watch Battery**. Notice as you type the new name, it appears at the bottom of the View itself.
4. Click on the "Add new label" button at the bottom of the Edit Window. Notice that the top of the Edit Window has changed to "label" to indicate that now you're editing a label, not the View itself.
5. Leave the "Text" field blank - it will come from the "Format" field on the Binding Window, below.
6. Click on the "..." by the the "Binding" field, and the Binding Window will open.
7. Click on the Signal K icon in the upper right of the Binding Window and a list of all Paths on your Server will pop up.
8. Click on "TWatchSK.battery" in the scrollable list and click OK. The pop-up list will close and the Path will be displayed in the Binding Window.
9. Change the "Format" field (which determines how the values from the Binding will be displayed) to **Battery: $$%**. The "$$" represents the value that will come from the specified Path for this Binding (TWatchSK.battery), and the "Battery:" and "%" are text that will display around the value. So the actual output will look like this: **Battery: 88%**.
10. Click on the "OK" button. The Binding Window will close, you'll see the Path displayed in the Edit Window, and the "Text" field will change to "Battery: --%". The "--" represents the "$$" that you entered in the "Format" field.
11. Click on File _ Save, then "OK", to save this new View to disk.

You should now have a View that looks like this:
![image](https://user-images.githubusercontent.com/15186790/140117214-596edf24-9cb9-4519-a9ba-14ff82ba86d2.png)

When loaded onto your watch (in a process described later), you'll have a single DynamicView that will look just like the body of the image above, but with the current value of the watch's battery percentage displayed instead of the "--".

### Example 2. Watch Battery Status Only - Version 2.0
Obviously, we didn't use very much of the screen for the first one, so now we'll do another one that uses much more of the screen to display the same information. We'll have three labels on this screen: "Watch Battery" at the top, then the battery percentage in the middle, then the word "Percent" at the bottom. We'll also introduce different colors and fonts in this example.

1. Select Edit _ New view from the menu and you'll see a new, blank View.
3. Change the "Name" field to **Watch Battery 2**.
4. In the "Layout" field, select "column_mid".
5. Click on the white square to the right of "Background", and the Color Window will pop up.
6. Click in the "Pick color" field, select Aqua, then click on "OK".
7. Click on the "Add new label" button at the bottom of the Edit Window.
8. Change the "Text" field to **Watch Battery**. Notice that the text is too big to fit in the View that's being built as you go.
9. Change the "Font" field to **montserrat32** to make it fit.
10. Click on the "Add new label" button at the bottom of the Edit Window.
11. Leave the "Text" field blank - it will come from the "Format" field on the Binding Window, below.
12. This label is going to display only a number - an integer from 1 to 100 - so we can make it really large. Select **roboto80** in the "Font" field.
13. Click on the "..." by the the "Binding" field, then the Signal K icon, then select "TWatchSK.battery", then click OK.
14. Since this label is going to display only the value from this Binding, put only **$$** into the "Format" field.
15. Click "OK" to close the Binding Window.
16. Click on the "Add new label" button at the bottom of the Edit Window.
17. Change the "Text" field to **Percent**. 
18. To make it consistent with the text at the top of the screen, change the "Font" to **montserrat32**.
19. Click on File _ Save, then "OK", to save this new View to disk.

You should now have a second View that looks like this:
![image](https://user-images.githubusercontent.com/15186790/140144567-fbcb512c-f7c0-4b8c-b771-e66a13ace3fd.png)

### Editing Existing DynamicViews
At this point, you have two Views. If you ever want to edit an existing View, the first thing you have to do is click on the little pencil icon in the lower left corner of the View you want to edit. That will open the Edit Window if it's not already open, or it will populate the already-open Edit Window with the data for the View whose pencil icon you clicked. The drop-down list at the top of the Edit Window always shows what you're editing - the View itself, or one of its labels.

Be sure to select the correct View, and then the correct label, before you make any changes. And when you're finished, always remember to File _ Save.

### Example 3. Three Values on One View
Our first example did not result in a very attractive screen - just one line at the top - and you would probably never want to use a screen like that. You might want to use a layout like our second example, if you don't have very many DynamicViews, especially for data that's very important. This example will demonstrate a more commonly used layout - one that will more efficiently use the screen by displaying three different Paths: TWatchSK.battery, TWatchSK.temperature, and TWatchSK.uptime (the number of hours, minutes, and seconds since the watch was last rebooted). Each of them will be displayed slightly differently.

1. Select Edit _ New view from the menu and you'll see a new, blank View.
2. Change the "Name" field to **All Watch Info**.
3. In the "Layout" field, select **column_mid**.
4. Click on the white square to the right of "Background", and the Color Window will pop up.
5. Move the Red, Green, and Blue sliders around until you get a color like shown below, then click on "OK".

![image](https://user-images.githubusercontent.com/15186790/140180604-21432b42-8658-46f0-b718-3142b0a35c41.png)

6. Click on the "Add new label" button at the bottom of the Edit Window.
7. Change the "Text" to **Watch Stats**.
8. Change the "Font" to **montserrat28**.
9. Change the "Color" to **DarkOrange**.
10. Create a new label and set these fields: Text = 10 asterisks; Font = **montserrat28**; Color = **DarkGreen**.
11. Create a new label and set these fields: Font = **montserrat28**; Color = **Aqua**; Binding = **TWatchSK.battery** (with Format = **Battery: $$%**).
12. Create a new label and set these fields: Font = **montserrat28**; Color = **Crimson**; Binding = **TWatchSK.temperature** (with Format = **CPU temp: $$**)
13. Create a new label and set these fields: Font = **montserrat28**; Color = **Blue**; Binding = **TWatchSK.uptime** (with Format = **$$**)

You should now have a new View that looks like this: ![image](https://user-images.githubusercontent.com/15186790/140182214-c72724a2-1002-4ef8-b07a-abd29499e779.png)

### About Colors
While the rest of the TWatchSK screens have different Day and Night modes, the DynamicView screens do not. So when choosing colors, consider how those colors will look in low light, when the screen's display brightness is very low, and also in daylight, when the brightness is high. The colors used in the previous example might work for your eyes, or they might not, so experiment!

### Manipulating Values in a Binding
If you installed the DynamicViews that we've created so far, the third example would have a line that would look something like this: **CPU temp: 293**. How can the watch's CPU be 293 degrees? Because it's sent to Signal K in degrees Kelvin, because all data goes to Signal K in "standard units", and for temperature, the standard unit is degrees Kelvin. But you would probably rather see temperatures in degrees Celsius or Fahrenheit, so Designer gives you a way to do that. (Not only convert temperature to different units, but convert any value for any reason.) Let's go back to our last example View and edit that temperature.

1. Click on the pencil icon in the lower left corner of the "All Watch Info" view. If it's not already open, the Edit Window will open with that View.
2. Using the drop-down list at the top, select the "CPU temp" label - it should be the 4th label from the top.
3. Click on "..." beside "Binding" to open the Binding Window.
4. In the "Offset" field, enter **-273.15**. That's how you convert from Kelvin to Celsius - you subtract 273.15. (Whatever you put in the "Offset" field will be ADDED to the raw value, which is why you need the minus sign.)
5. In the "Decimals" field, enter **1**, which will give you 1 decimal point of precision in the output. This isn't necessary - that fraction of a degree probably doesn't really matter - but it's a good time to illustrate this field.
6. Click "OK" to close the Binding Window.

If you want to display the temperature as Fahrenheit, that's a more complicated conversion, but still easily accomplished. The formula to convert from Kelvin to Fahrenheit is *degrees Kelvin × 1.8 - 459.67*. So the "Multiply" field would be **1.8** and the "Offset" field would be **-459.67**.

The only other field on the Binding Window that we haven't touched on yet is "Period (ms)". This is the number of milliseconds between updates of this Binding's value. It defaults to 1000 (1 second), but you can make it however many seconds you like. However, you probably don't need updates more often than once per second for any data, so while this field is customizable, there is probably very little benefit from changing it.

## Downloading DynamicView Into TWatchSK
There will be more examples of more complex DynamicViews below, but now is a good time to show you how to get your DynamicViews from your computer to your watch. It's simple!

1. On the watch, from the home screen (the screen that shows date and time), touch the four squares icon to bring up the menu.
2. Select the "Display" menu.
3. Tap the "Download DynamicViews" button. The Views you've defined with Designer will be downloaded from your Server to your watch, and TWatchSK will reboot.

That's it! Now, from the home screen, swipe left to see the first DynamicView, swipe again to see the second, and so on. (The watch must be connected to your wifi network, and your Server must be on, or you won't see any values in your DynamicViews.)

## Changing the Order of DynamicViews
If you have more than one View, the order of them may become important to you. For example, you might want to have the View that displays four main engine parameters be the first one you see when you swipe from the watch home screen, while the View showing less-important environmental data can be the last one you see when swiping. By default, Views will appear on the watch in the same order in which you created them. However, you can easily rearrange Views in Designer, which puts them into the same order on the watch. Here's how to re-order them.

If you still have the three Views that you created in the first three examples, above, you'll have Watch Battery, Watch Battery 2, and All Watch Info, so that's what we'll work with here. But you can work with any two or more views for this exercise.

1. We're going to move the All Watch Info View into the first position, before the Watch Batery View. Click on the icon on the bottom of the All Watch Info View that's the square and two arrows. That will open a thin vertical panel on the right side of all of the Views.
![image](https://user-images.githubusercontent.com/15186790/140533448-ee7cbe61-b641-4011-bd75-6d32de8a1e02.png)
*Showing the "Move view" icon at the bottom, and the new panel on the right side*
2. To move this View so that it's the first View - to the left of the Watch Battery View - simply click on the thin vertical panel on the right side of the Watch Battery View.
3. The All Watch Info View is inserted to the left of the Watch Battery View and the thin right vertical panels disappear from all of the Views. Repeat the above steps to move any other Views.
4. Select File _ Save to save your changes.

## Example 4. Manually Positioning Labels on the View
The previous examples have all used the default layout **column_mid**, which arranges all labels in a single column, and centers each one in the middle of the screen horizontally. But by choosing the layout called **off**, each label will get a "Location" field, which defines the X and Y coordinates for that label. In this example, we'll arrange our three bits of watch data using this approach.

1. Select Edit _ New view from the menu and you'll see a new, blank View.
2. Change the "Name" field to **All Watch Info 2**.
3. In the "Layout" field, select **off**.
4. Change the "Background" to **AntiqueWhite**.
5. Click on the "Add new label" button at the bottom of the Edit Window.
6. Change the "Text" to **Watch Stats**. Notice that the text is left justified on the View.
7. Change the "Font" to **montserrat32**.
8. Change the "Location" to **20;0**. Notice that the text "Watch Stats" is now horizontally centered on the View.
9. Create a new label and set the "Text" to 12 asterisks. Notice that as you type, the asterisks appear to be overwriting the "Watch Stats" text. That's because the Location of this new label is still set to the default of 0;0.
10. Change the "Font" to **montserrat28** and the "Color" to **Blue**.
11. Change the "Location" to **30;40**, and notice that the asterisks are now horizontally centered below the words "Watch Stats".
12. Create a new label and set these fields: Text = **Batt**; Font = **montserrat28**; Color = **Red**; Location = **10;68**.
13. Create a new label and set these fields: Text = leave it blank; Font = **montserrat28**; Color = **Red**; Binding = **TWatchSK.battery** (Format = **$$%**); Location = **10;105**.
14. Create a new label and set these fields: Text = **CPU temp**; Font = **montserrat28**; Color = **Gold**; Location = **89;70**.
15. Create a new label and set these fields: Text = leave it blank; Font = **montserrat28**; Color = **Gold**; Binding = **TWatchSK.temperature** (Offset = **-273.15**; Decimals = **1**; Format = **$$ C**); Location = **125;105**.
16. Create a new label and set these fields: Text = **TWatchSK Uptime**; Font = **montserrat14**; Color = **Green**; Location = **50;145**.
17. Create a new label and set these fields: Text = blank; Font = **montserrat28**; Color = **Green**; Binding = **TWatchSK.uptime** (Format = **$$**); Location = **70;165**.
18. If you want, follow the steps earlier in this document to move this new View into the very first position.
19. File _ Save to save the new View.

Following the steps earlier in this document, download the DynamicViews to your watch. When the watch reboots, make sure it connects to wifi and Signal K (look at the icons on the top-line status bar). Push the external button to turn off the screen, or just let it turn off at the normal screen time-out time, and let it stay off for at least a full minute. (Watch data is sent to the Server only when the screen is off, and only once every minute.) Now, wake-up the watch and swipe to the left to see your DynamicViews. The one you just created should look like the one below.

![image](https://user-images.githubusercontent.com/15186790/140786698-c6939f73-4929-42d2-8dc5-71d6aafc1570.png) ![image](https://user-images.githubusercontent.com/15186790/140789761-97ee13f6-3feb-4521-9606-0127ba0ffb9a.png)

*The View in Designer, and on the watch: not exactly WYSIWYG, but close!*

You'll notice that not every label looks exactly the same on the watch as it does in Designer - the row of asterisks, for example. So get your View looking the way you want in Designer, then upload to TWatchSK, and see how it looks. Then make changes, upload, and check your results until you get it the way you want it.

## Example 5. Adding a Gauge to a View

In April 2022, three new components were added to DynamicViews: gauges, switches, and buttons. This example will illustrate creating a new View with a gauge, and then the next two examples will add a switch, and then a button.

1. Select Edit _ New view from the menu and you'll see a new, blank View.
2. Change the "Name" field to **Gauge**.
3. In the "Layout" field, select **off**. This allows you to position each component exactly where you want it.
4. Click on the "Add new Gauge" button at the bottom of the Edit Window.
5. Click on the "..." by the the "Binding" field, then on the Signal K icon, then select "TWatchSK.battery", then click OK, then click OK a second time to close the Binding window.
6. Set the "Minimum" to **0** and the "Maximum" to **100**. This defines the bottom and top of the gauge's range. This is appropriate for the watch's battery percentage, but if your gauge is displaying engine oil pressure, for example, you might set the minimum and maximum to 0 and 60.
7. Change the "Location" to **18;125** and the "Size" to **120;120**. Notice that the gauge is now smaller and positioned more in the upper left corner.

By default, a gauge doesn't have a label, because you might want it to be on any side of the gauge, so if you want one, you need to create it:

8. Create a new label and set the "Text" to **Watch battery %**, the "Font" to **montserrat14** and the "Location" to **130;53**.
9. File _ Save to save the new View.

Now you have a new DynamicView with a gauge that displays the watch's battery %, in the top half of the screen.

![image](https://user-images.githubusercontent.com/15186790/165358162-52a13a96-e3af-4b7a-9527-4a6797a90035.png)

Using the "Size" and "Location" fields, you can very nicely display four gauges on a screen in a 2x2 layout. In the next two examples, we'll add a Switch and a Button to this View.

## Example 6. Adding a Switch to a View

In this example, we'll add a Switch to the "Gauge" View that we created in Example #5.

The Switch component is a simple On/Off switch. It displays the current value of the SK Path it's bound to, and when you move the switch from one side to the other, it toggles the value of that Path. So obviously, it can work only with Paths that are Booleans, such as electrical.light.anchor.state. When a DynamicView is displayed, if it contains a Switch, the Switch is set to the current value of its Path. If you tap the Switch to change its value, that also changes the value of the Path on the Server. If you have something else installed that monitors that Path, it can then change the actual status of the anchor light. "Something else" could be a [SensESP BoolSKPutRequest](https://signalk.org/SensESP/generated/docs/classsensesp_1_1_s_k_put_request_listener.html), for example.

1. Click on the "Edit view" button in the lower left corner of the "Gauge" view.
2. Change the "Name" field to **Gauge and Switch**.
3. Click on the "Add new Switch" button at the bottom of the editing window.
4. Click on the "..." next to Binding, and select a SK Path to bind to this Switch. Remember, it has to be a Path whose value is a Boolean. (If you don't have such a Path, don't worry - pick any Path - it will work for purposes of designing the DynamicView. It just won't actually work on the watch.)
5. Set the Location to **19;120** and the Sze to **80;40**.

Now there is a switch in the middle of the View, under the graph, but it has no label. By default, switches don't have a label, because you might want it to be on any side of the switch, so if you want one, you need to create it:

6. Click on the "Add new Label" button on the bottom of the Edit window.
7. Set these field values: Text = **Anchor light** ; Font = **montserrat14**; Location = **105;128**. Now we have a label for the switch.
8. File _ Save to save the modified View.

![image](https://user-images.githubusercontent.com/15186790/165358499-0238e520-b31b-4031-83b4-0d1dc4269b6a.png)



By changing the Size and Location of switches and their labels, you can comfortably have 6 switches on a screen, in a 3 tall x 2 wide layout.

## Example 7. Adding a Button to a View

In this example, we'll add a Button to the "Gauge and Switch" View that we created in Example #6.

The Button component behaves similar to a physical momentary switch: tap it, and a single thing happens. If you want that thing to happen again, you have to tap the button again. In our example, we'll make a button that tells the autopilot to turn 5 degrees to starboard. If you want to turn 10 degrees to starboard, you have to tap the button twice. Each tap of a button sends a PUT request to the Server, so there must be something installed on the Server that knows how to respond to the PUT request. In our example, that "something" is the [signalk-autopilot plug-in](https://github.com/SignalK/signalk-autopilot).

1. Click on the "Edit view" button in the lower left corner of the "Gauge and Switch" view.
2. Change the "Name" field to **Gauge, Switch, Button**.
3. Click on the "Add new Button" button at the bottom of the editing window.
4. Buttons get a text label, so set "Text" to **5º >>** and "Font" to **montserrat28**.
5. Click on the "..." next to "Put", and fill out the pop-up window as follows:
6. Click on the Signal K icon and select the Path that this PUT request is going to be sent to. You'll get this from the documentation of the API for the plug-in you're using. In our example, it will be **steering.autopilot.target.headingMagnetic**. (If you don't have this Path in your system, just pick any Path. It won't work, of course, but it will let you complete the creation of the button component.)
7. Select "Push number value" and put **5** in the value field. Pushing a value of 5 to that Path means "set the heading to 5 degrees to starboard from our current heading". If you want a turn to port, the value would be -5.
8. Click OK to close the pop-up window.
9. Back in the editing window, set the Location to **18;172** and the Sze to **110;44**.
10. File _ Save to save the modified View.

Now you have a DynamicView with a gauge, a switch, and a button that should look like this:

![image](https://user-images.githubusercontent.com/15186790/165355063-f5f903bd-f05a-408f-9126-ae26c44f9fef.png)

With a little creativity, you can make a DynamicView to give you control over your autopilot!

![image](https://user-images.githubusercontent.com/15186790/165354299-66805154-7958-427c-8b29-6d7b3a68bddc.png)


[![.NET](https://github.com/JohnySeven/TWatchSKDesigner/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JohnySeven/TWatchSKDesigner/actions/workflows/dotnet.yml)
