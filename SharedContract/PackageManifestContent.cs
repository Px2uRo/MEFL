using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContract
{
    internal class PackageManifestContent
    {
        public class Rootobject
        {
            public Arguments arguments { get; set; }
            public Assetindex assetIndex { get; set; }
            public string assets { get; set; }
            public int complianceLevel { get; set; }
            public Downloads downloads { get; set; }
            public string id { get; set; }
            public Javaversion javaVersion { get; set; }
            public Library[] libraries { get; set; }
            public Logging logging { get; set; }
            public string mainClass { get; set; }
            public int minimumLauncherVersion { get; set; }
            public DateTime releaseTime { get; set; }
            public DateTime time { get; set; }
            public string type { get; set; }
        }

        public class Arguments
        {
            public object[] game { get; set; }
            public object[] jvm { get; set; }
        }

        public class Assetindex
        {
            public string id { get; set; }
            public string sha1 { get; set; }
            public int size { get; set; }
            public int totalSize { get; set; }
            public string url { get; set; }
        }

        public class Downloads
        {
            public Client client { get; set; }
            public Client_Mappings client_mappings { get; set; }
            public Server server { get; set; }
            public Server_Mappings server_mappings { get; set; }
        }

        public class Client
        {
            public string sha1 { get; set; }
            public int size { get; set; }
            public string url { get; set; }
        }

        public class Client_Mappings
        {
            public string sha1 { get; set; }
            public int size { get; set; }
            public string url { get; set; }
        }

        public class Server
        {
            public string sha1 { get; set; }
            public int size { get; set; }
            public string url { get; set; }
        }

        public class Server_Mappings
        {
            public string sha1 { get; set; }
            public int size { get; set; }
            public string url { get; set; }
        }

        public class Javaversion
        {
            public string component { get; set; }
            public int majorVersion { get; set; }
        }

        public class Logging
        {
            public Client1 client { get; set; }
        }

        public class Client1
        {
            public string argument { get; set; }
            public File file { get; set; }
            public string type { get; set; }
        }

        public class File
        {
            public string id { get; set; }
            public string sha1 { get; set; }
            public int size { get; set; }
            public string url { get; set; }
        }

        public class Library
        {
            public Downloads1 downloads { get; set; }
            public string name { get; set; }
            public Rule[] rules { get; set; }
        }

        public class Downloads1
        {
            public Artifact artifact { get; set; }
        }

        public class Artifact
        {
            public string path { get; set; }
            public string sha1 { get; set; }
            public int size { get; set; }
            public string url { get; set; }
        }

        public class Rule
        {
            public string action { get; set; }
            public Os os { get; set; }
        }

        public class Os
        {
            public string name { get; set; }
        }

    }
}

