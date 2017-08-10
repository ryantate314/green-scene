using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GreenScene.Web.Controllers
{
   [Route("api/directorysearch")]
   public class DirectoryController : Controller
   {
      private IConfiguration _config;
      private string _rootDirectory;

      public DirectoryController(IConfiguration configuration)
      {
         _config = configuration;
         _rootDirectory = _config["ResourceDirectory"];
      }

      /// <summary>
      /// Returns a list of files and folders underneath the specified directory. Note
      /// this is combined with the configured root file directory.
      /// </summary>
      /// <param name="term"></param>
      /// <returns></returns>
      [HttpGet("")]
      public IEnumerable<string> Get(string term)
      {
         //if (term.Contains(".."))
         //{
         //   throw new InvalidOperationException("Cannot access specified directory.");
         //}
         IEnumerable<string> filesAndDirectories = null;
         try
         {
            filesAndDirectories = GetMatches(term);
         }
         catch (Exception)
         {
            
         }

         return filesAndDirectories;
      }

      /// <summary>
      /// Removes directory traversal attacks. Converts to platform directory
      /// separators.
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      private string SanitizePath(string path)
      {
         if (String.IsNullOrEmpty(path))
         {
            return Path.DirectorySeparatorChar.ToString();
         }
         path = path
               .Replace('\\', Path.DirectorySeparatorChar)
               .Replace('/', Path.DirectorySeparatorChar)
               .Replace("..", "")
               .Trim();

         string regex = $"^(\\{Path.DirectorySeparatorChar}?)(([a-zA-Z0-9\\-_ \\.]+)(\\{Path.DirectorySeparatorChar}[a-zA-Z0-9\\-_ \\.]+)*\\{Path.DirectorySeparatorChar}?)?$";
         System.Diagnostics.Debug.WriteLine(path);
         System.Diagnostics.Debug.WriteLine(regex);
         //if (!Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
         if (!System.Text.RegularExpressions.Regex.IsMatch(path, regex))
         {
            throw new Exception("Invalid path.");
         }
         //if (!String.IsNullOrEmpty(path) && path[0] != Path.DirectorySeparatorChar)
         //{
         //   path = Path.DirectorySeparatorChar + path;
         //}
         return path;
      }

      private IEnumerable<string> GetMatches(string path)
      {
         path = SanitizePath(path);
         string search = String.Empty;
         if (!String.IsNullOrEmpty(path))
         {
            //Check if there is a search term on the end of the path
            if (path.Last() != Path.DirectorySeparatorChar)
            {
               int lastSeparatorPos = path.LastIndexOf(Path.DirectorySeparatorChar);
               //Use the entire string for a search term if no separators are found
               search = path.Substring(lastSeparatorPos + 1);
               lastSeparatorPos = lastSeparatorPos == -1 ? 0 : lastSeparatorPos;
               path = path.Substring(0, lastSeparatorPos);//Will set to empty if index of 0
            }
         }
         string absolutePath = GenerateAbsolutePath(path);
         IEnumerable<string> directories = Directory.EnumerateDirectories(absolutePath);
         IEnumerable<string> files = Directory.EnumerateFiles(absolutePath);

         //Combine files and directories.
         IEnumerable<string> filesAndDirectories = directories.Concat(files);
            
         //Apply search term if any
         if (!String.IsNullOrEmpty(search))
         {
            filesAndDirectories = filesAndDirectories.Where(x =>
            {
               //Get last chunk of text
               int lastSlashIndex = x.LastIndexOf(Path.DirectorySeparatorChar);
               lastSlashIndex = lastSlashIndex == -1 ? 0 : lastSlashIndex;
               if (lastSlashIndex == x.Length - 1) { return false; }

               return x.Substring(lastSlashIndex + 1).StartsWith(search);
            });
         }

         filesAndDirectories = filesAndDirectories
            .Select(x => MakeRelativePath(x))
            .OrderBy(x => x);

         return filesAndDirectories;
      }

      private string GenerateAbsolutePath(string partialPath)
      {
         if (!String.IsNullOrEmpty(partialPath))
         {
            //If begins with directory separator, the Path.Combine function
            //thinks it's an absolute path. Remove it to force relative.
            if (partialPath[0] == Path.DirectorySeparatorChar)
            {
               partialPath = partialPath.Substring(1);
            }
         }

         //Assume config file has been configured with native directory separators
         string fullPath = Path.Combine(_rootDirectory, partialPath);
         return fullPath;
      }

      /// <summary>
      /// Creates a relative path from one file or folder to another.
      /// https://stackoverflow.com/a/340454
      /// </summary>
      /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
      /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
      /// <exception cref="ArgumentNullException"></exception>
      /// <exception cref="UriFormatException"></exception>
      /// <exception cref="InvalidOperationException"></exception>
      public String MakeRelativePath(String toPath)
      {
         //return toPath.Substring(_rootDirectory.Length);
         string fromPath = _rootDirectory;

         if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

         Uri fromUri = new Uri(fromPath);
         Uri toUri = new Uri(toPath);

         if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

         Uri relativeUri = fromUri.MakeRelativeUri(toUri);
         String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

         if (toUri.Scheme.Equals("file", StringComparison.CurrentCultureIgnoreCase))
         {
            relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
         }

         return relativePath;
      }
   }
}