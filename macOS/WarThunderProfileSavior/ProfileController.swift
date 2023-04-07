//
//  ProfileController.swift
//  WarThunderProfileSavior
//
//  Created by Frank on 2023/3/22.
//

import Foundation
import AppKit

class ProfileController: NSObject, NSMenuDelegate {
    lazy var settingWindowController = NSStoryboard(name: "Main", bundle: nil).instantiateInitialController() as! NSWindowController
    var settingViewController: SettingViewController {
        settingWindowController.contentViewController as! SettingViewController
    }
    
    lazy var statusItem: NSStatusItem = {
        let item = NSStatusBar.system.statusItem(withLength: 18)
        item.button?.image = NSImage(named: "warThunderIcon")
        item.button?.action = #selector(statusItemAction(_:))
        item.button?.sendAction(on: [.leftMouseUp, .rightMouseUp])
        item.button?.target = self
        item.button?.toolTip = "Left click to sync immediately. \nRight click to open advanced menu."
        
        return item
    }()
    
    lazy var menu: NSMenu = {
        let menu = NSMenu(title: "")
        menu.delegate = self
        menu.title = "Profile Savior"
        menu.addItem(
            withTitle: "Sync now",
            action: #selector(syncNow),
            keyEquivalent: "")
        menu.addItem(
            withTitle: "Settings",
            action: #selector(openSettings),
            keyEquivalent: "")
        menu.addItem(
            withTitle: "Quit",
            action: #selector(quit),
            keyEquivalent: "")
        menu.items.forEach{ $0.target = self }
        return menu
    }()
    
    static let savingPath = NSHomeDirectory().appending("/My Games/WarThunder/Saves/")
    var savingPath: String {
        ProfileController.savingPath
    }
    
    /// nil when profile does not exist.
    static var git: Git? {
        let profileIsExist = FileManager.default.fileExists(atPath: savingPath)
        return profileIsExist ? Git(savingPath) : nil
    }
    var git: Git? {
        ProfileController.git
    }
    
    var isUnderControl: Bool {
        guard let _ = git else { return false }
        
        return FileManager.default.fileExists(
            atPath: savingPath.appending(Git.extensionName))
    }
    
    func start() {
        statusItem.isVisible = true
    }
    
    @objc func statusItemAction(_ sender: NSStatusBarButton) {
        guard let event = NSApp.currentEvent else { return }
        if event.type == .rightMouseUp {
            statusItem.menu = menu
            statusItem.button?.performClick(nil)
        } else {
            syncNow()
        }
    }
    
    private var retryCount = 2
    private func resetRetryCount() {
        retryCount = 2
    }
    @objc func syncNow() {
        guard let git = git else { return }
        if !isUnderControl { git.`init`() }
        DispatchQueue.global(qos: .background).async {
            let result = git.pull()
            
            if result.isFailed {
                DispatchQueue.main.async {
                    guard self.retryCount > 0 else {
                        self.resetRetryCount()
                        return
                    }
                    self.syncNow()
                    self.retryCount -= 1
                }
                return
            } else {
                if !result.upToDate {
                    NotificationController.sendOneTimeNotification(
                        body: result.message, identifier: "pull")
                }
            }
            
            let commitResult = git.autoCommit()
            if commitResult.nothingToCommit { return }
            
            git.defaultRemoteNames?.forEach {
                let pushResult = git.push($0)
                NotificationController.sendOneTimeNotification(
                    body: pushResult.message, identifier: "push")
            }
        }
    }
    
    @objc func openSettings() {
        settingViewController.show()
    }
    
    @objc func quit() {
        NSApp.terminate(nil)
    }
    
    func menuDidClose(_ menu: NSMenu) {
        statusItem.menu = nil
    }
}

extension Git {
    @discardableResult
    func resetUnnecessarySetting() -> Output {
        return execute("reset", "HEAD", "--", "**/machine.blk", "common.blk")
    }
    
    @discardableResult
    func autoCommit() -> Output {
        stageAll()
        resetUnnecessarySetting()
        return commit()
    }
}
