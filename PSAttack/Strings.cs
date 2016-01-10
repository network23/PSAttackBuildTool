using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAttack.Utils;

namespace PSAttack
{
    class Strings
    {
        public static string version = "0.2.0-beta";
        public static string githubUserAgent = "PSAttack";
        public static string punchURL = "https://api.github.com/repos/jaredhaight/pspunch/releases";
        public static string punchUnzipDir = Path.Combine(PSAUtils.GetPSAttackDir(), "PSPunch");
        public static string punchZipPath = Path.Combine(PSAUtils.GetPSAttackDir(), "PSPunch.zip");
        public static string moduleSrcDir = Path.Combine(PSAUtils.GetPSAttackDir(), "ModuleSrc");
        public static string punchModulesDir = "PSPunch\\Modules\\";
        public static string punchResourcesDir = "PSPunch\\Resources\\";
        public static string punchCSProjFile = "PSPunch\\PSPunch.csproj";
        public static string punchBuildDir = Path.Combine(PSAUtils.GetPSAttackDir(), "PSPunchBuild");
        public static List<string> psaLogos = new List<string>()
        {
@"
  _____   _______       _______ _______       _____ _  __
 |  __ \ / ____\ \   /\|__   __|__   __|/\   / ____| |/ /
 | |__) | (___  \ \ /  \  | |     | |  /  \ | |    | ' / 
 |  ___/ \___ \  > / /\ \ | |     | | / /\ \| |    |  <  
 | |     ____) |/ / ____ \| |     | |/ ____ | |____| . \ 
 |_|    |_____//_/_/    \_|_|     |_/_/    \_\_____|_|\_\
",
@"
 (   (                                           )  
 )\ ))\ )   (      *   ) *   )   (       (    ( /(  
(()/(()/(   )\   ` )  /` )  /(   )\      )\   )\()) 
 /(_)/(_)((((_)(  ( )(_)( )(_)((((_)(  (((_)|((_)\  
(_))(_))__)\ _ )\(_(_()(_(_()) )\ _ )\ )\___|_ ((_) 
| _ / __\ (_)_\(_|_   _|_   _| (_)_\(_((/ __| |/ /  
|  _\__ \> / _ \   | |   | |    / _ \  | (__  ' <   
|_| |___/_/_/ \_\  |_|   |_|   /_/ \_\  \___|_|\_\  
",
@"
   ___ ____    _  _____ _____  _     ___      
  / _ / _\ \  /_\/__   /__   \/_\   / __\/\ /\
 / /_)\ \ \ \//_\\ / /\/ / /\//_\\ / /  / //_/
/ ___/_\ \/ /  _  / /   / / /  _  / /__/ __ \ 
\/    \__/_/\_/ \_\/    \/  \_/ \_\____\/  \/ 
                                              
",
@"
           __                                       
 _____ ____\ \  _____ _____ _____ _____ _____ _____ 
|  _  |   __\ \|  _  |_   _|_   _|  _  |     |  |  |
|   __|__   |> |     | | |   | | |     |   --|    -|
|__|  |_____/ /|__|__| |_|   |_| |__|__|_____|__|__|
           /_/                                      

",
@"
   ___  _____   ___ _______________  _______ __
  / _ \/ __\ \ / _ /_  __/_  __/ _ |/ ___/ //_/
 / ____\ \  > / __ |/ /   / / / __ / /__/ ,<   
/_/  /___/ /_/_/ |_/_/   /_/ /_/ |_\___/_/|_|  
                                               

",
@"
 ######   #####  #       #    ####### #######    #     #####  #    # 
 #     # #     #  #     # #      #       #      # #   #     # #   #  
 #     # #         #   #   #     #       #     #   #  #       #  #   
 ######   #####     # #     #    #       #    #     # #       ###    
 #             #   #  #######    #       #    ####### #       #  #   
 #       #     #  #   #     #    #       #    #     # #     # #   #  
 #        #####  #    #     #    #       #    #     #  #####  #    # 
                                                                     
",
@"
 ____    ____     __    ______  ______ ______ ______  ____    __  __     
/\  _`\ /\  _`\  /\ `\ /\  _  \/\__  _/\__  _/\  _  \/\  _`\ /\ \/\ \    
\ \ \L\ \ \,\L\_\\ `\ `\ \ \L\ \/_/\ \\/_/\ \\ \ \L\ \ \ \/\_\ \ \/'/'   
 \ \ ,__/\/_\__ \ `\ >  \ \  __ \ \ \ \  \ \ \\ \  __ \ \ \/_/\ \ , <    
  \ \ \/   /\ \L\ \ /  / \ \ \/\ \ \ \ \  \ \ \\ \ \/\ \ \ \L\ \ \ \\`\  
   \ \_\   \ `\____/\_/   \ \_\ \_\ \ \_\  \ \_\\ \_\ \_\ \____/\ \_\ \_\
    \/_/    \/_____\//     \/_/\/_/  \/_/   \/_/ \/_/\/_/\/___/  \/_/\/_/

",
@"
  _   __       ___ ___      _    
 |_) (_  \  /\  |   |  /\  /  |/ 
 |   __) / /--\ |   | /--\ \_ |\ 
                                 
",
@"
 ______   ______    _______ _______ _______ _______ _______ _     _ 
(_____ \ / _____)_ (_______(_______(_______(_______(_______(_)   | |
 _____) ( (____ ( \ _______    _       _    _______ _       _____| |
|  ____/ \____ \ ) |  ___  |  | |     | |  |  ___  | |     |  _   _)
| |      _____) (_/| |   | |  | |     | |  | |   | | |_____| |  \ \ 
|_|     (______/   |_|   |_|  |_|     |_|  |_|   |_|\______|_|   \_)
                                                                    
",
@"
      ___      ___         ___                          ___         ___         ___     
     /  /\    /  /\       /  /\        ___      ___    /  /\       /  /\       /__/|    
    /  /::\  /  /:/_     /  /::\      /  /\    /  /\  /  /::\     /  /:/      |  |:|    
   /  /:/\:\/  /:/ /\   /  /:/\:\    /  /:/   /  /:/ /  /:/\:\   /  /:/       |  |:|    
  /  /:/~/:/  /:/ /::\ /  /:/~/::\  /  /:/   /  /:/ /  /:/~/::\ /  /:/  ___ __|  |:|    
 /__/:/ /:/__/:/ /:/\:/__/:/ /:/\:\/  /::\  /  /::\/__/:/ /:/\:/__/:/  /  //__/\_|:|____
 \  \:\/:/\  \:\/:/~/:\  \:\/:/__\/__/:/\:\/__/:/\:\  \:\/:/__\\  \:\ /  /:\  \:\/:::::/
  \  \::/  \  \::/ /:/ \  \::/    \__\/  \:\__\/  \:\  \::/     \  \:\  /:/ \  \::/~~~~ 
   \  \:\   \__\/ /:/   \  \:\         \  \:\   \  \:\  \:\      \  \:\/:/   \  \:\     
    \  \:\    /__/:/     \  \:\         \__\/    \__\/\  \:\      \  \::/     \  \:\    
     \__\/    \__\/       \__\/                        \__\/       \__\/       \__\/    

",
@"
   _ \    ___| \ \     \     |    |                 |     | 
  |   | \___ \  \ \   _ \    __|  __|   _` |   __|  |  /  | 
  ___/        |   /  ___ \   |    |    (   |  (       <  _| 
 _|     _____/  _/ _/    _\ \__| \__| \__,_| \___| _|\_\ _) 
                                                            
"
        };
        public static string psaWarning = @"
 ############################################################
 #                                                          #
 #     PLEASE NOTE: This is a beta release of PS>Attack     #
 #   Things might be buggy. If you find something that's    #
 #             broken please submit an issue at             #
 #      https://github.com/jaredhaight/psattack/issues      #
 #        or even better, submit a pull request! :-D        #
 #                                                          #
 #         For more info view the release notes at          #
 #   https://www.github.com/jaredhaight/psattack/releases   #
 #                                                          #
 ############################################################
";
        public static string psaStartMsg = @"
 PS>Attack downloads a copy of PS>Punch, downloads the latest 
 versions of the files in modules.json, encrypts them and then
 compiles PS>Punch with these new and unique files. Antivirus
 software (including Windows Defender) may flag the downloaded
 files as malicious. Future error handling will alert to this
 fact. In the mean time you'll have to configure your AV soft
 ware to allow these actions.

 As part of the build process, PS>Attack relies on a full
 install of .NET 3.5. Targeting 3.5 allows PS>Punch to work
 on Windows 7 and up. If you encounter build errors, the first
 thing you should do is make sure you have the full version of
 .NET 3.5 installed. Google (or Duck Duck Go, or Bing, etc) is
 your friend.

 Press enter to start PS>Attack.

";
        public static string psaEndSuccess = @"
 Build complete! Your build of PS>Punch is available at: 

 {0}

 You'll need the PSPunch.exe and PSPunch.exe.config files, the 
 others are extra from the build process. PSPunch.exe should be
 run from the same folder that has that confif

 Press return to open up the folder. Thanks for using PS>Attack!
";
        public static string psaEndNoMSBuild = @"
 Hrm.. we couldn't find MSBuild.exe. That _should_ be here:

 {0}

 At least, that's where we were Windows is telling us your
 .NET install is (which should contain MSBuild.exe). Get
 MSBuild and then try again!

 Press return to close this window.
 ";
        public static string psaEndFailure = @"
 Oh no! It looks like the build failed. You should check the build
 output above and see if there's an obvious issue. If you can't 
 resolve the problem on your own, go ahead and submit an issue  at
 https://github.com/jaredhaight/psattack/issues/ and maybe we can 
 help. Make sure to include the output from the build process (the
 gray text after '[*] Building PSPunch..' upto this error message)

 Press return to close this window.
 ";
    }
}

