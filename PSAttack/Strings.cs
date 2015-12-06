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
        public static string githubUserAgent = "PSAttack";
        public static string punchURL = "https://api.github.com/repos/jaredhaight/pspunch/releases";
        public static string punchUnzipDir = Path.Combine(PSAUtils.GetPSAttackDir(),"PSPunch");
        public static string punchZipPath = Path.Combine(PSAUtils.GetPSAttackDir(), "PSPunch.zip");
        public static string moduleSrcDir = Path.Combine(PSAUtils.GetPSAttackDir(), "ModuleSrc");
        public static string punchModulesDir = "PSPunch\\Modules\\";
        public static string punchResDir = "PSPunch\\Resources\\";
        public static string punchBuildDir = Path.Combine(PSAUtils.GetPSAttackDir(), "PSPunchBuild");
        public static string psaLogo = @"

__________  _________     /\    _____   __    __                 __    
\______   \/   _____/    / /   /  _  \_/  |__/  |______    ____ |  | __
 |     ___/\_____  \    / /   /  /_\  \   __\   __\__  \ _/ ___\|  |/ /
 |    |    /        \  / /   /    |    \  |  |  |  / __ \\  \___|    < 
 |____|   /_______  / / /    \____|__  /__|  |__| (____  /\___  >__|_ \
                  \/  \/             \/                \/     \/     \/

";

        public static string psaWarning = @"
 ############################################################
 #                                                          #
 #    PLEASE NOTE: This is an alpha release of PS>Attack    #
 # There are plenty of bugs and not a lot of functionality. # 
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
        public static string psaEndMsg = @"
 Build complete! Some warning messages are expected from the compiler 
 so don't be alarmed. If there are errors running your build of 
 PS>Punch, please submit an issue on github referencing the errors
 that came up during the build.

 Your build of PS>Punch is available at: 

 {0}

 You only need the PSPunch.exe file, the others are extra from the
 build process.

 Press return to open up the folder. Thanks for using PS>Attack!
";
    }
}
