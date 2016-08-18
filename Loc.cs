using System;
using System.Collections;
using System.Globalization;
using Resources;

namespace NewIspNL
{
    public class Loc{
        public string IterateResource(string requiredResource){
            var resourceSet = Tokens.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach(DictionaryEntry entry in resourceSet){
                var key = entry.Key as string;
                if(key == null || !String.Equals(requiredResource.ToLower(), key.ToLower(), StringComparison.CurrentCultureIgnoreCase)) continue;
                var resource = entry.Value as string;
                if(resource == null) continue;
                return resource;
            }
            return requiredResource;
        }
    }
}
