# Sharpkov
*This project is a C# port of the now abandoned [PyrKov project](https://github.com/MayakoLyyn/Pyrkov).*

Generate new tweets based on old ones using a custom Markov Chains implementation. It's written in C#, using **ASP.Net Core 2**, it means that this project is cross-platform! You can run it on Linux, Mac and obviously Windows.

SharpKov also uses Twitter's REST API 1.1 through [TweetInvi](https://github.com/linvi/tweetinvi/) and has been made to be executed by a CRON task. The best frequency is something like every 30 minutes to avoid any restriction.

# Prerequisite before using SharpKov

## Create a Twitter app
SharpKov, and every application using Twitter API, requires some access keys. To get these keys, you have to create an application on your Twitter account and ask Twitter for your keys. Here is how you do it.

- Go on [Twitter App](https://apps.twitter.com/) and log in into your Twitter account
- Next to the title **Twitter Apps**, click on the "Create New App" button
- You'll have to type some informations :
    - **Name:** Name that will be showed for your application (visible on Tweetdeck for example)
    - **Description:** Write something to remember what is this application for
    - **Website:** Link to redirect users who have clicked on your application name
- Once your application is created, go into its settings then in the **Keys and Access Tokens** tab.
- Under **Application Settings**, you have the two first needed keys (Consumer Key, Consumer Secret)
- Under **Your Access Token**, you will have to create your access token. Then you'll get two more keys (Access Token, Access Token Secret)

Keep your keys, this application will need them.

# I want to use SharpKov
#### TODO - Release guide for Windows

## Windows

## Linux server
> /!\ This is based on a Debian 8 server

### Using SharpKov from the release

#### TODO - Release guide for Linux

### Using SharpKov from the sources

Clone the SharpKov repository in a folder:
```bash
git clone https://github.com/MayakoLyyn/SharpKov.git
cd SharpKov
```

You now have to configure the application. A configuration template is available in the *appSetting.default.json* file, copy and rename it to *appSettings.json*, then, open it.
```bash
cd Config/
cp appSetting.default.json appSettings.json
nano appSettings.json
```
> I use nano, but you obviously can use your favorite text editor.

Set up your keys then build SharpKov

#### TODO - build

#### Bonus: Add SharpKov to crontab
*I recommand to execute SharpKov every half-hour, not more, or you'll likely to exceed the API Rate Limit.*

Open crontab and add an entry for SharpKov:
```bash
sudo nano /etc/crontab
```

I set up my crontab to execute SharpKov every 30 minutes and to write a new log at each execution
```
*/30 * * * *    username    (cd ~/SharpKov && dotnet ./SharpKov.dll) > ~/SharpKov/crontab.log
```

# I want to contribute / fork

## Prerequisites
- [Visual Studio](https://www.visualstudio.com/fr/downloads/)
- [(Microsoft) .NET Core Framework](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites?tabs=netcore2x)
- [(Linux) .NET Core Framework](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x)
- [(macOS) .NET Core Framework](https://docs.microsoft.com/en-us/dotnet/core/macos-prerequisites?tabs=netcore2x)

# Installation guide

## Windows

Clone the SharpKov repository with your Git client in a folder. If you use git in a terminal:
```bash
git clone https://github.com/MayakoLyyn/SharpKov.git
cd SharpKov
```

You now have to configure the application. A configuration template is available in the Config folder, in a JSON file: *appSettings.default.json*. Copy then rename it to *appSettings.json*, then, open it with your favorite text editor (it's Visual Studio Code, isn't it,). Then, set up your keys, update your preferences and you're ready to go!

You can launch the solution and start it immediatly.

## Linux / macOS
As I don't use Linux or mac to develop this app, I don't have any guide for you. But it should be easy to install and similar to Windows.

# appSettings.json structure
***Note:** Don't forget to compare appSettings.default.json and your appSettings.json file as it can change at any time, offering more customization.*

    {
        "Infos": {
            "AccountName": "Account"
        },
        "Auth": {
            "ConsumerKey": "",
            "ConsumerSecret": "",
            "AccessToken": "",
            "AccessSecret": ""
        },
        "Preferences": {
            "Local": false,
            "ForceLastWord": false,
            "Logging": false,
            "TestMode": false 
        }
    }

## [Infos]
- **AccountName** : This key is not read by the application. It's only useful if you use more than one config file, you can keep the account name there.

## [Auth]
All these key are required and available in your Twitter Application on https://apps.twitter.com
- **ConsumerKey**: your Consumer Key
- **ConsumerSecret**:  your Consumer Secret Key
- **AccessToken**:  your Access Token
- **AccessSecret**:  your Access Token Secret

## [Preferences]
- **Local**: If its set to *True*, the generated tweet will be posted on your account once it's created. Otherwise, SharpKov will create a *temp.txt* containing 100 generated tweet.
- **ForceLastWord**: If its set to *True*, the last word of the generated tweet will be a last word from a real tweet.
- **Logging** : True means that output informations will be emitted. It's useful to understand what is happening. If this is set to False, SharpKov will remain silent.
- **TestMode** : *To be implemented.*
