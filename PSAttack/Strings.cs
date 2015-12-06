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
    }
}
