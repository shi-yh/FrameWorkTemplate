using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    public sealed class ChangeLanguageEventArgs:GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeLanguageEventArgs).GetHashCode();

        
        public override void Clear()
        {
            
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ChangeLanguageEventArgs Create()
        {
            ChangeLanguageEventArgs loadDictionaryFailureEventArgs = ReferencePool.Acquire<ChangeLanguageEventArgs>();

            return loadDictionaryFailureEventArgs;
        }
        
    }
}