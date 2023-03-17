# SvRooij.TeamsMonitor.Core

 [![Nuget badge][badge_nuget]][link_nuget]
 [![Number of github stars][badge_stars]][link_source]
 [![Number of github issues][badge_issues]][link_issues]
 [![Source on github][badge_source]][link_source]
 
 [![GitHub Sponsors][badge_sponsor]][link_sponsor]
 [![Check my blog][badge_blog]][link_blog]

A simple library you can use to connect to your local Teams Client. This library manages the required [websocket connection](#teams-has-a-local-api) so you can get instant updates from your Teams client.

You can also control various settings using this library. Just explore all the methods on the `TeamsSocket`.

## Sample code

```csharp
using var socket = new TeamsSocket(new TeamsSocketOptions("{token-from-teams-client}"));
socket.Update += (object sender, MeetingUpdate update) => { ... }
await socket.ConnectAsync(true, cancellationToken);
```

## dotnet tool available

There also is an [application](https://github.com/svrooij/teams-monitor/blob/main/README.md) available, if you just want to forward your Teams status to any webserver (like home assistant).

## Teams has a local api?

Yes, it does! If you never heard of it, that might be right because it was released February 1st 2023. Once you [enable](https://support.microsoft.com/en-us/office/connect-third-party-devices-to-teams-aabca9f2-47bb-407f-9f9b-81a104a883d6) it, you get a local api token.

I'm not explaining what the api looks like, as [Martijn Smit](https://lostdomain.notion.site/Microsoft-Teams-WebSocket-API-5c042838bc3e4731bdfe679e864ab52a) already did that. For now you just need to know, if you enable the local api your Teams client will open a websocket server at localhost post `8124`.

You can just use any client that supports websockets and connect to the following url.

`ws://localhost:8124?token=API-TOKEN-FROM-PRIVACY&protocol-version=1.0.0&manufacturer=MuteDeck&device=MuteDeck&app=MuteDeck&app-version=1.4`

If something changes to your meeting status (or any of the other values), you'll get a JSON encoded message on the open websocket connection that looks like:

```json
{
    "apiVersion": "1.0.0",
    "meetingUpdate": {
        "meetingState": {
            "isMuted": false,
            "isCameraOn": true,
            "isHandRaised": false,
            "isInMeeting": false,
            "isRecordingOn": false,
            "isBackgroundBlurred": false
        },
        "meetingPermissions": {
            "canToggleMute": false,
            "canToggleVideo": true,
            "canToggleHand": false,
            "canToggleBlur": false,
            "canToggleRecord": false,
            "canLeave": false,
            "canReact": false
        }
    }
}
```

## Socials

[![LinkedIn Profile][badge_linkedin]][link_linkedin]
[![Link Mastodon][badge_mastodon]][link_mastodon]
[![Follow on Twitter][badge_twitter]][link_twitter]
[![Check my blog][badge_blog]][link_blog]
[![Number of github stars][badge_stars]][link_source]

If you like my Teams Monitor, please give me a shout out on any of these platforms.

[badge_issues]: https://img.shields.io/github/issues/svrooij/teams-monitor?style=for-the-badge&logo=github
[badge_nuget]: https://img.shields.io/nuget/v/SvRooij.TeamsMonitor.Core?logo=nuget&style=for-the-badge
[badge_source]: https://img.shields.io/badge/source-svrooij%2Fteams--monitor-blue?style=for-the-badge&logo=github
[badge_sponsor]: https://img.shields.io/github/sponsors/svrooij?label=Github%20Sponsors&style=for-the-badge&logo=github
[badge_stars]: https://img.shields.io/github/stars/svrooij/teams-monitor?style=for-the-badge&logo=github
[link_issues]: https://github.com/svrooij/teams-monitor/issues
[link_nuget]: https://www.nuget.org/packages/svrooij.teamsmonitor.core
[link_source]: https://github.com/svrooij/teams-monitor
[link_sponsor]: https://github.com/sponsors/svrooij/

[badge_blog]: https://img.shields.io/badge/blog-svrooij.io-blue?style=for-the-badge
[badge_linkedin]: https://img.shields.io/badge/LinkedIn-stephanvanrooij-blue?style=for-the-badge&logo=linkedin
[badge_mastodon]: https://img.shields.io/mastodon/follow/109502876771613420?domain=https%3A%2F%2Fdotnet.social&label=%40svrooij%40dotnet.social&logo=mastodon&logoColor=white&style=for-the-badge
[badge_twitter]: https://img.shields.io/badge/follow-%40svrooij-1DA1F2?logo=twitter&style=for-the-badge&logoColor=white
[link_blog]: https://svrooij.io/
[link_linkedin]: https://www.linkedin.com/in/stephanvanrooij
[link_mastodon]: https://dotnet.social/@svrooij
[link_twitter]: https://twitter.com/svrooij