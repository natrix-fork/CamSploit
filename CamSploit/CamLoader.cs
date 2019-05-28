﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace ExploitMaker.Cam
{
    public static class CamLoader
    {
        public static IEnumerable<Camera> LoadFromTextFile(string filePath)
        {
            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), filePath);
            
            using (var file = new StreamReader(filePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var splitted = line.Split(':');

                    var cam = new Camera(splitted[0], splitted[1]);
                    yield return cam;
                }

                file.Close();
            }
        }

        public static IEnumerable<Camera> LoadFromShodanJsonFile(string filePath)
        {
            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), filePath);
            
            using (var fileReader = new StreamReader(filePath))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    dynamic json = JObject.Parse(line);

                    var cam = new Camera(json.http.host.ToString(), json.port.ToString())
                    {
                        Country = json.location.country_name,
                        City = json.location.city,
                        Description = json.title
                    };

                    yield return cam;
                }

                fileReader.Close();
            }
        }

        public static IEnumerable<Camera> LoadFromHost(string ipPort)
        {
            yield return new Camera(ipPort.Split(':')[0], ipPort.Split(':')[1]);
        }
    }
}