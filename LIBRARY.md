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
using var socket = new TeamsSocket(new TeamsSocketOptions("{token-from-previous-paired-session}") { AutoPair = true });
socket.Update += (object sender, MeetingUpdate update) => { ... }
// Be sure to save the token and reuse it next time when connecting.
socket.NewToken += (object sender, string token) => { ... }
socket.ServiceResponse += (object sender, ServiceResponse response) => { ... }
await socket.ConnectAsync(true, cancellationToken);
```

## dotnet tool available

There also is an [application](https://github.com/svrooij/teams-monitor/blob/main/README.md) available, if you just want to forward your Teams status to any webserver (like home assistant).

## Teams has a local api?

Yes, it does! If you never heard of it, that might be right because it was released February 1st 2023. Once you [enable](https://support.microsoft.com/office/connect-third-party-devices-to-teams-aabca9f2-47bb-407f-9f9b-81a104a883d6?wt.mc_id=SEC-MVP-5004985) it, Teams will open a websocket server on localhost.

[Full api docs](https://github.com/svrooij/teams-monitor#teams-has-a-local-api)

You can just use any client that supports websockets and connect to the following url.

ws://localhost:8124?token=61e9d3d4-dbd6-425d-b80f-8110f48f769c&protocol-version=2.0.0&manufacturer=YourManufacturer&device=YourDevice&app=YourApp&app-version=2.0.26`

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