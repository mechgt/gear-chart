# Gear Selection
Gear Selection plugin for SportTracks (Desktop).  Used to estimate the gear selected across a cycling activity.

Estimate which gears you used during a ride based on speed and cadence.  This will also display statistics of gear usage (see table in screenshot) and integrates with the Filtered Statistics (provides extra statistics and filtering capabilities) and Activity Documentation (gear chart can be added to documentation report) plugins.  You'll need a speed or GPS track or speed sensor, along with a cadence data track from a cadence sensor to properly use this plugin.

This software was open-sourced after SportTracks desktop was discontinued, and all restrictions have been removed.

### Getting Started
This plugin adds a new detail page to SportTracks.  You can access it as shown below:

![gs_menu](https://mechgt.com/st/images/gs_menu.png)


This will allow you to see the raw data track (red line), however you'll need to add some details on your bike setup to get an estimation of your gearing.  Details on this are shown in Settings section below.


### Main Display
The upper portion of the display is a list of all your possible different gear combinations.  Any duplicated combo's are listed as one entry (48 x 24 and 32 x 16 have same gear ratios for example).

The chart below shows how you change gears over time.  The red line is the raw calculated data and can be turned on or off with the 'Show raw data' menu option shown below.  Similarly, the blue line is the estimated value based on your cassette/chain ring setup and can also be turned on or off by the 'Show Est. gears' menu option.  High numbers on the y-axis (maybe 8 m/rev in the image below) are the harder faster gears, while smaller numbers (4 m/rev for instance) are easy climbing gears.  The x-axis is configurable for either time or distance using the menu shown below.

![gs_detail](https://mechgt.com/st/images/gs_detail.png)

You may also add more charts to the display (elevation or power for example) using the 'More Charts' toolbar button shown below.

![gs_charts](https://mechgt.com/st/images/gs_charts.png)


### Settings Page/Configuring Gear Selection
Gears are associated with different pieces of equipment.  This is so that you can have different configurations for different bikes, or different wheel sets.

![gs_settings](https://mechgt.com/st/images/gs_settings.png)

1) Select the equipment to associate with using the drop down box.  I recommend using a wheel or cassette.  Something more detailed than your overall bike in case you ever get a different wheel, or for when you replace your cassette.
3) Add number of teeth on each gear for your chainring and cassette.  Each sprocket should have an entry.  As shown in the picture above, I'm running a 9 speed cassette, and no granny-gears over here :).
3) See above, this is for the rear cassette.
4) Set your wheel circumference as measured in mm.  2096 mm is a typical approximation for a standard 700x23c road tire.  If you have a more exact measurement for your bike, you should enter it.

### Languages Supported:
English, Deutsch, español, français, svenska