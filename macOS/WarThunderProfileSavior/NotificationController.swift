//
//  NotificationController.swift
//  WarThunderProfileSavior
//
//  Created by Frank on 2023/3/21.
//

import Foundation
import UserNotifications
import AppKit

class NotificationController {
    static func requestNotificationAuthorization() {
        UNUserNotificationCenter.current().requestAuthorization(options: [.alert, .sound]) { isFinished, error in
            UNUserNotificationCenter.current().getNotificationSettings { settings in
                if settings.authorizationStatus != .authorized  {
                    NSWorkspace.shared.open(NSWorkspace.shared.urlForApplication(withBundleIdentifier: "com.apple.systempreferences")!)
                }
            }
        }
    }
    
    static func sendOneTimeNotification(title: String = "🎉👻🫥🎉",
                                        body: String, identifier: String) {
        let content = UNMutableNotificationContent()
        content.title = title
        content.body = body
        
        let trigger = UNTimeIntervalNotificationTrigger(timeInterval: 0.5, repeats: false)
        let notification = UNNotificationRequest(identifier: identifier, content: content, trigger: trigger)
        UNUserNotificationCenter.current().add(notification) { error in
            DispatchQueue.main.asyncAfter(deadline: .now() + 3) {
                UNUserNotificationCenter.current().removeAllDeliveredNotifications()
            }
        }
    }
}
