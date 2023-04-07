//
//  AppDelegate.swift
//  WarThunderProfileSavior
//
//  Created by Frank on 2023/3/16.
//

import Cocoa

class AppDelegate: NSObject, NSApplicationDelegate {
    static var profileController: ProfileController {
        (NSApp.delegate as! AppDelegate).profileController
    }
    let profileController = ProfileController()
    
    func applicationDidFinishLaunching(_ aNotification: Notification) {
        NotificationController.requestNotificationAuthorization()
        profileController.start()
        
        let _ = AppActivityMonitor.shared
    }

    func applicationWillTerminate(_ aNotification: Notification) {
        // Insert code here to tear down your application
    }

    func applicationSupportsSecureRestorableState(_ app: NSApplication) -> Bool {
        return true
    }


}

