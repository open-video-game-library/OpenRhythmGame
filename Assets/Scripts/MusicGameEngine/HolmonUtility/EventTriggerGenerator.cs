namespace HolmonUtility
{
    using System;
    using UnityEngine.EventSystems;

    public static class EventTriggerGenerator
    {
        /// <summary>
        /// EventTrigger��Event��o�^����ۂɕK�v��EventTrigger.Entry�^��Ԃ��܂�
        /// </summary>
        /// <param name="eventType">�o�^�������C�x���g�̃^�C�v</param>
        /// <returns></returns>
        public static EventTrigger.Entry Generate(EventTriggerType type, Action callback)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((data) => { callback(); });
            return entry;
        }

    }
}

