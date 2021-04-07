# Havit.Bootstrap
## General notes
1. The file structure as well as the build scripts are taken over from Bootstrap so the update process is straightforward
2. Do not modify contents of the bootstrap folder with exception of updating the source files to new version
3. HAVIT Bootstrap overrides are done in *scss* folder
4. NodeJS resp. NPM or YARN are needed to compile the CSS


## Update instructions
1. Download new sources from https://getbootstrap.com/docs/5.0/getting-started/download/
2. Replace content of folder *bootstrap* with downloaded source files
3. Compare *package.json* and *postcss.config.json* with new sources and update dependencies and build scripts accordingly
4. Make sure that the build process is working by running the build script

## Build instructions
There are several build scripts that take care of compiling, vendor prefixning and minifiing the Bootstrap CSS with HAVIT overrides. 
1. Build script "css" takes care of CSS compiliation, vendor prefixing and minification and runs only once
2. Build script "watch" acts as file-watcher and runs compilation scripts after saving changes and 
3. Build script "publish-to-blazor" moves files from *dist* folder to *Havit.Blazor.Components.Web.Bootstrap/wwwroot*