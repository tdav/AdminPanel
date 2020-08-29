using AdminPanel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel.Models
{

    public class MetaData : IDisposable
    {
        public readonly MyDbContext db;
        private readonly IServiceScope scope;

        public MetaData(IServiceScopeFactory ser)
        {
            scope = ser.CreateScope();
            db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        }

        public void Dispose()
        {
            db?.Dispose();
            scope?.Dispose();
        }

        private EntityList list;

        public EntityList GetList()
        {
            if (list == null)
            {
                var currentAssembly = typeof(GenericTypeControllerFeatureProvider).Assembly;
                var candidates = currentAssembly.GetExportedTypes().Where(x => x.GetCustomAttributes<GeneratedControllerAttribute>().Any());


                list = new EntityList();
                list.Entity = new List<MetaEntity>();

                foreach (var candidate in candidates)
                {
                    var atr = candidate.CustomAttributes.FirstOrDefault();

                    var entity = new MetaEntity();
                    entity.Name = candidate.Name;
                    entity.Route = atr.ConstructorArguments[0].Value.ToString();
                    entity.FealdsList = new List<MetaFeald>();

                    var entityType = db.Model.FindEntityType(candidate);
                    var dl = entityType.GetProperties().ToDictionary(x => x.Name);

                    MemberInfo[] memberInfos = candidate.GetMembers();
                    foreach (MemberInfo memberInfo in memberInfos)
                    {

                        Type itemType = null;
                        String memberName = memberInfo.Name;

                        var mf = new MetaFeald();
                        switch (memberInfo.MemberType)
                        {
                            case MemberTypes.Property:
                                itemType = candidate.GetProperty(memberName).PropertyType;
                                mf.Name = memberName;
                                mf.Type = TypeInfoToStr(itemType);


                                if (dl.TryGetValue(memberName, out var it))
                                {
                                    mf.Required = !it.IsNullable;
                                    if (it.IsForeignKey())
                                    {
                                        foreach (var fi in it.GetContainingForeignKeys())
                                        {
                                            mf.ForeignKey = GetForeignKeyRoute(fi.ToString());

                                            var sl = mf.ForeignKey.Split('.');
                                            mf.TypeEx = new MetaTypeEx() { EntityName = sl[0], IsArray = false };
                                        }
                                    }
                                }


                                if (mf.Type == "")
                                {
                                    mf.TypeEx = GetMetaTypeExInfo(itemType);

                                    if (!mf.TypeEx.IsArray)
                                        continue;
                                }

                                entity.FealdsList.Add(mf);
                                break;
                        }
                    }

                    list.Entity.Add(entity);
                }

            }

            return list;
        }

        private string GetForeignKeyRoute(string s)
        {
            var p = s.IndexOf("->");
            var sr = s.Substring(p + 3, s.Length - p - 3);

            p = sr.IndexOf("'}");
            var rr = sr.Substring(0, p);

            return rr.Replace(" {'", ".");
        }

        private string TypeInfoToStr(Type type)
        {
            switch (type.ToString())
            {
                case "System.DateTime": return "Date";
                case "System.Boolean": return "Boolean";

                case "System.Byte":
                case "System.SByte":
                case "System.Decimal":
                case "System.Double":
                case "System.Single":
                case "System.Int32":
                case "System.UInt32":
                case "System.Int64":
                case "System.UInt64":
                case "System.Int16":
                case "System.UInt16": return "Number";

                case "System.Guid":
                case "System.Char":
                case "System.String": return "String";
                case "System.Object": return "object";
                default:
                    return "";
            }
        }

        private MetaTypeEx GetMetaTypeExInfo(Type type)
        {
            var r = new MetaTypeEx();

            r.IsArray = type.IsArray || IsGenericArray(type);


            if (type.IsGenericType)
            {
                Type arguments = type.GetGenericArguments().FirstOrDefault();
                r.EntityName = arguments.Name;


            }
            else
            {
                r.EntityName = type.Name;
            }


            return r;
        }

        private bool IsGenericArray(Type type)
        {
            if (!type.IsGenericType) return false;

            string[] parentType = type.FullName.Split('`');

            switch (parentType[0])
            {
                case "System.Collections.Generic.List":
                case "System.Collections.Generic.Collection":
                    return true;

                default:
                    return false;
            }
        }

    }



    public class EntityList
    {
        public List<MetaEntity> Entity { get; set; }
    }

    public class MetaEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Route { get; set; }
        public string IconUrl { get; set; }
        public List<MetaFeald> FealdsList { get; set; }
    }


    public class MetaFeald
    {
        public bool Required { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string ForeignKey { get; set; }
        public MetaTypeEx TypeEx { get; set; }
    }

    public class MetaTypeEx
    {
        public bool IsArray { get; set; }
        public string EntityName { get; set; }
    }
}
