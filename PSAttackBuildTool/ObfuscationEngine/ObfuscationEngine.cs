using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSAttackBuildTool;
using PSAttackBuildTool.Utils;
using PSAttackBuildTool.PSAttack;

namespace PSAttackBuildTool.ObfuscationEngine
{
    class Rule
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Trigger { get; set; }
        public string Action { get; set; }
    }

    class Ruleset
    {
        public string Name { get; set;  }
        public string FileName { get; set; }
        public string Type { get; set; }
        public List<Rule> Rules { get; set; }
    }
    class ObfuscationEngine
    {
        public List<Ruleset> Rulesets { get; set; }
        public bool RandomizeText { get; set; }
        public Random rand { get; set; }
        public Dictionary<String,String> VariableKey { get; set; }

        public string ProcessSource(Display display, string sourcePath, GeneratedStrings generatedStrings, Attack attack)
        {
            string originalFileName = Path.GetFileName(sourcePath);
            string readScript = File.ReadAllText(sourcePath);
            string obfuscatedSourcePath = sourcePath.Replace(attack.unzipped_dir, Strings.obfuscatedSourceDir);
            string fileStub = obfuscatedSourcePath.Replace(Strings.obfuscatedSourceDir, "");
            obfuscatedSourcePath = Path.Combine(Strings.obfuscatedSourceDir, fileStub.Replace("PSAttack", generatedStrings.Store["psaReplacement"]));
            obfuscatedSourcePath = obfuscatedSourcePath.Replace("AttackState", generatedStrings.Store["attackStateReplacement"]);
            obfuscatedSourcePath = obfuscatedSourcePath.Replace("PSParam", generatedStrings.Store["psparamReplacement"]);

            // Process Rules
            foreach (Ruleset ruleset in this.Rulesets)
            {
                if (ruleset.Type == "SourceCode")
                {
                    if (originalFileName.ToLower().Contains(ruleset.FileName.ToLower()) || ruleset.FileName == "#ALL")
                    {
                        FileInfo file = new System.IO.FileInfo(obfuscatedSourcePath);
                        file.Directory.Create();
                        if (!(sourcePath.Contains(generatedStrings.Store["keyStoreFileName"]) || (sourcePath.Contains("Modules"))))
                        {
                            foreach (Rule rule in ruleset.Rules)
                            {
                                display.updateMessage("Running Replace Rule '" + rule.Name + "'");
                                readScript = RuleProcessor(display, rule, readScript);
                            }
                            File.WriteAllText(obfuscatedSourcePath, readScript);
                        }
                        else
                        {
                            File.Copy(sourcePath, obfuscatedSourcePath, true);
                        }
                    }
                }

            }
            return obfuscatedSourcePath;
        }

        public string ProcessScript(Display display, string scriptPath)
        {
            string originalFileName = Path.GetFileName(scriptPath);
            string obfuscatedScriptPath = Path.Combine(Strings.obfuscatedScriptsDir, Path.GetFileName(scriptPath));
            string readScript = File.ReadAllText(scriptPath);
            string modifiedScript = "";
           
            // Process Rules
            foreach (Ruleset ruleset in this.Rulesets)
            {
                if (ruleset.Type == "PowerShell")
                {
                    if (originalFileName.ToLower().Contains(ruleset.FileName.ToLower()) || ruleset.FileName == "#ALL")
                    {
                        foreach (Rule rule in ruleset.Rules)
                        {
                            readScript = RuleProcessor(display, rule, readScript);
                        }
                    }
                }

            }


            // Implicit Randomize Text Case Rule
            if (this.RandomizeText == true)
            {
                display.updateStatus("Running Rule: Randomize Text Case");
                Random rand = new Random();
                foreach (char c in readScript.ToCharArray())
                {
                    int i = rand.Next(int.MaxValue) % 2;
                    char modifiedChar = new Char();
                    modifiedChar = Char.ToLower(c);
                    if (i == 1)
                    {
                        modifiedChar = Char.ToUpper(c);

                    }
                    modifiedScript += modifiedChar;
                }
            }
            else
            {
                modifiedScript = readScript;
            }

            File.WriteAllText(obfuscatedScriptPath, modifiedScript);
            return obfuscatedScriptPath;
        }

        public string RuleProcessor(Display display, Rule rule, String scriptContents)
        {
            string modifiedContents = "";
            if (rule.Type == "replace")
            {
                display.updateMessage("Running Replace Rule '" + rule.Name + "'");
                Regex regex = new Regex(rule.Trigger, RegexOptions.IgnoreCase);
                string replacementText = rule.Action;

                if (rule.Action.Contains("#RANDOM"))
                {
                    replacementText = rule.Action.Replace("#RANDOM", PSABTUtils.RandomString(16, this.rand));
                }
                display.updateSecondaryMessage("Replacing " + rule.Trigger + " with " + replacementText);
                modifiedContents = regex.Replace(scriptContents, replacementText);
            }

            
            if (rule.Type == "ReplaceList")
            {
                display.updateMessage("Running ReplaceList Rule '" + rule.Name + "'");
                this.VariableKey = new Dictionary<string, string>();
                Regex regex = new Regex(rule.Trigger, RegexOptions.IgnoreCase);
                List<string> safeVars = new List<string>(new string[] {"true", "false", "null", "error" });
                string replacementText = rule.Action;
                Match hit = regex.Match(scriptContents);
                modifiedContents = scriptContents;
                while (hit.Success)
                {
                    if (safeVars.Contains(hit.Value.Replace("$","").ToLower()))
                    {
                        replacementText = null;
                    }
                    else if (this.VariableKey.ContainsKey(hit.Value))
                    {
                        display.updateSecondaryMessage("Found hit for key:" + hit.Value);
                        replacementText = this.VariableKey[hit.Value];
                    }
                    else
                    {
                        display.updateSecondaryMessage("Creating new string for key:" + hit.Value);
                        replacementText = rule.Action.Replace("#RANDOM", PSABTUtils.RandomString(32, this.rand));
                        this.VariableKey.Add(hit.Value, replacementText);
                    }
                    if (replacementText != null)
                    {
                        display.updateSecondaryMessage("Replacing " + hit.Value + " with " + replacementText);
                        string variable_match = @"(\$)" + hit.Value.Replace("$", "");
                        Regex regex_step2 = new Regex(variable_match, RegexOptions.IgnoreCase);
                        modifiedContents = regex_step2.Replace(modifiedContents, replacementText);
                    }
                    else {
                        display.updateSecondaryMessage("Safe variable  " + hit.Value + " found. Not replacing.");
                    }
                    hit = hit.NextMatch();
                }

            }
            return modifiedContents;
        }

        public ObfuscationEngine(GeneratedStrings generatedStrings)
        {
            this.RandomizeText = false;
            this.rand = new Random();
            this.Rulesets = new List<Ruleset>();

            // Mimikatz Rules
            Ruleset mimikatz = new Ruleset { Name = "Mimikatz", Type="PowerShell", FileName = "invoke-mimikatz.ps1", Rules = new List<Rule>() };
            mimikatz.Rules.Add(new Rule { Name = "'-mimikatz' Replace", Type = "replace", Trigger = "-mimikatz", Action = "-#RANDOM" });
            mimikatz.Rules.Add(new Rule { Name = "' mimikatz' Replace", Type = "replace", Trigger = " mimikatz", Action = " #RANDOM" });

            // General Rules
            Ruleset general = new Ruleset { Name = "General", Type = "PowerShell", FileName = "#ALL", Rules = new List<Rule>() };
            general.Rules.Add(new Rule { Name = "Variable Radomization", Type = "ReplaceList", Trigger = @"(\$)[A-Za-z\d]+\b", Action = "$#RANDOM" });

            // C# Rules
            Ruleset psaSource = new Ruleset { Name = "PSAttack", Type = "SourceCode", FileName = "#ALL", Rules = new List<Rule>() };
            string psaReplace = PSABTUtils.RandomString(this.rand.Next(6,15), this.rand);
            generatedStrings.Store.Add("psaReplacement", psaReplace);
            string attackStateReplacement = PSABTUtils.RandomString(this.rand.Next(6, 15), this.rand);
            generatedStrings.Store.Add("attackStateReplacement", attackStateReplacement);
            string psparamReplacement = PSABTUtils.RandomString(this.rand.Next(6, 15), this.rand);
            generatedStrings.Store.Add("psparamReplacement", psparamReplacement);
            psaSource.Rules.Add(new Rule { Name = "'PSAttack' Replace", Type = "replace", Trigger = "PSAttack", Action = psaReplace });
            psaSource.Rules.Add(new Rule { Name = "'PS>Attack' Replace", Type = "replace", Trigger = "PS>Attack", Action = psaReplace });
            psaSource.Rules.Add(new Rule { Name = "'attackState' Replace", Type = "replace", Trigger = "attackState", Action = attackStateReplacement });
            psaSource.Rules.Add(new Rule { Name = "'psparam' Replace", Type = "replace", Trigger = "psparam", Action = psparamReplacement });
            psaSource.Rules.Add(new Rule { Name = "'psaLogos' Replace", Type = "replace", Trigger = "psaLogos", Action = PSABTUtils.RandomString(this.rand.Next(8, 14), this.rand) });
            //this.Rulesets.Add(mimikatz);
            //this.Rulesets.Add(general);
            this.Rulesets.Add(psaSource);

        }
    }
}
