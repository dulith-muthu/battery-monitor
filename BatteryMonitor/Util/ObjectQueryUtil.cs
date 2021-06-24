using System;
using System.Collections.Generic;
using System.Management;

namespace BatteryMonitor.Util
{
    class ObjectQueryUtil
    {
        public IDictionary<string,string> ExecuteObjectQuerry(List<string> selections, string targetClass)
        {
			IDictionary<string, string> result = new Dictionary<string, string>();
			var selectionString = String.Join(",", selections);
			var queryString = String.Format("Select {0} FROM {1}", selectionString, targetClass);

			System.Management.ObjectQuery query = new ObjectQuery(queryString);
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

			ManagementObjectCollection collection = searcher.Get();

			foreach (ManagementObject mo in collection)
			{
				foreach (PropertyData property in mo.Properties)
				{
					if (!result.ContainsKey(property.Value.ToString()))
					{
						result.Add(property.Name, property.Value.ToString());
					}
				}
			}

			return result;
		}
    }
}
