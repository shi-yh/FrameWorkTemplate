
using System;
using GameFramework.Localization;
using LitJson;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class JsonLocalizationHelper: DefaultLocalizationHelper
    {
        public override bool ParseData(ILocalizationManager localizationManager, string dictionaryString, object userData)
        {
            try
            {
                string currentLanguage = GameEntry.Localization.Language.ToString();

                JsonData jd = JsonMapper.ToObject(dictionaryString);

                for (int i = 0; i < jd.Count; i++)
                {
                    JsonData cur = jd[i];
                    if (!localizationManager.AddRawString(int.Parse(cur["Id"].ToString()),cur[currentLanguage].ToString()))
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