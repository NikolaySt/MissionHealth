using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization;
using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.Catalogs
{
    public static class DatabaseInitializer
    {
        private static bool _initialized;
        private static readonly object Lock = new object();

        public static void Init()
        {
            lock (Lock)
            {
                if (_initialized) return;

                SuperCatalog.Init();

                InitSharedMaps();

                InitMaps();

                InitMapCaches();

                _initialized = true;
            }
        }

        static void InitSharedMaps()
        {
            BsonClassMap.RegisterClassMap<Content>(initializer =>
            {
                initializer.AutoMap();
                initializer.SetIgnoreExtraElements(true);
            });
        }

        static void InitMaps()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(SuperCatalog))))
            {
                if (type.IsAbstract) continue;

                var method = type.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);

                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }

        static void InitMapCaches()
        {
            //PopulateMapCache(typeof(BlogPost), BlogPostCatalog.MapperProperties);
        }

        static void PopulateMapCache(Type type, Dictionary<string, string> propMapper, string prefixGeneral = "", string prefixBson = "")
        {
            if (type.Assembly.GetName().Name != (typeof(DataObject).Assembly).GetName().Name) return;

            var _prefixGeneral = "";
            var _prefixBson = "";

            foreach (var prop in type.GetProperties())
            {
                if (prop.Name == "Id") continue;

                var memberName = GetBsonMemberNameWithReflection(type, prop.Name);

                if (!string.IsNullOrWhiteSpace(prefixGeneral))
                {
                    _prefixGeneral = prefixGeneral + "." + prop.Name;
                    _prefixBson = prefixBson + "." + memberName;
                }
                else
                {
                    _prefixGeneral = prop.Name;
                    _prefixBson = memberName;
                }

                if (_prefixGeneral != _prefixBson) propMapper.Add(_prefixGeneral, _prefixBson);

                PopulateMapCache(prop.PropertyType, propMapper, _prefixGeneral, _prefixBson);
            }
        }

        static string GetBsonMemberNameWithReflection(Type type, string propName)
        {
            do
            {
                var bsonClassMap = BsonClassMap.LookupClassMap(type);
                var bsonMemberMap = bsonClassMap.GetMemberMap(propName);
                if (bsonMemberMap != null)
                {
                    return bsonMemberMap.ElementName;
                }
                type = type.BaseType;
            } while (type != null);

            return "";
        }
    }
}
