﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Context
{
    public static class DbContextExtensions
    {
        public static void Visit(this DbContext context, object entity, Action<object> action)
        {
            Action<object, DbContext, HashSet<object>, Action<object>> visitFunction = null; // Initialize first to enable recursive call.
            visitFunction = (ent, contxt, hashset, act) =>
            {
                if (ent != null && !hashset.Contains(ent))
                {
                    hashset.Add(ent);
                    act(ent);
                    var entry = contxt.Entry(ent);
                    if (entry != null)
                    {
                        foreach (var np in contxt.GetNavigationProperies(ent.GetType()))
                        {
                            if (np.ToEndMember.RelationshipMultiplicity < RelationshipMultiplicity.Many)
                            {
                                var reference = entry.Reference(np.Name);
                                if (reference.IsLoaded)
                                {
                                    visitFunction(reference.CurrentValue, contxt, hashset, action);
                                }
                            }
                            else
                            {
                                var collection = entry.Collection(np.Name);
                                if (collection.IsLoaded)
                                {
                                    var sequence = collection.CurrentValue as IEnumerable;
                                    if (sequence != null)
                                    {
                                        foreach (var child in sequence)
                                        {
                                            visitFunction(child, contxt, hashset, action);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            visitFunction(entity, context, new HashSet<object>(), action);
        }

        // Get navigation properties of an entity type.
        public static IEnumerable<NavigationProperty> GetNavigationProperies(this DbContext context, Type type)
        {
            var oc = ((IObjectContextAdapter)context).ObjectContext;
            var objectType = ObjectContext.GetObjectType(type); // Works with proxies and original types.

            var entityType = oc.MetadataWorkspace.GetItems(DataSpace.OSpace).OfType<EntityType>()
                               .FirstOrDefault(et => et.Name == objectType.Name);
            return entityType != null
                ? entityType.NavigationProperties
                : Enumerable.Empty<NavigationProperty>();
        }
    }
}
