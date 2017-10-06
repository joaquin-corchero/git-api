# git-api
Little app that uses the git api

What I am and how I do it:
- MVC application built on <strong>.net Core 2.0</strong> that allows users to search github repos.
- Once repos are found, the first 5 will be displayed on a table.
- Underneath each of the repos, the last 5 commits related to it will be displayed.
- The application has been developed mostly using <strong>test first</strong>.
- <strong>No github authenticaion</strong> has been embeded so the number of requests for commits is restricted.
- A <strong>response cache</strong> of 180 seconds has been added to the search.

