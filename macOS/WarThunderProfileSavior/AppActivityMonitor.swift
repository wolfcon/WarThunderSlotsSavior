//
//  AppActivityMonitor.swift
//  WarThunderProfileSavior
//
//  Created by Frank on 2023/4/5.
//

import Foundation
import AppKit

class AppActivityMonitor: NSObject {
    static let shared = AppActivityMonitor()
    
    override init() {
        super.init()
        startObserving()
    }
    
    deinit {
        stopObserving()
    }
    
    struct KeyPath {
        static let runningApplications = "runningApplications"
    }
    struct Identifier {
        static let launcher = "com.gaijinent.WarThunderDL"
        static let game = "com.gaijinent.WarThunder"
    }
}


extension AppActivityMonitor {
    // MARK: Observing
    func startObserving() {
        NSWorkspace.shared.addObserver(self, forKeyPath: KeyPath.runningApplications, options: [.old, .new], context: nil)
    }
    func stopObserving() {
        NSWorkspace.shared.removeObserver(self, forKeyPath: KeyPath.runningApplications)
    }
    
    override func observeValue(forKeyPath keyPath: String?, of object: Any?, change: [NSKeyValueChangeKey : Any]?, context: UnsafeMutableRawPointer?) {
        guard keyPath == KeyPath.runningApplications else { return }
        
        if let newApps = change?[NSKeyValueChangeKey.newKey] as? [NSRunningApplication],
           newApps.contains(where: { $0.bundleIdentifier == Identifier.game }) {
            print("open warthunder")
        }
        if let oldApps = change?[NSKeyValueChangeKey.oldKey] as? [NSRunningApplication],
           oldApps.contains(where: { $0.bundleIdentifier == Identifier.game }) {
            print("close warthunder")
        }
    }
}
