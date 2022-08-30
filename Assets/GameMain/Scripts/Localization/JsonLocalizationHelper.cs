
using System;
using System.Collections.Generic;
using GameFramework.Localization;
using LitJson;
using GameMain.LocalizationGenerator;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class JsonLocalizationHelper: DefaultLocalizationHelper
    {

        
        public override bool ParseData(ILocalizationManager localizationManager, string dictionaryString, object userData)
        {
            try
            {
                JsonData jd = JsonMapper.ToObject(dictionaryString);
                
                for (int i = 0; i < jd.Count; i++)
                {
                    JsonData cur = jd[i];
                    
                    Dictionary<Language, string> tempData = new Dictionary<Language, string>();
                    
                    foreach (var VARIABLE in cur.Keys)
                    {
                        Language result;
                        
                        if (Enum.TryParse(VARIABLE, out result))
                        {
                            tempData.Add(result,cur[VARIABLE].ToString());
                        }
                    }
                    
                    if (!localizationManager.AddRawString(int.Parse(cur["Id"].ToString()),tempData))
                    {
                        Log.Warning("Can not add raw string with key '{0}' which may be invalid or duplicate.", cur["Id"]);
                        return false;
                    }
                }

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}