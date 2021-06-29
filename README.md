***HarperDB Studio Mobile*** is a cross-platform app (iOS, Android, UWP and more) for HarperDB Studio built during/for [HarperDB Hackathon on Hashnode](https://townhall.hashnode.com/announcing-harperdb-hackathon-on-hashnode). This project was made using "Xamarin Forms v4.0"


# FEATURES
- Supported on Android, iOS, UWP (Windows 10) and many more.
- Easily portable to other platforms as well (hopefully Windows 11 in near future too).
- Create new records, edit existing records or delete existing records for any table in any schema.
- Cached logins, so you don't have to enter everything again and again.
- ***Offline mode***, i.e., ability to sync your changes when you're offline. Read more about this below.

So, with the features out of the way, here's a video of a little walkthrough of the application.

### Application Demo
[![Check The YouTube Demo](https://img.youtube.com/vi/FnqcjziSreg/0.jpg)](https://www.youtube.com/watch?v=FnqcjziSreg)


This POC that I made, it gives a pretty good insight on which directions we can go in the final application (the planned updates mentioned below). I don't want to keep things "too technical" in this post, but, I do want to share some setbacks I faced while developing this application and certain things that bugged me a little. If you want to read more about the "offline mode", scroll down to see it in action and understand how it works.


# OFFLINE MODE
The HarperDB bounty program I was talking about, from where I got this idea. Actually, I've been wanting to build a cross-platform app utilising HarperDB ever since I came across the first Hackathon (2018), but, this offline mode idea has been stuck in my head ever since I came across this thread. So, here's what the offline mode does or works like.

Let's say you're trying to add a new row or update existing data and suddenly network goes off on your device. As soon as the app detects that your network is down, "*offline mode*" is switched on and from thereon, your operations are recorded and cached. By operations I mean.. adding a new row, updating an existing row and deleting any existing row. Yep, it's not just adding a new row that gets saved... even edited row data is saved and cached. So, even if you were to close the application after making those changes, the changes are retained. So, next time when you open the app and there's network connection, those cached operations are run in background and your data is synced with the online HarperDB Studio instance. You don't have to do anything.. it's taken care of. It just works. Pretty cool and handy, right? There are few optimisations I want to make in this area, just to squeeze out some better performance, but the current implementation doesn't lack anything. It works flawlessly.

Here's a little video demonstrating the offline mode.

[![Check The YouTube Demo](https://img.youtube.com/vi/05YpbXdVkq8/0.jpg)](https://www.youtube.com/watch?v=05YpbXdVkq8)


# FUTURE UPDATES
- Encrypted stored cached data for added security.
- Ability to create or delete Organizations/Instances.
- Complete offline mode (from "logging in" to browsing instances and organizations).
- UI/UX improvements.

I wrote a big paragraph on where HarperDB team should be working on, but this app I built isn't storing cached data in encrypted strings either. Well, there is a reason for that and it's that Xamarin doesn't have any inbuilt "cryptography" mechanism. They do have "SecureStorage", which is said to be better than storing any data in cache in plain string format. But, since I was developing and testing on "Simulator" (iOS), that implementation failed. So, I had to resort to normal App settings cache for storing data. But, when I'm done with these mentioned changes, I'll invest some time in that area as well.

The offline mode is good, the only limit of it as of now is that it kicks in only if you're on the grid page (where you can access tables and schema). So, if you're browsing Organizations or looking at Instance list, you can't browse the entire application in offline mode. So, I'd like to work on that area and will try to make this entire app "offline" itself.


# Running This Project :
The minimum Android OS that you need to run this project is Lollipop (5.0) (API Level 21). So, make sure you're satisfying the minimum requirements first. Otherwise, your handset won't be able to parse the apk file.

- ### Permissions Required :
    This application requires you to provide few permissions to it, in order to work properly. Here's the list of permissions that the application needs :
    - Internet Access
    - View WiFi Connections
    - Storage (Read/Write Perms For Cache)
    
- #### Instructions For Direct APK Installation :
    If you want to run this application on your android phone, please move over to the "[`Release`](https://github.com/Xonshiz/HarperDB-Studio-Mobile/releases/)" section and download the latest stable APK build for your android phone. You do not need any external libraries or application.

- #### Instructions For Developers/Testers :
     If you're a developer or any user who wishes to test this application and run this android project, it's recommended to install Visual Studio with Xamarin Support and Android SDKs on your system. Remember that Android SDKs should be in your local path for you to be able to compile the project properly. You can find the source code in the "[SOURCE](https://github.com/Xonshiz/HarperDB-Studio-Mobile/tree/master/src)" directory.

    If you do not happen to have Visual Studio, it is recommended to get it because it'll download all the required packages on its own, if they're not present. You can use Visual Studio's Free Community Edition. It'll work, as we've developed this application on it.
But, if for some reason, you don't want to or can't install Visual Studio, you will need to have .NET, Xamarin, Android SDK and required Packages in your system's local path for you to be able to compile and execute this application project.