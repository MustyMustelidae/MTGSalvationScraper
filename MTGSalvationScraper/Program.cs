#define SUMMER_MAGIC 
using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using MTGSalvationScraper.Properties;

namespace MTGSalvationScraper
{
    class Program
    {
        static void Main()
        {
            try //Not using UnhandledException event since it still shows an error dialogue, which most users will use to close the application, making the error message useless.
            {
                Console.SetWindowSize(Resources.IntroString.Length, 10);
                Console.WriteLine(Resources.IntroString);

                var settingsPath = Settings.Default.DefaultCardFileLocation;

                var regKey = Settings.Default.CockatriceRegistryKey;
                var regValue = Settings.Default.CockatriceRegistryValue;
                var path = Registry.GetValue(regKey, regValue, settingsPath) as string;
              
                
                if (!string.IsNullOrWhiteSpace(path))
                {
                    path = path.Replace("/", "\\").
                        Replace("\"", string.Empty)
                        .Trim();

                    if (!File.Exists(path))
                    {
                        const string xmlExtension = ".xml";
                        var searchPath = Directory.Exists(path) ? path : Directory.GetCurrentDirectory();

                        var xmlFiles = Directory.GetFiles(searchPath)
                            .Where(filePath => Path.GetExtension(filePath) == xmlExtension)
                            .ToList();
                        xmlFiles.RemoveAll(filePath => !Settings.Default.DefaultCardFileNames.Contains(filePath));
                        var defaultPath = xmlFiles
                            .FirstOrDefault(filePath => Settings.Default.DefaultCardFileNames
                                .Contains(Path.GetFileName(filePath)));

                        path = defaultPath ?? xmlFiles.FirstOrDefault();
                    }
                }
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                   
                        Console.WriteLine(Resources.CardFilePromptString);
                        path = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    {
                        Console.WriteLine(Resources.InvalidLocationMessage);
                        Console.ReadLine();
                        return;
                    }
                }
                try
                {
                    var newCards = UpdateFile(path);
                    Console.WriteLine(Resources.UpdateSuccessPrompt, newCards);
                }
                catch (ScraperException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.FatalErrorPrompt, ex.Message);
                    throw;
                }

                Console.ReadKey();
            }
            catch (Exception exception)
            {
                Console.WriteLine(Resources.InternalErrorPrompt,exception);
                Console.ReadKey();
            }
        }

        public static int UpdateFile(string path)
        {
            var dataProvider = new MtgSalvationCardDataProvider();
            var parser = new MtgSalvationCardDataParser();
#if SUMMER_MAGIC
            var modifier = new SummerMagicCardFileModifier();
#else
            var modifier = new CockatriceCardFileModifier();
#endif
           
            var fileGenerator = new CardFileGenerator(dataProvider, parser, modifier);
            string oldFile;
            try
            {
                oldFile = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                throw new ScraperException("Could not read cards.xml file.",ex);
            }
            int newCards;
            var setName = Settings.Default.SetName;
            var longSetName = Settings.Default.LongSetName;
            var newFile = fileGenerator.GenerateCardFile(oldFile, setName, longSetName, out newCards);
            try
            {
                const string backupPrefix = "bak";
                var backupPath = string.Format("{0}{1}", path, backupPrefix);
                if (!File.Exists(backupPath))
                {
                    File.Copy(path, backupPath);
                }
                File.WriteAllText(path, newFile);
             }
             catch (Exception ex)
             {
                 throw new ScraperException("Could not write new cards.xml file to disk.", ex);
             }
            return newCards;
        }

    }
}
