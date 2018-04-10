using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mParticle.ios
{
    public class MPEventDto : Dictionary<string, object>
    {
        private const string EventType = "EventType";
        private const string EventName = "EventName";
        private const string Category = "Category";
        private const string Info = "Info";
        private const string Duration = "Duration";
        private const string StartTime = "StartTime";
        private const string EndTime = "EndTime";
        private const string CustomFlags = "CustomFlags";

        public MPEventDto(MPEvent mpEvent)
        {
            Add(EventType, ((int)mpEvent.EventType).ToString());
            Add(EventName, mpEvent.EventName);
            if (!String.IsNullOrEmpty(mpEvent.Category))
            {
                Add(Category, mpEvent.Category);
            }
            if (mpEvent.Info != null)
            {
                Add(Info, mpEvent.Info);
            }
            if (mpEvent.Duration.HasValue)
            {
                Add(Duration, mpEvent.Duration.Value);
            }
            if (mpEvent.StartTime.HasValue)
            {
                Add(StartTime, mpEvent.StartTime.Value);
            }
            if (mpEvent.EndTime.HasValue)
            {
                Add(EndTime, mpEvent.EndTime.Value);
            }
            if (mpEvent.CustomFlags != null && mpEvent.CustomFlags.Count > 0)
            {
                Dictionary<string, string> customFlagMap = new Dictionary<string, string>();
                mpEvent.CustomFlags.ToList().ForEach(entry =>
                    {
                        string serializedList;
                        if (mpEvent.CustomFlags[entry.Key].Count > 0)
                        {
                            serializedList = "[";
                            mpEvent.CustomFlags[entry.Key].ForEach(s => serializedList = serializedList + s + ",");
                            serializedList = serializedList.Remove(serializedList.Count() - 1) + "]";
                        }
                        else
                        {
                            serializedList = "[]";
                        }
                        //remove surrounding parenthesis
                        customFlagMap.Add(entry.Key, serializedList);
                    });
                Add(CustomFlags, customFlagMap);
            }
        }
    }
}

