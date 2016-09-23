# teamcity-build-number-updater
Given a base directory and a TeamCity build counter, walk through each project subdirectory and update the "version" value in each project.json. Assume we're starting in the src dir in a project structure like:
    
     /<git root dir>
     |__/src
        |__/ProjectA
           |__project.json
        |__/ProjectB
           |__project.json
        |__ProjectName.sln
        |__global.json
