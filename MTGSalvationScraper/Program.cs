/*
 * No GoF patterns or IoC containers here...
 * 
 * Sooner or later I'll clean it up and make give it a proper UI since it seems MTGSalvation
 * is going to keep the current spoiler format (Something I didn't think was possible when I wrote this one night)
 * 
 * The only two "coding conventions" are: 
 * 
 *  Use interfaces where possible...
 *  
 *  ...And don't use the horrible nesting and giant functions you find in this file.
 *  I can identify at least 3 interfaces that should be extracted from Main() alone, don't replicate or extend that.
 *  (And if anyone ever need proof of the limitations of code metrics, Visual Studio rates every part of this project has having very high maintainability)
 * 
 * Any further changes to the code should be tested, I'll be implementing unit tests for most of the codebase.
 * 
 * To justify the work related in the changes mentioned above I want to extend the functionality of the program past spoilers (which are of a highly "seasonal" need).
 * 
 * I plan to allow for easier management of:
 *  Custom Cards
 *  Custom Images
 *  Sharing Cards
 *  Multiple Games (Hearthstone, Yu-Gi-Oh, etc.)
 *  Multiple Clients (including my client "Basilisk", which contrary to appearances is still in development)
 *  Supporting Gatherer and MTGJson
 */

#define SUMMER_MAGIC //Remove this for older cockatrice versions
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using MTGSalvationScraper;
using MTGSalvationScraper.Properties;
using TinyIoC;

namespace MTGSalvationScraper
{
    internal class Program
    {

        private const string XmlExtension = ".xml";

        private static void Main(string[] argStrings)
        {
            var parameters = new Parameters(argStrings);
            parameters.TryGetNamedArgument("noprompt");
            
            var program = new Program();
            program.SetupIoC(Settings.Default, new CockatriceFileFormat(Settings.Default,parameters,true));
            try
                //Not using UnhandledException event since it still shows an error dialogue, which most users will use to close the application, making the error message useless.
            {
                Console.SetWindowSize(Resources.IntroString.Length, 10);
                Console.WriteLine(Resources.IntroString);
#if SUMMER_MAGIC
                const string versionName = "Summer Magic";
#else
                
                const string versionName = "Versions /before/ Summer Magic";
#endif
                Console.WriteLine(Resources.VersionReminderPrompt, versionName);
                var path = program.GetCardFilePath();
                PreviousRunCardFileLocatorSource.SetPreviousRunLocation(path);
                try
                {
                    if (!ConfirmRestoreMode(path))
                    {
                        var newCards = UpdateFile(path,null,null);
                        Console.WriteLine(Resources.UpdateSuccessPrompt, newCards);
                    }
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
                Console.WriteLine(Resources.InternalErrorPrompt, exception);
                Console.ReadKey();
            }
        }

        public void SetupIoC(Settings settings, IFileFormat fileFormat)
        {

            var validExtensions = settings.CockatriceCardFileExtension.Cast<string>();
            var validNames = settings.DefaultCardFileNames.Cast<string>();

            var ioCContainer = TinyIoCContainer.Current;

            ioCContainer.Register<ICardFileLocationValidator>(new DefaultCardFileLocationValidator());
            
            ioCContainer.Register<IFileFormat>(fileFormat);
            ioCContainer.Register<ILogger>(new ConsoleLogger());

        }
        
        public string GetCardFilePath()
        {
            var validator = TinyIoCContainer.Current.Resolve<ICardFileLocationValidator>();
            var logger = TinyIoCContainer.Current.Resolve<ILogger>();
            //Allow lazy initialization of the container's resolution by using index and foreach (instead of ToArray/ToList and for, or Select)
            var cardFileLocatorIndex = 0; 
            foreach (var cardFileLocator in TinyIoCContainer.Current.Resolve<IFileFormat>().FileLocatorSources)
            {
                foreach (var source in TinyIoCContainer.Current.ResolveAll<ICardFileLocatorSource>())
                {

                    logger.Log(LogLevel.Info, "Searching {0} for Card File using Method #{1}: {2}", source.SourceName,
                        cardFileLocatorIndex, source.SourceDirectory);

                    string path;
                    cardFileLocator.TryFindCardFile(source, out path);

                    if (!validator.IsValidCardFileLocation(path)) continue;

                    logger.Log(LogLevel.Info, "Found valid card file at {0}", path);
                    return path;
                }
                cardFileLocatorIndex++;
            }

            throw new ScraperException("Could not find valid card file location");
        }

        public static int UpdateFile(string path,IMagicSet set,IFileFormat fileFormat)
        {
            var dataProvider = set.SetDataProvider;
            var parser = new MtgSalvationCardDataParserV2();
            var modifier = fileFormat.FileModifier;

            string oldFile;
            try
            {
                oldFile = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                throw new ScraperException("Could not read cards.xml file.", ex);
            }
            int newCards;
            var setName = Settings.Default.LongSetName;
            Console.WriteLine(Resources.FetchingDataPrompt);
            Console.WriteLine(Resources.CardsParsingPrompt);
            var parsedCards = dataProvider.GetCardElements();
            var numNewCards = parsedCards.Count;
            Console.WriteLine(Resources.CardsParsedPrompt, numNewCards);
            Console.WriteLine(Resources.GeneratingCardsPrompt);
            var newXmlFileString = modifier.AugmentCards(setName, setName, oldFile, parsedCards);
            Console.WriteLine(Resources.GeneratedCardsPrompt);
            var newFile = newXmlFileString;
            try
            {
                File.WriteAllText(path, newFile);
            }
            catch (Exception ex)
            {
                throw new ScraperException(
                    "Could not write cards.xml file to disk. Try running the program as an administrator and make sure no other programs are accessing it.",
                    ex);
            }
            throw new NotImplementedException();
            return newCards;
        }

        private static bool ConfirmRestoreMode(string path)
        {
            const string backupPrefix = "bak";
            var backupPath = string.Format("{0}{1}", path, backupPrefix);
            if (!File.Exists(backupPath))
            {
                Console.WriteLine(Resources.BackupPrompt, backupPath);
                File.Copy(path, backupPath);
                return false;
            }
            else
            {
                Console.WriteLine(Resources.RestoreBackupReminder, backupPath,
                    Path.ChangeExtension(backupPath, XmlExtension));
                const string restoreString = "y";
                Console.WriteLine(Resources.RestoreXmlPrompt, restoreString);
                if (Console.ReadLine() != restoreString) return false;

                byte[] backupBytes;
                try
                {
                    backupBytes = File.ReadAllBytes(backupPath);
                }
                catch (Exception exception)
                {
                    throw new ScraperException("Couldn't from read backup file.", exception);
                }
                File.WriteAllBytes(path, backupBytes);
                Console.WriteLine(Resources.RestoreCompleteRestored);
            }
            return true;
        }
    }
}