//
//  SettingViewController.swift
//  WarThunderProfileSavior
//
//  Created by Frank on 2023/3/16.
//

import Cocoa

class SettingViewController: NSViewController {

    @IBOutlet weak var gitPathTextField: NSTextField!
    @IBOutlet weak var gpgPathTextField: NSTextField!
    @IBOutlet weak var syncCheckBox: NSButton!
    @IBOutlet weak var remoteURLTextField: NSTextField!
    override func viewDidLoad() {
        super.viewDidLoad()
    }
    
    override func viewDidAppear() {
        super.viewDidAppear()
        
        
    }

    override var representedObject: Any? {
        didSet {
        // Update the view, if already loaded.
        }
    }

    func show() {
        view.window?.makeKeyAndOrderFront(self)
        view.window?.level = .floating
        NSApp.activate(ignoringOtherApps: true)
    }
    @IBAction func syncCheckBoxAction(_ sender: NSButton) {
        remoteURLTextField.isEnabled = sender.state == .on
    }
    @IBAction func applyButtonAction(_ sender: NSButton) {
        
    }
    
    @IBAction func debugButtonAction(_ sender: NSButton) {
        let git = Git(NSHomeDirectory().appending("/My Games/WarThunder/Saves\\ Copy/"))
        let result = git.commit()
        if result.code == 0 {
            
        }
    }
}

