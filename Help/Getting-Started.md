# Getting Started

## Install Tools

## Join GitHub and the SWHS-PC

You will need to create an account on github.com and join the SWHS-PC organization
to contribute code.

Once you've created a github account, send your github user name to one of the SWHS-PC
"owners" (Nick or Mr. Green) and we will invite you to become a member of the SWHS-PC
organization.

Once you've received and accepted the invitation, you can move on to the next step,
which is to clone the repository.

## Clone the Repository

Cloning a repository makes a copy of it on your computer, so you can make changes
locally.

First, create a folder on your computer for your cloned repositories. This could be
anywhere, but for the sake of these instructions, let's assume you create a GitHub
folder under your Documents folder and a SWHS-PC folder under that. Thus, the full
path will be something like:

    C:\Users\YourName\GitHub\SWHS-PC

Cloning the Users repository will create a Users folder under SWHS-PC.

Open a PowerShell window (or command prompt). The quickest way to do this is to
right-click the Start menu and choose "Windows PowerShell" from the popup menu.

At the PowerShell prompt, change to the directory you created earlier by typing
the following command and pressing ENTER:

    cd .\GitHub\SWHS-PC

Note: You can specify either an absolute path or a relative path with the cd command. 
The above example uses a relative path, which means the "." stands for whatever the 
current directory is. If the current directory is **C:\Users\YourName** then the above 
command changes to **C:\Users\YourName\GitHub\SWHS-PC**. You can also use ".." to 
represent the parent of a directory, so the command **cd ..** means go up one level.

Without closing PowerShell, open a web browser, log in to github, and navigate 
to the [page for this repository](https://github.com/SWHS-PC/Users).

Click the green "Clone or download button" and copy the URL from the box that
pops up. It should look something like https://github.com/SWHS-PC/Users.git.

Switch back to the PowerShell window and enter the following command (replacing
the URL with the one you copied from github):

    git clone https://github.com/SWHS-PC/Users.git

That's it! You've cloned the repo, and there should now be a Users folder under
the current (SWHS-PC) folder.

## Create a Topic Branch

## Create a "Hello World" Program

## Add, Commit, and Push your Changes

## Create a Pull Request


