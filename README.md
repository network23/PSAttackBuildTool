PS>Attack
=============

PS>Attack is a platform designed to generate a portable powershell attack environment. The generated environment is a self contained exe and comes with a lot of the latest and greatest Offensive PowerShell tools. 

It also uses three big tricks to evade Antivirus and Incident Response teams.

1) It does not call "powershell.exe", instead recreating a powershell environment by calling the .NET framework directly.
2) Modules are embeded in the exe as encrypted blobs and then decrypted into to memory. Unencrypted tools never touch disk.
3) A new key is used to generate the encrypted blobs is created everytime the enviroment is created.

The client that is generated is seeded from the [PSPunch repo](https://www.github.com/jaredhaight/PSPunch/).