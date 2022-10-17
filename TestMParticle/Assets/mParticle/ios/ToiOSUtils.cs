using System;
using mParticle;
using System.Collections.Generic;
using System.Collections;

namespace mParticle.ios
{
	internal class ToiOSUtils
	{
		public ToiOSUtils()
		{
		}

		internal string SerializeDictionary(IDictionary dictionary)
		{
			if (dictionary == null)
			{
				return null;
			}

			string serializedString = "{";

			foreach (DictionaryEntry entry in dictionary)
			{
				if (entry.Value is IDictionary)
				{
					serializedString += "\"" + entry.Key.ToString() + "\":" + SerializeDictionary(entry.Value as IDictionary) + ",";
				}
				else
				{
					serializedString += "\"" + entry.Key.ToString() + "\":\"" + entry.Value.ToString() + "\",";
				}
			}

			if (serializedString.Length > 1)
			{
				serializedString = serializedString.Remove(serializedString.Length - 1);
			}

			serializedString += "}";

			return serializedString;
		}

		internal long ToLong(string value, long fallback)
		{
			long val;
			if (Int64.TryParse(value, out val))
			{
				return val;
			}
			else
			{
				return fallback;
			}
		}

		internal bool ParseBoolean(string result, bool fallback)
		{
			bool val;
			if (Boolean.TryParse(result, out val))
			{
				return val;
			}
			else
			{
				return fallback;
			}
		}
	}
}

