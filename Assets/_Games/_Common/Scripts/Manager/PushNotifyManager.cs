using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

#if FIRESTORE
using DataConfig;
#endif

#if PUSHNOTIFY_ANDROID
using Unity.Notifications.Android;
#endif

#if PUSHNOTIFY_IOS
using Unity.Notifications.iOS;
#endif

public class PushNotifyManager : MonoBehaviourPersistence<PushNotifyManager>
{
#if FIRESTORE
    public DataSave dataSave => DataSave.Instance;
    public DataConfigModel dataConfig => DataManager.Instance.config;

    private string largeIcon = "icon_0";
    private string smallIcon = "icon_1";
    private string channelID = "channel_id";

    public void Start()
    {
#if PUSHNOTIFY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
#endif
    }

    internal void SendNotification(string title, string body, TimeSpan timeDelayFromNow, string smallIcon, string largeIcon, string customData)
    {
#if PUSHNOTIFY_ANDROID
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = body;

        if (smallIcon != null) notification.SmallIcon = smallIcon;
        if (smallIcon != null) notification.LargeIcon = largeIcon;
        if (customData != null) notification.IntentData = customData;

        notification.FireTime = DateTime.Now.Add(timeDelayFromNow);
        AndroidNotificationCenter.SendNotification(notification, channelID);
#endif

#if PUSHNOTIFY_IOS
            iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = timeDelayFromNow,
                Repeats = false,
            };

            iOSNotification notification = new iOSNotification()
            {
                Title = title,
                Subtitle = "",
                Body = body,
                Data = customData,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }

    public void SendNotification(string id)
    {
#if PUSHNOTIFY_ANDROID
        if (DataManager.Instance.isReady)
        {
            var pn = dataConfig.push_notification[id];

            if (pn == null)
            {
                Debug.LogError(string.Format("push_notify id {0} is not exist", id));
                return;
            }

            SendNotification(pn.title, pn.body, new System.TimeSpan(0, pn.minute, 0), smallIcon, largeIcon, null);
        }
#endif
    }

    void SendCombackNotification(string id, int minuteFirstOpenGame)
    {
#if PUSHNOTIFY_ANDROID
        if (DataManager.Instance.isReady)
        {
            var pn = dataConfig.push_notification[id];

            if (pn == null)
            {
                Debug.LogError(string.Format("push_notify id {0} is not exist", id));
                return;
            }

            var diffMinute = DateTime.UtcNow.Minute - minuteFirstOpenGame;
            if (diffMinute < pn.minute)
            {
                SendNotification(pn.title, pn.body, new System.TimeSpan(0, diffMinute, 0), smallIcon, largeIcon, null);
            }
        }
#endif
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        CancelAllNotifications();

#if PUSHNOTIFY_ANDROID
        if (DataSave.Instance.pushNotification)
        {
            if (!focusStatus)
            {
                var minuteFirstOpenGame = DateTime.Parse(DataSave.Instance.dateFirstOpenGame).Minute;

                SendCombackNotification("comeBack1", minuteFirstOpenGame);
                SendCombackNotification("comeBack2", minuteFirstOpenGame);
                SendCombackNotification("comeBack3", minuteFirstOpenGame);
                SendCombackNotification("comeBack4", minuteFirstOpenGame);
            }
        }
#endif
    }

    public void CancelAllNotifications()
    {
#if PUSHNOTIFY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
#endif

#if PUSHNOTIFY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
    }
#endif
}
