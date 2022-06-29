using System;
using System.Collections.Generic;
using mParticle;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace mParticle.ios
{
	public class ToCSUtils
	{
		public ToCSUtils()
		{
		}

		public static Dictionary<UserIdentity, string> toUserIdentityDictionary(string json)
		{
			Dictionary<string, object> intDictionary = toDictionary(json);
			Dictionary<UserIdentity, string> userIdentityDictionary = new Dictionary<UserIdentity, string>();
			intDictionary.ToList().ForEach(pair =>
				{
					userIdentityDictionary.Add((UserIdentity)Convert.ToInt32(pair.Key), pair.Value.ToString());
				});
			return userIdentityDictionary;
		}

		public static Dictionary<string, string> toStringDictionary(string json)
		{
			try
			{
				json = json.Substring(json.IndexOf('{') + 1, json.LastIndexOf('}'));
				string[] split;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				while ((split = json.Split(new char[]{ ':' }, 2)).Count() > 1)
				{
					var key = split[0].Trim();
					var valueRaw = split[1].TrimStart();
					if (valueRaw.IndexOf('{') == 0)
					{
						string[] values = popDictionaryStringFromString(valueRaw);
						dictionary.Add(key, values[0]);
						json = values[1];
					}
					else
					{
						var valueSplit = valueRaw.Split(new char[]{ ',' }, 2);
						string value = valueSplit[0].Trim();
						dictionary.Add(key, value);
						if (valueSplit.Count() > 1)
						{
							json = valueSplit[1];
						}
						else
						{
							json = "";
						}
					}
				}
				;
				return dictionary;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Dictionary<string, string>();
			}
		}

		public static Dictionary<string, object> toDictionary(string json)
		{
			try
			{
				json = json.Substring(json.IndexOf('{') + 1, json.LastIndexOf('}'));
				string[] split;
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				while ((split = json.Split(new char[]{ ':' }, 2)).Count() > 1)
				{
					var key = split[0].Trim().Substring(split[0].IndexOf('\"') + 1, split[0].LastIndexOf('\"') - 1);
					var valueRaw = split[1].TrimStart();
					if (valueRaw.IndexOf('{') == 0)
					{
						Dictionary<string, object> value = new Dictionary<string, object>();
						json = popDictionaryFromString(valueRaw, out value);
						dictionary.Add(key, value);
					}
					else
					{
						var valueSplit = valueRaw.Split(new char[]{ ',' }, 2);
						string value = valueSplit[0].Trim();
						dictionary.Add(key, value);
						if (valueSplit.Count() > 1)
						{
							json = valueSplit[1];
						}
						else
						{
							json = "";
						}
					}
				}
				;
				return dictionary;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new Dictionary<string, object>();
			}
		}

		private static string popDictionaryFromString(string valueRaw, out Dictionary<string, object> dictionary)
		{
			valueRaw = valueRaw.TrimStart();
			char[] charArray = valueRaw.ToCharArray();
			if (charArray[0].Equals('{'))
			{
				var count = 1;
				var index = 1;
				while (count > 0 && index <= valueRaw.Length)
				{
					var c = charArray[index++];
					if (c.Equals('{'))
					{
						count++;
					}
					if (c.Equals('}'))
					{
						count--;
					}
				}
				if (count == 0)
				{
					dictionary = toDictionary(valueRaw.Substring(0, index));
					return valueRaw.Substring(index, valueRaw.Count() - index);
				}
				else
				{
					dictionary = new Dictionary<string, object>();
					return "";
				}
			}
			dictionary = new Dictionary<string, object>();
			return valueRaw;

		}

		private static string[] popDictionaryStringFromString(string valueRaw)
		{
			valueRaw = valueRaw.TrimStart();
			char[] charArray = valueRaw.ToCharArray();
			if (charArray[0].Equals('{'))
			{
				var count = 1;
				var index = 1;
				while (count > 0 && index <= valueRaw.Length)
				{
					var c = charArray[index++];
					if (c.Equals('{'))
					{
						count++;
					}
					if (c.Equals('}'))
					{
						count--;
					}
				}
				if (count == 0)
				{
					return new string[]{ valueRaw.Substring(index, valueRaw.Count() - index), valueRaw.Substring(valueRaw.Count() - index, valueRaw.Count()) };
				} 
			}
			return new string[]{ valueRaw, "" };
		}
	}
}

//   Sample = "AssertAttribute1":"value101","AssertAttribute3":"value303","AssertAttribute2":"value202"
